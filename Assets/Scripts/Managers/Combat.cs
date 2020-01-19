using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    private static readonly float ENTER_TIME = .3f;
    private static readonly float EXIT_TIME = .3f;

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
    private float exitTimer;
    private float combatTimer;
    private bool combatEnded;

    private GameObject monsterSprite;
    private GameObject playerSprite;
    private SpriteRenderer pRenderer;
    private SpriteRenderer eRenderer;

    private Renderer combatDarkening;

    private GameObject combatFolder;
    private int phaseLastFrame;
    private GameData gameData;

    GameObject hitsplatTemplate;

    private void Init(Enemy monsterStats, CharacterStats playerStats, GameObject monsterToDelete)
    {
        AttackAnimationManager aam = AttackAnimationManager.Instance;
        aam.LoadCombatPawns(monsterStats, playerStats);
        this.monsterStats = monsterStats;
        this.playerStats = playerStats;
        this.monsterToDelete = monsterToDelete;
        gameData= GameData.Instance;
        //Create monster sprites:

        monsterSprite = new GameObject("Monster Sprite");
        playerSprite = new GameObject("Player Sprite");

        pRenderer = playerSprite.AddComponent<SpriteRenderer>();
        pRenderer.flipX = true;
        pRenderer.sprite = playerStats.combatSprites[0];

        eRenderer = monsterSprite.AddComponent<SpriteRenderer>();
        eRenderer.flipX = true;
        eRenderer.sprite = monsterStats.combatSprites[0];

        combatFolder = Camera.main.transform.Find("UI").Find("Combat").gameObject;
        combatDarkening = Camera.main.transform.Find("UI").Find("DarkeningPlane").gameObject.GetComponent<Renderer>();

        blade = Camera.main.transform.Find("UI").Find("Switchblade").Find("blade").gameObject.GetComponent<UISwitchbladeScript>();
        blade.StartOpen();

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

        Vector3 mScale = (Vector3)(monsterStats.scale * playerStats.forceOpponentAdditionalScale) + new Vector3(0, 0, 1);
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
        else if (exitTimer < EXIT_TIME)
        {
            HandleExitRoutine();
        }
        else
        {
            KillMonsterAndGetRewards();
            GameState.isInBattle = false;
            Destroy(monsterSprite.gameObject);
            Destroy(playerSprite.gameObject);
            combatDarkening.material.SetFloat("_Alpha", 0);
            blade.swayBall.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
            Destroy(this);
        }

        return;
    }

    Vector3 exitStartPos = new Vector3(-1000, -1000, -1000);

    private void HandleExitRoutine()
    {
        float amountThrough = exitTimer / EXIT_TIME;

        if (exitStartPos == new Vector3(-1000, -1000, -1000))
        {
            exitStartPos = playerSprite.transform.localPosition;
        }

        combatDarkening.material.SetFloat("_Alpha", (1-amountThrough) / 2.0f);

        blade.swayBall.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1-amountThrough);

        monsterSprite.GetComponent<SpriteRenderer>().color = new Color((1 - amountThrough), (1 - amountThrough), (1 - amountThrough), (1 - amountThrough));

        Vector3 targetPos = monsterStats.startPositionOnScreen;
        Vector3 lerpedPos = Vector3.Lerp(exitStartPos, targetPos, amountThrough);

        playerSprite.transform.localPosition = lerpedPos;

        exitTimer += Time.deltaTime;
    }

    bool playerDidDamageRecently;
    bool monsterDidDamageRecently;
    float previousTimeSinceLastMonsterAttack = -1000;
    float previousTimeSinceLastPlayerAttack = -1000;
    private UISwitchbladeScript blade;
    float buttonPressedTime = 1000;
    bool goodBlock = false;
    bool goodHit = false;
    float SWAY_TOLERANCE = .1f;

    private void HandleCombatLoop()
    {
        if (Application.isEditor)
        {
            SetMonsterAndPlayerScale();
        }

        //float playerAttackTime = playerStats.animation.GetAnimationLength();
        float playerAttackTime = AttackAnimation.HOP.GetAnimationLength();
        //float enemyAttackTime = playerStats.animation.GetAnimationLength();
        float enemyAttackTime = monsterStats.attackAnimation.GetAnimationLength();
        float totalTime = playerAttackTime + enemyAttackTime;

        float timeSinceLastPlayerAttack = combatTimer % totalTime;
        float timeSinceLastMonsterAttack = (combatTimer > enemyAttackTime ? (combatTimer - enemyAttackTime) % totalTime : 0);

        float playerDamagePoint = AttackAnimation.HOP.GetDamagePoint();
        float enemyDamagePoint = playerAttackTime + monsterStats.attackAnimation.GetDamagePoint();
        float rightSideTime = enemyDamagePoint - playerDamagePoint;
        float leftSideTime = totalTime - rightSideTime;

        float amountThrough = combatTimer % totalTime;
        bool swayIsRightSide = (amountThrough > playerDamagePoint && amountThrough < enemyDamagePoint);

        if (swayIsRightSide)
        {
            blade.HandleRightSideSway((amountThrough - playerDamagePoint) / rightSideTime);
        }
        else
        {
            blade.HandleLeftSideSway(((totalTime+  amountThrough - enemyDamagePoint)%totalTime) / leftSideTime);
        }

        //Debug.Log("Player:" + playerStats.homePositionOnScreen);

        int playerFrame = AttackAnimation.HOP.HandleAnimation(timeSinceLastPlayerAttack, playerSprite, monsterSprite, monsterStats, playerStats);
        int enemyFrame = monsterStats.attackAnimation.HandleAnimation(timeSinceLastMonsterAttack, monsterSprite, playerSprite, playerStats, monsterStats);
        playerSprite.GetComponent<SpriteRenderer>().sprite = playerStats.combatSprites[playerFrame];
        playerSprite.transform.localPosition = new Vector3(playerSprite.transform.localPosition.x, playerSprite.transform.localPosition.y, -.05f);
        monsterSprite.GetComponent<SpriteRenderer>().sprite = monsterStats.combatSprites[enemyFrame];

        if (Input.GetButtonDown("Attack/Defend") && buttonPressedTime > .5f)
        {
            buttonPressedTime = 0;
            if (playerDamagePoint - amountThrough < SWAY_TOLERANCE / 2.0f && playerDamagePoint - amountThrough > SWAY_TOLERANCE / -2.0f)
            {
                goodHit = true;
                blade.SpawnGoodParticles();
            }
            else if (enemyDamagePoint - amountThrough < SWAY_TOLERANCE/2.0f && enemyDamagePoint - amountThrough > SWAY_TOLERANCE / -2.0f)
            {
                goodBlock = true;
                blade.SpawnGoodParticles();
            }
            else
            {
                blade.SpawnErrorParticles();
            }
        }
        else
        {
            buttonPressedTime += Time.deltaTime;
        }
        combatTimer += Time.deltaTime;

        //Handle damage:
        if (timeSinceLastPlayerAttack < previousTimeSinceLastPlayerAttack)
        {
            playerDidDamageRecently = false;
        }
        if (timeSinceLastMonsterAttack < previousTimeSinceLastMonsterAttack)
        {
            monsterDidDamageRecently = false;
        }
        if (!playerDidDamageRecently && timeSinceLastPlayerAttack > AttackAnimation.HOP.GetDamagePoint() + (SWAY_TOLERANCE/2.0f))
        {
            this.DealDamageToEnemy();
            playerDidDamageRecently = true;
        }
        if (!monsterDidDamageRecently && timeSinceLastMonsterAttack > monsterStats.attackAnimation.GetDamagePoint() + (SWAY_TOLERANCE / 2.0f))
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
        if (gameData.minutes==10)
        {
            combatEnded = true;
            blade.StartClose();
            //GameState.endRunFlag = true;
            KillPlayerAndLoadNextScene();
            GameState.isInBattle = false;
        }
        if (playerStats.HP <= 0)
        {
            combatEnded = true;
            blade.StartClose();
            //GameState.endRunFlag = true;
            KillPlayerAndLoadNextScene();
            GameState.isInBattle = false;
        }
        if (monsterStats.HP <= 0)
        {
            blade.StartClose();
            combatEnded = true;
            playerStats.powersGained = Math.Max(monsterStats.powerGiven, playerStats.powersGained);
        }
    }

    private void KillPlayerAndLoadNextScene() {
        //
        GameState.isInBattle = false;
        Destroy(monsterSprite.gameObject);
        Destroy(playerSprite.gameObject);
        combatDarkening.material.SetFloat("_Alpha", 0);
        blade.swayBall.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
        Destroy(this);
        //At this point play 'death' animation on player, which should end in a death of the player. 
        //For now doing the scene transition to the death scene right away
        SceneManager.LoadScene("DeathScene");


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
        float incomingDamage = (int)playerStats.attack;
        if (goodHit) {incomingDamage *= 1.25f; }
        goodHit = false;
        incomingDamage = incomingDamage * (1 + playerStats.accessoryAttackPercent);
        incomingDamage -= monsterStats.defense;
        incomingDamage = Math.Max(incomingDamage, 0);
        float elementalDamage = incomingDamage * playerStats.currentPower * .25f;
        if (playerStats.currentPower == (int)ElementalPower.ICE) { elementalDamage *= 1 + playerStats.accessoryIceBonus;
            if (monsterStats.weakness == ElementalPower.ICE) { elementalDamage *= 2; }
            if (playerStats.mana >= 2) { playerStats.mana -= 2; }
                else { elementalDamage = 0; }
        }
        if (playerStats.currentPower == (int)ElementalPower.EARTH) { elementalDamage *= 1 + playerStats.accessoryEarthBonus;
            if (monsterStats.weakness == ElementalPower.EARTH) { elementalDamage *= 2; }
            if (playerStats.mana >= 4) { playerStats.mana -= 4; }
                else { elementalDamage = 0; }
        }
        if (playerStats.currentPower == (int)ElementalPower.FIRE) { elementalDamage *= 1 + playerStats.accessoryFireBonus;
            if (monsterStats.weakness == ElementalPower.FIRE) { elementalDamage *= 2; }
            if (playerStats.mana >= 6) { playerStats.mana -= 6; }
                else { elementalDamage = 0; }
        }
        if (playerStats.currentPower == (int)ElementalPower.AIR) { elementalDamage *= 1 + playerStats.accessoryAirBonus;
            if (monsterStats.weakness == ElementalPower.AIR) { elementalDamage *= 2; }
            if (playerStats.mana >= 8) { playerStats.mana -= 8; }
                else { elementalDamage = 0; }
        }
        incomingDamage *= 1 + RollCrit(); //should probably have an animation too

        monsterStats.HP -=  Mathf.RoundToInt(incomingDamage)+ Mathf.RoundToInt(elementalDamage);

        if (playerStats.accessoryHPVamp > 0) {//same. should have an animation...
            playerStats.HP = (int)((float) playerStats.HP * (1 + playerStats.accessoryHPVamp / 100));
            if (playerStats.HP > playerStats.MaxHP)
                { playerStats.HP = playerStats.MaxHP; }
            }
        if (playerStats.accessoryMPVamp > 0) {
            playerStats.mana = (int)((float)playerStats.mana * (1 + playerStats.accessoryMPVamp / 100));
            if (playerStats.mana > playerStats.MaxMana)
            { playerStats.mana = playerStats.MaxMana; }
        }

        //need elemental hit-splat if elemental damage is > 0
        GameObject hitsplat = GameObject.Instantiate(hitsplatTemplate);
        hitsplat.transform.position = monsterSprite.transform.position;
        hitsplat.GetComponent<Hitsplat>().Init(Mathf.RoundToInt(incomingDamage), Color.white);
        //Debug.Log("Monster HP:" + monsterStats.HP);
        CheckCombatOver();
    }

    private float RollCrit()
    {
        float critValue = 0;
        System.Random rand = new System.Random(100);
        int roll = rand.Next() + 1;
        if (roll <= playerStats.accessoryCritChance) { critValue = 1.5f; }

        return critValue;
    }

    private bool RollDodge()
    {
        bool dodgeValue = false;
        System.Random rand = new System.Random(100);
        int roll = rand.Next() + 1;
        if (roll <= playerStats.accessoryDodgeBonus) { dodgeValue = true ; }

        return dodgeValue;
    }

    private void DealDamageToPlayer()
    {
        
        float incomingDamage = monsterStats.attack;
        if (goodBlock) { incomingDamage *= .8f; }
        goodBlock = false;
        incomingDamage -= playerStats.defense;
        incomingDamage = Math.Max(incomingDamage, 0);
        if (RollDodge()) {
            incomingDamage = 0;
            //ShouldHaveDodgeEffect
        }
        playerStats.HP -= Mathf.RoundToInt(incomingDamage);
        GameObject hitsplat = GameObject.Instantiate(hitsplatTemplate);
        hitsplat.transform.position = monsterSprite.transform.position;
        hitsplat.GetComponent<Hitsplat>().Init(Mathf.RoundToInt(incomingDamage), Color.white);
        //Debug.Log("Player HP:" + playerStats.HP);
        CheckCombatOver();
    }

    private void HandleEntranceRoutine()
    {
        float amountThrough = enterTimer / ENTER_TIME;

        blade.HandleLeftSideSway(.5f);

        combatDarkening.material.SetFloat("_Alpha", amountThrough / 2.0f);
        blade.swayBall.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, amountThrough);

        Vector3 startPos = monsterStats.startPositionOnScreen;
        Vector3 targetPos = monsterStats.homePositionOnScreen;
        Vector3 lerpedPos = Vector3.Lerp(startPos, targetPos, amountThrough);
        monsterSprite.transform.localPosition = lerpedPos;

        startPos = playerStats.startPositionOnScreen;
        targetPos = playerStats.homePositionOnScreen;
        lerpedPos = Vector3.Lerp(startPos, targetPos, amountThrough);
        playerSprite.transform.localPosition = lerpedPos;

        enterTimer += Time.deltaTime;
    }
}
