using System.Collections;
using UnityEngine;
using NaughtyAttributes;

namespace Reless.MR
{
    /// <summary>
    /// 방의 크기를 키웁니다.
    /// </summary>
    public class RoomEnlarger : MonoBehaviour
    {
        /// <summary>
        /// RoomManager 레퍼런스
        /// </summary>
        private RoomManager RoomManager { get; set; }
        
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
        
        /// <summary>
        /// 방이 커지는 애니메이션의 커브입니다.
        /// </summary>
        [SerializeField]
        private AnimationCurve animationCurve;
        
        private void Start()
        {
            RoomManager = RoomManager.Instance;
            if (RoomManager is null)
            {
                RoomManager.OnMRUKSceneLoaded += () => RoomManager = RoomManager.Instance;
            }
        }

        /// <summary>
        /// 방의 크기를 키웁니다.
        /// </summary>
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public IEnumerator EnlargingRoom()
        {
            // 방의 크기를 키우는 애니메이션
            for (float elapsedTime = 0f; elapsedTime < enlargingDuration; elapsedTime += Time.deltaTime)
            {
                float scale = Mathf.Lerp(1f, enlargedScale,
                    Mathf.Lerp(0f, 1f, animationCurve.Evaluate(elapsedTime / enlargingDuration)));
                //RoomManager.Room.transform.localScale = Vector3.one * scale;
                
                Vector3 roomPos = RoomManager.Room.transform.localPosition;
                Vector3 pivot = new Vector3(GameManager.EyeAnchor.position.x, 0, GameManager.EyeAnchor.position.z); ;
                
                Vector3 relativePosition = roomPos - pivot; 

                var relativeScale = scale / RoomManager.Room.transform.localScale.x; 

                // Calculate final position post-scale
                Vector3 newPosition = pivot + relativePosition * relativeScale;

                // Finally, actually perform the scale/translation
                RoomManager.Room.transform.localScale = Vector3.one * scale;
                RoomManager.Room.transform.localPosition = newPosition;

                yield return null;
            }

            // 최종 값으로 방의 크기 변경
            RoomManager.Room.transform.localScale = Vector3.one * enlargedScale;
            
            yield return new WaitForSeconds(2f);
        }
        
        public void RestoreRoomScale()
        {
            RoomManager.Room.transform.position = Vector3.zero;
            RoomManager.Room.transform.localScale = Vector3.one;
        }
    }
}