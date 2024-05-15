using System;
using Reless.MR;
using UnityEngine;

namespace Reless.Ending
{
    /// <summary>
    /// 공간을 엔딩 씬에 맞게 변환합니다.
    /// </summary>
    public class EndingRoomTransformer : MonoBehaviour
    {
        private void Awake()
        {
            RoomManager.TryInvokeAction(TransformToOuter);
        }
        
        /// <summary>
        /// 공간을 문을 중심으로 바깥을 향하도록 상대적으로 변환합니다.
        /// </summary>
        private void TransformToOuter(RoomManager roomManager)
        {
            if (roomManager.UniqueDoor is not null)
            {
                var door = roomManager.UniqueDoor.transform;
                roomManager.SetRoomTransform(
                    newOrigin: new Vector3(door.position.x, 0, door.position.z), 
                    newForward: door.transform.forward);
            }
            else
            {
                roomManager.Room.gameObject.SetActive(false);
                throw new NotImplementedException($"{nameof(EndingRoomTransformer)} only supports a room with a single door");
            }
        }
    }
}