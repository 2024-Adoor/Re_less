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
        private TMP_Text framerate;
        
        private bool _showFramerate;
        
        private Coroutine _checkFramerate;
        
        public bool ShowFramerate
        {
            set
            {
                _showFramerate = value;
                framerate.gameObject.SetActive(value);
                
                if (_showFramerate)
                {
                    var updateInterval = new WaitForSecondsRealtime(0.5f);
                    _checkFramerate = StartCoroutine(CheckFramerate(updateInterval));
                }
            }
        }

        private void Start()
        {
            framerate.gameObject.SetActive(_showFramerate);
        }
        
        private IEnumerator CheckFramerate(WaitForSecondsRealtime updateInterval)
        {
            while (true)
            {
                yield return updateInterval;
                framerate.text = $"FPS: {1 / Time.unscaledDeltaTime}";
                
                if (_showFramerate is false) yield break;
            }
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
