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
    /// Логика взаимодействия для Inventory.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {
        Inventory _Requesting;
        Inventory _Target;
        public InventoryWindow()
        {
            InitializeComponent();
            _Requesting = new Inventory();
            _Target = new Inventory();
        }

        internal InventoryWindow(Inventory Requesting, Inventory Target)
        {
            InitializeComponent();
            _Requesting = Requesting;
            _Target = Target;
            ShowInventory();
        }

        private void ToTarget_Click(object sender, RoutedEventArgs e)
        {
            var a = RequestingItems.SelectedItem;
            if (a != null)
            {
                string name = a.ToString()!;
                Transmit(_Requesting, _Target, name);
                ShowInventory();
            }
        }

        private void ToRequesting_Click(object sender, RoutedEventArgs e)
        {
            var a = TargetItems.SelectedItem;
            if (a != null)
            {
                string name = a.ToString()!;
                Transmit(_Target, _Requesting, name);
                ShowInventory();
            }
        }

        private void Transmit(Inventory source, Inventory target, string name)
        {
            if (name != null)
            {
                Item item = source.Items.Find(x => x.Name == name)!;
                source.Items.Remove(item);
                target.Items.Add(item);
            }
        }

        private void ShowInventory()
        {
            RequestingItems.Items.Clear();
            TargetItems.Items.Clear();
            foreach (Item item in _Requesting.Items)
            {
                RequestingItems.Items.Add(item.Name);
            }
            foreach (Item item in _Target.Items)
            {
                TargetItems.Items.Add(item.Name);
            }
        }
    }
}
