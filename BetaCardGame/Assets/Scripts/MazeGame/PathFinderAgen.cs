using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;

public class PathFinderAgent : MonoBehaviour
{
    public AStarPathfinding pathfinder;
    public SkeletonAnimation skeletonAnimation;

    private List<Vector3Int> path;
    private int pathIndex;
    private bool isMoving = false;
    private int coinCount = 0;

    public GameObject imgWin;

    void Start()
    {
        SetAnimation("Warmup"); 
    }

    public void MoveTo(Vector3Int target)
    {
        path = pathfinder.FindPath(Vector3Int.FloorToInt(transform.position), target);
        pathIndex = 0;

        if (path != null && path.Count > 0)
        {
            isMoving = true;
            SetAnimation("Idle_running"); 
        }
    }

    void Update()
    {
        if (path == null || pathIndex >= path.Count)
        {
            if (isMoving)
            {
                isMoving = false;
                SetAnimation("Warmup"); 
            }
            return;
        }

        Vector3 targetWorldPos = path[pathIndex] + new Vector3(0.5f, 0.5f, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, 3f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.1f)
        {
            pathIndex++;
        }
    }

    void SetAnimation(string animationName)
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coinCount++;

            Debug.Log("Монет собрано: " + coinCount);

            if (coinCount >= 3)
            {
                imgWin.SetActive(true);
            }
        }
    }
}
