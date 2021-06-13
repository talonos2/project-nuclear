using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHelperTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject thePlayer;
    private SpriteRenderer toggleArrow;
    private SpriteRenderer activateArrow;
    public float timer = 0;
    public bool showArrowsAtAll;
    public bool showActivateArrow;
    public bool showToggleArrow;
    public bool disableTutorial;
    private CharacterStats characterStats;
    private EntityData entityData;
    void Start()
    {

        thePlayer = GameObject.FindGameObjectWithTag("Player");
        characterStats = thePlayer.GetComponent<CharacterStats>();
        entityData = thePlayer.GetComponent<EntityData>();
        toggleArrow = GameObject.Find("ToggleArrow").GetComponent<SpriteRenderer>();
        activateArrow = GameObject.Find("ActivateArrow").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.isInBattle || GameState.getFullPauseStatus()) { return; }
        timer += Time.deltaTime;
        if (timer > 25) {
            showArrowsAtAll = true;
        }
        if (entityData.distanceToEntity(this.transform) < .5) {
            disableTutorial = true;
        }
        if (disableTutorial) {
            toggleArrow.enabled=false;
            activateArrow.enabled = false;
            return;
        }
        if (showArrowsAtAll) {
            if (characterStats.currentPower == 1)
            {
                activateArrow.enabled = true;
                toggleArrow.enabled = false;
            }
            else {
                activateArrow.enabled = false;
                toggleArrow.enabled = true;
            }
        }

    }
}
