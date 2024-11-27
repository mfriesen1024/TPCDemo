using Godot;
using TPCTest.src;

internal partial class DashingPC : PlayerController
{
    [ExportCategory("Refs")]
    [Export] Area3D dashHitbox;
    [Export] GpuParticles3D particles;
    [Export] ParticleProcessMaterial particleProcessMaterial;
    [ExportCategory("Stats")]
    [Export(hintString: "In units s^-2")] float force = 40;
    [Export] float maxSpeed = 8;
    [Export] int length = 15;
    [Export] int cooldown = 15;
    [ExportCategory("Debug")]
    [Export(hintString: "Allows deceleration when too fast.")] bool debugEnableSlowdown = false;
    int timer = 0;

    public bool isDashing = false;

    public override void _Ready()
    {
        base._Ready();

        if(particles == null || particleProcessMaterial == null) { GD.PrintErr("Dash particle stuff is null."); }

        if (dashHitbox != null)
        {
            dashHitbox.BodyEntered += DashTriggerEntered;
            dashHitbox.AreaEntered += DashTriggerEntered;
        }
        else { GD.PrintErr("Player dash hitbox is null."); }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        // Replace with proper input handling.
        // If our timer is 0 and we're pressing the dash key, set timer to length.
        bool dashInput = Input.IsActionJustPressed("dash");
        if (dashInput && timer <= -cooldown) { SetDashState(true); }

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
            }

            // If we're too fast, attempt to slow down. Might just disable this actually.
            if (flatVel.Length() > maxSpeed && debugEnableSlowdown)
            {
                Velocity = Velocity.Lerp(Velocity.Normalized() * maxSpeed, 0.9f);
            }

            MoveAndSlide();
        }
        // If we're on cooldown, set dash bool to false.
        else { SetDashState(false); }

        // Tick our cooldown tracker.
        if (timer > -cooldown) { timer--; }
    }

    private void SetDashState(bool active)
    {
        if (active) { 
            timer = length; isDashing = true; 
            shouldProcessWalk = false;
            particles.ProcessMaterial = particleProcessMaterial;
            particles.Emitting = true;
        }
        else { isDashing = false;shouldProcessWalk = true; particles.Emitting = false; }
    }

    private void DashTriggerEntered(Node3D body)
    {
        // Firstly, we should only do things if the player is dashing.
        if (isDashing)
        {
            // Theres probably a better way to handle detection.
            if (body.IsInGroup("foe"))
            {
                Foe foe = body.GetParent() as Foe;
                foe.Damage();
            }
        }
    }

    internal override void Damage()
    {
        if (!isDashing)
        {
            base.Damage();
        }
        else { GD.Print("Player avoided damage by dashing."); }
    }
}
