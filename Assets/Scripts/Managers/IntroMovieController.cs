using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroMovieController : MonoBehaviour
{
    // Start is called before the first frame update
    //private float minDelayUntilNextScene = .125f;
    public VideoPlayer videoPlayed;
    private bool movieHasStarted;
    private bool fading;

    //private bool fadingOut;
    //private FadeOut fadeout;
    //private bool initFadeout;

    void Start()
    {
        videoPlayed.Play();
        //Debug.Log("before MM");
        //MusicManager.instance.FadeOutMusic(MusicManager.MENU, 1f);
        //Debug.Log("after MM");

        //FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE) ;
        // FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));

        //fadeout.InitNext("TitleScreen", .2f);
        //loadingTitleScreen = SceneManager.LoadSceneAsync("TitleScreen",LoadSceneMode.Additive);       
        //loadingTitleScreen.allowSceneActivation = false;
        //SceneManager.LoadScene("TitleScreen");
    }

    private void Update()
    {

        if (videoPlayed.isPlaying) {
            movieHasStarted = true;
        }
        if (movieHasStarted) {
            if (!videoPlayed.isPlaying&&!fading) {

                GameData.Instance.FloorNumber = 1;
                GameData.Instance.isCutscene = false;
                GameData.Instance.SetNextLocation(new Vector2Int(-13, -10), SpriteMovement.DirectionMoved.RIGHT);
                GameData.Instance.ResetTimer();

                FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
                fadeout.attachToGUI(this.transform.GetComponent<Canvas>());
                fadeout.InitNext("Map1-1", .3f);
                fading = true;

                //StartDungeonRun.StartRun();
            }
        }
        if (FWInputManager.Instance.GetKeyDown(InputAction.GO_BACK))
        {
            videoPlayed.Pause();
        }

     //       minDelayUntilNextScene -= Time.deltaTime;
       // if (minDelayUntilNextScene < 0) {


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
