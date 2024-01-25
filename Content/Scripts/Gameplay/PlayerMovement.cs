
using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;

namespace Shooter.Content.Scripts.Gameplay
{
    public class PlayerMovement: BehaviourComponent
    {
        [SerializedField] private Rigidbody movementBody;
        public override void Update()
        {
            base.Update();
            CheckInput();
        }
    
        private void CheckInput()
        {
            var forward = Input.IsKeyDown(Key.W) || Input.IsKeyDown(Key.Up);
            var back = Input.IsKeyDown(Key.S) || Input.IsKeyDown(Key.Down);
            var left = Input.IsKeyDown(Key.A) || Input.IsKeyDown(Key.Left);
            var right = Input.IsKeyDown(Key.D) || Input.IsKeyDown(Key.Right);
    
            var movForward = (forward ? 1f : 0f) + (back ? -1f: 0f); 
            var movSides = (left ? 1f : 0f) + (right ? -1f: 0f);
            var movVector = new Vector2f(movSides, movForward);
            ProcessMovement(movVector);
            //
        }
    
        private void ProcessMovement(Vector2f movement)
        {
            var impulseVector = new Vector3(movement.x, movement.y, 0) * Time.DeltaTime;
            movementBody.AddImpulse(impulseVector);
        }
    }
}