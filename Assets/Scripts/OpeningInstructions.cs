using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpeningInstructions : MonoBehaviour
{
    public bool nextText;
    public bool inputOpen;
    bool nextTextDebounce;
    
    public int instructionStage;

    string instructOne;
    string instructOne2;
    string instructTwo;
    string instructTwo2;
    string instructThree;
    string instructFinished;
    string instructFinished2;

    public GameObject instructionOne;
    public GameObject instructionTwo;
    public GameObject good;

    Animator animator;
    TextMeshProUGUI textOne;
    TextMeshProUGUI textTwo;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        textOne = instructionOne.GetComponent<TextMeshProUGUI>();
        textTwo = instructionTwo.GetComponent<TextMeshProUGUI>();
        
        instructionStage = -2;
        instructOne = "WASD to move";
        instructOne2 = "you cannot move onto spaces with no tiles on them";
        instructTwo = "tap to rotate";
        instructTwo2 = "rotating shifts movement to be relative to each clone";
        instructThree = "B to undo";
        instructFinished = "your ready to go (btw press B to undo)";
        instructFinished2 = "they must enter the teleporters at the same time";


        nextText = false;
        nextTextDebounce = true;

        textOne.text = instructOne;
        textTwo.text = instructOne2;
    }

    // Update is called once per frame
    void Update()
    {
        if(inputOpen && !(nextText && nextTextDebounce))
        {
            if((CloneManager2.movingObjects[0].isMoving || CloneManager2.movingObjects[1].isMoving) && instructionStage == 1)
            {
                animator.SetTrigger("next");
                good.SetActive(true);
            }
            else if(CloneManager2.movingObjects[0].isRotating && instructionStage == 2)
            {
                animator.SetTrigger("next");
            }
            else if(CloneManager2.isUndo && instructionStage == 3)
            {
                animator.SetTrigger("next");
            }
            if(CloneManager2.cleared)
            {
                animator.SetTrigger("levelCleared");
            }
        }
        if(nextText && nextTextDebounce)
        {
            nextTextDebounce = false;
            instructionStage+=3;
            if(instructionStage == 2)
            {
                //rotate
                CloneManager2.rotatingIsEnabled = true;
                textOne.text = instructTwo;
                textTwo.text = instructTwo2;

            }
            else if(instructionStage == 3)
            {
                //undo
                CloneManager2.undoingIsEnabled = true;
                textOne.text = instructThree;
                textTwo.text = "";

            }
            else if(instructionStage == 4)
            {
                //done
                textOne.text = instructFinished;
                textTwo.text = instructFinished2;
                animator.SetTrigger("end");
            }
            

        }
        else if(!nextText)
        {
            nextTextDebounce = true;
        }
    }
}
