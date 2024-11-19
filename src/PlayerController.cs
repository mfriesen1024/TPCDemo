using Godot;

public partial class PlayerController : CharacterBody3D
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;
    Vector2 inputDir;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        inputDir = Input.GetVector("left", "right", "up", "down");

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        velocity=HandleJump(velocity);

        velocity=HandleWalk(velocity);

        Velocity = velocity;
        MoveAndSlide();
    }

    private Vector3 HandleJump(Vector3 velocity)
    {
        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        return velocity;
    }

    private Vector3 HandleWalk(Vector3 velocity)
    {
        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        return velocity;
    }
}
