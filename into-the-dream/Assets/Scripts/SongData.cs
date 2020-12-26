using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    private string songCode;
    public string SongCode
    {
        get
        {
            return songCode;
        }
        set
        {
            songCode = value;
        }
    }

    private string name;
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    private string artist;
    public string Artist
    {
        get
        {
            return artist;
        }
        set
        {
            artist = value;
        }
    }

    private string difficulty;
    public string Difficulty
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = value;
        }
    }

    private int chapter;
    public int Chapter
    {
        get
        {
            return chapter;
        }
        set
        {
            chapter = value;
        }
    }

    private float bpm;
    public float Bpm
    {
        get
        {
            return bpm;
        }
        set
        {
            bpm = value;
        }
    }

    private float startingPoint;
    public float StartingPoint
    {
        get
        {
            return startingPoint;
        }
        set
        {
            startingPoint = value;
        }
    }

    private float runningTime;
    public float RunningTime
    {
        get
        {
            return runningTime;
        }
        set
        {
            runningTime = value;
        }
    }

    public SongData()
    {
        SongCode      = "defaultCode";
        Name          = "defaultName";
        Artist        = "defaultArtist";
        difficulty    = "defaulutDefficulty";
        Bpm           = 0.1f;
        StartingPoint = 0.1f;
        RunningTime   = 20.0f;
    }
    
    public SongData(string songCode, string name, string artist, string difficulty, float bpm, float startingPoint, float runningTime)
    {
        SongCode      = songCode;
        Name          = name;
        Artist        = artist;
        Difficulty    = difficulty;
        Bpm           = bpm;
        StartingPoint = startingPoint;
        RunningTime   = runningTime;
    }
}
