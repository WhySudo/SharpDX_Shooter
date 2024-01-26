
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        [SerializedField] private float aproxDelta;
        private WPF_Form _drawnWpfForm;

        private string drawText = "";

        private float aimX = 0f;
        private float aimY = 0f;

        private float width;
        private float height;

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

        public void SetKills(int count)
        {
            DrawText = $"Kills: {count}";
        }
        public Vector2 GetAimPixelPoint()
        {
            var pixelY = aimY + (height/2);
            var pixelX = aimX + (width/2);
            return new Vector2(pixelX,pixelY);
        }

        private Vector2 GetMainScreenPoint()
        {
            return new Vector2((width/2),(height/2));
        }

        private void UpdateScreenSize()
        {
            GraphicsCore.ViewportPanel.Dispatcher.Invoke(() =>
            {
                width = (float)(GraphicsCore.ViewportPanel.ActualWidth / 2);
                height = (float)(GraphicsCore.ViewportPanel.ActualHeight / 2);
                //Logger.Log(LogType.Info,$"ScreenSize: {GraphicsCore.ViewportPanel.ActualWidth}, {GraphicsCore.ViewportPanel.ActualHeight}");
            });
        }

        public Vector3 GetModifiedForward()
        {
            var aimPoint = GetAimPixelPoint();
            var camera = Camera.Current;
            var camForward = camera.GameObject.Transform.Forward * vectorModifier;
            var modificationX = camera.GameObject.Transform.Right * aimX / radius;
            var modificationY = camera.GameObject.Transform.Up * aimY / radius;
            var result = camForward + modificationX + modificationY;
            
            // var cameraPos = camera.GameObject.Transform.Position;
            // var aimWorld = cameraPos - camera.ScreenToWorld(aimPoint);
            // var dotProduct = aimWorld.dot( camera.GameObject.Transform.Forward);
            // var angle =  Math.Acos(dotProduct);
            // Logger.Log(LogType.Info,$" Angle: {dotProduct}");

            //return result.normalized();
            return camera.GameObject.Transform.Forward;
        }
        
        public Quaternion GetWeaponModifier()
        {
            UpdateScreenSize();
            var aimPoint = GetAimPixelPoint();
            var center = GetMainScreenPoint();
            var camera = Camera.Current;
            var cameraPos = camera.GameObject.Transform.Position;
            var aimWorld = camera.ScreenToWorld(aimPoint) - cameraPos;
            var centerWorld = camera.ScreenToWorld(center) - cameraPos;
            var deltaVec = aimWorld - centerWorld;
            //Logger.Log(LogType.Info,$"From: ({deltaVec}), aim: {aimPoint - center}");
            var rotation = FromToRotation(centerWorld, aimWorld);
            return rotation;
        }
        private Quaternion FromToRotation(Vector3 aFrom, Vector3 aTo)
        {
            aFrom.normalize();
            aTo.normalize();
            var cross = aFrom.cross(aTo);
            var dotProduct = aFrom.dot(aTo);
            var angle =  Math.Acos(dotProduct);
            
            //Logger.Log(LogType.Info,$"Angle: ({angle}); Cross: {cross.x}, {cross.y}, {cross.z}");
            if (Math.Abs(dotProduct - 1f) <= aproxDelta)
            {
                return Quaternion.Identity;
            }
            // else if (Math.Abs(dotProduct + 1f) <= aproxDelta) // If they are nearly antiparallel, create a 180-degree rotation
            // {
            //     Vector3 axis = Vector3.Cross(Vector3.forward, from);
            //     if (axis == Vector3.zero) axis = Vector3.Cross(Vector3.up, from); // Handle special case when from vector is parallel to up vector
            //     axis.Normalize();
            //     return Quaternion.AngleAxis(180.0f, axis);
            // }
            return Quaternion.FromAxisAngle( cross.normalized(), angle);
            //aFrom.
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
                width = (float)_drawnWpfForm.Width;
                height = (float)_drawnWpfForm.Height;
            });
            
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
                var curPos = setPos;
                var targetPos = curPos + moveDirection * addMovement;
                if (targetPos.magnitude() > radius)
                {
                    var deltaRadius = targetPos.magnitude() - radius;
                    setPos = curPos + moveDirection * (addMovement - deltaRadius);
                    addMovement = deltaRadius;
                    moveDirection = GetRandomDirection();
                    continue;
                }
                setPos = targetPos;
                break;
            } while (addMovement > 0f);

            AimX = setPos.x;
            AimY = setPos.y;
            var cam = Camera.Current;
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
