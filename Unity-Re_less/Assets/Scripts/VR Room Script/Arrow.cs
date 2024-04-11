using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float minScale = 0.5f; // 아래로 내려갈 때의 최소 스케일
    public float speed = 5.0f; // 진동 속도

    // 플레이어
    public GameObject Player;
    PlayerState _PlayerState;

    private Vector3 originalScale; // 오브젝트의 원래 스케일

    void Start()
    {
        originalScale = transform.localScale; // 오브젝트의 원래 스케일 저장
    }

    void Update()
    {
        _PlayerState = Player.GetComponent<PlayerState>();

        // 오브젝트가 위 아래로 진동하는 것을 시뮬레이션하기 위한 Sin 함수 사용
        float newY = Mathf.Sin(Time.time * speed) * 0.5f; // 높이 변화 계산

        // 새로운 스케일 계산
        float scale = Mathf.Lerp(minScale, 1.0f, newY + 0.5f); // Sin 함수의 값이 -1 ~ 1이므로 0 ~ 1 범위로 변환
        Vector3 newScale = originalScale * scale;

        // 새로운 스케일 적용
        transform.localScale = newScale;

        // Destroy - Player 트리거 isFriendUI
        if(_PlayerState.isFriendUI)
        {
            Destroy(gameObject);
        }
    }
}
