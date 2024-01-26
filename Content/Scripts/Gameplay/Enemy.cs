using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts.Gameplay
{

    public class Enemy : BehaviourComponent
    {
        [SerializedField] private int MaxHealth;

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
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (_currentHealth <= 0)
            {
                _spawner.OnEnemyDeath(this);
                GameObject.Destroy();
            }
        }
    }
}