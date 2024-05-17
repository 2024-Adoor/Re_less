using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reless
{
    /// <summary>
    /// VR에서 플레이어의 움직임을 조작하는 클래스입니다.
    /// Meta StarterSamples의 SimpleCapsuleWithStickMovement.cs를 바탕으로 작성되었습니다.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerControl : MonoBehaviour
    {
        private OVRCameraRig _cameraRig;
        private Rigidbody _rigidbody;

        public float jumpForce = 5.0f; // 점프에 가해질 힘
        private bool hasJumped = false;

        public bool isXdown;
        public bool isYdown;
        public bool isBdown;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _exitAction;

        /// <summary>
        /// 이동 속도
        /// </summary>
        public float speed = 4.0f;
        
        /// <summary>
        /// 회전량
        /// </summary>
        public float snapTurnAngle = 45.0f;
        
        private bool _readyToSnapTurn;
        
        private bool _exitReady;
        
        /// <summary>
        /// 꿈에서 나가기 액션을 활성화합니다.
        /// </summary>
        public void EnableExitAction() 
        {
            _exitReady = true;
            _exitAction.Enable();
        }
        
        private void Awake()
        {
            _cameraRig = GameManager.CameraRig; //GetComponentInChildren<OVRCameraRig>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        private IEnumerator Start()
        {
            _moveAction = GameManager.InputActions.VR.Move;
            _jumpAction = GameManager.InputActions.VR.Jump;
            _exitAction = GameManager.InputActions.VR.Exit;
            
            _jumpAction.performed += Jump;
            _exitAction.performed += ExitDream;
            
            // 꿈에서 나가는 액션은 추후 조건 달성 시 활성화될 것입니다.
            _exitAction.Disable();

            // 트래킹되도록 한 프레임을 기다리고 중심을 재조정합니다.
            yield return null;
            Recenter();
        }

        private void OnDestroy()
        {
            _jumpAction.performed -= Jump;
            _exitAction.performed -= ExitDream;
        }

        private void FixedUpdate()
        {
            RotatePlayerToHMD();
            if (_moveAction.inProgress) StickMovement();
            //SnapTurn();
            //Jump();
            X_Down();
            B_Down();
            Y_Fruit();
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Recenter()
        {
            var eye = GameManager.EyeAnchor;
            var trackingSpace = GameManager.CameraRig.trackingSpace;
            
            trackingSpace.position -= new Vector3(eye.position.x - transform.position.x, 0, eye.position.z - transform.position.z);
        }
        
                
        private void Jump(InputAction.CallbackContext context)
        {
            if (!hasJumped) _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasJumped = true;
        }
        
        private void ExitDream(InputAction.CallbackContext context)
        {
            if (_exitReady)
            {
                FindAnyObjectByType<ChapterControl>().ExitDream();
            }
        }

        // 정확히 뭐 하는거고 왜 필요한지 아직 모르겠음..
        private void RotatePlayerToHMD()
        {
            Transform root = _cameraRig.trackingSpace;
            Transform centerEye = _cameraRig.centerEyeAnchor;

            Vector3 prevPos = root.position;
            Quaternion prevRot = root.rotation;

            transform.rotation = Quaternion.Euler(0.0f, centerEye.rotation.eulerAngles.y, 0.0f);

            root.position = prevPos;
            root.rotation = prevRot;
        }

        
        /// <summary>
        /// 스틱 입력을 통해 플레이어를 움직입니다.
        /// </summary>
        private void StickMovement()
        {
            // 카메라의 현재 방향.
            Vector3 rotation = _cameraRig.centerEyeAnchor.rotation.eulerAngles;

            // y축 (수평 방향)만 필요하므로 x, z축을 0으로 합니다.
            rotation.z = 0f;
            rotation.x = 0f;
            Quaternion direction = Quaternion.Euler(rotation);

            // 스틱을 입력받습니다.
            Vector2 stickVector = _moveAction.ReadValue<Vector2>(); //OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            
            // 스틱의 입력을 움직일 방향으로 변환합니다.
            var moveDirection = Vector3.zero;
            moveDirection += direction * (stickVector.x * Vector3.right);
            moveDirection += direction * (stickVector.y * Vector3.forward);
            
            // 플레이어를 움직입니다.
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * (speed * Time.fixedDeltaTime));
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Untagged"))
            {
                hasJumped = false;
            }
        }

        // X 누르고 있을때 true / 뗄 때 false
        private void X_Down()
        {
            if(OVRInput.Get(OVRInput.RawButton.X))
            {
                isXdown = true;
            }
            else
            {
                isXdown = false;
            }
        }

        // B 누르고 있을때 true / 뗄 때 false
        private void B_Down()
        {
            if(OVRInput.GetDown(OVRInput.Button.Two))
            {
                isBdown = true;
            }
            else
            {
                isBdown = false;
            }
        }

        // Y 클릭
        private void Y_Fruit()
        {
            if(OVRInput.GetDown(OVRInput.RawButton.Y))
            {
                isYdown = true;
            }
            else
            {
                isYdown = false;
            }
        }
    }
}