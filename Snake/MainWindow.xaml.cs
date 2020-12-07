using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Globalization;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int SIZE = 512;   // размер игрового поля в px
        private const int DOT_SIZE = 16;    // размер одной ячейки змейки в px
        private const int ALL_DOTS = 512;

        // направления движения змейки
        private bool right = true;
        private bool left = false;
        private bool up = false;
        private bool down = false;

        private int dots;   // длина змейки
        private int appleX; 
        private int appleY;
        private int counterScore = 0;   // счёт
        private int[] x = new int[ALL_DOTS];
        private int[] y = new int[ALL_DOTS];

        private DispatcherTimer timer;    // таймер 
        double speed = 0.2;    // скорость игры (чем меньше, тем быстрее)

        public static string language; // язык

        string score_t, speed_t, status_t;

        public MainWindow()
        {
            InitializeComponent();
            initGame();
            drawField();
            drawBorders();
            language = "ru-RU";
        }

        private void MainGameLoop(object sender, EventArgs e)
        {
            update();
        }

        private void initGame()
        {
            scoreLabel.SetResourceReference(DataContextProperty, "Score");
            score_t = scoreLabel.Content.ToString();
            scoreLabel.Content += " : " + counterScore;
            speedLabel.SetResourceReference(DataContextProperty, "Speed");
            speed_t = speedLabel.Content.ToString();
            gameStatusLabel.SetResourceReference(DataContextProperty, "Status");
            status_t = gameStatusLabel.Content.ToString();
            dots = 2;

            // Начальное положение змейки:
            for (int i = 0; i < dots; i++)
            {
                x[i] = 192 - (i * DOT_SIZE);
                y[i] = 144;
            }

            timer = new DispatcherTimer();
            UpdateSpeed();
            timer.Tick += MainGameLoop;
            timer.Start();
            createApple();
        }

        private void createApple()
        {   
            do
            {
                appleX = new Random().Next(DOT_SIZE, SIZE - DOT_SIZE);
            } while (appleX % DOT_SIZE != 0);
            do
            {
                appleY = new Random().Next(DOT_SIZE, SIZE - DOT_SIZE);
            } while (appleY % DOT_SIZE != 0);
        }

        private void move()
        {
            for (int i = dots; i > 0; i--)
            {
                x[i] = x[i - 1];
                y[i] = y[i - 1];
            }

            if (right)
            {
                x[0] += DOT_SIZE;
            }
            if (left)
            {
                x[0] -= DOT_SIZE;
            }
            if (down)
            {
                y[0] += DOT_SIZE;
            }
            if (up)
            {
                y[0] -= DOT_SIZE;
            }
        }

        private void checkApple()
        {
            if (x[0] == appleX && y[0] == appleY)
            {
                dots++;
                createApple();
                counterScore++; // Очки за яблоко
                UpdateSpeed();
                scoreLabel.Content = score_t + " : " + counterScore;
            }
        }

        private void checkCollisions()
        {
            for (int i = dots; i > 0; i--)
            {
                if (i > 4 && x[0] == x[i] && y[0] == y[i])
                {
                    textbox.SetResourceReference(TagProperty, "Game_Over");
                    gameStatusLabel.Content = status_t + textbox.Tag + "";
                    Death_Sound();
                    textbox.SetResourceReference(TagProperty, "Message_eaten");
                    string message_1 = textbox.Tag + " " + counterScore;
                    textbox.SetResourceReference(TagProperty, "Message_continue");
                    string message_2 = textbox.Tag + "";
                    MessageBox.Show(message_1 + "\n" +message_2);
                    speed = 0.2;
                    UpdateSpeed();
                    dots = 2;
                    counterScore = 0;

                    // Начальное положение змейки:
                    for (int j = 0; j < dots; j++)
                    {
                        x[j] = 192 - (j * DOT_SIZE);
                        y[j] = 144;
                    }
                    scoreLabel.Content = score_t + " : " + counterScore;
                }
            }

            if (x[0] > SIZE - 2*DOT_SIZE)
            {

                textbox.SetResourceReference(TagProperty, "Game_Over");
                gameStatusLabel.Content = status_t + textbox.Tag + "";
                Death_Sound();
                textbox.SetResourceReference(TagProperty, "Message_hit");
                string message_1 = textbox.Tag + " " + counterScore;
                textbox.SetResourceReference(TagProperty, "Message_continue");
                string message_2 = textbox.Tag + "";
                MessageBox.Show(message_1 + "\n" + message_2);
                speed = 0.2;
                UpdateSpeed();
                dots = 2;
                counterScore = 0;

                // Начальное положение змейки:
                for (int i = 0; i < dots; i++)
                {
                    x[i] = 192 - (i * DOT_SIZE);
                    y[i] = 144;
                }
                scoreLabel.Content = score_t + " : " + counterScore;
            }
            if (x[0] < 0 + DOT_SIZE)
            {
                textbox.SetResourceReference(TagProperty, "Game_Over");
                gameStatusLabel.Content = status_t + textbox.Tag + "";
                Death_Sound();
                textbox.SetResourceReference(TagProperty, "Message_hit");
                string message_1 = textbox.Tag + " " + counterScore;
                textbox.SetResourceReference(TagProperty, "Message_continue");
                string message_2 = textbox.Tag + "";
                MessageBox.Show(message_1 + "\n" + message_2);
                speed = 0.2;
                UpdateSpeed();
                dots = 2;
                counterScore = 0;

                // Начальное положение змейки:
                for (int i = 0; i < dots; i++)
                {
                    x[i] = 192 - (i * DOT_SIZE);
                    y[i] = 144;
                }
                scoreLabel.Content = score_t + " : " + counterScore;
            }
            if (y[0] > SIZE - 2*DOT_SIZE)
            {
                textbox.SetResourceReference(TagProperty, "Game_Over");
                gameStatusLabel.Content = status_t + textbox.Tag + "";
                Death_Sound();
                textbox.SetResourceReference(TagProperty, "Message_hit");
                string message_1 = textbox.Tag + " " + counterScore;
                textbox.SetResourceReference(TagProperty, "Message_continue");
                string message_2 = textbox.Tag + "";
                MessageBox.Show(message_1 + "\n" + message_2);
                speed = 0.2;
                UpdateSpeed();
                dots = 2;
                counterScore = 0;

                // Начальное положение змейки:
                for (int i = 0; i < dots; i++)
                {
                    x[i] = 192 - (i * DOT_SIZE);
                    y[i] = 144;
                }
                scoreLabel.Content = score_t + " : " + counterScore;
            }
            if (y[0] < 0 + DOT_SIZE)
            {
                textbox.SetResourceReference(TagProperty, "Game_Over");
                gameStatusLabel.Content = status_t + textbox.Tag + "";
                Death_Sound();
                textbox.SetResourceReference(TagProperty, "Message_hit");
                string message_1 = textbox.Tag + " " + counterScore;
                textbox.SetResourceReference(TagProperty, "Message_continue");
                string message_2 = textbox.Tag + "";
                MessageBox.Show(message_1 + "\n" + message_2);
                speed = 0.2;
                UpdateSpeed();
                dots = 2;
                counterScore = 0;

                // Начальное положение змейки:
                for (int i = 0; i < dots; i++)
                {
                    x[i] = 192 - (i * DOT_SIZE);
                    y[i] = 144;
                }
                scoreLabel.Content = score_t + " : " + counterScore;
            }
        }

        private void update()
        {
            textbox.SetResourceReference(TagProperty, "In_Game");
            gameStatusLabel.Content = status_t + textbox.Tag + "";
            move();
            checkApple();
            checkCollisions();
            drawField();
            drawApple();
            drawSnake();
            return;
        }

        private void drawBorders()
        {
            Rectangle borderTop = new Rectangle();
            Rectangle borderBottom = new Rectangle();
            Rectangle borderLeft = new Rectangle();
            Rectangle borderRight = new Rectangle();

            borderTop.Fill = borderBottom.Fill = borderLeft.Fill = borderRight.Fill = Brushes.Black;
            borderTop.Stroke = borderBottom.Stroke = borderLeft.Stroke = borderRight.Stroke = Brushes.Black;
            borderTop.HorizontalAlignment = borderBottom.HorizontalAlignment = borderLeft.HorizontalAlignment = borderRight.HorizontalAlignment = HorizontalAlignment.Left;
            borderTop.VerticalAlignment = borderBottom.VerticalAlignment = borderLeft.VerticalAlignment = borderRight.VerticalAlignment = VerticalAlignment.Top;

            borderTop.Width = borderBottom.Width = SIZE;
            borderTop.Height = borderBottom.Height = DOT_SIZE;

            borderLeft.Width = borderRight.Width = DOT_SIZE;
            borderLeft.Height = borderRight.Height = SIZE - 2 * DOT_SIZE;

            // Top border
            borderTop.Margin = new Thickness
            {
                Left = 0,
                Top = 0,
            };
            gameField.Children.Add(borderTop);

            // Bottom border
            borderTop.Margin = new Thickness
            {
                Left = 0,
                Top = SIZE - DOT_SIZE,
            };
            gameField.Children.Add(borderBottom);

            // Left border
            borderLeft.Margin = new Thickness
            {
                Left = 0,
                Top = DOT_SIZE,
            };
            gameField.Children.Add(borderLeft);

            // Right border
            borderRight.Margin = new Thickness
            {
                Left = SIZE - DOT_SIZE,
                Top = DOT_SIZE,
            };
            gameField.Children.Add(borderRight);

            return;
        }

        private void drawField()
        {
            Rectangle field = new Rectangle();

            field.Fill = Brushes.GreenYellow;
            field.Stroke = Brushes.GreenYellow;
            field.HorizontalAlignment = HorizontalAlignment.Left;
            field.VerticalAlignment = VerticalAlignment.Top;

            field.Width = field.Height = SIZE - 2 * DOT_SIZE;

            // field
            field.Margin = new Thickness
            {
                Left = DOT_SIZE,
                Top = DOT_SIZE,
            };
            gameField.Children.Add(field);

            return;
        }

        private void drawSnake()
        {
            Image SnakeHead = new Image(); // голова змеи

            SnakeHead.Source = new BitmapImage(new Uri("Resources/headOfSnake.png", UriKind.Relative));
            SnakeHead.HorizontalAlignment = HorizontalAlignment.Left;
            SnakeHead.VerticalAlignment = VerticalAlignment.Top;
            SnakeHead.Width = SnakeHead.Height = DOT_SIZE;

            SnakeHead.Margin = new Thickness
            {
                Left = x[0],
                Top = y[0],
            };
            gameField.Children.Add(SnakeHead);

            for (int i = 1; i < dots; i++) // остальное тело змеи
            {
                Rectangle snakeDot = new Rectangle();

                snakeDot.Fill = Brushes.DarkGreen;
                snakeDot.Stroke = Brushes.DarkGreen;
                snakeDot.HorizontalAlignment = HorizontalAlignment.Left;
                snakeDot.VerticalAlignment = VerticalAlignment.Top;

                snakeDot.Width = snakeDot.Height = DOT_SIZE;

                // apple
                snakeDot.Margin = new Thickness
                {
                    Left = x[i],
                    Top = y[i],
                };
                gameField.Children.Add(snakeDot);
            }

            return;
        }

        private void drawApple()
        {
            Rectangle myApple = new Rectangle();

            // Генерация цвета или текстуры у фрукта:

            //SolidColorBrush[] brushes = { Brushes.Red, Brushes.Yellow, Brushes.Orange, Brushes.Purple };
            //int indexOfColor = new Random().Next(brushes.Length);

            //myApple.Fill = brushes[indexOfColor];
            //myApple.Stroke = brushes[indexOfColor];

            myApple.Fill = Brushes.Red;
            myApple.Stroke = Brushes.Red;
            myApple.HorizontalAlignment = HorizontalAlignment.Left;
            myApple.VerticalAlignment = VerticalAlignment.Top;

            myApple.Width = myApple.Height = DOT_SIZE;

            // apple
            myApple.Margin = new Thickness
            {
                Left = appleX,
                Top = appleY,
            };

            gameField.Children.Add(myApple);

            return;
        }
        
        private void UpdateSpeed()
        {
			if (counterScore % 2 == 0 && counterScore != 0)
            {
                speed -= 0.005;
			}
            speedLabel.Content = speed_t + " : " + speed;
            timer.Interval = TimeSpan.FromSeconds(speed);
        }
        private void Death_Sound() // Та самая отсылка на MGS
        {
            //Uri uri = new Uri("/Resources/Death_Sound.wav", UriKind.RelativeOrAbsolute);
			//SoundPlayer player = new SoundPlayer(uri.ToString());
			//player.Play();
		}

        // Обработчик нажатий
        private void myKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key.ToString())
            {
                case "Right":
                    up = false;
                    right = true;
                    down = false;
                    left = false;
                    break;
                case "Left":
                    up = false;
                    left = true;
                    down = false;
                    right = false;
                    break;
                case "Up":
                    up = true;
                    down = false;
                    left = false;
                    right = false;
                    break;
                case "Down":
                    up = false;
                    down = true;
                    left = false;
                    right = false;
                    break;
                default:
                    timer.Stop();
                    textbox.SetResourceReference(TagProperty, "Pause_score");
                    string message_1 = textbox.Tag + " " + counterScore;
                    textbox.SetResourceReference(TagProperty, "Pause_continue");
                    string message_2 = textbox.Tag + "";
                    MessageBox.Show(message_1 + "\n" + message_2);
                    timer.Start();
                    break;
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Window1 menu = new Window1();
            menu.Show();
            Close();
        }
    }
}
