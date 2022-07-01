using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{

    public AudioSource SpeedUpSound;
    public float SpeedUpTime = 4.0f;    //4 seconds

    void OnTriggerEnter(Collider other)
    {
        SpeedUpSound.Play();
        PlayerMove.maxAcceleration = 20.0f;
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpeedUpTime > 0.0f) SpeedUpTime -= Time.deltaTime;
        else PlayerMove.maxAcceleration = 10.0f;
    }
}

