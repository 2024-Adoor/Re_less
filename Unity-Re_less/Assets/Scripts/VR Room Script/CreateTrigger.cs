using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class CreateTrigger : MonoBehaviour
    {
        public GameObject nextTrigger;
        public float delayInSeconds = 3f; // ������ �� ���� 

        // Start is called before the first frame update
        void Start()
        {
            nextTrigger.SetActive(false);
        }

        public void Create()
        {
            // ���� Ʈ���Ÿ� Ȱ��ȭ
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
