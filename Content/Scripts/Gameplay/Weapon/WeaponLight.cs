using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;

namespace Shooter.Content.Scripts.Gameplay.Weapon;

public class WeaponLight : BehaviourComponent
{
    [SerializedField] private GameObject targetLight;
    private bool lightEnabled = true;
    public override void Start()
    {
        base.Start();
        lightEnabled = false;
    }

    public override void Update()
    {
        base.Update();
        CheckInput();
    }

    private void CheckInput()
    {
        var press = Input.IsMouseButtonPressed(MouseButton.Right);
        if(!press) return;
        lightEnabled = !lightEnabled;
        targetLight.LocalEnabled = lightEnabled;
    }
}