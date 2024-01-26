using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts.Gameplay;

public class EnemySounds: BehaviourComponent
{
    [SerializedField] private Sound HitSound;
    [SerializedField] private Sound DeathSound;
    [SerializedField] private GameObject enemyObject;
    public override void Start()
    {
        base.Start();
    }
}