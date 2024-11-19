using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    Node3D camParent;

    Vector2 moveDir;
    Vector2 lookDir;

    public override void _Ready()
    {
        camParent = (Node3D)GetChild(0);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Replace with proper input handling.
        moveDir = Input.GetVector("left", "right", "up", "down");

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        velocity=HandleJump(velocity);

        velocity=HandleWalk(velocity);

        HandleLook();

        Velocity = velocity;
        MoveAndSlide();
    }

    // Replace with proper input handling.
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if(@event is InputEventMouseMotion mouseMotion)
        {
            lookDir = mouseMotion.Relative;
        }
    }


    private Vector3 HandleJump(Vector3 velocity)
    {
        // Replace with proper input handling.
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
        Vector3 direction = (Transform.Basis * new Vector3(moveDir.X, 0, moveDir.Y)).Normalized();
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

    // Replace this with a proper look script.
    private void HandleLook()
    {
        // Rotate the camera parent by our vertical mouse move.
        camParent.RotateObjectLocal(camParent.Basis.X,-lookDir.Y/50);
        // Then rotate ourselves by our horizontal move.
        RotateObjectLocal(Basis.Y, -lookDir.X/50);

        // Replace with proper input handling
        lookDir = Vector2.Zero;
    }
}
