﻿using System;
using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
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
            var cam = Camera.Current;
            var aimPoint = _aim.GetWeaponModifier();
            Logger.Log(LogType.Info, $"Aim Modification: {aimPoint.ToEuler()}");
            var shootDirection = aimPoint * GameObject.Transform.Forward;
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