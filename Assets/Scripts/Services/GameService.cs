using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Lumin;

public class GameService : MonoBehaviour
{
    public static GameService Instance { get; private set; }

    [SerializeField] private float playerSpeed = 4f;
    [SerializeField] private float playerRotationSpeed = 90f;
    [SerializeField] private PlayerView playerViewPrefab = null;
    [SerializeField] private Transform playeSpawnPos;
    [SerializeField] private SnailView snailViewPrefab;
    [SerializeField] private Transform snailSpawnPos;
    [SerializeField] private float snailFollowRange = 0f;
    [SerializeField] private UIService uiService;
    [SerializeField] private Transform mathTrialPos;
    [SerializeField] private Transform memoryPos;
    [SerializeField] private TrialStandView trialViewPrefab;
    [SerializeField] private int trialsToBeCompleted = 2;
    [SerializeField] private GameObject door;
    [SerializeField] private EndGamePopup endGamePopup;

    private PlayerController playerController;
    private SnailController snailController;

    private TrialStandController mathTrial;
    private TrialStandController memoryTrial;

    private EventService eventService;

    private int completedTrials = 0;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InitializeService();
        InstantiateEntities();
        InitializeEvents();
    }

    public void InitializeEvents()
    {
        eventService.OnColorTrialCompleted.AddListener(OnTrialCompleted);
        eventService.OnMathTrialCompleted.AddListener(OnTrialCompleted);
    }

    private void OnTrialCompleted()
    {
        completedTrials++;
        eventService.OnTrialCompleted.InvokeEvent(completedTrials);
        if(completedTrials == trialsToBeCompleted)
        {
            door.SetActive(false);
        }
    }

    private void InitializeService()
    {
        eventService = new EventService();
        uiService.Init(eventService, trialsToBeCompleted);
    }

    private void InstantiateEntities()
    {
        PlayerModel playerModel = new PlayerModel(playerSpeed, playerRotationSpeed);
        playerController = new PlayerController(playerModel, playerViewPrefab, playeSpawnPos.position);

        SnailModel snailModel = new SnailModel(playerController.GetViewTransform(), snailFollowRange);
        snailController = new SnailController(snailModel, snailViewPrefab, snailSpawnPos.position);

        TrialStandModel mathTrialModel = new TrialStandModel(TrialType.MathChallange);
        mathTrial = new TrialStandController(trialViewPrefab, mathTrialModel, mathTrialPos.position, uiService);

        TrialStandModel memoryTrialModel = new TrialStandModel(TrialType.MemoryChallange);
        memoryTrial = new TrialStandController(trialViewPrefab, memoryTrialModel, memoryPos.position, uiService);
    }

    public void EndGame(bool hasWon)
    {
        endGamePopup.Open(hasWon);
    }

    private void OnDestroy()
    {
        eventService.OnColorTrialCompleted.RemoveListener(OnTrialCompleted);
        eventService.OnMathTrialCompleted.RemoveListener(OnTrialCompleted);

        if (Instance == this)
        {
            Instance = null;
        }
    }
}