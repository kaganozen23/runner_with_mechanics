using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float TimeHolder = 0.0f;

    void Update()
    {
        TimeHolder += Time.deltaTime;
        this.GetComponent<Text>().text = ((int)TimeHolder).ToString();
    }
}
