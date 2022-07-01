using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectCoins : MonoBehaviour
{
    public AudioSource CoinSound;
    public GameObject coinCounterText;  //reference to UI counter
    public float MagnetCollectDistance = 10.0f;
    public float CoinPullSpeed = 50.0f;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerMove.SelfObject)
        {
            CoinSound.Play();
            //increment UI counter
            coinCounterText.GetComponent<Text>().text = ((int.Parse(coinCounterText.GetComponent<Text>().text))+1).ToString();
            //set coin inactive
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //im with the magnet & my forward position vector is close to coin
        if (PlayerMove.WithMagnet && (Mathf.Abs(transform.position.z - PlayerMove.PlayerZPos) < MagnetCollectDistance))
        {
            // Pull coin towards me
            //Because i move my character controller every frame, this coin will hit me 100% (actually i will hit it)
            transform.position = Vector3.MoveTowards(transform.position,PlayerMove.myTransform.position,CoinPullSpeed * Time.deltaTime);
        }
    }
}
