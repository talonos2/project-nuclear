using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    private float delayWithCleanMap=10;
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
    public GameObject[] bubbleSpwanersToClose;

    public Renderer[] GooFallMaterialsToChange;
    public Renderer[] GooCrestMaterialsToChange;
    public Renderer[] GooMaterialsToChange;


    private bool playedParticleEffect = false;
    private float timeSinceSequenceStarted;


    void Start()
    {
        sMapRender = finalMap.GetComponent<Renderer>();
        sCrystalRender = crystalObject.GetComponent<Renderer>();
        sMapRender.material = new Material(sMapRender.material);
        sCrystalRender.material = new Material(sCrystalRender.material);
        groundShadow = finalMap.GetComponent<GroundShadow>();
    }

    private void ReplaceAll(Renderer[] toReplaceAllOf, Material toReplaceWith)
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameData.Instance.victory || GameState.getFullPauseStatus())
            return;

        if (!playedParticleEffect&& timeSinceSequenceStarted>1.1f)
        {
            this.GetComponent<ParticleSystem>().Play();
            playedParticleEffect = true;
            foreach (GameObject i in bubbleSpwanersToClose)
            {
                i.SetActive(false);
            }
            MusicManager.instance.StopAllMusic();
            GameData.Instance.pauseTimer = true;
        }

        timeSinceSequenceStarted += Time.deltaTime;
        float cleaningAmount = timeSinceSequenceStarted * 2 - 2;
        foreach (Renderer r in GooFallMaterialsToChange)
        {
            r.material.SetFloat("_gooCleanAmount", (cleaningAmount));
        }
        foreach (Renderer r in GooCrestMaterialsToChange)
        {
            r.material.SetFloat("_gooCleanAmount", (cleaningAmount));
        }
        foreach (Renderer r in GooMaterialsToChange)
        {
            r.material.SetFloat("_gooCleanAmount", (cleaningAmount));
        }

        GameData.Instance.isInDialogue = true;
        if (groundShadow.resetShadow){
            groundShadow.resetShadow = false;
        }
        delayWithCleanMap -= Time.deltaTime;
        if (cleanMap) {
            if (delayWithCleanMap <= 0) {
                LoadSaveController saving = new LoadSaveController();

                int villagersDead = GameData.Instance.VillagersDead();
                if (villagersDead == GameData.Instance.RunNumber - 1 || villagersDead==10)
                {
                    GameData.Instance.Worst = 1;
                }

                if (villagersDead == 0)
                {
                    GameData.Instance.Perfect = 1;
                }
                else GameData.Instance.Perfect = 0;

                saving.AutoSave();


                if (GameData.Instance.RunNumber <= 6)
                {
                    PersistentSaveDataManager.Instance.EndingsSeen[GameData.Instance.RunNumber - 1, 0] = true;
                }
                else
                {
                    if (GameData.Instance.Perfect == 1)
                    {
                        PersistentSaveDataManager.Instance.EndingsSeen[GameData.Instance.RunNumber - 1, 2] = true;
                    }
                    else if (GameData.Instance.Worst == 1)
                    {
                        PersistentSaveDataManager.Instance.EndingsSeen[GameData.Instance.RunNumber - 1, 0] = true;
                    }
                    else
                    {
                        PersistentSaveDataManager.Instance.EndingsSeen[GameData.Instance.RunNumber - 1, 1] = true;
                    }
                }
                FinalWinterAchievementManager.Instance.CheckEndingsSeen();
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
        //slime.gameObject.SetActive(false);
        water.gameObject.SetActive(true);

    }
}
