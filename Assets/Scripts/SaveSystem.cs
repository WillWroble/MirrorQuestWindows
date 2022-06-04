using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{

    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GameData.mq";
        FileStream stream = new FileStream(path,FileMode.Create);

        GameData data = DataManager.gameData;
        formatter.Serialize(stream, data);
        stream.Close();
        
    }
    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/GameData.mq";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            int[] oldMinMoves = new int[11]{8,15,17,13,33,10,29,67,43,41,50};
            
            
            if(data.build < DataManager.currentBuild)
            {
                Debug.Log("adopting old save file");

                GameData nData = new GameData();

                for(int i=0 ;i<data.savedMoves.Length ;i++)
                {
                    //nData.savedMoves[i] = data.savedMoves[i];
                    
                    if(data.build == 0)
                    {
                        //for first version
                        if(data.savedMoves[i] < oldMinMoves[i] && data.savedMoves[i] != 0)
                        {
                            nData.platinumStars[i] = true;    
                        }
                        
                        nData.savedMoves[i] = data.savedMoves[i];                              
                        
                    }
                    else
                    {
                        //for all other versions(with platinum star data)
                        if(data.platinumStars[i])
                        {
                            nData.platinumStars[i] = true;
                        }
                        nData.savedMoves[i] = data.savedMoves[i];
                    }
                }
                //copy old settings data
                nData.doubleTapSpeed = data.doubleTapSpeed;
                nData.doubleTapToUndo = data.doubleTapToUndo;
                nData.inGameMusic = data.inGameMusic;
                nData.inGameMusicf= data.inGameMusicf;
                nData.inLevelSelectMusic = data.inLevelSelectMusic;
                nData.inLevelSelectMusicf = data.inLevelSelectMusicf;
                nData.levelElevenIsOpen = data.levelElevenIsOpen;
                nData.openingInstructions = data.openingInstructions;
                nData.tapToRotate = data.tapToRotate;

                return nData;
            }
            

            return data;

        }
        else
        {
            Debug.Log("fresh game");

            GameData data = new GameData();

            return data;
            
        }
    }
}


