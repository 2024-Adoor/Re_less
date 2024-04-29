using UnityEngine;

namespace Reless
{
    /// <summary>
    /// UI가 눈을 따라가도록 합니다.
    /// </summary>
    public class FollowEye : MonoBehaviour
    {
        private Transform _eyeTransform;
    
        public bool keepHorizontal;
    
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
}


