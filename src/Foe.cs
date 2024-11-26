using Godot;

namespace TPCTest.src
{
    internal partial class Foe : Node3D
    {
        // Replace with proper stat handling
        [ExportCategory("Stats")]
        [Export] int hp = 1;
        [ExportCategory("Refs")]
        [Export] protected Area3D contactHitbox;
        [Export] protected PhysicsBody3D hurtbox;

        public override void _Ready()
        {
            if (contactHitbox == null) { GD.PrintErr("hitbox is null!"); }
            else
            {
                contactHitbox.BodyEntered += OnTriggerEnter;
                contactHitbox.AreaEntered += OnTriggerEnter;
            }
            if (hurtbox == null) { GD.PrintErr("hurtbox is null!"); }
            // hurtbox init here
            else { }
        }

        protected virtual void OnTriggerEnter(Node3D body)
        {
            // Replace with a more robust detection system.
            bool comparison = body is PlayerController;

            if (comparison)
            {
                PlayerController playerController = (PlayerController)body;
                playerController.Damage();
            }
        }

        public virtual void Damage()
        {
            GD.Print($"Hi im {Name} and i took damage.");
        }
    }
}
