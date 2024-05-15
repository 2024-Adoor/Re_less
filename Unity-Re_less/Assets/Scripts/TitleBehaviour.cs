using System;
using System.Collections;
using NaughtyAttributes;
using Reless.MR;
using Reless.UI;
using UnityEngine;
using Logger = Reless.Debug.Logger;

namespace Reless
{
    /// <summary>
    /// 게임 시작 시의 동작을 정의합니다.
    /// </summary>
    public class TitleBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 방 안에서 시작했는지 여부
        /// </summary>
        [ShowNonSerializedField]
        private bool _startedInRoom;
        
        /// <summary>
        /// RoomManager 레퍼런스
        /// </summary>
        private RoomManager _roomManager;
        
        private void Awake()
        {
            if (RoomManager.Instance is RoomManager roomManager)
            {
                _roomManager = roomManager;
            }
            else
            {
                // RoomManager가 없다면 비활성화하고 방이 로드될 때 켜지도록 이벤트에 등록합니다.
                this.enabled = false;
                RoomManager.OnMRUKSceneLoaded += () =>
                {
                    _roomManager = RoomManager.Instance;
                    this.enabled = true;
                };
            }
        }

        private void OnEnable()
        {
            GameManager.PhaseChanged += OnTitle;
            
            // 이미 Title 단계라면 바로 실행합니다.
            if (GameManager.CurrentPhase is GamePhase.Title) StartCheck();
        }
        
        private void OnDisable()
        {
            GameManager.PhaseChanged -= OnTitle;
        }
        
        private void OnTitle(GamePhase phase)
        {
            if (phase is not GamePhase.Title) return;
            
            StartCheck();
        }

        /// <summary>
        /// 게임 시작 시 해야 할 확인을 시작합니다.
        /// </summary>
        private void StartCheck()
        {
            Logger.Log($"{nameof(TitleBehaviour)}: StartCheck");
            
            // 방 안에서 시작했는지 확인합니다.
            _startedInRoom = _roomManager.Room.IsPositionInRoom(GameManager.EyeAnchor.position);
            
            if (_startedInRoom)
            {
                Logger.Log($"{nameof(TitleBehaviour)}: Player is started in the room, checking approach door...");
                
                GuideText.SetText("문으로 다가가 게임 시작...");
                
                // 방 안에서 시작했다면 문에 다가갔을 때 오프닝을 시작합니다.
                StartCoroutine(CheckingApproachDoor(
                    onApproach: () =>
                    {
                        GuideText.StartFadeOutText(after: 0.5f);
                        GameManager.CurrentPhase = GamePhase.Opening;
                    },
                    until: () => GameManager.CurrentPhase is not GamePhase.Title));
            }
            else
            {
                Logger.Log($"{nameof(TitleBehaviour)}: Player is not started in the room, checking enter room...");
                
                GuideText.SetText("방 안으로 들어가 게임 시작...");
                
                // 방 안에서 시작하지 않았다면 방을 들어갈 때 오프닝을 시작합니다.
                StartCoroutine(CheckingEnterRoom(
                    onEnter: () =>
                    {
                        GuideText.StartFadeOutText(after: 0.5f);
                        GameManager.CurrentPhase = GamePhase.Opening;
                    },
                    until: () => GameManager.CurrentPhase is not GamePhase.Title));
            }
        }
        
        /// <summary>
        /// 방에 들어갔는지 확인합니다.
        /// </summary>
        /// <param name="onEnter">방에 들어갔을 때 실행할 동작</param>
        /// <param name="until">확인을 중단할 조건</param>
        /// <returns>이 함수는 유니티 코루틴 루틴입니다.</returns>
        private IEnumerator CheckingEnterRoom(Action onEnter, Func<bool> until)
        {
            while (!until())
            {
                if (_roomManager.Room.IsPositionInRoom(GameManager.EyeAnchor.position))
                {
                    Logger.Log($"{nameof(TitleBehaviour)}: Player entered the room.");
                    onEnter?.Invoke();
                    yield break;
                }
                
                yield return null;
            }
        }
        
        /// <summary>
        /// 문에 접근했는지 확인합니다.
        /// </summary>
        /// <param name="onApproach">문에 접근했을 때 실행할 동작</param>
        /// <param name="until">확인을 중단할 조건</param>
        /// <returns>이 함수는 유니티 코루틴 루틴입니다.</returns>
        private IEnumerator CheckingApproachDoor(Action onApproach, Func<bool> until)
        {
            while (!until())
            {
                var distance = _roomManager.ClosestDoorDistance(GameManager.EyeAnchor.position, out _);
                if (distance == 0f)
                {
                    Logger.LogWarning($"{nameof(TitleBehaviour)}: Door not found.");
                    
                    // 문이 없으면 바로 실행합니다.
                    onApproach?.Invoke();
                    yield break;
                }
                
                if (distance < 0.5f)
                {
                    Logger.Log($"{nameof(TitleBehaviour)}: Player approached the door.");
                    onApproach?.Invoke();
                    yield break;
                }
                
                yield return null;
            }
        }
    }
}


