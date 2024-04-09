using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEndScene : MonoBehaviour
{
    // ���ο� ���� �̸�
    public string sceneName;

    // �浹�� �߻����� �� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        // ���� �浹�� ��ü�� �±װ� "Player"�� ��쿡�� ���� ��ȯ�մϴ�.
        if (other.CompareTag("Player"))
        {
            // �� ��ȯ
            SceneManager.LoadScene(sceneName);
        }
    }
}
