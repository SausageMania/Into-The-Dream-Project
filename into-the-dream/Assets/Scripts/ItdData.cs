using System.Collections;
using System.Collections.Generic;

namespace ItdData
{
    public static class User
    {
        /*
            currentSongCode는 현재 접근해야 하는 곡의 코드를 저장
            MainScene에서 별을 클릭할 때 Update되고,
            그 상태에서 앨범아트를 클릭하여 PlayScene으로 넘어가면 이 코드에 해당하는 곡으로 세팅을 한다.
        */
        private static string currentSongCode = "0000";
        public static string CurrentSongCode
        {
            get
            {
                return currentSongCode;
            }
            set
            {
                currentSongCode = value;
            }
        }

        private static int currentSongScore = 0;
        public static int CurrentSongScore
        {
            get
            {
                return currentSongScore;
            }
            set
            {
                currentSongScore = value;
            }
        }

        private static int numberOfPerfect = 0;
        public static int NumberOfPerfect
        {
            get
            {
                return numberOfPerfect;
            }
            set
            {
                numberOfPerfect = value;
            }
        }

        private static int numberOfGood = 0;
        public static int NumberOfGood
        {
            get
            {
                return numberOfGood;
            }
            set
            {
                numberOfGood = value;
            }
        }

        private static int numberOfMiss = 0;
        public static int NumberOfMiss
        {
            get
            {
                return numberOfMiss;
            }
            set
            {
                numberOfMiss = value;
            }
        }

        private static int combo = 0;
        public static int Combo
        {
            get
            {
                return combo;
            }
            set
            {
                combo = value;
            }
        }

        private static string accuracy = "";
        public static string Accuracy
        {
            get
            {
                return accuracy;
            }
            set
            {
                accuracy = value;
            }
        }

        private static bool isValid = false;
        public static bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                isValid = value;
            }
        }
    }

    public static class Song
    {
        public static SongData getSongData(string songCode)
        {
            SongData songData = new SongData();

            if     (songCode == "0001") // 작은별
            {
                songData.SongCode      = "0001";
                songData.Name          = "작은별";
                songData.Artist        = "W.A.Mozart";
                songData.Difficulty    = "Easy";
                songData.Chapter       = 1;
                songData.Bpm           = 1.4f;
                songData.StartingPoint = 25.0f;
                // songData.RunningTime   = 71.0f;
                songData.RunningTime = 10.0f; // 테스트용
            }
            else if(songCode == "0002") // 나비야
            {
                // 예시 데이터
                songData.SongCode      = "0002";
                songData.Name          = "나비야";
                songData.Artist        = "Franz Wiedemann";
                songData.Difficulty    = "Easy";
                songData.Chapter       = 1;
                songData.Bpm           = 0.1f;
                songData.StartingPoint = 0.1f;
                songData.RunningTime   = 20.0f;
            }
            else if (songCode == "0003") // 곰세마리
            {
                songData.SongCode      = "0003";
                songData.Name          = "곰세마리";
                songData.Artist        = "작자미상";
                songData.Difficulty    = "Easy";
                songData.Chapter       = 1;
                songData.Bpm           = 1.4f;
                songData.StartingPoint = 25.0f;
                songData.RunningTime   = 71.0f;
            }
            else if (songCode == "0004") // 울면안돼
            {
                songData.SongCode      = "0004";
                songData.Name          = "울면안돼";
                songData.Artist        = "John Frderick Coots" + "\n" + "Haven Gilespie";
                songData.Difficulty    = "Easy";
                songData.Chapter       = 1;
                songData.Bpm           = 0.1f;
                songData.StartingPoint = 0.1f;
                songData.RunningTime   = 20.0f;
            }
            else if (songCode == "0005") // 둥근해가 떴습니다
            {
                songData.SongCode      = "0005";
                songData.Name          = "둥근해가" + "\n" + "떴습니다";
                songData.Artist        = "N/A";
                songData.Difficulty    = "Easy";
                songData.Chapter       = 2;
                songData.Bpm           = 0.1f;
                songData.StartingPoint = 0.1f;
                songData.RunningTime   = 20.0f;
            }
            else if (songCode == "0006") // 거미가 줄을타고 올라갑니다
            {
                songData.SongCode = "0006";
                songData.Name = "거미가 줄을타고" + "\n" + "올라갑니다";
                songData.Artist = "N/A";
                songData.Difficulty = "Easy";
                songData.Chapter = 2;
                songData.Bpm = 0.1f;
                songData.StartingPoint = 0.1f;
                songData.RunningTime = 20.0f;
            }
            else if (songCode == "0007") // 졸려졸려
            {
                songData.SongCode = "0007";
                songData.Name = "졸려졸려";
                songData.Artist = "N/A";
                songData.Difficulty = "Easy";
                songData.Chapter = 2;
                songData.Bpm = 0.1f;
                songData.StartingPoint = 0.1f;
                songData.RunningTime = 20.0f;
            }
            else if (songCode == "0008") // 똑같아요
            {
                songData.SongCode = "0008";
                songData.Name = "똑같아요";
                songData.Artist = "N/A";
                songData.Difficulty = "Easy";
                songData.Chapter = 2;
                songData.Bpm = 0.1f;
                songData.StartingPoint = 0.1f;
                songData.RunningTime = 20.0f;
            }
            else                        // 존재하지 않는 SongCode
            {
                songData.SongCode      = "error";
                songData.Name          = "error";
                songData.Artist        = "error";
                songData.Difficulty    = "error";
                songData.Chapter       = -1;
                songData.Bpm           = 0.1f;
                songData.StartingPoint = 0.1f;
                songData.RunningTime   = 20.0f;
            }

            return songData;
        }
    }

    public static class Scale
    {
        public const float C3 = -2.7f;
        public const float Cs3 = -2.4f;
        public const float D3 = -2.1f;
        public const float Ds3 = -1.8f;
        public const float E3 = -1.5f;
        public const float F3 = -1.2f;
        public const float Fs3 = -0.9f;
        public const float G3 = -0.6f;
        public const float Gs3 = -0.3f;
        public const float A3 = 0.0f;
        public const float As3 = 0.3f;
        public const float B3 = 0.6f;

        public const float C4 = 0.9f;
        public const float Cs4 = 1.2f;
        public const float D4 = 1.5f;
        public const float Ds4 = 1.8f;
        public const float E4 = 2.1f;
        public const float F4 = 2.4f;
        public const float Fs4 = 2.7f;
        public const float G4 = 3.0f;
        public const float Gs4 = 3.3f;
        public const float A4 = 3.6f;
        public const float As4 = 3.9f;
        public const float B4 = 4.2f;
        public const float C5 = 4.5f;
    }
}