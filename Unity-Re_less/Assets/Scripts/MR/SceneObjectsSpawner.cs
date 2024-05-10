using System;
using System.Collections.Generic;
using System.Linq;
using Meta.XR.MRUtilityKit;
using Reless.Game;
using UnityEngine;
using static Meta.XR.MRUtilityKit.MRUKAnchor.SceneLabels;
using Logger = Reless.Debug.Logger;

namespace Reless.MR
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

        [SerializeField]
        private GameObject popupBookPrefab;

        [SerializeField]
        private List<GameObject> wallHintPrefabs;
        
        /// <summary>
        /// RoomManager 레퍼런스
        /// </summary>
        private RoomManager RoomManager { get; set; }

        private void Awake()
        {
            if (RoomManager.Instance is null)
            {
                Logger.LogWarning("RoomManager is not loaded yet.");
                RoomManager.OnMRUKSceneLoaded += () => RoomManager = RoomManager.Instance;
                RoomManager.OnMRUKSceneLoaded += SpawnAll;
            }
            else
            {
                Logger.Log("RoomManager is already loaded.");
                RoomManager = RoomManager.Instance;
                SpawnAll();
            }
        }
        
        public void SpawnAll()
        {
            Logger.Log("Spawning all scene objects.");
            SpawnOuterPrefabs();
            SpawnPopupBook();
            SpawnWallHint();
        }
        
        private void SpawnOuterPrefabs()
        {
            var anchors = MRUK.Instance.GetCurrentRoom().Anchors;

            foreach (MRUKAnchor door in anchors.Where(anchor => anchor.GetLabelsAsEnum() == DOOR_FRAME))
            {
                MRUKAnchor wallWithDoor = door.ParentAnchor ? door.ParentAnchor : FindClosestWall(door);
                
                var wallPosition = wallWithDoor.transform.position;
                
                var floorNextDoor = Spawn(outerPrefabs.floorNextToDoor, door);
                var floorNextToWall = Spawn(outerPrefabs.floorNextToWall, wallWithDoor);

                TranslateToBottom(floorNextDoor, door);
                TranslateToBottom(floorNextToWall, wallWithDoor);

                floorNextDoor.transform.Translate(0, 0.01f, 0);
                
                ScaleX(floorNextDoor, door);
                ScaleX(floorNextToWall, wallWithDoor);

                GameObject wall; 
                {
                    var go = new GameObject("Outer Wall");
                    wall = Spawn(go, wallWithDoor);
                    Destroy(go);
                }
                
                var doorBound = door.PlaneBoundary2D;
                var wallBound = wallWithDoor.PlaneBoundary2D;
                
                var doorCenter = door.GetAnchorCenter();
                var wallCenter = wallWithDoor.GetAnchorCenter();
                var wallRotation = wallWithDoor.transform.localRotation.eulerAngles.y;

                var doorOffsetX = (wallCenter.x - doorCenter.x) * Mathf.Cos(wallRotation * Mathf.Deg2Rad) - (wallCenter.z - doorCenter.z) * Mathf.Sin(wallRotation * Mathf.Deg2Rad);
                var doorOffsetY = wallCenter.y - doorCenter.y;
                    
                    var mesh = new Mesh
                    {
                        vertices = new Vector3[]
                        {
                            new(doorBound[0].x - doorOffsetX, doorBound[0].y - doorOffsetY, 0),
                            new(doorBound[1].x - doorOffsetX, doorBound[1].y - doorOffsetY, 0),
                            new(doorBound[2].x - doorOffsetX, doorBound[2].y - doorOffsetY, 0),
                            new(doorBound[3].x - doorOffsetX, doorBound[3].y - doorOffsetY, 0),
                            new(wallBound[0].x, wallBound[0].y, 0),
                            new(wallBound[1].x, wallBound[1].y, 0),
                            new(wallBound[2].x, wallBound[2].y, 0),
                            new(wallBound[3].x, wallBound[3].y, 0),
                        },
                        triangles = new[]
                        {
                            7, 3, 0,
                            7, 0, 4,
                            0, 5, 4,
                            0, 1, 5,
                            1, 6, 5,
                            1, 2, 6,
                            2, 7, 6,
                            2, 3, 7,
                        },
                    };
                    mesh.RecalculateNormals();
                    wall.AddComponent<MeshFilter>().mesh = mesh;
                    wall.AddComponent<MeshRenderer>().material = outerPrefabs.WallMaterial;
                wall.transform.position = wallPosition;
            }
            
            // 문의 부모 앵커가 없는 경우를 위해 가장 가까운 벽을 찾습니다.
            MRUKAnchor FindClosestWall(MRUKAnchor door) => anchors
                .Where(anchor => anchor.GetLabelsAsEnum() == WALL_FACE)
                .OrderBy(wall => Vector3.Distance(wall.GetAnchorCenter(), door.GetAnchorCenter()))
                .First();
        }
        
        /// <summary>
        /// 팝업북을 스폰합니다.
        /// </summary>
        private void SpawnPopupBook()
        {
            var anchors = MRUK.Instance.GetCurrentRoom().Anchors;
            
            // 아무 테이블 하나에서 스폰
            var table = anchors.FirstOrDefault(anchor => anchor.GetLabelsAsEnum() == TABLE);
            if (table == null)
            {
                Logger.LogError("There is no table.");
                return;
            }
            
            var book = Spawn(popupBookPrefab, table);
            
            GameManager.Instance.PopupBook = book.GetComponent<PopupBook>();
            
            // 위치: 위
            TranslateToTop(book, table);
            
            book.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            book.transform.localPosition = Vector3.zero;
            book.transform.localRotation = Quaternion.Euler(0, -90, -90);
            book.transform.Rotate(Vector3.up, 90);
            
            // 처음 스폰 시 비활성화
            book.SetActive(false);
        }

        private void SpawnWallHint()
        {
            {
                var hint = Spawn(wallHintPrefabs[0], RoomManager.KeyWall);
                hint.transform.Rotate(Vector3.right, 90);
                hint.transform.Translate(0.75f, 0.05f, 0);
                GameManager.Instance.spawnedWallHints.Add(hint);
                hint.SetActive(false);
            }
            {
                var hint = Spawn(wallHintPrefabs[1], RoomManager.KeyWall);
                hint.transform.Rotate(Vector3.right, 90);
                hint.transform.Translate(0.25f, 0.05f, 0);
                GameManager.Instance.spawnedWallHints.Add(hint);
                hint.SetActive(false);

            }
            {
                var hint = Spawn(wallHintPrefabs[2], RoomManager.KeyWall);
                hint.transform.Rotate(Vector3.right, 90);
                hint.transform.Translate(-0.25f, 0.05f, 0);
                GameManager.Instance.spawnedWallHints.Add(hint);
                hint.SetActive(false);

            }
            {
                var hint = Spawn(wallHintPrefabs[3], RoomManager.KeyWall);
                hint.transform.Rotate(Vector3.right, 90);
                hint.transform.Translate(-0.75f, 0.05f, 0);
                GameManager.Instance.spawnedWallHints.Add(hint);
                hint.SetActive(false);

            }
        }
        
        private static GameObject Spawn(GameObject prefab, MRUKAnchor anchor)
        {
            var spawned = Instantiate(prefab, anchor.transform);
            spawned.name = $"{prefab.name} (Spawned)";
            return spawned;
        }
        
        private void TranslateToBottom(GameObject obj, MRUKAnchor anchor)
        {
            obj.transform.Translate(0, -((anchor.PlaneRect?.height ?? 0) / 2), 0);
        }
        
        private void TranslateToTop(GameObject obj, MRUKAnchor anchor)
        {
            obj.transform.Translate(0, (anchor.PlaneRect?.height ?? 0) / 2, 0);
        }
            
        private void ScaleX(GameObject obj, MRUKAnchor anchor)
        {
            float scaleFactor = (anchor.PlaneRect?.width ?? 1) / obj.GetComponent<MeshFilter>().mesh.bounds.size.x;
            obj.transform.localScale = new Vector3(scaleFactor, 1, 1);
        }
    }
}


