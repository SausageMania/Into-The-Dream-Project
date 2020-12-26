using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public Vector2 position;
    private GameObject player;
    private SpriteRenderer Renderer;
    private object coll;
    private int numberOfNotes;

    void Start()
    {
        position = transform.position;

        player   = GameObject.Find("Player");
        Renderer = player.GetComponent<SpriteRenderer>();

        numberOfNotes = 31; // 임의로 넣은 값 (GetNumberOfNotes()가 구현되면 지울 예정)
        // numberOfNotes = GetNumberOfNotes();
        ItdData.User.NumberOfMiss = numberOfNotes;
    }

    void Update()
    {
        if(NoteGenerator.isNotesReady)
        {
            // 노트를 왼쪽으로 이동
            position.x += -7.0f * Time.deltaTime;
            transform.position = position;

            // Player를 지나치면 Combo 초기화
            if(transform.position.x < -8.0f)
            {
                ItdData.User.Combo = 0;
                ItdData.User.Accuracy = "Miss";
            }

            // 화면을 넘어가면 SetActive(false)
            if(transform.position.x < -12.0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // 플레이어와 충돌하면 제거
    void OnTriggerEnter2D(Collider2D col)
    {
        // IsValid가 true일 때만 충돌 적용(Mic의 Loud가 일정 수준 이상일 때만 적용)
        if(ItdData.User.IsValid)
        {
            SoundManager.instance.PlaySound("Coin", 1); // 효과음 재생
            Destroy(gameObject); // 노트 제거

            if(col.gameObject.tag == "FrontCollider") // Perfect 판정
            {
                Debug.Log(col.gameObject.tag);
                UIManager.score += 1000000 / numberOfNotes;
                ItdData.User.NumberOfPerfect += 1;
                ItdData.User.Combo           += 1;
                ItdData.User.Accuracy        = "Perfect";
            }
            else if(col.gameObject.tag == "BackCollider")  // Good 판정
            {
                Debug.Log(col.gameObject.tag);
                UIManager.score += 1000000 / numberOfNotes / 2;
                ItdData.User.NumberOfGood += 1;
                ItdData.User.Combo        += 1;
                ItdData.User.Accuracy        = "Good";
            }

            /*
                점수 오차 보정

                각 노트의 점수가 1,000,000 / 노트 개수(int) 이므로 나머지로 인해 오차가 발생함
                만약 전부 Perfect 판정일 경우 1,000,000점으로 인정될 수 있도록 계산
                (모든 노트가 Perfect라면 마지막 노트가 더 많은 점수를 부여받는 것임)
            */
            if(UIManager.score >= 1000000 - (1000000 % numberOfNotes))
            {
                UIManager.score = 1000000;
            }

            // Miss 개수 갱신
            ItdData.User.NumberOfMiss = numberOfNotes - ItdData.User.NumberOfPerfect - ItdData.User.NumberOfGood;
        }
    }
}
