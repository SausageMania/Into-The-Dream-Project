using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyricsController : MonoBehaviour
{
    public Vector2 position;

    void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        if (NoteGenerator.isNotesReady)
        {
            // 가사를 왼쪽으로 이동
            position.x += -7.0f * Time.deltaTime;
            transform.position = position;

            // 화면 밖(왼쪽)으로 넘어가면 제거
            if (transform.position.x < -20.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
