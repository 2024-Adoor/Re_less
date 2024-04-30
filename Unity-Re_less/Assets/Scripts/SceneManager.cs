using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reless
{
    /// <summary>
    /// 게임의 씬을 관리하는 래핑 클래스입니다.
    /// </summary>
    public static class SceneManager
    {
        public static AsyncOperation LoadAsync(BuildScene scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)scene, mode);
        }
        
        public static AsyncOperation UnloadAsync(BuildScene scene)
        {
            return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)scene);
        }
        
        public static UnityEngine.SceneManagement.Scene GetScene(BuildScene scene)
        {
            return UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex((int)scene);
        }
        
        public static BuildScene AsBuildScene(this UnityEngine.SceneManagement.Scene scene)
        {
            return (BuildScene)scene.buildIndex;
        }
        
        /// <summary>
        /// 현재 활성화된 씬
        /// </summary>
        public static BuildScene ActiveScene => (BuildScene)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }
}