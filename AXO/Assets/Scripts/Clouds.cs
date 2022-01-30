using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    private float speed = -0.1f;
    private float offsetX;

    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offsetX += (Time.deltaTime * speed) / 10;
        material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
    }
}
