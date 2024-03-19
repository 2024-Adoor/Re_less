using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCubePosition : MonoBehaviour
{
    public GameObject stonePrefab; // 프리팹을 직접 할당할 변수
    private GameObject stoneInstance; // 생성된 프리팹 인스턴스를 저장할 변수

    public Vector3 prefabScale = Vector3.one; // 프리팹의 스케일을 설정할 변수
    public Vector3 prefabOffset = Vector3.zero; // 프리팹의 위치 오프셋을 설정할 변수

    void Start()
    {
        if (stonePrefab != null)
        {
            // 저장된 Cube의 위치 불러와서 Stone 오브젝트에 적용
            float posX = PlayerPrefs.GetFloat("CubePosX");
            float posY = PlayerPrefs.GetFloat("CubePosY");
            float posZ = PlayerPrefs.GetFloat("CubePosZ");

            // 프리팹 인스턴스 생성 및 스케일 및 오프셋 적용
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
