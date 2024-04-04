using System;
using NaughtyAttributes;
using Oculus.Interaction;
using Reless;
using UnityEngine;

/// <summary>
/// 페이지의 넘김과 펼쳐짐을 담당하는 클래스입니다.
/// </summary>
public class PageUnfolding : MonoBehaviour
{
    /// <summary>
    /// 팝업북의 레퍼런스
    /// </summary>
    [SerializeField, ReadOnly]
    private PopupBook popupBook;
    
    /// <summary>
    /// 플레이어가 페이지를 잡고 있는 지 확인하기 위한 IInteractableView의 레퍼런스
    /// </summary>
    [SerializeField, Interface(typeof(IInteractableView)), Required]
    private UnityEngine.Object interactableView;
    
    /// <summary>
    /// 페이지가 펼쳐지는 속도
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// 페이지가 펼쳐지는 가속도
    /// </summary>
    [SerializeField] 
    private float acceleration;
    
    /// <summary>
    /// 시간에 따라 가속도를 누적하기 위한 변수
    /// </summary>
    private float _timedAcceleration;
    
    /// <summary>
    /// 왼쪽 페이지인인지 오른쪽 페이지인지 확인합니다.
    /// </summary>
    private bool _isLeft;
    
    /// <summary>
    /// 플레이어가 페이지를 잡고 있습니다.
    /// </summary>
    [ShowNativeProperty]
    private bool IsGrabbed => (interactableView as IInteractableView)?.State == InteractableState.Select;
    
    /// <summary>
    /// 페이지가 완전히 수직으로 펼쳐져 있습니다.
    /// </summary>
    [ShowNativeProperty]
    private bool IsUnfolded => Mathf.Approximately((transform.localRotation * Vector3.up).y, 1f);
    
    /// <summary>
    /// 페이지를 반 이상 넘기고 있습니다.
    /// </summary>
    [ShowNativeProperty]
    private bool IsTurnedMoreThanHalf => (transform.localRotation * Vector3.up).y < 0;

    /// <summary>
    /// 페이지를 완전히 넘겼습니다.
    /// </summary>
    [ShowNativeProperty]
    private bool IsTurnedCompletely => Mathf.Approximately((transform.localRotation * Vector3.up).y, -1f);
    
    /// <summary>
    /// 페이지를 책의 평면 이상으로 넘겼습니다.
    /// </summary>
    [ShowNativeProperty]
    private bool IsTurnedMoreThanSurface => (transform.localRotation * Vector3.forward).y < 0;
    
    /// <summary>
    /// 펼쳐진 페이지의 회전각
    /// </summary>
    private Quaternion UnfoldedRotation => Quaternion.LookRotation(_isLeft ? Vector3.left : Vector3.right, Vector3.up);
    
    /// <summary>
    /// 넘겨진 페이지의 회전각
    /// </summary>
    private Quaternion TurnedRotation => Quaternion.LookRotation(_isLeft ? Vector3.right : Vector3.left, Vector3.down);

    private void Start()
    {
        // 시작 시 페이지가 왼쪽인지 오른쪽인지 확인하고 저장합니다.
        _isLeft = (transform.localRotation * Vector3.forward).x < 0;
    }

    private void LateUpdate()
    {
        // 플레이어가 페이지를 잡고 있나요?
        if (IsGrabbed)
        {
            // 플레이어는 페이지를 잡아 자유롭게 회전할 수 있는 상태입니다. 페이지의 회전이 책을 넘어가서는 안 되므로 제한합니다.
            ConstrainRotationToWithinBook();
            
            // 가속도 초기화
            _timedAcceleration = 0;
        }
        else
        {
            // 페이지가 펼쳐져 있나요?
            if (IsUnfolded)
            {
                // 가속도 초기화
                _timedAcceleration = 0;
                
                // 이미 펼쳐져 있다면 아무것도 할 필요가 없습니다.
            }
            else
            {
                // 가속도를 누적합니다.
                _timedAcceleration += acceleration * Time.deltaTime;
                
                // 페이지를 펼칩니다.
                Unfolding();
            }
        }
    }

    /// <summary>
    /// 현재 페이지 각도를 기반으로 페이지를 펼칩니다.
    /// </summary>
    private void Unfolding()
    {
        // 페이지를 반 이상 넘겼나요?
        if (IsTurnedMoreThanHalf)
        {
            // 넘기는 방향으로 페이지를 펼칩니다.
            transform.localRotation = Quaternion.RotateTowards(to: TurnedRotation, 
                from: transform.localRotation, maxDegreesDelta: Time.deltaTime * speed * _timedAcceleration);

            // 페이지를 완전히 넘겼나요?
            if (IsTurnedCompletely)
            {
                // 페이지 넘김을 시뮬레이션합니다 - 페이지의 회전을 초기 상태로 돌립니다.
                transform.localRotation = UnfoldedRotation;
                
                if (_isLeft)
                {
                    // 왼쪽 페이지를 넘겼습니다 - 팝업북을 이전 페이지로 넘깁니다.
                    popupBook.PageIndex--;
                }
                else
                {
                    // 오른쪽 페이지를 넘겼습니다 - 팝업북을 다음 페이지로 넘깁니다.
                    popupBook.PageIndex++;
                }
            }
        }
        else
        {
            // 원래 방향으로 페이지를 펼칩니다.
            transform.localRotation = Quaternion.RotateTowards(to: UnfoldedRotation, 
                from: transform.localRotation, maxDegreesDelta: Time.deltaTime * speed * _timedAcceleration);
        }
    }
    
    /// <summary>
    /// 페이지의 회전을 책의 평면 위로 제한합니다.
    /// </summary>
    private void ConstrainRotationToWithinBook()
    {
        if (IsTurnedMoreThanSurface)
        {
            transform.localRotation = IsTurnedMoreThanHalf ? TurnedRotation : UnfoldedRotation;
        }
    }

    private void OnValidate()
    {
        popupBook = GetComponentInParent<PopupBook>();
    }
}
