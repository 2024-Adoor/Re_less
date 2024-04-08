using Meta.XR.MRUtilityKit;
using Reless;
using UnityEngine;

/// <summary>
/// 챕터가 끝나고 꿈에서 나가기 위한 패스스루 문 동작을 담당하는 클래스입니다.
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
