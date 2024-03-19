using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{   
    public GameObject[] cars; // ������ų �ڵ��� ������Ʈ���� �迭

    public SpawnCars[] spawnCarsScripts; // SpawnCars ��ũ��Ʈ �迭

    public GameObject objectToChangeMaterial_1; // ���׸����� ������ ������Ʈ
    public GameObject objectToChangeMaterial_2;

    public Material newMaterial_1; // ������ ���ο� ���׸���
    public Material newMaterial_2;

    public float buttonPressDepth = 0.5f; // ��ư�� �Ʒ��� ������ �Ÿ�
    public float movementSpeed = 1.0f; // �������� �ӵ�

    private bool isPressed = false; // ��ư�� ���ȴ��� ����
    private Vector3 initialPosition; // �ʱ� ��ġ

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            transform.position += Vector3.down * movementSpeed * Time.deltaTime;

            // ��ư�� ��ǥ ��ġ�� �����ϸ� isPressed�� false�� �����Ͽ� �� �̻� �������� �ʵ��� ��
            if (transform.position.y <= initialPosition.y - buttonPressDepth)
            {
                isPressed = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ��ư�� "Player" ������Ʈ�� �浹���� ���� ��ư�� �۵�
        if(collision.gameObject.tag == "Player")
        {
            // ��ư�� ������, ���׸����� ����
            isPressed = true;
            objectToChangeMaterial_1.GetComponent<Renderer>().material = newMaterial_1;
            objectToChangeMaterial_2.GetComponent<Renderer>().material = newMaterial_2;

            // ��� �ڵ��� ������Ʈ�� �̵��� ����
            foreach(GameObject car in cars)
            {
                Cars carScript = car.GetComponent<Cars>();
                if(carScript != null)
                {
                    carScript.isMoving = false;
                }
            }

            // ��� SpawnCars ��ũ��Ʈ�� ���� ������ ���� ���� �޼��� ȣ��
            foreach(SpawnCars spawnCarsScript in spawnCarsScripts)
            {
                spawnCarsScript.StopSpawn();
            }
        }
    }
}
