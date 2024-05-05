using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class CreateTrigger : MonoBehaviour
    {
        public GameObject nextTrigger;
        public float delayInSeconds = 3f; // 딜레이 초 설정 

        // Start is called before the first frame update
        void Start()
        {
            nextTrigger.SetActive(false);
        }

        public void Create()
        {
            // 다음 트리거를 활성화
            nextTrigger.SetActive(true);
            StartCoroutine(TriggerSequence());
        }

        IEnumerator TriggerSequence()
        {
            yield return new WaitForSeconds(delayInSeconds);

            gameObject.SetActive(false);
        }
    }
}
