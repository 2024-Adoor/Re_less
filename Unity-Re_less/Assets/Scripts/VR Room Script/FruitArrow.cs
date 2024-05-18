using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitArrow : MonoBehaviour
{
    public float minScale = 0.5f; // �Ʒ��� ������ ���� �ּ� ������
    public float speed = 5.0f; // ���� �ӵ�

    // ��ũ�� Fruit 
    public GameObject Fruit;

    private Vector3 originalScale; // ������Ʈ�� ���� ������

    void Start()
    {
        originalScale = transform.localScale; // ������Ʈ�� ���� ������ ����
    }

    void Update()
    {
        if(Fruit == null)
        {
            Destroy(gameObject);
        }

        /*// ������Ʈ�� �� �Ʒ��� �����ϴ� ���� �ùķ��̼��ϱ� ���� Sin �Լ� ���
        float newY = Mathf.Sin(Time.time * speed) * 0.5f; // ���� ��ȭ ���

        // ���ο� ������ ���
        float scale = Mathf.Lerp(minScale, 1.0f, newY + 0.5f); // Sin �Լ��� ���� -1 ~ 1�̹Ƿ� 0 ~ 1 ������ ��ȯ
        Vector3 newScale = originalScale * scale;

        // ���ο� ������ ����
        transform.localScale = newScale;*/
    }
}
