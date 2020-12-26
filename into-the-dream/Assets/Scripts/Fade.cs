using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public UnityEngine.UI.Image fade;
    float fades = 0.5f;
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(fades > 0.0f && time >= 0.1f)
        {
            fades -= 0.1f;
            fade.color = new Color(0, 0, 0, fades);
            time = 0;
        } else if (fades <= 0.0f)
        {
            // 다음씬으로 넘어가거나 다음 행동을 할 것에 대해서 쓸 것
            time = 0;
        }
    }
}
