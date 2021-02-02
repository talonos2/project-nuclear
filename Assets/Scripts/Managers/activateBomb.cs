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
    public GameObject explosion;
    public GameObject fusedBomb;
    public GameObject bombEmissive;
    private bool playingFuseAnimation;
    private bool playingExplosionAnimation;
    protected Renderer sRender1;
    protected Renderer sRender2;
    public int maxFrames=12;
    public float framesPerSecond = 18;
    protected float timeSinceLastFrame = 0;
    protected float offsetFix = .00001f;
    protected int currentFrame = 0;
    private CharacterStats stats;
    public GameObject sparklies;
    // Start is called before the first frame update
    void Start()
    {
        if (GameData.Instance.PowersGained >= 3) {
            sparklies.SetActive(true);
        }
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
        if (GameState.getFullPauseStatus() ) return;

        if (playingFuseAnimation) {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame >= (1f / framesPerSecond))
            {
                timeSinceLastFrame = 0;
                currentFrame += 1;
                if (currentFrame == maxFrames)
                {
                    playingFuseAnimation = false;
                    playingExplosionAnimation = true;
                    currentFrame = 0;
                    SoundManager.Instance.PlaySound("Explosion", 1);
                    fusedBomb.SetActive(false);
                    bombEmissive.SetActive(false);
                    explosion.SetActive(true);
                    this.sRender2 = explosion.GetComponentInChildren<MeshRenderer>();
                    this.sRender2.material = new Material(this.sRender2.material);
                    sRender2.material.SetFloat("_Frame", currentFrame + offsetFix);

                }
                else {
                    sRender1.material.SetFloat("_Frame", currentFrame + offsetFix);
                }
                
            }
        }

        if (playingExplosionAnimation)
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame >= (1f / framesPerSecond))
            {
                timeSinceLastFrame = 0;
                currentFrame += 1;
                if (currentFrame == 1) {
                    detonateBombCutscene();
                    stats.HP -= 150;
                    if (stats.HP < 1) { stats.HP = 1; }
                }

                if (currentFrame == maxFrames)
                {
                    playingExplosionAnimation = false;
                    currentFrame = 0;
                    explosion.SetActive(false);
                    GameState.isInBattle = false;
                    GameData.Instance.isCutscene = false;
                    Destroy(this.gameObject);
                }
                else
                {
                    sRender2.material.SetFloat("_Frame", currentFrame + offsetFix);
                }
            }
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

    private void detonateBombCutscene()
    {
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

    }



    public override void ProcessClick(CharacterStats stats)
    {
        if (GameState.getFullPauseStatus() || GameData.Instance.isCutscene) return;
        this.stats = stats;
        if (stats.currentPower != 3) {
            gabTextController.AddGabToPlay("You might be able to light the fuse if only you had some fire magic.");
        }
        if (stats.currentPower == 3 && stats.mana >= 24)
        {

            playingFuseAnimation = true;
            bomb.SetActive(false);
            fusedBomb.SetActive(true);
            bombEmissive.SetActive(true);
            this.sRender1 = fusedBomb.GetComponentInChildren<MeshRenderer>();
            this.sRender1.material = new Material(this.sRender1.material);
            sRender1.material.SetFloat("_Frame", currentFrame + offsetFix);
            SoundManager.Instance.PlaySound("fuseForBomb", 1);
            stats.mana -= 24;
            GameData.Instance.map2_4Shortcut = true;
            GameData.Instance.isCutscene = true;
            GameState.isInBattle = true;
        }
        else if (stats.currentPower == 3 && stats.mana < 24) { gabTextController.AddGabToPlay("Not enough mana to light the fuse."); }
    }
}
