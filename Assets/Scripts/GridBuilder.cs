using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GridBuilder : MonoBehaviour
{
    
    public int defaultLevel;
    public static int levelIndex;
    int cloneCounter;
    int teleporterCounter;
    
    private static Camera cam;

    public GameObject prefabBlock;
    public GameObject block;
    public GameObject cloneManager;

    public static int[,] currentLevel;
    public static int[,] levelComponents;

    public static Vector2Int[] subTeleporterCoordinates;

    public static Vector2Int[] startCoordinates;
    public static float[] startRotations;

    //public List<Tuple<int, int, GameObject>> movableBoxes;
    public static Dictionary<Vector2Int, GameObject> movableBoxs;
    public static Dictionary<GameObject, Vector2Int> sliderCoordinates;

    public static Dictionary<Vector2Int, SubObject> boxs;


    public Sprite voidBlockTex;
    public Sprite regBlockTex;
    public Sprite boxBlockTex;
    public Sprite stoneBlockTex;
    public Sprite subTeleporterTex;
    public Sprite deadSubTeleporterTex;
    public Sprite smoothBlockTex;
    public Sprite sliderBlockTex;
    public Sprite drainBlockTex;

    GameObject[,] blocks;

    public static int stageHeight;



    // Start is called before the first frame update
    
    void Start()
    {
        if(defaultLevel >= 0)
        {
            levelIndex = defaultLevel;
        }
        cam = Camera.main;

        boxs = new Dictionary<Vector2Int, SubObject>();

        startCoordinates = new Vector2Int[2];
        startRotations = new float[2];
        cloneCounter = 0;
        teleporterCounter = 0;

        if(levelIndex != 10)
        {
            stageHeight = 10;
            initializeSmallLevel();
        }
        else
        {
            stageHeight = 12;
            initializeBigLevel();
        }
        Debug.Log(stageHeight);


        //movableBoxes = new List<Tuple<int, int, GameObject>>();
        //movableBoxs = new Dictionary<Vector2Int, GameObject>();
        //sliderCoordinates = new Dictionary<GameObject, Vector2Int>();

        //voidBlockTex = Resources.Load<Sprite>("Textures/NewVoidBlock");
        //regBlockTex = Resources.Load<Sprite>("Textures/RegBlock6");
        //boxBlockTex = Resources.Load<Sprite>("Textures/BoxBlock7");
        //stoneBlockTex = Resources.Load<Sprite>("Textures/TeleportStone1");

        
        
        cloneManager.SetActive(true);

        //setTexture(blocks[3,3],1);

        
    }
    public void setTexture(GameObject b, int x, int y)
    {

        SpriteRenderer spriteRender = b.GetComponent<SpriteRenderer>();
        switch(currentLevel[x+1,y+1])
        {
            case 0:
            spriteRender.sprite = voidBlockTex;
            break;

            case 1:
            spriteRender.sprite = regBlockTex;
            break;

            case 2:
            spriteRender.sprite = boxBlockTex;
            break;

            case 3:
            spriteRender.sprite = stoneBlockTex;
            break;

            case 4:
            spriteRender.sprite = subTeleporterTex;
            subTeleporterCoordinates[teleporterCounter] = 
            new Vector2Int(y, stageHeight-x);
            teleporterCounter++;
            b.transform.Translate(0,0,1);
            break;

            case 14:
            spriteRender.sprite = deadSubTeleporterTex;
            subTeleporterCoordinates[teleporterCounter] = 
            new Vector2Int(y, stageHeight-x);
            teleporterCounter++;
            b.transform.Translate(0,0,1);
            break;


            case 5:
            spriteRender.sprite = smoothBlockTex;
            b.transform.Translate(0,0,2);
            break;

            case 6:
            spriteRender.sprite = sliderBlockTex;
            //movableBoxs.Add(new Vector2Int(y,10-x), b);
            //sliderCoordinates.Add(b, new Vector2Int(y,10-x));


            SubObject myBox = new SubObject(b, new Vector2Int(y,stageHeight-x));
            boxs.Add(new Vector2Int(y,stageHeight-x), myBox);
            b.transform.Translate(0,0,-1);
            //puts smooth tile underneath
            GameObject b2 = Instantiate(prefabBlock);
            b2.transform.position = b.transform.position; b2.transform.Translate(0,0,3);
            b2.GetComponent<SpriteRenderer>().sprite = smoothBlockTex;
            levelComponents[x+1,y+1] = 1;
            break;

            case 7:
            spriteRender.sprite = drainBlockTex;
            b.transform.Translate(0,0,1);
            break;


            case 360:
            spriteRender.sprite = regBlockTex;
            startCoordinates[cloneCounter] = new Vector2Int(y, stageHeight-x);
            startRotations[cloneCounter] = 0;
            cloneCounter++;
            break;

            case 180:
            spriteRender.sprite = regBlockTex;
            startCoordinates[cloneCounter] = new Vector2Int(y, stageHeight-x);
            startRotations[cloneCounter] = 180;
            cloneCounter++;
            break;

            case 90:
            spriteRender.sprite = regBlockTex;
            startCoordinates[cloneCounter] = new Vector2Int(y, stageHeight-x);
            startRotations[cloneCounter] = 90;
            cloneCounter++;
            break;

            case 270:
            spriteRender.sprite = regBlockTex;
            startCoordinates[cloneCounter] = new Vector2Int(y, stageHeight-x);
            startRotations[cloneCounter] = 270;
            cloneCounter++;
            break;

        }

    }
    void initializeSmallLevel()
    {
        blocks = new GameObject[10,7]; //was 11

        subTeleporterCoordinates = new Vector2Int[2];

        LevelLibrary.Open();

        currentLevel = LevelLibrary.GetLevel(levelIndex);
        currentLevel = addPadding(currentLevel);
        levelComponents = new int[12,9];
        initializeWithZero(levelComponents, 12, 9);
        

        for(int x=0; x<10; x++)
        {
            for(int y=0; y<7; y++)
            {
                
                //block = GameObject.Find("Block");
                block = Instantiate(prefabBlock);
                block.transform.position = new Vector3(y-3,-x+4.4f,1);//4.25
                

                setTexture(block, x, y);
                blocks[x,y] = block;

                
            }
        }
    }
    void initializeBigLevel()
    {
        blocks = new GameObject[12,9]; //was 11

        subTeleporterCoordinates = new Vector2Int[2];

        LevelLibrary.Open();

        currentLevel = LevelLibrary.GetLevel(levelIndex);
        currentLevel = addPadding(currentLevel);
        levelComponents = new int[14,11];
        initializeWithZero(levelComponents, 14, 11);
        

        for(int x=0; x<12; x++)
        {
            for(int y=0; y<9; y++)
            {
                
                //block = GameObject.Find("Block");
                block = Instantiate(prefabBlock);
                block.transform.position = new Vector3(y-4,-x+5.4f,1);//4.25
                

                setTexture(block, x, y);
                blocks[x,y] = block;

                
            }
        }
    }
    void initializeWithZero(int[,] a, int x, int y)
    {
        for(int i = 0; i<x; i++)
        {
            for(int j = 0; j<y; j++)
            {
                a[i,j] = 0;
            }
        }
    }
    int[,] addPadding(int[,] a)
    {
        int[,] myArray = new int[a.GetLength(0)+2, a.GetLength(1)+2];
        initializeWithZero(myArray, myArray.GetLength(0), myArray.GetLength(1));
        for(int i = 0; i<a.GetLength(0); i++)
        {
            for(int j = 0; j<a.GetLength(1); j++)
            {
                myArray[i+1,j+1] = a[i,j];
            }
        }

        return myArray;
    }

}
