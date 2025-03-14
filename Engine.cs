using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Runtime.ConstrainedExecution;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Work1
{
    internal static class Engine
    {
        static bool IsGameOver = false;

        public static Scene _current_scene = new Scene(10, 10);

        public static Player Player
        {
            get { return (Player)_current_scene.Entities.Find(x => x.Id == 0); }
        }

        public static Object GetObject(System.Drawing.Point position)
        {
            if (position.X < _current_scene.World.Length && position.Y < _current_scene.World[0].Length)
            {
                return _current_scene.World[position.X][position.Y].Object;
            }
            else
            {
                return null!;
            }
        }

        public static void GameOver()
        {
            IsGameOver = true;
        }

        public static void Damage(int x, int y, int damage)
        {
            Chunk chunk = _current_scene.World[x][y];
            if (chunk.Entity != null)
            {
                if (chunk.Entity.GetType().GetInterface(typeof(Interfaces.IHitable).Name) != null)
                {
                    var t = (Interfaces.IHitable)chunk.Entity;
                    t.onHit(damage);
                }
            }
        }

        public static void KillEntity(Entity entity)
        {
            if (entity != null)
            {
                _current_scene.Entities.Remove(entity);
                _current_scene.World[entity.Position.X][entity.Position.Y].Entity = null!;

                if (entity.GetType().GetInterface(typeof(Interfaces.IDeath).Name) != null)
                {
                    var t = (Interfaces.IDeath)entity;
                    t.Death();
                }
            }
        }

        public static void RemoveEntity(Entity target)
        {
            _current_scene.Entities.Remove(target);
            _current_scene.World[target.Position.X][target.Position.Y].Entity = null!;
        }

        public static void MoveEntity(Entity entity, int x, int y)
        {
            if (entity != null)
            {
                if (_current_scene.World[x][y].Entity == null && _current_scene.World[x][y].Collision == false)
                {
                    _current_scene.World[x][y].Entity = entity;
                    _current_scene.World[entity.Position.X][entity.Position.Y].Entity = null!;
                    entity.Position = new System.Drawing.Point(x, y);

                    if (_current_scene.World[x][y].Object != null)
                    {
                        if (_current_scene.World[x][y].Object.GetType().GetInterface(typeof(Interfaces.ITriggerable).Name) != null)
                        {
                            var t = (Interfaces.ITriggerable)_current_scene.World[x][y].Object;
                            t.Trigger(entity);
                        }
                    }
                }
            }
        }

        public static Chunk Raycast(System.Drawing.Point source, System.Drawing.Point target)
        {
            if (target.X < _current_scene.World.Length && target.Y < _current_scene.World[0].Length)
            {
                double dx = target.X - source.X;
                double dy = target.Y - source.Y;
                double ox = 0;
                double oy = 0;
                if (dx > 0)
                {
                    ox = 1;
                }
                else
                {
                    ox = -1;
                }
                if (dy > 0)
                {
                    oy = 1;
                }
                else
                {
                    oy = -1;
                }

                Chunk res = null!;

                if (dx == 0)
                {
                    if (dy == 0)
                    {
                        return null!;
                    }
                    else
                    {
                        for (double i = 0.6; i <= dy * oy; i+=0.1)
                        {
                            var c = _current_scene.World[source.X][Convert.ToInt32(source.Y + i * oy)];
                            if (c.Collision || c.Entity != null)
                            {
                                res = c;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    double der = dy / dx;
                    for (double i = 0.6; i <= dx * ox; i+=0.1)
                    {
                        double x = (source.X + i * ox);
                        double y = (source.Y + i * ox * der);
                        var c = _current_scene.World[Convert.ToInt32(x)][Convert.ToInt32(y)];
                        if (c.Collision || c.Entity != null)
                        {
                            res = c;
                            break;
                        }
                    }
                }

                return res;
            }
            else
            {
                return null!;
            }
        }

        public static async void Tick()
        {
            while (true)
            {
                Entity entity = _current_scene.Entities.Find(x => x.Id == 1)!;
                
                if (entity != null)
                {
                    Random rnd = new Random();
                    int o = rnd.Next(4);
                    switch (o)
                    {
                        case 0:
                            entity.Orientation = Orientations.North;
                            break;
                        case 1:
                            entity.Orientation = Orientations.East;
                            break;
                        case 2:
                            entity.Orientation = Orientations.South;
                            break;
                        case 3:
                            entity.Orientation = Orientations.West;
                            break;
                    }
                    //MoveEntity(entity, entity.Position.X + entity.Orientation.X, entity.Position.Y + entity.Orientation.Y);
                }
                await Task.Delay(Defaults.TickRate);
            }
        }

        public static async void Render(Canvas screen)
        {
            if (!IsGameOver)
            {
                Point center = new Point(screen.Width / 2, screen.Height / 2);
                Point zero_point = new Point(center.X - Settings.render_distace * Defaults.TileSize * Settings.zoom, center.Y - Settings.render_distace * Defaults.TileSize * Settings.zoom);
                while (true)
                {
                    Player pl = (Player)_current_scene.Entities.Find(x => x.Id == 0)!;
                    //Render world
                    screen.Children.Clear();
                    var world = Engine._current_scene.World;
                    for (int i = pl.Position.X - Settings.render_distace; i < world.Length && i < pl.Position.X + Settings.render_distace; i++)
                    {
                        if (i >= 0)
                        {
                            for (int j = pl.Position.Y - Settings.render_distace; j < world[i].Length && j < pl.Position.Y + Settings.render_distace; j++)
                            {
                                if (j >= 0)
                                {
                                    Rectangle tile = new Rectangle()
                                    {
                                        Width = Defaults.TileSize * Settings.zoom,
                                        Height = Defaults.TileSize * Settings.zoom,
                                        Fill = new ImageBrush(world[i][j].Ground.Texture),
                                        Stretch = Stretch.UniformToFill,
                                        SnapsToDevicePixels = true

                                    };
                                    Canvas.SetZIndex(tile, 1);
                                    Canvas.SetLeft(tile, ((i - pl.Position.X) * Defaults.TileSize) * Settings.zoom + center.X - Defaults.TileSize * Settings.zoom / 2);
                                    Canvas.SetTop(tile, ((j - pl.Position.Y) * Defaults.TileSize) * Settings.zoom + center.Y - Defaults.TileSize * Settings.zoom / 2);

                                    tile.IsEnabled = false;

                                    screen.Children.Add(tile);

                                    if (world[i][j].Object != null)
                                    {
                                        Rectangle obj = new Rectangle()
                                        {
                                            Width = Defaults.TileSize * Settings.zoom,
                                            Height = Defaults.TileSize * Settings.zoom,
                                            Fill = new ImageBrush(world[i][j].Object.Texture),
                                            Stretch = Stretch.UniformToFill,
                                            SnapsToDevicePixels = true
                                        };
                                        Canvas.SetZIndex(obj, 1);
                                        Canvas.SetLeft(obj, ((i - pl.Position.X) * Defaults.TileSize) * Settings.zoom + center.X - Defaults.TileSize * Settings.zoom / 2);
                                        Canvas.SetTop(obj, ((j - pl.Position.Y) * Defaults.TileSize) * Settings.zoom + center.Y - Defaults.TileSize * Settings.zoom / 2);

                                        tile.IsEnabled = false;

                                        screen.Children.Add(obj);
                                    }

                                    if (world[i][j].Entity != null)
                                    {
                                        Rectangle obj = new Rectangle()
                                        {
                                            Width = Defaults.TileSize * Settings.zoom,
                                            Height = Defaults.TileSize * Settings.zoom,
                                            Fill = new ImageBrush(world[i][j].Entity.Texture),
                                            Stretch = Stretch.UniformToFill,
                                            SnapsToDevicePixels = true
                                        };
                                        Canvas.SetZIndex(obj, 1);
                                        Canvas.SetLeft(obj, ((i - pl.Position.X) * Defaults.TileSize) * Settings.zoom + center.X - Defaults.TileSize * Settings.zoom / 2);
                                        Canvas.SetTop(obj, ((j - pl.Position.Y) * Defaults.TileSize) * Settings.zoom + center.Y - Defaults.TileSize * Settings.zoom / 2);

                                        tile.IsEnabled = false;

                                        screen.Children.Add(obj);
                                    }
                                }
                            }
                        }
                    }

                    //Render Player
                    
                    Rectangle player = new Rectangle()
                    {
                        Width = 16 * Settings.zoom,
                        Height = 16 * Settings.zoom,
                        Fill = new ImageBrush(pl.Texture)
                    };

                    Canvas.SetZIndex(player, 3);
                    Canvas.SetLeft(player, center.X - Defaults.TileSize * Settings.zoom / 2);
                    Canvas.SetTop(player, center.Y - Defaults.TileSize * Settings.zoom / 2);

                    screen.Children.Add(player);

                    BitmapImage weaponTexture = pl.Weapon.Texture;

                    Rectangle weapon = new Rectangle()
                    {
                        Width = weaponTexture.Width * Settings.zoom * 0.75,
                        Height = weaponTexture.Height * Settings.zoom * 0.75,
                        Fill = new ImageBrush(weaponTexture),
                        RenderTransform = new RotateTransform(45)
                    };
                    Canvas.SetZIndex(weapon, 4);

                    //var a = Mouse.GetPosition(screen);
                    System.Windows.Point b = Mouse.GetPosition(screen);
                    double x = b.X - center.X;
                    double y = b.Y - center.Y;
                    weapon.RenderTransform = new RotateTransform((Math.Atan2(y, x)) * 180 / Math.PI);
                    

                    Canvas.SetLeft(weapon, (0.0) * Defaults.TileSize * Settings.zoom + center.X);
                    Canvas.SetTop(weapon, (0.0) * Defaults.TileSize * Settings.zoom + center.Y);

                    screen.Children.Add(weapon);

                    Rectangle el = new Rectangle()
                    {
                        Width = Defaults.TileSize * Settings.zoom,
                        Height = Defaults.TileSize * Settings.zoom,
                        Fill = new SolidColorBrush(Colors.Red)
                    };

                    Canvas.SetZIndex(el, 5);

                    Canvas.SetLeft(el, Defaults.TileSize * Settings.zoom);
                    Canvas.SetTop(el, 0);

                    screen.Children.Add(el);

                    await Task.Delay(10);
                }
            }
        }
    }
}
