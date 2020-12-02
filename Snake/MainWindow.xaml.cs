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

        // статус в игре игорок или нет
        private bool inGame = true;

        //направления движения змейки
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
        private Image border;   // тексура границы карты
        private Image field;    // текстура игрового поля

        private DispatcherTimer timer;    // 

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
            dots = 3;

            // Начальное положение змейки:
            for (int i = 0; i < dots; i++)
            {
                x[i] = 48 - (i * DOT_SIZE);
                y[i] = 48;
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += MainGameLoop;
            timer.Start();
            createApple();
        }

        private void createApple()
        {
            appleX = new Random(20).Next() * DOT_SIZE;
            appleY = new Random(20).Next() * DOT_SIZE;
        }

        private void loadImages()
        {
            
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
                counterScore += 5; // Очки за яблоко
            }
        }

        private void checkCollisions()
        {
            for (int i = dots; i > 0; i--)
            {
                if (i > 4 && x[0] == x[i] && y[0] == y[i])
                {
                    inGame = false;
                }
            }

            if (x[0] > SIZE)
            {
                inGame = false;
            }
            if (x[0] < 0)
            {
                inGame = false;
            }
            if (y[0] > SIZE)
            {
                inGame = false;
            }
            if (y[0] < 0)
            {
                inGame = false;
            }
        }

        private void update()
        {
            if (inGame)
            {
                drawSnake();
                drawApple();
                checkApple();
                checkCollisions();
                move();
            }
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

        }

        private void drawSnake()
        {

        }

        private void drawApple()
        {

        }

        // Обработчик нажатий
        private void myKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left && !right)
            {
                up = false;
                down = false;
                left = true;
            }
            else if (e.Key == Key.D || e.Key == Key.Right && !left)
            {
                up = false;
                down = false;
                right = true;
            }
            else if (e.Key == Key.W || e.Key == Key.Up && !down)
            {
                left = false;
                right = false;
                up = true;
            }
            else if (e.Key == Key.S || e.Key == Key.Down && !up)
            {
                left = false;
                right = false;
                down = true;
            }
        }
    }
}
