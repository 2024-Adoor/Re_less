using System;
using Reless.MR;
using UnityEngine;
using UnityEngine.Assertions;
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
            if (GameManager.CurrentPhase is GamePhase.Ending) BootstrapEnding();
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
            _isInEnding = true;
            
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
        private void BootstrapEnding()
        {
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

            SceneManager.LoadAsync(BuildScene.Ending);
        }
        
    }
}