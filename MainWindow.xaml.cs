using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Work1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(Screen, BitmapScalingMode.NearestNeighbor);
            Engine._current_scene = new Scene(10, 10);
            Engine.Render(Screen);
            //Engine.Tick();
            //GUI_Controller.Render(Screen);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Player player = Engine.Player;
            switch (e.Key)
            {
                case Key.C:
                    Engine.Player.MeleeAttack();
                    break;
                case Key.W:
                    player.Move(e);
                    break;
                case Key.A:
                    player.Move(e);
                    break;
                case Key.S:
                    player.Move(e);
                    break;
                case Key.D:
                    player.Move(e);
                    break;
                case Key.X:
                    player.Interact(new System.Drawing.Point(player.Position.X + player.Orientation.X, player.Position.Y + player.Orientation.Y));
                    break;
                case Key.I:
                    EquipmentWindow eqw = new EquipmentWindow();
                    eqw.ShowDialog();
                    break;
            }
        }

        private void Screen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Engine.Player.DistantAttack(e, Screen);
        }

        private void Screen_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas aa = Screen;
            Point a = e.GetPosition(Screen);

            bool a1 = a.X < Screen.ActualWidth - 1;
            bool a2 = a.X >= 0;
            bool a3 = a.Y < Screen.ActualHeight - 1;
            bool a4 = a.Y >= 0;

            if (a1 && a2 && a3 && a4)
            {
                Mouse.Capture(Screen);
            }
            else
            {
                Mouse.Capture(null);
            }
        }

        private void Screen_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.Capture(Screen);
        }
    }
}