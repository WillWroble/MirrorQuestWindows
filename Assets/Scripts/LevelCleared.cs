using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCleared : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public float acceleration;

    float maxDistance;
    float distance;
    float dTime;
    static bool isMoving;

    public GameObject myCanvas;

    public GameObject nextLevel;

    public GameObject cloneManager;

    void Start()
    {
        //float canvasWidth = (1334f*Screen.width)/Screen.height;
        //float canvasScale;
        //SetupEndScreen();
        //transform.localPosition = new Vector3((canvasWidth/2)+400, 60, 0);
        //nextLevel.transform.localPosition = new Vector3(-(canvasWidth/2)-400, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
        if(isMoving)
        {
            dTime = speed*Time.deltaTime*maxDistance*(1+acceleration*(1-distance/maxDistance));
            float canvasScale = 1f;//myCanvas.transform.localScale.x;

            if(distance < dTime)
            {
                dTime = distance;
                //distance = 7;
                isMoving = false;
                //put more stuff here
            }
            else
            {
                distance -= dTime;
            }
            transform.Translate(Vector3.left*dTime*canvasScale);
            nextLevel.transform.Translate(Vector3.right*dTime*canvasScale);

        }
    }
    public void MoveText()
    {
        SetupEndScreen();
        isMoving = true;
    }
    void SetupEndScreen()
    {
        float canvasScale = myCanvas.transform.localScale.x;
        Debug.Log(canvasScale);
       
        maxDistance = Screen.width/2f+400f*canvasScale;
        distance = maxDistance;

        transform.position = new Vector3(Screen.width+400*canvasScale, (Screen.height/2f)+60*canvasScale, 0);
        nextLevel.transform.position = new Vector3(-400*canvasScale, Screen.height/2f, 0);
    }
}
