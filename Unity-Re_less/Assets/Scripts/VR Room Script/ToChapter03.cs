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
                // é�� 3 ���� ����
                // ����� ����� é��3 ���� UI ����� bool�� ó�� 
                Ch03Trigger = true;

                // ����� ���� on (off���� ��Ȱ��ȭ, ����� ��ư ���׸��� ����)
                OffScreen.SetActive(false);
                OnScreen.SetActive(true);
                ButtonRenderer.material = newMaterial;

                _UI_Canvas.Chapter03_StartUI();
            }
        }
    }
}
