using System.Collections;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;

namespace Reless
{
    
    /// <summary>
    /// 작성 중
    /// </summary>
    public class RoomEnlarger : MonoBehaviour
    {
        private MRUKRoom _room;
        
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
            if (enlargedScale == default) enlargedScale = 15f;
            if (enlargingDuration == default) enlargingDuration = 2f;
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
                    float scale = Mathf.Lerp(0.1f, 1.0f, animationCurve.Evaluate(elapsedTime / enlargingDuration)) * enlargedScale;
                    _room.transform.localScale = Vector3.one * scale;
                    yield return null;
                }
                
                // 최종 값으로 방의 크기 변경
                _room.transform.localScale = Vector3.one * enlargedScale;
            }
        }
        
        public void Initialize()
        {
            _room = FindObjectOfType<MRUKRoom>();
        }
    }
}