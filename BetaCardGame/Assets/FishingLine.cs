using UnityEngine;

public class FishingLine : MonoBehaviour
{
    public Transform hook; 
    public Transform catchPoint; 
    private Transform caughtFish = null; 
    private bool isCatching = false;

    void Update()
    {
        if (isCatching) 
        {
            isCatching = false;
            MoveLine(caughtFish.position); 
        }

        if (!isCatching && caughtFish != null)
        {
            PullFishToBoat();
        }
    }

    public void FishCatch(Fish fish) { caughtFish = fish.gameObject.transform; isCatching = true; }

    void MoveLine(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle * (-1));
        transform.localScale = new Vector3(transform.localScale.x, distance, transform.localScale.z);
        hook.position = target;
    }

    void PullFishToBoat()
    {
        if (caughtFish != null)
        {
            caughtFish.position = hook.position;
            hook.position = Vector3.MoveTowards(hook.position, catchPoint.position, Time.deltaTime * 5f);
            if (Vector3.Distance(hook.position, catchPoint.position) < 0.1f)
            {
                Destroy(caughtFish.gameObject);
                caughtFish = null;
                ResetLine();
            }
        }
    }

    void ResetLine()
    {
        hook.position = transform.position;
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
    }
}
