using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{   
    public GameObject Player;
    PlayerState _PlayerState;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {
            _PlayerState = Player.GetComponent<PlayerState>();
        }

        if(_PlayerState.isYUp)
        {
            Destroy(gameObject);
        }
    }
}
