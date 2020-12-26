using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class SoundDrawer : MonoBehaviour
{
    AudioSource _audioSource;

    private float[] samples;
    private float[] smoothSamples;
    private const int sampleCount = 4096;
    private const int sampleRate = 44100;

    private float[] loudness;
    private const int loudCount = 512;
    private float loud;

    private float maxFreq;
    private float peakFreq;
    private float save_peak;

    private int sampleIndex;
    private float amplitude;

    private float[] noteFreqs;
    public static int noteNumber;

    private string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    private int octave = 0;
    private string note = "C";


    // Start is called before the first frame update

    void Awake()
    {
        noteFreqs = new float[108];
        for (int i = 0; i < 108; i++)
            noteFreqs[i] = 440f * Mathf.Pow(2, (i - 57) / 12.0f);
        peakFreq = 0.0f;
        save_peak = 0.0f;
    }

    void Start()
    {
        maxFreq = sampleRate * 0.5f;
        samples = new float[sampleCount];
        smoothSamples = new float[sampleCount];
        loudness = new float[loudCount];
        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(TimeUpdate(0.05f)); //0.05초 마다 Coroutine을 돌립니다.(Update 대용)
    }


    private IEnumerator TimeUpdate(float delayTime)
    {
        float time = 0;

        sampleIndex = 0;
        amplitude = 0.01f;
        while (true)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();   //아래의 코드를 실행시키는 시간 측정
            time += delayTime;
            sw.Start();
            GetLoudnessAudioSource();
            if (loud > 8.0f)
            {
                GetSpectrumAudioSource();
                for (int i = 0; i < sampleCount; i++)
                {
                    smoothSamples[i] = Mathf.Lerp(smoothSamples[i], samples[i], Time.deltaTime * 10);

                    if (smoothSamples[i] > amplitude)
                    {
                        amplitude = smoothSamples[i];
                        sampleIndex = i;
                    }
                }
                peakFreq = ((float)sampleIndex / sampleCount) * maxFreq * 1.09f;    //음역 계산(정확도를 위해 1.09추가 곱)

                if (save_peak != peakFreq)
                {
                    save_peak = peakFreq;

                    noteNumber = ToNoteNumber(save_peak) + 12;   //임의로 값을 조정. 추후에 수정예정.
                    note = noteNames[noteNumber % 12];
                    octave = noteNumber / 12;

                    sw.Stop();
                    time += sw.ElapsedMilliseconds * 1000;
                    time %= 1000; // 버그 임시조치

                    string fullnote = note + octave;
                    WriteFile("NoteGenerator.txt", fullnote, time.ToString("F2"));
                }
                Debug.Log("주파수 영역 : " + peakFreq + " / " + noteNumber + " / " + note + octave
                       + "\n소리 세기 : " + loud + " / 시간 : " + time);
            }
            else if(loud < 4.0f) //정확도 개선
            {
                peakFreq = 0.0f;
                save_peak = 0.0f;
                sampleIndex = 0;
                amplitude = 0.005f;
                Debug.Log("소리 없음");
            }

            yield return new WaitForSeconds(delayTime);
        }
    }

    void GetLoudnessAudioSource()
    {
        loud = 0;
        // playerController에 loud 연결
        // getloud를 만드는 방향으로.

        float total_sound = 0;
        _audioSource.GetOutputData(loudness, 0);
        foreach (float s in loudness)
        {
            total_sound += Mathf.Abs(s);
        }
        loud = total_sound / 256 * 100;
    }

    void GetSpectrumAudioSource()
    {
        //_audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        _audioSource.GetSpectrumData(smoothSamples, 0, FFTWindow.BlackmanHarris);
    }

    int ToNoteNumber(float freq)
    {
        int note_num = 1;
        for (; note_num < 107; note_num++)
        {
            float prev = noteFreqs[note_num - 1],
                  next = noteFreqs[note_num + 1],
                  current = noteFreqs[note_num];

            float min = (prev + current) / 2,
                  max = (next + current) / 2;


            if (min <= freq && freq <= max)
                return note_num;
        }

        return 0;
    }

    void WriteFile(string FileName, string fullnote, string time)
    {
        FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
        sw.WriteLine(fullnote + " " + time);
        sw.Flush();
        sw.Close();
    }
}
