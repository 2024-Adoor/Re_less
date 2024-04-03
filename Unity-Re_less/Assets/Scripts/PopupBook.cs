using System.Collections.Generic;
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
        
        [SerializeField]
        private List<GameObject> leftPopups = new List<GameObject>();
        
        [SerializeField]
        private List<GameObject> rightPopups = new List<GameObject>();
        
        [SerializeField]
        private GameObject leftPage;
        
        [SerializeField]
        private GameObject rigthPage;

        // TODO:
        
        public void TurnNextPage()
        {
            _currentPageIndex++;
            
            var leftPagePopup = Instantiate(leftPopups[_currentPageIndex], leftPage.transform);
            var leftPageBackPopup = Instantiate(rightPopups[_currentPageIndex - 1], rigthPage.transform);
            leftPageBackPopup.transform.Rotate(0, 180, 0);
            var rightPagePopup = Instantiate(rightPopups[_currentPageIndex], rigthPage.transform);
            var rightPageBackPopup = Instantiate(leftPopups[_currentPageIndex - 1], leftPage.transform);
            rightPageBackPopup.transform.Rotate(0, 180, 0);
        }
        
        public void TurnPreviousPage()
        {
            _currentPageIndex--;
            
            var leftPagePopup = Instantiate(leftPopups[_currentPageIndex], leftPage.transform);
            var leftPageBackPopup = Instantiate(rightPopups[_currentPageIndex + 1], rigthPage.transform);
            leftPageBackPopup.transform.Rotate(0, 180, 0);
            var rightPagePopup = Instantiate(rightPopups[_currentPageIndex], rigthPage.transform);
            var rightPageBackPopup = Instantiate(leftPopups[_currentPageIndex + 1], leftPage.transform);
            rightPageBackPopup.transform.Rotate(0, 180, 0);
        }
    }
}
