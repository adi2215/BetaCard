using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectorManager : MonoBehaviour
{
    public ItemData[] letters;

    public List<ItemData> Current_letters;

    public List<Image> images;

    public GameObject img;

    private bool gameOver = false;

    public void CatchLetter(ItemData item)
    {
        if (gameOver)
            return;

        int index = 0;

        foreach (ItemData letter in Current_letters)
        {
            if (letter.Equals(item))
            {
                SetTransparency(1f, images[index]);
                images.Remove(images[index]);

                Current_letters.Remove(letter);
                    
                break;
            }

            index++;
        }

        if (Current_letters.Count == 0)
        {
            gameOver = true;
            img.SetActive(true); 
        }
    }

    public void SetTransparency(float alpha, Image img)
    {
        Color color = img.color; 
        color.a = alpha; 
        img.color = color;
    }
}
