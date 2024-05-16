using System;
using System.Collections;
using System.Collections.Generic;
using Reless;
using UnityEngine;

public class Trigger_ChangeMaterial : MonoBehaviour
{
    public Material newMaterial;
    Renderer rendMaterial;

    // Start is called before the first frame update
    void Start()
    {
        rendMaterial = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            rendMaterial.material = newMaterial;
            GetComponent<CreateTrigger>()?.Create();

            if(gameObject.name == "UI_MonitorTrigger")
            {
                gameObject.SetActive(false);
            }
        }
    }
}
