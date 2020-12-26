using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오디오정보 클래스
[System.Serializable] // 이걸 적어줘야 인스펙터창에 나온다
public class Sound
{
    public string name; // 오디오이름
    public AudioClip clip; // 오디오파일
}

// 오디오를 관리하는 클래스(싱글톤) - 배경음악,효과음,음성을 따로관리
public class SoundManager : MonoBehaviour
{
    private AudioSource audioSetting;
    // 인스턴스를 정적으로 선언
    public static SoundManager instance;
    // private가 생략되어있음 - [SerializeField]를 써줘야 인스펙터창에 뜬다

    [SerializeField] public Sound[]       effectSounds;
    [SerializeField] public AudioSource[] effectPlayer;

    [SerializeField] public Sound[]     bgmSounds;
    [SerializeField] public AudioSource bgmPlayer;

    public float targetValue = 0;
    public AudioSource audioSource;

    #region singleton
    // 싱글톤을 위한 초기화과정
    void Awake()
    {
        audioSetting        = GetComponent<AudioSource>();
        float setVol        = PlayerPrefs.GetFloat("setvol");
        audioSetting.volume = setVol;
        // 인스턴스가 null인 상태라면
        if (instance == null)
        {
            instance = this; // 인스턴스는 자기 자신
            //DontDestroyOnLoad(gameObject); // 다른 씬에서도 사용할 수 있게 지정
        }
        else
        {
            Destroy(gameObject); // 인스턴스가 이미 생성중이라면 파괴하고 새로 만들기
        }
    }
    #endregion singleton

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine(LerpFunction(targetValue, 5));
    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = audioSource.volume;

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = endValue;
    }


    // BGM재생 메소드 파라미터로 이름을 받는다
    void PlayBGM(string p_name)
    {
        // bgmInfo의 배열만큼 반복실행
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            // 파라미터로 넘어온 이름과 bgmInfo의 이름과 비교
            if (p_name == bgmSounds[i].name)
            {
                // bgmInfo에 담겨있는 오디오클립을 재생하고 반복문을 빠져나간다
                bgmPlayer.clip = bgmSounds[i].clip;
                bgmPlayer.Play();
                return;
            }
        }
        Debug.LogError("p_name" + "에 해당되는 BGM이 없습니다");
    }

    // 배경음악을 멈춘다
    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    // 배경음악을 일시정지한다
    public void PauseBGM()
    {
        StartCoroutine(LerpFunction(targetValue, 2));
        Invoke("Pausebgm", 2);
    }
    public void Pausebgm()
    {
        bgmPlayer.Pause();
    }

    // 배경음악의 일시정지를 푼다
    public void UnPauseBGM()
    {
        CancelInvoke("bgmPlayer.Pause");
        bgmPlayer.UnPause();
    }

    // 효과음을 재생 파라미터로 이름을 받는다
    void PlayEffectSound(string p_name)
    {
        // effectSounds의 배열만큼 반복실행
        for (int i = 0; i < effectSounds.Length; i++)
        {
            // 파라미터로 넘어온 이름과 bgmInfo의 이름과 비교
            if (p_name == effectSounds[i].name)
            {
                // 효과음 플레이어의 갯수만큼 반복실행
                for (int j = 0; j < effectPlayer.Length; j++)
                {
                    if (!effectPlayer[j].isPlaying)
                    {
                        effectPlayer[j].clip = effectSounds[i].clip;
                        effectPlayer[j].Play();
                        return;
                    }
                }
                return;
            }
        }
        Debug.LogError(p_name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    // 재생중인 모든 효과음을 멈춘다
    public void StopAllEffectSound()
    {
        // 효과음 플레이어의 갯수만큼 반복실행
        for (int i = 0; i < effectPlayer.Length; i++)
        {
            effectPlayer[i].Stop(); // 효과음 재생 정지
        }
    }

    //--------- 외부에서 호출하는 메소드 --------------
    // 파일이름과 어떤타입의 오디오인지를 인수로 넘겨받는다

    ///
    ///p_Type : 0 -> BGM 배경음악 재생
    ///p_Type : 1 -> SE 효과음 재생
    ///
    public void PlaySound(string p_name, int p_type)
    {
        // 넘겨받은 타입변수값에 따라서 해당 플레이어를 재생시킨다
        if      (p_type == 0) PlayBGM(p_name);
        else if (p_type == 1) PlayEffectSound(p_name);
        else    Debug.LogError("해당하는 타입의 오디오플레이어가 없습니다");
    }

    // 오디오재생을 정지하는 메소드
    public void StopAudio(string p_type)
    {
        if      (p_type == "BGM") StopBGM();
        else if (p_type == "SE")  StopAllEffectSound();
        else    Debug.LogError("해당하는 타입의 오디오플레이어가 없습니다");
    }

    // 모든 오디오재생을 정지하는 메소드
    public void StopAllAudio()
    {
        StopBGM();
        StopAllEffectSound();
    }

}