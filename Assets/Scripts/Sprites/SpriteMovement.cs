using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpriteMovement : EntityData
{
    //Constants:
    protected const float JUMP_HEIGHT = 1f;
    protected float FLOATING_POINT_FIX = .00001f;

    //Sprite Animation:
    private float timeSinceLastAnimation = 0;
    private int animationStep = 0;
    private DirectionMoved lastAnimatedFacing = DirectionMoved.DOWN;

    public float MoveSpeed = 6;
    public float framesPerSecond = 30;

    protected GameObject MapGrid;
    //protected Vector2 mapZeroLocation;
    //protected EntityGrid mapEntityGrid;
    protected GroundMaterialGrid groundMaterialGrid;
    protected EnvironmentalSoundMagnitudeGrid environmentalSoundMagnitudeGrid;
    protected bool currentlyMoving = false;
    protected GameObject ThePlayer;

    private string spriteNames;

    protected float movedSoFar = 0;
    protected float movedSoFarX = 0;
    protected float movedSoFarY = 0;
    protected float jumpSpeed = 1.25f;
    protected bool jumpQueued = false;
    protected bool dashQued = false;
    protected float queDistance = .75f;
    protected float tempFramesPerSecond = 0;
    protected float tempMovementSpeed;
    public static bool jumping;
    protected GameData gameData;

    protected Vector2Int characterLocation;
    protected Vector2Int characterNextLocation;
    [HideInInspector]
    public DirectionMoved facedDirection = DirectionMoved.LEFT;
    protected Vector2Int homeLocation;
    protected Vector2Int exitLocation = new Vector2Int(0, 0);
    protected float waitTimer = 0;

    //Stuff to do with forced jumping:
    protected Vector2Int jumpTarget;
    protected Vector3 jumpStartPos;
    protected float timeLeftInForcedJump = 0;
    protected float totalTimeInForcedJump = 0;
    protected float forcedJumpHeight = 0;


    public enum DirectionMoved
    { NONE, UP, RIGHT, DOWN, LEFT }

    public void Start()
    {
        InitializeNewMap();
        InitializeSpriteLocation();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
        gameData = GameData.Instance;
        tempFramesPerSecond = framesPerSecond;
        tempMovementSpeed = MoveSpeed;
    }

    public void SetRenderer()
    {
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
    }

    private void InitializeSpriteLocation()
    {
        characterLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        characterLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
        MapGrid.GetComponent<EntityGrid>().grid[characterLocation.x, characterLocation.y] = this.gameObject;
        homeLocation.x = characterLocation.x;
        homeLocation.y = characterLocation.y;
    }

    protected void SetCurrentLocation()
    {
        characterLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        characterLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
    }

    protected float MoveToNextSquare()
    {
        return MoveDirection(tempMovementSpeed, facedDirection);
    }

    public void SetNewMovespeed(float newSpeed)
    {
        tempMovementSpeed = newSpeed;
    }

    public void SetOldMovespeed()
    {
        tempMovementSpeed = MoveSpeed;
    }

    protected void SetNextLocationActual(int characterLocX, int characterLocY)
    {
        characterNextLocation.x = characterLocX;
        characterNextLocation.y = characterLocY;
        UpdateNewEntityGridLocation();
        RemoveOldEntityGridLocation();
        characterLocation = characterNextLocation;
        if (this is CharacterMovement)
        {
            UpdateEnvironmentSound();
        }
    }

    protected void SetNextLocation(DirectionMoved nextStep)
    {
        if (nextStep == DirectionMoved.UP)
        {
            characterNextLocation.x = characterLocation.x;
            characterNextLocation.y = characterLocation.y + 1;
        }
        if (nextStep == DirectionMoved.DOWN)
        {
            characterNextLocation.x = characterLocation.x;
            characterNextLocation.y = characterLocation.y - 1;

        }
        if (nextStep == DirectionMoved.LEFT)
        {
            characterNextLocation.x = characterLocation.x - 1;
            characterNextLocation.y = characterLocation.y;
        }
        if (nextStep == DirectionMoved.RIGHT)
        {
            characterNextLocation.x = characterLocation.x + 1;
            characterNextLocation.y = characterLocation.y;

        }
        if (this is CharacterMovement)
        {
            UpdateEnvironmentSound();
        }
    }

    internal bool IsJumping()
    {
        return jumping;
    }

    internal bool IsInAForcedJump()
    {
        return timeLeftInForcedJump > 0;
    }

    protected bool IsMoveLocationPassable(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || IsPlatformUp(LocX, LocY))
        {
            if (IsLocationDoodadMonsterPassible(LocX, LocY) && IsLocationEntityPassible(LocX, LocY))
                MoveableLocation = true;
        }

        return MoveableLocation;
    }

    protected bool IsPlayerMoveLocationPassable(int LocX, int LocY)
    {

        bool MoveableLocation = false;
        if (LocX < 0 || LocY < 0)
        {
            return MoveableLocation;
        }

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL
            || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || IsPlatformUp(LocX, LocY))
        {
            if (IsLocationDoodadPlayerPassible(LocX, LocY) && IsLocationPlayerEntityPassable(LocX, LocY))
                MoveableLocation = true;
        }
        return MoveableLocation;
    }

    protected bool IsMoveLocationMonsterChaseable(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        if (!MapGrid.GetComponent<PassabilityGrid>().InRange(LocX, LocY)) { return false; }

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL
            || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || IsPlatformUp(LocX, LocY))
        {
            if (IsLocationDoodadMonsterPassible(LocX, LocY) && IsLocationEntityPassible(LocX, LocY))
                MoveableLocation = true;
        }
        return MoveableLocation;
    }

    protected bool IsMoveLocationFlyingMonsterChaseable(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        if (!MapGrid.GetComponent<PassabilityGrid>().InRange(LocX, LocY)) { return false; }

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL
            || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.AIR || IsPlatformUp(LocX, LocY))
        {
            if (IsLocationDoodadMonsterPassible(LocX, LocY) && IsLocationEntityPassible(LocX, LocY))
                MoveableLocation = true;
        }
        return MoveableLocation;
    }

    private bool IsPlatformUp(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        GameObject doddadObject = MapGrid.GetComponent<DoodadGrid>().grid[LocX, LocY];

        if (doddadObject != null)
        {
            if (doddadObject.GetComponent<DoodadData>().isPlatformTerrain)
                MoveableLocation = true;
        }
        return MoveableLocation;
    }

    protected bool IsPlayerInMonsterTerritory(int LocX, int LocY)
    {
        bool attackable = false;
        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER)
            attackable = true;
        return attackable;

    }
    protected bool IsRandomMoveLocationPassable(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER)
        {
            if (IsLocationDoodadMonsterPassible(LocX, LocY) && IsLocationEntityPassible(LocX, LocY))
                MoveableLocation = true;
        }

        return MoveableLocation;
    }

    protected bool IsAntiRandomMoveLocationPassable(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL)
        {
            if (IsLocationDoodadMonsterPassible(LocX, LocY) && IsLocationEntityPassible(LocX, LocY))
                MoveableLocation = true;
        }

        return MoveableLocation;
    }

    protected bool IsLocationDoodadPlayerPassible(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        GameObject DoddadObject = MapGrid.GetComponent<DoodadGrid>().grid[LocX, LocY];
        if (DoddadObject == null)
        {
            MoveableLocation = true;
        }
        else
        {
            DoodadData doodadObject = DoddadObject.GetComponent<DoodadData>();
            if (doodadObject.isPassable)
                MoveableLocation = true;
            if (doodadObject.isSpike && gameData.hasted)
                MoveableLocation = true;
            if (doodadObject.isExit || doodadObject.isWindShifter || doodadObject.isPlatformTerrain)
                MoveableLocation = true;

        }
        return MoveableLocation;
    }
    protected bool IsLocationDoodadMonsterPassible(int LocX, int LocY)
    {
        bool MoveableLocation = false;

        GameObject DoddadObject = MapGrid.GetComponent<DoodadGrid>().grid[LocX, LocY];
        if (DoddadObject == null)
        {
            MoveableLocation = true;
        }
        else
        {
            if (DoddadObject.GetComponent<DoodadData>().isPassable || DoddadObject.GetComponent<DoodadData>().isPlatformTerrain)
                MoveableLocation = true;
        }
        return MoveableLocation;

    }

    protected bool IsLocationEntityPassible(int LocX, int LocY)
    {
        bool MoveableLocation = false;
        GameObject entityInLocation = MapGrid.GetComponent<EntityGrid>().grid[LocX, LocY];
        if (entityInLocation == null)
            MoveableLocation = true;
        return MoveableLocation;
    }

    protected bool IsLocationPlayerEntityPassable(int LocX, int LocY)
    {
        bool MoveableLocation = false;
        GameObject entityInLocation = MapGrid.GetComponent<EntityGrid>().grid[LocX, LocY];
        if (entityInLocation == null)
            MoveableLocation = true;
        return MoveableLocation;
    }

    protected GameObject IsThereAMonster()
    {
        GameObject EnemyPresent = null;
        GameObject EntityToFight = MapGrid.GetComponent<EntityGrid>().grid[characterNextLocation.x, characterNextLocation.y];

        if (EntityToFight != null)
        {
            if (EntityToFight.GetComponent<EntityData>().isAMonster)
            {
                EnemyPresent = EntityToFight;
            }
        }
        return EnemyPresent;
    }

    protected GameObject IsThereAPlayer(int CharLocX, int CharLocY)
    {
        GameObject EnemyPresent = null;
        GameObject EntityToFight = MapGrid.GetComponent<EntityGrid>().GetEntity(CharLocX, CharLocY);
        //Make internal function to pull entity from entity grid and make boundery checks. 

        if (EntityToFight != null)
        {
            if (EntityToFight.GetComponent<EntityData>().isMainCharacter)
            {
                EnemyPresent = EntityToFight;
            }
        }
        return EnemyPresent;
    }

    protected void RemoveOldEntityGridLocation()
    {
        MapGrid.GetComponent<EntityGrid>().grid[characterLocation.x, characterLocation.y] = null;
    }

    protected void UpdateNewEntityGridLocation()
    {

        MapGrid.GetComponent<EntityGrid>().grid[characterNextLocation.x, characterNextLocation.y] = this.gameObject;
    }

    private void UpdateEnvironmentSound()
    {
        if (environmentalSoundMagnitudeGrid)
        {
            SoundManager.Instance.ChangeEnvironmentVolume(environmentalSoundMagnitudeGrid.grid[characterLocation.x, characterLocation.y]);
        }
        else
        {
            SoundManager.Instance.ChangeEnvironmentVolume(0);
        }
    }

    public void InitializeNewMap()
    {
        MapGrid = GetMapGrid();
        mapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        mapEntityGrid = MapGrid.GetComponent<EntityGrid>();
        groundMaterialGrid = MapGrid.GetComponent<GroundMaterialGrid>();
        environmentalSoundMagnitudeGrid = MapGrid.GetComponent<EnvironmentalSoundMagnitudeGrid>();
        if (this is CharacterMovement)
        {
            if (environmentalSoundMagnitudeGrid)
            {
                if (SoundManager.Instance.currentlyPlayingEnvTrack != environmentalSoundMagnitudeGrid.envSound)
                {
                    SoundManager.Instance.ChangeEnvironmentTrack(environmentalSoundMagnitudeGrid.envSound);
                }
                UpdateEnvironmentSound();
            }
            else
            {
                SoundManager.Instance.ChangeEnvironmentTrack();
            }
        }
    }

    public GameObject GetMapGrid()
    {
        GameObject MapGridRetrieved = GameObject.Find("Grid");
        if (isOnCutsceneMap) MapGridRetrieved = GameObject.Find("Grid2");
        return MapGridRetrieved;
    }

    protected bool ValidMoveLocation(int inputDirection)
    {
        if (inputDirection == (int)DirectionMoved.NONE)
            return false;

        if (inputDirection == (int)DirectionMoved.RIGHT
            && this.MapGrid.GetComponent<PassabilityGrid>().grid[characterLocation.x + 1, characterLocation.y] == PassabilityType.NORMAL)
            return true;

        if (inputDirection == (int)DirectionMoved.LEFT
            && this.MapGrid.GetComponent<PassabilityGrid>().grid[characterLocation.x - 1, characterLocation.y] == PassabilityType.NORMAL)
            return true;

        if (inputDirection == (int)DirectionMoved.UP
            && this.MapGrid.GetComponent<PassabilityGrid>().grid[characterLocation.x, characterLocation.y + 1] == PassabilityType.NORMAL)
            return true;

        if (inputDirection == (int)DirectionMoved.DOWN
            && this.MapGrid.GetComponent<PassabilityGrid>().grid[characterLocation.x, characterLocation.y - 1] == PassabilityType.NORMAL)
            return true;

        return false;
    }

    protected void AnimateMove(DirectionMoved dir)
    {
        bool changed = false;
        timeSinceLastAnimation += Time.deltaTime;
        if (timeSinceLastAnimation >= 1 / tempFramesPerSecond)
        {
            timeSinceLastAnimation = 0;
            animationStep = (animationStep + 1) % GetFramesInFilmstrip();
            changed = true;
        }
        if (facedDirection != lastAnimatedFacing)
        {
            lastAnimatedFacing = facedDirection;
            changed = true;
        }
        if (changed)
        {
            sRender.material.SetFloat("_Frame", animationStep + FLOATING_POINT_FIX + (dir.GetHeroSpriteOffeset() * (GetFramesInFilmstrip() + GetNumberOfIdleFrames()) + GetNumberOfIdleFrames()));
        }

    }

    protected virtual int GetFramesInFilmstrip()
    {
        return 6;
    }

    protected virtual int GetNumberOfIdleFrames()
    {
        return 1;
    }

    public float MoveDirection(float tempMovementSpeed, DirectionMoved dir)
    {
        float DistanceToMove = Time.deltaTime * tempMovementSpeed;
        movedSoFar += DistanceToMove;

        AnimateMove(dir);

        if (movedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (movedSoFar - 1);
            movedSoFar = 0;
        }

        Vector3 getMotion = DistanceToMove * dir.GetDirectionVector();
        transform.position = transform.position + getMotion;
        return movedSoFar;
    }

    public bool JumpToTarget()
    {
        timeLeftInForcedJump = Mathf.Max(timeLeftInForcedJump - Time.deltaTime, 0);
        float t = (totalTimeInForcedJump-timeLeftInForcedJump)/ totalTimeInForcedJump;

        Debug.Log(t + ", " + timeLeftInForcedJump + ", " + totalTimeInForcedJump);

        Vector3 newPosition = Vector3.Lerp(jumpStartPos, jumpStartPos + new Vector3(jumpTarget.x, jumpTarget.y,0), t);
        float h = -((2 * t - 1) * (2 * t - 1))+1;
        h *= forcedJumpHeight;

        Debug.Log(t + ", " + jumpStartPos + ", " + jumpTarget);

        transform.position = new Vector3(newPosition.x, newPosition.y + h, transform.position.z);

        //AnimateSpinning();

        return false;
    }

    public void SetLookDirection()
    {
        sRender.material.SetFloat("_Frame", (7*facedDirection.GetHeroSpriteOffeset()) + FLOATING_POINT_FIX);
    }

    protected void TiePositionToGrid()
    {
        transform.position = new Vector3(characterLocation.x + (int)mapZeroLocation.x, characterLocation.y + (int)mapZeroLocation.y, transform.position.z);
    }

    protected DirectionMoved GetChaseStep()
    {
        if (gameData.stealthed)
        {
            return DirectionMoved.NONE;
        }
        int TempX = ThePlayer.GetComponent<CharacterMovement>().characterLocation.x - characterLocation.x;
        int TempY = ThePlayer.GetComponent<CharacterMovement>().characterLocation.y - characterLocation.y;

        if (TempX == 0 && TempY == 0)
        {
            return DirectionMoved.NONE;
        }
        //Pick an axis based on a random number with the abs value distances as it's input
        int AxisChosen = Random.Range(0, (Math.Abs(TempX) + Math.Abs(TempY))) + 1;
        if (Math.Abs(TempX) - AxisChosen >= 0)
        {
            //go x direction
            if (TempX < 0) { return DirectionMoved.LEFT; }
            else { return DirectionMoved.RIGHT; }
        }
        else
        {
            //go y Direction
            if (TempY < 0) { return DirectionMoved.DOWN; }
            else { return DirectionMoved.UP; }
        }
    }
}

public static class DirectionExtensions
{
    public static Vector3 GetDirectionVector(this SpriteMovement.DirectionMoved dir)
    {
        switch (dir)
        {
            case SpriteMovement.DirectionMoved.LEFT:
                return Vector3.left;
            case SpriteMovement.DirectionMoved.RIGHT:
                return Vector3.right;
            case SpriteMovement.DirectionMoved.UP:
                return Vector3.up;
            case SpriteMovement.DirectionMoved.DOWN:
                return Vector3.down;
        }
        return Vector3.zero;
    }

    public static int GetHeroSpriteOffeset(this SpriteMovement.DirectionMoved dir)
    {
        switch (dir)
        {
            case SpriteMovement.DirectionMoved.LEFT:
                return 1;
            case SpriteMovement.DirectionMoved.RIGHT:
                return 2;
            case SpriteMovement.DirectionMoved.UP:
                return 3;
            case SpriteMovement.DirectionMoved.DOWN:
                return 0;
        }
        return 0;
    }
}
