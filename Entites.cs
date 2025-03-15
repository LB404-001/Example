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
            protected set { _HP = value; }
        }
        public BitmapImage Texture
        {
            get { return _texture; }
            protected set { _texture = value; }
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

    internal class NPC : Entity, Interfaces.IHitable
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
        }
    }

    internal class Player : Entity, Interfaces.IHitable, Interfaces.IHasInventory
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
            _weapon.Use(this.Position, this.Orientation);
        }

        public void onHit(int damage)
        {
            this.HP -= damage;
            if (this.HP <= 0)
            {
                this.Texture = new BitmapImage(new Uri(Defaults.NullTexturePath));
                Engine.GameOver();
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
            string playerTexturePath = Defaults.PlayerTexturePath;
            switch (Orientation)
            {
                case var value when value == Orientations.North:
                    playerTexturePath += "\\North.png";
                break;
                case var value when value == Orientations.South:
                    playerTexturePath += "\\South.png";
                    break;
                case var value when value == Orientations.West:
                    playerTexturePath += "\\West.png";
                    break;
                case var value when value == Orientations.East:
                    playerTexturePath += "\\East.png";
                    break;
            }
            Texture = new BitmapImage(new Uri(playerTexturePath));
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

        public Player(string name) : base(name, 0, new BitmapImage(), new Point(0,0))
        {
            this.Position = new Point(0,0);
            this.inventory = new Inventory();
        }

        public Player(string name, Point position) : base(name, 0, new BitmapImage(new Uri("F:\\Projects\\Work1\\Textures\\Entities\\Player\\Player.png")), new Point(0, 0))
        {
            this.Position = position;
            this.HP = 6;
            this.inventory = new Inventory();
            inventory.Items.Add(this.Weapon);
        }
    }
}
