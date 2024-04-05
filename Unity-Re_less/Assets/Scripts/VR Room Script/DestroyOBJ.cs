using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOBJ : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 "CH02_OBJ"인 경우
        if (other.gameObject.CompareTag("CH02_OBJ"))
        {
            // 충돌한 오브젝트 삭제
            Destroy(other.gameObject);
            // Debug.Log("Trigger detected");
        }
    }
}
