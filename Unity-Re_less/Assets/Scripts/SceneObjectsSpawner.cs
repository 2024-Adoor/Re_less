using System;
using System.Linq;
using Meta.XR.MRUtilityKit;
using UnityEngine;

namespace Reless
{
    /// <summary>
    /// MRUtilityKit을 사용하여 공간에 필요한 오브젝트의 생성을 담당합니다.
    /// </summary>
    public class SceneObjectsSpawner : MonoBehaviour
    {
        /// <summary>
        /// 방 바깥에 생성되는 프리팹의 그룹
        /// </summary>
        [Serializable]
        private struct OuterPrefabs
        {
            /// <summary>
            /// 문 바깥의 바닥에 생성될 프리팹
            /// </summary>
            [SerializeField]
            public GameObject floorNextToDoor;
            
            /// <summary>
            /// 문이 있는 벽의 바닥에 생성될 프리팹
            /// </summary>
            [SerializeField]
            public GameObject floorNextToWall;
            
            /// <summary>
            /// 벽에 생성될 프리팹
            /// </summary>
            [SerializeField]
            public GameObject wall;
            
            /// <summary>
            /// 문의 왼쪽에 생성될 로고의 왼쪽 부분 프리팹
            /// </summary>
            [SerializeField]
            public GameObject logoLeft;
            
            /// <summary>
            /// 문의 오른쪽에 생성될 로고의 오른쪽 부분 프리팹
            /// </summary>
            [SerializeField]
            public GameObject logoRight;
        }
        
        [SerializeField]
        private OuterPrefabs outerPrefabs;
        
        public void SpawnOuterPrefabs()
        {
            var anchors = MRUK.Instance.GetAnchors();

            foreach (MRUKAnchor door in anchors.Where(anchor =>
                         anchor.GetLabelsAsEnum() == MRUKAnchor.SceneLabels.DOOR_FRAME))
            {
                MRUKAnchor doorContainingWall = door.ParentAnchor;
                
                var floorNextDoor = Spawn(outerPrefabs.floorNextToDoor, door);
                TranslateToBottom(floorNextDoor, door);
                ScaleX(floorNextDoor, door);
                var floorNextToWall = Spawn(outerPrefabs.floorNextToWall, doorContainingWall);
                TranslateToBottom(floorNextToWall, doorContainingWall);
                ScaleX(floorNextToWall, doorContainingWall);
                
            }
        }
        
        private static GameObject Spawn(GameObject prefab, MRUKAnchor anchor)
        {
            var spawned = Instantiate(prefab, anchor.transform);
            spawned.name = prefab.name;
            return spawned;
        }
        
        private void TranslateToBottom(GameObject obj, MRUKAnchor anchor)
        {
            obj.transform.Translate(0, -(anchor.GetAnchorSize().y / 2), 0);
        }
            
        private void ScaleX(GameObject obj, MRUKAnchor anchor)
        {
            float scaleFactor = anchor.GetAnchorSize().x / obj.GetComponent<MeshFilter>().mesh.bounds.size.x;
            obj.transform.localScale = new Vector3(scaleFactor, 1, 1);
        }
    }
}


