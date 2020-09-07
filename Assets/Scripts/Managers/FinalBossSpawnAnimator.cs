using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossSpawnAnimator : BossSpawnAnimator
{
    protected Renderer sRenderToSky;
    public GameObject spriteBossToSky;
    public GameObject spriteBossToGround;
    private bool phase1;
    private bool initialwaiting;
    public int totalInSkyFrames;
    public float framesInSkyPerSecond;
    public GameObject slimeOnCrystalBase;

    // Start is called before the first frame update
    new void Start()
    {
        this.sRender = spriteBossToGround.GetComponent<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        this.sRenderToSky = spriteBossToSky.GetComponent<Renderer>();
        this.sRenderToSky.material = new Material(this.sRenderToSky.material);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
        phase1 = true;
        initialwaiting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.fullPause) return;
        if (!spawnBoss)
        {
            return;
        }

        if (initialwaiting)
        {
            frameCounter += Time.deltaTime;
            if (frameCounter > 3.33f) {
                frameCounter = 0;
                initialwaiting = false;
            }
        }          
        else if (phase1) {
            sRenderToSky.enabled = true;
            sRenderToSky.material.SetFloat("_Frame", currentFrame + offsetFix);

            frameCounter += Time.deltaTime;
            Destroy(slimeOnCrystalBase);
            if (frameCounter > 1 / framesInSkyPerSecond)
            {
                currentFrame += 1;
                if (currentFrame == 3)
                { SoundManager.Instance.PlaySound("BossDrop", 1); }
                if (currentFrame >= totalInSkyFrames)
                {
                    //Instantiate(bossSpawnedPrefab, this.transform.parent.position, Quaternion.identity);
                    currentFrame = 0;
                    phase1 = false;
                    sRenderToSky.enabled = false;
                    frameCounter = 0;
                }
                else
                {
                    sRenderToSky.material.SetFloat("_Frame", currentFrame + offsetFix);
                    frameCounter = 0;
                }
            }

        }
        else
        {
            sRender.enabled = true;
            sRender.material.SetFloat("_Frame", currentFrame + offsetFix);
            shadowAdded.enabled = true;
            frameCounter += Time.deltaTime;

            /*Debug.Log("frameCounter "+ frameCounter);
            if (frameCounter >= BOSS_SPAWN_SOUND_START_TIME&&!playedSpawnSound)
            {
                Debug.Log("Here, after spawning sound?");
                SoundManager.Instance.PlaySound("SlimeBossSpawn", 1);
                playedSpawnSound = true;
            }*/

            if (frameCounter > 1 / framesPerSecond)
            {
                currentFrame += 1;
                if (currentFrame == 3)
                { SoundManager.Instance.PlaySound("SlimeBossSpawn", 1); }
                if (currentFrame >= totalFrames)
                {
                    Instantiate(bossSpawnedPrefab, this.transform.parent.position, Quaternion.identity);
                    Destroy(this.transform.parent.gameObject);
                }
                else
                {
                    sRender.material.SetFloat("_Frame", currentFrame + offsetFix);
                    frameCounter = 0;
                }
            }

        }

    }
}
