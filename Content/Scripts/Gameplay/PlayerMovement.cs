
using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;

namespace Shooter.Content.Scripts.Gameplay
{
    public class PlayerMovement: BehaviourComponent
    {
        [SerializedField] private float moveSpeed;
        [SerializedField] private float detectUpSpeed;
        [SerializedField] private float jumpImpulse;


        private Rigidbody _rigidbody;
        public void SetRotation(float zRot)
        {
            var setVector = new Vector3(0, 0, zRot);
            var setRot = Quaternion.FromEuler(setVector);
            GameObject.Transform.Rotation = setRot;
        }
        
        public override void Start()
        {
            _rigidbody = GameObject.GetComponent<Rigidbody>();
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
            var jumpPressed = Input.IsKeyDown(Key.Space);
    
            var movForward = 0f + (forward ? 1f : 0f) + (back ? -1f: 0f); 
            var movSides = 0f + (left ? -1f : 0f) + (right ? 1f: 0f);
            var modifySpeed = (Input.IsKeyDown(Key.LeftShift) ? 2f : 1f);
            var movVector = new Vector2f(movSides, movForward);
            
            ProcessMovement(movVector, modifySpeed, jumpPressed);
            //
        }

        private void ProcessMovement(Vector2f movement, float speedModify = 1f, bool jump = false)
        {
            //Logger.Log(LogType.Info, $"RigidSpeed :({_rigidbody.Velocity})");
            var normalizedMov = movement.normalized();
            if (double.IsNaN(normalizedMov.x) || double.IsNaN(normalizedMov.y) )
            {
                return;
            }
            var realVector = new Vector3();
            realVector += GameObject.Transform.Forward * movement.y * (float)Time.DeltaTime * moveSpeed * speedModify;
            realVector += GameObject.Transform.Right * movement.x * (float)Time.DeltaTime * moveSpeed * speedModify;
            //GameObject.Transform.Position = 
            //var direction = cameraTarget.Transform.Forward;
            
            // = new Vector3(movement.x, movement.y, 0).normalized().projectOnFlat(Vector3.Up);
            //var impulseVector = new Vector3(movement.x, movement.y, 0).normalized() ;
            
            var position = GameObject.Transform.Position + realVector;
            GameObject.Transform.Position = position;
            var upVelocity = _rigidbody.Velocity.z;
            if (upVelocity <= detectUpSpeed && jump)
            {
                //_rigidbody.AddImpulse(Vector3.Up * jumpImpulse); 
            }
            //movementBody.AddForce(impulseVector);
        }
    }
}