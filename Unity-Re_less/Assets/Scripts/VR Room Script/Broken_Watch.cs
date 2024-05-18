using System.Linq;
using UnityEngine;

public class Broken_Watch : MonoBehaviour
{
    private int collisionCount = 0;     // 충돌 횟수를 저장할 변수
    public GameObject newPrefab1;  // 변경할 프리팹1
    public GameObject newPrefab2;  // 변경할 프리팹2
    
    /// <summary>
    /// 때리기를 감지하기 위한 플레이어의 손(컨트롤러 포함)에 있는 콜라이더들
    /// </summary>
    [SerializeField]
    private Collider[] handColliders;

    public Ch02ObjectSpawner[] ch02ObjectSpawners;

    GameObject newObject_1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (handColliders.Contains(other))
        {
            collisionCount++; // 충돌 횟수 증가
            Debug.Log("충돌 횟수: " + collisionCount);

            // 충돌 횟수 1이면 프리팹 변경 1
            if (collisionCount == 1)
            {
                //gameObject.SetActive(false);
                newObject_1 = Instantiate(newPrefab1, newPrefab1.transform.position, newPrefab1.transform.rotation);
            }
            // 충돌 횟수 2이면 프리팹 변경 2
            else if (collisionCount == 2)
            {
                Destroy(gameObject);
                Destroy(newObject_1);
                Instantiate(newPrefab2, newPrefab2.transform.position, newPrefab2.transform.rotation);

                foreach (var spawner in ch02ObjectSpawners) { spawner.StopSpawn(); }
            }
        }
    }
}
