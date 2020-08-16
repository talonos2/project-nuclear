using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCaroselManager : MonoBehaviour
{
    public Sprite[] offSprites;
    public Sprite[] onSprites;
    public ParticleSystem useParticles;

    public SpriteRenderer thereIsOnlyOneRightNowLaterThereWillBeMore;

    int elementIThinkIsSelected = 0;
    bool doIThinkImBeingUsed = false;

    CharacterStats stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        thereIsOnlyOneRightNowLaterThereWillBeMore.gameObject.SetActive(stats.powersGained > 0);
        if (elementIThinkIsSelected != stats.currentPower || doIThinkImBeingUsed != IsPowerBeingUsed())
        {
            elementIThinkIsSelected = stats.currentPower;
            doIThinkImBeingUsed = IsPowerBeingUsed();
            thereIsOnlyOneRightNowLaterThereWillBeMore.sprite = (doIThinkImBeingUsed ? onSprites[elementIThinkIsSelected] : offSprites[elementIThinkIsSelected]);
        }
    }

    private bool IsPowerBeingUsed()
    {
        switch (elementIThinkIsSelected)
        {
            case 0:
                return false;
            case 1:
                return GameData.Instance.dashing;
            case 2:
                return GameData.Instance.stealthed;
            case 3:
                return GameData.Instance.hasted;
            case 4:
                return GameData.Instance.jumping;
            default:
                Debug.LogWarning("Something went wrong: Power is not 0-4. HAX!"); //Honeslty we've probs already thrown an AOoB Exception...
                return false;
        }

    }
}
