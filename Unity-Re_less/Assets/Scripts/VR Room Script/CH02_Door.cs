using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Door2.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            _ChapterControl = Player.GetComponent<ChapterControl>();
            _PlayerState = Player.GetComponent<PlayerState>();

            if(_ChapterControl.CurrentChapter is Chapter.Chapter2 && _PlayerState.FruitCount == 2)
            {
                Door2.SetActive(true);
                if(Door2.activeSelf)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
