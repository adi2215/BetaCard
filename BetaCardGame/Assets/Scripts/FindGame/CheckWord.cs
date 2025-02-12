using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckWord : MonoBehaviour
{
    private MainManager main;
    private ItemData item;
    public TextMeshProUGUI _text;

    void Start() => main = GetComponentInParent<MainManager>();

    public void Setup(ItemData item) {
        this.item = item;
        _text.text = item.itemName;
    }

    public void CheckIcon() => main.Checking(item);
}
