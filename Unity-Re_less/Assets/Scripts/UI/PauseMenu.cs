using System;
using System.Collections;
using System.Linq;
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
        
        [SerializeField]
        private TMP_Dropdown gamePhaseDropdown;

        private void Start()
        {
            Disable();
            framerate.gameObject.SetActive(_showFramerate);
            qualitySettingLabel.text = "Quality Setting Level: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
            
            AddGamePhaseDropdownOptions();
            UpdateGamePhaseDropdown(GameManager.CurrentPhase);
            GameManager.PhaseChanged += UpdateGamePhaseDropdown;
        }
        
        private void AddGamePhaseDropdownOptions()
        {
            gamePhaseDropdown.ClearOptions();
            gamePhaseDropdown.AddOptions(Enum.GetNames(typeof(GamePhase)).ToList());
            gamePhaseDropdown.onValueChanged.AddListener(index => GameManager.CurrentPhase = (GamePhase)index);
        }
        
        private void UpdateGamePhaseDropdown(GamePhase phase)
        {
            gamePhaseDropdown.value = (int)phase;
        }
        
        public void Enable()
        {
            gameObject.SetActive(true);
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
        }

#region Binding Actions

        public static bool SpaceWarp
        {
            set
            {
                OVRManager.SetSpaceWarp(value);
                Logger.Log(value ? "Enable" : "Disable" + " the SpaceWarp");
            }
        }

        public static void LoadMainScene() => GameManager.LoadMainScene();

        public static void LoadVRScene() => GameManager.LoadVRScene();

        public bool ShowFramerate
        {
            set
            {
                _showFramerate = value;
                framerate.gameObject.SetActive(value);
            }
        }
#endregion
    }
}
