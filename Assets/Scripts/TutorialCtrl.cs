using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCtrl : MonoBehaviour
{
    public CustomButton prvBtn;
    public CustomButton nextBtn;
    public CustomButton playGameBtn;

    public GameObject[] tutorialPages;
    private int currentPageIndex = 0;
    private System.Action onTutorialComplete;

    public void OpenTutorial(Action onTutorialComplete)
    {
        this.onTutorialComplete = onTutorialComplete;

        prvBtn.RemoveAllAndAddListener(PreviousPage);
        nextBtn.RemoveAllAndAddListener(NextPage);
        playGameBtn.RemoveAllAndAddListener(() =>
               {
                   CloseTutorial();
                   onTutorialComplete?.Invoke();
               });

        gameObject.SetActive(true);
        currentPageIndex = 0;
        for (int i = 1; i < tutorialPages.Length; i++)
        {
            tutorialPages[i].SetActive(false);
        }
        tutorialPages[currentPageIndex].SetActive(true);
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        prvBtn.gameObject.SetActive(currentPageIndex > 0);
        nextBtn.gameObject.SetActive(currentPageIndex < tutorialPages.Length - 1);
        playGameBtn.gameObject.SetActive(currentPageIndex == tutorialPages.Length - 1);
    }

    private void NextPage()
    {
        if (currentPageIndex < tutorialPages.Length - 1)
        {
            tutorialPages[currentPageIndex].SetActive(false);
            tutorialPages[++currentPageIndex].SetActive(true);
            UpdateButtonVisibility();
        }
    }

    private void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            tutorialPages[currentPageIndex].SetActive(false);
            tutorialPages[--currentPageIndex].SetActive(true);
            UpdateButtonVisibility();
        }
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}
