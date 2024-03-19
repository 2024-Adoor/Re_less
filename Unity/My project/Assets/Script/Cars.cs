using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cars : MonoBehaviour
{   
    public float CarSpeed = 1.0f; 
    public float CarDirection = 1.0f;
    public bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            transform.position += transform.forward * CarSpeed * CarDirection * Time.deltaTime;
        }
    }
}
