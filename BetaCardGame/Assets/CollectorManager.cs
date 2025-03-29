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
            int index = 0;

            foreach (ItemData letter in Current_letters)
            {
                if (letter.Equals(item))
                {
                    SetTransparency(1f, images[index]);
                    images.Remove(images[index]);

                    Current_letters.Remove(letter);

                    StartCoroutine(MoveAndDestroy(letterObj, imagesPos[index]));
                    imagesPos.Remove(imagesPos[index]);
                    notLetter = false;
                        
                    break;
                }

                index++;
            }

            if (notLetter)
            {
                Debug.Log("Delete77");
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
        float duration = 0.5f;
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
            Destroy(moveObj.gameObject, 0.3f);
    }

    public void SetTransparency(float alpha, Image img)
    {
        Color color = img.color; 
        color.a = alpha; 
        img.color = color;
    }
}
