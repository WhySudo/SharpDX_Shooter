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
        [SerializedField] private GameObject playerAvatar;
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
            var realForward = _aim.GetModifiedForward();
            var camRay = new Ray(cam.GameObject.Transform.Position, realForward);
            var setPointToFocus = new Vector3();
            // if (Raycast.HitMesh(camRay, out var camResult))
            // {
            //     setPointToFocus = camResult.HitPoint;
            // }
            // else
            // {
            //     Logger.Log(LogType.Info, "Camera hasn't shoot anything");
            //     return;
            // }

            var direction = setPointToFocus - _shootPoint.Transform.Position;
            //
            //var shootDirection = aimPoint * _shootPoint.Transform.Up;
            var shootDirection = aimPoint*direction.normalized();
            
            //Logger.Log(LogType.Info, $"shootdir : {realForward}");
            //var shootRay = new Ray(_shootPoint.Transform.Position, shootDirection);
            if (Raycast.HitMesh(camRay, out var camResult))
            {
                //Logger.Log(LogType.Info,camResult.HitObject.Name);
                var enemyHit = camResult.HitObject.GetComponent<Enemy>();
                if (enemyHit != null)
                {
                    enemyHit.TakeDamage(damage);
                    Logger.Log(LogType.Info, "Enemy shoot!");
                }

                var selfHit = camResult.HitObject.GetComponent<PlayerMovement>();
                if (selfHit != null)
                {
                    var dist = cam.GameObject.Transform.Position - camResult.HitPoint;
                    //Logger.Log(LogType.Info, $"Self shoot Distance: {dist.magnitude()}!");
                }
            }
        }
    }
}