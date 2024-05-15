using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reless.VR;

namespace Reless
{
    public class SetActive : MonoBehaviour
    {   
        public GameObject endingCharacters;

        // Ending
        [SerializeField]
        EndingBehaviour _EndingBehaviour;
        
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Renderer>().enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(endingCharacters.activeSelf)
            {
                GetComponent<Renderer>().enabled = true;
            }

            if(_EndingBehaviour._isEndChatFin)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
