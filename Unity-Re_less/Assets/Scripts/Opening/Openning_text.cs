using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Openning_text : MonoBehaviour
{
    public string[] texts; // �ؽ�Ʈ ���
    public float changeInterval = 3f; // �ؽ�Ʈ ���� ���� (��)
    private int currentIndex = 0;
    private Text textComponent;

    void Start()
    {
        textComponent = GetComponentInChildren<Text>();
        StartCoroutine(ChangeTextRoutine());
    }

    IEnumerator ChangeTextRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);
            currentIndex = (currentIndex + 1) % texts.Length;
            textComponent.text = texts[currentIndex];
        }
    }
}