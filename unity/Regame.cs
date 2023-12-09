using UnityEngine.SceneManagement;
using UnityEngine;

public class Regame : MonoBehaviour
{
    public static Regame instance = null;

    public int resetCount = 0;

    private const string ResetKey = "ResetCount";
    public void Awake()
    {
        instance = this;
        resetCount = PlayerPrefs.GetInt(ResetKey, 0);
    }

    public void ResetAll()
    {
        resetCount = 1;
        PlayerPrefs.SetInt(ResetKey, resetCount);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("SceneType1");
    }
}