using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveCounter : MonoBehaviour
{
    // Start is called before the first frame update

    private TextMeshProUGUI myText;
    public int numOfMoves;
    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
        numOfMoves = 0;
    }

    // Update is called once per frame
    public void Plus()
    {
        numOfMoves++;
        myText.text = numOfMoves.ToString();
    }
    public void Minus()
    {
        numOfMoves--;
        myText.text = numOfMoves.ToString();
    }
    public void ResetCounter()
    {
        numOfMoves = 0;
        myText.text = numOfMoves.ToString();
    }
}
