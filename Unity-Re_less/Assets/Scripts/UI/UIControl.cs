using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reless.UI
{
    public class UIControl : MonoBehaviour
    {
        private InputActions _inputActions;
        private InputAction _openPauseMenuAction;
        
        [SerializeField] private GameObject _pauseMenu;
        
        // Start is called before the first frame update
        private void Start()
        {
            _inputActions = GameManager.InputActions;
            _openPauseMenuAction = _inputActions.Constant.TogglePauseMenu;
            _openPauseMenuAction.performed += _ => TogglePauseMenu();
            _inputActions.Enable();
        }

        private void TogglePauseMenu()
        {
            bool opened = _pauseMenu.activeSelf;
            
            if (opened)
            {
                _inputActions.VR.Enable();
                _inputActions.MR.Enable();
            }
            else
            {
                _inputActions.VR.Disable();
                _inputActions.MR.Disable();
            }
            _pauseMenu.SetActive(!opened);
        }
    }
}


