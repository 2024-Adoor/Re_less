using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOBJ : MonoBehaviour
{
    // 충돌 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "Destroy"인 경우
        if (collision.gameObject.CompareTag("CH02_OBJ"))
        {
            // 충돌한 오브젝트 삭제
            Destroy(collision.gameObject);
            Debug.Log("Collision detected");
        }
    }
}
