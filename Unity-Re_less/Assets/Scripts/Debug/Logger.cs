using UnityEngine;
using Conditional = System.Diagnostics.ConditionalAttribute;

namespace Reless.Debug
{
    /// <summary>
    /// 디버그 모드에서만 로그를 출력하는 래핑 클래스입니다.
    /// </summary>
    /// <remarks>
    /// 씬과 무관하게 UnityEvent에서도 사용할 수 있도록 ScriptableObject를 이용합니다.
    /// </remarks>
    [CreateAssetMenu(fileName = "Logger", menuName = "Scriptable Object/Logger", order = int.MaxValue)]
    public class Logger : ScriptableObject
    {
        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void Log(string message) => UnityEngine.Debug.Log(message);
        
        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void Log(object message) => UnityEngine.Debug.Log(message);
        
        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void Log(string message, Object context) => UnityEngine.Debug.Log(message, context);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogWarning(string message) => UnityEngine.Debug.LogWarning(message);
        
        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogWarning(object message) => UnityEngine.Debug.LogWarning(message);
        
        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogWarning(string message, Object context) => UnityEngine.Debug.LogWarning(message, context);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogError(string message) => UnityEngine.Debug.LogError(message);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogError(object message) => UnityEngine.Debug.LogError(message);
        
        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogError(string message, Object context) => UnityEngine.Debug.LogError(message, context);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogException(System.Exception exception) => UnityEngine.Debug.LogException(exception);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogFormat(string format, params object[] args) => UnityEngine.Debug.LogFormat(format, args);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogWarningFormat(string format, params object[] args) => UnityEngine.Debug.LogWarningFormat(format, args);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void LogErrorFormat(string format, params object[] args) => UnityEngine.Debug.LogErrorFormat(format, args);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message) => UnityEngine.Debug.Assert(condition, message);

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEBUG")]
        public static void AssertFormat(bool condition, string format, params object[] args) => UnityEngine.Debug.AssertFormat(condition, format, args);
    }
}
