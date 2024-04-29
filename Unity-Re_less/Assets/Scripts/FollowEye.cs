using Reless;
using UnityEngine;

public class FollowEye : MonoBehaviour
{
    private Transform _eyeTransform;
    
    public bool keepHorizontal = false;
    
    private void Start()
    {
        _eyeTransform = GameManager.EyeAnchor;
    }

    private void Update()
    {
        transform.position = _eyeTransform.position;
        transform.rotation = keepHorizontal switch
        {
            true => Quaternion.AngleAxis(_eyeTransform.eulerAngles.y, Vector3.up),
            false => _eyeTransform.rotation
        };
    }
}
