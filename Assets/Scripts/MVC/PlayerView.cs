using UnityEngine;

public class PlayerView :  MonoBehaviour
{
    private PlayerController controller;
    private CharacterController characterController;
    [SerializeField] private Animator animator;

    private const float crossFadeTime = 0.1f;
    private const string idleAnim = "idle";
    private const string walkAnim = "walk";

    private AnimState currentState;

    private enum AnimState
    {
        None = 0,
        Idle = 1,
        Walking = 2,
    }
    public Vector2 MovementValue {  get; private set; }
    public Transform CameraTransform { get; private set; }

    [field: SerializeField] public Transform PlayerBody {  get; private set; }

    private void Awake()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        CameraTransform = Camera.main.transform;
    }

    public void SetController(PlayerController controller)
    {
        this.controller = controller;
    }

    private void Update()
    {
        float yMovementInput = Input.GetAxis("Vertical");
        float xMovementInput = Input.GetAxis("Horizontal");

        MovementValue = new Vector2 (yMovementInput, xMovementInput);
        MovementValue.Normalize();

        Move();
    }

    public bool IsGrounded() => characterController.isGrounded;

    public void PlayMovementAnimation(Vector3 movement)
    {
        if(movement == Vector3.zero && currentState != AnimState.Idle)
        {
            currentState = AnimState.Idle;
            animator.CrossFadeInFixedTime(idleAnim, crossFadeTime);
        }
        else if(movement != Vector3.zero && currentState != AnimState.Walking)
        {
            currentState = AnimState.Walking;
            animator.CrossFadeInFixedTime(walkAnim, crossFadeTime);
        }
    }

    public void Move()
    {
        var movement = controller.CalculateMovement(Time.deltaTime);

        characterController.Move(movement *  Time.deltaTime);
    }
}
