using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    private string coin;
    public GameObject coinCounterText;
    public static GameObject ccText;
    // Start is called before the first frame update
    void Start()
    {
        ccText = coinCounterText;
        try{
            StreamReader reader = new StreamReader("Coin.dat");
            coin = reader.ReadLine();   //file exist
            reader.Close();
            coinCounterText.GetComponent<Text>().text = coin;
        }
        catch   //file does not exist
        {
            try
            {
                StreamWriter writer = new StreamWriter("Coin.dat");
                writer.WriteLine("0");
                writer.Close();
            }
            catch
            {
                Debug.Log("Some problems occur on file-ops!");
            }
        }
    }

    public static void WriteCoinAmount()
    {
        try
        {
            StreamWriter writer = new StreamWriter("Coin.dat");
            writer.WriteLine(ccText.GetComponent<Text>().text);
            writer.Close();
        }
        catch
        {
            Debug.Log("Some problems occur on file-ops!");
        }        
    }
}
