using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStatus : MonoBehaviour
{
    private Image image;

    // Update is called once per frame
    void Update()
    {
        image = GameObject.FindWithTag("Star").GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
    }
}
