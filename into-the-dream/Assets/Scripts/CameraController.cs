using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Camera     mainCamera;

    public float speed      = 0.3f;
    public float cameraSize = 5.0f;

    /*void Start()
    {
        // empty
    }
    */

    void Update()
    {
        // player의 y좌표에 따른 카메라 Zoom In/Out 구현
        float y = System.Math.Abs(player.transform.position.y);

        if(y >= 4.0f)
        {
            cameraSize = 10.0f;
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraSize, Time.deltaTime / speed);
        }
        else
        {
            cameraSize = 5.0f;
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraSize, Time.deltaTime / speed);
        }
    }
}
