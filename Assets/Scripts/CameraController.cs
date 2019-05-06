using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject hero;
    public float cameraSpeed = 100f;


    void Update()
    {
        Vector3 newPos = new Vector3(hero.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPos, cameraSpeed * Time.deltaTime);
    }
}
