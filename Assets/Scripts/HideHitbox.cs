using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideHitbox : MonoBehaviour
{
    
    public enum scenes
    {
        inGame,
        levelSelect
    }
    public scenes thisScene;
    
    public GameObject b1;
    public GameObject b2;
    public GameObject b3;
    public GameObject b4;
    public GameObject b5;
    public GameObject b6;

    public GameObject c1;


    

    // Start is called before the first frame update
    void Start()
    {
        if(thisScene == scenes.inGame)
        {
            b1.GetComponent<Image>().color = new Color (1,1,1,0);
            b2.GetComponent<Image>().color = new Color (1,1,1,0);
            b3.GetComponent<Image>().color = new Color (1,1,1,0);
            b4.GetComponent<Image>().color = new Color (1,1,1,0);
            b5.GetComponent<Image>().color = new Color (1,1,1,0);
            b6.GetComponent<Image>().color = new Color (1,1,1,0);
        }
        else
        {
            c1.GetComponent<Image>().color = new Color (1,1,1,0);
        }

    }

}
