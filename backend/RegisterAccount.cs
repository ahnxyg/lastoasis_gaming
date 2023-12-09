using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class RegisterAccount : LoginBase
{
	[SerializeField]
	private Image imageID;              // ID 필드 색상 변경
	[SerializeField]
	private TMP_InputField inputFieldID;            // ID 필드 텍스트 정보 추출
	[SerializeField]
	private Image imagePW;              // PW 필드 색상 변경
	[SerializeField]
	private TMP_InputField inputFieldPW;            // PW 필드 텍스트 정보 추출
	[SerializeField]
	private Image imageConfirmPW;           // Confirm PW 필드 색상 변경
	[SerializeField]
	private TMP_InputField inputFieldConfirmPW; // Confirm PW 필드 텍스트 정보 추출
	[SerializeField]
	private Image imageEmail;               // E-mail 필드 색상 변경
	[SerializeField]
	private TMP_InputField inputFieldEmail;     // E-mail 필드 텍스트 정보 추출

	[SerializeField]
	private Button btnRegisterAccount;      // "계정 생성" 버튼 (상호작용 가능/불가능)

	/// <summary>
	/// "계정 생성" 버튼을 눌렀을 때 호출
	/// </summary>
	public void OnClickRegisterAccount()
	{
		// 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
		ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);

		// 필드 값이 비어있는지 체크
		if (IsFieldDataEmpty(imageID, inputFieldID.text, "ID")) return;
		if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "PW")) return;
		if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "check PW")) return;
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "email address")) return;

		// 비밀번호와 비밀번호 확인의 내용이 다를 때
		if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
		{
			GuideForIncorrectlyEnteredData(imageConfirmPW, "wrong PW.");
			return;
		}

		// 메일 형식 검사
		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncorrectlyEnteredData(imageEmail, "wrong email address.(ex. address@xx.xx)");
			return;
		}

		// "계정 생성" 버튼의 상호작용 비활성화
		btnRegisterAccount.interactable = false;
		SetMessage("account create...");

		// 뒤끝 서버 계정 생성 시도
		CustomSignUp();
	}

	/// <summary>
	/// 계정 생성 시도 후 서버로부터 전달받은 message를 기반으로 로직 처리
	/// </summary>
	private void CustomSignUp()
	{
		Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
		{
			// "계정 생성" 버튼 상호작용 활성화
			btnRegisterAccount.interactable = true;

			// 계정 생성 성공
			if (callback.IsSuccess())
			{
				// E-mail 정보 업데이트
				Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
				{
					if (callback.IsSuccess())
					{
						SetMessage($"success. {inputFieldID.text} welcome.");

						// 계정 생성에 성공했을 때 해당 계정의 게임 정보 생성
						BackendGameData.Instance.GameDataInsert();


						// Lobby 씬으로 이동
						Utils.LoadScene(SceneNames.GameStart);
					}
				});
			}
			// 계정 생성 실패
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 409:   // 중복된 customId 가 존재하는 경우
						message = "already esixt ID.";
						break;
					case 403:   // 차단당한 디바이스일 경우
						message = callback.GetMessage();
						break;
					case 401:   // 프로젝트 상태가 '점검'일 경우
					case 400:   // 디바이스 정보가 null일 경우
					default:
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("ID"))
				{
					GuideForIncorrectlyEnteredData(imageID, message);
				}
				else
				{
					SetMessage(message);
				}
			}
		});
	}
}

