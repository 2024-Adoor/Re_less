using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using Oculus.Interaction.Input;
using UnityEngine;

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

        [SerializeField, HideInInspector]
        private OVRCameraRig _cameraRig;

        [ShowNonSerializedField]
        private bool _startedInRoom;
        
        private MRUKRoom _currentRoom;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void OnSceneLoaded()
        {
            _currentRoom = MRUK.Instance.GetCurrentRoom();
            _startedInRoom = _currentRoom.IsPositionInRoom(_cameraRig.centerEyeAnchor.position);
            if (!_startedInRoom) StartCoroutine(CheckEnterRoom());
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        private IEnumerator CheckEnterRoom()
        {
            if (_currentRoom.IsPositionInRoom(_cameraRig.centerEyeAnchor.position))
            {
                _currentPhase = Phase.Opening;
                yield break;
            }
            yield return null;
        }

        private void OnValidate()
        {
            _cameraRig ??= FindObjectOfType<OVRCameraRig>();
        }
    }
}

