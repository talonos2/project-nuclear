﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transitionToEndRunScreen : MonoBehaviour
{
    public static void LoadEndRunScene() {
        SceneManager.LoadScene("EndRunScreen");
    }
}
