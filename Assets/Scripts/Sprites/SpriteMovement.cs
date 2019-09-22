using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpriteMovement : MonoBehaviour
{
    // Start is called before the first frame update

   // public Sprite MovementSprite;
    //public Sprite[] MovementSprites;
    public float MoveSpeed = 6;
    public float framesPerSecond = 30;

    protected GameObject MapGrid;
    protected Vector2 mapZeroLocation;
    protected EntityGrid mapEntityGrid;
    protected bool currentlyMoving = false;
    protected GameObject ThePlayer;

    //private SpriteRenderer spriteR;
    private string spriteNames;

    private float movedSoFar = 0;
    private float timeSinceLastAnimation = 0;
    private int animationStep = 0;
    protected GameData gameData;

    protected Vector2Int characterLocation;
    protected Vector2Int characterNextLocation;
    [HideInInspector]
    public DirectionMoved facedDirection = DirectionMoved.LEFT;
    protected Renderer sRender;
    protected Vector2Int homeLocation;
    protected Vector2Int exitLocation = new Vector2Int(0, 0);

    public enum DirectionMoved
    { NONE, UP, RIGHT, DOWN, LEFT }

    public void Start()
    {
        InitializeNewMap();
        InitializeSpriteLocation();
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
        ThePlayer = GameObject.FindGameObjectWithTag("Player");
        gameData =GameObject.Find("GameStateData").GetComponent<GameData>();


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

    protected void SetCurrentLocation() {
        //Vector3 roundLocation = new Vector3((int)Math.Round(this.transform.position.x), (int)Math.Round(this.transform.position.y),0);
        //this.transform.position = roundLocation;
        characterLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        characterLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
        //Debug.Log("" + CharacterLocation.x + " "+CharacterLocation.y);
    }

    protected float MoveToNextSquare()
    {

        float finishedMoving = 0;
        if (facedDirection == DirectionMoved.LEFT)
        {
            finishedMoving = MoveLeft(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.RIGHT)
        {
            finishedMoving = MoveRight(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.UP)
        {
            finishedMoving = MoveUp(MoveSpeed);
        }
        if (facedDirection == DirectionMoved.DOWN)
        {
            finishedMoving = MoveDown(MoveSpeed);
        }
        return finishedMoving;
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


    }

    protected bool IsMoveLocationPassable(int LocX, int LocY) {

        bool MoveableLocation = false;


        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL  || IsPlatformUp(LocX, LocY)) {
            if (IsLocationDoodadFree(LocX, LocY) && IsLocationEntityFree(LocX, LocY))
                MoveableLocation = true;
        }
           
        
        return MoveableLocation;
    }

    protected bool IsPlayerMoveLocationPassable(int LocX, int LocY)
    {

        bool MoveableLocation = false;

        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL
            || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || IsPlatformUp(LocX, LocY))
        {
            if (IsLocationDoodadFree(LocX, LocY) && IsLocationEntityFree(LocX, LocY))
                MoveableLocation = true;
        }
        return MoveableLocation;
    }

    protected bool IsMoveLocationMonsterChaseable(int LocX, int LocY) {
        bool MoveableLocation = false;


        if (MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.NORMAL
            || MapGrid.GetComponent<PassabilityGrid>().grid[LocX, LocY] == PassabilityType.MONSTER || IsPlatformUp(LocX, LocY))
        {
            if (IsLocationDoodadFree(LocX, LocY) && IsLocationEntityFree(LocX, LocY))
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
            if (IsLocationDoodadFree(LocX, LocY) && IsLocationEntityFree(LocX, LocY))
                MoveableLocation = true;
        }
           
        //Toggleable Terrain check
       

        return MoveableLocation;

    }

    protected bool IsLocationDoodadFree(int LocX, int LocY) {
        bool MoveableLocation = true;

        GameObject DoddadObject = MapGrid.GetComponent<DoodadGrid>().grid[LocX, LocY];
        if (DoddadObject != null)
        {
            if (DoddadObject.GetComponent<DoodadData>().isBlockableTerrain)
                MoveableLocation = false;
            if (DoddadObject.GetComponent<DoodadData>().isBackgroundCharacter)
                MoveableLocation = false;
        }
        return MoveableLocation;

    }

    protected bool IsLocationEntityFree(int LocX, int LocY) {
        bool MoveableLocation = false;
        GameObject entityInLocation = MapGrid.GetComponent<EntityGrid>().grid[LocX, LocY];
        if (entityInLocation == null)
            MoveableLocation = true;
        else if (entityInLocation.GetComponent<EntityData>().isPassable)
            MoveableLocation = true;



        return MoveableLocation;
    }

  /*  protected bool IsLocationEntityFreeExceptPlayer(int LocX, int LocY)
    {
        bool MoveableLocation = false;
        GameObject entityToCheck = MapGrid.GetComponent<EntityGrid>().grid[LocX, LocY];
        if (entityToCheck == null) { MoveableLocation = true; }
        else if (entityToCheck.GetComponent<EntityData>().isMainCharacter) {
            MoveableLocation = true;
        }            
        return MoveableLocation;
    }*/

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
        GameObject EntityToFight = MapGrid.GetComponent<EntityGrid>().grid[CharLocX, CharLocY]; ;

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

    public void InitializeNewMap() {
        MapGrid = GetMapGrid();
        mapZeroLocation = MapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        mapEntityGrid = MapGrid.GetComponent<EntityGrid>();
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

    // Update is called once per frame



    void Update()
    {
       


    }


    public float MoveDown(float MoveSpeed){

        float DistanceToMove = Time.deltaTime * MoveSpeed;
        movedSoFar += DistanceToMove;

        AnimateMoveDown();


        if (movedSoFar > 1)
        {
            DistanceToMove = DistanceToMove - (movedSoFar - 1);
            movedSoFar = 0;
        }
        Vector3 getMotion = new Vector3(0.0f, -DistanceToMove, 0.0f);
        transform.position = transform.position + getMotion;
        //transform.position.y = (float)(int)Math.Round(transform.position.y);
        return movedSoFar;
    }

    public void FaceDown()
    {
        sRender.material.SetInt("_Frame", 0);
        animationStep = 0;
    }

    private void AnimateMoveDown()
    {
        timeSinceLastAnimation += Time.deltaTime;
        if (timeSinceLastAnimation >= 1 / framesPerSecond) { timeSinceLastAnimation = 0;
            animationStep += 1;
            if (animationStep > 6) { animationStep = 1;  }
            sRender.material.SetInt("_Frame", animationStep);
        }

    }

    public float MoveUp(float MoveSpeed)
    {
    
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * MoveSpeed;
        movedSoFar += DistanceToMove;

        AnimateMoveUp();

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
        sRender.material.SetInt("_Frame", 21);
        animationStep = 0;
    }

    
    private void AnimateMoveUp()
    {

        timeSinceLastAnimation += Time.deltaTime;
        if (timeSinceLastAnimation >= 1 / framesPerSecond)
        {
            timeSinceLastAnimation = 0;
            animationStep += 1;
            if (animationStep > 6) { animationStep = 1; }
            sRender.material.SetInt("_Frame", animationStep+21);
        }
      
    }
    public float MoveLeft(float MoveSpeed)
    {
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * MoveSpeed;
        movedSoFar += DistanceToMove;

        AnimateMoveLeft();
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
        sRender.material.SetInt("_Frame", 7);
        animationStep = 0;
    }

    private void AnimateMoveLeft()
    {
        timeSinceLastAnimation += Time.deltaTime;
        if (timeSinceLastAnimation >= 1 / framesPerSecond)
        {
            timeSinceLastAnimation = 0;
            animationStep += 1;
            if (animationStep > 6) { animationStep = 1; }
            sRender.material.SetInt("_Frame", animationStep+7);
        }

    }
    public float MoveRight(float MoveSpeed)
    {
        float DistanceToMove = 1;
        DistanceToMove = Time.deltaTime * MoveSpeed;
        movedSoFar += DistanceToMove;
        AnimateMoveRight();
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
        sRender.material.SetInt("_Frame", 14);
        animationStep = 0;
    }

    private void AnimateMoveRight()
    {
        timeSinceLastAnimation += Time.deltaTime;
        if (timeSinceLastAnimation >= 1 / framesPerSecond)
        {
            timeSinceLastAnimation = 0;
            animationStep += 1;
            if (animationStep > 6) { animationStep = 1; }
            sRender.material.SetInt("_Frame", animationStep+14);
        }
       
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

    protected DirectionMoved GetChaseStep()
    {
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
