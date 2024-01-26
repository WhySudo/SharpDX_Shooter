using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts.Gameplay.Weapon
{

    public class WeaponSounds: BehaviourComponent
    {
        [SerializedField] private Sound shootSound;
        private Weapon _source;
        private SoundSource _soundSource;

        public override void Start()
        {
            _source = GameObject.GetComponent<Weapon>();
            _soundSource = GameObject.GetComponent<SoundSource>();
            _source.OnWeaponShoot += OnShoot;
        }

        private void OnShoot()
        {
            _soundSource.play(shootSound);
        }

        protected override void OnDestroy()
        {
            _source.OnWeaponShoot -= OnShoot;
        }
    }
}