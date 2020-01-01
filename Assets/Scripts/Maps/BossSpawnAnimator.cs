using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public int totalFrames;
    public float framesPerSecond;
    private float frameCounter = 0;
    private int currentFrame = 0;
    protected Renderer sRender;
    protected GameObject ThePlayer;
    private bool spawnBoss;
    public GameObject bossSpawnedPrefab;
    private float offsetFix = .00001f;

    void Start()
    {
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (ThePlayer.transform.position.y>-9.5f&& !spawnBoss) {
            spawnBoss = true;
            sRender.enabled = true;
            sRender.material.SetFloat("_Frame", currentFrame+offsetFix);
            Debug.Log("Did I transform?");
        }
        if (!spawnBoss) {
            return;
        }
        frameCounter += Time.deltaTime;
        if (frameCounter > 1 / framesPerSecond)
        {
            currentFrame += 1;
            if (currentFrame >= totalFrames)
            {
                Instantiate(bossSpawnedPrefab, this.transform.position + new Vector3(0, -.75f, -10), Quaternion.identity);
                Destroy(this.gameObject);
            }
            else {
                sRender.material.SetFloat("_Frame", currentFrame+ offsetFix);
                frameCounter = 0;
            }



        }
    }
}
