using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Work1
{
    public struct Transform
    {
        double angle;

    }
    internal abstract class Effect
    {
        protected double _x;
        protected double _y;
        protected double _angle;
        protected Rectangle _view;
        protected List<BitmapImage> _images;

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        public Rectangle View
        {
            get { return _view; }
            protected set { _view = value; }
        }

        public List<BitmapImage> Images
        {
            get { return _images; }
            protected set { _images = value; }
        }

        public abstract void Play();

        public Effect(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    internal class Fire : Effect
    {
        public override async void Play()
        {
            Engine._current_scene.Effects.Add(this);
            foreach (BitmapImage image in _images)
            {
                View = new Rectangle
                {
                    Height = image.Height * Settings.zoom,
                    Width = image.Width * Settings.zoom,
                    Fill = new ImageBrush(image),
                    RenderTransform = new RotateTransform(Angle, image.Width * Settings.zoom / 2, image.Height * Settings.zoom / 2)
                };
                await Task.Delay(200);
            }
            Engine._current_scene.Effects.Remove(this);
        }

        public Fire() : base (-0.5,-0.5)
        {
            Images = new List<BitmapImage>();
            Images.Add(new BitmapImage(new Uri("Effects\\Fire\\fire.png", UriKind.Relative)));
        }
    }

    internal class Hit : Effect
    {
        public override async void Play()
        {
            Engine._current_scene.Effects.Add(this);
            foreach (BitmapImage image in _images)
            {
                View = new Rectangle
                {
                    Height = image.Height * Settings.zoom,
                    Width = image.Width * Settings.zoom,
                    Fill = new ImageBrush(image),
                    RenderTransform = new RotateTransform(Angle, image.Width * Settings.zoom / 2, image.Height * Settings.zoom / 2)
                };
                await Task.Delay(100);
            }
            Engine._current_scene.Effects.Remove(this);
        }
        public Hit() : base(-0.5,-0.5)
        {
            Images = new List<BitmapImage>();
            Images.Add(new BitmapImage(new Uri("Effects\\Hit\\1.png", UriKind.Relative)));
            Images.Add(new BitmapImage(new Uri("Effects\\Hit\\2.png", UriKind.Relative)));
            Images.Add(new BitmapImage(new Uri("Effects\\Hit\\3.png", UriKind.Relative)));
            //Images.Add(new BitmapImage(new Uri("Effects\\Hit\\4.png", UriKind.Relative)));
            //Images.Add(new BitmapImage(new Uri("Effects\\Hit\\5.png", UriKind.Relative)));
        }
    }

    internal class Bleed : Effect
    {
        public override async void Play()
        {
            //Images = new List<BitmapImage>();
            var a = Engine._current_scene.World[Convert.ToInt32(this.X + 0.5)][Convert.ToInt32(this.Y + 0.5)].Object;
            if (a == null)
            {
                a = Objects.Blood(0);
            }
            else
            {
                if (a.GetType() == typeof(Blood))
                {
                    a = Objects.Blood(((Blood)a).Level + 1);
                }
            }
            //Images.Add(a.Texture);
            Engine._current_scene.World[Convert.ToInt32(this.X + 0.5)][Convert.ToInt32(this.Y + 0.5)].Object = a;
            Engine._current_scene.Effects.Add(this);
            foreach (BitmapImage image in _images)
            {
                View = new Rectangle
                {
                    Height = image.Height * Settings.zoom,
                    Width = image.Width * Settings.zoom,
                    Fill = new ImageBrush(image),
                    RenderTransform = new RotateTransform(Angle, image.Width * Settings.zoom / 2, image.Height * Settings.zoom / 2)
                };
                await Task.Delay(50);
            }
            Engine._current_scene.Effects.Remove(this);
        }
        public Bleed() : base(-0.5,-0.5)
        {
            Images = new List<BitmapImage>();
            Images.Add(new BitmapImage(new Uri("Effects\\Bleed\\0.png", UriKind.Relative)));
            Images.Add(new BitmapImage(new Uri("Effects\\Bleed\\1.png", UriKind.Relative)));
            Images.Add(new BitmapImage(new Uri("Effects\\Bleed\\2.png", UriKind.Relative)));
        }
    }

    internal class Effects
    {
        static public Fire Fire()
        {
            return new Fire();
        }
        static public Hit Hit()
        {
            return new Hit();
        }

        static public Bleed Bleed()
        {
            return new Bleed();
        }
    }
}
