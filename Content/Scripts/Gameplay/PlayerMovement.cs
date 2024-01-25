
using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;

namespace Shooter.Content.Scripts.Gameplay
{
    public class PlayerMovement: BehaviourComponent
    {
        [SerializedField] private GameObject cameraTarget;
        [SerializedField] private float moveSpeed;

        public override void Start()
        {
            //movementBody = this.GameObject.GetComponent<Rigidbody>();
        }

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
    
            var movForward = 0f + (forward ? 1f : 0f) + (back ? -1f: 0f); 
            var movSides = 0f + (left ? -1f : 0f) + (right ? 1f: 0f);
            
            var movVector = new Vector2f(movSides, movForward);
            ProcessMovement(movVector);
            //
        }

        private void ProcessMovement(Vector2f movement)
        {
            //GameObject.Transform.Position = 
            var direction = cameraTarget.Transform.Forward;
            var realVector = new Vector3(movement.x, movement.y, 0).normalized().projectOnFlat(Vector3.Up);
            var impulseVector = new Vector3(movement.x, movement.y, 0).normalized() * Time.DeltaTime * moveSpeed;
            if (double.IsNaN(impulseVector.x) || double.IsNaN(impulseVector.y) )
            {
                return;
            }
            var position = GameObject.Transform.Position + impulseVector;
            GameObject.Transform.Position = position;
            //movementBody.AddForce(impulseVector);
        }
    }
}