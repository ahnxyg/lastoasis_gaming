using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public static FPS Instance { get; private set; }
    public static int frames = 0;

    public static float fps;
    public float alpha = 0.5f;

    private static float timeScale = 1f;
    public float defaultTimeScale;

    public static float emaFPS = 0;

    public static bool start = false;

    /*private void Awake()
    {
        FPS.Instance = this;
        GameManager.reloaded = true;
    }*/

    private void Start()
    {
        if (this.defaultTimeScale < 0.2f || this.defaultTimeScale > 16)
        {
            this.defaultTimeScale = 1f;
        }
        FPS.timeScale = this.defaultTimeScale;
    }

    // Update is called once per frame
    /*void Update()
    {
        FPS.frames++;
        if (GameManager.reloaded)
        {
            Time.timeScale = FPS.timeScale;
            GameManager.reloaded = false;
        }

        this.AdjustTimeScale();

        if (Time.deltaTime > 0)
        {
            FPS.fps = 1 / (Time.deltaTime / FPS.timeScale);
        }
        else
        {
            FPS.fps = 0;
        }
        if (FPS.fps > 0)
        {
            if (!FPS.start)
            {
                FPS.emaFPS = fps;
                FPS.start = true;
            }
            else
            {
                FPS.emaFPS = fps * this.alpha + FPS.emaFPS * (1 - this.alpha);
                Debug.Log($"Current FPS: {fps}, AVG: {FPS.emaFPS}");
            }
        }
    }*/

    public void SetTimeScale(bool normalSpeed)
    {
        if (normalSpeed)
        {
            FPS.timeScale = 1f;
        }

        else
        {
            FPS.timeScale = this.defaultTimeScale;
        }
        Debug.Log($"timeScale: {FPS.timeScale}");
    }

    void AdjustTimeScale()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (FPS.timeScale + 0.2f <= 16)
            {
                FPS.timeScale = FPS.timeScale + 0.2f;
            }
            Time.timeScale = FPS.timeScale;
            Debug.Log($"Time scale was set to {FPS.timeScale}");

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (FPS.timeScale - 0.2f >= 0f)
            {
                FPS.timeScale = FPS.timeScale - 0.2f;
            }
            Time.timeScale = FPS.timeScale;
            Debug.Log($"Time scale was changed to {FPS.timeScale}");
        }
        else if (Input.GetKey(KeyCode.Home))
        {
            FPS.timeScale = this.defaultTimeScale;
            Time.timeScale = FPS.timeScale;
            Debug.Log($"Time scale was changed to {FPS.timeScale}");
        }
    }
}
