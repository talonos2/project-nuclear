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
    void Start()
    {
        groundObject = GameObject.Find("Ground").GetComponent<GroundShadow>();
        shadowTexture = groundObject.shadowTexture;
        glowTexture = groundObject.glowTexture;
        this.sRender = this.GetComponentInChildren<Renderer>();
        sRender.material.SetTexture("_Shadows", shadowTexture);
        sRender.material.SetTexture("_Glow", glowTexture);
        PassabilityGrid passGrid = GameObject.Find("Grid").GetComponent<PassabilityGrid>();
        Vector4 tempVector = new Vector4(passGrid.width, passGrid.height, 0, 0);
        sRender.material.SetVector("_MapXY", tempVector);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
        //sRender.material.SetInt("_LightRad", ThePlayer.GetComponentInChildren<Renderer>().material.GetInt("_LightRad"));
        sRender.material.SetInt("_LightRad", 6);

        transform.position=transform.position + new Vector3(groundObject.mapOffset.x, groundObject.mapOffset.y, 0);

    }

    // Update is called once per frame
    void Update()
    {

        sRender.material.SetVector("_HeroXY", ThePlayer.transform.position);//shouldn't it be the sprite position rather than the player position?
    }
}
