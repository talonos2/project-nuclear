using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    AsyncOperation loadingTitleScreen;
    private float minDelayUntilNextScene = 2.5f;
    public AudioListener listenerToSilence;
    private bool fadingOut;
    private FadeOut fadeout;
    private bool initFadeout;

    void Start()
    {
        FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE) ;
       // FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));

        //fadeout.InitNext("TitleScreen", .2f);
        loadingTitleScreen = SceneManager.LoadSceneAsync("TitleScreen",LoadSceneMode.Additive);       
        loadingTitleScreen.allowSceneActivation = false;
        //SceneManager.LoadScene("TitleScreen");
    }

    private void Update()
    {
        minDelayUntilNextScene -= Time.deltaTime;
        if (minDelayUntilNextScene < 0) {
            if (loadingTitleScreen.isDone) {
                if (!initFadeout) {
                    //fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
                    //fadeout.InitScenelessFadeout(5.5f);
                    //fadingOut = true;
                    //initFadeout = true;
                }
                GameData.Instance.startSceneLoaded = true;
                listenerToSilence.enabled = false;
                SceneManager.UnloadSceneAsync("LogoScreen");
                loadingTitleScreen.allowSceneActivation = true;

                //SceneManager.SetActiveScene(SceneManager.GetSceneByName("TitleScreen"));
            }
        }

        if (minDelayUntilNextScene < -4)
        {
            SceneManager.LoadScene("TitleScreen");
            GameData.Instance.startSceneLoaded = true;
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
