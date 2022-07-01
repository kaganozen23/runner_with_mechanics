using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningCoins : MonoBehaviour
{
    public int turningSpeed = 1;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f,turningSpeed, 0.0f, Space.World);
    }
}
