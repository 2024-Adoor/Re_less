using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class ToChapter03 : MonoBehaviour
    {
        public bool Ch03Trigger = false;

        public GameObject OnScreen; 
        public GameObject OffScreen;

        public GameObject MonitorButton;
        Renderer ButtonRenderer;
        public Material newMaterial; 
        
        public Canvas  UI;
        UI_Canvas _UI_Canvas;

        // Start is called before the first frame update
        void Start()
        {
            ButtonRenderer = MonitorButton.GetComponent<Renderer>(); 
            OnScreen.SetActive(false);
            _UI_Canvas = UI.GetComponent<UI_Canvas>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                // 챕터 3 시작 세팅
                // 모니터 보라는 챕터3 시작 UI 제어용 bool값 처리 
                Ch03Trigger = true;

                // 모니터 전원 on (off오브 비활성화, 모니터 버튼 머테리얼 변경)
                OffScreen.SetActive(false);
                OnScreen.SetActive(true);
                ButtonRenderer.material = newMaterial;

                _UI_Canvas.Chapter03_StartUI();
            }
        }
    }
}
