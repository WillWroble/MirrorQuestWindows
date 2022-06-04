using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackroundScroller : MonoBehaviour
{
    public float scrollSpeed;
    Vector2 offSet;
    // Start is called before the first frame update
    void Start()
    {
        offSet = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        offSet.x += Time.deltaTime*scrollSpeed;
        GetComponent<MeshRenderer>().material.mainTextureOffset = offSet;
    }
}
