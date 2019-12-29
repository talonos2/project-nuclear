using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewCrystalLevelController : MonoBehaviour
{
    public bool healthCrystal;
    public bool manaCrystal;
    public bool attackCrystal;
    public bool defenseCrystal;
    public GameObject crystalPanel;
    public Text startingCrystals;
    public Text crystalBonus;
    // Start is called before the first frame update
    void Start()
    {
        startingCrystals.text = "0/100";
        crystalBonus.text = "+60";
        crystalPanel.transform.localScale = new Vector3(.5f,1,1) ;
        if (manaCrystal) crystalBonus.text = "+40";

        //Make functions in CrystalBuffManager to pull the next bonus level from crystals and apply them as needed. 
        //Make something to slowly fill bar once determined the difference in levels, including when doing more than one level at once. Maybe it does one level at a time, 
        //aka recalc bar size upon 'load' and upon hitting a new 'tier'. 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
