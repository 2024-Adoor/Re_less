using System;
using UnityEngine;

public class Ch02Object : MonoBehaviour
{
    [NonSerialized]
    public float Speed = 1.0f;
    
    [NonSerialized]
    public float Direction = 1.0f;
    
    /// <summary>
    /// 이 오브젝트에 대한 엔드 트리거
    /// </summary>
    [NonSerialized]
    public Collider EndTrigger;

    /// <summary>
    /// 엔드 트리거에 진입했을 때 발생하는 이벤트
    /// </summary>
    public event Action<Ch02Object> EndTriggerEntered;

    // Update is called once per frame
    private void Update()
    {
        // z축 방향으로 이동
        Vector3 movement = new Vector3(0, 0, Speed * Direction * Time.deltaTime);
        transform.position += movement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == EndTrigger)
        {
            // 이벤트 발생시키기
            EndTriggerEntered?.Invoke(this);
        }
    }
}