using System;
using System.Collections;
using System.Collections.Generic;
using Reless.MR;
using UnityEngine;

namespace Reless
{
    /// <summary>
    /// 그려서 얻은 오브젝트
    /// </summary>
    public class ObtainingObject : MonoBehaviour
    {
        [SerializeField] [Range(1, 3)]
        private int chapter;

        private readonly WaitForSeconds _respawnCheckInterval = new(2f);
        
        private Coroutine _respawnCheckCoroutine;
        
        private void Start()
        {
            transform.localScale = Vector3.one * 5;
            
            _respawnCheckCoroutine = StartCoroutine(CheckRespawnNeeded(
                respawnCondition: () => RoomManager.Instance.Room.IsPositionInRoom(transform.position) is false, 
                onNeeded: () => Respawn(GameManager.Instance.PlayerPosition)));
        }

        private void OnDestroy()
        {
            StopCoroutine(_respawnCheckCoroutine);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        private IEnumerator CheckRespawnNeeded(Func<bool> respawnCondition, Action onNeeded)
        {
            while (true)
            {
                yield return _respawnCheckInterval;
                
                if (respawnCondition())
                {
                    onNeeded();
                }
            }
        }
        
        public void Respawn(Vector3 position)
        {
            transform.position = position;
        }
        
        public void Snapped()
        {
            Debug.Log($"Snapped: {gameObject.name}");

            transform.localScale = Vector3.one;
            
            MainBehaviour mainBehaviour = FindObjectOfType<MainBehaviour>();
            mainBehaviour.AchieveEnterCondition();
        }
    }
}


