using System;
using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;
using Shooter.Content.Scripts.UI;

namespace Shooter.Content.Scripts.Gameplay.Weapon
{
    public class Weapon : BehaviourComponent
    {
        [SerializedField] private float reloadRate;
        [SerializedField] private int damage;
        [SerializedField] private GameObject UiController;
        [SerializedField] private GameObject _shootPoint;
        public event Action OnWeaponShoot;

        private UIAimController _aim;

        private bool _canShoot = true;
        private float _shootTimer = 0f;
        public override void Start()
        {
            base.Start();
            _aim = UiController.GetComponent<UIAimController>();
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
            if (shootPressed)
            {
                Input.CursorState = CursorState.HiddenAndLocked;
            }
            if (shootPressed && _canShoot)
            {
                Shoot();
            }
            if (Input.IsKeyDown(Key.Escape))
            {
                Input.CursorState = CursorState.Default;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();Input.CursorState = CursorState.Default;
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
            var cam = Camera.Current;
            //var aimPoint = Quaternion.Identity;
            var aimPoint = _aim.GetWeaponModifier();
            //
            var shootDirection = aimPoint * GameObject.Transform.Up;
            var shootRay = new Ray(_shootPoint.Transform.Position, shootDirection);
            if (Raycast.HitMesh(shootRay, out var result))
            {
                var enemyHit = result.HitObject.GetComponent<Enemy>();
                if (enemyHit != null)
                {
                    enemyHit.TakeDamage(damage);
                    Logger.Log(LogType.Info, "Enemy shoot!");
                }
            }
        }
    }
}