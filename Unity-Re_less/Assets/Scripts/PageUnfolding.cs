using System;
using NaughtyAttributes;
using Oculus.Interaction;
using UnityEngine;

public class PageUnfolding : MonoBehaviour
{
    [SerializeField]
    private InteractableGroupView _interactableGroupView;
    
    [SerializeField]
    private float _speed;

    [SerializeField] 
    private bool _left;
    
    [SerializeField]
    private float _acceleration = 0.2f;
    
    private float _accelerationTime;
    
    private float _yAngle;

    private void LateUpdate()
    {
        if (_interactableGroupView.State == InteractableState.Select)
        {
            if (_left ? transform.up.x < 0 : transform.up.x > 0)
            {
                if (_left ? transform.forward.x < 0 : transform.forward.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0f, _left ? -90f: 90f, 0f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(-180f, _left ? -90f: 90f, 0f);
                }
            }
            _accelerationTime = 0;
            return;
        }

        _accelerationTime += _acceleration;
            

        if (_left ? transform.forward.x < 0 : transform.forward.x > 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, _left ? -90f: 90f, 0f), Time.deltaTime * _speed * _accelerationTime);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(-180f, _left ? -90f: 90f, 0f), Time.deltaTime * _speed * _accelerationTime);
        }
    }
}
