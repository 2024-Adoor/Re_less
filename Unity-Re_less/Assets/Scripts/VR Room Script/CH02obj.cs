using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH02obj : MonoBehaviour
{   
    public float Speed = 1.0f; 
    public float Direction = 1.0f;
    public bool isMoving = true;
    public Collider endTrigger;
    
    public event Action<CH02obj> EndTriggerEntered;

    private void OnEnable()
    {
        isMoving = true;
    }
    
    private void OnDisable()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if(isMoving)
        {
            // z축 방향으로 이동
            Vector3 movement = new Vector3(0, 0, Speed * Direction * Time.deltaTime);
            transform.position += movement;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other == endTrigger)
        {
            EndTriggerEntered?.Invoke(this);
        }
    }
}