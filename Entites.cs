using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Work1
{
    internal abstract class Entity
    {
        private string _name;
        private int _id;
        private BitmapImage _texture;
        private Point _orientation;
        private int _HP;
        private Point _position;
        private int _MaxHP;
        private string _texture_path;
        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }
        public int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }
        public int HP
        {
            get { return _HP; }
            set { _HP = value; }
        }
        public int MaxHP
        {
            get { return _MaxHP; }
            protected set { _MaxHP = value; }
        }

        public string TexturePath
        {
            get { return _texture_path; }
            protected set { _texture_path = value; }
        }
        public BitmapImage Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Point Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public Entity()
        {
            _name = "null_entity";
            _id = -1;
            _texture = new BitmapImage(new Uri("F:\\Projects\\Work1\\Textures\\System\\NULL.png"));
            _orientation = Orientations.North;
            _HP = 0;
            _position = new Point(0, 0);
        }

        public Entity(string name, int id, BitmapImage texture, Point position)
        {
            _name = name;
            _id = id;
            _texture = texture;
            _orientation = Orientations.North;
            _HP = 0;
            _position = position;
        }
    }

    internal abstract class NPC : Entity, Interfaces.IHitable
    {
        public void onHit(int damage)
        {
            this.HP -= damage;
            if (this.HP <= 0)
            {
                this.Texture = new BitmapImage(new Uri(Defaults.NullTexturePath));
                Engine.KillEntity(this);
            }
        }

        public NPC(string name, int id, BitmapImage texture, int hp, Point position) : base(name, id, texture, position)
        {
            //this.Texture = new BitmapImage(new Uri("F:\\Projects\\Work1\\Textures\\Entities\\NPC\\target.png"));
            this.HP = hp;
            this.MaxHP = hp;
        }

        public NPC(string name, int id, BitmapImage texture, int hp, int maxhp, Point position) : base(name, id, texture, position)
        {
            //this.Texture = new BitmapImage(new Uri("F:\\Projects\\Work1\\Textures\\Entities\\NPC\\target.png"));
            this.HP = hp;
            this.MaxHP = maxhp;
        }
    }

    internal abstract class Aggressive : Entity, Interfaces.IHitable
    {
        public abstract void Attack();

        public abstract void Move();

        public void onHit(int damage)
        {
            this.HP -= damage;
            if (this.HP <= 0)
            {
                this.Texture = new BitmapImage(new Uri(Defaults.NullTexturePath));
                Engine.KillEntity(this);
            }
        }

        public abstract void AI();

        public Aggressive(string name, int id, BitmapImage texture, int hp, int maxhp, Point position) : base(name, id, texture, position)
        {
            this.HP = hp;
            this.MaxHP = maxhp;
        }
    }

    internal class Zombie : Aggressive, Interfaces.IHasOrientatedTexture
    {
        List<Point> orientations = new List<Point>();
        public override void Attack()
        {
            int x = Position.X + Orientation.X;
            int y = Position.Y + Orientation.Y;
            Engine.Damage(x, y, 1);
        }
        public override void Move()
        {
            Engine.MoveEntity(this, Position.X + Orientation.X, Position.Y + Orientation.Y);
        }
        public override void AI()
        {
            //((Interfaces.IHasOrientatedTexture)this).OrientateTexture();
            
            Chunk a = Engine.Raycast(this.Position, Engine.Player.Position);
            if (a != null)
            {
                if (a.Entity == Engine.Player)
                {
                    Console.WriteLine($"Entity:{this.Name} - See an Entity:{Engine.Player.Name}");
                    orientations.Clear();

                    double[][] mesh = new double[Engine._current_scene.World.Length][];
                    for (int i = 0; i < mesh.Length; i++)
                    {
                        mesh[i] = new double[Engine._current_scene.World[i].Length];
                        for (int j = 0; j < mesh[i].Length; j++)
                        {
                            mesh[i][j] = double.PositiveInfinity;
                        }
                    }
                    mesh[this.Position.X][this.Position.Y] = 0;
                    int sc = 0;
                    while (sc < 99 && mesh[Engine.Player.Position.X][Engine.Player.Position.Y] == double.PositiveInfinity)
                    {
                        for (int i = 1; i < mesh.Length - 1; i++)
                        {
                            for (int j = 1; j < mesh[i].Length - 1; j++)
                            {
                                if (mesh[i][j] != double.PositiveInfinity)
                                {
                                    if (!Engine._current_scene.World[i - 1][j].Collision && mesh[i - 1][j] == double.PositiveInfinity)
                                    {
                                        mesh[i - 1][j] = mesh[i][j] + 1;
                                    }
                                    if (!Engine._current_scene.World[i + 1][j].Collision && mesh[i + 1][j] == double.PositiveInfinity)
                                    {
                                        mesh[i + 1][j] = mesh[i][j] + 1;
                                    }
                                    if (!Engine._current_scene.World[i][j - 1].Collision && mesh[i][j - 1] == double.PositiveInfinity)
                                    {
                                        mesh[i][j - 1] = mesh[i][j] + 1;
                                    }
                                    if (!Engine._current_scene.World[i][j + 1].Collision && mesh[i][j + 1] == double.PositiveInfinity)
                                    {
                                        mesh[i][j + 1] = mesh[i][j] + 1;
                                    }
                                }
                            }
                        }

                        sc++;
                    }
                    sc = 0;
                    int x = Engine.Player.Position.X;
                    int y = Engine.Player.Position.Y;
                    while ((x != this.Position.X || y != this.Position.Y) && sc < 99)
                    {
                        double min = double.PositiveInfinity;
                        mesh[x][y] = double.PositiveInfinity;
                        Point orientation = new Point(x, y);
                        if (x != 0)
                        {
                            if (mesh[x - 1][y] < min)
                            {
                                min = mesh[x - 1][y];
                                orientation = new Point(-1, 0);
                            }
                        }
                        if (x != mesh.Length - 1)
                        {
                            if (mesh[x + 1][y] < min)
                            {
                                min = mesh[x + 1][y];
                                orientation = new Point(1, 0);
                            }
                        }
                        if (y != 0)
                        {
                            if (mesh[x][y - 1] < min)
                            {
                                min = mesh[x][y - 1];
                                orientation = new Point(0, -1);
                            }
                        }
                        if (y != mesh[0].Length - 1)
                        {
                            if (mesh[x][y + 1] < min)
                            {
                                min = mesh[x][y + 1];
                                orientation = new Point(0, 1);
                            }
                        }
                        orientations.Add(new Point(-orientation.X, -orientation.Y));
                        x += orientation.X;
                        y += orientation.Y;

                        

                        sc++;
                    }
                    
                }
            }
            if (orientations.Count != 0)
            {
                Orientation = orientations.Last();
                ((Interfaces.IHasOrientatedTexture)this).OrientateTexture();
                Attack();
                Move();
                orientations.Remove(Orientation);
            }
            //Random rnd = new Random();
            //int r = rnd.Next(4);
            //switch (r)
            //{
            //    case 0:
            //        Orientation = Orientations.North;
            //        break;
            //    case 1:
            //        Orientation = Orientations.East;
            //        break;
            //    case 2:
            //        Orientation = Orientations.South;
            //        break;
            //    case 3:
            //        Orientation = Orientations.West;
            //        break;
            //}
            //((Interfaces.IHasOrientatedTexture)this).OrientateTexture();
            //r = rnd.Next(2);
            //if (r > 0)
            //{
            //    Move();
            //}
            //Attack();
        }
        public Zombie(string name, int id, BitmapImage texture, int hp, int maxhp, Point position) : base(name, id, texture, hp, maxhp, position)
        {
            this.TexturePath = "Textures\\Entities\\Zombie";
            ((Interfaces.IHasOrientatedTexture)this).OrientateTexture();
        }
    }

    internal class Player : Entity, Interfaces.IHitable, Interfaces.IHasInventory, Interfaces.IHasOrientatedTexture
    {
        private Weapon _weapon = Weapons.Pistol;
        private Inventory inventory;

        public Inventory Inventory
        {
            get { return inventory; }
        }

        public Weapon Weapon
        {
            get { return _weapon; }
            set { _weapon = value; }
        }

        public void DistantAttack(MouseEventArgs e, Canvas screen)
        {
            var mp = e.GetPosition(screen);
            double dx = mp.X - screen.Width / 2;
            double dy = mp.Y - screen.Height / 2;
            Point orientation = new Point(Convert.ToInt32(dx / Defaults.TileSize / Settings.zoom), Convert.ToInt32(dy / Defaults.TileSize / Settings.zoom));
            //var a = _weapon.GetType();
            if (_weapon.GetType().Name == "ClipWeapon")
            {
                _weapon.Use(this.Position, orientation);
            }
        }

        public void MeleeAttack()
        {
            if (_weapon.GetType().Name == "MeeleWeapon")
            {
                _weapon.Use(this.Position, this.Orientation);
            }
        }

        public void onHit(int damage)
        {
            this.HP -= damage;
            Effect a = Effects.Bleed();
            a.X += this.Position.X;
            a.Y += this.Position.Y;
            a.Play();
            //var a = Engine._current_scene.World[this.Position.X][this.Position.Y].Object;
            //if (a == null)
            //{
            //    a = Objects.Blood(0);
            //}
            //else
            //{
            //    if (a.GetType() == typeof(Blood))
            //    {
            //        a = Objects.Blood(((Blood)a).Level+1);
            //    }
            //}
            //Engine._current_scene.World[this.Position.X][this.Position.Y].Object = a;
            if (this.HP <= 0)
            {
                this.Texture = new BitmapImage(new Uri(Defaults.NullTexturePath));
                Engine.IsGameOver = true;
            }
        }

        public void Move(KeyEventArgs e)
        {
            Point NewPosition = new Point(0, 0);
            Point NewOrientation = this.Orientation;
            switch (e.Key)
            {
                case Key.W:
                    NewPosition = new Point(Position.X, Position.Y - 1);
                    NewOrientation = Orientations.North;
                    break;
                case Key.A:
                    NewPosition = new Point(Position.X - 1, Position.Y);
                    NewOrientation = Orientations.West;
                    break;
                case Key.S:
                    NewPosition = new Point(Position.X, Position.Y + 1);
                    NewOrientation = Orientations.South;
                    break;
                case Key.D:
                    NewPosition = new Point(Position.X + 1, Position.Y);
                    NewOrientation = Orientations.East;
                    break;
            }
            Orientation = NewOrientation;
            ((Interfaces.IHasOrientatedTexture)this).OrientateTexture();
            Engine.MoveEntity(this, NewPosition.X, NewPosition.Y);
        }

        public void Interact(Point position)
        {
            Object target = Engine.GetObject(position);
            if (target != null)
            {
                if (target.GetType().GetInterface(typeof(Interfaces.IInteractable).Name) != null)
                {
                    ((Interfaces.IInteractable)target).Interaction(this);
                }
            }
        }

        public Player(string name, Point position) : base(name, 0, new BitmapImage(new Uri("F:\\Projects\\Work1\\Textures\\Entities\\Player\\Player.png")), new Point(0, 0))
        {
            this.Position = position;
            this.HP = 5;
            this.MaxHP = 5;
            this.inventory = new Inventory();
            this.TexturePath = "Textures\\Entities\\Player";
            this.Orientation = Orientations.East;
            inventory.Items.Add(this.Weapon);
        }
    }

    internal static class Abilities
    {
        public static void Bleed()
        {

        }
    }
}
