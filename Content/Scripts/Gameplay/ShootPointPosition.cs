using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts.Gameplay
{

    public class ShootPointPosition : BehaviourComponent
    {
        [SerializedField] private GameObject target;
        [SerializedField] private float targetDistance;

        public override void Update()
        {
            base.Update();
            GameObject.Transform.Position = target.Transform.Position + target.Transform.Forward * targetDistance;
            GameObject.Transform.Rotation = target.Transform.Rotation;
        }
    }
}