using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tanks.Model;
using Tanks.Control;

namespace Tanks
{
    /// <summary>
    /// глобальные параметры
    /// </summary>
    public class GlobalConst 
    {
        public static int WindowWidth = 800;      //размеры окна
        public static int WindowHight = 600;
        public static int WinStatisticsWidth = 200; //ширина области статистики

        public static int TimerInterval = 50;     //время между тактами (чем больше, тем медленее)
        public static int GameLife = 100;        //количество жизней
        public static int CountBonusLife = 2;   //количество танков, что бы получить бонусную жизнь
        public static int CountEnemyTanks = 4;   //количество вражеских танков одновременно

        public static int TankSize = 80;         //размер танка
        public static int MoveStep = 20;         //шаг перемещения
        public static int Definition = TankSize / 2;  //точность (попаданий)

        public static string Path = "pic\\";// "..\\..\\pic\\";                  //путь для файлов, ..- уровень выше
                                                                      //public static string ImageGamerTank = Path + "tank_v1G2.png";   //изображение танка игрока
        public static Image background = Image.FromFile(Path + "marble.jpg");      //фон
        
        public static Image[] PictureGamerTank = new Image[4];                    //массив картинок для танка игрока GamerTank
        //public static Dictionary<string, Image> PictureGamerTank;               
        public static Image[] PictureEnemyTank = new Image[4];                    //массив картинок для вражеского танка
        public static Image[] PictureBullet = new Image[3];                       //массив картинок для снаряда

        public static void Init()
        {         
            //загрузка массив картинок для танка игрока GamerTank
            PictureGamerTank[0] = Image.FromFile(Path + "tank_v1G2_Up.png"); 
            PictureGamerTank[1] = Image.FromFile(Path + "tank_v1G2_Down.png");
            PictureGamerTank[2] = Image.FromFile(Path + "tank_v1G2_Left.png");
            PictureGamerTank[3] = Image.FromFile(Path + "tank_v1G2_Right.png");
            //
            //загрузка массив картинок для вражеского танка EnemyTank
            PictureEnemyTank[0] = Image.FromFile(Path + "tank_Enemy_Up.png");
            PictureEnemyTank[1] = Image.FromFile(Path + "tank_enemy_Down.png");
            PictureEnemyTank[2] = Image.FromFile(Path + "tank_Enemy_Left.png");
            PictureEnemyTank[3] = Image.FromFile(Path + "tank_Enemy_Right.png");
            //
            //загрузка картинки для снаряда
            PictureBullet[0] = Image.FromFile(Path + "bullet_0.png");
            PictureBullet[1] = Image.FromFile(Path + "bullet_1.png");
            PictureBullet[2] = Image.FromFile(Path + "bullet_2.png");
        }
    }
    public partial class Form1 : Form
    {
        Game game;               //игровая модель игра
        Controller ControlGame;       //контроллер игры
        //Statistics GameStatistics;    //статистика

        public Form1()
        {
            InitializeComponent();
            GlobalConst.Init();
            DoubleBuffered = true; //чтоб картинка не мигала
            
            BackgroundImage = GlobalConst.background;
            ClientSize = new Size(GlobalConst.WindowWidth + GlobalConst.WinStatisticsWidth, GlobalConst.WindowHight);   //задаем размер окна
            game = new Game();                                                   //запускаем игровую модель
            ControlGame = new Controller();                                     //запускаем контроллер                  
            //GameStatistics = new Statistics();
            //
            //зацикливаем игру
            int time = 0;                                         //количество циклов таймера 
            Timer timer = new Timer();                            //создаем таймер
            timer.Interval = GlobalConst.TimerInterval;           //период таймера
            timer.Tick += (sender, args) =>             //
            {               
                foreach (Bullet b in ModelsGame.listBullets)
                {
                    //проверка на попадания
                    foreach (Bullet b1 in ModelsGame.listBullets)      //снаряд со снарядом
                        b.bump(b1); 
                    foreach (EnemyTank t in ModelsGame.listEnemyTnks)  //снаряд с танком
                        b.bump(t);
                    b.bump(ModelsGame.gamerTnk);   //проверяем попадания по танку игрока
                    b.move();                    //изменения локации
                }
                ModelsGame.AddEnemyTanks();
                ControlGame.ContolEnemyTanks(5,time);
                //
                //удаление уничтоженных объектов
                if (time % 10 == 0)  //задержка удаления
                    ModelsGame.RemoveAll();                  
                time++;
                Invalidate();                 //перерисовка графики
            };
            timer.Start();                   //запуск таймера
            //
            //подписываемся на событие: конец игры
            GameStatistics.onGameOver += () => 
            { 
                timer.Stop();                       //останавливаем игру
                MessageBox.Show("Game over");       //вызываем окно с текстом: конец игры         
            };
        }
        //
        /// <summary>
        /// отрисовка графики
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //отрисовываем объекты из класса ModelsGame
            foreach (Bullet bullet in ModelsGame.listBullets)
            {
                if (bullet.IsEnemy)    //по разному отрисовываем вражеские и дружеские снаряды
                    g.DrawImage(
                        GlobalConst.PictureBullet[1], 
                        bullet.X, bullet.Y, 
                        GlobalConst.TankSize, GlobalConst.TankSize);
                else
                    g.DrawImage(
                        GlobalConst.PictureBullet[0], 
                        bullet.X, bullet.Y, 
                        GlobalConst.TankSize, GlobalConst.TankSize);
            }
            foreach (EnemyTank etank in ModelsGame.listEnemyTnks)
            {
                if (!etank.distroy) //если не взорван 
                    g.DrawImage(
                        GlobalConst.PictureEnemyTank[(int)etank.direction], 
                        etank.X, etank.Y, 
                        GlobalConst.TankSize, GlobalConst.TankSize);
                else
                    g.DrawImage(
                        GlobalConst.PictureBullet[2],
                        etank.X, etank.Y,
                        GlobalConst.TankSize, GlobalConst.TankSize);
            }
            //отрисовка танка игрока
            var gtank = ModelsGame.gamerTnk;
            g.DrawImage(GlobalConst.PictureGamerTank[(int)gtank.direction], gtank.X, gtank.Y, GlobalConst.TankSize, GlobalConst.TankSize);
            //
            //отрисовка поля статистики
            g.DrawRectangle(new Pen(Color.Red, 5), GlobalConst.WindowWidth, 0, GlobalConst.WinStatisticsWidth, GlobalConst.WindowHight);
            Font font = new Font(FontFamily.GenericSansSerif, 26);
            Brush brush = Brushes.Blue;
            g.DrawString("   Игровая \n cтатистика", font, brush, GlobalConst.WindowWidth, 30);
            int y = 150;            
            g.DrawString("  Счет: ", font, brush, GlobalConst.WindowWidth, y);  //счет
            g.DrawImage(                                                        //иконка танка врага
                GlobalConst.PictureEnemyTank[0],
                GlobalConst.WindowWidth+5,
                y+60);
            font = new Font(FontFamily.GenericSansSerif, 35);
            g.DrawString("" + GameStatistics.Score, font, brush, GlobalConst.WindowWidth + 50, y + 50);
            y = 450;
            font = new Font(FontFamily.GenericSansSerif, 26);
            g.DrawString("  Жизни: ", font, brush, GlobalConst.WindowWidth, y);   //жизни
            g.DrawImage(                                                          //иконка танка игрока
                GlobalConst.PictureGamerTank[0], 
                GlobalConst.WindowWidth+5, 
                y+60);
            font = new Font(FontFamily.GenericSansSerif, 35);
            g.DrawString("" + GameStatistics.GameLife, font, brush, GlobalConst.WindowWidth+50, y+50);
        }








        protected override void OnKeyDown(KeyEventArgs e)       //обработка кнопок
        {
            ControlGame.ControlKey(e.KeyCode);                 //передача в контроллер
            /*
            ViewGamerTank.Location = new Point(Game.GamerTank.X, Game.GamerTank.Y);             //изменение положения
            ViewGamerTank.Image = GlobalConst.PictureGamerTank[(int) Game.GamerTank.direction]; //изменение направления
            */

        }
    }
}
