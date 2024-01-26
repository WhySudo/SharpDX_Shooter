using Engine;
using Engine.BaseAssets.Components;
using Shooter.Content.Scripts.Gameplay;
using Shooter.Content.Scripts.UI;

namespace Shooter.Content.Scripts;

public class KillsCounter: BehaviourComponent
{
    [SerializedField] private GameObject spawner;
    [SerializedField] private GameObject uiObject;

    private UIAimController _aimController;
    private EnemySpawner _spawner;

    private int counter = 0;
    public override void Start()
    {
        base.Start();
        _aimController = uiObject.GetComponent<UIAimController>();
        _spawner = spawner.GetComponent<EnemySpawner>();
        counter = 0;
        _spawner.OnEnemyDeathEvent += OnEnemyDied;
        _aimController.SetKills(counter);
    }

    private void OnEnemyDied()
    {
        counter++;
        _aimController.SetKills(counter);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _spawner.OnEnemyDeathEvent -= OnEnemyDied;
    }
}