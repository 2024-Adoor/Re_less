using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTemp : MonoBehaviour
{
    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = target.transform.position;
        }
    }
}
