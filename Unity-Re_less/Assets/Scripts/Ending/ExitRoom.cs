using System.Collections;
using System.Linq;
using NaughtyAttributes;
using Reless.MR;
using Reless.UI;
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
        private OVRPassthroughLayer doorSppPassthrough;

        private OVRPassthroughLayer _mainPassthrough;

        private float _initialPlayerToDoorDistance;
        
        public bool IsPlayerExited { get; set; }

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
            _mainPassthrough = FindObjectsByType<OVRPassthroughLayer>(FindObjectsSortMode.None).ToList()
                .Single(layer => layer != doorSppPassthrough);
            
            // 시작 시점의 플레이어 위치와 가장 가까운 문 위치 사이의 거리를 저장합니다.
            _initialPlayerToDoorDistance = roomManager.ClosestDoorDistance(GameManager.EyeAnchor.localPosition, out _);
            Debug.Logger.Log($"{nameof(ExitRoom)}: Initial player to door distance = <b>{_initialPlayerToDoorDistance}<b/>");

            // 패스스루 이펙트 메쉬를 문 패스스루 레이어에 추가합니다.
            foreach (var sppPassthroughMesh in roomManager.DoorSppPassthroughMeshes)
            {
                doorSppPassthrough.AddSurfaceGeometry(sppPassthroughMesh);

                /*Debug.Log($"Add Passthrough Mesh of {sppPassthroughMesh.transform.parent.name}");
                roomPassthrough.AddSurfaceGeometry(sppPassthroughMesh);*/
            }
        }

        private void StartUpdatePassthrough(RoomManager roomManager)
        {
            StartCoroutine(UpdatePassthrough());

            IEnumerator UpdatePassthrough()
            {
                // 플레이어가 방 안에 있는 동안
                while (roomManager.Room.IsPositionInRoom(GameManager.EyeAnchor.position))
                {
                    // 플레이어와 가장 가까운 문 사이의 거리를 계산합니다.
                    var distance = roomManager.ClosestDoorDistance(GameManager.EyeAnchor.localPosition, out var doorPosition);

                    if (distance != 0)
                    {
                        // 플레이어가 문과 가까운 정도를 계산합니다.
                        float nearness = 1f - Mathf.Clamp01(distance / _initialPlayerToDoorDistance);

                        const float initialBrightness = -0.5f;

                        // 플레이어가 문에 가까워질수록 문 패스스루가 하얗게 밝아집니다.
                        doorSppPassthrough.SetBrightnessContrastSaturation(brightness: Mathf.Lerp(0, 0.5f, nearness));
                        
                        // 플레이어가 문에 가까워질수록 패스스루가 어두움에서 밝아집니다.
                        _mainPassthrough.SetBrightnessContrastSaturation(brightness: Mathf.Lerp(initialBrightness, 0, nearness));
                    }

                    yield return null;
                }

                // 패스스루 밝기는 기본값으로 설정합니다.
                _mainPassthrough.SetBrightnessContrastSaturation(brightness: 0);

                IsPlayerExited = true;
                
                GuideText.SetText("THE END");
            }
        }
    }
}