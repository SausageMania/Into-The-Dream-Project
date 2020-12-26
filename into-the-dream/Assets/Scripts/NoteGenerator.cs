using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NoteInfomation;
using System.IO;
using System.Text;

public class NoteGenerator : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject lyricsPrefab;
    public List<Note> notes = new List<Note>();
    public SongData songData = new SongData();
    public static bool isNotesReady = false;


    void Start()
    {
        // [1] MainScene에서 노래 코드(정보)를 받고 해당 BPM 및 노트를 적용
        songData = ItdData.Song.getSongData(ItdData.User.CurrentSongCode);

        // [2] 해당 노래에 맞는 노트들을 List에 추가
        // 추후 SongCode에 따라 노트를 찍는 기능 개발해야 함
        CreateSong();

        // [3] 준비된 노트들을 생성
        CreateCoin();

        // [4] 노래 재생 및 노트 이동 시작
        isNotesReady = true;
        SoundManager.instance.PlaySound(songData.SongCode, 0);

        // [5] 노래가 끝나면 isNotesReady를 false로 바꾸고 결과창으로 이동
    }
    
    void CreateCoin()
    {
        foreach (Note note in notes)
        {
            note.Time *= songData.Bpm;
            note.Time += songData.StartingPoint;

            GameObject coin = Instantiate(coinPrefab) as GameObject;
            coin.transform.localScale = new Vector2(0.25f, 0.25f);
            coin.transform.position = new Vector2(note.Time, note.Scale);

            // 가사가 있는 노트만 출력
            if (!note.Lyrics.Equals("-"))
            {
                GameObject lyrics = Instantiate(lyricsPrefab) as GameObject;
                lyrics.transform.position = new Vector2(note.Time, note.Scale - 1.0f);
                lyrics.GetComponent<TextMesh>().text = note.Lyrics;
            }
        }
    }

    // 노래 많아지면 각 노래의 노트 정보들을 한 곳에 모아두거나 DB에 저장하는 작업이 필요할 것 같음
    void CreateSong()
    {
        TextAsset asset = Resources.Load<TextAsset>("NoteGenerator");
        string str = asset.text;
        StringReader sr = new StringReader(str);
        //StreamReader sr = new StreamReader("NoteGenerator.txt");
        string line = sr.ReadLine();
        while (line != null)
        {
            string[] note_value = line.Split(' ');

            float scale = Convert_to_Scale(note_value[0]);
            float time = float.Parse(note_value[1])*5.5f - 14.0f; //임시로 Note의 x좌표 위치를 잡아놓음.
            string lyrics = (note_value[2] != null) ? note_value[2] : "";

            notes.Add(new Note(time, scale, lyrics));
            Debug.Log("Time : " + time + " Scale :" + scale + "Lyrics :" + lyrics);
            line = sr.ReadLine();
        }

        /*
        notes.Add(new Note(0 , ItdData.Scale.B3, "반"));
        notes.Add(new Note(0.35f , ItdData.Scale.D4, "짝"));
        notes.Add(new Note(0.8f, ItdData.Scale.G4, "반"));
        notes.Add(new Note(1.25f , ItdData.Scale.B3, "짝"));
        notes.Add(new Note(4 , ItdData.Scale.D4));

        notes.Add(new Note(7 , ItdData.Scale.C4));
        notes.Add(new Note(10, ItdData.Scale.E4));
        notes.Add(new Note(13, ItdData.Scale.E4));

        notes.Add(new Note(16, ItdData.Scale.D4));
        notes.Add(new Note(17, ItdData.Scale.Fs4));
        notes.Add(new Note(18, ItdData.Scale.A4));
        notes.Add(new Note(19, ItdData.Scale.C5));
        notes.Add(new Note(20, ItdData.Scale.B4));
        notes.Add(new Note(21, ItdData.Scale.A4));
        notes.Add(new Note(22, ItdData.Scale.G4));
        notes.Add(new Note(25, ItdData.Scale.G4));

        // ==============================================================

        notes.Add(new Note(31, ItdData.Scale.B3, "깊"));
        notes.Add(new Note(32, ItdData.Scale.D4, "은"));
        notes.Add(new Note(33, ItdData.Scale.G4, "산"));
        notes.Add(new Note(34, ItdData.Scale.B3));
        notes.Add(new Note(35, ItdData.Scale.D4, "속"));

        notes.Add(new Note(38, ItdData.Scale.C4, "옹"));
        notes.Add(new Note(40, ItdData.Scale.E4, "달"));
        notes.Add(new Note(43, ItdData.Scale.E4, "샘"));

        notes.Add(new Note(45, ItdData.Scale.D4, "누"));
        notes.Add(new Note(46, ItdData.Scale.Fs4, "가"));
        notes.Add(new Note(47, ItdData.Scale.A4, "와"));
        notes.Add(new Note(48, ItdData.Scale.C5, "서"));
        notes.Add(new Note(49, ItdData.Scale.B4, "먹"));
        notes.Add(new Note(50, ItdData.Scale.A4, "나"));
        notes.Add(new Note(51, ItdData.Scale.G4, "요"));

        //// ==============================================================

        //notes.Add(new Note(14.75f, ItdData.Scale.E3, "맑"));
        //notes.Add(new Note(15.00f, ItdData.Scale.G3, "고"));
        //notes.Add(new Note(15.25f, ItdData.Scale.C4, "맑"));
        //notes.Add(new Note(15.50f, ItdData.Scale.E3));
        //notes.Add(new Note(15.75f, ItdData.Scale.G4, "은"));

        //notes.Add(new Note(16.50f, ItdData.Scale.F3, "옹"));
        //notes.Add(new Note(17.00f, ItdData.Scale.A3, "달"));
        //notes.Add(new Note(17.50f, ItdData.Scale.A3, "샘"));

        //notes.Add(new Note(18.00f, ItdData.Scale.G3, "누"));
        //notes.Add(new Note(18.25f, ItdData.Scale.B3, "가"));
        //notes.Add(new Note(18.50f, ItdData.Scale.D4, "와"));
        //notes.Add(new Note(18.75f, ItdData.Scale.F4, "서"));
        //notes.Add(new Note(19.00f, ItdData.Scale.E4, "먹"));
        //notes.Add(new Note(19.25f, ItdData.Scale.D4, "나"));
        //notes.Add(new Note(19.50f, ItdData.Scale.C4, "요"));

        //// ==============================================================

        //notes.Add(new Note(21.25f, ItdData.Scale.C4, "새"));
        //notes.Add(new Note(21.75f, ItdData.Scale.E4, "벽"));
        //notes.Add(new Note(22.25f, ItdData.Scale.G4, "에"));

        //notes.Add(new Note(22.75f, ItdData.Scale.F4, "토"));
        //notes.Add(new Note(23.50f, ItdData.Scale.E4, "끼"));
        //notes.Add(new Note(23.75f, ItdData.Scale.D4, "가"));

        //notes.Add(new Note(24.25f, ItdData.Scale.G4, "눈"));
        //notes.Add(new Note(24.75f, ItdData.Scale.B4, "비"));
        //notes.Add(new Note(25.00f, ItdData.Scale.D4, "비"));
        //notes.Add(new Note(25.25f, ItdData.Scale.F4,"고"));

        //notes.Add(new Note(25.75f, ItdData.Scale.E4, "일"));
        //notes.Add(new Note(26.50f, ItdData.Scale.D4, "어"));
        //notes.Add(new Note(26.75f, ItdData.Scale.C4, "나"));

        //// ==============================================================

        //notes.Add(new Note(27.50f, ItdData.Scale.E3, "세"));
        //notes.Add(new Note(27.75f, ItdData.Scale.G3, "수"));
        //notes.Add(new Note(28.00f, ItdData.Scale.C4, "하"));
        //notes.Add(new Note(28.25f, ItdData.Scale.E3));
        //notes.Add(new Note(28.50f, ItdData.Scale.G3, "러"));

        //notes.Add(new Note(29.25f, ItdData.Scale.F3, "왔"));
        //notes.Add(new Note(29.75f, ItdData.Scale.A3, "다"));
        //notes.Add(new Note(30.25f, ItdData.Scale.A3, "가"));

        //notes.Add(new Note(30.75f, ItdData.Scale.G3, "물"));
        //notes.Add(new Note(31.00f, ItdData.Scale.B3, "만"));
        //notes.Add(new Note(31.25f, ItdData.Scale.D4, "먹"));
        //notes.Add(new Note(31.50f, ItdData.Scale.F4, "고"));
        //notes.Add(new Note(31.75f, ItdData.Scale.E4, "가"));
        //notes.Add(new Note(32.00f, ItdData.Scale.D4, "지"));
        //notes.Add(new Note(32.25f, ItdData.Scale.C4, "요"));
        */
    }

    float Convert_to_Scale(string scale)
    {
        float result = 0.0f;
        switch (scale)
        {
            case "C4":
                result = -2.7f;
                break;
            case "Cs4":
                result = -2.4f;
                break;
            case "D4":
                result = -2.1f;
                break;
            case "Ds4":
                result = -1.8f;
                break;
            case "E4":
                result = -1.5f;
                break;
            case "F4":
                result = -1.2f;
                break;
            case "Fs4":
                result = -0.9f;
                break;
            case "G4":
                result = -0.6f;
                break;
            case "Gs4":
                result = -0.3f;
                break;
            case "A4":
                result = 0.0f;
                break;
            case "As4":
                result = 0.3f;
                break;
            case "B4":
                result = 0.6f;
                break;
            case "C5":
                result = 0.9f;
                break;
            case "Cs5":
                result = 1.2f;
                break;
            case "D5":
                result = 1.5f;
                break;
            case "Ds5":
                result = 1.8f;
                break;
            case "E5":
                result = 2.1f;
                break;
            case "F5":
                result = 2.4f;
                break;
            case "Fs5":
                result = 2.7f;
                break;
            case "G5":
                result = 3.0f;
                break;
            case "Gs5":
                result = 3.3f;
                break;
            case "A5":
                result = 3.6f;
                break;
            case "As5":
                result = 3.9f;
                break;
            case "B5":
                result = 4.2f;
                break;
            case "C6":
                result = 4.5f;
                break;
        }
        return result;
    }
}