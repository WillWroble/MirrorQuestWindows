using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    //current build
    public int build;

    //player progress
    public int[] savedMoves;
    public bool[] platinumStars;
    public bool levelElevenIsOpen;

    
    //sound settings
    public bool inGameMusic;
    public bool inGameSound;
    public bool inLevelSelectMusic;
    public bool inLevelSelectSound;

    public float inGameMusicf;
    public float inGameSoundf;
    public float inLevelSelectMusicf;
    public float inLevelSelectSoundf;

    //game settings
    public bool doubleTapToUndo;
    public bool tapToRotate;
    public int doubleTapSpeed;

    public bool openingInstructions;

    public GameData()
    {
        build = DataManager.currentBuild;
        savedMoves = new int[12]{0,0,0,0,0,0,0,0,0,0,0,0};
        platinumStars = new bool[13]{false, false, false, false, false, false,
        false, false, false, false, false, false,false};
        levelElevenIsOpen = false;
        inLevelSelectMusicf = 1;
        inGameMusicf = 1;
        inLevelSelectMusic = false;
        inGameMusic = false;
        openingInstructions = true;
        tapToRotate = false;
        doubleTapToUndo = false;
        doubleTapSpeed = 1;
    }
}
