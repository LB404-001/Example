using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Work1
{
    /// <summary>
    /// Логика взаимодействия для Equipment.xaml
    /// </summary>
    public partial class EquipmentWindow : Window
    {
        static bool IsUpdatig = false;
        private Player player;
        public EquipmentWindow()
        {
            InitializeComponent();
            player = Engine.Player;
            ShowItems();
        }

        private void TakeOffButton_Click(object sender, RoutedEventArgs e)
        {
            string item = InventoryItems.SelectedItem.ToString()!;
            if (item != null)
            {
                if (item == player.Weapon.Name)
                {
                    player.Weapon = Weapons.Hand;
                }
            }
            ShowItems();
        }

        private void EquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            string item = InventoryItems.SelectedItem.ToString()!;
            if (item != null)
            {
                var a = player.Inventory.Items.Find(x => x.Name == item)!;
                if (a.GetType().IsSubclassOf(typeof(Weapon)))
                {
                    player.Weapon = (Weapon)player.Inventory.Items.Find(x => x.Name == item)!;
                }
            }
            ShowItems();
        }

        private void ShowItems()
        {
            IsUpdatig = true;
            InventoryItems.Items.Clear();
            foreach (var item in player.Inventory.Items)
            {
                InventoryItems.Items.Add(item.Name);
            }
            LabelWeaponName.Content = player.Weapon.Name;
            IsUpdatig = false;
        }

        private void InventoryItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EquipmentButton.IsEnabled = false;
            TakeOffButton.IsEnabled = false;
            UseButton.IsEnabled = false;
            if (!IsUpdatig)
            {
                string item = InventoryItems.SelectedItem.ToString()!;
                if (item != null)
                {
                    var a = player.Inventory.Items.Find(x => x.Name == item)!;
                    if (a != null)
                    {
                        if (a.GetType().IsSubclassOf(typeof(Weapon)))
                        {
                            if (player.Weapon == a)
                            {
                                TakeOffButton.IsEnabled = true;
                            }
                            EquipmentButton.IsEnabled = true;
                        }
                        if (a.GetType().GetInterface(typeof(Interfaces.IUsable).Name) != null)
                        {
                            UseButton.IsEnabled = true;
                        }
                    }
                }
            }
        }

        private void UseButton_Click(object sender, RoutedEventArgs e)
        {
            string item = InventoryItems.SelectedItem.ToString()!;
            if (item != null)
            {
                var a = player.Inventory.Items.Find(x => x.Name == item)!;
                if (a != null)
                {
                    if (a.GetType().GetInterface(typeof(Interfaces.IUsable).Name) != null)
                    {
                        ((Interfaces.IUsable)a).Use(Engine.Player, Engine.Player.Position);
                    }
                }
            }
            ShowItems();
        }
    }
}
