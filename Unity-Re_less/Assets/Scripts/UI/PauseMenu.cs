using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Logger = Reless.Debug.Logger;

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
        
        [SerializeField]
        private TMP_Text qualitySettingLabel;
        
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
            qualitySettingLabel.text = "Quality Setting Level: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
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
                Logger.Log(value ? "Enable" : "Disable" + " the SpaceWarp");
            }
        }
    }
}
