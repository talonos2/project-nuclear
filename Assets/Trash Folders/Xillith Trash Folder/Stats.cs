using UnityEngine;

public abstract class Stats : MonoBehaviour
{
    public int MaxHP;
    public int HP;
    public float attack;
    public float defense;

    public Vector2 startPositionOnScreen = new Vector2(.85f, 1);
    public Vector2 homePositionOnScreen = new Vector2(-.5f, -1);
}