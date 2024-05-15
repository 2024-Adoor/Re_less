using System.Collections;
using Reless.MR;
using UnityEngine;

namespace Reless.Debug
{
    /// <summary>
    /// 에디터에서 엔딩 씬에서 플레이할 경우 적절히 플레이가 시뮬레이션 되도록 도와주는 클래스입니다.
    /// </summary>
    public class EndingInspector : MonoBehaviour
    {
        private void Start()
        {
            // 게임 상태를 엔딩으로 변경합니다.
            GameManager.CurrentPhase = GamePhase.Ending;

            RoomManager.TryInvokeAction(_ => FixCameraPosition());
        }

        /// <summary>
        /// 엔딩 씬의 디버그용 카메라는 변환과 관계 없이 항상 문에서 바깥쪽을 바라봐야 합니다.
        /// </summary>
        private void FixCameraPosition()
        {
            StartCoroutine(WaitForOneFrame());
            
            IEnumerator WaitForOneFrame()
            {
                // EndingRoomTransform가 트래킹 스페이스에 변환을 수행하는 동안 대기합니다.
                yield return new WaitForEndOfFrame();
                
                Logger.Log($"{nameof(EndingInspector)}: Fixing camera position for testing.");
                
                // 트래킹 스페이스를 다시 초기화합니다.
                GameManager.CameraRig.trackingSpace.localPosition = Vector3.zero;
                GameManager.CameraRig.trackingSpace.localRotation = Quaternion.identity;
            }
        }
    }
}