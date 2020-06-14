using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    private float delayWithCleanMap=6;
    private bool cleanMap;
    public GameObject finalMap;
    public Texture finalMapShadow;
    public GameObject crystalObject;
    public Texture newCrystals;
    public GameObject slime;
    public GameObject water;
    internal GroundShadow groundShadow;
    protected Renderer sMapRender;
    protected Renderer sCrystalRender;



    void Start()
    {
        sMapRender= finalMap.GetComponent<Renderer>();
        sCrystalRender= crystalObject.GetComponent<Renderer>();
        sMapRender.material = new Material(sMapRender.material);
        sCrystalRender.material = new Material(sCrystalRender.material);
        groundShadow = finalMap.GetComponent<GroundShadow>();


    }

        // Update is called once per frame
        void Update()
    {
        if (!GameData.Instance.victory)
            return;
        GameState.fullPause = true;
        if (groundShadow.resetShadow){
            groundShadow.resetShadow = false;
        }
        delayWithCleanMap -= Time.deltaTime;
        if (cleanMap) {
            if (delayWithCleanMap <= 0) {
                SceneManager.LoadScene("WinScreen");
            }
        }
        else if (delayWithCleanMap <= 5) {
            cleanMap = true;
            CleanTheMap();
        }
    }

    private void CleanTheMap()
    {
        //change crystals and fog maps.
        //finalMapMaterial
        sMapRender.material.SetTexture("_Shadows",finalMapShadow);
        //sMapRender.material.
        sCrystalRender.material.mainTexture = newCrystals;
        groundShadow.shadowTexture = finalMapShadow;
        groundShadow.resetShadow = true;
        slime.gameObject.SetActive(false);
        water.gameObject.SetActive(true);

    }
}
