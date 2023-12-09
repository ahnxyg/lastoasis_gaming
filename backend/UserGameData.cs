[System.Serializable]
public class UserGameData
{
	public int level;           // Lobby Scene에 보이는 플레이어 레벨
	public float experience;        // Lobby Scene에 보이는 플레이어 경험치
	public int heart;           // 게임 플레이에 소모되는 재화
	public int gold;
	public bool t2Unlocked;
	public bool t3Unlocked;


	public void Reset()
	{
		level = 1;
		experience = 0;
		heart = 30;
		gold = 0;
		t2Unlocked = false;
		t3Unlocked = false;
	}
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
*/