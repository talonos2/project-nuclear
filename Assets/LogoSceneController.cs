using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        fadeout.InitNext("TitleScreen", .2f);
        //SceneManager.LoadScene("TitleScreen");
    }


}
