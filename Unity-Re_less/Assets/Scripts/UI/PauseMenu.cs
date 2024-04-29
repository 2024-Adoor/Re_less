using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Reless.UI
{
    /// <summary>
    /// 일시정지 메뉴를 제어하고 하위 UI 요소에 등록할 액션들을 정의합니다.
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private Framerate framerate;
        
        private bool _showFramerate;
        
        public bool ShowFramerate
        {
            set
            {
                _showFramerate = value;
                framerate.gameObject.SetActive(value);
            }
        }

        private void Start()
        {
            Disable();
            framerate.gameObject.SetActive(_showFramerate);
        }
        
        public void Enable()
        {
            gameObject.SetActive(true);
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public static bool SpaceWarp
        {
            set
            {
                OVRManager.SetSpaceWarp(value);
                Debug.Log(value ? "Enable" : "Disable" + " the SpaceWarp");
            }
        }
    }
}
