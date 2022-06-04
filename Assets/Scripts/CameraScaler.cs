using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Camera))]
//[ExecuteInEditMode]
public class CameraScaler : MonoBehaviour
{
     // Set this to the in-world distance between the left & right edges of your scene.
    public float notchLength;
    public bool isBigLevel;
    public float sceneWidth;
    public float sceneHeight;
    public GameObject canvas;
    public GameObject counter;
    public GameObject resetButton;

    public enum sceneSizes
    {
        inGame,
        levelSelect
    }
    public sceneSizes sizes;

    Camera cam;
    CanvasScaler canvasScaler;
    void Start() 
    {
        cam = GetComponent<Camera>();

        canvasScaler = canvas.GetComponent<CanvasScaler>();

        if(sizes == sceneSizes.inGame)
        {
            Debug.Log(GridBuilder.levelIndex);
            if(GridBuilder.levelIndex!=10)
            {
                isBigLevel = false;
            }
            else
            {
                isBigLevel = true;
            }
            
            if(isBigLevel)
            {
                Debug.Log("big level");
                sceneWidth = 11.14285714286f;
                sceneHeight = 16.7047619048f;
            }
            else
            {
                Debug.Log("reg level");
                sceneWidth = 7.14285714286f;
                sceneHeight = 12.7047619048f;
            }
            
        }
        else if(sizes == sceneSizes.levelSelect)
        {
            sceneWidth = 5.62218890f;
            sceneHeight = 10f;
        }

        if((Screen.height*1f/Screen.width*1f)>(1335f/750f))
        {
            //is a longer aspect ratio

            float unitsPerPixel = sceneWidth / Screen.width;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            cam.orthographicSize = desiredHalfHeight;
            canvasScaler.matchWidthOrHeight = 0;

            if(sizes == sceneSizes.inGame)
            {
                //account for notch
                float canvasScale = canvas.transform.localScale.x;
                counter.transform.Translate(new Vector2(0,-notchLength*canvasScale));
                resetButton.transform.Translate(new Vector2(0,notchLength*canvasScale));

            }
        }
        else
        {
            //is the same or flatter aspect ratio
            
            cam.orthographicSize = sceneHeight/2;
            canvasScaler.matchWidthOrHeight = 1;

        }
       
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update() 
    {
        
    }
}
