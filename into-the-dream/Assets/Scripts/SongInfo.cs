using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SongInfo : MonoBehaviour
{
    public RectTransform container;
    private int usingPanel;
    
    public GameObject songInfo;
    public static bool toggle;
    private float waitingTime, timer;
    SongData songData;

    void Start()
    {
        // container안에 있는 패널 개수 = usingPanel
        usingPanel = container.childCount;
        // 0번은 Title이므로 스킵, onClick.Addlistener 각각 별마다 대기
        for(int i = 1; i< usingPanel; i++)
        {
            for(int j = 0; j < container.GetChild(i).childCount; j++)
            {
                container.GetChild(i).GetChild(j).GetComponent<Button>().onClick.AddListener(Click);
            }
        }

        toggle      = false;
        waitingTime = 0.8f;
        timer       = 0.0f;
    }

    void Update()
    {
        if (toggle)
        {
            timer += Time.deltaTime;
            if(timer >= waitingTime)
            {
                songInfo.SetActive(true);
                SoundManager.instance.PauseBGM();
                SoundManager.instance.PlaySound(songData.SongCode, 1);
            }
        }
        else
        {
            songInfo.SetActive(false);
            SoundManager.instance.StopAllEffectSound();
            SoundManager.instance.UnPauseBGM();
            timer = 0.0f;
        }
    }

    void Click()
    {
        songData = ItdData.Song.getSongData(ItdData.User.CurrentSongCode);
        Debug.Log(ItdData.User.CurrentSongCode);
        toggle = !toggle;
    }
}
