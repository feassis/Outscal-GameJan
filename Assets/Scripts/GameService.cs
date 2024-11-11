using UnityEngine;

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

    private PlayerController playerController;
    private SnailController snailController;


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
        InstantiateEntities();
    }

    private void InstantiateEntities()
    {
        PlayerModel playerModel = new PlayerModel(playerSpeed, playerRotationSpeed);
        playerController = new PlayerController(playerModel, playerViewPrefab, playeSpawnPos.position);

        SnailModel snailModel = new SnailModel(playerController.GetViewTransform(), snailFollowRange);
        snailController = new SnailController(snailModel, snailViewPrefab, snailSpawnPos.position);


    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}