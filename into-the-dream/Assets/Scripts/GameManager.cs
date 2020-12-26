using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject PauseCanvas;
    public GameObject SettingCanvas;
    private string thisScene;

    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name; // 실행 중인 씬의 이름을 가져온다
    }
    void Awake()
    {
        // 60프레임 고정
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // 해상도 대응
        /* Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        float scaleWidth = 1f / scaleHeight;

        if(scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        camera.rect = rect;
        */
    }

    void Update()
    {
        if(Input.GetButton("PauseButton"))
        {
            Time.timeScale = 0.0f;
            PauseCanvas.SetActive(true);
        }
    }

    public void ClickPauseButton()
    {
        SoundManager.instance.Pausebgm();
        Time.timeScale = 0f;
        PauseCanvas.SetActive(!PauseCanvas.activeSelf);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(thisScene);
    }

    public void Resume()
    {
        SoundManager.instance.UnPauseBGM();
        PauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 종료한다
#endif
    }

    public void GotoMain()
    {
        ScrollSnapRect.CheckGameStatus = true;
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainScene");
        SoundManager.instance.StopBGM();
    }

    public void ClickSettingButton()
    {
        SettingCanvas.SetActive(!SettingCanvas.activeSelf);
    }

    public void SceneSwitch()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void ActiveScoreUI()
    {
        SceneManager.LoadScene("ScoreScene");
    }
}