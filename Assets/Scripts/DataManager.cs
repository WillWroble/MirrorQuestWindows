using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameData gameData;
    public static int currentBuild;
    
    void Start()
    {
        //CURRENT BUILD
        currentBuild = 1;
        //CURRENT BUILD

        gameData = SaveSystem.LoadData();
        Debug.Log(gameData.platinumStars[6]);
        Debug.Log(gameData.savedMoves[6]);

    }
}
