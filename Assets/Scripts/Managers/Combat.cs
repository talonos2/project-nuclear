﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    private static readonly float ENTER_TIME = .3f;
    private static readonly float EXIT_TIME = .3f;
    private bool oneLastEnterFrame = true;

    //Starts a fight, creating a new game object with an instance of this monobehavior.
    internal static void InitiateFight(GameObject Player, GameObject monster)
    {
        SoundManager.Instance.PlaySound("CombatIn", 1f);
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
    private GameObject monsterHPBarHolder;
    private Image monsterHPBar;

    GameObject hitsplatTemplate;

    GameObject[] eleVFXes = new GameObject[4];
    GameObject[] eleSwitchVFXes = new GameObject[4];

    private static readonly float finalMonsterHPBarPosition = 18.483f;
    private static readonly float startMonsterHPBarPosition = 24.5f;

    private void Init(Enemy monsterStats, CharacterStats playerStats, GameObject monsterToDelete)
    {
        MusicManager.instance.TurnOnCombatMusic();


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
        combatFolder = Camera.main.transform.Find("UI").Find("Combat").gameObject;
        combatDarkening = Camera.main.transform.Find("UI").Find("DarkeningPlane").gameObject.GetComponent<Renderer>();
        monsterHPBarHolder = Camera.main.transform.Find("UI").Find("Bars Canvas").Find("Enemy HP Bar Back").gameObject;
        monsterHPBarHolder.GetComponent<Image>().color = Color.white;
        monsterHPBarHolder.GetComponent<RectTransform>().localPosition = new Vector3(startMonsterHPBarPosition, monsterHPBarHolder.GetComponent<RectTransform>().localPosition.y, monsterHPBarHolder.GetComponent<RectTransform>().localPosition.z);
        monsterHPBar = monsterHPBarHolder.transform.Find("Enemy HP Bar").GetComponent<Image>();
        monsterHPBar.GetComponent<UIEHPScript>().BindToMonster(monsterStats);
        monsterHPBar.color = Color.white;

        blade = Camera.main.transform.Find("UI").Find("Switchblade").Find("blade").gameObject.GetComponent<UISwitchbladeScript>();
        blade.StartOpen();

        playerSprite.transform.SetParent(combatFolder.transform);
        playerSprite.transform.localPosition = playerStats.startPositionOnScreen;

        monsterSprite.transform.SetParent(combatFolder.transform);
        monsterSprite.transform.localPosition = monsterStats.startPositionOnScreen;

        SetMonsterAndPlayerScale();

        hitsplatTemplate = Resources.Load<GameObject>("Hitsplat");
        eleVFXes[0] = Resources.Load<GameObject>("IceSlashFX");
        eleVFXes[1] = Resources.Load<GameObject>("EarthSlashFX");
        eleVFXes[2] = Resources.Load<GameObject>("FireSlashFX");
        eleVFXes[3] = Resources.Load<GameObject>("AirSlashFX");

        eleSwitchVFXes[0] = Resources.Load<GameObject>("IceSwitchFX");
        eleSwitchVFXes[1] = Resources.Load<GameObject>("EarthSwitchFX");
        eleSwitchVFXes[2] = Resources.Load<GameObject>("FireSwitchFX");
        eleSwitchVFXes[3] = Resources.Load<GameObject>("AirSwitchFX");
    }

    private void SetMonsterAndPlayerScale()
    {
        Vector3 pScale = (Vector3)(playerStats.scale * monsterStats.forceOpponentAdditionalScale) + new Vector3(0, 0, 1);
        playerSprite.transform.localScale = pScale*1/6;

        Vector3 mScale = (Vector3)(monsterStats.scale * playerStats.forceOpponentAdditionalScale) + new Vector3(0, 0, 1);
        monsterSprite.transform.localScale = mScale * 1 / 6;
    }

    public void Update()
    {
        if (enterTimer < ENTER_TIME)
        {
            HandleEntranceRoutine();
            oneLastEnterFrame = true;
        }
        else if (oneLastEnterFrame)
        {
            HandleEntranceRoutine();
            oneLastEnterFrame = false;
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

        monsterHPBar.color = new Color(1, 1, 1, 1-amountThrough);
        monsterHPBarHolder.GetComponent<Image>().color = new Color(1, 1, 1, 1-amountThrough);

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
    bool playerDidSoundRecently;
    bool monsterDidSoundRecently;
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
        float playerAttackTime = AttackAnimation.PLAYER_HOP.GetAnimationLength();
        //float enemyAttackTime = playerStats.animation.GetAnimationLength();
        float enemyAttackTime = monsterStats.attackAnimation.GetAnimationLength();
        float totalTime = playerAttackTime + enemyAttackTime;

        float timeSinceLastPlayerAttack = combatTimer % totalTime;
        float timeSinceLastMonsterAttack = (combatTimer > enemyAttackTime ? (combatTimer - enemyAttackTime) % totalTime : 0);

        float playerDamagePoint = AttackAnimation.PLAYER_HOP.GetDamagePoint();
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
                SoundManager.Instance.PlaySound("Combat/badClick", 1f);
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
            playerDidSoundRecently = false;
        }
        if (timeSinceLastMonsterAttack < previousTimeSinceLastMonsterAttack)
        {
            monsterDidDamageRecently = false;
            monsterDidSoundRecently = false;
        }
        if (!playerDidDamageRecently && timeSinceLastPlayerAttack > AttackAnimation.PLAYER_HOP.GetDamagePoint() + (SWAY_TOLERANCE/2.0f))
        {
            this.DealDamageToEnemy();
            playerDidDamageRecently = true;
        }
        if (!monsterDidDamageRecently && timeSinceLastMonsterAttack > monsterStats.attackAnimation.GetDamagePoint() + (SWAY_TOLERANCE / 2.0f))
        {
            this.DealDamageToPlayer();
            monsterDidDamageRecently = true;
        }
        if (!playerDidSoundRecently && timeSinceLastPlayerAttack > AttackAnimation.PLAYER_HOP.GetAttackSoundPoint())
        {
            AttackAnimation.PLAYER_HOP.PlaySound();
            playerDidSoundRecently = true;
        }
        if (!monsterDidSoundRecently && timeSinceLastMonsterAttack > monsterStats.attackAnimation.GetAttackSoundPoint())
        {
            monsterStats.attackAnimation.PlaySound();
            monsterDidSoundRecently = true;
        }
        previousTimeSinceLastMonsterAttack = timeSinceLastMonsterAttack;
        previousTimeSinceLastPlayerAttack = timeSinceLastPlayerAttack;
    }

    private void CheckCombatOver()
    {
        //TODO: If the time is up, combat ends immediately.
        if (gameData.minutes==10)
        {
            MusicManager.instance.TurnOffCombatMusic();
            combatEnded = true;
            GameState.isInBattle = false;
            blade.StartClose();
            //GameState.endRunFlag = true;
            KillPlayerAndLoadNextScene(true);

        }

        if (playerStats.HP <= 0)
        {
            MusicManager.instance.TurnOffCombatMusic();
            combatEnded = true;
            GameState.isInBattle = false;
            blade.StartClose();
            //GameState.endRunFlag = true;
            KillPlayerAndLoadNextScene(false);

        }

        if (monsterStats.HP <= 0)
        {
            MusicManager.instance.TurnOffCombatMusic();
            blade.StartClose();
            combatEnded = true;
            
        }
    }

    private void KillPlayerAndLoadNextScene(bool timeOut) {
        if (!timeOut)
        {
            GameData.Instance.deathTime = GameData.Instance.timer;
        }
        else
        {
            GameData.Instance.killer = monsterStats.name;
        }
        GameState.isInBattle = false;
        Destroy(monsterSprite.gameObject);
        Destroy(playerSprite.gameObject);
        combatDarkening.material.SetFloat("_Alpha", 0);
        blade.swayBall.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
        Destroy(this);
        //At this point play 'death' animation on player, which should end in a death of the player. 
        //For now doing the scene transition to the death scene right away
        SoundManager.Instance.PlayPersistentSound("TakenByCurse");
        MusicManager.instance.FadeOutMusic(-2, 3);
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
        if (monsterStats.iceBoss) {
            GameData.Instance.iceBoss1 = true;
            playerStats.powersGained = Math.Max(1, playerStats.powersGained); }
        if (monsterStats.earthBoss) {
            GameData.Instance.earthBoss1 = true;
            playerStats.powersGained = Math.Max(2, playerStats.powersGained); }
        if (monsterStats.fireBoss) {
            GameData.Instance.fireBoss1 = true;
            playerStats.powersGained = Math.Max(3, playerStats.powersGained); }
        if (monsterStats.airBoss) {
            GameData.Instance.airBoss1 = true;
            playerStats.powersGained = Math.Max(4, playerStats.powersGained); }
        if (monsterStats.deathBoss) {
            GameData.Instance.deathBoss1 = true;
        }
        if (monsterStats.finalBoss) {
            GameData.Instance.victory = true;
        }

        playerStats.PushCharacterData();
        Destroy(monsterToDelete);
    }

    private void DealDamageToEnemy()
    {
        float incomingDamage = (int)playerStats.attack;

        if (goodHit) { SoundManager.Instance.PlaySound("Combat/GoodHit", .5f); }
        else { SoundManager.Instance.PlaySound("Combat/BadHit", .5f); }

        incomingDamage = incomingDamage * (1 + playerStats.accessoryAttackPercent/100);
        incomingDamage -= monsterStats.defense;
        incomingDamage = Math.Max(incomingDamage, 0);

        bool hitWeak = false;

        float elementalDamage = incomingDamage * playerStats.currentPower * .25f;
        if (playerStats.currentPower == (int)ElementalPower.ICE) { elementalDamage = incomingDamage * (.25f+playerStats.accessoryIceBonus/100);
            if (monsterStats.weakness == ElementalPower.ICE) { elementalDamage *= 2; hitWeak = true; }
            if (playerStats.mana >= 2) { playerStats.mana -= 2; }
                else { elementalDamage = 0; }
        }
        if (playerStats.currentPower == (int)ElementalPower.EARTH) { elementalDamage = incomingDamage *(.5f+ playerStats.accessoryEarthBonus/100);
            if (monsterStats.weakness == ElementalPower.EARTH) { elementalDamage *= 2; hitWeak = true; }
            if (playerStats.mana >= 4) { playerStats.mana -= 4; }
                else { elementalDamage = 0; }
        }
        if (playerStats.currentPower == (int)ElementalPower.FIRE) { elementalDamage = incomingDamage * (.75f+playerStats.accessoryFireBonus/100);
            if (monsterStats.weakness == ElementalPower.FIRE) { elementalDamage *= 2; hitWeak = true; }
            if (playerStats.mana >= 6) { playerStats.mana -= 6; }
                else { elementalDamage = 0; }
        }
        if (playerStats.currentPower == (int)ElementalPower.AIR) { elementalDamage = incomingDamage * (1.0f+playerStats.accessoryAirBonus/100);
            if (monsterStats.weakness == ElementalPower.AIR) { elementalDamage *= 2; hitWeak = true; }
            if (playerStats.mana >= 8) { playerStats.mana -= 8; }
                else { elementalDamage = 0; }
        }

        if (goodHit) { incomingDamage *= 1.25f; }
        goodHit = false;

        incomingDamage *= 1 + RollCrit(); //should probably have an animation too

        monsterStats.HP -=  Mathf.RoundToInt(incomingDamage)+ Mathf.RoundToInt(elementalDamage);

        if (playerStats.accessoryHPVamp > 0) {//same. should have an animation...
            playerStats.HP = (int)((float) playerStats.MaxHP * (1 + playerStats.accessoryHPVamp / 100));
            //Note: The vamp items have a multiplier higher then what is shown This is to gausian give them the
            //the correct amount of life steal when dealing with remainders. The change is stat +.2
            if (playerStats.HP > playerStats.MaxHP)
                { playerStats.HP = playerStats.MaxHP; }
            }
        if (playerStats.accessoryMPVamp > 0) {
            playerStats.mana = (int)((float)playerStats.MaxMana * (1 + playerStats.accessoryMPVamp / 100));
            if (playerStats.mana > playerStats.MaxMana)
            { playerStats.mana = playerStats.MaxMana; }
        }

        //need elemental hit-splat if elemental damage is > 0
        GameObject hitsplat = GameObject.Instantiate(hitsplatTemplate);
        hitsplat.transform.position = monsterSprite.transform.position+(Vector3)monsterStats.gettingStruckPointOffset+AttackAnimationManager.Instance.monsterHitsplatOffset;
        hitsplat.GetComponent<Hitsplat>().Init(Mathf.RoundToInt(incomingDamage), Color.white);

        if (elementalDamage > 0)
        {
            GameObject eleHitsplat = GameObject.Instantiate(hitsplatTemplate);
            eleHitsplat.transform.position = monsterSprite.transform.position + (Vector3)monsterStats.gettingStruckPointOffset + AttackAnimationManager.Instance.monsterHitsplatOffset - new Vector3(0, .8f, 0);
            eleHitsplat.GetComponent<Hitsplat>().Init(Mathf.RoundToInt(elementalDamage), ((ElementalPower)playerStats.currentPower).EleColor());
            eleHitsplat.GetComponent<Hitsplat>().SetEleEffective(hitWeak);
            GameObject eleVFX = GameObject.Instantiate(eleVFXes[playerStats.currentPower-1]);
            SoundManager.Instance.PlaySound("Combat/EleVFXSound" + playerStats.currentPower, 1);
            /*switch ((ElementalPower)playerStats.currentPower)
            {
                case ElementalPower.ICE:
                    eleVFX = GameObject.Instantiate(eleVFXes[0]);
                    break;
                case ElementalPower.EARTH:
                    eleVFX = GameObject.Instantiate(eleVFXes[1]);
                    break;
                case ElementalPower.FIRE:
                    eleVFX = GameObject.Instantiate(eleVFXes[2]);
                    break;
                case ElementalPower.AIR:
                    eleVFX = GameObject.Instantiate(eleVFXes[3]);
                    break;
            }*/
            eleVFX.transform.position = playerSprite.transform.position - (Vector3)playerStats.strikingPointOffset - new Vector3(-2 * monsterStats.forceOpponentAdditionalScale.x, -5.5f * monsterStats.forceOpponentAdditionalScale.y, -.2f);
            eleVFX.transform.localScale *= monsterStats.forceOpponentAdditionalScale;
        }
        //Debug.Log("Monster HP:" + monsterStats.HP);
        CheckCombatOver();
    }

    public void DisplayElementSwitchVFX(int currentPower)
    {
        /*GameObject eleVFX = null;
        switch ((ElementalPower)currentPower)
        {
            case ElementalPower.ICE:
                eleVFX = GameObject.Instantiate(eleSwitchVFXes[0]);
                eleVFX.transform.position = playerSprite.transform.position - new Vector3(1 * monsterStats.forceOpponentAdditionalScale.x, -4.5f * monsterStats.forceOpponentAdditionalScale.y, .2f);
                eleVFX.transform.localScale *= monsterStats.forceOpponentAdditionalScale;
                return;
            case ElementalPower.EARTH:
                eleVFX = GameObject.Instantiate(eleSwitchVFXes[1]);
                eleVFX.transform.position = playerSprite.transform.position - new Vector3(1 * monsterStats.forceOpponentAdditionalScale.x, -4.5f * monsterStats.forceOpponentAdditionalScale.y, .2f); ;
                eleVFX.transform.localScale *= monsterStats.forceOpponentAdditionalScale;
                return;
            case ElementalPower.FIRE:
                eleVFX = GameObject.Instantiate(eleSwitchVFXes[2]);
                eleVFX.transform.position = playerSprite.transform.position - new Vector3(1 * monsterStats.forceOpponentAdditionalScale.x, -4.5f * monsterStats.forceOpponentAdditionalScale.y, .2f); ;
                eleVFX.transform.localScale *= monsterStats.forceOpponentAdditionalScale;
                return;
            case ElementalPower.AIR:
                eleVFX = GameObject.Instantiate(eleSwitchVFXes[3]);
                eleVFX.transform.position = playerSprite.transform.position - new Vector3(1 * monsterStats.forceOpponentAdditionalScale.x, -4.5f * monsterStats.forceOpponentAdditionalScale.y, .2f); ;
                eleVFX.transform.localScale *= monsterStats.forceOpponentAdditionalScale;
                return;
            default:
                return;
        }*/ //Return to fix this later.
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

        if (goodBlock) { SoundManager.Instance.PlaySound("Combat/GoodBlock", 1f); }
        else { SoundManager.Instance.PlaySound("Combat/BadBlock", .5f); }

        float incomingDamage = monsterStats.attack;
        incomingDamage -= playerStats.defense;
        if (goodBlock) { incomingDamage *= .8f; }
        goodBlock = false;
        incomingDamage = Math.Max(incomingDamage, 0);
        if (RollDodge()) {
            incomingDamage = 0;
            //ShouldHaveDodgeEffect
        }
        playerStats.HP -= Mathf.RoundToInt(incomingDamage);
        GameObject hitsplat = GameObject.Instantiate(hitsplatTemplate);
        hitsplat.transform.position = playerSprite.transform.position+(Vector3)playerStats.gettingStruckPointOffset+AttackAnimationManager.Instance.playerHitsplatOffset;
        hitsplat.GetComponent<Hitsplat>().Init(Mathf.RoundToInt(incomingDamage), Color.white);
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

        monsterHPBarHolder.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(startMonsterHPBarPosition, finalMonsterHPBarPosition, amountThrough), monsterHPBarHolder.GetComponent<RectTransform>().localPosition.y, monsterHPBarHolder.GetComponent<RectTransform>().localPosition.z);

        startPos = playerStats.startPositionOnScreen;
        targetPos = playerStats.homePositionOnScreen;
        lerpedPos = Vector3.Lerp(startPos, targetPos, amountThrough);
        playerSprite.transform.localPosition = lerpedPos;

        enterTimer += Time.deltaTime;
    }
}
