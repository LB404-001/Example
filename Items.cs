using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
            _texture = new BitmapImage(new Uri("Textures\\System\\NULL.png"));
        }

        public Item(string name, int id, BitmapImage textures)
        {
            _name = name;
            _id = id;
            _texture = textures;
        }
    }
}
