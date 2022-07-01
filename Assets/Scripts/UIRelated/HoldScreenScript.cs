using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldScreenScript : MonoBehaviour
{
    private GameObject SelfObject;

    void Start()
    {
        Input.simulateMouseWithTouches = true;  // Considering no need for multitouch support;;;
        SelfObject = this.gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) SelfObject.SetActive(false);
        else SelfObject.SetActive(true);
    }
}
