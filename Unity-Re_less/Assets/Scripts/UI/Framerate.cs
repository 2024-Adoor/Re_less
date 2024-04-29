using TMPro;
using UnityEngine;

namespace Reless.UI
{
    public class Framerate : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        private float _timer;
        
        private void Update()
        {
            _timer += Time.unscaledDeltaTime;
            if (_timer < 0.5f) return;
            _timer = 0f;
            
            label.text = $"FPS: {1 / Time.unscaledDeltaTime}";
        }
    }
}
