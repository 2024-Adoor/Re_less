using System;
using System.Collections;
using NaughtyAttributes;
using Reless.MR;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Logger = Reless.Debug.Logger;

namespace Reless.Game
{
    /// <summary>
    /// 그려서 얻은 오브젝트
    /// </summary>
    public class ObtainingObject : MonoBehaviour
    {
        /// <summary>
        /// 오브젝트가 속하는 챕터
        /// </summary>
        [SerializeField]
        public Chapter chapter;

        private readonly WaitForSeconds _respawnCheckInterval = new(2f);
        
        private Coroutine _respawnCheckCoroutine;
        
        private MainBehaviour _mainBehaviour;
        
        private Rigidbody _rigidbody;
        
        private InputAction _respawnAction;
        
        public bool IsSnapped { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            transform.localScale = Vector3.one * 5;
        }

        private void Start()
        {
            _mainBehaviour = FindAnyObjectByType<MainBehaviour>();
            Assert.IsNotNull(_mainBehaviour, $"{nameof(ObtainingObject)}: There is no MainBehaviour in the scene.");
            
            // 리스폰 액션을 등록합니다.
            _respawnAction = GameManager.InputActions.MR.Respawn;
            _respawnAction.performed += Respawn;
            
            // 리스폰 체크 루틴을 시작합니다.
            /*_respawnCheckCoroutine = StartCoroutine(CheckRespawnNeeded(
                respawnCondition: () => this.transform.position.y < -10, 
                onNeeded: () => Respawn(GameManager.EyeAnchor.position)));*/
        }

        private void OnDestroy()
        {
            _respawnAction.performed -= Respawn;
            StopCoroutine(_respawnCheckCoroutine);
        }

        /// <summary>
        /// 리스폰이 필요한지 체크합니다.
        /// </summary>
        /// <param name="respawnCondition">리스폰 필요 조건을 정의하는 함수</param>
        /// <param name="onNeeded">리스폰 필요 시 실행될 액션</param>
        /// <returns>이 메서드는 유니티 코루틴입니다.</returns>
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
        /// 리스폰 액션
        /// </summary>
        /// <param name="context"></param>
        private void Respawn(InputAction.CallbackContext context) => Respawn();
        
        /// <summary>
        /// 리스폰합니다.
        /// </summary>
        [Button(enabledMode: EButtonEnableMode.Editor)]
        public void Respawn()
        {
            // 오른쪽 컨트롤러 위치에 리스폰.
            Respawn(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
        }
        
        /// <summary>
        /// 대상 위치에 리스폰합니다.
        /// </summary>
        /// <param name="position">리스폰할 위치</param>
        private void Respawn(Vector3 position)
        {
            // 위치를 옮깁니다.
            transform.position = position;
            
            // 리스폰 시 속도를 초기화합니다. (방 밖으로 떨어진 경우 중력가속도를 없애는 등)
            _rigidbody.velocity = Vector3.zero; 
            _rigidbody.angularVelocity = Vector3.zero;
        }
        
        public void OnSnapped()
        {
            Logger.Log($"{this.gameObject.name}: Snapped");
            IsSnapped = true;

            transform.localScale = Vector3.one;

            _mainBehaviour.AchieveEnterCondition();
            
            // 스냅되면 중력을 꺼야 합니다.
            GetComponent<Rigidbody>().useGravity = false;
        }
        
        // 가구현, 프리팹 이벤트에 등록 필요
        public void OnUnsnapped()
        {
            Logger.Log($"{this.gameObject.name}: Unsnapped");

            transform.localScale = Vector3.one * 5;
            
            // 스냅이 풀리면 중력을 다시 켜야 합니다.
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}


