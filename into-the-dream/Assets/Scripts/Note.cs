using System.Collections;
using System.Collections.Generic;

namespace NoteInfomation
{
    public class Note
    {
        // 박자 정보
        private float time;
        public float Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

        // 음계 정보
        private float scale;
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        // 가사 정보
        private string lyrics;
        public string Lyrics
        {
            get
            {
                return lyrics;
            }

            set
            {
                lyrics = value;
            }
        }

        public Note(float time, float scale)
        {
            this.time   = time;
            this.scale  = scale;
            this.lyrics = "-";
        }

        public Note(float time, float scale, string lyrics)
        {
            this.time   = time;
            this.scale  = scale;
            this.lyrics = lyrics;
        }
    }
}