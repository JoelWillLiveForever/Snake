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

        //private bool SpaceBar = false;
        private bool isGameOver = false;

        private int dots;   // длина змейки
        private int appleX; 
        private int appleY;
        private int counterScore = 0;   // счёт
        private int[] x = new int[ALL_DOTS];
        private int[] y = new int[ALL_DOTS];

        private DispatcherTimer timer;    // таймер 
        double speed = 0.2;    // скорость игры (чем меньше, тем быстрее)

        // ЗВУК
        SoundPlayer GameOverSound = new SoundPlayer("../../Resources/Death_Sound.wav");
        SoundPlayer AppleEaten = new SoundPlayer("../../Resources/AppleEaten.wav");
        SoundPlayer ButtonClick = new SoundPlayer("../../Resources/Button_Click.wav");
        MediaPlayer BackgroundMusic = new MediaPlayer();
		internal static string language;

		public MainWindow()
        {
            InitializeComponent();
            initGame();
            drawField();
            drawBorders();
        }

        private void MainGameLoop(object sender, EventArgs e)
        {
            update();
        }

        private void initGame()
        {
            scoreLabel.Content = "Score : " + counterScore;
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
            BackgroundMusic.Open(new Uri("../../Resources/MGS_Encounter.wav", UriKind.RelativeOrAbsolute));
            BackgroundMusic.Play();
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
                AppleEaten.Play();
                dots++;
                createApple();
                counterScore++; // Очки за яблоко
                UpdateSpeed();
                scoreLabel.Content = "Score : " + counterScore;
            }
        }

        private void checkCollisions()
        {
            for (int i = dots; i > 0; i--)
            {
                if (i > 4 && x[0] == x[i] && y[0] == y[i])
                {
                    isGameOver = true;
                }
            }

            if (x[0] > SIZE - 3*DOT_SIZE)
            {
                isGameOver = true;
            }
            if (x[0] < 0 + 2*DOT_SIZE)
            {
                isGameOver = true;
            }
            if (y[0] > SIZE - 3*DOT_SIZE)
            {
                isGameOver = true;
            }
            if (y[0] < 0 + 2*DOT_SIZE)
            {
                isGameOver = true;
            }
        }
        
        private void update()
        {
            move();
            checkApple();
            checkCollisions();
            gameStatusLabel.Content = "Status : In Game";
            drawField();
            drawApple();
            drawSnake();
            if (isGameOver)
            {
                timer.Stop();
                gameStatusLabel.Content = "Status : Game Over";
                BackgroundMusic.Stop();
                GameOverSound.Play();
                drawGameOver();
                drawScore();
                drawPressSpace();
            }
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

        private void drawGamePaused()
        {
            Label GameOverLb = new Label();
            GameOverLb.Content = "PAUSED";
            GameOverLb.Margin = new Thickness
            {
                Left = 175,
                Top = 156
            };
            GameOverLb.Height = 54;
            GameOverLb.Width = 174;
            GameOverLb.FontSize = 35;
            GameOverLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#NFS font");
            GameOverLb.BorderBrush = Brushes.White;

            gameField.Children.Add(GameOverLb);

            return;
        }

        private void drawGameOver()
        {
            Label GameOverLb = new Label();
            GameOverLb.Content = "GAME OVER";
            GameOverLb.Margin = new Thickness
            {
                Left = 138,
                Top = 158
            };
            GameOverLb.Height = 54;
            GameOverLb.Width = 251;
            GameOverLb.FontSize = 35;
            GameOverLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#NFS font");
            GameOverLb.BorderBrush = Brushes.White;

            gameField.Children.Add(GameOverLb);

            return;
        }

        private void drawScore()
        {
            Label ScoreLb = new Label();
            ScoreLb.Content = "YOUR SCORE : " + counterScore;
            ScoreLb.Margin = new Thickness
            {
                Left = 165,
                Top = 202
            };
            ScoreLb.Height = 38;
            ScoreLb.Width = 200;
            ScoreLb.FontSize = 20;
            ScoreLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#NFS font");
            ScoreLb.BorderBrush = Brushes.White;

            gameField.Children.Add(ScoreLb);

            return;
        }

        private void drawPressEnter()
        {
            Label PressSpaceLb = new Label();
            PressSpaceLb.Content = "PRESS ENTER TO CONTINUE";
            PressSpaceLb.Margin = new Thickness
            {
                Left = 112,
                Top = 231
            };
            PressSpaceLb.Height = 34;
            PressSpaceLb.Width = 310;
            PressSpaceLb.FontSize = 20;
            PressSpaceLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#NFS font");
            PressSpaceLb.BorderBrush = Brushes.White;

            gameField.Children.Add(PressSpaceLb);

            return;
        }

        private void drawPressSpace()
        {
            Label PressSpaceLb = new Label();
            PressSpaceLb.Content = "PRESS SPACE TO RESTART";
            PressSpaceLb.Margin = new Thickness
            {
                Left = 112,
                Top = 231
            };
            PressSpaceLb.Height = 34;
            PressSpaceLb.Width = 310;
            PressSpaceLb.FontSize = 20;
            PressSpaceLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#NFS font");
            PressSpaceLb.BorderBrush = Brushes.White;

            gameField.Children.Add(PressSpaceLb);

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
            speedLabel.Content = "Speed : " + speed;
            timer.Interval = TimeSpan.FromSeconds(speed);
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
                case "D":
                    up = false;
                    right = true;
                    down = false;
                    left = false;
                    break;
                case "A":
                    up = false;
                    left = true;
                    down = false;
                    right = false;
                    break;
                case "W":
                    up = true;
                    down = false;
                    left = false;
                    right = false;
                    break;
                case "S":
                    up = false;
                    down = true;
                    left = false;
                    right = false;
                    break;
                case "Escape":
                    ButtonClick.Play();
                    timer.Stop();
                    gameStatusLabel.Content = "Status : Paused";
                    BackgroundMusic.Pause();
                    drawGamePaused();
                    drawScore();
                    drawPressEnter();
                    break;
                case "Return":
                    timer.Start();
                    ButtonClick.Play();
                    BackgroundMusic.Play();
                    break;
                case "Space":
                    GameOverSound.Stop();
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
                    scoreLabel.Content = "Score : " + counterScore;
                    isGameOver = false;
                    BackgroundMusic.Open(new Uri("../../Resources/MGS_Encounter.wav", UriKind.RelativeOrAbsolute));
                    BackgroundMusic.Play();
                    timer.Start();
                    break;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Stop();
            ButtonClick.Play();
            timer.Stop();
            Window1 menu = new Window1();
            menu.Show();
            Close();
        }
    }
}
