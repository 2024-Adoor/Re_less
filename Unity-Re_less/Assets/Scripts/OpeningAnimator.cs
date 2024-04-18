using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class OpeningAnimator : MonoBehaviour
{
    /// <summary>
    /// 오프닝 애니메이션의 각 장면들
    /// </summary>
    public List<GameObject> scenes;

    private void Awake()
    {
        foreach (var scene in scenes) { Assert.IsNotNull(scene); }
    }

    // Start is called before the first frame update
    public void EnableScene(int index)
    {
        if (index < 0 || index >= scenes.Count)
        {
            Debug.LogError("Invalid index");
            return;
        }
        
        scenes[index].SetActive(true);
    }
    
    public void DisableScene(int index)
    {
        if (index < 0 || index >= scenes.Count)
        {
            Debug.LogError("Invalid index");
            return;
        }
        
        scenes[index].SetActive(false);
    }
}
