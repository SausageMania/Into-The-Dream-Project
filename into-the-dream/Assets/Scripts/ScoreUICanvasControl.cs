using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUICanvasControl : MonoBehaviour
{
    public GameObject difficultyUI, songNameUI, scoreInfoUI, rankUI, perfectUI, goodUI, missUI, bestScoreUI;

    void Start()
    {
        difficultyUI = GameObject.Find("Difficulty");
        songNameUI   = GameObject.Find("SongName");
        scoreInfoUI  = GameObject.Find("ScoreInfo");
        rankUI       = GameObject.Find("Rank");
        perfectUI    = GameObject.Find("PerfectCombo");
        goodUI       = GameObject.Find("GoodCombo");
        missUI       = GameObject.Find("MissCombo");
        bestScoreUI  = GameObject.Find("BestScore");

        // ItdData에서 곡 정보를 받아옴
        SongData songData = ItdData.Song.getSongData(ItdData.User.CurrentSongCode);

        // 받아온 정보를 Canvas에 Mapping
        difficultyUI.GetComponent<Text>().text = songData.Difficulty;
        songNameUI  .GetComponent<Text>().text = songData.Name;
        scoreInfoUI .GetComponent<Text>().text = ItdData.User.CurrentSongScore.ToString("0,000,000");
        perfectUI   .GetComponent<Text>().text = ItdData.User.NumberOfPerfect.ToString();
        goodUI      .GetComponent<Text>().text = ItdData.User.NumberOfGood.ToString();
        missUI      .GetComponent<Text>().text = ItdData.User.NumberOfMiss.ToString();

        // Rank 등급 분기 (임시 구현)
        if (ItdData.User.CurrentSongScore >= 1000000)
        {
            rankUI.GetComponent<Text>().text = "SS";
        }
        else if (ItdData.User.CurrentSongScore < 1000000 && ItdData.User.CurrentSongScore >= 950000)
        {
            rankUI.GetComponent<Text>().text = "S";
        }
        else if (ItdData.User.CurrentSongScore < 950000 && ItdData.User.CurrentSongScore >= 900000)
        {
            rankUI.GetComponent<Text>().text = "A";
        }
        else if (ItdData.User.CurrentSongScore < 900000 && ItdData.User.CurrentSongScore >= 850000)
        {
            rankUI.GetComponent<Text>().text = "B";
        }
        else if (ItdData.User.CurrentSongScore < 850000 && ItdData.User.CurrentSongScore >= 800000)
        {
            rankUI.GetComponent<Text>().text = "C";
        }
        else
        {
            rankUI.GetComponent<Text>().text = "Fail";
        }

        // BestScore가 갱신된 상태라면 BestScore임을 표시해야 함
        if (ItdData.User.CurrentSongScore == PlayerPrefs.GetInt("BestScore#" + ItdData.User.CurrentSongCode, 0))
        {
            Debug.Log("BestScore 갱신");
            bestScoreUI.GetComponent<Text>().text = "Best\nScore";
        }
        else
        {
            bestScoreUI.GetComponent<Text>().text = "";
        }
    }

    public void restart()
    {
        ScrollSnapRect.CheckGameStatus = true;
        SceneManager.LoadSceneAsync("PlayScene");
    }
}
