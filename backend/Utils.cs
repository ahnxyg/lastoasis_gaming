using UnityEngine.SceneManagement;

public enum SceneNames { Logo=0,Login,GameStart,}

public class Utils
{
    public static string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;

    }

    public static void LoadScene(string SceneName = "")
    {
        if (SceneName == "")
        {
            SceneManager.LoadScene(GetActiveScene());

        }
        else
        {
            SceneManager.LoadScene(SceneName);
        }

    }
    public static void LoadScene(SceneNames sceneName)
    {
        //SceneName 열거형으로 받아온 경우ToString()처리
        SceneManager.LoadScene(sceneName.ToString());
    }

}
