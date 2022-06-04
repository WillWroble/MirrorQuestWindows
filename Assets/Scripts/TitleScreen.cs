using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    private Touch touch;

    public GameObject panel;
    Animator myAnim;
    
    void Start()
    {
        myAnim = panel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0)
        {
            touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Ended)
            {
                StartCoroutine(LoadScene("LevelSelect"));
            }
        }
        if(Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LoadScene("LevelSelect"));
        }
    }
    IEnumerator LoadScene(string name)
    {
        myAnim.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);

    }
}
