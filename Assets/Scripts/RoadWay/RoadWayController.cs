using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadWayController : MonoBehaviour
{
    public static Quaternion rot;

    void Start()
    {
        rot = transform.rotation;
    }

    void Update()
    {
        if (transform.rotation != rot) transform.rotation = Quaternion.Slerp(transform.rotation, rot, PlayerMove.cameraRotationSpeed *Time.deltaTime);   //slowly rotate the camera
    }

}
