using System;
using Meta.XR.MRUtilityKit;
using Reless.MR;
using UnityEngine;
using UnityEngine.Assertions;

namespace Reless.Ending
{
    public class EndingObjectsSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject polaroidsPrefab;
        
        [SerializeField]
        private GameObject villainPrefab;
        
        private void Awake()
        {
            OnEnding(GameManager.CurrentPhase);
            GameManager.PhaseChanged += OnEnding;
        }
        
        private void OnDestroy()
        {
            GameManager.PhaseChanged -= OnEnding;
        }
        
        /// <summary>
        /// 현재 게임 단계가 Ending이면 엔딩에 필요한 동작을 수행합니다.
        /// </summary>
        private void OnEnding(GamePhase phase)
        {
            if (phase is not GamePhase.Ending) return;

            if (RoomManager.Instance is RoomManager roomManager)
            {
                // 엔딩에 필요한 오브젝트 생성
                CreateEndObjects();
            }
            else
            {
                // RoomManager가 없다면 방이 로드된 뒤에 생성되도록 이벤트에 등록합니다.
                RoomManager.OnMRUKSceneLoaded += CreateEndObjects;
            }
            
            void CreateEndObjects()
            {
                Assert.IsNotNull(RoomManager.Instance);
                
                Transform popupBook = GameManager.Instance.PopupBook.transform;

                // 팝업북의 180도 y축 회전한 값
                var rotation = popupBook.rotation.eulerAngles;
                rotation.y += 180;
                var popupBookBack=  Quaternion.Euler(rotation);
                
                // 팝업북 위치에 폴라로이드 생성
                Instantiate(polaroidsPrefab, popupBook.position, popupBookBack, popupBook.parent);
                
                // 팝업북 위치에 빌런이 생성
                var villain = Instantiate(villainPrefab , popupBook.position, popupBookBack, popupBook.parent);
                
                // 팝업북 제거
                Destroy(popupBook.gameObject);
                
                if (RoomManager.Instance.UniqueDoor is MRUKAnchor door)
                {
                    // 빌런이가 문을 바라보도록 회전
                    villain.transform.LookAt(new Vector3(door.transform.position.x, villain.transform.position.y, door.transform.position.z));
                    villain.transform.Rotate(Vector3.up, 180);
                }
                else
                {
                    throw new NotImplementedException($"{nameof(MainBehaviour)}.{nameof(CreateEndObjects)} only supports a room with a single door");
                }
            }
        }
    }
}