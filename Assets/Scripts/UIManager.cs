using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
   
    bool isSoundVolume;
    bool isMusicVolume;

    public enum scenes
    {
        inGame,
        LevelSelect
    }
    public scenes scene;
    public GameObject window;
    public GameObject soundIcon;
    public GameObject musicIcon;
    public Transform parent;
    public GameObject otherActiveObject;
    public GameObject musicPlayer;
    public GameObject sliderOne;
    public GameObject sliderTwo;
    public GameObject ttuToggle;
    public GameObject ttrToggle;
    public GameObject dtsController;
    
    public GameObject openingInstructions;

    //in game ui
    /*
    public GameObject settingsButton;
    public GameObject counter;
    public GameObject undoButton;
    public GameObject resetButton;
    */



    //public GameObject soundPlayer;
    
    public Animator transitionAnim;

    public Sprite volumeOn;
    public Sprite volumeOff;

    
    // Start is called before the first frame update
    void Start()
    {
        
        isSoundVolume = true;
        isMusicVolume = true;
        parent = parent.transform;
        
        if(scene == scenes.LevelSelect)
        {
            musicPlayer.GetComponent<AudioSource>().volume = 
            DataManager.gameData.inLevelSelectMusicf;
            musicPlayer.GetComponent<AudioSource>().mute = 
            DataManager.gameData.inLevelSelectMusic;

            sliderTwo.GetComponent<Slider>().value = DataManager.gameData.inLevelSelectMusicf;

            if(DataManager.gameData.inLevelSelectMusic)
            {
                musicIcon.GetComponent<Image>().sprite = volumeOff;
                isMusicVolume = false;
            }
            else
            {
                musicIcon.GetComponent<Image>().sprite = volumeOn;
                isMusicVolume = true;
            }
        }
        else
        {
            musicPlayer.GetComponent<AudioSource>().volume = 
            DataManager.gameData.inGameMusicf;
            musicPlayer.GetComponent<AudioSource>().mute = 
            DataManager.gameData.inGameMusic;

            sliderTwo.GetComponent<Slider>().value = DataManager.gameData.inGameMusicf;

            if(DataManager.gameData.inGameMusic)
            {
                musicIcon.GetComponent<Image>().sprite = volumeOff;
                isMusicVolume = false;
            }
            else
            {
                musicIcon.GetComponent<Image>().sprite = volumeOn;
                isMusicVolume = true;
            }

            //start tutorial
            if(DataManager.gameData.openingInstructions)
            {
                openingInstructions.SetActive(true);
            }

        }
        //controlls the toggles
        if(DataManager.gameData.doubleTapToUndo)
        {
            ttuToggle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            ttuToggle.GetComponent<Toggle>().isOn = false;
        }
        if(DataManager.gameData.tapToRotate)
        {
            ttrToggle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            ttrToggle.GetComponent<Toggle>().isOn = false;
        }

        dtsController.GetComponent<TMP_Dropdown>().value = DataManager.gameData.doubleTapSpeed;
    }

    // Update is called once per frame
    ///*
    void Update()
    {
        //button.transform.Translate(Vector3.left*Time.deltaTime*50);
        
        //IN EDITOR ONLY
        
        if(Input.GetKeyDown("k"))
        {
            ScreenshotHandler.TakeScreenshot_Static(750, 600);
        }
        
    }
    //*/
    
    public void OpenGame()
    {
        if(LevelSelectMovement.index-2 != 11 || CheckIfLevelElevenIsOpen())
        {
            StartCoroutine(LoadScene("InGame"));
            if(DataManager.gameData.openingInstructions)
            {
                GridBuilder.levelIndex = 0;
            }
            else
            {
                GridBuilder.levelIndex = LevelSelectMovement.index-2;
            }
        }  
    }
    public void OpenLevelSelect()
    {
        CloneManager2.tapCount = 0;
        if(CloneManager2.hint)
        {
            Debug.Log("hint");
            CloneManager2.hint = false;
            StartCoroutine(LoadScene("HintScreen"));

        }
        else
        {
            StartCoroutine(LoadScene("LevelSelect"));
            LevelSelectMovement.index = GridBuilder.levelIndex+2;
        }
        
    }
    public void GoToNextLevel()
    {
        GridBuilder.levelIndex += 1;
        StartCoroutine(LoadScene("InGame"));
        //GridBuilder.levelIndex += 1;
    }
    public void OpenSettings()
    {
        window.SetActive(true);
        if(scene == scenes.inGame)
        {
            otherActiveObject.SetActive(false);
        }
        else
        {
            LevelSelectMovement.freeze = true;
        }
    }
    public void CloseSettings()
    {
        float musicVolume;
        bool isMusicMute;
        if(scene == scenes.LevelSelect)
        {
            musicVolume = DataManager.gameData.inLevelSelectMusicf;
            isMusicMute = DataManager.gameData.inLevelSelectMusic;
        }
        else
        {
            musicVolume = DataManager.gameData.inGameMusicf;
            isMusicMute = DataManager.gameData.inGameMusic;
        }
        sliderTwo.GetComponent<Slider>().value = musicVolume;
        musicPlayer.GetComponent<AudioSource>().volume = musicVolume;
        musicPlayer.GetComponent<AudioSource>().mute = isMusicMute;

        window.SetActive(false);
        if(scene == scenes.inGame)
        {
            otherActiveObject.SetActive(true);
        }
        else
        {
            LevelSelectMovement.freeze = false;
        }

        ttuToggle.GetComponent<Toggle>().isOn = DataManager.gameData.doubleTapToUndo;
        ttrToggle.GetComponent<Toggle>().isOn = DataManager.gameData.tapToRotate;

        dtsController.GetComponent<TMP_Dropdown>().value = DataManager.gameData.doubleTapSpeed;
        
        
    }
    public void SaveSettings()
    {
        if(scene == scenes.LevelSelect)
        {
            DataManager.gameData.inLevelSelectMusicf = musicPlayer.GetComponent<AudioSource>().volume;
            DataManager.gameData.inLevelSelectMusic = musicPlayer.GetComponent<AudioSource>().mute;

        }
        else
        {
            DataManager.gameData.inGameMusicf = musicPlayer.GetComponent<AudioSource>().volume;
            DataManager.gameData.inGameMusic = musicPlayer.GetComponent<AudioSource>().mute;

        }
        DataManager.gameData.doubleTapToUndo = ttuToggle.GetComponent<Toggle>().isOn;
        DataManager.gameData.tapToRotate = ttrToggle.GetComponent<Toggle>().isOn;
        //CloneManager2.undoingIsEnabled = DataManager.gameData.doubleTapToUndo;
        //CloneManager2.rotatingIsEnabled = DataManager.gameData.tapToRotate;
        DataManager.gameData.doubleTapSpeed = dtsController.GetComponent<TMP_Dropdown>().value;

        SaveSystem.SaveData();

        if(scene == scenes.inGame)
        {
            CloneManager2.SettingsCycle();
            otherActiveObject.SetActive(true);
        }
        else
        {
            LevelSelectMovement.freeze = false;
        }

        window.SetActive(false);

    }
    public void ToggleSoundVolume()
    {
        if(isSoundVolume)
        {
            soundIcon.GetComponent<Image>().sprite = volumeOff;
        }
        else
        {
            soundIcon.GetComponent<Image>().sprite = volumeOn;
        }

        isSoundVolume = !isSoundVolume;

    }
    public void ToggleMusicVolume()
    {
        if(isMusicVolume)
        {
            musicIcon.GetComponent<Image>().sprite = volumeOff;
            musicPlayer.GetComponent<AudioSource>().mute = true;
        }
        else
        {
            musicIcon.GetComponent<Image>().sprite = volumeOn;
            musicPlayer.GetComponent<AudioSource>().mute = false;
        }
        
        isMusicVolume = !isMusicVolume;
        
    }
    public void changeSoundVolume()
    {
        //does nothing as of now
    }
    public void changeMusicVolume()
    {
        musicPlayer.GetComponent<AudioSource>().volume = 
        sliderTwo.GetComponent<Slider>().value;
    }
    public void ToggleTTU()
    {
        
        //CloneManager2.undoingIsEnabled = !CloneManager2.undoingIsEnabled;
        
    }
    public void ToggleTTR()
    {
        
        //CloneManager2.rotatingIsEnabled= !CloneManager2.rotatingIsEnabled;
        
    }
    public void ResetGame()
    {
        GameData blankGame = new GameData();

        DataManager.gameData = blankGame;
        SaveSystem.SaveData();
        StartCoroutine(LoadScene("LevelSelect"));
        
    }
    public static bool CheckIfLevelElevenIsOpen()
    {
        //level not open yet will be in version 1.1
        //return false;
        
        if(DataManager.gameData.levelElevenIsOpen)
        {
            return true;
        }
        bool isOpen = true;
        for(int i = 0; i < 10; i++)
        {
            if(DataManager.gameData.savedMoves[i] == 0)
            {
                isOpen = false;
            }
        }
        if(isOpen)
        {
            DataManager.gameData.levelElevenIsOpen = true;
            SaveSystem.SaveData();
        }
        return isOpen;
        
    }
    
    IEnumerator LoadScene(string name)
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);

    }
    

}
