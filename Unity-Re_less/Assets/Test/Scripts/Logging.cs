using UnityEngine;

namespace Test
{
    [CreateAssetMenu(fileName = "Logging", menuName = "Scriptable Object/Logging", order = int.MaxValue)]
    public class Logging : ScriptableObject
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }
    }
}
