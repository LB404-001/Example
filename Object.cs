using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Work1
{
    internal abstract class Object
    {
        private BitmapImage _texture;
        private string _name;

        public BitmapImage Texture
        {
            get { return _texture; }
            protected set { _texture = value; }
        }

        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }


        public Object(string name, BitmapImage texture)
        {
            _texture = texture;
            _name = name;
        }
    }

    internal class Chest : Object, Interfaces.IInteractable
    {
        private Inventory inventory;

        public Inventory Inventory
        {
            get { return inventory; }
        }

        public void Interaction(Entity Requesting)
        {
            if (Requesting.GetType().GetInterface(typeof(Interfaces.IHasInventory).Name) != null)
            {
                InventoryWindow inventoryWindow = new InventoryWindow(((Interfaces.IHasInventory)Requesting).Inventory, this.Inventory);
                //Uri a = new Uri(("\\Textures\\Objects\\Furniture\\Chest_open.png"), UriKind.Relative);
                this.Texture = new BitmapImage(new Uri("Textures\\Objects\\Furniture\\Chest_open.png", UriKind.Relative));
                inventoryWindow.ShowDialog();
                this.Texture = new BitmapImage(new Uri("Textures\\Objects\\Furniture\\Chest.png", UriKind.Relative));
            }
        }
        public Chest(string name, BitmapImage texture) : base (name, texture)
        {
            this.inventory = new Inventory();
        }
    }

    internal class Ground : Object
    {
        public Ground(string name, BitmapImage texture) : base(name, texture)
        {

        }
    }

    internal class StaticObject : Object
    {
        public StaticObject(string name, BitmapImage texture) : base(name, texture)
        {

        }
    }

    internal class Trap : Object, Interfaces.ITriggerable
    {
        private int _damage;

        public int Damage
        {
            get { return _damage; }
            protected set { _damage = value; }
        }

        public void Trigger(Entity entity)
        {
            if (entity.GetType().GetInterface(typeof(Interfaces.IHitable).Name) != null)
            {
                var t = (Interfaces.IHitable)entity;
                t.onHit(_damage);
            }
        }
        public Trap(string name, BitmapImage texture, int damage) : base(name, texture)
        {
            _damage = damage;
        }
    }

    internal class Void : Object
    {
        public Void() : base("void", new BitmapImage(new Uri(Defaults.VoidTexturePath)))
        {

        }
    }
}
