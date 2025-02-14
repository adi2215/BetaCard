using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAutoMove : MonoBehaviour
{
    public EditableLine editableLine;
    public float speed = 5f;
    public Transform arrow;
    public TrailBall trailManager;

    private int currentPairIndex = 0;
    private float t = 0f;
    private bool isMoving = false;
    private bool shouldStartNewTrail = false;

    void Update()
    {
        if (isMoving)
        {
            MoveAlongCurve();
        }
    }

    public void StartMoving()
    {
        isMoving = true;
        t = 0f;
        currentPairIndex = 0;
        shouldStartNewTrail = false;

        trailManager.StartNewTrail();
    }

    private void MoveAlongCurve()
    {
        if (editableLine.pointPairs.Count == 0) return;

        PointPair pair = editableLine.pointPairs[currentPairIndex];

        if (shouldStartNewTrail)
        {
            shouldStartNewTrail = false;
            trailManager.StartNewTrail();
        }

        t += Time.deltaTime * speed / pair.lineRenderer.positionCount;

        if (t >= 1f)
        {
            t = 0f;

            Debug.Log("ff");

            currentPairIndex++;

            if (currentPairIndex >= editableLine.pointPairs.Count)
            {
                isMoving = false;
                GameStageManager.Instance.NextStage();
                return;
            }
            StartNewTrailOnNextSegment();
        }
        else
        {
            transform.position = BezierQulity.GetPointOnCurve(pair, t);
            trailManager.AddPoint(transform.position);
            UpdateArrowDirection(pair);
        }
    }

    private void StartNewTrailOnNextSegment()
    {
        shouldStartNewTrail = true;
    }

    private void UpdateArrowDirection(PointPair pair)
    {
        if (arrow == null) return;

        Vector3 direction = BezierQulity.GetDirection(pair, t);
        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, angle - 45);
        }
    }

    private IEnumerator RemoveLastPointCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay / 1000f);
        StartNewTrailOnNextSegment();
    }
}
