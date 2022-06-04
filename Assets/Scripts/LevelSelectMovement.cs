using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMovement : MonoBehaviour
{
    
    bool isMoving;
    public static bool freeze;
    public bool freeAspectMode;
    public static int index;
    int direction;
    float distance;
    float distanceRemaining;
    float dTime;
    const float scalingRate = 0.039473f;
    public float speed;
    public float pullBackSpeed;

    public static int[] savedMoves;

    Touch touch;

    public GameObject outerLeftBoundry;
    public GameObject outerRightBoundry;
    public GameObject leftBoundry;
    public GameObject rightBoundry;
    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public GameObject six;
    public GameObject seven;
    public GameObject eight;
    public GameObject nine;
    public GameObject ten;
    public GameObject eleven;
    //public GameObject twelve;
    //public GameObject thirteen;
    //public GameObject fourteen;
    //public GameObject fifteen;
    public GameObject zeroOuterLeft;
    public GameObject zeroLeft;
    public GameObject zero;
    public GameObject zeroOuterRight;
    public GameObject zeroRight;

    

    public GameObject musicPlayer;
    public AudioClip hiddenLevelMusic;
    public AudioClip regMusic;
    private AudioSource source;
    int revConstant;

    public GameObject[] allLevels;
    public static GameObject[] levelsInQueue;

    public Button startButton;


    // Start is called before the first frame update
    void Start()
    {
        source = musicPlayer.GetComponent<AudioSource>();
        freeze = false;
        
        allLevels = new GameObject[]{outerLeftBoundry, leftBoundry, one, two, three, four, five, six, 
        seven, eight, nine, ten, eleven, rightBoundry, outerRightBoundry};
        levelsInQueue = new GameObject[5];
        //levelsInQueue = new GameObject[]{leftBoundry, one, two, three, four};
        //index = 2;
        if(index == 0)
        {
            index = 2;
        }
        if(index == 99)
        {
            source.clip = hiddenLevelMusic;
            source.Play();
            
            levelsInQueue[0] = zeroOuterLeft;
            levelsInQueue[1] = zeroLeft;
            levelsInQueue[2] = zero;
            levelsInQueue[3] = zeroRight;
            levelsInQueue[4] = zeroOuterRight;

            for(int i = 0; i<11; i++)
            {
                allLevels[i].transform.position = new Vector3(7.6f,0,0);
                allLevels[i].transform.localScale  = new Vector3(0.45f, 0.45f, 1);
            }

            allLevels[3].transform.position = new Vector3(11.4f,0,0);
            allLevels[3].transform.localScale  = new Vector3(0.3f, 0.3f, 1);
            allLevels[4].transform.position = new Vector3(15.2f,0,0);
            allLevels[4].transform.localScale  = new Vector3(0.15f, 0.15f, 1);

            revConstant = -1;

        }
        else
        {
            for(int i = 0; i<5; i++)
            {
                levelsInQueue[i] = allLevels[index-2+i];
                
            }
            //each below index
            for(int i = 0; i<index-1; i++)
            {
                allLevels[i].transform.position = new Vector3(-7.6f,0,0);
                allLevels[i].transform.localScale = new Vector3(0.45f, 0.45f, 1);
                
                
            }
            //each above index
            for(int i = index+2; i<15; i++)
            {
                allLevels[i].transform.position = new Vector3(7.6f,0,0);
                allLevels[i].transform.localScale = new Vector3(0.45f, 0.45f, 1);
                

            }

            revConstant = 1;
        }
        
        levelsInQueue[1].transform.position = new Vector3(-3.8f,0,0);
        levelsInQueue[2].transform.position = new Vector3(0,0,0);
        levelsInQueue[3].transform.position = new Vector3(3.8f,0,0);

        levelsInQueue[1].transform.localScale = new Vector3(0.6f,0.6f,1);
        levelsInQueue[2].transform.localScale = new Vector3(0.75f,0.75f,1);
        levelsInQueue[3].transform.localScale = new Vector3(0.6f,0.6f,1);
        

        for(int i = 0; i<12; i++)
        {
            LevelBlock blockData;
            if(i==11)
            {
                blockData = zero.GetComponent<LevelBlock>();
            }
            else
            {
                blockData = allLevels[i+2].GetComponent<LevelBlock>();
            }
            blockData.numOfMoves = DataManager.gameData.savedMoves[i];
            blockData.isPlatinum = DataManager.gameData.platinumStars[i];
            if(blockData.minMoves > blockData.numOfMoves && blockData.numOfMoves != 0)
            {
                DataManager.gameData.platinumStars[i] = true;
                SaveSystem.SaveData();
            }
            blockData.Setup();
            
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d"))
        {
            Debug.Log("Right Arrow");
            ShiftForward();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a"))
        {
            Debug.Log("Left Arrow");
            ShiftBackward();
        }
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                isMoving= false;
            }
            if((distance<-1.9 && index+3<=14) || (distance>1.9 && index-3>=0 && index != 99))
            {
                //shift
                if(distance<-1.9)
                {
                    //shift forward
                    index++;
                    distance = distance+3.8f;
                    allLevels[index+2].transform.position += new Vector3(distance,0,0);
                    allLevels[index+2].transform.localScale = new Vector3(0.45f-distance*scalingRate, 0.45f-distance*scalingRate, 0);
                    levelsInQueue[0].transform.localScale = new Vector3(0.45f, 0.45f, 0);
                    levelsInQueue[0].transform.position = new Vector3(-7.6f,0,0);

                }
                else if(distance>1.9)
                {
                    //shift backward
                    index--;
                    distance = distance-3.8f;
                    allLevels[index-2].transform.position += new Vector3(distance,0,0);
                    allLevels[index-2].transform.localScale = new Vector3(0.45f+distance*scalingRate, 0.45f+distance*scalingRate, 0);
                    levelsInQueue[4].transform.localScale = new Vector3(0.45f, 0.45f, 0);
                    levelsInQueue[4].transform.position = new Vector3(7.6f,0,0);

                }

                levelsInQueue[0] = allLevels[index-2];
                levelsInQueue[1] = allLevels[index-1];
                levelsInQueue[2] = allLevels[index];
                levelsInQueue[3] = allLevels[index+1];
                levelsInQueue[4] = allLevels[index+2];
                
            }
            else if((index-3 == -1 || index == 99) && distance*revConstant>1100)
            {
                
                Debug.Log("found hidden level");
                
                distance = distance - (1100+7.6f)*revConstant;

                levelsInQueue[2].transform.position = new Vector3(7.6f*revConstant,0,0);
                levelsInQueue[2].transform.localScale = new Vector3(0.45f, 0.45f, 0);

                levelsInQueue[2+revConstant].transform.position = new Vector3(11.4f*revConstant,0,0);
                levelsInQueue[2+revConstant].transform.localScale = new Vector3(0.3f, 0.3f, 0);

                levelsInQueue[2+2*revConstant].transform.position = new Vector3(15.2f*revConstant,0,0);
                levelsInQueue[2+2*revConstant].transform.localScale = new Vector3(0.15f, 0.15f, 0);

                if(revConstant==-1)
                {
                    
                    index = 2;
                    levelsInQueue[0] = allLevels[0];
                    levelsInQueue[1] = allLevels[1];
                    levelsInQueue[2] = allLevels[2];
                    levelsInQueue[3] = allLevels[3];
                    levelsInQueue[4] = allLevels[4];
                }
                else
                {
                    
                    index = 99;
                    levelsInQueue[0] = zeroOuterLeft;
                    levelsInQueue[1] = zeroLeft;
                    levelsInQueue[2] = zero;
                    levelsInQueue[3] = zeroRight;
                    levelsInQueue[4] = zeroOuterRight;
                }
                levelsInQueue[2].transform.position += new Vector3(7.6f*revConstant+distance,0,0);
                levelsInQueue[2].transform.localScale = new Vector3(0.45f + (7.6f+distance*revConstant)*scalingRate, 0.45f + (7.6f+distance*revConstant)*scalingRate, 0);
                levelsInQueue[2-revConstant].transform.position += new Vector3(7.6f*revConstant+distance,0,0);
                levelsInQueue[2-revConstant].transform.localScale = new Vector3(0.3f + (7.6f+distance*revConstant)*scalingRate, 0.3f + (7.6f+distance*revConstant)*scalingRate, 0);
                levelsInQueue[2-2*revConstant].transform.position += new Vector3(7.6f*revConstant+distance,0,0);
                levelsInQueue[2-2*revConstant].transform.localScale = new Vector3(0.15f + (7.6f+distance*revConstant)*scalingRate, 0.15f + (7.6f+distance*revConstant)*scalingRate, 0);
                /*
                levelsInQueue[2+revConstant].transform.position += new Vector3(7.6f*revConstant+distance,0,0);
                levelsInQueue[2+revConstant].transform.localScale = new Vector3(0.45f + (7.6f+distance*revConstant)*scalingRate, 0.45f + (7.6f+distance*revConstant)*scalingRate, 0);
                levelsInQueue[2+2*revConstant].transform.position += new Vector3(7.6f+distance*revConstant,0,0);
                levelsInQueue[2+2*revConstant].transform.localScale = new Vector3(0.45f + (7.6f+distance*revConstant)*scalingRate, 0.45f + (7.6f+distance*revConstant)*scalingRate, 0);
                */
                revConstant*=-1;
                
                
            }
            if(touch.phase == TouchPhase.Ended)
            {
                startButton.interactable = false;
                isMoving = true;
                
                direction = (int)Mathf.Sign(distance);
            }
            else if(!freeze)
            {
                dTime = touch.deltaPosition.x*speed;

                scaleLevels();

                levelsInQueue[0].transform.Translate(new Vector2(dTime,0));
                levelsInQueue[1].transform.Translate(new Vector2(dTime,0));
                levelsInQueue[2].transform.Translate(new Vector2(dTime,0));
                levelsInQueue[3].transform.Translate(new Vector2(dTime,0));
                levelsInQueue[4].transform.Translate(new Vector2(dTime,0));

                distance+=dTime;
            }

        }
        if(isMoving && !freeze)
        {
            
            dTime = -direction*Time.deltaTime*pullBackSpeed;


            if(Mathf.Abs(distance) < Mathf.Abs(dTime))
            {
                //has fully reset
                dTime = -distance;
                distance = 0;
                isMoving = false;
                startButton.interactable = true;


            }
            else
            {
                distance += dTime;
            }

            scaleLevels();

            levelsInQueue[0].transform.Translate(new Vector2(dTime, 0));
            levelsInQueue[1].transform.Translate(new Vector2(dTime, 0));
            levelsInQueue[2].transform.Translate(new Vector2(dTime, 0));
            levelsInQueue[3].transform.Translate(new Vector2(dTime, 0));
            levelsInQueue[4].transform.Translate(new Vector2(dTime, 0));


            
        }
        if(distance*revConstant>100)
        {
            
            if(distance*revConstant > 1000)
            {
                if((distance - dTime)*revConstant < 1000)
                {
                    AudioListener.volume = 1;
                }
            }
            else if(distance*revConstant > 600)
            {
                AudioListener.volume += revConstant*dTime/400;

                if((distance - dTime)*revConstant < 600)
                {
                    if(revConstant == -1)
                    {
                        source.clip = regMusic;
                    }
                    else
                    {
                        source.clip = hiddenLevelMusic;
                    }
                    source.Play();
                }
                
            }
            else if(distance*revConstant < 500)
            {
                AudioListener.volume -= revConstant*dTime/400;

                if((distance - dTime)*revConstant > 500)
                {
                    if(revConstant == -1)
                    {
                        source.clip = hiddenLevelMusic;
                    }
                    else
                    {
                        source.clip = regMusic;
                    }
                    source.Play();
                }
                if((distance + dTime)*revConstant < 100)
                {
                    AudioListener.volume = 1;
                }

            }
            
        }
        
    }
    void scaleLevels()
    {
        

        for(int i = 0; i<5; i++)
        {
            float rate = (Mathf.Abs(levelsInQueue[i].transform.position.x)
            -Mathf.Abs(levelsInQueue[i].transform.position.x+dTime))*scalingRate;
            
            levelsInQueue[i].transform.localScale += new Vector3(rate, rate, 0);
        }
    }
    void ShiftForward()
    {
        if(index == 12 || isMoving || index==99)
        {
            return;
        }
        index++;
        distance = 3.8f;
        direction = 1;
        isMoving = true;

        //allLevels[index+1].SetActive(true);
        //allLevels[index-2].SetActive(false);
        allLevels[index+2].transform.position += new Vector3(distance,0,0);
        allLevels[index+2].transform.localScale = new Vector3(0.45f-distance*scalingRate, 0.45f-distance*scalingRate, 0);
        levelsInQueue[0].transform.localScale = new Vector3(0.45f, 0.45f, 0);
        levelsInQueue[0].transform.position = new Vector3(-7.6f,0,0);

        levelsInQueue[0] = allLevels[index-2];
        levelsInQueue[1] = allLevels[index-1];
        levelsInQueue[2] = allLevels[index];
        levelsInQueue[3] = allLevels[index+1];
        levelsInQueue[4] = allLevels[index+2];
    }
    void ShiftBackward()
    {
        if(index==2 || isMoving || index==99)
        {
            return;
        }
        index--;
        distance = -3.8f;
        direction = -1;
        isMoving = true;

        //allLevels[index-1].SetActive(true);
        //allLevels[index+2].SetActive(false);
        allLevels[index-2].transform.position += new Vector3(distance,0,0);
        allLevels[index-2].transform.localScale = new Vector3(0.45f+distance*scalingRate, 0.45f+distance*scalingRate, 0);
        levelsInQueue[4].transform.localScale = new Vector3(0.45f, 0.45f, 0);
        levelsInQueue[4].transform.position = new Vector3(7.6f,0,0);

        levelsInQueue[0] = allLevels[index-2];
        levelsInQueue[1] = allLevels[index-1];
        levelsInQueue[2] = allLevels[index];
        levelsInQueue[3] = allLevels[index+1];
        levelsInQueue[4] = allLevels[index+2];
    }

}
