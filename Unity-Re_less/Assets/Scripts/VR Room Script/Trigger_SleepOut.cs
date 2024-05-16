using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class Trigger_SleepOut : MonoBehaviour
    {
        public GameObject Character;
        public GameObject NextTrigger;
        public float delayInSeconds = 3f; // 딜레이 초 설정 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(Character.name == "Character_Cat")
                {
                    Cat_AniManage _Cat_AniManage = Character.GetComponent<Cat_AniManage>();

                    _Cat_AniManage.isSleepOut = true;

                    TriggerSequence();
                }
                else
                {
                    AniManage _AniManage = Character.GetComponent<AniManage>();

                    _AniManage.isSleepOut = true;

                    TriggerSequence();
                }
            }
        }

        IEnumerator TriggerSequence()
        {
            yield return new WaitForSeconds(delayInSeconds);

            gameObject.SetActive(false);
        }
    }
}
