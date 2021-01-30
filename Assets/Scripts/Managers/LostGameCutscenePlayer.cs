using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostGameCutscenePlayer : MonoBehaviour
{
    public float MoveSpeed = 4;
    private float DistanceToMove;
    public float distanceMovedSoFar;
    private bool soundPlaying = false;
    private Canvas thisCanvas;
    private bool loadingFadout;

    // Start is called before the first frame update
    void Start()
    {
        thisCanvas = GameObject.Find("Ground").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!soundPlaying) {
            SoundManager.Instance.PlaySound("longHowlingWind", .8f);
            soundPlaying = true;
        }

        DistanceToMove = Time.deltaTime*MoveSpeed;
        distanceMovedSoFar += DistanceToMove;

        Vector3 getMotion=new Vector3 (0,0,0);
        if (distanceMovedSoFar < 35)
        {
            getMotion = new Vector3(0, -DistanceToMove, 0);
        }
        else if (distanceMovedSoFar < 43.5)
        {
            getMotion = new Vector3(0, DistanceToMove, 0);
        }
        else if (distanceMovedSoFar < 70)
        {
            getMotion = new Vector3(-DistanceToMove, 0, 0);
        }
        else {
            getMotion = new Vector3(DistanceToMove, 0, 0);
        }

        if (distanceMovedSoFar > 89 && !loadingFadout)
        {
            loadingFadout = true;
            //SceneManager.LoadScene("TitleScreen");
            //GameObject.Find("SnowParticles").SetActive(false);
            FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
            fadeout.attachToGUI(thisCanvas);
            fadeout.InitNext("Credits", 4f);
        }

        transform.position = transform.position + getMotion;
    }
}
