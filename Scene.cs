using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;

namespace Work1
{
    internal class Scene
    {
        private Chunk[][] _world;
        private List<Entity> _entities;
        private List<Object> _staticObjects;
        private List<Effect> _effects = new List<Effect>(0);

        public List<Effect> Effects
        {
            get { return _effects; }
            //set { _effects = value; }
        }
        public Chunk[][] World
        {
            get { return _world; }
            private set { _world = value; }
        }
        public List<Entity> Entities
        {
            get { return _entities; }
            private set { _entities = value; }
        }
        public List<Object> StaticObjects
        {
            get { return _staticObjects; }
            private set { _staticObjects = value; }
        }

        public Scene(int x, int y)
        {
            _world = new Chunk[x][];
            for (int i = 0; i < x; i++)
            {
                _world[i] = new Chunk[y];
                for (int j = 0; j < y; j++)
                {
                    Point position = new Point(i, j);
                    BitmapImage texture = new BitmapImage();
                    texture.BeginInit();
                    texture.UriSource = new Uri(Defaults.GroundTexture, UriKind.Relative);
                    texture.EndInit();
                    _world[i][j] = new Chunk(new Ground($"ground_{i*y+j}", texture), null!, null!, false, new Point(i, j));

                }
            }
            _entities = new List<Entity>(0);
            Player player = new Player("player", new Point(1, 1));
            player.Inventory.Add(Items.Beer);
            player.Inventory.Add(Weapons.Knife);
            player.Inventory.Add(Weapons.Rifle);
            _entities.Add(player);
            

            for (int i = 0; i < x; i++)
            {
                _world[0][i].Collision = true;
                _world[y-1][i].Collision = true;

                BitmapImage texture = new BitmapImage(new Uri(("Textures\\Objects\\Buildings\\Walls\\CBW.png"), UriKind.Relative));
                StaticObject wall = new StaticObject($"wall_{0}:{i}", texture);
                _world[0][i].Object = wall;
                wall = new StaticObject($"wall_{y-1}:{i}", texture);
                _world[y-1][i].Object = wall;
            }
            for (int i = 0; i < y; i++)
            {
                _world[i][0].Collision = true;
                _world[i][x - 1].Collision = true;

                BitmapImage texture = new BitmapImage(new Uri(("Textures\\Objects\\Buildings\\Walls\\CBW.png"), UriKind.Relative));
                StaticObject wall = new StaticObject($"wall_{i}:{0}", texture);
                _world[i][0].Object = wall;
                wall = new StaticObject($"wall_{i}:{x - 1}", texture);
                _world[i][x-1].Object = wall;
            }

            _world[5][5].Object = new Trap("trap1", new BitmapImage(new Uri("Textures\\Objects\\Trigerrable\\Trap_active.png", UriKind.Relative)), 3);
            _world[3][5].Entity = new Zombie("target", 10, new BitmapImage(new Uri(("Textures\\Objects\\Buildings\\Walls\\CBW.png"), UriKind.Relative)), 100, 100, new Point(3,5));
            _world[3][4].Object = new StaticObject($"wall", new BitmapImage(new Uri(("Textures\\Objects\\Buildings\\Walls\\CBW.png"), UriKind.Relative)));
            _world[1][2].Object = new StaticObject($"wall", new BitmapImage(new Uri(("Textures\\Objects\\Buildings\\Walls\\CBW.png"), UriKind.Relative)));
            _world[3][4].Collision = true;
            _entities.Add(_world[3][5].Entity);

            _world[6][7].Object = Objects.Chest();//new Chest("testchest", new BitmapImage(new Uri("F:\\Projects\\Work1\\Textures\\Objects\\Furniture\\Chest.png")));
            _world[6][7].Collision = true;


        }
    }
}
