using System.Collections;
using System.Collections.Generic;
//using UnityEngine.iOS;
using UnityEngine;
using System;


public class CloneManager2 : MonoBehaviour
{

    //public Sprite regclone;
    //public Sprite disabledClone;
    
    public static int[,] cLevelComponents;
    public static Dictionary<Vector2Int, SubObject> cBoxs;
    public GameObject cloneOne;
    public GameObject cloneTwo;
    Animator[] animators;
    

    //storing moves
    List<ObjController[]> moveArchives;

    //detecting input
    public static bool rotatingIsEnabled;
    public static bool undoingIsEnabled;
    public static bool cleared;
    public static bool hint;
    public static int tapCount;

    public static float tapTime;

    public const float tapTimeA = 0.16f;
    public const float tapTimeB = 0.24f;
    public const float tapTimeC = 0.3f;

    float tapTimer;
    Vector2 firstContact;
    Vector2 swipe;
    Vector3 rotation;
    public static Vector2Int[] teleporterCoordinates;

    Touch touch;
    

    //actually moving
    public static bool isUndo;
    public bool dontTeleport;
    private bool[] wasDisabled;
    int angle;
    private float dTime;
    private float distance;
    public float rotateSpeed;
    public float speed;
    public float acceleration;


    //public static List<MovingObject>[]  movingObjects;
    public static ObjController[] movingObjects;

    public GameObject shadeScreen;
    public GameObject levelCleared;
    public GameObject isContinue;
    

    public GameObject counterObj;
    MoveCounter counter;

    ObjController c1;
    ObjController c2;

    static int h;


    void Start()
    {
        //Debug.Log("hello?");
        Debug.Log("start");
        SettingsCycle();
        //undoingIsEnabled = DataManager.gameData.doubleTapToUndo;
        //rotatingIsEnabled = DataManager.gameData.tapToRotate;

        c1 = new ObjController(true, cloneOne, GridBuilder.startCoordinates[0]);
        c2 = new ObjController(true, cloneOne, GridBuilder.startCoordinates[1]);

        animators = new Animator[2]{cloneOne.GetComponent<Animator>(), cloneTwo.GetComponent<Animator>()};
        
        movingObjects = new ObjController[2]{c1, c2};//new List<MovingObject>[2]{new List<MovingObject>(), new List<MovingObject>()};
        //movingObjects[0].Add(c1); movingObjects[1].Add(c2);
        cBoxs = new Dictionary<Vector2Int, SubObject>();
        cLevelComponents = new int[14,11];

        //cLevelComponents = new int[12,9];
        //cBoxs = new Dictionary<Vector2Int, Box>();
        wasDisabled = new bool[2]{false, false};
        
        tapCount = 0;
        isUndo = false;
        cleared = false;
        hint = false;
        distance = 1;
        

        moveArchives = new List<ObjController[]>();
        counter = counterObj.GetComponent<MoveCounter>();

        h = GridBuilder.stageHeight;

        if(DataManager.gameData.openingInstructions)
        {
            rotatingIsEnabled = false;
            undoingIsEnabled = false;
            DataManager.gameData.openingInstructions = false;
            
        }
        else
        {
            rotatingIsEnabled = DataManager.gameData.tapToRotate;
            undoingIsEnabled = DataManager.gameData.doubleTapToUndo;
        }

        SetupClones();

    }
    // Update is called once per frame
    void Update()
    {
        //keyboard controls
        if(CanMove())
        {
            if(Input.GetKeyDown("w") || Input.GetKeyDown("a")
             || Input.GetKeyDown("s") || Input.GetKeyDown("d") || 
             Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)
             || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                //moving
                //counter.Plus();
                movingObjects[0].isMoving = true; movingObjects[1].isMoving = true;
                if(Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    
                    angle = 90;

                }
                else if(Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    
                    angle = -90;

                }
                else if(Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    
                    angle = 180;

                }
                else if(Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    
                    angle = 0;

                }
                CheckForObstructions();
                ArchiveMove();
            }
            else if((Input.GetKeyDown("l") || Input.GetKeyDown("r")) && rotatingIsEnabled)
            {
                //rotating
                Vector3 rotation = Vector3.zero;
                if(Input.GetKeyDown("l"))
                {
                    rotation = new Vector3(0,0,90);
                }
                else if(Input.GetKeyDown("r"))
                {
                    rotation = new Vector3(0,0,-90);
                }
                movingObjects[0].Rotate(rotation); movingObjects[1].Rotate(rotation);
                ArchiveMove();
                
            }
            else if(Input.GetKeyDown("b") /*&& undoingIsEnabled*/)
            {
                //undo move
                UndoMove();

            }
            else if(Input.GetKeyDown("space"))
            {
                Debug.Log("space key pressed");
            }
        }
        //ios controls
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                //firstContact = touch.position;
                tapTimer = tapTime+Time.deltaTime;
                tapCount++;
                //tapHasEnded = false;

            }
            /*
            else if(touch.phase == TouchPhase.Ended)
            {
                tapHasEnded = true;
                swipe = touch.position - firstContact;
            }
            */

        }
        if(tapCount>0)
        {
            
            tapTimer -= Time.deltaTime;

            if(CanMove())
            {
                
                swipe = touch.deltaPosition;//touch.position - firstContact;

                if(swipe.magnitude > 25)//if(tapTimer<0.11 && swipe.magnitude > 26)
                {
                    //moving
                    movingObjects[0].isMoving = true; movingObjects[1].isMoving = true;
                    if(Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                    {

                        //horizontal
                        if(swipe.x > 0)
                        {
                            //right
                            angle = 0;
                        }
                        else
                        {
                            //left
                            angle = 180;

                        }
                    }
                    else //if(Mathf.Abs(swipe.y) > Mathf.Abs(swipe.x)) //its here!!!
                    {
                        //vertical
                        if(swipe.y > 0)
                        {
                            //up
                            angle = 90;
                        }
                        else
                        {
                            //down
                            angle = -90;

                        }
                    }
                    CheckForObstructions();
                    ArchiveMove();
                    tapCount = 0;
                    
                }
                else if(tapCount == 1 && tapTimer < 0)
                {
                    //rotating
                    if(rotatingIsEnabled)
                    {

                        if(touch.position.x < Screen.width/2)
                        {
                            rotation = new Vector3(0,0,90);
                        }
                        else
                        {
                            rotation = new Vector3(0,0,-90);
                        }
                        movingObjects[0].Rotate(rotation); movingObjects[1].Rotate(rotation); 
                        ArchiveMove();
                    }
                    tapCount = 0;
                }
                else if(tapCount > 1)
                {
                    if(undoingIsEnabled)
                    {
                        UndoMove();
                    }
                    tapCount = 0;
                }
            }
        }
        //handles movement
        for(int i = 0; i < 2; i++)
        {
            if(movingObjects[i].isMoving)
            {
                dTime = Time.deltaTime*speed*(1+acceleration*(1-distance));
        
                if(distance<=dTime)
                {
                    dTime = distance;                    
                    movingObjects[i].isMoving = false;                    
                }

                //moves each object together
                foreach(SubObject b in movingObjects[i].AsList())
                {
                    b.obj.transform.Translate(new Vector3(movingObjects[i].nextMove.x,movingObjects[i].nextMove.y,0)*dTime, Space.World);

                }
                if(i==1 || !movingObjects[1].isMoving)
                {
                    //end of move frame
                    if(!movingObjects[0].isMoving && !movingObjects[1].isMoving)
                    {
                        //happens at end of both movements
                        Debug.Log("update coordinates");
                        movingObjects[0].UpdateCoordinates(); movingObjects[1].UpdateCoordinates();

                        distance = 1;

                        isUndo = false;
                        dontTeleport = false;

                        int[] cBlocks = new int[2]{0,0};
                        for(int j = 0; j<2; j++)
                        {
                            SubObject c = movingObjects[j].GetCloneObject();
                            if(c==null)
                            {
                                Debug.Log("no clone here");
                            }
                
                            cBlocks[j] = GridBuilder.currentLevel[h+1-c.blockCoordinates.y, 1+c.blockCoordinates.x];
                            if(j==1 && cBlocks[0] == 3 && cBlocks[1] == 3)
                            {
                                LevelFinished();
                            }
                            else if(!wasDisabled[j] && cBlocks[j] == 7)
                            {
                                movingObjects[j].isDisabled = true;
                                wasDisabled[j] = true;
                                animators[j].SetTrigger("BecameDisabled");

                            }
                            else if(wasDisabled[j] && cBlocks[j] != 7)
                            {
                                movingObjects[j].isDisabled = false;
                                wasDisabled[j] = false;
                                animators[j].SetTrigger("BecameEnabled");
                            }
                        }
                    }
                    else
                    {
                        distance -= dTime;
                    }
                }              
            }
            else if(movingObjects[i].isRotating)
            {
                dTime = Time.deltaTime*rotateSpeed;
                
                if(distance<=dTime)
                {
                    dTime = distance;
                    movingObjects[i].isRotating = false;
                    
                }
                if(i==1)
                {
                    if(!movingObjects[i].isRotating)
                    {
                        //end of movement
                        distance = 1;
                        isUndo = false;
                    }
                    else
                    {
                        distance -= dTime;
                    }
                }

                movingObjects[i].GetCloneObject().obj.transform.Rotate(movingObjects[i].rotation*dTime);

            }
        }
    }
    public static Vector2Int Teleport(Vector2Int coordinates, GameObject g = null)
    {
        Vector2Int coordinates2;
        if(coordinates == teleporterCoordinates[0])
        {
            //move to second item
            coordinates2 = teleporterCoordinates[1];
        }
        else
        {
            //move to first item
            coordinates2 = teleporterCoordinates[0];
        }
        if(g != null)
        {
            g.transform.position = new Vector3(coordinates.x-3-((h/2)-5) ,coordinates.y-5.6f - (h-10) + ((h/2)-5),0);
        }
        return coordinates2;
    }
    void Check(ObjController output)
    {
        
        SubObject obj = output.AsList()[0] as SubObject;
        Vector2Int coordinates = obj.nextCoordinates;
        Vector2Int coordinates2 = coordinates+output.nextMove;

        
        int currentBlock = GridBuilder.currentLevel[h+1-coordinates.y, 1+coordinates.x];
        int componentBlock = cLevelComponents[h+1-coordinates.y, 1+coordinates.x];
        
        if(currentBlock==2 || output.isDisabled)
        {
            //given clone hit a block
            Debug.Log("obstruction");
            output.isMoving = false;
            output.SetNextCoordinates(Vector2Int.zero);

            return;
        }
        if(currentBlock==0)
        {
            Debug.Log("illegal move because of void square");
            
            if(output == movingObjects[0] || output == movingObjects[1])
            {
                //outputs to base
                movingObjects[0].isMoving = false; movingObjects[1].isMoving = false;
                movingObjects[0].SetNextCoordinates(Vector2Int.zero); movingObjects[1].SetNextCoordinates(Vector2Int.zero);
            }
            else
            {
                //outputs to a sublist
                output.isMoving = false;
                output.ChangeLevelComponents(1);
            }
            
            return;
        }
        if(componentBlock==1)
        {
            //given block hit a movable object
            int nextBlock = GridBuilder.currentLevel[h+1-coordinates2.y, 1+coordinates2.x];

            if(nextBlock != 0 && nextBlock != 4 && nextBlock != 14 && nextBlock != 5 && nextBlock != 6 && nextBlock != 7)
            {
                Debug.Log("im sick :(");
                //this chain wont move
                foreach(MovingObject b in output.subList)
                {
                    b.SetNextCoordinates(Vector2Int.zero);
                }
                if(output != movingObjects[0] && output != movingObjects[1])
                {
                    output.ChangeLevelComponents(1);
                }
                output.isMoving = false;

                return;
            }
            else
            {
                //check next square
                Debug.Log("hit movable object");
                
                SubObject b = cBoxs[coordinates];
                b.SetNextCoordinates(output.nextMove);
                
                cLevelComponents[h+1-coordinates.y, 1+coordinates.x] = 0;
                cBoxs.Remove(coordinates);
                              
                output.subList.Insert(0, b);

                Check(output);

            }            
        }
        if(currentBlock==4 && !obj.isTeleport && output.isMoving)
        {
            //this object is on a teleporter
            Debug.Log("on teleporter");
            Vector2Int coordinates3 = Teleport(coordinates);
            
            ObjController subClone = new ObjController(false, obj.obj, obj.blockCoordinates);
        
            (subClone.subList[0] as SubObject).isClone = true;
            (subClone.subList[0] as SubObject).isTeleport = true;
            (subClone.subList[0] as SubObject).nextCoordinates = coordinates3;
            //(subClone.subList[0] as SubObject).isClone = obj.isClone;

            subClone.isMoving = true;
            subClone.nextMove = output.nextMove;

            Check(subClone);
            
            if(subClone.isMoving)
            {
                output.subList.Remove(obj);
                ObjController c = new ObjController();
                c.subList = new List<MovingObject>(output.subList);
                output.subList.Clear();
                output.subList.Add(subClone);
                output.subList.Add(c);
                
            }
        } 
    }
    //check for complications of move
    void CheckForObstructions()
    {
        
        for(int i = 0; i<2; i++)
        {
            
            SubObject c = movingObjects[i].GetCloneObject();
            movingObjects[i].subList.Clear();
            movingObjects[i].subList.Add(c);
            
        }
        for(int i = 0; i < 2; i++)
        {
            GameObject obj = movingObjects[i].GetCloneObject().obj;
            movingObjects[i].SetNextCoordinates(new Vector2Int(Mathf.RoundToInt(Mathf.Cos((obj.transform.eulerAngles.z+angle)*Mathf.Deg2Rad)),
            Mathf.RoundToInt(Mathf.Sin((obj.transform.eulerAngles.z+angle)*Mathf.Deg2Rad))));
            
        }
        if(movingObjects[0].GetCloneObject().nextCoordinates == movingObjects[1].GetCloneObject().blockCoordinates
        && movingObjects[1].GetCloneObject().nextCoordinates == movingObjects[0].GetCloneObject().blockCoordinates)
        {
            Debug.Log("Move canceled because of ghosting");
            movingObjects[0].isMoving = false; movingObjects[1].isMoving = false;

        }
        for(int i = 0; i < 2; i++)
        {
            
            Check(movingObjects[i]);
            
        }
        //ObjController r1 = movingObjects[0]; ObjController r2 = movingObjects[1];
        ///*
        if(CheckForCollision(movingObjects))// || CheckForCollision(new ObjController[2]{movingObjects[0], movingObjects[0]}) || CheckForCollision(new ObjController[2]{movingObjects[1], movingObjects[1]}))
        {
            Debug.Log("illegal move because of collision");
            movingObjects[0].isMoving = false; movingObjects[1].isMoving = false;
        }
        //*/
        for(int i = 0; i<2; i++)
        {
            if(wasDisabled[i])
            {
                movingObjects[i].isDisabled = true;
            }
        }
        if(!movingObjects[0].isMoving && !movingObjects[1].isMoving)
        {
            movingObjects[0].ChangeLevelComponents(1); movingObjects[1].ChangeLevelComponents(1);
        }
                
    }
    public bool CheckForCollision(ObjController[] m)
    {
        for(int i = 0; i<2; i++)
        {
            Debug.Log("hello?");
            Debug.Log(m[i].subList.Count);
            if(m[i].subList.Count == 0)
            {
                Debug.Log("empty sublist");
                return false;
            }
            if(m[i].subList[0] is ObjController)
            {
                Debug.Log("found a subList");
                ObjController c = m[i].subList[0] as ObjController;
                if(CheckForCollision(new ObjController[2]{c, m[1-i]}) || CheckForCollision(new ObjController[2]{c, m[i].subList[1] as ObjController}))
                {
                    Debug.Log("sublist canceled");
                    c.ChangeLevelComponents(1);
                    if(m[1-i].isMoving)
                    {
                        Check(m[1-i]);
                    }
                    SubObject myObj = c.subList[c.subList.Count-1] as SubObject;
                    myObj.isTeleport = false;
                    myObj.SetNextCoordinates(movingObjects[i].nextMove);
                    m[i].subList[0] = myObj;
                }
                break;
            }
        }
        foreach(SubObject b in m[0].AsMainList())
        {
            foreach(SubObject c in m[1].AsMainList())
            {
                
                if(b.nextCoordinates == c.nextCoordinates && b != c)
                {
                    Debug.Log("COLLISION");
                    for(int i = 0; i<2; i++)
                    {
                        if(m[i].isDisabled)
                        {
                            m[i].isDisabled = false;
                            m[i].isMoving = true;
                            m[i].SetNextCoordinates(m[1-i].nextMove);
                            Check(m[i]);                         
                            return CheckForCollision(m);
                        }
                    }
                    return true;
                }
            }
        }
        return false;
    }
    public void UndoMove()
    {
        //undo move
        if(moveArchives.Count != 0 && CanMove())
        {
            movingObjects = moveArchives[0];

            if(movingObjects[0].isMoving || movingObjects[1].isMoving)
            {
                counter.Minus();
                movingObjects[0].ChangeLevelComponents(0);
                movingObjects[1].ChangeLevelComponents(0);
            }
            moveArchives.RemoveAt(0);
            //nextMoves
        }
        for(int i = 0; i<2; i++)
        {
            foreach(SubObject b in movingObjects[i].AsList())
            {
                b.CheckForTeleporter(b.nextCoordinates+movingObjects[i].nextMove*-1);
            }
        }


        //for button command
        tapCount = 0;

    }
    public void SetupClones()
    {
        
        distance = 1;
        teleporterCoordinates = GridBuilder.subTeleporterCoordinates;

        animators[0].SetTrigger("Reset");
        animators[1].SetTrigger("Reset");
        wasDisabled[0] = false;
        wasDisabled[1] = false;

        cLevelComponents = CopyArray(GridBuilder.levelComponents);
        //copies dictionary
        cBoxs = new Dictionary<Vector2Int, SubObject>();
        
        foreach(KeyValuePair<Vector2Int, SubObject> e in GridBuilder.boxs)
        {
            SubObject cBox = new SubObject(e.Value.obj, e.Value.blockCoordinates);
            cBoxs.Add(e.Key, cBox);
        }
        
        for(int x=0; x<h; x++)
        {
            for(int y=0; y<h-3; y++)
            {
                if(cLevelComponents[x+1, y+1] == 1)
                {
                    cBoxs[new Vector2Int(y, h-x)].obj.transform.position = new Vector3(y-3-((h/2)-5),-x+4.4f+((h/2)-5),1);//4.25
                }
            }
        }
        c1 = new ObjController(true, cloneOne, GridBuilder.startCoordinates[0]);
        c2 = new ObjController(true, cloneTwo, GridBuilder.startCoordinates[1]);
      
        movingObjects = new ObjController[2]{c1, c2};        

        movingObjects[0].GetCloneObject().obj.transform.position = new Vector3(GridBuilder.startCoordinates[0].x-3-((h/2)-5) ,GridBuilder.startCoordinates[0].y-5.6f-(h-10)+((h/2)-5),0);
        movingObjects[1].GetCloneObject().obj.transform.position = new Vector3(GridBuilder.startCoordinates[1].x-3-((h/2)-5) ,GridBuilder.startCoordinates[1].y-5.6f-(h-10)+((h/2)-5),0);
        movingObjects[0].GetCloneObject().obj.transform.eulerAngles = new Vector3(0, 0, 360-GridBuilder.startRotations[0]);
        movingObjects[1].GetCloneObject().obj.transform.eulerAngles = new Vector3(0, 0, 360-GridBuilder.startRotations[1]);
        tapCount = 0;
        counter.ResetCounter();
        moveArchives.Clear();

    }
    public void LevelFinished()
    {
        Debug.Log("stage cleared");
        cleared = true;
        shadeScreen.SetActive(true);
        levelCleared.GetComponent<LevelCleared>().MoveText();
        
        int currentMoves = counter.numOfMoves;
        int movesIndex;
        if(GridBuilder.levelIndex == 97)
        {
            movesIndex = 11;
        }
        else
        {
            movesIndex = GridBuilder.levelIndex;
        }
        int savedMoves = DataManager.gameData.savedMoves[movesIndex];
        if(currentMoves < savedMoves || savedMoves == 0)
        {
            Debug.Log("system has saved");
            DataManager.gameData.savedMoves[movesIndex] = currentMoves;
            SaveSystem.SaveData();
        }
        if(GridBuilder.levelIndex == 10)
        {
            isContinue.SetActive(false);

            if(DataManager.gameData.savedMoves[11] == 0)
            {
                hint = true;    
            }
            
        }
        else if((GridBuilder.levelIndex == 9 && !UIManager.CheckIfLevelElevenIsOpen()) || GridBuilder.levelIndex == 97)
        {
            isContinue.SetActive(false);
        }
        else
        {
            isContinue.SetActive(true);
        }
        
        gameObject.SetActive(false);
    }
    bool CanMove()
    {
        bool b = true;
        ObjController c1 = movingObjects[0];
        ObjController c2 = movingObjects[1];

        if(c1.isMoving || c2.isMoving || c1.isRotating || c2.isRotating)
        {
            b = false;
        }
        return b;
    }
    void OnEnable()
    {
        tapCount = 0;
    }
    int[,] CopyArray(int[,] x)
    {
        int[,] myArry = new int[h+2,h-1];
        
        for(int i = 0; i<h+2; i++)
        {
            for(int j = 0; j<h-1; j++)
            {
                myArry[i,j] = x[i,j];
            }
        }
        
        return myArry;
    }
    void ArchiveMove()
    {
        if(!CanMove())
        {
            if(movingObjects[0].isMoving || movingObjects[1].isMoving)
            {
                counter.Plus();
            }
            
            moveArchives.Insert(0, new ObjController[2]{CopyObject(movingObjects[0]), CopyObject(movingObjects[1])});
        }
    }
    ObjController CopyObject(ObjController c)
    {
        ObjController myClone = new ObjController();

        myClone.isMoving = c.isMoving;
        myClone.isRotating = c.isRotating;
        myClone.isDisabled = c.isDisabled;
        myClone.rotation = c.rotation*-1;
        myClone.nextMove = c.nextMove*-1;

        foreach(SubObject b in c.AsList())
        {
            SubObject myObject = new SubObject(b.obj, b.nextCoordinates);
            
            myObject.isTeleport = b.isTeleport;
            myObject.nextCoordinates = b.blockCoordinates;
            myObject.isClone = b.isClone;
            
            myClone.subList.Add(myObject);
            
            
        }
        return myClone;
    }
    public static void SettingsCycle()
    {
        Debug.Log("running settings cycle");
        undoingIsEnabled = DataManager.gameData.doubleTapToUndo;
        rotatingIsEnabled = DataManager.gameData.tapToRotate;
        
        int n = DataManager.gameData.doubleTapSpeed;
        if(n == 0)
        {
            tapTime = tapTimeA;
            Debug.Log("bitconnect");
        }
        else if(n==1)
        {
            tapTime = tapTimeB;
        }
        else if(n==2)
        {
            tapTime = tapTimeC;
        }
    }
}