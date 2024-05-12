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

        private InputAction _moveAction;
        private InputAction _jumpAction;

        /// <summary>
        /// 이동 속도
        /// </summary>
        public float speed = 4.0f;
        
        /// <summary>
        /// 회전량
        /// </summary>
        public float snapTurnAngle = 45.0f;
        
        private bool _readyToSnapTurn;
        
        private void Awake()
        {
            _cameraRig = GameManager.CameraRig; //GetComponentInChildren<OVRCameraRig>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _moveAction = GameManager.InputActions.VR.Move;
            _jumpAction = GameManager.InputActions.VR.Jump;
            _jumpAction.performed += _ =>
            {
                if (!hasJumped) _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                hasJumped = true;
            };
            
            Recenter();
        }
        
        private void FixedUpdate()
        {
            RotatePlayerToHMD();
            if (_moveAction.inProgress) StickMovement();
            //SnapTurn();
            //Jump();
            X_Friend();
            Y_Fruit();
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Recenter()
        {
            var eye = GameManager.EyeAnchor;
            var trackingSpace = GameManager.CameraRig.trackingSpace;
            
            trackingSpace.position -= new Vector3(eye.position.x - transform.position.x, 0, eye.position.z - transform.position.z);
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
        
        /// <summary>
        /// 스틱으로 바로 회전하기
        /// </summary>
        private void SnapTurn()
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
            {
                if (!_readyToSnapTurn) return;
                
                _readyToSnapTurn = false;
                
                // rotationAngle만큼 왼쪽으로 회전합니다.
                transform.RotateAround(transform.position, Vector3.up, -snapTurnAngle);
            }
            else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
            {
                if (!_readyToSnapTurn) return;
                
                _readyToSnapTurn = false;
                
                // rotationAngle만큼 오른쪽으로 회전합니다.
                transform.RotateAround(transform.position, Vector3.up, snapTurnAngle);
            }
            else
            {
                _readyToSnapTurn = true;
            }
        }

        private void Jump()
        {
            if (OVRInput.GetDown(OVRInput.Button.One) && !hasJumped)
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                hasJumped = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Untagged"))
            {
                hasJumped = false;
            }
        }

        // X 클릭
        private void X_Friend()
        {
            if(OVRInput.GetDown(OVRInput.RawButton.X))
            {
                isXdown = true;
            }
            else
            {
                isXdown = false;
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