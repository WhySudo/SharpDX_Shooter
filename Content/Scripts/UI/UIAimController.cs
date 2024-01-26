
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;
using Shooter.Content.WPF_UI;

namespace Shooter.Content.Scripts.UI
{
    public class UIAimController: BehaviourComponent, INotifyPropertyChanged
    {
        [SerializedField] private float moveSpeed;
        [SerializedField] private float radius;
        [SerializedField] private float vectorModifier;
        private WPF_Form _drawnWpfForm;

        private string drawText = "";

        private float aimX = 0f;
        private float aimY = 0f;

        private Vector2f currentPoint = new Vector2f();
        private Vector2f moveDirection = new Vector2f();
        public string DrawText
        {
            get => drawText;
            set
            {
                drawText = value;
                OnPropertyChanged();
            }
        }

        public float AimX
        {
            get => aimX;
            set
            {
                aimX = value;
                OnPropertyChanged();
            }
        }
        public float AimY
        {
            get => aimY;
            set
            {
                aimY = value;
                OnPropertyChanged();
            }
        }

        public Quaternion GetWeaponModifier()
        {
            return Quaternion.Identity;
        }
        
        public override void Start()
        {
            
            base.Start();
            moveDirection = GetRandomDirection();
            GraphicsCore.ViewportPanel.Dispatcher.Invoke(() =>
            {
                _drawnWpfForm = new WPF_Form();
                GraphicsCore.ViewportPanel.Children.Add(_drawnWpfForm);
                _drawnWpfForm.DataContext = this;
            });
            DrawText = "Merhaba";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            
            
            return true;
        }

        public override void Update()
        {
            //DrawText = $"{1f / Time.DeltaTime}";
            ProcessMovement();
        }


        private void ProcessMovement()
        {
            var currentPos = new Vector2f(AimX, AimY);
            var addMovement =(float) Time.DeltaTime * moveSpeed;
            var setPos = currentPos;
            do
            {
                var curPos = new Vector2f(AimX, AimY);
                var targetPos = curPos + moveDirection * addMovement;
                if (targetPos.magnitude() > radius)
                {
                    var deltaRadius = targetPos.magnitude() - radius;
                    targetPos = curPos + moveDirection * (addMovement - deltaRadius);
                    addMovement = deltaRadius;
                    moveDirection = GetRandomDirection();
                    continue;
                }
                setPos = targetPos;
                break;
            } while (addMovement > 0f);

            AimX = setPos.x;
            AimY = setPos.y;
        }

        private Vector2f GetRandomDirection()
        {
            var xValue = (float)Random.Shared.NextDouble();
            var yValue = MathF.Sqrt((float) (1f - (xValue*xValue)));
            var dirRandom = Random.Shared.Next(0, 4);
            var xDirection = dirRandom % 2 == 1 ? -1f : 1f;
            var yDirection = dirRandom / 2 == 1 ? -1f : 1f;
            var result = new Vector2f(xDirection * xValue, yDirection * yValue);
            return result;
        }
    }
}
