using System.Collections;
using UnityEngine;
using NaughtyAttributes;

namespace Reless.MR
{
    
    /// <summary>
    /// 작성 중
    /// </summary>
    public class RoomEnlarger : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private RoomManager roomManager;
        
        /// <summary>
        /// 방의 크기를 키울 때 사용할 크기 배율입니다.
        /// </summary>
        [SerializeField]
        private float enlargedScale;

        /// <summary>
        /// 방이 커지는 애니메이션이 진행되는 시간(초)입니다.
        /// </summary>
        [SerializeField]
        private float enlargingDuration;
        
        [SerializeField]
        private AnimationCurve animationCurve;
        
        private void OnValidate()
        {
            roomManager = FindObjectOfType<RoomManager>();
        }

        /// <summary>
        /// 방의 크기를 키웁니다.
        /// </summary>
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void EnlargeRoom()
        {
            StartCoroutine(EnlargingRoom());
            
            IEnumerator EnlargingRoom()
            {
                // 방의 크기를 키우는 애니메이션
                for (float elapsedTime = 0f; elapsedTime < enlargingDuration; elapsedTime += Time.deltaTime)
                {
                    float scale = Mathf.Lerp(1f, enlargedScale, Mathf.Lerp(0f, 1f, animationCurve.Evaluate(elapsedTime / enlargingDuration)));
                    roomManager.Room.transform.localScale = Vector3.one * scale;
                    yield return null;
                }
                
                // 최종 값으로 방의 크기 변경
                roomManager.Room.transform.localScale = Vector3.one * enlargedScale;
            }
        }
        
        public void RestoreRoomScale()
        {
            roomManager.Room.transform.localScale = Vector3.one;
        }
    }
}