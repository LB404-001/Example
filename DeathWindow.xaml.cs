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
    /// Логика взаимодействия для DeathWindow.xaml
    /// </summary>
    public partial class DeathWindow : Window
    {
        public DeathWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Engine._current_scene = new Scene(10,10);
            Engine.IsGameOver = false;
            this.DialogResult = true;
        }
    }
}
