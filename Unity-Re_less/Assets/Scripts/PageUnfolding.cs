using Oculus.Interaction;
using UnityEngine;

public class PageUnfolding : MonoBehaviour
{
    private InteractableGroupView _interactableGroupView;
    
    private float _foldingSpeed = 1;

    const float MinAngle = 0;
    const float MaxAngle = 180;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactableGroupView.State == InteractableState.Select) return;
        
        switch (transform.rotation.x)
        {
            case <= MinAngle:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case > MinAngle and < 90:
                transform.Rotate(Vector3.right, Time.deltaTime * -_foldingSpeed);
                break;
            case > 90 and < MaxAngle:
                transform.Rotate(Vector3.right, Time.deltaTime * _foldingSpeed);
                break;
            case <= MaxAngle:
                transform.rotation = Quaternion.Euler(180, 0, 0);
                break;
        }
    }
}
