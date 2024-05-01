using System.Linq;
using UnityEngine;

namespace Reless
{
    /// <summary>
    /// Additive Load 시 로드되면 안 되는 오브젝트를 제거합니다.
    /// </summary>
    public class ExcludeOnAdditiveLoad : MonoBehaviour
    {
        private void Awake()
        {
            // 활성 씬이 오브젝트가 속한 씬이 아니라면 Additive로 로드된 것으로 간주
            if (SceneManager.ActiveScene != gameObject.scene.AsBuildScene())
            {
                foreach (var excludeObject in gameObject.scene.GetRootGameObjects().Where(go => go.CompareTag("ExcludeOnAdditiveLoad")))
                {
                    Debug.Log($"{nameof(ExcludeOnAdditiveLoad)}: Destroy {excludeObject.name}");
                    Destroy(excludeObject);
                }
                
                Destroy(this.gameObject);
            }
        }
    }
}
