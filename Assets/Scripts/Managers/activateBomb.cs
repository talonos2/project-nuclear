using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateBomb : EntityData
{
    public GameObject finalMap;
    public Texture newMap;
    public Texture newShadowmap;
    public TextAsset newPathabilityMap;
    public PassabilityGrid currentPassabilityGrid;
    public GameObject currentGround;
    public GameObject mushrooms1;
    public GameObject mushrooms2;
    public GameObject rocks;
    public GameObject bomb;
    public GameObject monster1;
    public GameObject monster2;
    public GameObject monster3;
    public GabTextController gabTextController;
    protected Renderer sMapRender;
    protected Renderer sCrystalRender;
    internal GroundShadow groundShadow;
    private bool waiting;
    private float waitingLeft;
    // Start is called before the first frame update
    void Start()
    {
        mapGrid = GameObject.Find("Grid");
        mapZeroLocation = mapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        entityLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        entityLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
        mapEntityGrid = mapGrid.GetComponent<EntityGrid>();
        mapEntityGrid.grid[entityLocation.x, entityLocation.y] = this.gameObject;
        sMapRender = finalMap.GetComponent<Renderer>();
        sMapRender.material = new Material(sMapRender.material);
        groundShadow = finalMap.GetComponent<GroundShadow>();
        if (GameData.Instance.map2_4Shortcut) {
            detonateBomb();
        }
    }

    void Update()
    {
        if (GameState.getFullPauseStatus() || !waiting) return;

        waitingLeft -= Time.deltaTime;
        if (waitingLeft < 0) {
            SoundManager.Instance.PlaySound("Explosion", 1);
            detonateBomb();
            GameState.isInBattle = false;
            GameData.Instance.isCutscene = false;
            waiting = false;
        }

    }
        private void detonateBomb() {
        currentGround.GetComponent<Renderer>().material.mainTexture = newMap;
        currentPassabilityGrid.passabilityMap = newPathabilityMap;
        currentPassabilityGrid.configurePathabilityGrid();

        sMapRender.material.SetTexture("_Shadows", newShadowmap);
        groundShadow.shadowTexture = newShadowmap;

        removeEntity();
        Destroy(mushrooms1);
        Destroy(rocks);
        Destroy(mushrooms2);
        Destroy(bomb);
        Destroy(monster1);
        Destroy(monster2);
        Destroy(monster3);
        Destroy(this.gameObject);
    }

    public override void ProcessClick(CharacterStats stats)
    {
        if (waiting) return;
        if (stats.currentPower != 3) {
            gabTextController.AddGabToPlay("You might be able to light the fuse if only you had some fire magic.");
        }
        if (stats.currentPower == 3 && stats.mana >= 24)
        {
            stats.HP -= 150;
            if (stats.HP < 1) { stats.HP = 1; }
            stats.mana -= 24;
            GameData.Instance.map2_4Shortcut = true;
            SoundManager.Instance.PlaySound("fuseForBomb", 1);
            waiting = true;
            waitingLeft = 2f;
            GameData.Instance.isCutscene = true;
            GameState.isInBattle = true;
        }
        else if (stats.currentPower == 3 && stats.mana <= 24) { gabTextController.AddGabToPlay("Not enough mana to light the fuse."); }
    }
}
