using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimation : MonoBehaviour
{
   public float rotationSpeed = 100.0f;
    public float upDownSpeed = 1.0f;
    public float upDownRange = 1.0f;
    public float minStayTime = 1.0f;
    public float maxStayTime = 0.5f;

    private float startY;
    private float stayTime = 0.0f;
    private bool goingUp = true;

    void Start()
    {
        // ������Ʈ�� �ʱ� y ��ġ�� ����մϴ�.
        startY = transform.position.y;

        // ������ �� �ִϸ��̼��� ����մϴ�.
        PlayAnimation();
    }

    void Update()
    {
        // ȸ���� ���� �Է��� ���� �ʰ�, ������ �� �ٷ� ����մϴ�.
        // ���Ʒ����� �������� �����մϴ�.

        // upDownSpeed�� ���� ���ο� y ��ġ�� ����մϴ�.
        float newY = startY + Mathf.Sin(Time.time * upDownSpeed) * upDownRange;

        // ���ο� ��ġ�� ���� ��ġ�� ���̰� �ſ� ���� ��, ���� ������ �����մϴ�.
        if (Mathf.Abs(newY - transform.position.y) < 0.001f)
        {
            // ���� �ö󰡰� �ְ�, �ְ����� �����ߴٸ�
            if (goingUp && newY > transform.position.y)
            {
                // �ְ����� ���������Ƿ� �������� �Ʒ��� �������� �մϴ�.
                goingUp = false;
                // �ְ����� ���������Ƿ� �ӹ����� �ð��� �ּ� �ӹ����� �ð����� �����մϴ�.
                stayTime = minStayTime;
            }
            // �Ʒ��� �������� �ְ�, �������� �����ߴٸ�
            else if (!goingUp && newY < transform.position.y)
            {
                // �������� ���������Ƿ� �������� ���� �ö󰡾� �մϴ�.
                goingUp = true;
                // �������� ���������Ƿ� �ӹ����� �ð��� �ִ� �ӹ����� �ð����� �����մϴ�.
                stayTime = maxStayTime;
            }
        }
        // ���ο� ��ġ�� ������Ʈ�� �̵��մϴ�.
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, newY, transform.position.z), Time.deltaTime);

        // ���� ������ ���, �ӹ����� �ð��� ī��Ʈ �ٿ��մϴ�.
        if (stayTime > 0)
        {
            stayTime -= Time.deltaTime;
        }
    }
}
