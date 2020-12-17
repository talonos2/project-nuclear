using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToSceneAfterTime : MonoBehaviour
{
    public float timeBeforeTransition = 2;
    public string sceneToTransitionTo = "DeathScene";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeTransition -= Time.deltaTime;
        if (timeBeforeTransition < 0)
        {
            FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
            fadeout.InitNext(sceneToTransitionTo, 2);
            GameState.isInBattle = false;
            GameObject.Destroy(this);
        }
    }
}
