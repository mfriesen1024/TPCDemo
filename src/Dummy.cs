using Godot;

namespace TPCTest.src
{
    // A training dummy that changes colours when hit.
    internal partial class Dummy : Foe
    {
        [Export] MeshInstance3D meshInstance;
        StandardMaterial3D material;
        [Export] Color defaultColour = new Color(1,0.75f,0.5f), hurtColour = new Color(0.5f,0.1f,0.1f);

        public override void _Ready()
        {
            base._Ready();

            if (contactHitbox == null || meshInstance == null) { return; }
            else
            {
                contactHitbox.BodyExited += OnTriggerExit;
                contactHitbox.AreaExited += OnTriggerExit;

                material = meshInstance.Mesh.SurfaceGetMaterial(0) as StandardMaterial3D;
                material.AlbedoColor = defaultColour;
            }
        }

        private void OnTriggerExit(Node3D body)
        {
            material.AlbedoColor = defaultColour;
        }

        public override void Damage()
        {
            base.Damage();

            material.AlbedoColor = hurtColour;
        }
    }
}
