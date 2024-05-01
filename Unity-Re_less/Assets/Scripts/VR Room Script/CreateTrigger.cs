using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class CreateTrigger : MonoBehaviour
    {
        public GameObject nextTrigger;
        public float delayInSeconds = 3f; // ������ �� ���� 

        private Renderer objectRenderer; // ������ ������Ʈ ����

        // Start is called before the first frame update
        void Start()
        {
            nextTrigger.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            objectRenderer = GetComponent<Renderer>(); // ������ ������Ʈ ��������

            // ���� ���׸��� �̸��� "Green"�� ���
            if (objectRenderer.material.name == "Green (Instance)")
            {
                // ���� Ʈ���Ÿ� Ȱ��ȭ
                nextTrigger.SetActive(true);

                StartCoroutine(TriggerSequence());
            }
        }

        IEnumerator TriggerSequence()
        {
            yield return new WaitForSeconds(delayInSeconds);

            gameObject.SetActive(false);
        }
    }
}
