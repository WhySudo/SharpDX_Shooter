using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts;

public class KillsCounter: BehaviourComponent
{
    [SerializedField] private GameObject spawner;
}