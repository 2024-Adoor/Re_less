using System;
using System.Collections;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reless
{
    /// <summary>
    /// 게임의 전반을 관리하는 클래스입니다.
    /// NOTE: 향후 분리될 수 있음
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public enum Phase
        {
            Title,
            Opening,
            Tutorial,
            Chapter1,
            Chapter2,
            Chapter3,
            Ending,
        }
        
        private Phase _currentPhase;

        public Phase CurrentPhase
        {
            get => _currentPhase;
            set
            {
                _currentPhase = value;

                switch (_currentPhase)
                {
                    case Phase.Title: StartTitle(); break;
                    case Phase.Opening: StartOpening(); break;
                    case Phase.Tutorial: StartTutorial(); break;
                    case Phase.Chapter1: StartChapter1(); break;
                    case Phase.Chapter2: StartChapter2(); break;
                    case Phase.Chapter3: StartChapter3(); break;
                    case Phase.Ending: OnEnding(); break;
                }
            }
        }

        private void OnEnding()
        {
            throw new NotImplementedException();
        }

        private void StartChapter3()
        {
            throw new NotImplementedException();
        }

        private void StartChapter2()
        {
            throw new NotImplementedException();
        }

        private void StartChapter1()
        {
            throw new NotImplementedException();
        }

        private void StartTutorial()
        {
            throw new NotImplementedException();
        }

        private void StartTitle()
        {
            SceneManager.LoadSceneAsync("MainScene");
        }
        
        private void StartOpening()
        {
            StartCoroutine(LoadingOpeningScene());
            
            // 벽 생성과 어두워지는 동안 로딩하고 다 어두워진 다음에 실제로 로드.
            
            
            // 패스스루로 된 벽 생성, 어두워지기 시작, 벽 중 하나를 골라서 지우기.
            
            IEnumerator LoadingOpeningScene()
            {
                var asyncLoad = SceneManager.LoadSceneAsync("Opening", LoadSceneMode.Additive);
                
                // 씬 활성화는 어두워지고 나서.
                asyncLoad.allowSceneActivation = false;
                
                float timer = 0f;
                float duration = 3f;
                float darkedBrightness = -0.5f;
                
                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    
                    passthroughLayer.SetBrightnessContrastSaturation(brightness: Mathf.Lerp(0, darkedBrightness, timer / duration));
                    yield return null;
                }
                
                asyncLoad.allowSceneActivation = true;
            }
        }

        [SerializeField, HideInInspector]
        private OVRCameraRig cameraRig;
        
        [SerializeField]
        private OVRPassthroughLayer passthroughLayer;

        [ShowNonSerializedField]
        private bool _startedInRoom;
        
        private MRUKRoom _currentRoom;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        /// <summary>
        /// MRUK의 OnSceneLoaded 이벤트에 연결되어 호출됩니다.
        /// </summary>
        public void OnSceneLoaded()
        {
            _currentRoom = MRUK.Instance.GetCurrentRoom();
            _startedInRoom = _currentRoom.IsPositionInRoom(cameraRig.centerEyeAnchor.position);
            
            if (_startedInRoom)
            {
                // 방 안에서 시작했다면 문에 다가갔을 때 오프닝을 시작합니다.
                StartCoroutine(CheckApproachDoor(
                    onApproach: () => { CurrentPhase = Phase.Opening; },
                    until: () => _currentPhase is Phase.Opening));
            }
            else
            {
                // 방 안에서 시작하지 않았다면 방을 들어갈 때 오프닝을 시작합니다.
                StartCoroutine(CheckEnterRoom(
                    onEnter: () => { CurrentPhase = Phase.Opening; },
                    until: () => _currentPhase is Phase.Opening));
            }
        }

        private IEnumerator CheckEnterRoom(Action onEnter, Func<bool> until)
        {
            while (!until())
            {
                if (_currentRoom.IsPositionInRoom(cameraRig.centerEyeAnchor.position))
                {
                    Debug.Log("Enter Room");
                    onEnter?.Invoke();
                    yield break;
                }
                
                yield return null;
            }
        }
        
        private IEnumerator CheckApproachDoor(Action onApproach, Func<bool> until)
        {
            while (!until())
            {
                throw new NotImplementedException();
                
                yield return null;
            }
        }
          
        private void RestartCurrentPhase()
        {
            CurrentPhase = _currentPhase;
        }

        private void OnValidate()
        {
            cameraRig ??= FindObjectOfType<OVRCameraRig>();
        }
    }
}

