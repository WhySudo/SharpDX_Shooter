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
        [SerializedField] private float spawnRadius;
        [SerializedField] private float spawnHeight;

        private List<Enemy> _enemies = new List<Enemy>();

        public event Action OnEnemyDeathEvent;
        public void OnEnemyDeath(Enemy instance)
        {
            _enemies.Remove(instance);
            SpawnEnemyAtRandom();
            OnEnemyDeathEvent?.Invoke();
        }
        
        private Vector3 GetSpawnPoint()
        {
            var setSpawnRadius = Random.Shared.NextDouble() * spawnRadius;
            var x = Random.Shared.NextDouble();
            var y = Math.Sqrt(1 - (x * x));
            x *= setSpawnRadius;
            y *= setSpawnRadius;
            var dirRandom = Random.Shared.Next(0, 4);
            var xDirection = dirRandom % 2 == 1 ? -1f : 1f;
            var yDirection = dirRandom / 2 == 1 ? -1f : 1f;
            var result = new Vector3(xDirection * x, yDirection * y, spawnHeight);
            return result;
        }

        private void SpawnEnemyAtRandom()
        {
            var point = GetSpawnPoint();
            Logger.Log(LogType.Info, $"NextSpawn at: {point}");
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
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                var spawnPoint = spawnPoints[i];
                SpawnEnemy(spawnPoint.Position);
            }
        }
    }
}