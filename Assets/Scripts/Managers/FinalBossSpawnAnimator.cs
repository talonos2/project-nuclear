using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossSpawnAnimator : BossSpawnAnimator
{
    // Start is called before the first frame update
     new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.fullPause) return;
        if (!spawnBoss)
        {
            return;
        }
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
