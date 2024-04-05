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
        // 오브젝트의 초기 y 위치를 기억합니다.
        startY = transform.position.y;

        // 시작할 때 애니메이션을 재생합니다.
        PlayAnimation();
    }

    void Update()
    {
        // 회전은 이제 입력을 받지 않고, 시작할 때 바로 재생합니다.
        // 위아래로의 움직임을 구현합니다.

        // upDownSpeed에 따라 새로운 y 위치를 계산합니다.
        float newY = startY + Mathf.Sin(Time.time * upDownSpeed) * upDownRange;

        // 새로운 위치와 현재 위치의 차이가 매우 작을 때, 멈춘 것으로 간주합니다.
        if (Mathf.Abs(newY - transform.position.y) < 0.001f)
        {
            // 위로 올라가고 있고, 최고점에 도달했다면
            if (goingUp && newY > transform.position.y)
            {
                // 최고점에 도달했으므로 다음에는 아래로 내려가야 합니다.
                goingUp = false;
                // 최고점에 도달했으므로 머무르는 시간을 최소 머무르는 시간으로 설정합니다.
                stayTime = minStayTime;
            }
            // 아래로 내려가고 있고, 최저점에 도달했다면
            else if (!goingUp && newY < transform.position.y)
            {
                // 최저점에 도달했으므로 다음에는 위로 올라가야 합니다.
                goingUp = true;
                // 최저점에 도달했으므로 머무르는 시간을 최대 머무르는 시간으로 설정합니다.
                stayTime = maxStayTime;
            }
        }
        // 새로운 위치로 오브젝트를 이동합니다.
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, newY, transform.position.z), Time.deltaTime);

        // 멈춘 상태인 경우, 머무르는 시간을 카운트 다운합니다.
        if (stayTime > 0)
        {
            stayTime -= Time.deltaTime;
        }
    }
}
