using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work1
{
    internal class Inventory
    {
        List<Item> items;

        public List<Item> Items
        {
            get { return items; }

        }

        public Item Get(int sourcePosition)
        {
            if (sourcePosition >= items.Count) throw new ArgumentOutOfRangeException("out of inventory range");

            return items[sourcePosition];
        }

        public virtual void Add(Item item)
        {
            items.Add(item);
        }

        public virtual void Remove(Item item)
        {
            items.Remove(item);
        }

        public void Remove(int idx)
        {
            if (idx < items.Count)
            {
                items.RemoveAt(idx);
            }
        }

        public Inventory()
        {
            items = new List<Item>(0);
            items.Add(new Item("test", 1, new System.Windows.Media.Imaging.BitmapImage(new Uri("Textures\\System\\NULL.png", UriKind.Relative))));
        }
    }

    internal class Equipment : Inventory
    {
        public Weapon weapon;

        public override void Remove(Item item)
        {
            if (item.GetType() == typeof(Weapon))
            {
                Items.Remove(item);
                weapon = null!;
            }
        }

        public override void Add(Item item)
        {
            if (item.GetType() == typeof(Weapon))
            {
                Items.Add(item);
                weapon = (Weapon)item;
            }
        }

        public Equipment()
        {
            weapon = null!;
        }

    }

    internal class Cell
    {
        public int Count;
        public Item Item;

        public Cell(int count, Item item)
        {
            this.Count = count;
            this.Item = item;
        }
    }
}
