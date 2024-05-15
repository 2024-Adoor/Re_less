using System.Collections;
using Reless.MR;
using UnityEngine;

namespace Reless.Ending
{
    /// <summary>
    /// 엔딩에서 방을 나가는 동작과 관련된 클래스입니다.
    /// </summary>
    public class ExitRoom : MonoBehaviour
    {
        /// <summary>
        /// 룸 메쉬에 대한 surface-projected 패스스루 레이어
        /// </summary>
        [SerializeField]
        private OVRPassthroughLayer roomPassthrough;

        private float _initialPlayerToDoorDistance;
        
        private bool _isPlayerExited;

        private void Awake()
        {
            RoomManager.TryInvokeAction(Setup);
        }

        private void Start()
        {
            RoomManager.TryInvokeAction(StartUpdatePassthrough);
        }

        private void Setup(RoomManager roomManager)
        {
            // 시작 시점의 플레이어 위치와 가장 가까운 문 위치 사이의 거리를 저장합니다.
            _initialPlayerToDoorDistance = roomManager.ClosestDoorDistance(GameManager.EyeAnchor.localPosition, out _);
            Debug.Logger.Log($"{nameof(ExitRoom)}: Initial player to door distance = <b>{_initialPlayerToDoorDistance}<b/>");

            // 패스스루 이펙트 메쉬를 룸 패스스루 레이어에 추가합니다.
            foreach (var sppPassthroughMesh in roomManager.SppPassThroughMeshes)
            {
                roomPassthrough.AddSurfaceGeometry(sppPassthroughMesh);

                /*Debug.Log($"Add Passthrough Mesh of {sppPassthroughMesh.transform.parent.name}");
                roomPassthrough.AddSurfaceGeometry(sppPassthroughMesh);*/
            }
        }

        private void StartUpdatePassthrough(RoomManager roomManager)
        {
            StartCoroutine(UpdatePassthrough());

            IEnumerator UpdatePassthrough()
            {
                // 플레이어와 가장 가까운 문 사이의 거리를 계산합니다.
                var distance = roomManager.ClosestDoorDistance(GameManager.EyeAnchor.localPosition, out var doorPosition);
            
                if (distance != 0)
                {
                    // 플레이어가 문과 가까운 정도를 계산합니다.
                    float nearness = 1f - Mathf.Clamp01(distance / _initialPlayerToDoorDistance);
                
                    const float initialBrightness = -0.5f;
                    
                    // 플레이어가 문에 가까워질수록 패스스루가 밝아집니다.
                    roomPassthrough.SetBrightnessContrastSaturation(brightness: Mathf.Lerp(initialBrightness, 0, nearness));
                    
                }

                // 플레이어가 방 밖으로 나갔나요?
                if (roomManager.Room.IsPositionInRoom(GameManager.EyeAnchor.position) is false)
                {
                    // 룸 패스스루 밝기는 기본값으로 설정합니다.
                    roomPassthrough.SetBrightnessContrastSaturation(brightness: 0);
                    
                    _isPlayerExited = true;
                    yield break;
                }
                
                
                yield return null;
            }
        }
    }
}