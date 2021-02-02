using System;
using System.Collections;
using System.Collections.Generic;
using Naninovel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsExit : MonoBehaviour
{
    // Start is called before the first frame update

    public float timeSpent;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FWInputManager.Instance.GetKeyDown(InputAction.GO_BACK))
        {
            CloseCredits();
        }
        timeSpent += Time.deltaTime;
        if (timeSpent > 72) { CloseCreditsSoft(); }
    }

    public void CloseCredits()
    {
        SoundManager.Instance.PlayPersistentSound("MenuNope", 1);
        SceneManager.LoadScene("TitleScreen");
        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        //fadeout.InitNext("TitleScreen");
    }

    public void CloseCreditsSoft() {
        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        fadeout.InitNext("TitleScreen");
       // SceneManager.LoadScene("TitleScreen");
    }
}
