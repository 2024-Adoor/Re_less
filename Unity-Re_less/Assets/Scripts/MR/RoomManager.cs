using System;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;

namespace Reless.MR
{
    /// <summary>
    /// MRUK를 통한 현실 방 관리
    /// </summary>
    public class RoomManager : MonoBehaviour
    {
        [NonSerialized]
        public MRUKRoom Room;
        
        public static RoomManager Instance 
        {
            get
            {
                // 이미 인스턴스가 존재하면 반환합니다.
                if (_instance != null) return _instance;
                
                // 씬에서 RoomManager를 찾습니다.
                _instance = FindObjectOfType<RoomManager>();
                if (_instance != null) return _instance;
                
                // 씬에 RoomManager가 없으면 새로 생성합니다.
                Debug.LogWarning($"{nameof(RoomManager)}: There is no instance in the scene. Creating new one.");
                _instance = new GameObject(nameof(RoomManager)).AddComponent<RoomManager>();
                return _instance;
            }
        }
        private static RoomManager _instance;
        
        // Start is called before the first frame update
        void Awake()
        {
            if (_instance is not null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            MakeObjectDontDestroyOnLoad();
        }

        private void MakeObjectDontDestroyOnLoad()
        {
            if (_instance == null)
            {
                Debug.Log($"{nameof(RoomManager)}: Making object don't destroy on load.");
                DontDestroyOnLoad(Instance.gameObject);
            }
        }

        public bool RoomObjectActive { set => gameObject.SetActive(value); }
            

        public void OnSceneLoaded()
        {
            Debug.Log($"{nameof(RoomManager)}: Scene loaded.");
            
            DebugRoomInfo();
            
            // 생성된 방을 자식으로 설정합니다.
            Room = MRUK.Instance.GetCurrentRoom();
            Room.transform.parent = this.transform;
        }

        private void DebugRoomInfo()
        {
            var rooms = MRUK.Instance.GetRooms();
            Debug.Log($"{nameof(RoomManager)}: Room count: {rooms.Count}");
            foreach (var room in rooms)
            {
                Debug.Log($"{nameof(RoomManager)}: Room name: {room.name}");
            }
        }
        

        // Update is called once per frame
        void Update()
        {
        }
        
        void Temp(){}
    }
}
