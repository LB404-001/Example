using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work1
{
    internal class Chunk
    {
        private Ground _ground = null!;
        private Entity _entity = null!;
        private Object _object = null!;
        private Point _position = new Point(0,0);
        private bool _collision = false;

        public Point Position
        {
            get { return _position; }
            private set { _position = value; }
        }
        public Ground Ground
        {
            get { return _ground; }
            set { _ground = value; }
        }
        public Entity Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
        public Object Object
        {
            get { return _object; }
            set { _object = value; }
        }
        public bool Collision
        {
            get { return _collision; }
            set { _collision = value; }
        }

        public Chunk()
        {
            //this.Object = new Void();
        }

        public Chunk(Ground ground, Object obj, Entity entity, bool collision, Point position)
        {
            this.Ground = ground;
            this.Object = obj;
            this.Entity = entity;
            this.Collision = collision;
            this.Position = position;
        }
    }
}
