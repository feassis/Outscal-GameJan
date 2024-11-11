using UnityEngine;

public class PlayerController
{
    private PlayerModel model;
    private PlayerView view;

    public PlayerController(PlayerModel model, PlayerView viewPrefab, Vector3 spawnPos)
    {
        this.model = model;
        this.view = GameObject.Instantiate<PlayerView>(viewPrefab);
        this.view.transform.position = spawnPos;
        this.view.SetController(this);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        if(movement == Vector3.zero)
        {
            return;
        }
        view.PlayerBody.rotation = Quaternion.Lerp(view.PlayerBody.rotation,
            Quaternion.LookRotation(movement), GetRotationSpeed() * deltaTime);
    }

    public Transform GetViewTransform() => view.transform;

    public Vector3 CalculateMovement(float deltaTime)
    {
        var cameraForward = view.CameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        var cameraRight = view.CameraTransform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movement = new Vector3();

        movement.x = view.MovementValue.x;
        movement.y = 0;
        movement.z = view.MovementValue.y;

        movement.Normalize();
        movement *= GetSpeed();

        movement = cameraForward * movement.x + cameraRight * movement.z;

        view.PlayMovementAnimation(movement);

        FaceMovementDirection(movement, deltaTime);

        return (movement + Vector3.up * (view.IsGrounded() ? 0 : Physics.gravity.y));
    }

    public float GetSpeed() => model.Speed;
    public float GetRotationSpeed() => model.RotationSpeed;
}
