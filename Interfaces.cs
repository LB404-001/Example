using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Work1
{
    internal class Interfaces
    {
        internal interface IItem
        {
            string Name { get; }
            int Id { get; }
        }

        internal interface IEntity
        {
            string Name { get; }
            string Id { get; }
        }

        internal interface IUsable
        {
            void Use(Entity user, Point target);
        }

        internal interface IInteractable
        {
            void Interaction(Entity Requesting);
        }

        internal interface IHasInventory
        {
            Inventory Inventory { get; }
        }

        internal interface ITriggerable
        {
            void Trigger(Entity entity);
        }

        internal interface IHitable
        {
            void onHit(int damage);
        }

        internal interface IDeath
        {
            void Death();
        }

        internal interface IWeapon
        {

        }

        internal interface IHasOrientatedTexture
        {
            public Point Orientation
            {
                get;
            }
            public string TexturePath
            {
                get;
            }
            public BitmapImage Texture
            {
                get;
                protected set;
            }
            public void OrientateTexture()
            {
                
                try
                {
                    string texturePath;// = $"{TexturePath}\\East.png";

                    switch (Orientation)
                    {
                        case var value when value == Orientations.North:
                            texturePath = $"{TexturePath}\\North.png";
                            Texture = new BitmapImage(new Uri(texturePath, UriKind.Relative));
                            break;
                        case var value when value == Orientations.South:
                            texturePath = $"{TexturePath}\\South.png";
                            Texture = new BitmapImage(new Uri(texturePath, UriKind.Relative));
                            break;
                        case var value when value == Orientations.West:
                            texturePath = $"{TexturePath}\\West.png";
                            Texture = new BitmapImage(new Uri(texturePath, UriKind.Relative));
                            break;
                        case var value when value == Orientations.East:
                            texturePath = $"{TexturePath}\\East.png";
                            Texture = new BitmapImage(new Uri(texturePath, UriKind.Relative));
                            break;
                    }
                }
                catch
                {

                }
            }
        }
    }
}
