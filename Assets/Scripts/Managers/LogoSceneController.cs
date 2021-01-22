using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE) ;
        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        fadeout.InitNext("TitleScreen", .2f);
        //SceneManager.LoadScene("TitleScreen");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    Debug.Log("Space pressed in logo scene");
        //}
        if (FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE)) { Debug.Log("keypressed in Logos"); }

    }


}
