using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow_Interaction : EntityData
{

        private Vector2Int pawnLocation;
        //private GameObject mapGrid;
        //private Vector2 mapZeroLocation;
        //private Renderer sRender;
        private bool waitFrameAfterDialogue;
    private bool HittingScarecrow = false;
    private float timeSinceLastAnimation;
    public float framesPerSecond=6;
    protected float FLOATING_POINT_FIX = .00001f;
    private float frameNumber = 0;
    private EntityData playerEntityData;

    // Start is called before the first frame update
    void Start()
        {
            InitializeSpriteLocation();
            this.sRender = this.GetComponentInChildren<Renderer>();
            this.sRender.material = new Material(this.sRender.material);

        GameObject playerFinder = GameObject.FindGameObjectWithTag("Player");
        if (playerFinder) playerEntityData = playerFinder.GetComponent<EntityData>();
        else playerEntityData = this;

        }


        private void InitializeSpriteLocation()
        {
            mapGrid = GameObject.Find("Grid");
            mapZeroLocation = mapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
            pawnLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
            pawnLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
            mapGrid.GetComponent<EntityGrid>().grid[pawnLocation.x, pawnLocation.y] = this.gameObject;

        }

        public override void ProcessClick(CharacterStats stats)
        {
        if (GameState.fullPause == true || GameData.Instance.isInDialogue || HittingScarecrow) return;

        HittingScarecrow = true;
        float scarecrowDistance = playerEntityData.distanceToEntity(this.transform);
        if (scarecrowDistance < 8.9f) {
            float playSoundOnVolume = Math.Min(.9f - (scarecrowDistance / 9), .25f);
            SoundManager.Instance.PlaySound("Punching_Scarecrow", Math.Max(playSoundOnVolume,0));
        }

        

        }

    private void AnimateHit()
    {
        if (HittingScarecrow == false) return;

        timeSinceLastAnimation += Time.deltaTime;
        if (timeSinceLastAnimation >= 1 / framesPerSecond)
        {
            timeSinceLastAnimation = 0;
            frameNumber += 1;
            if (frameNumber == 6) {
                frameNumber = 0;
                HittingScarecrow = false;
            }
            sRender.material.SetFloat("_Frame", FLOATING_POINT_FIX + frameNumber);
        }

    }

    void Update()
    {
        AnimateHit();
    }
}
