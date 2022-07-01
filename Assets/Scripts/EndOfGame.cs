using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfGame : MonoBehaviour
{
    public static int finished_as;
    public GameObject FinishText;
    public GameObject RestartButton;
    public GameObject ReturntoMenu;
    
    void start()
    {
        finished_as = 0;
        FinishText = GameObject.Find("Canvas/FinishText");
        RestartButton = GameObject.Find("Canvas/RestartButton");
        ReturntoMenu = GameObject.Find("Canvas/ReturntoMenu");
    }

    void OnTriggerEnter(Collider other)
    {
        finished_as++;
        if (other.gameObject == PlayerMove.SelfObject) //that i reached finish line (plane)
        {
            FinishGame();
        }
        else //that a rival reached finish line
        {
            StartCoroutine(RivalFinishAnim());
        }

    }

    IEnumerator RivalFinishAnim()
    {
        for (float iteration = RivalController.RivalSelfObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed; iteration > 0.0f; iteration -= 1.0f)
        {
            RivalController.RivalSelfObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed -= 1.0f;
            RivalController.RivalSelfObject.transform.rotation = Random.rotation;
            yield return new WaitForSeconds(Random.Range(0.1f,0.2f));
        }
    }

    void FinishGame()
    {
        //finish animation script
        //Destroy(PlayerMove);
        PlayerMove.SelfObject.transform.Rotate(0.0f,180.0f,0.0f,Space.World);
        FileManager.WriteCoinAmount();  //write collected coins to file
        var order = "";
        switch (finished_as)
        {
            case 1:
                order = "first";
                break;
            case 2:
                order = "second";
                break;
            case 3:
                order = "third";
                break;
        }
        FinishText.GetComponent<Text>().text += order;
        FinishText.SetActive(true);
        RestartButton.SetActive(true);
        ReturntoMenu.SetActive(true);
    }
}