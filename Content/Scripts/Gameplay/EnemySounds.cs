using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts.Gameplay;

public class EnemySounds: BehaviourComponent
{
    [SerializedField] private Sound HitSound;
    [SerializedField] private Sound DeathSound;
    private Enemy _enemy;
    private SoundSource _soundSource;
    public override void Start()
    {
        base.Start();
        _enemy = GameObject.GetComponent<Enemy>();
        _soundSource = GameObject.GetComponent<SoundSource>();
        _enemy.OnDeath += OnDeath;
        _enemy.OnHit += OnHit;
    }

    private void OnHit()
    {
        _soundSource.Play(HitSound);
    }

    private void OnDeath()
    {
        _soundSource.Play(DeathSound);
    }

    protected override void OnDestroy()
    {
        _enemy.OnDeath -= OnDeath;
        _enemy.OnHit -= OnHit;
    }
}