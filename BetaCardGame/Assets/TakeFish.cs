using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class TakeFish : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    public GameObject pointFish;

    public ItemData[] letters;

    public List<ItemData> Current_letters;
    
    public List<Image> images;

    void Start()
    {
        Debug.Log(skeletonAnimation.AnimationState.GetCurrent(0));
    }

    public void CheckFishName(Fish fish, string fishName)
    {
        bool findCorrectFish = false;
        int index = 0;

        fishName = fishName.ToUpper(); 
        foreach (ItemData letter in Current_letters)
        {
            if (fishName.Contains(letter.itemName.ToString().ToUpper()))
            {
                findCorrectFish = true;
                fish.CorrectFish(pointFish);
                SetTransparency(1f, images[index]);
                images.Remove(images[index]);
                Current_letters.Remove(letter);

                break;
            }

            index++;
        }

        if (findCorrectFish)
            skeletonAnimation.AnimationState.SetAnimation(0, "CJ_\"Yeas\"", false);
        else
            skeletonAnimation.AnimationState.SetAnimation(0, "CJ_\"No\"", false);

        StartCoroutine(PlayAnimationAfterDelay(2f, "CJ_idle"));
    }

    private IEnumerator PlayAnimationAfterDelay(float delay, string animationName)
    {
        yield return new WaitForSeconds(delay);
        skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
    }

    public void SetTransparency(float alpha, Image img)
    {
        Color color = img.color; 
        color.a = alpha; 
        img.color = color;
    }
}
