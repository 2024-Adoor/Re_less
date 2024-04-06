using System;
using System.Linq;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;
using static Meta.XR.MRUtilityKit.MRUKAnchor.SceneLabels;

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
            /// 문이 있는 벽 바깥의 재질
            /// </summary>
            [SerializeField]
            public Material WallMaterial;
            
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

            foreach (MRUKAnchor door in anchors.Where(anchor => anchor.GetLabelsAsEnum() == DOOR_FRAME))
            {
                MRUKAnchor wallWithDoor = door.ParentAnchor ? door.ParentAnchor : FindClosestWall(door);
                
                var wallPosition = wallWithDoor.transform.position;
                
                var floorNextDoor = Spawn(outerPrefabs.floorNextToDoor, door);
                TranslateToBottom(floorNextDoor, door);
                ScaleX(floorNextDoor, door);
                
                var floorNextToWall = Spawn(outerPrefabs.floorNextToWall, wallWithDoor);
                TranslateToBottom(floorNextToWall, wallWithDoor);
                ScaleX(floorNextToWall, wallWithDoor);
                
                var doorBound = door.PlaneBoundary2D;
                var wallBound = wallWithDoor.PlaneBoundary2D;
                var doorWallDifferentX = door.GetAnchorCenter().z - wallWithDoor.GetAnchorCenter().z;
                var doorWallDifferentY = wallWithDoor.GetAnchorCenter().y - door.GetAnchorCenter().y;
                
                var go = new GameObject("Outer Wall");
                var wall = Spawn(go, wallWithDoor);
                Destroy(go);
                {
                    var mesh = new Mesh
                    {
                        vertices = new Vector3[]
                        {
                            new(wallBound[1].x, wallBound[1].y, 0), // wall left bottom
                            new(doorBound[1].x - doorWallDifferentX, doorBound[1].y - doorWallDifferentY, 0), // door left bottom
                            new(doorBound[2].x - doorWallDifferentX, doorBound[2].y - doorWallDifferentY, 0), // door left top
                            new(doorBound[3].x - doorWallDifferentX, doorBound[3].y - doorWallDifferentY, 0), // door right top
                            new(doorBound[0].x - doorWallDifferentX, doorBound[0].y - doorWallDifferentY, 0), // door right bottom
                            new(wallBound[0].x, wallBound[0].y, 0), // wall right bottom
                            new(wallBound[3].x, wallBound[3].y, 0), // wall right top
                            new(wallBound[2].x, wallBound[2].y, 0), // wall left top
                        },
                        triangles = new[]
                        {
                            0, 1, 2,
                            0, 2, 7,
                            2, 6, 7,
                            2, 3, 6,
                            3, 5, 6,
                            3, 4, 5,
                        },
                    };

                    mesh.RecalculateNormals();
                    wall.AddComponent<MeshFilter>().mesh = mesh;
                    wall.AddComponent<MeshRenderer>().material = outerPrefabs.WallMaterial;
                }
                wall.transform.position = wallPosition;
                
            }
            
            // 문의 부모 앵커가 없는 경우를 위해 가장 가까운 벽을 찾습니다.
            MRUKAnchor FindClosestWall(MRUKAnchor door) => anchors
                .Where(anchor => anchor.GetLabelsAsEnum() == WALL_FACE)
                .OrderBy(wall => Vector3.Distance(wall.GetAnchorCenter(), door.GetAnchorCenter()))
                .First();
        }
        
        private static GameObject Spawn(GameObject prefab, MRUKAnchor anchor)
        {
            var spawned = Instantiate(prefab, anchor.transform);
            spawned.name = $"{prefab.name} (Spawned)";
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


