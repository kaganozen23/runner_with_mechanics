using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningBars : MonoBehaviour
{
    public int turningSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f,0.0f, turningSpeed, Space.World);
    }
}
