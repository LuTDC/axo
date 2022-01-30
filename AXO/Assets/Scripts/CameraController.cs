using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private Transform up, down, left, right;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        if(transform.position.x > right.position.x) transform.position = new Vector3(right.position.x, transform.position.y, transform.position.z);
        if(transform.position.x < left.position.x) transform.position = new Vector3(left.position.x, transform.position.y, transform.position.z);
        if(transform.position.y > up.position.y) transform.position = new Vector3(transform.position.x, up.position.y, transform.position.z);
        if(transform.position.y < down.position.y) transform.position = new Vector3(transform.position.x, down.position.y, transform.position.z);
    }
}
