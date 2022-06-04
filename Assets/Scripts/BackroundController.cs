using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackroundController : MonoBehaviour
{
    MeshRenderer mesh;

    public Material backroundOne;
    public Material backroundTwo;
    public Material backroundThree;
    public Material backroundFour;
    public Material backroundFive;
    public Material backroundZero;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        if(GridBuilder.levelIndex == 97)
        {
            //0
            mesh.material = backroundZero;

        }
        else if(GridBuilder.levelIndex == 10)
        {
            //11
            mesh.material = backroundFive;
        }
        else if(GridBuilder.levelIndex > 7)
        {
            //9&10
            mesh.material = backroundFour;
        }
        else if(GridBuilder.levelIndex > 5)
        {
            //7&8
            mesh.material = backroundThree;
        }
        else if(GridBuilder.levelIndex > 2)
        {
            //4&5&6
            mesh.material = backroundTwo;
        }
        else
        {
            //1&2&3
            mesh.material = backroundOne;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
