using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Reless.Debug.Logger;

namespace Reless
{
    public class CH02_Door : MonoBehaviour
    {
        public GameObject Player;
        public GameObject Door2;
        ChapterControl _ChapterControl;
        PlayerState _PlayerState;

        // Start is called before the first frame update
        void Start()
        {
            _ChapterControl = Player.GetComponent<ChapterControl>();
            _PlayerState = Player.GetComponent<PlayerState>();
            Door2.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(_PlayerState.FruitCount == 2)
            {
                Logger.Log("FruitCount is 2");
                Door2.SetActive(true);
                if(Door2.activeSelf)
                {
                    Logger.Log("Door2 is active, this door is destroyed.");
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
