using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    private float distance;
    private Vector2 offset;

    private bool isDown = true;

    // Start is called before the first frame update
    void Start()
    {
        distance = Random.Range(0.1f, 0.5f); 
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDown){
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.005f);
            if(transform.position.y <= offset.y - distance) isDown = false;
        }
        else{
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.005f);
            if(transform.position.y >= offset.y + distance) isDown = true;
        }
    }
}
