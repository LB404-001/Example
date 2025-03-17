using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
