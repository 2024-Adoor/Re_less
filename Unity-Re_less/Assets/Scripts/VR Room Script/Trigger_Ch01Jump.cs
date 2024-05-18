using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class Trigger_Ch01Jump : MonoBehaviour
    {
        // 챕터 1 열매 
        public GameObject ch01Fruit;
        bool isFin = false;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            // 챕터 1 열매가 비활성화 되었을때 트리거 활성화 && 한번만 실행
            if(!ch01Fruit.activeSelf && !isFin)
            {
                gameObject.SetActive(true);
                isFin = true;
            }
        }
    }
}
