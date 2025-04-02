using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class TakeFish : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    public Fish pointFish = null;

    public Transform pointStartCatch;

    public ItemData[] letters;

    public List<ItemData> Current_letters;
    
    public List<Image> images;

    public FishingLine fishLine;
    
    private Bone lineBone;

    private bool isCatching = false;

    void Start()
    {
        lineBone = skeletonAnimation.Skeleton.FindBone("line");
    }

    public void CheckFishName(Fish fish, string fishName)
    {
        if (pointFish == null && !isCatching)
        {
            bool findCorrectFish = false;
            int index = 0;

            fishName = fishName.ToUpper(); 
            foreach (ItemData letter in Current_letters)
            {
                if (fishName.Contains(letter.itemName.ToString().ToUpper()))
                {
                    findCorrectFish = true;
                    SetTransparency(1f, images[index]);
                    images.Remove(images[index]);

                    if (lineBone != null && fish != null)
                    {
                        fish.CorrectFish(lineBone, skeletonAnimation);
                        pointFish = fish;
                        skeletonAnimation.AnimationState.ClearTrack(0);
                    }

                    Current_letters.Remove(letter);

                    break;
                }

                index++;
            }

            if (!findCorrectFish)
            {
                TrackEntry entryNoAnim = skeletonAnimation.AnimationState.SetAnimation(0, "CJ_\"No\"", false);
                entryNoAnim.TimeScale = 1.5f;

                skeletonAnimation.AnimationState.AddAnimation(0, "CJ_idle", true, 0);
            }
        }

        //StartCoroutine(PlayAnimationAfterDelay(4f, "CJ_idle", true));
    }

    void LateUpdate()
    {
        if (pointFish != null && !isCatching)
        {
            if (!pointFish.fishTook)
                skeletonAnimation.AnimationState.SetEmptyAnimation(0, 0);

            Vector3 boneWorldPosition = skeletonAnimation.transform.TransformPoint(new Vector3(lineBone.WorldX, lineBone.WorldY, 0));

            Vector3 newPosition = Vector3.MoveTowards(boneWorldPosition, pointFish.fishTook ? pointStartCatch.position : pointFish.transform.position, 8f * Time.deltaTime);

            Vector3 localPosition = skeletonAnimation.transform.InverseTransformPoint(newPosition);

            lineBone.WorldX = localPosition.x;
            lineBone.WorldY = localPosition.y;

            lineBone.SetLocalPosition(localPosition);


            if (Vector3.Distance(boneWorldPosition, pointFish.transform.position) < 0.1f)
            {

                Debug.Log("fffe");

                pointFish.fishTook = true;

                skeletonAnimation.AnimationState.SetAnimation(0, "CJ_Hand_carching", true);

                if (Vector3.Distance(boneWorldPosition, pointStartCatch.position) < 0.05f)
                {
                    StartCoroutine(PlayAnimationAfterDelay(0.01f, "CJ_Fish_catch", "CJ_idle"));
                    isCatching = true;
                }
            }
        }
    }

    private IEnumerator PlayAnimationAfterDelay(float delay, string animationName, string nextAnimation)
    {
        yield return new WaitForSeconds(delay);

        TrackEntry catchAnimation = skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);

        catchAnimation.TrackTime = 0.01f;

        StartCoroutine(AnimationDelay(2f, nextAnimation));

        Destroy(pointFish.gameObject, 0.6f);
    }

    private IEnumerator AnimationDelay(float delay, string nextAnimation)
    {
        yield return new WaitForSeconds(delay);

        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
        isCatching = false;
        pointFish = null;
    }

    private IEnumerator AnimationDelayCatch(float delay, string nextAnimation)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("fe");

        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
    }

    public void SetTransparency(float alpha, Image img)
    {
        Color color = img.color; 
        color.a = alpha; 
        img.color = color;
    }
}
