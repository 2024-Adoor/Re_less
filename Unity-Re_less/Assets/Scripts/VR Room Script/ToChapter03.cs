using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class ToChapter03 : MonoBehaviour
    {
        public Canvas  UI;
        UI_Canvas _UI_Canvas;

        // Start is called before the first frame update
        void Start()
        {
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
                _UI_Canvas.MonitorOn = true;
            }
        }
    }
}
