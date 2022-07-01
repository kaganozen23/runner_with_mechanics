using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float MagnetTime = 4.0f;    //4 seconds

    // Update is called once per frame
    void Update()
    {
        if (PlayerMove.WithMagnet)
        {
            if (MagnetTime > 0.0f) MagnetTime -= Time.deltaTime;
            else 
            {
                PlayerMove.WithMagnet = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerMove.WithMagnet = true;
        GetComponent<MeshRenderer>().enabled = false;
        //Destroy(GetComponent<CapsuleCollider>());
        //this.gameObject.SetActive(false);
    }
}