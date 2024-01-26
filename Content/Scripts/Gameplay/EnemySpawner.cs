using System;
using System.Collections.Generic;
using System.Linq;
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
            SpawnEnemyAtRandom();
        }
        
        private Vector3 GetSpawnPoint()
        {
            var spawnPoints = GameObject.Transform.Children;
            var count = spawnPoints.Count;
            var spawnIndex = Random.Shared.Next(0, count);
            return spawnPoints[spawnIndex].Position;
        }

        private void SpawnEnemyAtRandom()
        {
            var point = GetSpawnPoint();
            SpawnEnemy(point);
        }
        private void SpawnEnemy(Vector3 point)
        {
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
            var spawnPoints = GameObject.Transform.Children.ToList();
            for (int i = 0; i < enemysCount; i++)
            {
                var point = Random.Shared.Next(0, spawnPoints.Count);
                var spawnPoint = spawnPoints[point];
                SpawnEnemy(spawnPoint.Position);
                spawnPoints.Remove(spawnPoint);
            }
        }
    }
}