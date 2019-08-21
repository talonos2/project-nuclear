using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadowLoader : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture shadowTexture;
    private Renderer sRender;
    private GameObject ThePlayer;
    void Start()
    {
        shadowTexture= GameObject.Find("Ground").GetComponent<GroundShadow>().shadowTexture;
        this.sRender = this.GetComponentInChildren<Renderer>();
        sRender.material.SetTexture("_Shadows", shadowTexture);
        PassabilityGrid passGrid = GameObject.Find("Grid").GetComponent<PassabilityGrid>();
        Vector4 tempVector = new Vector4(passGrid.width, passGrid.height, 0, 0);
        sRender.material.SetVector("_MapXY", tempVector);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");

        //.SetTexture("Shadows", shadowTexture);
        // foreach (var component in this.GetComponents<Component>())
        //{
        //     if (component != this) Debug.Log(component.GetType());
        // }
    }

    // Update is called once per frame
    void Update()
    {

        sRender.material.SetVector("_HeroXY", ThePlayer.transform.position/5);
    }
}
