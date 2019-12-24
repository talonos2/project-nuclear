using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transitionToEndRunScreen : MonoBehaviour
{
    public void LoadEndRunScene() {
        SceneManager.LoadScene("EndRunScreen");
    }
}
