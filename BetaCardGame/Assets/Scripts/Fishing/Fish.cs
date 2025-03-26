using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 vec = new Vector3(0, 0, 0);
    private float leftEdge;
    private string letter;
    private string skinName;
    public bool fishTook = false;
    public Image letter_Img;
    private Vector3 _pointFish;
    private Bone _bone;
    public SkeletonAnimation _skeletonAnimation;
    private bool fishCatch = false;

    public string[] availableSkins; 
    private int currentSkinIndex = 0;

    public void fishDirection(Vector3 _vec) => vec = _vec;

    public void giveLetter(string _letter) => letter = _letter;

    public void giveSkin(int _skinName) => ChangeSkin(_skinName);

    public void giveImageLetter(Sprite _letter) => letter_Img.sprite = _letter;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        if (fishTook && fishCatch)
        {
            Vector3 bonePosition = new Vector3(_bone.WorldX, _bone.WorldY, 0f);
            Vector3 worldPosition = _skeletonAnimation.transform.TransformPoint(bonePosition);
            transform.position = worldPosition;
        }
        else if (!fishCatch)
        {
            transform.position += vec * speed * Time.deltaTime;

            if (transform.position.x < leftEdge) {
                Destroy(gameObject, 1);
            }
        }
    }

    void OnMouseDown()
    {
        TakeFish takeFish = FindObjectOfType<TakeFish>();
        if (takeFish.pointFish == null)
            takeFish.CheckFishName(this, letter);
    }

    public void CorrectFish(Bone bone, SkeletonAnimation skeletonAnimation)
    {
        fishCatch = true;
        _bone = bone;
        _skeletonAnimation = skeletonAnimation;
    }

    public void ChangeSkin(int index)
    {
        if (_skeletonAnimation == null || index < 0 || index >= availableSkins.Length)
        {
            Debug.LogError("Неверный индекс скина!");
            return;
        }

        string newSkinName = availableSkins[index];
        Skeleton skeleton = _skeletonAnimation.skeleton;
        Skin newSkin = _skeletonAnimation.Skeleton.Data.FindSkin(newSkinName);

        if (newSkin != null)
        {
            skeleton.SetSkin(newSkin);
            skeleton.SetSlotsToSetupPose();
            _skeletonAnimation.AnimationState.Apply(skeleton);

            _skeletonAnimation.initialSkinName = newSkinName;
            currentSkinIndex = index;
        }
        else
        {
            Debug.LogError($"Скин '{newSkinName}' не найден!");
        }
    }

    public void NextSkin()
    {
        int newIndex = (currentSkinIndex + 1) % availableSkins.Length; 
        ChangeSkin(newIndex);
    }

    public void PreviousSkin()
    {
        int newIndex = (currentSkinIndex - 1 + availableSkins.Length) % availableSkins.Length; // Предыдущий скин
        ChangeSkin(newIndex);
    }

}
