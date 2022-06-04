using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelBlock : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isPlatinum;
    public int numOfMoves;
    public int minMoves;

    public GameObject moveCounter;
    public GameObject star;
    public GameObject wow;

    public GameObject locked;

    public Sprite goldenStar;
    public Sprite platinumStar;

    void Start()
    {
        
    }
    public void Setup()
    {
        moveCounter = transform.GetChild(1).gameObject;
        star = transform.GetChild(3).gameObject;
        wow = transform.GetChild(4).gameObject;
        
        if(locked != null && DataManager.gameData.levelElevenIsOpen)
        {
            locked.SetActive(false);
        }
        if(numOfMoves > 0)
        {
            star.SetActive(true);
            moveCounter.GetComponent<TextMeshPro>().text = numOfMoves.ToString();
            if(isPlatinum)
            {
                Debug.Log("platinum");
                star.GetComponent<SpriteRenderer>().sprite = platinumStar;
                
            }
            else if(numOfMoves == minMoves)
            {
                star.GetComponent<SpriteRenderer>().sprite = goldenStar;
            }
        }
    }
}
