using System;
using UnityEngine;

namespace Test
{
    /// <summary>
    /// 테스트 시각화 목적으로 Material의 색상을 변경하는 클래스입니다.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class ChangeMaterialColor : MonoBehaviour
    {
        [SerializeField]
        private Color materialColor;

        // Renderer 컴포넌트 참조
        private Renderer _renderer;

        void Start()
        {
            // Renderer 컴포넌트 가져오기
            _renderer = GetComponent<Renderer>();
            _renderer.material.color = materialColor;
        }

        private void OnValidate()
        {
            if (Application.isPlaying && _renderer != null)
            {
                _renderer.material.color = materialColor;
            }
        }
    }
}

