using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMovieController : MonoBehaviour
{
    // Start is called before the first frame update
    private float minDelayUntilNextScene = 125f;
    //private bool fadingOut;
    //private FadeOut fadeout;
    //private bool initFadeout;

    void Start()
    {
        Debug.Log("before MM");
        MusicManager.instance.FadeOutMusic(MusicManager.MENU, 1f);
        Debug.Log("after MM");

        //FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE) ;
        // FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));

        //fadeout.InitNext("TitleScreen", .2f);
        //loadingTitleScreen = SceneManager.LoadSceneAsync("TitleScreen",LoadSceneMode.Additive);       
        //loadingTitleScreen.allowSceneActivation = false;
        //SceneManager.LoadScene("TitleScreen");
    }

    private void Update()
    {

        if (FWInputManager.Instance.GetKeyDown(InputAction.GO_BACK))
        {
            StartDungeonRun.StartRun();
        }

            minDelayUntilNextScene -= Time.deltaTime;
        if (minDelayUntilNextScene < 0) {

            StartDungeonRun.StartRun();

        }



        /*if (fadingOut) {
            if (fadeout.scenelessFadeOutFinished())
             {
            GameData.Instance.startSceneLoaded = true;
            listenerToSilence.enabled = false;
            SceneManager.UnloadSceneAsync("LogoScreen");
            loadingTitleScreen.allowSceneActivation = true;
            }
        }*/


        //if (FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE)) { Debug.Log("keypressed in Logos"); }

    }


}
