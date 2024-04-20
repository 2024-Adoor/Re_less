using System;
using System.Collections;
using Reless.MR;
using UnityEngine;

namespace Reless
{
    /// <summary>
    /// 그려서 얻은 오브젝트
    /// </summary>
    public class ObtainingObject : MonoBehaviour
    {
        /// <summary>
        /// 오브젝트가 속하는 챕터
        /// </summary>
        [SerializeField] [Range(1, 3)]
        private int chapter;

        private readonly WaitForSeconds _respawnCheckInterval = new(2f);
        
        private Coroutine _respawnCheckCoroutine;
        
        private void Start()
        {
            transform.localScale = Vector3.one * 5;
            
            // 리스폰 체크 루틴을 시작합니다.
            _respawnCheckCoroutine = StartCoroutine(CheckRespawnNeeded(
                respawnCondition: () => RoomManager.Instance.Room.IsPositionInRoom(transform.position) is false, 
                onNeeded: () => Respawn(GameManager.EyeAnchor.position)));
        }

        private void OnDestroy()
        {
            StopCoroutine(_respawnCheckCoroutine);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        /// <summary>
        /// 리스폰이 필요한지 체크합니다.
        /// </summary>
        /// <param name="respawnCondition">리스폰 필요 조건을 정의하는 함수</param>
        /// <param name="onNeeded">리스폰 필요 시 실행될 액션</param>
        /// <returns></returns>
        private IEnumerator CheckRespawnNeeded(Func<bool> respawnCondition, Action onNeeded)
        {
            while (true)
            {
                yield return _respawnCheckInterval;
                
                if (respawnCondition())
                {
                    onNeeded();
                }
            }
        }
        
        /// <summary>
        /// 리스폰합니다.
        /// </summary>
        /// <param name="position">리스폰할 위치</param>
        public void Respawn(Vector3 position)
        {
            // 단순히 위치를 옮깁니다.
            transform.position = position;
        }
        
        public void Snapped()
        {
            Debug.Log($"Snapped: {gameObject.name}");

            transform.localScale = Vector3.one;
            
            MainBehaviour mainBehaviour = FindObjectOfType<MainBehaviour>();
            mainBehaviour.AchieveEnterCondition(chapter);
            
            // 스냅되면 중력을 꺼야 합니다.
            GetComponent<Rigidbody>().useGravity = false;
        }
        
        // 가구현, 프리팹 이벤트에 등록 필요
        public void Unsnapped()
        {
            Debug.Log($"Unsnapped: {gameObject.name}");

            transform.localScale = Vector3.one * 5;
            
            // 스냅이 풀리면 중력을 다시 켜야 합니다.
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}


