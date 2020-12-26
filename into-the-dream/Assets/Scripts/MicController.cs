using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicController : MonoBehaviour
{
    AudioSource voiceAudio;

    private float[] samples;
    private float[] smoothSamples;
    private const int sampleCount = 4096;
    private const int sampleRate = 44100;

    private float[] loudness;
    private const int loudCount = 512;
    public static float loud;

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

    private GameObject player;
    private SpriteRenderer Renderer;

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
        int min       = 0;
        int max       = 0;
        maxFreq       = sampleRate * 0.5f;
        samples       = new float[sampleCount];
        smoothSamples = new float[sampleCount];
        loudness      = new float[loudCount];

        voiceAudio      = GetComponent<AudioSource>();
        //voiceAudio.clip = Microphone.Start(null, true, 3599, sampleRate);   //1시간동안 마이크 연결
        Microphone.GetDeviceCaps(Microphone.devices[0], out min, out max);
        voiceAudio.clip = Microphone.Start(Microphone.devices[0], true, 3599, max);

        //while (!(Microphone.GetPosition(null) > 0));
        while (!(Microphone.GetPosition(Microphone.devices[0]) > 1))
        {
            // Wait until the recording has started
        }
        voiceAudio.loop = true;  // 마이크 loop 기능
        voiceAudio.mute = false; // 마이크 mute 기능
        voiceAudio.Play();

        player   = GameObject.Find("Player");
        Renderer = player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sampleIndex = 0;
        amplitude = 0.005f;

        GetLoudnessAudioSource();
        if (loud > 6.0f)
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
            save_peak = peakFreq;
        }
        noteNumber = ToNoteNumber(save_peak) + 12;   //임의로 값을 조정. 추후에 수정예정.
        note = noteNames[noteNumber % 12];
        octave = noteNumber / 12;

        if(loud > 6.0f)
        {
            Debug.Log("주파수 영역 : " + peakFreq + " / " + noteNumber + " / " + note + octave
           + "\n소리 세기 : " + loud + " / Amplitude : " + amplitude);

            // loud가 일정 수준 이상이면 player 활성화
            Renderer.material.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, 1.0f);
            ItdData.User.IsValid = true;
        }
        else
        {
            Debug.Log("세기 : " + loud);

            // loud가 일정 수준 이상이면 player 비활성화(투명화)
            Renderer.material.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, 0.5f);
            ItdData.User.IsValid = false;
        }
    }

    void GetLoudnessAudioSource() // 마이크의 소리의 세기 값을 받아옵니다.
    {
        loud = 0;
        float total_sound = 0;
        voiceAudio.GetOutputData(loudness, 0);
        foreach (float s in loudness)
        {
            total_sound += Mathf.Abs(s);
        }
        loud = total_sound / 256 * 100;
    }

    void GetSpectrumAudioSource()
    {
        voiceAudio.GetSpectrumData(samples      , 0, FFTWindow.BlackmanHarris);
        voiceAudio.GetSpectrumData(smoothSamples, 0, FFTWindow.BlackmanHarris);
    }

    int ToNoteNumber(float freq)
    {
        int note_num = 1;
        for (; note_num < 107; note_num++)
        {
            float prev    = noteFreqs[note_num - 1],
                  next    = noteFreqs[note_num + 1],
                  current = noteFreqs[note_num];

            float min = (prev + current) / 2,
                  max = (next + current) / 2;


            if (min <= freq && freq <= max)
                return note_num;
        }

        return 0;
    }
}
