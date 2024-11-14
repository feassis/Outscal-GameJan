using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIService : MonoBehaviour
{
    [SerializeField] private MemoryMiniGame MemoryMinigame;
    [SerializeField] private MathPuzzle mathPuzzle;
    [SerializeField] private TextMeshProUGUI completedTrialsText;
    [SerializeField] private TextMeshProUGUI totalTrialsText;

    private EventService eventService;

    public void Init(EventService eventService, int totalTrialNum)
    {
        this.eventService = eventService;
        MemoryMinigame.Init(eventService);
        mathPuzzle.Init(eventService);
        totalTrialsText.text = totalTrialNum.ToString();

        eventService.OnTrialCompleted.AddListener(OnTrialCompleted);
    }

    private void OnTrialCompleted(int completed)
    {
        completedTrialsText.text = completed.ToString();
    }

    public void OpenTrialUI(TrialType trialType)
    {
        switch (trialType)
        {
            case TrialType.None:
                break;
            case TrialType.MathChallange:
                mathPuzzle.Open();
                break;
            case TrialType.MemoryChallange:
                MemoryMinigame.Open();
                break;
        }
    }
}
