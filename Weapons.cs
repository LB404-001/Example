using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Work1
{
    internal abstract class Weapon : Item, Interfaces.IWeapon
    {
        protected int _MeleeDamage = 1;

        public abstract void Use(Point target, Point Orientation);

        public Weapon(string name, int id, int damage, BitmapImage texture) : base(name, id, texture)
        {
            _MeleeDamage = damage;
        }
    }

    internal abstract class DistanceWeapon : Weapon
    {
        protected double _range;
        protected int _DistanceDamage = 1;

        public DistanceWeapon(string name, int id, int damage, double range, BitmapImage texture) : base(name, id, 0, texture)
        {
            _DistanceDamage = damage;
            _range = range;
        }
    }

    internal class ClipWeapon : DistanceWeapon
    {
        private int _maxAmmoCount;
        private int _currentAmmoCount;

        public override void Use(Point Position, Point Orientation)
        {
            if (_currentAmmoCount > 0)
            {
                if (Math.Sqrt(Math.Pow(Convert.ToDouble(Orientation.X), 2) + Math.Pow(Convert.ToDouble(Orientation.Y), 2)) > _range)
                {
                    double der = Convert.ToDouble(Orientation.Y) / Convert.ToDouble(Orientation.X);
                    double x;
                    double y;
                    if (Orientation.X == 0)
                    {
                        x = 0;
                        y = _range;
                    }
                    else
                    {
                        if (Orientation.Y == 0)
                        {
                            x = _range;
                            y = 0;
                        }
                        else
                        {
                            x = Math.Sqrt(Math.Pow(_range, 2) / (Math.Pow(der, 2) + 1));
                            y = x * der;
                        }
                    }
                    Orientation = new Point(Convert.ToInt32(x), Convert.ToInt32(y));
                }
                Chunk targetChunk = Engine.Raycast(Position, new Point(Position.X + Orientation.X, Position.Y + Orientation.Y));
                if (targetChunk != null)
                {
                    if (targetChunk.Entity != null)
                    {
                        Engine.Damage(targetChunk.Entity.Position.X, targetChunk.Entity.Position.Y, _DistanceDamage);
                    }
                }
                _currentAmmoCount -= 1;
            }
            else
            {

            }
        }

        public ClipWeapon(string name, int id, int damage, double range, int maxAmmoCount, BitmapImage texture) : base(name, id, damage, range, texture)
        {
            _maxAmmoCount = maxAmmoCount;
            _currentAmmoCount = maxAmmoCount;
        }
    }

    internal class MeleeWeapon : Weapon
    {
        public override void Use(Point Position, Point Orientation)
        {
            int x = Position.X + Orientation.X;
            int y = Position.Y + Orientation.Y;
            Engine.Damage(x, y, _MeleeDamage);
        }

        public MeleeWeapon(string name, int id, int damage, BitmapImage texture) : base(name, id, damage, texture)
        {

        }
    }

    internal class Weapons
    {
        public static MeleeWeapon Hand = new MeleeWeapon("Hand", 0, 1, new BitmapImage(new Uri("Textures\\Items\\Weapons\\Hand.png", UriKind.Relative)));
        public static MeleeWeapon Knife = new MeleeWeapon("Knife", 1, 5, new BitmapImage(new Uri("Textures\\Items\\Weapons\\Knife.png", UriKind.Relative)));
        public static ClipWeapon Pistol = new ClipWeapon("Pistol", 2, 15, 5, 5, new BitmapImage(new Uri("Textures\\Items\\Weapons\\Pistol.png", UriKind.Relative)));
    }
}
