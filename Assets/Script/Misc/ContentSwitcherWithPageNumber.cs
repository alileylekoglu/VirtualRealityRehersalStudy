using UnityEngine;
using TMPro;
using Photon.Pun;

public class ContentSwitcherWithPageNumber : MonoBehaviourPun
{
    public GameObject[] contents;
    public TextMeshProUGUI pageNumberText;

    private int currentIndex = 0;

    private void Start()
    {
        UpdateContentVisibility();
        UpdatePageNumber();
    }

    // Called when the "Next" button is pressed
    public void NextContent()
    {
        if (currentIndex < contents.Length - 1)
        {
            currentIndex++;
            UpdateContentVisibility();
            UpdatePageNumber();
        }
    }

    // Called when the "Previous" button is pressed
    public void PreviousContent()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateContentVisibility();
            UpdatePageNumber();
        }
    }

    private void UpdateContentVisibility()
    {
        for (int i = 0; i < contents.Length; i++)
        {
            contents[i].SetActive(i == currentIndex);
        }
    }

    private void UpdatePageNumber()
    {
        if (pageNumberText != null)
        {
            pageNumberText.text = (currentIndex + 1).ToString() + "/" + contents.Length;
        }
    }
}