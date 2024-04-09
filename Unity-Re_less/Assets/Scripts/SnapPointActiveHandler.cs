using Oculus.Interaction;
using Reless;
using UnityEngine;

public class SnapPointActiveHandler : MonoBehaviour
{
    [SerializeField, Range(1, 3)]
    private int chapter;
    
    private SnapInteractable _snapInteractable;
    
    // Start is called before the first frame update
    void Start()
    {
        _snapInteractable = GetComponentInChildren<SnapInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 챕터와 일치하는 경우에만 SnapInteractable을 활성화합니다.
        _snapInteractable.enabled = GameManager.Instance.CurrentPhase switch
        {
            GameManager.Phase.Chapter1 => chapter == 1,
            GameManager.Phase.Chapter2 => chapter == 2,
            GameManager.Phase.Chapter3 => chapter == 3,
            _ => false
        };
    }
}
