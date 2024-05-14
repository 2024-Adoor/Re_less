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
                // é�� 3 ���� ����
                // ����� ����� é��3 ���� UI ����� bool�� ó�� 
                _UI_Canvas.MonitorOn = true;
            }
        }
    }
}
