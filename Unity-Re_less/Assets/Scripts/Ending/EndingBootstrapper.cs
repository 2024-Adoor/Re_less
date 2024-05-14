using System;
using NaughtyAttributes;
using Reless.MR;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using SceneManager = Reless.Util.SceneManager;

namespace Reless.Ending
{
    /// <summary>
    /// 엔딩을 준비하고 불러옵니다.
    /// </summary>
    public class EndingBootstrapper : MonoBehaviour
    {
        private bool _isInEnding;
        
        private void Awake()
        {
            OnEnding(GameManager.CurrentPhase);
            GameManager.PhaseChanged += OnEnding;
        }
        
        private void OnDestroy()
        {
            GameManager.PhaseChanged -= OnEnding;
        }

        private void OnEnding(GamePhase phase)
        {
            if (phase is not GamePhase.Ending)
            {
                if (_isInEnding) ExitEnding();
                return;
            }
            
            BootstrapEnding();
        }
        
        private void ExitEnding()
        {
            if (!_isInEnding) return;
            
            RoomManager.Instance?.RevertRoomTransform();
            
            _isInEnding = false;
        }
        
        /// <summary>
        /// 엔딩을 시작합니다.
        /// </summary>
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void BootstrapEnding()
        {
            if (_isInEnding) return;
            _isInEnding = true;
            
            var roomManager = RoomManager.Instance;
            Assert.IsNotNull(roomManager);

            if (roomManager.UniqueDoor is not null)
            {
                var door = roomManager.UniqueDoor.transform;
                roomManager.SetRoomTransform(
                    newOrigin: new Vector3(door.position.x, 0, door.position.z), 
                    newForward: door.transform.forward);
            }
            else
            {
                throw new NotImplementedException($"{nameof(EndingBootstrapper)} only supports a room with a single door");
            }

            SceneManager.LoadAsync(BuildScene.Ending, LoadSceneMode.Additive);
        }
        
    }
}