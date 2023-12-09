using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.MLAgents;

public class BlueRedAcademy : MonoBehaviour
{
    public static BlueRedAcademy instance = null;
    public static ActionsButton[] btn;

    private static int round = 0;
    public float frequency;
    private FPS timeControl;

    public BlueAgent blue;
    public RedAgent red;

    public int resetCount = 0;
    private bool needsReset = true; // 씬 재로드 필요 여부를 나타내는 플래그

    private const string ResetKey = "ResetCount";
    public void Awake()
    {
        instance = this;
        this.timeControl = GetComponent<FPS>();
        needsReset = true; // Awake에서 플래그 초기화
        resetCount = PlayerPrefs.GetInt(ResetKey, 0);
        BlueRedAcademy.round++;

        BlueActionSpace b_Btn = this.gameObject.AddComponent<BlueActionSpace>();
        RedActionSpace r_Btn = this.gameObject.AddComponent<RedActionSpace>();

        btn = new ActionsButton[2] { b_Btn, r_Btn };
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (this.blue.Terminal && this.red.Terminal && BlueRedAcademy.round > 0)
        {
            this.EndEpisode();
            float epsilon = Random.Range(0.0f, 1.0f);

            bool normalSpeed;
            if (epsilon < this.frequency)
            {
                normalSpeed = true;
            }
            else
            {
                normalSpeed = false;
            }
            this.ResetAll();
            this.timeControl.SetTimeScale(normalSpeed);
        }
    }

    public void ResetAll()
    {
        resetCount = 1;
        PlayerPrefs.SetInt(ResetKey, resetCount);
        PlayerPrefs.Save();
        if (needsReset)
        {
            needsReset = false; // 씬 재로드 후 플래그를 false로 설정
            UnityEngine.SceneManagement.SceneManager.LoadScene("SceneType1");
        }
    }

    void EndEpisode()
    {
        this.blue.EndEpisode();
        this.red.EndEpisode();
    }

    // 씬 로드 완료 시 호출되는 메서드
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        needsReset = true; // 새 씬 로드 후 플래그 다시 활성화
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트 리스너 등록
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 씬 로드 이벤트 리스너 제거
    }
}