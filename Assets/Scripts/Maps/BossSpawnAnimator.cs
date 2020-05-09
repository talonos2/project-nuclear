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
    public float yPositionSpawning=-10.5f;
    public float xPositionSpawning = 0;

    void Start()
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
