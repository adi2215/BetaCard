using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerFindLetter : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> letterSlots;
    [SerializeField] private List<LetterData> letterDataList;
    [SerializeField] private TextMeshProUGUI letterActive;
    [SerializeField] private AudioSource soundLetterActive;

    private LetterData activeLetter;

    private List<LetterData> availableLetters;
    private List<LetterData> availableLettersCopy;

    [SerializeField] private GameObject WonNotif;

    private void Start()
    {
        availableLetters = new List<LetterData>(letterDataList);
        availableLettersCopy = new List<LetterData>(letterDataList);
        AssignRandomLetter();
        AssignRandomActiveLetter();
    }
    
    private void AssignRandomLetter()
    {
        foreach (var slot in letterSlots)
        {
            LetterData randomLetter = availableLettersCopy[Random.Range(0, availableLettersCopy.Count)];
            
            slot.text = randomLetter.letter;
            availableLettersCopy.Remove(randomLetter);
        }
    }

    private void AssignRandomActiveLetter()
    {
        if (availableLetters.Count == 0) return;
        
        activeLetter = availableLetters[Random.Range(0, availableLetters.Count)];
        letterActive.text = activeLetter.letter;
        soundLetterActive.clip = activeLetter.sound;
    }
    
    public void CheckFoundLetter(TextMeshProUGUI foundSlot)
    {
        if (foundSlot.text != activeLetter.letter) return;
        
        availableLetters.Remove(activeLetter);
        if (availableLetters.Count > 0)
        {
            AssignRandomActiveLetter();
        }
        else
        {
            WonNotif.SetActive(true);
        }
    }
}