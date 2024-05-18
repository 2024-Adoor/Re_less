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
        
        private Renderer _renderer;
        
        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(endingCharacters.activeSelf)
            {
                _renderer.enabled = true;
            }

            if(_EndingBehaviour._isEndChatFin)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
