using System;
using System.Collections;
using NaughtyAttributes;
using Reless.MR;
using UnityEngine;

namespace Reless
{
    public class TitleBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 방 안에서 시작했는지 여부
        /// </summary>
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
            GameManager.OnTitle += StartCheck;
            
            if (GameManager.Instance.CurrentPhase is GamePhase.Title) StartCheck();
        }
        
        private void OnDisable()
        {
            GameManager.OnTitle -= StartCheck;
        }

        private void StartCheck()
        {
            _startedInRoom = _roomManager.Room.IsPositionInRoom(GameManager.EyeAnchor.position);
            
            if (_startedInRoom)
            {
                // 방 안에서 시작했다면 문에 다가갔을 때 오프닝을 시작합니다.
                StartCoroutine(CheckingApproachDoor(
                    onApproach: () => { GameManager.Instance.CurrentPhase = GamePhase.Opening; },
                    until: () => GameManager.Instance.CurrentPhase is not GamePhase.Title));
            }
            else
            {
                // 방 안에서 시작하지 않았다면 방을 들어갈 때 오프닝을 시작합니다.
                StartCoroutine(CheckingEnterRoom(
                    onEnter: () => { GameManager.Instance.CurrentPhase = GamePhase.Opening; },
                    until: () => GameManager.Instance.CurrentPhase is not GamePhase.Title));
            }
        }
        
        private IEnumerator CheckingEnterRoom(Action onEnter, Func<bool> until)
        {
            while (!until())
            {
                if (_roomManager.Room.IsPositionInRoom(GameManager.EyeAnchor.position))
                {
                    onEnter?.Invoke();
                    yield break;
                }
                
                yield return null;
            }
        }
        
        private IEnumerator CheckingApproachDoor(Action onApproach, Func<bool> until)
        {
            while (!until())
            {
                var distance = _roomManager.ClosestDoorDistance(GameManager.EyeAnchor.position, out _);
                if (distance == 0f)
                {
                    Debug.LogWarning("Door not found.");
                    yield break;
                }
                
                if (distance < 0.5f)
                {
                    onApproach?.Invoke();
                    yield break;
                }
                
                yield return null;
            }
        }
    }
}

