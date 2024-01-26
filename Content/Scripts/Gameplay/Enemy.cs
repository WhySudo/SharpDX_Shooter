using System;
using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts.Gameplay
{

    public class Enemy : BehaviourComponent
    {
        [SerializedField] private int MaxHealth;

        public event Action OnHit;
        public event Action OnDeath;
        private int _currentHealth;

        private EnemySpawner _spawner;
        public void SetSpawner(EnemySpawner spawner)
        {
            _spawner = spawner;
        }
        public override void Start()
        {
            _currentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnHit?.Invoke();
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
                _spawner.OnEnemyDeath(this);
                GameObject.Destroy();
            }
        }
    }
}