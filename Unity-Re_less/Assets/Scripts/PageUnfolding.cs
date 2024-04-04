using NaughtyAttributes;
using Oculus.Interaction;
using Reless;
using UnityEngine;

/// <summary>
/// 페이지가 공중에서 놓아졌을 때 펼쳐지게 하는 클래스입니다.
/// </summary>
public class PageUnfolding : MonoBehaviour
{
    [SerializeField]
    private PopupBook popupBook;
    
    [SerializeField]
    private InteractableGroupView interactableGroupView;
    
    [SerializeField]
    private float speed;
    
    [SerializeField]
    private float acceleration = 0.2f;
    
    private float _accelerationTime;
    
    private float _yAngle;
    
    /*[ShowNativeProperty]
    private float upX => transform.up.x;
    [ShowNativeProperty]
    private float upY => transform.up.y;
    [ShowNativeProperty]
    private float upZ => transform.up.z;
    [ShowNativeProperty]
    private float rightX => transform.right.x;
    [ShowNativeProperty]
    private float rightY => transform.right.y;
    [ShowNativeProperty]
    private float rightZ => transform.right.z;
    [ShowNativeProperty]
    private float forwardX => transform.forward.x;
    [ShowNativeProperty]
    private float forwardY => transform.forward.y;
    [ShowNativeProperty]
    private float forwardZ => transform.forward.z;*/

    private void LateUpdate()
    {
        var rotation = transform.rotation;
        bool turnMoreThanHalf = transform.up.y < 0;
        bool isLeft = transform.forward.x < 0;
        
        // 플레이어가 페이지를 잡고 있나요?
        if (interactableGroupView.State == InteractableState.Select)
        {
            // 페이지의 회전각을 책 위로 제한합니다.
            if (transform.forward.y < 0)
            {
                if (transform.up.y > 0)
                {
                    transform.rotation = Quaternion.Euler(0f, isLeft ? -90f : 90f, 0f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(-180f, isLeft ? -90f : 90f, 0f);
                }
            }
            _accelerationTime = 0;
            return;
        }
        
        _accelerationTime += acceleration;

        
        
        // 반 이상 넘겼나요?
        if (turnMoreThanHalf)
        {
            Debug.LogError("Turned More Than Half");
            // 넘긴 방향으로 페이지가 펼쳐지도록 회전합니다.
            transform.rotation = Quaternion.RotateTowards(
                from: transform.rotation, 
                to: Quaternion.Euler(-180f, rotation.eulerAngles.y, 0f), 
                maxDegreesDelta: Time.deltaTime * speed * _accelerationTime);
            
            // 페이지를 완전히 넘겼나요?
            if (transform.forward.y <= 0)
            {
                Debug.LogError("Turned");
                // 넘기지 않은 상태로 돌아갑니다.
                transform.rotation = Quaternion.Euler(0f, isLeft ? -90f : 90f, 0f);
                
                if (isLeft)
                {
                    popupBook.TurnNextPage();
                }
                else
                {
                    popupBook.TurnPreviousPage();
                }
            }
        }
        else
        {
            // 원래 방향으로 페이지가 펼쳐지도록 회전합니다.
            transform.rotation = Quaternion.RotateTowards(
                from: transform.rotation, 
                to: Quaternion.Euler(0f, isLeft ? -90f : 90f, 0f), 
                maxDegreesDelta: Time.deltaTime * speed * _accelerationTime);
        }
    }
}
