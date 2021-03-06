﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadowLoader : MonoBehaviour
{
    // Start is called before the first frame update
    private GroundShadow groundObject;
    private Texture shadowTexture;
    private Texture glowTexture;
    private Renderer sRender;
    private GameObject ThePlayer;
    public bool actualGround;
    public float additionalZOffset;
    public bool groundItem;
    public bool belowGroundItem;
    public bool aboveItem;
    public bool thePlayerjumping;
    public bool spikes;
    private float xPosition;
    private float yPostioin;
    public int lightRadius=6;
    public bool isOnCutsceneMap;
    public bool isThisJohnDoe;
    SpriteMovement mover;
    SpriteMovement pMover;
    public bool isAPlayer;
    public bool isAMonster;
    void Start()
    {
        groundObject = GameObject.Find("Ground").GetComponent<GroundShadow>();
        if (isOnCutsceneMap) groundObject = GameObject.Find("Ground2").GetComponent<GroundShadow>();
        shadowTexture = groundObject.shadowTexture;
        glowTexture = groundObject.glowTexture;
        this.sRender = this.GetComponentInChildren<Renderer>();
        sRender.material.SetTexture("_Shadows", shadowTexture);
        sRender.material.SetTexture("_Glow", glowTexture);
        PassabilityGrid passGrid = GameObject.Find("Grid").GetComponent<PassabilityGrid>();
        if (isOnCutsceneMap) passGrid = GameObject.Find("Grid2").GetComponent<PassabilityGrid>();
        Vector4 tempVector = new Vector4(passGrid.width, passGrid.height, 0, 0);
        sRender.material.SetVector("_MapXY", tempVector);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
        //sRender.material.SetInt("_LightRad", ThePlayer.GetComponentInChildren<Renderer>().material.GetInt("_LightRad"));
        sRender.material.SetInt("_LightRad", 6);
        sRender.material.SetVector("_CutsceneMapOffset", groundObject.shadowMapOffset);

        if (GameData.Instance.FloorNumber == 0)
            sRender.material.SetInt("_LightRad", 0);

        transform.position = transform.position + new Vector3(groundObject.mapOffset.x, groundObject.mapOffset.y, 0);
        xPosition = transform.localPosition.x;
        yPostioin = transform.localPosition.y;

        if (isAMonster)
        {
            //Case for monsters
            mover = transform.parent.GetComponent<SpriteMovement>();

       }
        if (isAPlayer && this.transform.parent.parent.GetComponent<SpriteMovement>() != null)
        {
            //Case for Player
            pMover = transform.parent.parent.GetComponent<SpriteMovement>();
        }
    }

    public void setOnCutsceneMap() {
        groundObject = GameObject.Find("Ground2").GetComponent<GroundShadow>();
        shadowTexture = groundObject.shadowTexture;
        glowTexture = groundObject.glowTexture;
        this.sRender = this.GetComponentInChildren<Renderer>();
        sRender.material.SetTexture("_Shadows", shadowTexture);
        sRender.material.SetTexture("_Glow", glowTexture);
        PassabilityGrid passGrid = GameObject.Find("Grid2").GetComponent<PassabilityGrid>();
        Vector4 tempVector = new Vector4(passGrid.width, passGrid.height, 0, 0);
        sRender.material.SetVector("_MapXY", tempVector);
        if (GameData.Instance.FloorNumber != 0)  ThePlayer = GameObject.FindGameObjectWithTag("Player");
        //sRender.material.SetInt("_LightRad", ThePlayer.GetComponentInChildren<Renderer>().material.GetInt("_LightRad"));
        sRender.material.SetInt("_LightRad", 6);
        sRender.material.SetVector("_CutsceneMapOffset", groundObject.shadowMapOffset);
        if (GameData.Instance.FloorNumber == 0)
            sRender.material.SetInt("_LightRad", 0);

        transform.position = transform.position + new Vector3(groundObject.mapOffset.x, groundObject.mapOffset.y, 0);
        xPosition = transform.localPosition.x;
        yPostioin = transform.localPosition.y;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posit;
        if (GameData.Instance.FloorNumber != 0) {
            posit = ThePlayer.transform.GetChild(0).GetChild(0).position;
            sRender.material.SetVector("_HeroXY", posit);//shouldn't it be the sprite position rather than the player position?
        }
          
        sRender.material.SetInt("_LightRad", lightRadius);
        this.transform.localPosition = new Vector3(xPosition,yPostioin,CalculateZCoor());
        if (groundObject.resetShadow) {
            sRender.material.SetTexture("_Shadows", groundObject.shadowTexture);
        }
    }

    private float CalculateZCoor()
    {
        float zPosition= - 10 + this.transform.position.y / 100;
        if (isThisJohnDoe && GameData.Instance.RunNumber<15) {
            belowGroundItem = true;
        }

        if (belowGroundItem)
            zPosition = 1;
        if (actualGround)
            zPosition = 0;
        if (groundItem)
            zPosition = -1;
        if (aboveItem)
            zPosition = -20;
        if (mover!=null)
        {
            //Case for monsters
            if (mover.IsInAForcedJump())
            {
                zPosition -= .01f;
                //zPosition -= this.transform.parent.transform.localPosition.y / 100;
            }
        }

        if (pMover != null)
        {
            //The case of the player jumping
            if ((pMover.IsInAForcedJump() || pMover.IsJumping()))
            {
                zPosition -= .01f;
                //zPosition -= this.transform.parent.transform.localPosition.y / 100;
            }
        }

        if (spikes) {
            zPosition += .01f;
        }
        return zPosition+ additionalZOffset;
    }
}
