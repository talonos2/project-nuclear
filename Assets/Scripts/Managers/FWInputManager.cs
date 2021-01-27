using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FWInputManager: Singleton<FWInputManager>
{
    public Dictionary<InputAction, KeyCode[]> keyBindings = new Dictionary<InputAction, KeyCode[]>();

    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("FWInputManager started");
        if (!started)
        {
            SetToArrowKeys();
        }
    }



    public void SetToArrowKeys()
    {
        //Debug.Log("SneakyKeybind to Arrows!");
        keyBindings = new Dictionary<InputAction, KeyCode[]>();
        keyBindings.Add(InputAction.LEFT, new KeyCode[] { KeyCode.LeftArrow });
        keyBindings.Add(InputAction.RIGHT, new KeyCode[] { KeyCode.RightArrow });
        keyBindings.Add(InputAction.UP, new KeyCode[] { KeyCode.UpArrow });
        keyBindings.Add(InputAction.DOWN, new KeyCode[] { KeyCode.DownArrow });
        keyBindings.Add(InputAction.MENU_LEFT, new KeyCode[] { KeyCode.LeftArrow, KeyCode.A });
        keyBindings.Add(InputAction.MENU_RIGHT, new KeyCode[] { KeyCode.RightArrow, KeyCode.D });
        keyBindings.Add(InputAction.MENU_UP, new KeyCode[] { KeyCode.UpArrow, KeyCode.W });
        keyBindings.Add(InputAction.MENU_DOWN, new KeyCode[] { KeyCode.DownArrow, KeyCode.S });
        keyBindings.Add(InputAction.ROTATE_LEFT, new KeyCode[] { KeyCode.A });
        keyBindings.Add(InputAction.ROTATE_RIGHT, new KeyCode[] { KeyCode.D });
        keyBindings.Add(InputAction.USE_POWER, new KeyCode[] { KeyCode.C });
        keyBindings.Add(InputAction.REST, new KeyCode[] { KeyCode.R });
        keyBindings.Add(InputAction.ACTIVATE, new KeyCode[] { KeyCode.Z, KeyCode.Space, KeyCode.Return });
        keyBindings.Add(InputAction.GO_BACK, new KeyCode[] {  KeyCode.Backspace, KeyCode.Escape, KeyCode.Delete });
    }

    internal bool IsWASD()
    {
        return keyBindings[InputAction.LEFT][0] == KeyCode.A;
    }

    public void SetToWASD()
    {
        //Debug.Log("SneakyKeybind to WASD!");
        keyBindings = new Dictionary<InputAction, KeyCode[]>();
        keyBindings.Add(InputAction.LEFT, new KeyCode[] { KeyCode.A });
        keyBindings.Add(InputAction.RIGHT, new KeyCode[] { KeyCode.D });
        keyBindings.Add(InputAction.UP, new KeyCode[] { KeyCode.W });
        keyBindings.Add(InputAction.DOWN, new KeyCode[] { KeyCode.S });
        keyBindings.Add(InputAction.MENU_LEFT, new KeyCode[] { KeyCode.LeftArrow, KeyCode.A });
        keyBindings.Add(InputAction.MENU_RIGHT, new KeyCode[] { KeyCode.RightArrow, KeyCode.D });
        keyBindings.Add(InputAction.MENU_UP, new KeyCode[] { KeyCode.UpArrow, KeyCode.W });
        keyBindings.Add(InputAction.MENU_DOWN, new KeyCode[] { KeyCode.DownArrow, KeyCode.S });
        keyBindings.Add(InputAction.ROTATE_LEFT, new KeyCode[] { KeyCode.LeftArrow });
        keyBindings.Add(InputAction.ROTATE_RIGHT, new KeyCode[] { KeyCode.RightArrow });
        keyBindings.Add(InputAction.USE_POWER, new KeyCode[] { KeyCode.UpArrow });
        keyBindings.Add(InputAction.REST, new KeyCode[] { KeyCode.R });
        keyBindings.Add(InputAction.ACTIVATE, new KeyCode[] { KeyCode.Z, KeyCode.Space, KeyCode.Return });
        keyBindings.Add(InputAction.GO_BACK, new KeyCode[] { KeyCode.Backspace, KeyCode.Escape, KeyCode.Delete });
    }

    public bool GetKeyDown(InputAction action)
    {
        if (SceneManager.GetActiveScene().name=="LogoScreen") { return false ; }
        if (!started)
        {
            SetToArrowKeys();
            started = true;
        }
        foreach (KeyCode key in keyBindings[action])
        {
            if (Input.GetKeyDown(key))
            {
//                Debug.Log("key pressed in FWInputManager " + key);
                return true;
            }
        }
        return false;
    }



    
    public bool GetKeyUp(InputAction action)
    {
        if (!started)
        {
            SetToArrowKeys();
            started = true;
        }
        foreach (KeyCode key in keyBindings[action])
        {
            if (Input.GetKeyUp(key))
            {
                return true;
            }
        }
        return false;
    }

    public bool GetKey(InputAction action)
    {
        if (!started)
        {
            SetToArrowKeys();
            started = true;
        }
        foreach (KeyCode key in keyBindings[action])
        {
            if (Input.GetKey(key))
            {
                return true;
            }
        }
        return false;
    }

}
