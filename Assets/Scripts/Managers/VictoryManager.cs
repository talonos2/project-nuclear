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
    public Texture finalMapGlow;
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
        if (!GameData.Instance.victory || GameState.fullPause)
            return;
        GameData.Instance.isInDialogue = true;
        if (groundShadow.resetShadow){
            groundShadow.resetShadow = false;
        }
        delayWithCleanMap -= Time.deltaTime;
        if (cleanMap) {
            if (delayWithCleanMap <= 0) {
                LoadSaveController saving = new LoadSaveController();

                int victory = GameData.Instance.Mara + GameData.Instance.Sara + GameData.Instance.Pendleton + GameData.Instance.Douglass + GameData.Instance.Norma
                    + GameData.Instance.Derringer + GameData.Instance.Melvardius + GameData.Instance.Todd + GameData.Instance.Devon + GameData.Instance.McDermit;
                if (victory == 0) GameData.Instance.Worst = 1;
                if (victory != 10) GameData.Instance.Perfect = 0;
                saving.AutoSave();

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
        sMapRender.material.SetTexture("_Glow", finalMapGlow);
        //sMapRender.material.
        sCrystalRender.material.mainTexture = newCrystals;
        groundShadow.shadowTexture = finalMapShadow;
        groundShadow.glowTexture = finalMapGlow;
        groundShadow.resetShadow = true;
        slime.gameObject.SetActive(false);
        water.gameObject.SetActive(true);

    }
}
