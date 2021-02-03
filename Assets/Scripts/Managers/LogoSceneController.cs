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

    void Start()
    {
        FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE);
        loadingTitleScreen = SceneManager.LoadSceneAsync("TitleScreen",LoadSceneMode.Additive);       
        loadingTitleScreen.allowSceneActivation = false;
    }

    private void Update()
    {
        minDelayUntilNextScene -= Time.deltaTime;
        if (minDelayUntilNextScene < 0) {
            if (loadingTitleScreen.isDone)
            {

                GameData.Instance.startSceneLoaded = true;
                listenerToSilence.enabled = false;
                SceneManager.UnloadSceneAsync("LogoScreen");
                loadingTitleScreen.allowSceneActivation = true;

            }
        }

        if (minDelayUntilNextScene < -4)
        {
            SceneManager.LoadScene("TitleScreen");
            GameData.Instance.startSceneLoaded = true;
        }

    }


}
