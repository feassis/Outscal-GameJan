public class PlayerModel
{
    public float Speed { get; private set; }
    public float RotationSpeed { get; private set; }

    public PlayerModel(float speed, float rotationSpeed)
    {
        Speed = speed;
        RotationSpeed = rotationSpeed;
    }
}
