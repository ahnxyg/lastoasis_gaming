using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AccountManager : MonoBehaviour
{
	[SerializeField] InputField idInput;
	[SerializeField] InputField passwordInput;
	[SerializeField] InputField infoInput;

	[SerializeField] string url;

	public void LoginClick() => StartCoroutine(AccountCo("login"));

	public void RegisterClick() => StartCoroutine(AccountCo("register"));

	public void SaveClick() => StartCoroutine(AccountCo("save"));

	IEnumerator AccountCo(string command) 
	{
		WWWForm form = new WWWForm();
		form.AddField("command", command);
		form.AddField("id", idInput.text);
		form.AddField("password", passwordInput.text);
		form.AddField("info", infoInput.text);

		UnityWebRequest www = UnityWebRequest.Post(url, form);

		yield return www.SendWebRequest();
		print(www.downloadHandler.text);
	}
}