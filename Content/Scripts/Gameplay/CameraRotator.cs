using System;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;

namespace Shooter.Content.Scripts.Gameplay
{
    public class CameraRotator : BehaviourComponent
    {
        [SerializedField] private float rotationValue;


        private PlayerMovement _target;
        public override void Start()
        {
            _target = GameObject.Transform.Parent.GameObject.GetComponent<PlayerMovement>();
            base.Start();
        }

        public override void Update()
        {
            base.Update();
            CheckInput();
        }

        private void CheckInput()
        {
            var mouseDelta = -Input.GetMouseDelta();
            var globalRotation = _target.GameObject.Transform.Rotation.ToEuler();
            var oldLocalRotation = GameObject.Transform.LocalRotation.ToEuler().x;
            globalRotation.z += rotationValue * mouseDelta.x;
            if (_target != null)
            {
                _target.SetRotation((float)globalRotation.z);
            }
            oldLocalRotation += rotationValue * mouseDelta.y;
            oldLocalRotation = MathF.Min((float)oldLocalRotation, 3.14159f/2f); // Limit vertical rotation
            oldLocalRotation = MathF.Max((float)oldLocalRotation, -3.14159f/2f); // Limit vertical rotation
            GameObject.Transform.LocalRotation = Quaternion.FromAxisAngle(Vector3.Right, oldLocalRotation);
            
        }
    }
}