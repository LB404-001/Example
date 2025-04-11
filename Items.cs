using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace Work1
{
    internal class Item
    {
        private string _name;
        private int _id;
        private BitmapImage _texture;
        public string Name
        {
            get { return _name; }
        }
        public int Id
        {
            get { return _id; }
        }

        public BitmapImage Texture
        {
            get { return _texture; }

        }

        public Item()
        {
            _id = -1;
            _name = "null_item";
            _texture = new BitmapImage(new Uri("Textures\\System\\NULL.png", UriKind.Relative));
        }

        public Item(string name, int id, BitmapImage textures)
        {
            _name = name;
            _id = id;
            _texture = textures;
        }
    }

    internal class Heal : Item, Interfaces.IUsable
    {
        int heal_value = 0;
        int current_use_count = 0;
        int max_use_count = 0;

        public int MaxUseCount
        {
            get { return max_use_count; }
            protected set { max_use_count = value; }
        }

        public int CurrentUseCount
        {
            get { return current_use_count; }
            protected set { current_use_count = value; }
        }

        public void Use(Entity user, Point target)
        {
            user.HP += heal_value;
            if (user.HP > user.MaxHP)
            {
                user.HP = user.MaxHP;
            }
            current_use_count--;
            if (current_use_count <= 0)
            {
                ((Player)user).Inventory.Remove(this);
            }
        }

        public Heal(string name, int id, BitmapImage texture, int healValue, int MaxUseCount) : base (name, id, texture)
        {
            heal_value = healValue;
            max_use_count = MaxUseCount;
        }
    }

    internal class Items
    {
        public static Heal Beer = new Heal("Beer", 11, new BitmapImage(), 3, 1);
    }
}
