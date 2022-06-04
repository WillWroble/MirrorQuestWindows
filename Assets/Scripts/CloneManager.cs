using System.Collections;
using System.Collections.Generic;
//using UnityEngine.iOS;
using UnityEngine;
using System;


public class CloneManager : MonoBehaviour
{

    int[,] cLevelComponents;
    Dictionary<Vector2Int, GameObject> cMovableBoxs;
    Dictionary<GameObject, Vector2Int> cSliderCoordinates;
    //storing moves
    List<Tuple<bool, bool, bool, Vector2Int[], Vector3, List<GameObject>[], bool>> moveArchives;

    //detecting input
    public static bool rotatingIsEnabled;
    public static bool undoingIsEnabled;
    public static bool cleared;
    public static int tapCount;
    public float tapTime;
    float tapTimer;
    Vector2 firstContact;
    Vector2 swipe;
    Vector3 direction;
    Vector3 rotation;
    public Vector2Int[] nextCoordinates;
    public Vector2Int[] blockCoordinates;
    public Vector2Int[] teleporterCoordinates;
    public static bool[] cloneMovements;
    public static bool[] cloneRotations;
    public GameObject[] clones;
    public Vector2Int[] nextMoves;
    public int[] test;
    Touch touch;
    

    //actually moving
    public static bool isUndo;
    public bool dontTeleport;
    int angle;
    private float dTime;
    private float distance;
    public float rotateSpeed;
    public float speed;
    public float acceleration;


    public List<GameObject>[]  movingObjects;

    public GameObject shadeScreen;
    public GameObject levelCleared;
    

    public GameObject counterObj;
    MoveCounter counter;


    void Start()
    {
        //Debug.Log("hello?");
        Debug.Log("start");
        undoingIsEnabled = DataManager.gameData.doubleTapToUndo;
        rotatingIsEnabled = DataManager.gameData.tapToRotate;
        
        tapCount = 0;
        isUndo = false;
        dontTeleport = false;
        cleared = false;
        clones = new GameObject[]{GameObject.Find("CloneOne"), GameObject.Find("CloneTwo")};
        //blockCoordinates = new Vector2Int[]{new Vector2Int(3,6), new Vector2Int(4,6)};
        //blockCoordinates = new Vector2Int[2];
        nextCoordinates = new Vector2Int[2];
        cloneMovements = new bool[]{false, false};
        cloneRotations = new bool[]{false, false};
        nextMoves = new Vector2Int[]{Vector2Int.up, Vector2Int.up};
        distance = 1;//new float[]{1, 1};
        
        movingObjects = new List<GameObject>[2]{new List<GameObject>(), new List<GameObject>()};
        moveArchives = new List<Tuple<bool, bool, bool, Vector2Int[], Vector3, List<GameObject>[], bool>>();

        counter = counterObj.GetComponent<MoveCounter>();

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
        if(!cloneMovements[0] && !cloneMovements[1] && !cloneRotations[0] && !cloneRotations[1])
        {
            if(Input.GetKeyDown("w") || Input.GetKeyDown("a")
             || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
            {
                //moving
                //counter.Plus();
                MoveClones(true);
                if(Input.GetKeyDown("w"))
                {
                    
                    direction = Vector3.up;
                    angle = 90;

                }
                else if(Input.GetKeyDown("s"))
                {
                    
                    direction = Vector3.down;
                    angle = -90;

                }
                else if(Input.GetKeyDown("a"))
                {
                    
                    direction = Vector3.left;
                    angle = 180;

                }
                else if(Input.GetKeyDown("d"))
                {
                    
                    direction = Vector3.right;
                    angle = 0;

                }
                CheckForObstructions();
                
            }
            else if((Input.GetKeyDown("l") || Input.GetKeyDown("r")) && rotatingIsEnabled)
            {
                //rotating
                RotateClones(true);
                if(Input.GetKeyDown("l"))
                {
                    rotation = new Vector3(0,0,90);
                }
                else if(Input.GetKeyDown("r"))
                {
                    rotation = new Vector3(0,0,-90);
                }

                moveArchives.Insert(0, new Tuple<bool, bool, bool, Vector2Int[], Vector3, List<GameObject>[], bool>
                (false, false, false, new Vector2Int[2], rotation*-1, new List<GameObject>[2]{new List<GameObject>(), new List<GameObject>()}, false));
            }
            else if(Input.GetKeyDown("b") && undoingIsEnabled)
            {
                //undo move
                UndoMove();

            }
            else if(Input.GetKeyDown("space"))
            {
                RevealArchive(0);
                RevealArchive(1);

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

            if(!cloneMovements[0] && !cloneMovements[1] && !cloneRotations[0] && !cloneRotations[1])
            {
                
                swipe = touch.deltaPosition;//touch.position - firstContact;

                if(swipe.magnitude > 25)//if(tapTimer<0.11 && swipe.magnitude > 26)
                {
                    //moving
                    MoveClones(true);
                    if(Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                    {

                        //horizontal
                        if(swipe.x > 0)
                        {
                            //right
                            direction = new Vector3(1,0,0);
                            angle = 0;
                        }
                        else
                        {
                            //left
                            direction = new Vector3(-1,0,0);
                            angle = 180;

                        }
                    }
                    else //if(Mathf.Abs(swipe.y) > Mathf.Abs(swipe.x)) //its here!!!
                    {
                        //vertical
                        if(swipe.y > 0)
                        {
                            //up
                            direction = new Vector3(0,1,0);
                            angle = 90;
                        }
                        else
                        {
                            //down
                            direction = new Vector3(0,-1,0);
                            angle = -90;

                        }
                    }
                    CheckForObstructions();
                    tapCount = 0;
                    
                }
                else if(tapCount == 1 && tapTimer < 0)
                {
                    //rotating
                    if(rotatingIsEnabled)
                    {
                        RotateClones(true);

                        if(touch.position.x < Screen.width/2)
                        {
                            rotation = new Vector3(0,0,90);
                        }
                        else
                        {
                            rotation = new Vector3(0,0,-90);
                        }

                        moveArchives.Insert(0, new Tuple<bool, bool, bool, Vector2Int[], Vector3, List<GameObject>[], bool>
                        (false, false, false, new Vector2Int[2], rotation*-1, new List<GameObject>[2]{new List<GameObject>(), new List<GameObject>()}, false));
                    }
                    tapCount = 0;
                }
                else if(tapCount > 1 && undoingIsEnabled)
                {
                    UndoMove();
                    tapCount = 0;
                }
            }
        }
        //handles movement
        for(int i = 0; i < 2; i++)
        {
            if(cloneMovements[i])
            {
                
                dTime = Time.deltaTime*speed*(1+acceleration*(1-distance));
        
                if(distance<=dTime)
                {
                    dTime = distance;
                    blockCoordinates[i] += nextMoves[i];

                    cloneMovements[i] = false;
                    
                }

                clones[i].transform.Translate(direction*dTime);

                if(!cloneMovements[i] && GridBuilder.currentLevel[11-blockCoordinates[i].y, 1+blockCoordinates[i].x] == 4 && !isUndo && !dontTeleport)
                {    
                    blockCoordinates[i] = Teleport(i, blockCoordinates[i], clones[i]); 
                }

                foreach(GameObject b in movingObjects[i])
                {
                    Vector2Int coordinates = cSliderCoordinates[b];
                    cMovableBoxs.Remove(coordinates);
                    coordinates = indexConvert(coordinates);
                    cLevelComponents[coordinates.x, coordinates.y] = 0;
                    
                }
                foreach(GameObject b in movingObjects[i])
                {
                    b.transform.Translate(new Vector3(nextMoves[i].x,nextMoves[i].y,0)*dTime, Space.World);
                    
                    if(!cloneMovements[i])
                    {
                        //happens at end
                        Vector2Int coordinates = cSliderCoordinates[b];
                        Vector2Int coordinates2 = coordinates+nextMoves[i];

                        cSliderCoordinates.Remove(b);

                        if(GridBuilder.currentLevel[11-coordinates2.y, 1+coordinates2.x] == 4 && !isUndo)
                        {
                            coordinates2 = Teleport(i, coordinates2, b);
                        }
                        
                        cMovableBoxs.Add(coordinates2, b);
                        cSliderCoordinates.Add(b, coordinates2);

                        coordinates2 = indexConvert(coordinates2);
                        cLevelComponents[coordinates2.x, coordinates2.y] = 1;

                    }

                }
                if(i==1 || !cloneMovements[1])
                {
                    if(!cloneMovements[0] && !cloneMovements[1])
                    {
                        //happens at end of both movements
                        distance = 1;

                        isUndo = false;
                        dontTeleport = false;

                        if(GridBuilder.currentLevel[11-blockCoordinates[0].y, 1+blockCoordinates[0].x] == 3
                        && GridBuilder.currentLevel[11-blockCoordinates[1].y, 1+blockCoordinates[1].x] == 3)
                        {
                            LevelFinished();
                        }
                    }
                    else
                    {
                        distance -= dTime;
                    }
                }              
            }
            else if(cloneRotations[i])
            {
                dTime = Time.deltaTime*rotateSpeed;
                
                if(distance<=dTime)
                {
                    dTime = distance;
                    cloneRotations[i] = false;
                    
                }
                if(i==1)
                {
                    if(!cloneRotations[0])
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

                clones[i].transform.Rotate(rotation*dTime);

            }
        }
        /*
        if(cloneMovements[0])
        {
            clones[0].Move(direction);
            blockCoordinates[0] += nextMoves[0];
        }
        if(cloneMovements[1])
        {
            clones[1].Move(direction);
            blockCoordinates[1] += nextMoves[1];
        }
        if(rotateClones)
        {
            clones[0].Rotate(rotation);
            clones[1].Rotate(rotation);
        }
        
        MoveClones(false);
        rotateClones = false;
        */
    }
    void MoveClones(bool b)
    {
        cloneMovements[0] = b;
        cloneMovements[1] = b;
    }
    void RotateClones(bool b)
    {
        cloneRotations[0] = b;
        cloneRotations[1] = b;
    }
    Vector2Int indexConvert(Vector2Int v)
    {
        return new Vector2Int(11-v.y,1+v.x);
    }
    Vector2Int Teleport(int i, Vector2Int coordinates, GameObject g = null)
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
            g.transform.position = new Vector3(coordinates2.x-3 ,coordinates2.y-5.6f,0);
        }
        return coordinates2;
    }
    
    List<GameObject> CheckForMovableBlocks(Vector2Int coordinates, int i, bool b1, bool b2)
    {
        
        List<GameObject> myList = new List<GameObject>();
        Vector2Int coordinates2 = coordinates+nextMoves[i];

        
        int currentBlock = GridBuilder.currentLevel[11-coordinates.y, 1+coordinates.x];
        int componentBlock = cLevelComponents[11-coordinates.y, 1+coordinates.x];
        
        if(currentBlock==2)
        {
            //given clone hit a block
            cloneMovements[i] = false;
            nextCoordinates[i] = blockCoordinates[i];
            return myList;
        }
        if(currentBlock==4 && b1)
        {
            //this object is on a teleporter
            b1 = false;

            Vector2Int coordinates3 = Teleport(i, coordinates);
        
            //stuff on other teleporter
            myList = CheckForMovableBlocks(coordinates3, i, b1, b2);
            
            if(!cloneMovements[i])
            {
                //teleporter is blocked off
                cloneMovements[i] = true;
                dontTeleport = true;
                myList = new List<GameObject>();
            }
        }
        if(componentBlock==1)
        {
            //given block hit a movable object
            int nextBlock = GridBuilder.currentLevel[11-coordinates2.y, 1+coordinates2.x];

            if(nextBlock != 0 && nextBlock != 4 && nextBlock != 5 && nextBlock != 6)
            {
                //nothing will move
                Debug.Log("nothing will happen");
                cloneMovements[i] = false;
                if(b1)
                {
                    nextCoordinates[i] = blockCoordinates[i];
                }
            }
            else
            {
                //check next square
                Debug.Log("hit movable object");
                myList.InsertRange(0,CheckForMovableBlocks(coordinates2, i, b1, b2));
                
                if(cloneMovements[i])
                {
                    //this block is moving
                    GameObject g = cMovableBoxs[coordinates];
                    
                    myList.Insert(0,g);
                    
                }
            }            
        } 
        if(coordinates == blockCoordinates[1-i] && b2)
        {
            //sees if a block will become and obstruction due to not moving
            Debug.Log("obstruction");
            CheckForMovableBlocks(coordinates+nextMoves[1-i], 1-i, true, false);
            if(!cloneMovements[1-i])
            {
                cloneMovements[i] = false;
            }
        }
    
        return myList;
    }
    //check for complications of move
    void CheckForObstructions()
    {
        
        for(int i = 0; i < 2; i++)
        {
            nextMoves[i] = new Vector2Int(Mathf.RoundToInt(Mathf.Cos((clones[i].transform.eulerAngles.z+angle)*Mathf.Deg2Rad)),
            Mathf.RoundToInt(Mathf.Sin((clones[i].transform.eulerAngles.z+angle)*Mathf.Deg2Rad)));
            
            nextCoordinates[i] = blockCoordinates[i]+nextMoves[i];
        }
        for(int i = 0; i < 2; i++)
        {

            movingObjects[i] = CheckForMovableBlocks(nextCoordinates[i], i, true, true);
            
        }

        Vector2Int[][] coordinates = new Vector2Int[2][];
        coordinates[0] = new Vector2Int[movingObjects[0].Count];
        coordinates[1] = new Vector2Int[movingObjects[1].Count];
        //remove duplicates
        foreach(GameObject b in movingObjects[0])
        {
            if(movingObjects[1].Contains(b))
            {
                movingObjects[1].Remove(b);
            }
        }
        //check for illegal moves
        for(int i = 0; i < 2; i++)
        {
            //clone against clone
            if(nextCoordinates[i] == nextCoordinates[1-i] || GridBuilder.currentLevel[11-nextCoordinates[i].y, 1+nextCoordinates[i].x] == 0)
            {
                Debug.Log("illegal move");
                MoveClones(false);
            }
            //clone against block
            for(int j = 0; j<movingObjects[i].Count; j++)
            {
                GameObject b  = movingObjects[i][j];
                coordinates[i][j] = cSliderCoordinates[b];

                if(cloneMovements[i])
                {
                    coordinates[i][j] += nextMoves[i];
                }
                if(GridBuilder.currentLevel[11-coordinates[i][j].y, 1+coordinates[i][j].x]==0 || coordinates[i][j] == nextCoordinates[1-i])
                {
                    //illegal move
                    Debug.Log("illegal move");
                    MoveClones(false);
                }
            }
        }
        //block against block
        for(int i = 0; i<movingObjects[0].Count; i++)
        {
            for(int j = 0; j<movingObjects[1].Count; j++)
            {
                if(coordinates[1][j] == coordinates[0][i])
                {
                    //illegal move
                    Debug.Log("illegal move");
                    MoveClones(false);
                }
            }
        }
        if(nextCoordinates[0] == blockCoordinates[1] && nextCoordinates[1] == blockCoordinates[0])
        {
            //is switching
            Debug.Log("kind of illegal");
            MoveClones(false);
        }
        if(cloneMovements[0] || cloneMovements[1])
        {
            counter.Plus();
            moveArchives.Insert(0, new Tuple<bool, bool, bool, Vector2Int[], Vector3, List<GameObject>[], bool>(true, cloneMovements[0],
            cloneMovements[1], new Vector2Int[]{nextMoves[0]*-1, nextMoves[1]*-1}, direction*-1, new List<GameObject>[2]{new List<GameObject>(movingObjects[0]), new List<GameObject>(movingObjects[1])}, dontTeleport));
        }
        //Debug.Log(nextCoordinates[0]);
        //Debug.Log(nextCoordinates[1]);
    }
    public void UndoMove()
    {
        //undo move
        if(moveArchives.Count != 0 && !cloneMovements[0] && !cloneMovements[1] && !cloneRotations[0] && !cloneRotations[1])
        {
            isUndo = true;

            if(moveArchives[0].Item1)
            {
                //moving
                movingObjects = moveArchives[0].Item6;
                dontTeleport = moveArchives[0].Item7;

                cloneMovements[0] = moveArchives[0].Item2;
                cloneMovements[1] = moveArchives[0].Item3;
                nextMoves = moveArchives[0].Item4;
                direction = moveArchives[0].Item5;

                for(int i = 0; i<2; i++)
                {
                    if(!cloneMovements[i])
                    {
                        continue;
                    }
                    if(GridBuilder.currentLevel[11-blockCoordinates[i].y, 1+blockCoordinates[i].x] == 4 && !dontTeleport)
                    {
                        blockCoordinates[i] = Teleport(i, blockCoordinates[i], clones[i]);
                    }
                    foreach(GameObject b in movingObjects[i])
                    {
                        Vector2Int coordinates = cSliderCoordinates[b];
                        if(GridBuilder.currentLevel[11-coordinates.y, 1+coordinates.x] == 4 && !dontTeleport)
                        {
                            //on teleporter
                            cMovableBoxs.Remove(coordinates);
                            cSliderCoordinates.Remove(b);
                            cLevelComponents[11-coordinates.y, 1+coordinates.x] = 0;

                            coordinates = Teleport(i, coordinates, b);

                            cSliderCoordinates.Add(b, coordinates);
                            cLevelComponents[11-coordinates.y, 1+coordinates.x] = 1;

                        }
                    }
                }
                counter.Minus();
            }
            else
            {
                //rotating
                RotateClones(true);
                rotation = moveArchives[0].Item5;

            }
            moveArchives.RemoveAt(0);
            //nextMoves
        }
        //for button command
        tapCount = 0;

    }
    public void SetupClones()
    {
        Debug.Log("setup");
        MoveClones(false);
        RotateClones(false);
        distance = 1;
        blockCoordinates = new Vector2Int[]{GridBuilder.startCoordinates[0], GridBuilder.startCoordinates[1]};
        teleporterCoordinates = GridBuilder.subTeleporterCoordinates;

        cLevelComponents = CopyArray(GridBuilder.levelComponents);
        cMovableBoxs = new Dictionary<Vector2Int, GameObject>(GridBuilder.movableBoxs);
        cSliderCoordinates = new Dictionary<GameObject, Vector2Int>(GridBuilder.sliderCoordinates);
        
        for(int x=0; x<10; x++)
        {
            for(int y=0; y<7; y++)
            {
                
                if(cLevelComponents[x+1,y+1] == 1)
                {
                    cMovableBoxs[new Vector2Int(y, 10-x)].transform.position = new Vector3(y-3,-x+4.4f,1);//4.25
                    Debug.Log("carnegie melon");
                }
            }
        }
        
        clones[0].transform.position = new Vector3(blockCoordinates[0].x-3 ,blockCoordinates[0].y-5.6f,0);
        clones[1].transform.position = new Vector3(blockCoordinates[1].x-3 ,blockCoordinates[1].y-5.6f,0);
        clones[0].transform.eulerAngles = new Vector3(0, 0, 360-GridBuilder.startRotations[0]);
        clones[1].transform.eulerAngles = new Vector3(0, 0, 360-GridBuilder.startRotations[1]);
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
        int savedMoves = DataManager.gameData.savedMoves[GridBuilder.levelIndex];
        if(currentMoves < savedMoves || savedMoves == 0)
        {
            Debug.Log("system has saved");
            DataManager.gameData.savedMoves[GridBuilder.levelIndex] = currentMoves;
            SaveSystem.SaveData();
        }
        
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        tapCount = 0;
    }
    int[,] CopyArray(int[,] x)
    {
        int[,] myArry = new int[12,9];
        
        for(int i = 0; i<12; i++)
        {
            for(int j = 0; j<9; j++)
            {
                myArry[i,j] = x[i,j];
            }
        }
        
        return myArry;
    }
    void RevealArchive(int x)
    {
        for(int i = 0; i < moveArchives.Count; i++)
        {
            Debug.Log(moveArchives[i].Item6[x].Count);
        }
    }
}