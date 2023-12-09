using System;
using UnityEngine;
using BackEnd;
using UnityEngine.Events;

public class BackendGameData
{
	[System.Serializable]
	public class GameDataLoadEvent : /*UnityEngine.Events.*/UnityEvent { }
	public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

	private static BackendGameData instance = null;
	public static BackendGameData Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new BackendGameData();
			}

			return instance;
		}
	}

	private UserGameData userGameData = new UserGameData();
	public UserGameData UserGameData => userGameData;

	private string gameDataRowInDate = string.Empty;

	/// <summary>
	/// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가
	/// </summary>
	public void GameDataInsert()
	{
		// 유저 정보를 초기값으로 설정
		userGameData.Reset();

		// 테이블에 추가할 데이터로 가공
		Param param = new Param()
		{
			{ "level",      userGameData.level },
			{ "experience", userGameData.experience },
			{ "heart",      userGameData.heart },
			{ "gold",      userGameData.gold },
            { "t2Unlocked", userGameData.t2Unlocked },
            { "t3Unlocked", userGameData.t3Unlocked }
        };

		// 첫 번째 매개변수는 뒤끝 콘솔의 "게임 정보 관리" 탭에 생성한 테이블 이름
		Backend.GameData.Insert("USER_DATA", param, callback =>
		{
			// 게임 정보 추가에 성공했을 때
			if (callback.IsSuccess())
			{
				// 게임 정보의 고유값
				gameDataRowInDate = callback.GetInDate();

				Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {callback}");
			}
			// 실패했을 때
			else
			{
				Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다. : {callback}");
			}
		});
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에서 유저 정보를 불러올 때 호출
	/// </summary>
	public void GameDataLoad()
	{
		Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
		{
			// 게임 정보 불러오기에 성공했을 때
			if (callback.IsSuccess())
			{
				Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {callback}");

				// JSON 데이터 파싱 성공
				try
				{
					LitJson.JsonData gameDataJson = callback.FlattenRows();

					// 받아온 데이터의 개수가 0이면 데이터가 없는 것
					if (gameDataJson.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						// 불러온 게임 정보의 고유값
						gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
						// 불러온 게임 정보를 userData 변수에 저장
						userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
						userGameData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
						userGameData.heart = int.Parse(gameDataJson[0]["heart"].ToString());
						userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
						userGameData.t2Unlocked = bool.Parse(gameDataJson[0]["t2Unlocked"].ToString());
                        userGameData.t3Unlocked = bool.Parse(gameDataJson[0]["t3Unlocked"].ToString());
                        onGameDataLoadEvent?.Invoke();
					}
				}
				// JSON 데이터 파싱 실패
				catch (System.Exception e)
				{
					// 유저 정보를 초기값으로 설정
					userGameData.Reset();
					// try-catch 에러 출력
					Debug.LogError(e);
				}
			}
			// 실패했을 때
			else
			{
				Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다. : {callback}");
			}
		});
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에 있는 유저 데이터 갱신
	/// </summary>
	public void GameDataUpdate(UnityAction action = null)
	{
		if (userGameData == null)
		{
			Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
						   "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
			return;
		}

		Param param = new Param()
		{
			{ "level",      userGameData.level },
			{ "experience", userGameData.experience },
			{ "heart",      userGameData.heart },
			{ "gold",      userGameData.gold },
			{ "t2Unlocked", userGameData.t2Unlocked },
			{ "t3Unlocked", userGameData.t3Unlocked }
		};

		// 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
		if (string.IsNullOrEmpty(gameDataRowInDate))
		{
			Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
		}
		// 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
		// 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
		else
		{
			Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

			Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

					action?.Invoke();
				}
				else
				{
					Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
				}
			});
		}
	}


    public void UpdateUserRanking(Action callback = null)
	{
		try
		{
			Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
			{
				if (callback.IsSuccess())
				{
					var rows = callback.FlattenRows();
					if (rows.Count > 0)
					{
						string userInDate = rows[0]["inDate"].ToString();
						int userLevel = int.Parse(rows[0]["level"].ToString()); // level 칼럼을 사용
						Debug.Log(userLevel + "= 레벨\n");
						Debug.Log(userInDate + "= inDate\n");
						UpdateRankingWithLevel(userInDate, userLevel);
					}
					else
					{
						Debug.LogError("유저 데이터가 존재하지 않습니다.");
					}
				}
				else
				{
					Debug.LogError("데이터 조회 실패: " + callback);
				}
			});
		}
		catch (System.Exception ex)
		{
			Debug.LogError("UpdateUserRanking 예외 발생: " + ex.Message);
		}
		callback?.Invoke();
	}

	// 랭킹에 레벨을 업데이트하는 메서드 추가
	private void UpdateRankingWithLevel(string userInDate, int level)
	{
		try
		{
			Param param = new Param();
			param.Add("level", level);

			Backend.URank.User.UpdateUserScore("754cf570-9426-11ee-8cb0-55a457ec4ebc", "USER_DATA", userInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log("랭킹 업데이트 성공");
				}
				else
				{
					Debug.LogError("랭킹 업데이트 실패: " + callback);
				}
			});
		}
		catch (System.Exception ex)
		{
			Debug.LogError("UpdateRankingWithLevel 예외 발생: " + ex.Message);
		}
	}
}
