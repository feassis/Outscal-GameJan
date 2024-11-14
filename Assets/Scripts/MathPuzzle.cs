using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MathPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI completedTrialsText;
    [SerializeField] private TextMeshProUGUI totalTrialsAmountText;
    [SerializeField] private TextMeshProUGUI leftNumText;
    [SerializeField] private TextMeshProUGUI rightNumText;
    [SerializeField] private TextMeshProUGUI simbolTextText;
    [SerializeField] private Button solveButton;
    [SerializeField] private TMP_InputField responseField;
    [SerializeField] private List<OperationConfig> operationConfigs = new List<OperationConfig>();
    [SerializeField] private AudioSource rightSound;
    [SerializeField] private AudioSource wrongSound;
    [SerializeField] private Button closeButton;

    private EventService eventService;
    private List<MathChallange> mathChallanges = new List<MathChallange>();

    private int completedTrials = 0;

    private void Awake()
    {
        solveButton.onClick.AddListener(Evaluate);
        closeButton.onClick.AddListener(Close);
    }

    public void Init(EventService eventService)
    {
        this.eventService = eventService;

        CreateMathProblems();
    }

    private void Evaluate()
    {
        if(completedTrials >= operationConfigs.Count)
        {
            return;
        }

        if (mathChallanges[completedTrials].Check(int.Parse(responseField.text)))
        {
            completedTrials++;
            rightSound.Play();
            UpdateText();

            if (completedTrials >= mathChallanges.Count)
            {
                eventService.OnMathTrialCompleted.InvokeEvent();
            }
        }
        else
        {
            wrongSound.Play();
        }
    }

    private void UpdateText()
    {
        completedTrialsText.text = completedTrials.ToString();
        totalTrialsAmountText.text = mathChallanges.Count.ToString();

        if(completedTrials < mathChallanges.Count)
        {
            leftNumText.text = mathChallanges[completedTrials].LeftNum.ToString();
            rightNumText.text = mathChallanges[completedTrials].RightNum.ToString();

            simbolTextText.text = mathChallanges[completedTrials].Op switch
            {
                Operation.Subtraction => "-",
                Operation.None => "throw new NotImplementedException()",
                Operation.Adition => "+",
                Operation.Multiplication => "*",
                Operation.Division => "/"
            };
        }
        else
        {
            leftNumText.text = "";
            rightNumText.text = "";
            simbolTextText.text = "";
        }
    }

    public void Open()
    {
        panel.SetActive(true);
        UpdateText();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    private void CreateMathProblems()
    {
        for(int i = 0; i < operationConfigs.Count; i++)
        {
            var leftNum = Random.Range(operationConfigs[i].LowestNum, operationConfigs[i].HighestNum + 1);
            var rightNum = Random.Range(operationConfigs[i].LowestNum, operationConfigs[i].HighestNum + 1);

            mathChallanges.Add(new MathChallange(leftNum, rightNum, operationConfigs[i].Operation));
        }
    }

    [Serializable]
    private class OperationConfig
    {
        public int LowestNum;
        public int HighestNum;
        public Operation Operation;
    }

    private class MathChallange
    {
        public int LeftNum;
        public int RightNum;
        public Operation Op;

        public MathChallange(int leftNum, int rightNum, Operation operation)
        {
            LeftNum = leftNum;
            RightNum = rightNum;
            Op = operation;
        }

        public bool Check(int resp)
        {
            return Op switch
            {
                Operation.Subtraction => LeftNum - RightNum == resp,
                Operation.Adition => LeftNum + RightNum == resp,
                Operation.Multiplication => LeftNum * RightNum == resp,
                Operation.Division => LeftNum / RightNum == resp,
                _ => false
            };
        }
    }

    public enum Operation
    {
        None = 0,
        Subtraction = 1,
        Adition = 2,
        Multiplication = 3,
        Division = 4,
    }
}
