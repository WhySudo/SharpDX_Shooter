using System;
using System.Collections.Generic;
using Engine;
using Engine.Assets;
using Engine.BaseAssets.Components;
using LinearAlgebra;

namespace Shooter.Content.Scripts.Gameplay
{
    public class EnemySpawner: BehaviourComponent
    {
        [SerializedField] private Prefab enemyPrefab;
        [SerializedField] private int enemysCount;

        private List<Enemy> _enemies = new List<Enemy>();

        public void OnEnemyDeath(Enemy instance)
        {
            _enemies.Remove(instance);
            SpawnEnemy();
        }
        
        private Vector3 GetSpawnPoint()
        {
            var spawnPoints = GameObject.Transform.Children;
            var count = spawnPoints.Count;
            var spawnIndex = Random.Shared.Next(0, count);
            return spawnPoints[spawnIndex].Position;
        }

        private void SpawnEnemy()
        {
            var point = GetSpawnPoint();
            var instance = enemyPrefab.Instantiate();
            instance.Transform.Position = point;
            var enemy = instance.GetComponent<Enemy>();
            enemy.SetSpawner(this);
            _enemies.Add(enemy);
        }

        public override void Start()
        {
            base.Start();
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < enemysCount; i++)
            {
                SpawnEnemy();
            }
        }
    }
}