using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEndScene : MonoBehaviour
{
    // 새로운 씬의 이름
    public string sceneName;

    // 충돌이 발생했을 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 만약 충돌한 객체의 태그가 "Player"인 경우에만 씬을 전환합니다.
        if (other.CompareTag("Player"))
        {
            // 씬 전환
            SceneManager.LoadScene(sceneName);
        }
    }
}
