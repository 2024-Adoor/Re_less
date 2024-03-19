using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCubePosition : MonoBehaviour
{
    public GameObject stonePrefab; // �������� ���� �Ҵ��� ����
    private GameObject stoneInstance; // ������ ������ �ν��Ͻ��� ������ ����

    public Vector3 prefabScale = Vector3.one; // �������� �������� ������ ����
    public Vector3 prefabOffset = Vector3.zero; // �������� ��ġ �������� ������ ����

    void Start()
    {
        if (stonePrefab != null)
        {
            // ����� Cube�� ��ġ �ҷ��ͼ� Stone ������Ʈ�� ����
            float posX = PlayerPrefs.GetFloat("CubePosX");
            float posY = PlayerPrefs.GetFloat("CubePosY");
            float posZ = PlayerPrefs.GetFloat("CubePosZ");

            // ������ �ν��Ͻ� ���� �� ������ �� ������ ����
            Vector3 spawnPosition = new Vector3(posX, posY, posZ) + prefabOffset;
            stoneInstance = Instantiate(stonePrefab, spawnPosition, Quaternion.identity);
            stoneInstance.transform.localScale = prefabScale;
        }
        else
        {
            Debug.LogError("Stone prefab is not assigned!");
        }
    }
}
