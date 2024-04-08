using Meta.XR.MRUtilityKit;
using Reless;
using UnityEngine;

/// <summary>
/// é�Ͱ� ������ �޿��� ������ ���� �н����� �� ������ ����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class ExitDreamBehaviour : MonoBehaviour
{
    private GameManager _gameManager;
    
    [SerializeField]
    private OVRPassthroughLayer roomSurfacePassthrough;
    
    private Vector3 _startPlayerPosition;

    private Vector3 _doorPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _startPlayerPosition = _gameManager.PlayerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
