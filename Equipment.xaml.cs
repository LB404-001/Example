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
            InventoryItems.Items.Clear();
            foreach (var item in player.Inventory.Items)
            {
                InventoryItems.Items.Add(item.Name);
            }
            LabelWeaponName.Content = player.Weapon.Name;
        }
    }
}
