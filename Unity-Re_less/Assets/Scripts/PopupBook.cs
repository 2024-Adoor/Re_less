using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
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
        [ShowNonSerializedField]
        private int _currentPageIndex = 0;

        /// <summary>
        /// 페이지 메시의 두께
        /// </summary>
        private const float PageThickness = 0.002f;
        
        public int PageIndex
        {
            get => _currentPageIndex;
            set
            {
                DeactivatePopups(_currentPageIndex);
                _currentPageIndex = value;
                ActivatePopups(_currentPageIndex);
            }
        }
        
        /// <summary>
        /// 팝업북 페이지가 펼쳐지는 정도에 따라 팝업이 눌리는 정도를 조절하는 애니메이션 커브입니다.
        /// </summary>
        [SerializeField]
        private AnimationCurve popupPressureCurve;
        
        [SerializeField]
        private GameObject leftPage;
        
        [SerializeField]
        private GameObject rightPage;

        [SerializeField] 
        private GameObject fixedLeftPage;
        
        [SerializeField]
        private GameObject fixedRightPage;
        
        [SerializeField]
        private List<GameObject> leftPopups = new List<GameObject>();
        
        [SerializeField]
        private List<GameObject> rightPopups = new List<GameObject>();
        
        [SerializeField]
        private GameObject leftPopupsContainer;
        
        [SerializeField]
        private GameObject rightPopupsContainer;

        /// <summary>
        /// 팝업북에 붙는 팝업들의 위치 유형입니다.
        /// </summary>
        private enum PopupPosition
        {
            FixedLeft, // 고정된 왼쪽 페이지에 붙은 팝업
            LeftBack, // 왼쪽 페이지의 뒤쪽에 붙은 팝업
            Left, // 왼쪽 페이지에 붙은 팝업
            Right, // 오른쪽 페이지에 붙은 팝업
            RightBack, // 오른쪽 페이지의 뒤쪽에 붙은 팝업
            FixedRight // 고정된 오른쪽 페이지에 붙은 팝업
        }

        
        /// <summary>
        /// 해당하는 위치의 팝업을 가져옵니다.
        /// </summary>
        /// <param name="pageIndex">기준이 되는 페이지 인덱스</param>
        /// <param name="popupPosition">가져올 팝업의 위치 유형</param>
        /// <returns>팝업 GameObject</returns>
        private GameObject GetPopup(int pageIndex, PopupPosition popupPosition) => popupPosition switch
        {
            PopupPosition.FixedLeft => leftPopups.ElementAtOrDefault(pageIndex - 1),
            PopupPosition.LeftBack => rightPopups.ElementAtOrDefault(pageIndex - 1),
            PopupPosition.Left => leftPopups.ElementAtOrDefault(pageIndex),
            PopupPosition.Right => rightPopups.ElementAtOrDefault(pageIndex),
            PopupPosition.RightBack => leftPopups.ElementAtOrDefault(pageIndex + 1),
            PopupPosition.FixedRight => rightPopups.ElementAtOrDefault(pageIndex + 1),
            _ => null
        };
        
        private GameObject GetCurrentPopup(PopupPosition popupPosition) => GetPopup(_currentPageIndex, popupPosition);
        
        private void Start()
        {
            PageIndex = 0;
        }
        
        private void Update()
        {
            UpdatePopupsPressure();
        }

        /// <summary>
        /// 왼쪽 페이지와 오른쪽 페이지가 펼쳐진 각도에 따라 각 위치에 있는 팝업이 펼쳐지는 정도를 조절합니다.
        /// </summary>
        private void UpdatePopupsPressure()
        {
            var leftUp = leftPage.transform.up;
            var rightUp = rightPage.transform.up;
            
            float leftPageOpenAngle = Mathf.Clamp(leftUp.x, 0f, 1f);
            float rightPageOpenAngle = Mathf.Clamp(-rightUp.x, 0f, 1f);
            
            float leftPressure = popupPressureCurve.Evaluate(leftPageOpenAngle);
            float rightPressure = popupPressureCurve.Evaluate(rightPageOpenAngle);

            // 수직(90도) 보다 더 열리게 펼쳐졌는지 여부. 
            bool leftPageAngleUpperVertical = leftUp.y < 0;
            bool rightPageAngleUpperVertical = rightUp.y < 0;

            var fixedLeftPopup = GetCurrentPopup(PopupPosition.FixedLeft);
            var leftBackPopup = GetCurrentPopup(PopupPosition.LeftBack);
            var leftPopup = GetCurrentPopup(PopupPosition.Left);
            var rightPopup = GetCurrentPopup(PopupPosition.Right);
            var rightBackPopup = GetCurrentPopup(PopupPosition.RightBack);
            var fixedRightPopup = GetCurrentPopup(PopupPosition.FixedRight);
            
            // 페이지를 넘기는 각도가 90도를 넘어가면 (왼쪽)
            if (leftPageAngleUpperVertical)
            {
                // 자신 페이지 아래에 눌려 있던 팝업들은 이제 제약이 필요 없습니다 - 완전히 풀립니다.
                if (fixedLeftPopup is not null) fixedLeftPopup.transform.localScale = Vector3.one;
                if (leftBackPopup is not null) leftBackPopup.transform.localScale = Vector3.one;
                
                // 페이지를 완전히 넘겨가는 경우 자신 페이지와 반대쪽 페이지 위의 팝업은 점점 작아져야 합니다.
                if (leftPopup is not null) leftPopup.transform.localScale = new Vector3(1f, leftPressure, 1f);
                if (rightPopup is not null) rightPopup.transform.localScale = new Vector3(1f, leftPressure, 1f);
            }
            // 페이지를 넘기는 각도가 90도 이하라면
            else
            {
                // 자신 페이지 아래의 팝업들은 눌려 있어야 합니다. 특히 완전히 펼쳐져 있는 경우에 아래에 숨겨져 있을 팝업들의 크기는 0입니다.
                if (fixedLeftPopup is not null) fixedLeftPopup.transform.localScale = new Vector3(1f, leftPressure, 1f);
                if (leftBackPopup is not null) leftBackPopup.transform.localScale = new Vector3(1f, leftPressure, 1f);
                
                // 90도 이하의 펼친 각도에서 자신 페이지의 팝업은 완전히 풀려 있습니다. - 단 상대 페이지 또한 펼쳐져 있어야 합니다.
                if (leftPopup is not null && !rightPageAngleUpperVertical) leftPopup.transform.localScale = Vector3.one;
            }

            // 페이지를 넘기는 각도가 90도를 넘어가면 (오른쪽)
            if (rightPageAngleUpperVertical)
            {
                // 자신 페이지 아래에 눌려 있던 팝업들은 이제 제약이 필요 없습니다 - 완전히 풀립니다.
                if (rightPopup is not null) rightPopup.transform.localScale = Vector3.one;
                if (rightBackPopup is not null) rightBackPopup.transform.localScale = Vector3.one;
                
                // 페이지를 완전히 넘겨가는 경우 자신 페이지와 반대쪽 페이지 위의 팝업은 점점 작아져야 합니다.
                if (rightPopup is not null) rightPopup.transform.localScale = new Vector3(1f, rightPressure, 1f);
                if (leftPopup is not null) leftPopup.transform.localScale = new Vector3(1f, rightPressure, 1f);
            }
            // 페이지를 넘기는 각도가 90도 이하라면
            else
            {
                // 자신 페이지 아래의 팝업들은 눌려 있어야 합니다. 특히 완전히 펼쳐져 있는 경우에 아래에 숨겨져 있을 팝업들의 크기는 0입니다.
                if (fixedRightPopup is not null) fixedRightPopup.transform.localScale = new Vector3(1f, rightPressure, 1f);
                if (rightBackPopup is not null) rightBackPopup.transform.localScale = new Vector3(1f, rightPressure, 1f);
                
                // 90도 이하의 펼친 각도에서 자신 페이지의 팝업은 완전히 풀려 있습니다. - 단 상대 페이지 또한 펼쳐져 있어야 합니다.
                if (rightPopup is not null && !leftPageAngleUpperVertical) rightPopup.transform.localScale = Vector3.one;
            }
        }

        private static void ActivatePopup(GameObject popup, GameObject page)
        {
            popup.SetActive(true);
            popup.transform.SetParent(page.transform);
        }

        private void ActivatePopups(int pageIndex)
        {
            var fixedLeftPopup = GetPopup(pageIndex, PopupPosition.FixedLeft);
            var leftBackPopup = GetPopup(pageIndex, PopupPosition.LeftBack);
            var leftPopup = GetPopup(pageIndex, PopupPosition.Left);
            var rightPopup = GetPopup(pageIndex, PopupPosition.Right);
            var rightBackPopup = GetPopup(pageIndex, PopupPosition.RightBack);
            var fixedRightPopup = GetPopup(pageIndex, PopupPosition.FixedRight);

            if (fixedLeftPopup is not null)
            {
                ResetPopupTransform(fixedLeftPopup);
                ActivatePopup(fixedLeftPopup, fixedLeftPage);
            }
            if (leftBackPopup is not null)
            {
                ActivatePopup(leftBackPopup, leftPage);
                SetBackPopupTransform(leftBackPopup, isLeft: true);
            }
            if (leftPopup is not null)
            {
                ResetPopupTransform(leftPopup);
                ActivatePopup(leftPopup, leftPage);
            }
            if (rightPopup is not null)
            {
                ResetPopupTransform(rightPopup);
                ActivatePopup(rightPopup, rightPage);
            }
            if (rightBackPopup is not null)
            {
                ActivatePopup(rightBackPopup, rightPage);
                SetBackPopupTransform(rightBackPopup, isLeft: false);
            }
            if (fixedRightPopup is not null)
            {
                ResetPopupTransform(fixedRightPopup);
                ActivatePopup(fixedRightPopup, fixedRightPage);
            }

            // 페이지 뒤쪽에 붙는 팝업의 트랜스폼을 설정합니다.
            void SetBackPopupTransform(GameObject popup, bool isLeft)
            {
                popup.transform.localRotation = Quaternion.Euler(180, isLeft ? -90 : 90, 0);
                popup.transform.Translate(0f, -PageThickness, 0f);
            }
            
            // 이전에 페이지 뒤에 붙는 팝업으로 사용되었다면 회전이 바뀌었을 수 있으니 부모 지정 전에 회전값을 초기화합니다.
            void ResetPopupTransform(GameObject popup)
            {
                popup.transform.localRotation = Quaternion.identity;
                popup.transform.localPosition = Vector3.zero;
            }
        }
        
        private static void DeactivatePopup(GameObject popup, GameObject container)
        {
            popup.SetActive(false);
            popup.transform.SetParent(container.transform);
        }

        private void DeactivatePopups(int pageIndex)
        {
            var fixedLeftPopup = GetPopup(pageIndex, PopupPosition.FixedLeft);
            var leftBackPopup = GetPopup(pageIndex, PopupPosition.LeftBack);
            var leftPopup = GetPopup(pageIndex, PopupPosition.Left);
            var rightPopup = GetPopup(pageIndex, PopupPosition.Right);
            var rightBackPopup = GetPopup(pageIndex, PopupPosition.RightBack);
            var fixedRightPopup = GetPopup(pageIndex, PopupPosition.FixedRight);
            
            if (fixedLeftPopup is not null) DeactivatePopup(fixedLeftPopup, leftPopupsContainer);
            if (leftBackPopup is not null) DeactivatePopup(leftBackPopup, rightPopupsContainer);
            if (leftPopup is not null) DeactivatePopup(leftPopup, leftPopupsContainer);
            if (rightPopup is not null) DeactivatePopup(rightPopup, rightPopupsContainer);
            if (rightBackPopup is not null) DeactivatePopup(rightBackPopup, leftPopupsContainer);
            if (fixedRightPopup is not null) DeactivatePopup(fixedRightPopup, rightPopupsContainer);
        }
        
        #region Debug
#if UNITY_EDITOR
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [BoxGroup("Debug"), SerializeField, OnValueChanged(nameof(SetPageIndex_method)), ShowIf(nameof(IsPlayMode_))]
        private int setPageIndex_;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private bool IsPlayMode_ => Application.isPlaying;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private void SetPageIndex_method() => PageIndex = setPageIndex_;
#endif
        #endregion
    }
}
