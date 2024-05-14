using UnityEngine;
using Logger = Reless.Debug.Logger;

namespace Reless.Util
{
    /// <summary>
    /// Additive Load 시 로드되면 안 되는 오브젝트에 부착되어 Additive Load 시 자신을 제거합니다.
    /// </summary>
    /// <remarks>
    /// 이 스크립트의 실행 우선순위는 다른 스크립트보다 앞으로 설정되어야 합니다.
    /// </remarks>
    public class ExcludeOnAdditiveLoad : MonoBehaviour
    {
        private void OnEnable()
        {
            // 오브젝트가 속한 씬이 활성 씬이 아니라는 것은 Additive로 로드된 씬이라는 것을 의미합니다.
            if (SceneManager.ActiveScene == this.gameObject.scene.AsBuildScene()) 
                return;
            
            Logger.Log($"{nameof(ExcludeOnAdditiveLoad)}: Destroy <b>{this.name}</b>", this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
