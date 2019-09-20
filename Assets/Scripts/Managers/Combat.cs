using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private static readonly float ENTER_TIME = .5f;

    //Starts a fight, creating a new game object with an instance of this monobehavior.
    internal static void InitiateFight(GameObject Player, GameObject monster)
    {
        Enemy monsterStats = monster.GetComponent<Enemy>();
        CharacterStats playerStats = Player.GetComponent<CharacterStats>();

        GameObject go = new GameObject();
        Combat toInit = go.AddComponent<Combat>();

        toInit.Init(monsterStats, playerStats, monster);

        //Flipping this switch should pause all map-related stuff in the scene. The combat component we just created will flip it off when combat concludes.
        GameState.isInBattle = true;
    }

    public Enemy monsterStats;
    public CharacterStats playerStats;
    private GameObject monsterToDelete;
    private float enterTimer;
    private float combatTimer;
    private bool combatEnded;

    private GameObject monsterSprite;
    private GameObject playerSprite;
    private SpriteRenderer pRenderer;
    private SpriteRenderer eRenderer;

    private GameObject combatFolder;
    private int phaseLastFrame;

    GameObject hitsplatTemplate;

    private void Init(Enemy monsterStats, CharacterStats playerStats, GameObject monsterToDelete)
    {
        AttackAnimationManager aam = AttackAnimationManager.Instance;
        aam.LoadCombatPawns(monsterStats, playerStats);
        this.monsterStats = monsterStats;
        this.playerStats = playerStats;
        this.monsterToDelete = monsterToDelete;

        //Create monster sprites:

        monsterSprite = new GameObject("Monster Sprite");
        playerSprite = new GameObject("Player Sprite");

        pRenderer = playerSprite.AddComponent<SpriteRenderer>();
        pRenderer.flipX = true;
        pRenderer.sprite = Resources.LoadAll<Sprite>("DELETE LATER")[1];
        pRenderer.sortingOrder = 2;

        eRenderer = monsterSprite.AddComponent<SpriteRenderer>();
        eRenderer.flipX = true;
        eRenderer.sprite = monsterStats.combatSprites[0];
        eRenderer.sortingOrder = 1;

        combatFolder = Camera.main.transform.Find("UI").Find("Combat").gameObject;

        playerSprite.transform.SetParent(combatFolder.transform);
        playerSprite.transform.localPosition = playerStats.startPositionOnScreen;

        monsterSprite.transform.SetParent(combatFolder.transform);
        monsterSprite.transform.localPosition = monsterStats.startPositionOnScreen;

        SetMonsterAndPlayerScale();

        hitsplatTemplate = Resources.Load<GameObject>("Hitsplat");
    }

    private void SetMonsterAndPlayerScale()
    {
        Vector3 pScale = (Vector3)(playerStats.scale * monsterStats.forceOpponentAdditionalScale) + new Vector3(0, 0, 1);
        playerSprite.transform.localScale = pScale*1/6;

        Vector3 mScale = (Vector3)(playerStats.scale * monsterStats.forceOpponentAdditionalScale) + new Vector3(0, 0, 1);
        monsterSprite.transform.localScale = pScale * 1 / 6;
    }

    public void Update()
    {
        if (enterTimer < ENTER_TIME)
        {
            HandleEntranceRoutine();
        }
        else if (!combatEnded)
        {
            HandleCombatLoop();
        }
        else
        {
            Destroy(monsterSprite.gameObject);
            Destroy(playerSprite.gameObject);
            Destroy(this);
        }

        return;
    }

    bool playerDidDamageRecently;
    bool monsterDidDamageRecently;
    float previousTimeSinceLastMonsterAttack = -1000;
    float previousTimeSinceLastPlayerAttack = -1000;

    private void HandleCombatLoop()
    {
        combatTimer += Time.deltaTime;

        if (Application.isEditor)
        {
            SetMonsterAndPlayerScale();
        }

        //float playerAttackTime = playerStats.animation.GetAnimationLength();
        float playerAttackTime = AttackAnimation.HOP.GetAnimationLength();
        //float enemyAttackTime = playerStats.animation.GetAnimationLength();
        float enemyAttackTime = AttackAnimation.HOP.GetAnimationLength();
        float totalTime = playerAttackTime + enemyAttackTime;

        float totalAnimationTime = AttackAnimation.HOP.GetAnimationLength() + AttackAnimation.HOP.GetAnimationLength();
        float timeSinceLastPlayerAttack = combatTimer % totalAnimationTime;
        float timeSinceLastMonsterAttack = (combatTimer > enemyAttackTime ? (combatTimer - enemyAttackTime) % totalAnimationTime : 0);

        Debug.Log("Player:" + playerStats.homePositionOnScreen);

        int playerFrame = AttackAnimation.HOP.HandleAnimation(timeSinceLastPlayerAttack, playerSprite, monsterSprite, monsterStats, playerStats);
        int enemyFrame = AttackAnimation.HOP.HandleAnimation(timeSinceLastMonsterAttack, monsterSprite, playerSprite, playerStats, monsterStats);
        playerSprite.GetComponent<SpriteRenderer>().sprite = playerStats.combatSprites[playerFrame];
        monsterSprite.GetComponent<SpriteRenderer>().sprite = monsterStats.combatSprites[enemyFrame];

        //Handle damage:
        if (timeSinceLastPlayerAttack < previousTimeSinceLastPlayerAttack)
        {
            playerDidDamageRecently = false;
        }
        if (timeSinceLastMonsterAttack < previousTimeSinceLastMonsterAttack)
        {
            monsterDidDamageRecently = false;
        }
        if (!playerDidDamageRecently && timeSinceLastPlayerAttack > AttackAnimation.HOP.GetDamagePoint())
        {
            this.DealDamageToEnemy();
            playerDidDamageRecently = true;
        }
        if (!monsterDidDamageRecently && timeSinceLastMonsterAttack > AttackAnimation.HOP.GetDamagePoint())
        {
            this.DealDamageToPlayer();
            monsterDidDamageRecently = true;
        }
        previousTimeSinceLastMonsterAttack = timeSinceLastMonsterAttack;
        previousTimeSinceLastPlayerAttack = timeSinceLastPlayerAttack;
    }

    private void CheckCombatOver()
    {
        //TODO: If the time is up, combat ends immediately.
        if (false/*TimeIsUp()*/)
        {
            combatEnded = true;
            GameState.endRunFlag = true;
            GameState.isInBattle = false;
        }
        if (playerStats.HP <= 0)
        {
            combatEnded = true;
            GameState.endRunFlag = true;
            GameState.isInBattle = false;
        }
        if (monsterStats.HP <= 0)
        {
            combatEnded = true;
            KillMonsterAndGetRewards();
            GameState.isInBattle = false;
        }
    }

    private void KillMonsterAndGetRewards()
    {
        switch (monsterStats.crystalType)
        {
            case CrystalType.All:
                playerStats.AttackCrystalsGained += monsterStats.crystalDropAmount;
                playerStats.defenseCrystalsGained += monsterStats.crystalDropAmount;
                playerStats.HealthCrystalsGained += monsterStats.crystalDropAmount;
                playerStats.ManaCrystalsGained += monsterStats.crystalDropAmount;
                break;
            case CrystalType.ATTACK:
                playerStats.AttackCrystalsGained += monsterStats.crystalDropAmount;
                break;
            case CrystalType.DEFENSE:
                playerStats.defenseCrystalsGained += monsterStats.crystalDropAmount;
                break;
            case CrystalType.HEALTH:
                playerStats.HealthCrystalsGained += monsterStats.crystalDropAmount;
                break;
            case CrystalType.MANA:
                playerStats.ManaCrystalsGained += monsterStats.crystalDropAmount;
                break;
            default:
                break;
        }
        playerStats.AddExp(monsterStats.ExpGiven);
        playerStats.PushCharacterData();
        Destroy(monsterToDelete);
    }

    private void DealDamageToEnemy()
    {
        float incomingDamage = (int)playerStats.attack + playerStats.weaponBonusAttack;
        incomingDamage -= monsterStats.defense;
        incomingDamage = Math.Max(incomingDamage, 0);
        monsterStats.HP -=  Mathf.RoundToInt(incomingDamage);
        GameObject hitsplat = GameObject.Instantiate(hitsplatTemplate);
        hitsplat.transform.position = monsterSprite.transform.position;
        hitsplat.GetComponent<Hitsplat>().Init(Mathf.RoundToInt(incomingDamage), Color.white);
        Debug.Log("Monster HP:" + monsterStats.HP);
        CheckCombatOver();
    }

    private void DealDamageToPlayer()
    {
        
        int incomingDamage = (int)monsterStats.attack;
        incomingDamage -= (int)(playerStats.defense+playerStats.armorBonusDefense);
        incomingDamage = Math.Max(incomingDamage, 0);
        playerStats.HP -= Mathf.RoundToInt(incomingDamage);
        GameObject hitsplat = GameObject.Instantiate(hitsplatTemplate);
        hitsplat.transform.position = monsterSprite.transform.position;
        hitsplat.GetComponent<Hitsplat>().Init(Mathf.RoundToInt(incomingDamage), Color.white);
        Debug.Log("Player HP:" + playerStats.HP);
        CheckCombatOver();
    }

    private void HandleEntranceRoutine()
    {
        Debug.Log("Time: " + enterTimer / ENTER_TIME);
        Vector3 startPos = monsterStats.startPositionOnScreen;
        Vector3 targetPos = monsterStats.homePositionOnScreen;
        Vector3 lerpedPos = Vector3.Lerp(startPos, targetPos, enterTimer / ENTER_TIME);
        monsterSprite.transform.localPosition = lerpedPos;

        startPos = playerStats.startPositionOnScreen;
        targetPos = playerStats.homePositionOnScreen;
        lerpedPos = Vector3.Lerp(startPos, targetPos, enterTimer / ENTER_TIME);
        playerSprite.transform.localPosition = lerpedPos;

        enterTimer += Time.deltaTime;
    }
}
