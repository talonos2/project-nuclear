using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map2_3ShortcutController : MonoBehaviour
{
    public GameObject boulderInRiver;
    public GameObject ground;
    public PassabilityGrid grid;
    public GameObject boulder1;
    public GameObject boulder2;
    public GameObject boulder3;
    public EnvironmentalSoundMagnitudeGrid environmentGrid;
    public TextAsset newPassabilityMap;
    public TextAsset newSoundMap;
    public Texture newMap;


    // Start is called before the first frame update
    void Start()
    {

        setupShortcut();
    }

    public void setupShortcutForCutscene() {
        grid = GameObject.Find("Grid2").GetComponent<PassabilityGrid>();
        Debug.Log("Am I setting up the shortcut");
        if (GameData.Instance.map1_3toMap2_3Shortcut)
        {
            boulder1.SetActive(true);
            boulder2.SetActive(true);
            boulder3.SetActive(true);
            boulderInRiver.SetActive(true);
            grid.passabilityMap = newPassabilityMap;
            grid.configurePathabilityGrid();
            ground.GetComponent<Renderer>().material.mainTexture = newMap;
            environmentGrid.passabilityMap = newSoundMap;
            environmentGrid.configureSoundGrid();
        }
        else
        {
            boulder1.SetActive(false);
            boulder2.SetActive(false);
            boulder3.SetActive(false);
            boulderInRiver.SetActive(false);
        }
    }
    public void setupShortcut() {

        if (GameData.Instance.map1_3toMap2_3Shortcut)
        {
            boulder1.SetActive(true);
            boulder2.SetActive(true);
            boulder3.SetActive(true);
            boulderInRiver.SetActive(true);
            grid.passabilityMap = newPassabilityMap;
            grid.configurePathabilityGrid();
            ground.GetComponent<Renderer>().material.mainTexture = newMap;
            environmentGrid.passabilityMap = newSoundMap;
            environmentGrid.configureSoundGrid();
        }
        else
        {
            boulder1.SetActive(false);
            boulder2.SetActive(false);
            boulder3.SetActive(false);
            boulderInRiver.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
