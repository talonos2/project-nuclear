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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FWInputManager.Instance.GetKeyDown(InputAction.GO_BACK)|| 
            FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE)||
            FWInputManager.Instance.GetKeyDown(InputAction.USE_POWER))
        {
            CloseCredits();
        }
    }

    public void CloseCredits()
    {
        SoundManager.Instance.PlayPersistentSound("MenuNope", 1);
        SceneManager.LoadScene("TitleScreen");
    }
}
