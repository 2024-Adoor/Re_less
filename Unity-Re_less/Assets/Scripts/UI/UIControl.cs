using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reless.UI
{
    /// <summary>
    /// UI를 제어하는 클래스입니다.
    /// </summary>
    public class UIControl : MonoBehaviour
    {
        private InputActions _inputActions;
        private InputAction _openPauseMenuAction;
        
        [SerializeField] 
        private PauseMenu pauseMenu;
        
        // Start is called before the first frame update
        private void Start()
        {
            _inputActions = GameManager.InputActions;
            _openPauseMenuAction = _inputActions.Constant.TogglePauseMenu;
            _openPauseMenuAction.performed += TogglePauseMenu;
            _inputActions.Enable();
        }

        private void OnDestroy()
        {
            _openPauseMenuAction.performed -= TogglePauseMenu;
            _inputActions.Disable();
        }

        private void TogglePauseMenu(InputAction.CallbackContext context)
        {
            bool opened = pauseMenu.isActiveAndEnabled;
            
            if (opened)
            {
                _inputActions.VR.Enable();
                _inputActions.MR.Enable();
                pauseMenu.Disable();
            }
            else
            {
                _inputActions.VR.Disable();
                _inputActions.MR.Disable();
                pauseMenu.Enable();
            }
            
        }
    }
}


