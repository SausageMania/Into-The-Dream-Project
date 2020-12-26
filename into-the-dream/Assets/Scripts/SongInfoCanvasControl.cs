using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SongInfoCanvasControl : MonoBehaviour
{
    public GameObject image, mainCamera;
    public GameObject songCodeUI, nameUI, artistUI, bpmUI, difficultyUI, maxScoreUI;

    public void SongData()
    {
        // 클릭한 Object의 이름을 기준으로 ItdData에서 곡 정보를 받아옴
        SongData songData = ItdData.Song.getSongData(EventSystem.current.currentSelectedGameObject.name);

        // 받아온 정보를 Canvas에 Mapping
        image       .GetComponent<Image>().sprite = Resources.Load("images/" + songData.SongCode, typeof(Sprite)) as Sprite;
        nameUI      .GetComponent<Text>() .text   = songData.Name;
        difficultyUI.GetComponent<Text>() .text   = songData.Difficulty;
        maxScoreUI  .GetComponent<Text>() .text   = PlayerPrefs.GetInt("BestScore#" + songData.SongCode, 0).ToString("0,000,000");

        // PlayScene에서 곡을 구별하기 위해 ItdData의 CurrentSongCode 최신화
        ItdData.User.CurrentSongCode = songData.SongCode;
    }

    public void SwtichButton()
    {
        mainCamera.GetComponent<GameManager>().SceneSwitch();
        SoundManager.instance.StopBGM();
    }
}
