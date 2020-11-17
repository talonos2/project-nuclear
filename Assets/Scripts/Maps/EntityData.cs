using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData : MonoBehaviour
{

    public bool isMainCharacter = false;    
    public bool isAMonster = false;
    public bool isLargeMonster = false;
    public bool isInteractableObject = false;
    public bool isItem = false;
    public bool isSwitch = false;
    public bool isNPC = false;
    //public bool isSpike = false;
    //public bool isPassable = false;
    protected Vector2Int entityLocation;
    protected GameObject mapGrid;
    protected Renderer sRender;
    protected EntityGrid mapEntityGrid;
    protected Vector2 mapZeroLocation;
    public bool isOnCutsceneMap;

    public virtual void ProcessClick(CharacterStats stats)
    {
        //Debug.Log("Nothing to Interact With"); Commented out as all monsters have this as a default now
    }

    public void InitializeEntity() {
        mapGrid = GameObject.Find("Grid");
        mapZeroLocation = mapGrid.GetComponent<PassabilityGrid>().GridToTransform(new Vector2(0, 0));
        entityLocation.x = (int)Math.Round(this.transform.position.x) - (int)mapZeroLocation.x;
        entityLocation.y = (int)Math.Round(this.transform.position.y) - (int)mapZeroLocation.y;
        mapEntityGrid = mapGrid.GetComponent<EntityGrid>();
        mapEntityGrid.grid[entityLocation.x, entityLocation.y] = this.gameObject;
        this.sRender = this.GetComponentInChildren<Renderer>();
        this.sRender.material = new Material(this.sRender.material);
    }
    public void removeEntity() {
        mapEntityGrid.grid[entityLocation.x, entityLocation.y] = null;
    }

    public float distanceToEntity(Transform pollingTransform) {
        float distanceToEntity=(float)Math.Pow(Math.Pow(this.transform.position.x - pollingTransform.position.x, 2) + Math.Pow(this.transform.position.y - pollingTransform.position.y, 2), .5);
        return distanceToEntity;
    }

}
