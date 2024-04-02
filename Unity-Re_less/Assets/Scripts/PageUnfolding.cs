using Oculus.Interaction;
using UnityEngine;

/// <summary>
/// 페이지가 공중에서 놓아졌을 때 펼쳐지게 하는 클래스입니다.
/// </summary>
public class PageUnfolding : MonoBehaviour
{
    [SerializeField]
    private InteractableGroupView interactableGroupView;
    
    [SerializeField]
    private float speed;

    [SerializeField] 
    private bool left;
    
    [SerializeField]
    private float acceleration = 0.2f;
    
    private float _accelerationTime;
    
    private float _yAngle;

    private void LateUpdate()
    {
        // 플레이어가 페이지를 잡고 있나요?
        if (interactableGroupView.State == InteractableState.Select)
        {
            // 페이지의 회전각을 책 위로 제한합니다.
            if (left ? transform.up.x < 0 : transform.up.x > 0)
            {
                if (left ? transform.forward.x < 0 : transform.forward.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0f, left ? -90f: 90f, 0f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(-180f, left ? -90f: 90f, 0f);
                }
            }
            _accelerationTime = 0;
            return;
        }

        _accelerationTime += acceleration;
            
        // 페이지가 놓아졌을 때 펼쳐지게 합니다.
        if (left ? transform.forward.x < 0 : transform.forward.x > 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, left ? -90f: 90f, 0f), Time.deltaTime * speed * _accelerationTime);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(-180f, left ? -90f: 90f, 0f), Time.deltaTime * speed * _accelerationTime);
        }
    }
}
