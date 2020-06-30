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
    //protected Renderer sRender;
    protected Vector2Int homeLocation;
    protected Vector2Int exitLocation = new Vector2Int(0, 0);
    protected float waitTimer = 0;

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

    public void SetRenderer() {
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

        float finishedMoving = 0;
        if (facedDirection == DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(tempMovementSpeed);
        }
        if (facedDirection == DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(tempMovementSpeed);
        }
        if (facedDirection == DirectionMoved.UP)
        {
            finishedMoving = MoveUp(tempMovementSpeed);
        }
        if (facedDirection == DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(tempMovementSpeed);
        }
        return finishedMoving;
    }

    protected void SetNextLocationActual(int characterLocX, int characterLocY) {
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
    protected void SetNextLocation(DirectionMoved nextStep) {

        //Debug.Log("next step loc x " + CharacterLocation.x +" y "+ CharacterLocation.y);

       
        if (nextStep == DirectionMoved.UP) {
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
            characterNextLocation.x = characterLocation.x-1;
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

    protected bool IsMoveLocationPassable(int LocX, int LocY) {

        bool MoveableLocation = false;


        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || IsPlatformUp(LocX, LocY)) {
            if (IsLocationDoodadMonsterPassible(LocX, LocY) && IsLocationEntityPassible(LocX, LocY))
                MoveableLocation = true;
        }
           
        
        return MoveableLocation;
    }

    protected bool IsPlayerMoveLocationPassable(int LocX, int LocY)
    {

        bool MoveableLocation = false;
        if (LocX < 0 || LocY < 0) {
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

    protected bool IsMoveLocationMonsterChaseable(int LocX, int LocY) {
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
            || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.AIR || IsPlatformUp(LocX, LocY) )
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

    protected bool IsPlayerInMonsterTerritory(int LocX, int LocY) {
        bool attackable = false;
        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER)
            attackable = true;
        return attackable;

    }
    protected bool IsRandomMoveLocationPassable(int LocX, int LocY) {

        bool MoveableLocation = false;

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER) {
            if (IsLocationDoodadMonsterPassible(LocX, LocY) && IsLocationEntityPassible(LocX, LocY))
                MoveableLocation = true;
        }
           
        //Toggleable Terrain check
       

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

        //Toggleable Terrain check


        return MoveableLocation;

    }

    protected bool IsLocationDoodadPlayerPassible(int LocX, int LocY) {
        bool MoveableLocation = false;

        GameObject DoddadObject = MapGrid.GetComponent<DoodadGrid>().grid[LocX, LocY];
        if (DoddadObject == null)
        {
            MoveableLocation = true;
        }
        else {
            DoodadData doodadObject = DoddadObject.GetComponent<DoodadData>();
            if (doodadObject.isPassable)
                MoveableLocation = true ;
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
            if (DoddadObject.GetComponent<DoodadData>().isPassable|| DoddadObject.GetComponent<DoodadData>().isPlatformTerrain)
                MoveableLocation = true;
        }
        return MoveableLocation;

    }

    protected bool IsLocationEntityPassible(int LocX, int LocY) {
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

    protected GameObject isThereAMonster() {
        GameObject EnemyPresent = null;
        GameObject EntityToFight = MapGrid.GetComponent<EntityGrid>().grid[characterNextLocation.x, characterNextLocation.y]; 

        if (EntityToFight != null) {
            if (EntityToFight.GetComponent<EntityData>().isAMonster) {
                EnemyPresent = EntityToFight;
            }
        }
        return EnemyPresent;
    }

    protected GameObject isThereAPlayer(int CharLocX, int CharLocY)
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

    protected void RemoveOldEntityGridLocation() {

        MapGrid.GetComponent<EntityGrid>().grid[characterLocation.x, characterLocation.y] = null;
    }
    protected void UpdateNewEntityGridLocation() {

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

    public void InitializeNewMap() {
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
        //GameObject exitLocationObj = GameObject.Find("Exit");
        //exitLocation.x= exitLocationObj.transform.
        //omponent<DoodadGrid>().grid[CharacterLocation.x, CharacterLocation.y]; 
    }

    public GameObject GetMapGrid() {
        GameObject MapGridRetrieved = GameObject.Find("Grid");
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

    public float MoveDown(float tempMovementSpeed)
    {

        float DistanceToMove = Time.deltaTime * tempMovementSpeed;
        movedSoFar += DistanceToMove;

        AnimateMove(DirectionMoved.DOWN);


        if (movedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (movedSoFar - 1);
            movedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(0.0f, -DistanceToMove, 0.0f);
        transform.position = transform.position + getMotion;
        return movedSoFar;
    }

    public void FaceDown()
    {
        sRender.material.SetFloat("_Frame", 0+ FLOATING_POINT_FIX);
        animationStep = 0;
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
            sRender.material.SetFloat("_Frame", animationStep + FLOATING_POINT_FIX+(dir.GetHeroSpriteOffeset()*(GetFramesInFilmstrip()+GetNumberOfIdleFrames()) + GetNumberOfIdleFrames()));
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

    public float MoveUp(float tempMovementSpeed)
    {
    
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * tempMovementSpeed;
        movedSoFar += DistanceToMove;

        AnimateMove(DirectionMoved.UP);

        if (movedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (movedSoFar - 1);
            movedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(0.0f, DistanceToMove, 0.0f);
        transform.position = transform.position + getMotion;
        return movedSoFar;

    }

    public void FaceUp()
    {
        sRender.material.SetFloat("_Frame", 21+FLOATING_POINT_FIX);
        animationStep = 0;
    }

    public bool JumpToTarget(float jumpMoveSpeed,Vector2Int jumpDistance)
    {
        float DistanceToMoveX = Time.deltaTime * jumpMoveSpeed * jumpDistance.x;
        float DistanceToMoveY = Time.deltaTime * jumpMoveSpeed * jumpDistance.y;

        movedSoFarX += DistanceToMoveX;
        movedSoFarY += DistanceToMoveY;

        //AnimateSpinning();

        if (Math.Abs(movedSoFarX) > Math.Abs(jumpDistance.x) || Math.Abs(movedSoFarY) > Math.Abs(jumpDistance.y))
        {
            movedSoFarX = 0;
            movedSoFarY = 0;
            return true;
        }
        Vector3 getMotion = new Vector3(DistanceToMoveX, DistanceToMoveY, 0.0f);
        transform.position = transform.position + getMotion;
        
        return false;
    }

    public float MoveLeft(float tempMovementSpeed)
    {
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * tempMovementSpeed;
        movedSoFar += DistanceToMove;

        AnimateMove(DirectionMoved.LEFT);
        if (movedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (movedSoFar - 1);
            movedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(-DistanceToMove, 0.0f,  0.0f);
        transform.position = transform.position + getMotion;

        return movedSoFar;
    }

    public void FaceLeft()
    {
        sRender.material.SetFloat("_Frame", 7+ FLOATING_POINT_FIX);
        animationStep = 0;
    }
    public float MoveRight(float tempMovementSpeed)
    {
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * tempMovementSpeed;
        movedSoFar += DistanceToMove;
        AnimateMove(DirectionMoved.RIGHT);
        if (movedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (movedSoFar - 1);
            movedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(DistanceToMove, 0.0f, 0.0f);
        transform.position = transform.position + getMotion;

        return movedSoFar;
    }

    public void FaceRight()
    {
        sRender.material.SetFloat("_Frame", 14+ FLOATING_POINT_FIX);
        animationStep = 0;
    }

    public void SetLookDirection()
    {
        if (facedDirection == DirectionMoved.DOWN)
            FaceDown();
        if (facedDirection == DirectionMoved.UP)
            FaceUp();
        if (facedDirection == DirectionMoved.LEFT)
            FaceLeft();
        if (facedDirection == DirectionMoved.RIGHT)
            FaceRight();
    }

    protected void TiePositionToGrid()
    {
        transform.position = new Vector3(characterLocation.x + (int)mapZeroLocation.x, characterLocation.y + (int)mapZeroLocation.y, transform.position.z);
    }

    protected DirectionMoved GetChaseStep()
    {
        if (gameData.stealthed) {
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


    public static int GiveMeOne(this Vector3 foo)
    {
        return 1;
    }
}
