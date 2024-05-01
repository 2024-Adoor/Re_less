using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class CreateTrigger : MonoBehaviour
    {
        public GameObject nextTrigger;
        public float delayInSeconds = 3f; // 딜레이 초 설정 

        private Renderer objectRenderer; // 렌더러 컴포넌트 저장

        // Start is called before the first frame update
        void Start()
        {
            nextTrigger.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            objectRenderer = GetComponent<Renderer>(); // 렌더러 컴포넌트 가져오기

            // 현재 머테리얼 이름이 "Green"인 경우
            if (objectRenderer.material.name == "Green (Instance)")
            {
                // 다음 트리거를 활성화
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
