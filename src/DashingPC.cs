using Godot;

internal partial class DashingPC : PlayerController
{
    [Export(hint: PropertyHint.None, hintString: "Allows deceleration when too fast.")] bool debugEnableSlowdown = false;
    [Export(hintString:"In units s^-2")] float force = 40;
    [Export] float maxSpeed = 8;
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
        GD.Print($"Dashinput: {dashInput}.");
        if (dashInput && timer <= -cooldown) { timer = length; isDashing = true; shouldProcessWalk = false; }

        if (timer > 0)
        {
            double factor = delta * force;

            // Construct a fake 2d vector for our forward direction.
            Vector3 flatVel = Velocity; flatVel.Y = 0;

            // If we're too slow, attempt to dash.
            if (flatVel.Length() < maxSpeed)
            {
                // Add force.
                Vector3 flatForward = -Basis.Z; flatForward.Y = 0; flatForward = flatForward.Normalized();
                Velocity += flatForward * (float)factor;
                GD.Print($"Hi, dash should happen now. Also factor is {factor}");
            }

            // If we're too fast, attempt to slow down. Might just disable this actually.
            if (flatVel.Length() > maxSpeed && debugEnableSlowdown)
            {
                Velocity = Velocity.Lerp(Velocity.Normalized() * maxSpeed, 0.9f);
                GD.Print("Oh, we're slowing down.");
            }

            GD.Print($"Apparently we're moving by {Velocity} per second.");
            MoveAndSlide();
        }
        // If we're on cooldown, set dash bool to false.
        else { isDashing = false; shouldProcessWalk = true; }

        // Tick our cooldown tracker.
        if (timer > -cooldown) { timer--; }
    }
}
