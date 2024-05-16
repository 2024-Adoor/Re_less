using System.Collections;
using Reless.UI;
using UnityEngine;

namespace Reless.Ending
{
    public class EndingMessage : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            GuideText.SetText("팝업북이 사라졌어요. 대신 다른 것이 보이네요.", duration: 4f);
            yield return new WaitForSeconds(1f);
            GuideText.SetText("이 사진들은 뭘까요?", duration: 3f);
            
            // 7초가 지나도 플레이어가 방을 나가지 않았다면 안내 메시지를 띄웁니다.
            yield return new WaitForSeconds(7f);
            if (FindAnyObjectByType<ExitRoom>().IsPlayerExited is false)
            {
                GuideText.SetText("빌런이가 문 밖을 가리키고 있어요. 밖으로 나가볼까요?");
            }
        }
    }
}