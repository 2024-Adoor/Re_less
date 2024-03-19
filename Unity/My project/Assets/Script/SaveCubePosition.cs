using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCubePosition : MonoBehaviour
{
    private Vector3 lastPosition; // ������ ����� ť���� ��ġ�� �����ϴ� ����

    void Start()
    {
        // �ʱ⿡ ���� ��ġ�� ���� ��ġ�� ����
        lastPosition = transform.position;
    }

    void Update()
    {
        // ���� ť���� ��ġ�� ���� ��ġ�� ���Ͽ� ����Ǿ����� Ȯ��
        Vector3 currentPosition = transform.position;
        if (currentPosition != lastPosition)
        {
            // ����� ��ġ�� PlayerPrefs�� ����
            PlayerPrefs.SetFloat("CubePosX", currentPosition.x);
            PlayerPrefs.SetFloat("CubePosY", currentPosition.y);
            PlayerPrefs.SetFloat("CubePosZ", currentPosition.z);
            PlayerPrefs.Save(); // ������� ����

            // ����� ��ġ�� ���� ��ġ�� ������Ʈ
            lastPosition = currentPosition;

            // ���������� ����� ��ġ ���
            Debug.Log("Cube Position Changed: " + currentPosition);
        }
    }
}
