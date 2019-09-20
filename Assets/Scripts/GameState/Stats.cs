using UnityEngine;

public abstract class Stats : MonoBehaviour
{
    public int MaxHP;
    public int HP;
    public float attack;
    public float defense;

    public Vector2 startPositionOnScreen = new Vector2(.85f, 1);
    public Vector2 homePositionOnScreen = new Vector2(-.5f, -1);
    public Vector2 strikingPointOffset = new Vector2(0, 0);
    public Vector2 gettingStruckPointOffset = new Vector2(0, 0);
    public Vector2 scale = new Vector2(1, 1);
    public Vector2 forceOpponentAdditionalScale = new Vector2(1, 1);
}