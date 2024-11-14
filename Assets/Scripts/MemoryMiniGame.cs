using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MemoryMiniGame : MonoBehaviour
{
    [SerializeField] private List<ColorButton> colorButtons;
    [SerializeField] private GameObject panel;
    [SerializeField] private List<TrialsConfig> trialsConfig;
    [SerializeField] private Button closePanelButton;
    [SerializeField] private TextMeshProUGUI completedTrialsText;
    [SerializeField] private TextMeshProUGUI totalTrialsAmountText;
    [SerializeField] private float showSequenceEntryDuration = 1f;
    [SerializeField] private TextMeshProUGUI lockedText;
    [SerializeField] private AudioSource bellSound;
    [SerializeField] private AudioSource wrongSound;
    [SerializeField] private AudioSource rightSound;

    private List<Sequence> sequences = new List<Sequence>();

    private int completedTrials = 0;
    private int sequenceIndex = 0;

    private bool isLocked;

    private EventService eventService;

    private void Awake()
    {
        closePanelButton.onClick.AddListener(Close);
    }

    private void PlaySequence()
    {
        StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        isLocked = true;
        UpdateText();
        yield return new WaitForSeconds(showSequenceEntryDuration);

        for (int i = 0; i < sequences[completedTrials].SortedSequence.Count; i++)
        {
            int sortedNum = sequences[completedTrials].SortedSequence[i];
            colorButtons[sortedNum].Toggle(true);
            bellSound.Play();
            yield return new WaitForSeconds(showSequenceEntryDuration);
            colorButtons[sortedNum].Toggle(false);
        }

        yield return new WaitForSeconds(showSequenceEntryDuration);

        isLocked = false;
        UpdateText();
    }

    public void Init(EventService eventService)
    {
        this.eventService = eventService;

        CreateSequences();

        for(int i = 0; i < colorButtons.Count; i++)
        {
            colorButtons[i].Setup(eventService, i);
        }

        UpdateText();

        eventService.OnColorButtonClicked.AddListener(ProcessColorButtonClicked);
    }

    private void OnDestroy()
    {
        eventService.OnColorButtonClicked.RemoveListener(ProcessColorButtonClicked);
    }

    private void ProcessColorButtonClicked(int input)
    {
        if(isLocked)
        {
            return;
        }

        if (!sequences[completedTrials].Check(sequenceIndex, input))
        {
            sequenceIndex = 0;
            wrongSound.Play();
            PlaySequence();
            return;
        }

        sequenceIndex++;
        rightSound.Play();

        if (sequenceIndex >= sequences[completedTrials].SortedSequence.Count)
        {
            completedTrials++;
            sequenceIndex = 0;
            UpdateText();

            if (completedTrials >= sequences.Count)
            {
                eventService.OnColorTrialCompleted.InvokeEvent();

                isLocked = true;
                Close();
                return;
            }
            else
            {
                PlaySequence();
            }
        }
    }

    private void UpdateText()
    {
        completedTrialsText.text = completedTrials.ToString();
        totalTrialsAmountText.text = trialsConfig.Count.ToString();
        lockedText.text = isLocked ? "Locked" : "Ready";
    }

    private void CreateSequences()
    {
        for (int i = 0; i < trialsConfig.Count; i++)
        {
            Sequence sequence = new Sequence();

            sequence.SetupIndex(GetRandomSequence(trialsConfig[i].sequenceSize));

            sequences.Add(sequence);
        }
    }


    private List<int> GetRandomSequence(int size)
    {
        List<int> sequence = new List<int>();

        for (int i = 0; i < size; i++)
        {
            sequence.Add(Random.Range(0, 9));
        }

        return sequence;
    }

    public void Open()
    {
        if (panel.activeSelf)
        {
            return;
        }

        panel.SetActive(true);

        PlaySequence();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    [Serializable]
    private struct TrialsConfig
    {
        public int sequenceSize;
    }


    [Serializable]
    public class Sequence
    {
        public List<int> SortedSequence = new List<int>();

        public void SetupIndex(List<int> sortedSequence)
        {
            SortedSequence.Clear();
            SortedSequence.AddRange(sortedSequence);
        }

        public bool Check(int index, int pressedNum)
        {
            return pressedNum == SortedSequence[index];
        }
    }


    [Serializable]
    private struct ColorButton
    {
        public int id;
        public Button Button;
        public GameObject Fill;
        private EventService eventService;

        public void Setup(EventService eventService, int id)
        {
            this.id = id;
            this.eventService = eventService;
            Button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            eventService.OnColorButtonClicked.InvokeEvent(id);
        }

        public void Toggle(bool isOn)
        {
            Fill.SetActive(isOn);
        }
    }
}
