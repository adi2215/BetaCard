using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using System.Collections;

public class PathFinderAgent : MonoBehaviour
{
    public AStarPathfinding pathfinder;
    public SkeletonAnimation skeletonAnimation;

    private List<Vector3Int> path;
    private int pathIndex;
    private bool isMoving = false;
    private int coinCount = 0;

    public GameObject imgWin;

    public LightSystem lightSystem;

    public Vector3Int startPosition;

    private Vector3 lastPosition;

    bool walk = true;

    bool objTakeit = false;

    void Start()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "Jolly_idle", true);
        startPosition = Vector3Int.FloorToInt(transform.position);
        lightSystem.InitializeLight(startPosition);
        lastPosition = transform.position;
    }

    public void MoveTo(Vector3Int target)
    {
        if (!lightSystem.IsTileLit(target)) return;

        path = pathfinder.FindPath(Vector3Int.FloorToInt(transform.position), target);
        pathIndex = 0;
        walk = true;
    }

    void Update()
    {
        if (path == null || pathIndex >= path.Count)
        {
            if (isMoving)
            {
                isMoving = false;
                skeletonAnimation.AnimationState.SetAnimation(0, "Jolly_idle", true);
            }
            return;
        }

        Vector3 targetWorldPos = path[pathIndex] + new Vector3(0.5f, 0.5f, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, 3f * Time.deltaTime);
        
        float epsilon = 0.01f;
        if (Vector3.Distance(transform.position, targetWorldPos) > epsilon && walk)
        {
            Debug.Log((transform.position - targetWorldPos).normalized);
            UpdateAnimation((transform.position - targetWorldPos).normalized);
            walk = false;
        }

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.1f)
        {
            Debug.Log(path[pathIndex]);

            if (!objTakeit)
                isMoving = true;
                
            lightSystem.UpdateLight(path[pathIndex]);
            pathIndex++;
        }
    }


    void UpdateAnimation(Vector3 direction)
    {
        if (Mathf.Abs(1f - Mathf.Abs(direction.x)) > 0.1)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Jolly_move_top", true);
            transform.localScale = new Vector3(1, 1, 1) * 0.09f;
        }

        else if (direction.x < 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Jolly_move_foreward", true);
            transform.localScale = new Vector3(1, 1, 1) * 0.09f;
        }
        else if (direction.x > 0) 
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Jolly_move_foreward", true);
            transform.localScale = new Vector3(-1, 1, 1) * 0.09f; 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            isMoving = false;
            objTakeit = true;

            TextMeshProUGUI text = collision.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();

            if (text.text == "F")
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Jolly_happy", false);
                StartCoroutine(ShowWinImageWithDelay(1.5f, true)); 
            }
            else
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Jolly_Sad", false);
                StartCoroutine(ShowWinImageWithDelay(1.5f, false)); 
            }
        }
    }

    private IEnumerator ShowWinImageWithDelay(float delay, bool rightLetter)
    {
        yield return new WaitForSeconds(delay + 1f);
        isMoving = true;
        objTakeit = false;

        if (rightLetter)
        {
            yield return new WaitForSeconds(delay);
            imgWin.SetActive(true);
        }
    }
}
