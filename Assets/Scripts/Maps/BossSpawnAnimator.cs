using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public int totalFrames;
    public float framesPerSecond;
    protected float frameCounter = 0;
    protected int currentFrame = 0;
    protected Renderer sRender;
    protected GameObject ThePlayer;
    internal bool spawnBoss;
    public GameObject bossSpawnedPrefab;
    protected float offsetFix = .00001f;
    public float yPositionSpawning=-10.5f;
    public float xPositionSpawning = 0;
    protected static readonly float BOSS_SPAWN_SOUND_START_TIME = .5f;
    protected bool playedSpawnSound = false;

    public void Start()
    {
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (ThePlayer.transform.position.y>= yPositionSpawning && ThePlayer.transform.position.x== xPositionSpawning && !spawnBoss) {
            spawnBoss = true;
            sRender.enabled = true;
            sRender.material.SetFloat("_Frame", currentFrame+offsetFix);
            //Debug.Log("Did I transform?");
        }
        if (!spawnBoss) {
            return;
        }
        sRender.enabled = true;
        sRender.material.SetFloat("_Frame", currentFrame + offsetFix);
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
