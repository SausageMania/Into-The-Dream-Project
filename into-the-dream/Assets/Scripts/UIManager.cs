using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    SongData songData;

    GameObject scoreUI;
    GameObject songInfoUI;
    GameObject progressBarUI;
    GameObject ScoreUI;
    GameObject accuracyUI;
    GameObject comboUI;

    public GameObject mainCamera;

    public static int score;
    public float playTime;

    void Start()
    {
        // CurrentSongCode를 기준으로 UI 최신화
        songData = ItdData.Song.getSongData(ItdData.User.CurrentSongCode);
     
        score    = 0;
        playTime = 0.0f;

        scoreUI       = GameObject.Find("Score");
        songInfoUI    = GameObject.Find("SongInfo");
        progressBarUI = GameObject.Find("ProgressBar");
        ScoreUI       = GameObject.Find("ScoreUIPanel");
        accuracyUI    = GameObject.Find("Accuracy");
        comboUI       = GameObject.Find("Combo");

        songInfoUI   .GetComponent<Text  >().text     = songData.Artist + " - " + songData.Name;
        progressBarUI.GetComponent<Slider>().maxValue = songData.RunningTime;
    }

    void Update()
    {
        playTime += Time.deltaTime;

        scoreUI      .GetComponent<Text  >().text  = score.ToString("0,000,000");
        progressBarUI.GetComponent<Slider>().value = playTime;
        accuracyUI   .GetComponent<Text  >().text  = ItdData.User.Accuracy;
        comboUI      .GetComponent<Text  >().text  = ItdData.User.Combo.ToString() + " Combo";

        // 노래가 끝나면 결과창 표시
        if(progressBarUI.GetComponent<Slider>().value >= progressBarUI.GetComponent<Slider>().maxValue)
        {
            // Debug.Log("Song Finish Event");

            // ScoreScene에 Score 값을 전달하기 위해 임시로 currentSongScore 변수에 저장
            ItdData.User.CurrentSongScore = score;

            // Debug.Log("이번 판의 Score : " + score.ToString());
            // Debug.Log("기존 BestScore : " + PlayerPrefs.GetInt("BestScore#" + songData.SongCode, 0));

            // BestScore일 경우 갱신
            if(score > PlayerPrefs.GetInt("BestScore#" + songData.SongCode, 0))
            {
                PlayerPrefs.SetInt("BestScore#" + songData.SongCode, score);
            }

            mainCamera.GetComponent<GameManager>().ActiveScoreUI();
            // GameScene의 진행 로직을 멈추고 (현재 Log가 1프레임마다 띄워지는 상태)
            // 결과 패널을 띄운다.
        }
    }
}
