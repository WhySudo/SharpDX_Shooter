using System;
using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts.Gameplay.Weapon
{
    public class Weapon : BehaviourComponent
    {
        [SerializedField] private float reloadRate;

        public event Action OnWeaponShoot;

        private GameObject _shootPoint;

        private bool _canShoot = true;
        private float _shootTimer = 0f;
        public override void Start()
        {
            base.Start();
            _shootPoint = GameObject.Transform.Children[0].GameObject;
        }

        public override void Update()
        {
            base.Update();
            TimerUpdate();
            CheckForShootInput();
        }

        private void CheckForShootInput()
        {
            var shootPressed = Input.IsMouseButtonDown(MouseButton.Left) || Input.IsKeyDown(Key.LeftCtrl);
            if (shootPressed && _canShoot)
            {
                Shoot();
            }
        }
        public void Shoot()
        {
            ProcessShoot();
            OnWeaponShoot?.Invoke();
            _canShoot = false;
            _shootTimer = reloadRate;
        }

        private void TimerUpdate()
        {
            if(_canShoot)return;
            _shootTimer -= (float) Time.DeltaTime;
            if (_shootTimer <= 0f)
            {
                _shootTimer = 0f;
                _canShoot = true;
            }
        }

        private void ProcessShoot()
        {
            var shootDirection = GameObject.Transform.Up;
            var shootRay = new Ray(_shootPoint.Transform.Position, shootDirection);
            if (Raycast.HitMesh(shootRay, out var result))
            {
            }
        }
    }
}