using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubblemaker : MonoBehaviour
{
    public GameObject bubble;
    public float height=1;
    public float width=1;
    public float frequency=1;

    private float timeUntilNext;
    // Start is called before the first frame update
    void Start()
    {
        timeUntilNext = Random.Range(.7f, 1.3f)*frequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.fullPause && !GameState.isInBattle)
        {
            timeUntilNext -= Time.deltaTime;
        }
        if (timeUntilNext < 0)
        {
            timeUntilNext = Random.Range(.7f, 1.3f) * frequency;
            SpawnBubble();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(Vector3.right * (width+.36f) + transform.position, Vector3.left * (width + .36f) + transform.position);
        Gizmos.DrawLine(Vector3.up * (height + .41f) + transform.position, Vector3.down * (height+.41f) + transform.position);
    }

    private void SpawnBubble()
    {
        Vector2 direction = Random.insideUnitCircle;
        Vector3 bubblePosition = new Vector3(direction.x * width, direction.y * height, 0) + this.transform.position;
        GameObject.Instantiate(bubble, bubblePosition, this.transform.rotation);
    }
}
