using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HintScreen : MonoBehaviour
{
    
    public Animator transitionAnim;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ToMenu());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ToMenu()
    {
        yield return new WaitForSeconds(5f);
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LevelSelect");
        LevelSelectMovement.index = 12;

    }
    
}
