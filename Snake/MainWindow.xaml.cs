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

        private int dots;   // длина змейки
        private int appleX; 
        private int appleY;
        private int counterScore = 0;   // счёт
        private int[] x = new int[ALL_DOTS];
        private int[] y = new int[ALL_DOTS];
        
        private Image dot;      // текстура змейки
        private Image apple;    // текстура яблока
        private Image border;   // текстура границы карты
        private Image field;    // текстура игрового поля

        private DispatcherTimer timer;    // таймер 
        double speed = 0.2;    // скорость игры (чем меньше, тем быстрее)

        public MainWindow()
        {
            InitializeComponent();
            loadImages();
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
            scoreLabel.Content = "Score: " + counterScore;
            dots = 2;

            // Начальное положение змейки:
            for (int i = 0; i < dots; i++)
            {
                x[i] = 48 - (i * DOT_SIZE);
                y[i] = 48;
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

        private void loadImages()
        {
            //border = new Image();
            //border.HorizontalAlignment = HorizontalAlignment.Left;
            //border.VerticalAlignment = VerticalAlignment.Top;
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
                FruitExistance = false;
                dots++;
                createApple();
                counterScore++; // Очки за яблоко
                UpdateSpeed();
                scoreLabel.Content = "Score: " + counterScore;
            }
        }

        private void checkCollisions()
        {
            for (int i = dots; i > 0; i--)
            {
                if (i > 4 && x[0] == x[i] && y[0] == y[i])
                {
                    //timer.Stop();
                    gameStatusLabel.Content = "Status: Game Over";
                    MessageBox.Show("You have eaten yourself! Your score: " + counterScore);
                    MessageBox.Show("You can continue by pressing OK");
                    //timer.Start();
                    speed = 0.2;
                    UpdateSpeed();
                    dots = 2;
                    counterScore = 0;
                    for (int j = 0; j < dots; j++)
                    {
                        x[j] = 48 - (j * DOT_SIZE);
                        y[j] = 48;
                    }
                    drawBorders();
                    scoreLabel.Content = "Score: " + counterScore;
                }
            }

            if (x[0] > SIZE - 2*DOT_SIZE)
            {
                //timer.Stop();
                gameStatusLabel.Content = "Status: Game Over";
                MessageBox.Show("You hit the wall! Your score: " + counterScore);
                MessageBox.Show("You can continue by pressing OK");
                //timer.Start();
                speed = 0.2;
                UpdateSpeed();
                dots = 2;
                counterScore = 0;
                for (int i = 0; i < dots; i++)
                {
                    x[i] = 48 - (i * DOT_SIZE);
                    y[i] = 48;
                }
                drawBorders();
                scoreLabel.Content = "Score: " + counterScore;
            }
            if (x[0] < 0 + DOT_SIZE)
            {
                //timer.Stop();
                gameStatusLabel.Content = "Status: Game Over";
                MessageBox.Show("You hit the wall! Your score: " + counterScore);
                MessageBox.Show("You can continue by pressing OK");
                //timer.Start();
                speed = 0.2;
                UpdateSpeed();
                dots = 2;
                counterScore = 0;
                for (int i = 0; i < dots; i++)
                {
                    x[i] = 48 - (i * DOT_SIZE);
                    y[i] = 48;
                }
                drawBorders();
                scoreLabel.Content = "Score: " + counterScore;
            }
            if (y[0] > SIZE - 2*DOT_SIZE)
            {
                //timer.Stop();
                gameStatusLabel.Content = "Status: Game Over";
                MessageBox.Show("You hit the wall! Your score: " + counterScore);
                MessageBox.Show("You can continue by pressing OK");
                //timer.Start();
                speed = 0.2;
                UpdateSpeed();
                dots = 2;
                counterScore = 0;
                for (int i = 0; i < dots; i++)
                {
                    x[i] = 48 - (i * DOT_SIZE);
                    y[i] = 48;
                }
                drawBorders();
                scoreLabel.Content = "Score: " + counterScore;
            }
            if (y[0] < 0 + DOT_SIZE)
            {
                //timer.Stop();
                gameStatusLabel.Content = "Status: Game Over";
                MessageBox.Show("You hit the wall! Your score: " + counterScore);
                MessageBox.Show("You can continue by pressing OK");
                //timer.Start();
                speed = 0.2;
                UpdateSpeed();
                dots = 2;
                counterScore = 0;
                for (int i = 0; i < dots; i++)
                {
                    x[i] = 48 - (i * DOT_SIZE);
                    y[i] = 48;
                }
                drawBorders();
                scoreLabel.Content = "Score: " + counterScore;
            }
        }

        private void update()
        {
            gameStatusLabel.Content = "Status: In Game";
            move();
            checkApple();
            checkCollisions();
            drawField();
            drawApple();
            drawBorders();
            drawSnake();
            return;
        }

        private void drawBorders()
        {
            if (border == null)
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
            // генерация если есть текстура
            // ...


        }

        private void drawField()
        {
            if (field == null)
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
            // если есть текстура


        }

        private void drawSnake()
        {
            if (dot == null)
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
            // если есть текстура

        }

        private void drawApple()
        {
            if (apple == null)
            {
                Rectangle myApple = new Rectangle();

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
            // если есть текстура

        }
        

        private void UpdateSpeed()
        {
			if (counterScore % 2 == 0 && counterScore != 0)
            {
                speed -= 0.005;
			}
            speedLabel.Content = "Speed: " + speed;
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
                default:
                    timer.Stop();
                    MessageBox.Show("Pause! Your score for now: " + counterScore + "\n To continue press Ok!");
                    timer.Start();
                    break;
            }

        }
    }
}
