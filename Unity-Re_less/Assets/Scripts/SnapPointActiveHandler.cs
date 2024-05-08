using Oculus.Interaction;
using UnityEngine;
using static Reless.Chapter;

namespace Reless
{
    public class SnapPointActiveHandler : MonoBehaviour
    {
        [SerializeField]
        private Chapter chapter;
    
        private SnapInteractable _snapInteractable;
    
        void Start()
        {
            _snapInteractable = GetComponentInChildren<SnapInteractable>();
        }

        private void Update()
        {
            // 현재 챕터와 일치하는 경우에만 SnapInteractable을 활성화합니다.
            _snapInteractable.enabled = GameManager.CurrentChapter switch
            {
                Chapter1 => chapter == Chapter1,
                Chapter2 => chapter == Chapter2,
                Chapter3 => chapter == Chapter3,
                _ => false
            };
        }
    }
}
