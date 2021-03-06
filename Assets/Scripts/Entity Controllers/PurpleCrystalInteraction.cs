﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleCrystalInteraction : EntityData
{
    private Vector2Int pawnLocation;
  //  private GameObject mapGrid;
    private EntityGrid entityGrid;
   // private Vector2 mapZeroLocation;
 //   private Renderer sRender;
    public GameObject purpleCrystalBreaking;
    public GameObject monsterToSpawn;
    public float monsterSpawnDelayBase=10;
    public float monsterSpeed = 4;
    private float monsterSpawnDelay=10;
    private bool spawning = false;


    // Start is called before the first frame update
    void Start()
    {
        InitializeSpriteLocation();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        StartSpawningProcess();
        
    }


    private void InitializeSpriteLocation()
    {
        mapGrid = GameObject.Find("Grid");
        mapZeroLocation = mapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        pawnLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        pawnLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
        entityGrid = mapGrid.GetComponent<EntityGrid>();
        entityGrid.grid[pawnLocation.x, pawnLocation.y] = this.gameObject;

    }

    public override void ProcessClick(CharacterStats stats)
    {

        if (GameState.getFullPauseStatus() == true || GameData.Instance.isInDialogue) return;

        //SpawnNewMonster();

        Instantiate(purpleCrystalBreaking, this.transform.position , Quaternion.identity);
        SoundManager.Instance.PlaySound("CrystalShatter", 1);
        SoundManager.Instance.PlaySound("WraithSpawn", 1);
        Destroy(this.gameObject);

        //instantiate a broken crystal and destroy this object

    }

    public void SpawnNewMonster() {
        if (entityGrid.grid[pawnLocation.x, pawnLocation.y + 1] == null)
        {
            GameObject spanwedMonster= Instantiate(monsterToSpawn, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            MM_ChaseEverywhere monsterMovement= spanwedMonster.GetComponent<MM_ChaseEverywhere>();
            monsterMovement.SetSpawningCrystal(this.gameObject);
            monsterMovement.MoveSpeed = monsterSpeed;
        }
        else if (entityGrid.grid[pawnLocation.x + 1, pawnLocation.y + 1] == null) {
            GameObject spanwedMonster=Instantiate(monsterToSpawn, this.transform.position + new Vector3(1, 1, 0), Quaternion.identity);
            MM_ChaseEverywhere monsterMovement = spanwedMonster.GetComponent<MM_ChaseEverywhere>();
            monsterMovement.SetSpawningCrystal(this.gameObject);
            monsterMovement.MoveSpeed = monsterSpeed;
        }
        else if (entityGrid.grid[pawnLocation.x - 1, pawnLocation.y + 1] == null)
        {
            GameObject spanwedMonster=Instantiate(monsterToSpawn, this.transform.position + new Vector3(-1, 1, 0), Quaternion.identity);
            MM_ChaseEverywhere monsterMovement = spanwedMonster.GetComponent<MM_ChaseEverywhere>();
            monsterMovement.SetSpawningCrystal(this.gameObject);
            monsterMovement.MoveSpeed = monsterSpeed;
        }
        SoundManager.Instance.PlaySound("WraithSpawn", 1);


    }

    internal void StartSpawningProcess()
    {
        spawning = true;
        monsterSpawnDelay = monsterSpawnDelayBase;
    }

    void Update()
    {

        if (GameState.isInBattle || GameState.getFullPauseStatus())
        {
            return;
        }
        if (spawning) {
            monsterSpawnDelay -= Time.deltaTime;
            if (monsterSpawnDelay <= 0) {
                SpawnNewMonster();
                spawning = false;
            }
                
        }

    }
}
