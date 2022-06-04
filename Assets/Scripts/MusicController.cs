using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource source;

    public AudioClip auralSpaceImPM;
    public AudioClip auralSpaceInPM;
    public AudioClip auralSpaceJPM;
    public AudioClip nitralVoices;
    public AudioClip auralSpaceCPM;

    public AudioClip stageXF777;

    void Start()
    {
        source = GetComponent<AudioSource>();

        if(GridBuilder.levelIndex == 97)
        {
            //0
            source.clip = stageXF777;
        }
        else if(GridBuilder.levelIndex == 10)
        {
            //11
            source.clip = auralSpaceCPM;

        }
        else if(GridBuilder.levelIndex > 7)
        {
            //9&10
            source.clip = nitralVoices;
        }
        else if(GridBuilder.levelIndex > 5)
        {
            //7&8
            source.clip = auralSpaceJPM;
        }
        else if(GridBuilder.levelIndex > 2)
        {
            //4&5&6
            Debug.Log("hai");
            source.clip = auralSpaceInPM;
        }
        else
        {
            //1&2&3
            source.clip = auralSpaceImPM;
        }
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
