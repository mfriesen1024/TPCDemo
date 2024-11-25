using Godot;

internal partial class DashingPC : PlayerController
{
    [Export(hint: PropertyHint.None, hintString: "Allows deceleration when too fast.")] bool debugEnableSlowdown = false;
    [Export] float force = 5;
    [Export] float maxSpeed = 6;
    [Export] int length = 15;
    [Export] int cooldown = 15;
    int timer = 0;

    public bool isDashing = false;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        // Replace with proper input handling.
        // If our timer is 0 and we're pressing the dash key, set timer to length.
        bool dashInput = Input.IsActionJustPressed("dash");
        if (dashInput && timer <= -cooldown) { timer = length; isDashing = true; }

        if (timer > 0)
        {
            double factor = delta * maxSpeed;

            // Construct a fake 2d vector for our forward direction.
            Vector3 flatVel = Velocity; flatVel.Y = 0;

            // If we're too slow, attempt to dash.
            if (flatVel.Length() < maxSpeed)
            {
                // Add force.
                Velocity += Basis.Z * (float)factor;
            }

            // If we're too fast, attempt to slow down. Might just disable this actually.
            if (flatVel.Length() > maxSpeed && debugEnableSlowdown)
            {
                Velocity = Velocity.Lerp(Velocity.Normalized() * maxSpeed, 0.9f);
            }
        }
        // If we're on cooldown, set dash bool to false.
        else { isDashing = false; }

        // Tick our cooldown tracker.
        if (timer > -cooldown) { timer--; }

        MoveAndSlide();
    }
}
