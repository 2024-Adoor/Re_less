using NaughtyAttributes;
using UnityEngine;

namespace Reless.Game
{
    /// <summary>
    /// 플레이어가 그려야 하는 밑그림 오브젝트 자식의 콜라이더가 있는 오브젝트들에 부착되어 플레이어가 그린 선이 콜라이더에 있는지 확인합니다.
    /// </summary>
    public class DrawingChecker : MonoBehaviour
    {
        /// <summary>
        /// Pen의 레퍼런스
        /// </summary>
        public Pen Pen
        {
            set => _pen ??= value; 
        }
        private Pen _pen;

        /// <summary>
        /// 닿고 있는 플레이어가 그린 선의 콜라이더 개수
        /// </summary>
        [ShowNonSerializedField, ReadOnly]
        private int _triggeredColliders;
    
        /// <summary>
        /// 닿고 있는 플레이어가 그린 선의 콜라이더가 하나 이상 있으면 true입니다.
        /// </summary>
        [ShowNativeProperty]
        public bool Checked => _triggeredColliders > 0;
    
        /// <summary>
        /// 콜라이더가 비교하고자 하는 콜라이더인 플레이어가 그린 선의 콜라이더인지 확인합니다.
        /// </summary>
        private bool IsDrawingMeshCollider(Collider collider)
            => collider is MeshCollider otherMeshCollider && _pen.IsDrawingMeshCollider(otherMeshCollider);
    
        private void OnTriggerEnter(Collider other)
        {
            // 플레이어가 그린 선의 콜라이더가 아니라면 무시합니다.
            if (!IsDrawingMeshCollider(other)) return;
        
            // 닿고 있는 플레이어의 그림 콜라이더가 하나 추가되었다는 것을 기록합니다.
            _triggeredColliders++;
        }

        private void OnTriggerExit(Collider other)
        {
            // 플레이어가 그린 선의 콜라이더가 아니라면 무시합니다.
            if (!IsDrawingMeshCollider(other)) return;
        
            // 닿고 있는 플레이어의 그림 콜라이더가 하나 빠졌다는 것을 기록합니다.
            _triggeredColliders--;
        }
    }
}
