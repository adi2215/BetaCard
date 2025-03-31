using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CollectorManager : MonoBehaviour
{
    public ItemData[] letters;
    public List<ItemData> Current_letters = new List<ItemData>();
    public List<Image> images = new List<Image>();
    public List<Transform> imagesPos = new List<Transform>();
    public GameObject img;
    private bool gameOver = false;

    public void CatchLetter(MovingObject letterObj, ItemData item)
    {
        if (!gameOver)
        {
            bool notLetter = true;

            for (int i = 0; i < Current_letters.Count; i++)
            {
                if (Current_letters[i].Equals(item)) 
                {
                    StartCoroutine(SlowMotionEffect());
                    SetTransparency(1f, images[i]);
                    StartCoroutine(MoveAndDestroy(letterObj, imagesPos[i]));

                    images.RemoveAt(i);
                    imagesPos.RemoveAt(i);
                    Current_letters.RemoveAt(i);

                    notLetter = false;
                    gameOver = true;
                    break;
                }
            }

            if (notLetter)
            {
                Debug.Log(letterObj.name + " не найден в Current_letters, удаляю.");
                Destroy(letterObj.gameObject, 0.05f);
                return;
            }

            if (Current_letters.Count == 0)
            {
                gameOver = true;
                img.SetActive(true);
            }
        }
    }



    private IEnumerator MoveAndDestroy(MovingObject moveObj, Transform targetPosition)
    {
        float duration = 0.7f;
        float elapsedTime = 0f;
        Vector3 startPos = moveObj.transform.position;
        Vector3 startScale = moveObj.transform.localScale;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            moveObj.transform.position = Vector3.Lerp(startPos, targetPosition.position, progress);
            moveObj.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            elapsedTime += Time.deltaTime * 2f;
            yield return null;
        }

        moveObj.transform.position = targetPosition.position;
        moveObj.transform.localScale = Vector3.zero;

        if (moveObj.gameObject != null)
        {
            Destroy(moveObj.gameObject, 0.05f);
            gameOver = false;
        }
    }

    private IEnumerator SlowMotionEffect()
    {
        float slowMotionTimeScale = 0.3f;
        float transitionTime = 0.5f;

        float elapsedTime = 0f;
        float startScale = Time.timeScale;

        while (elapsedTime < transitionTime)
        {
            Time.timeScale = Mathf.Lerp(startScale, slowMotionTimeScale, elapsedTime / transitionTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = slowMotionTimeScale;
        yield return new WaitForSecondsRealtime(0.4f);

        elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            Time.timeScale = Mathf.Lerp(slowMotionTimeScale, 1f, elapsedTime / transitionTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
    }

    public void SetTransparency(float alpha, Image img)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }
}
