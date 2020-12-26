using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private MeshRenderer render;

    public float speed = 1.0f;
    public float offset;

    private void Start()
    {
        render = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        offset += Time.deltaTime * speed;
        render.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
