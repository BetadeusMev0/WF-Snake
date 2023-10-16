using Accessibility;
using System.Diagnostics;
using System.Drawing;

namespace Snake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            map = new Bitmap(800, 800);
            graphics = Graphics.FromImage(map);

            colors = new Color[] { gameArea.BackColor, Color.Black, Color.Red };


            snake.Add(new Point(3, 5));
            snake.Add(new Point(4, 5));
            snake.Add(new Point(5, 5));

            SpawnBerry(); 

            DrawFrame();

            gameArea.Image = map;

            debug();


            

            timer.Tick += Timer_Tick;

            timer.Interval = 200;

            timer.Start();
        }
        System.Windows.Forms.Timer timer = new();

        private void Timer_Tick(object? sender, EventArgs e)
        {
            OneTick();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        List<Point> snake = new();


        Bitmap map = new Bitmap(10, 10);

        Graphics graphics;

        private List<Point> tmp = new();

        private byte[,] cells = new byte[17, 15];

        private Color[] colors = null;

        Size sizeStndr = new Size(40, 40);

        Random rnd = new Random();


        private void SpawnBerry() 
        {
            byte x = 0;
            byte y = 0;

            while (true) 
            {
                x = Convert.ToByte(rnd.Next(0,17));
                y = Convert.ToByte(rnd.Next(0,15));
                if (cells[x, y] == 0) break;
            }
            cells[x, y] = 2;
        }



        private void SnakeToMap()
        {
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 17; j++)
                    if (cells[j,i] == 1) cells[j, i] = 0;

            foreach (Point point in snake)
            {
                cells[point.X, point.Y] = 1;
            }

        }

        private List<Point> prevPos = new();

        private byte nextVector = 0;






        private void snakeMove()
        {
            
            Point point = new Point();

            switch (nextVector)
            {
                case 0:
                    point = snake.Last();
                    point.X++;
                    break;
                case 1:
                    point = snake.Last();
                    point.Y++;
                    break;
                case 2:
                    point = snake.Last();
                    point.Y--;
                    break;
                case 3:
                    point = snake.Last();
                    point.X--;
                    break;
            }

            if (CheckSnakeWall(point)) gameOver = true;
            else
            {
                if (cells[point.X, point.Y] != 2) { snake.Remove(snake.First()); }
                else SpawnBerry();

                snake.Add(point);
            }
        }

        private bool gameOver = false;


        private bool CheckSnakeWall(Point point) 
        {
            if
            (point.X > 16 || point.Y > 14 || point.X < 0 || point.Y < 0) return true;

            foreach (Point pnt in snake) 
            {
                if (pnt == point) return true;
            }


            return false;
        }



        private void DrawFrame()
        {
            SnakeToMap();


            for (int i = 0; i < 17; i++)
                for (int j = 0; j < 15; j++)
                {
                    DrawPixel(GetPoint(i, j), colors[cells[i, j]]);
                }

        }

        public Point GetPoint(int x, int y)
        {
            return new Point(x * 40, y * 40);
        }


        private void DrawPixel(Point point, Color color)
        {
            Rectangle rectangle = new Rectangle(point, sizeStndr);
            Size size = sizeStndr;
            size.Width--;
            size.Height--;
            rectangle = new Rectangle(point, size);
            graphics.DrawRectangle(new Pen(Color.Black), rectangle);
            for (int i = 0; size.Width > 0; i++)
            {
                size.Width--;
                size.Height--;
                rectangle = new Rectangle(point, size);
                graphics.DrawRectangle(new Pen(color), rectangle);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        nextVector = 0;
                        break;

                    case Keys.Up: nextVector = 2; break;
                    case Keys.Down: nextVector = 1; break;
                    case Keys.Left: nextVector = 3; break;
                }
                
            
            
        }

        private void OneTick() 
        {
            if (!gameOver)
            {
                snakeMove();
                DrawFrame();
                gameArea.Image = map;
                debug();
            }
            else { label1.Text = "Game Over!"; timer.Stop(); }
            
        }

        
        private void debug() 
        {
            label1.Text = string.Empty;
            foreach(var point in snake) 
            {
                label1.Text += point;
            }
        }


    }
}