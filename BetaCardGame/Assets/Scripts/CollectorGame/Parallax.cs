using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
    public float animationSpeed = 1f;
    public float delayBeforeStart = 10f; 
    private MeshRenderer meshRenderer;
    private bool gameStarted = false;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    private void Start()
    {
        StartCoroutine(StartAfterDelay(delayBeforeStart));
    }
    
    private IEnumerator StartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameStarted = true;
    }

    private void Update()
    {
        if (!gameStarted) return;
        meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }
}
