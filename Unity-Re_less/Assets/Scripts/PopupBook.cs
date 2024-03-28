using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

namespace Reless
{
    /// <summary>
    /// 팝업북과 관련된 동작을 담당하는 클래스입니다.
    /// <remarks> 현재 구조화 중, 구현 예정 </remarks> 
    /// </summary>
    public class PopupBook : MonoBehaviour
    {
        /// <summary>
        /// 현재 펼쳐진 페이지의 쪽수를 나타냅니다.
        /// </summary>
        private int _currentPageIndex = 0;
        
        private List<GameObject> _pages = new List<GameObject>();
        
        private InteractableGroupView _leftInteractable;
        private InteractableGroupView _rightInteractable;

        private float _foldingSpeedAcceleration;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void OnEnable()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (_leftInteractable.State != InteractableState.Select)
            {
                if (_leftInteractable.gameObject.transform.rotation.x is > 0 and < 90)
                {
                    StartCoroutine(Folding(_leftInteractable.gameObject.transform, 0));

                }
                
                
            }
        }

        private IEnumerator Folding(Transform transform, float targetAngle)
        {
            // 각도 차이에 따라 회전 방향이 다릅니다.
            float angleDifference = targetAngle - transform.rotation.x;
            bool clockwise = angleDifference < 0;
            float delta = Time.deltaTime;
            
            while (true)
            {
                transform.Rotate(Vector3.right, delta);

                if (clockwise)
                {
                    if (transform.rotation.x > targetAngle) break;

                    delta += _foldingSpeedAcceleration;
                }
                else
                {
                    if (transform.rotation.x < targetAngle) break;
                    
                    delta -= _foldingSpeedAcceleration;
                }
                
                yield return null;
            }
            
            transform.rotation = Quaternion.Euler(targetAngle, 0, 0);
        }
    }
}
