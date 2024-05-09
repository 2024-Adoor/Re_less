using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Reless.VR
{
    /// <summary>
    /// VR에서의 엔딩 담당
    /// </summary>
    public class EndingBehaviour : MonoBehaviour
    {
        [Header("Characters Chat UI")] 
        [SerializeField]
        private GameObject endChatSuji;
        
        [SerializeField]
        private GameObject endChatClock;
        
        [SerializeField]
        private GameObject endChatCat; 
        
        [SerializeField]
        private GameObject endChatCactus;
        
        [Header("Ending Characters")]
        [SerializeField]
        private GameObject suji;
        
        [SerializeField]
        private GameObject endingCharactersPrefab;

        [Header("Surprised Characters")]
        [SerializeField]
        private GameObject sujiSurprisedPrefab; 
        
        [SerializeField] 
        private GameObject CharactersSurprisedPrefab;
        
        [Header("")] 

        [SerializeField]
        private float upwardSpeed;
        
        [SerializeField]
        private Transform endRespawnTrigger;
        
        private PlayerState _playerState;
        private Rigidbody _playerRigidbody;
        
        /// <summary>
        /// 엔딩 중인지 여부
        /// </summary>
        private bool _isInEnding;
        
        
        private void Start()
        {
            endChatSuji.SetActive(false);
            endChatClock.SetActive(false);
            endChatCat.SetActive(false);
            endChatCactus.SetActive(false);
            
            _playerState = FindAnyObjectByType<PlayerState>();
            _playerRigidbody = _playerState.GetComponent<Rigidbody>();
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void StartEnding()
        {
            if (_isInEnding) return;
            _isInEnding = true;
            
            // 엔딩 앰비언트 라이팅 적용
            FindAnyObjectByType<RoomLighting>().ApplyEndingAmbientColor();

            // 엔딩 캐릭터 생성
            var endingCharacters = Instantiate(endingCharactersPrefab);
            
            StartCoroutine(Ending());

            IEnumerator Ending()
            {
                yield return ShowEndChat();
                yield return new WaitForSeconds(2f);
                _playerRigidbody.useGravity = false;
                
                // 기존 엔딩 캐릭터 제거
                Destroy(suji);
                Destroy(endingCharacters);
                
                // 놀라는 캐릭터 생성
                //// : 프리팹에 정해진 위치를 불러와서 위치를 정하면 위치가 바뀌면 안맞을수도 있음... 가능하면 수정 필요
                Instantiate(sujiSurprisedPrefab, sujiSurprisedPrefab.transform.position, sujiSurprisedPrefab.transform.rotation);
                Instantiate(CharactersSurprisedPrefab, CharactersSurprisedPrefab.transform.position, CharactersSurprisedPrefab.transform.rotation);

                while (_playerState.transform.position.y < endRespawnTrigger.position.y)
                {
                    UpPlayerToward(endRespawnTrigger);
                    yield return null;
                }
                
            }
        }
        
        private IEnumerator ShowEndChat()
        {
            // 대사를 보여줄 시간
            var showingDuration = new WaitForSeconds(1f); 
            
            // 1. 수지 대사
            endChatSuji.SetActive(true);
            yield return showingDuration;
            endChatSuji.SetActive(false);

            // 2. 시계토끼 대사
            endChatClock.SetActive(true);
            yield return showingDuration;
            endChatClock.SetActive(false);

            // 3. 체셔캣 대사
            endChatCat.SetActive(true);
            yield return showingDuration;
            endChatCat.SetActive(false);

            // 4. 선인장 대사
            endChatCactus.SetActive(true);
            yield return showingDuration;
            endChatCactus.SetActive(false);        
        }
        
        private void UpPlayerToward(Transform target)
        {   
            // y축 방향으로의 거리 계산
            float distanceToTargetY = Mathf.Abs(target.position.y - _playerState.transform.position.y);
    
            // 이동하는데 필요한 시간 계산
            float timeToReachTargetY = distanceToTargetY / upwardSpeed;
    
            // 목표 지점까지 일정한 속도로 y축 이동
            float newY = Mathf.MoveTowards(_playerState.transform.position.y, target.position.y, upwardSpeed * Time.deltaTime);
        
            // 현재 x와 z 위치를 유지한 채로 y값을 갱신하여 새로운 위치 설정
            Vector3 newPosition = new Vector3(_playerState.transform.position.x, newY, _playerState.transform.position.z);
        
            // 새로운 위치로 이동
            _playerState.transform.position = newPosition;
        }
    }
}
