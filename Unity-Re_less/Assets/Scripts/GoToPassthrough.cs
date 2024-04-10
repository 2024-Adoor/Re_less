using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reless;

public class GoToPassthrough : MonoBehaviour
{
	public int chapterNum;	
	
	public GameObject Chat_Clock;
	public GameObject Chat_Cat;
	public GameObject Chat_Cactus;

	bool Ch01_Clear = false;
	bool Ch02_Clear = false;
	bool Ch03_Clear = false;
	
	/// <summary>
	/// ExitRoom 씬을 이미 로드했다면 추가로 로드하지 않도록 확인합니다.
	/// </summary>
	private bool _loadExitRoomExecuted = false;

	// 페이드 UI
	private FadeUI fadeUI;

	void Start()
	{
		Renderer renderer = GetComponent<Renderer>();
		renderer.enabled = false;

		GameObject fadeUIObject = GameObject.Find("FadeUI");

        // FadeUI 스크립트를 가져옵니다.
        fadeUI = fadeUIObject.GetComponent<FadeUI>();
	}

	void Update()
	{
		if(chapterNum == 1)
		{
			Renderer rend = Chat_Clock.GetComponent<Renderer>();

			// 챕터 1 클리어 조건 : 시계토끼 대사 끝 (=머테리얼 이름이 Clock_7)
			if(rend.material.name.Contains("Clock_7"))
			{
				GetComponent<Renderer>().enabled = true;
				Ch01_Clear = true;
			}
		}
		if(chapterNum == 2)
		{
			// 챕터 1 클리어 조건 : 체셔캣 && 모자장수 대사 끝 (=머테리얼 이름이 Cat_5 / Cactus_5)
			Renderer rend1 = Chat_Cat.GetComponent<Renderer>();
			Renderer rend2 = Chat_Cactus.GetComponent<Renderer>();

			if(rend1.material.name.Contains("Cat_5") && rend2.material.name.Contains("Cactus_5"))
			{
				GetComponent<Renderer>().enabled = true;
				Ch02_Clear = true;
			}
		}
		if(chapterNum == 3)
		{
			// 챕터 3 클리어 조건 : 방에서 커져서 문을 열고 트리거에 도달했는가 = 트리거에 도달만 하면 됨
			GetComponent<Renderer>().enabled = true;
			Ch03_Clear = true;
		}
		
		bool loadExitRoomScene = Ch01_Clear || Ch02_Clear || Ch03_Clear;

		if (loadExitRoomScene && !_loadExitRoomExecuted)
		{
			GameManager.Instance.LoadExitDreamScene();
			_loadExitRoomExecuted = true;
		}
	}

	// GameManager.Instance.LoadExitDreamScene();

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if(Ch01_Clear || Ch02_Clear || Ch03_Clear)
			{
				fadeUI.WhiteFadeOut();
				GameManager.Instance.LoadMainScene();
			}
		}
	}
}
