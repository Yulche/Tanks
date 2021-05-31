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

        public static int TimerInterval = 50;
        
        public static int TankSize = 80;         //размер танка
        public static int MoveStep = 20;         //шаг перемещения

        static string Path = "..\\..\\pic\\";                  //путь для файлов, ..- уровень выше
       //public static string ImageGamerTank = Path + "tank_v1G2.png";   //изображение танка игрока
        
        
        public static Image[] PictureGamerTank = new Image[4];                    //массив картинок для танка
        //public static Dictionary<string, Image> PictureGamerTank;     
        public static Image[] PictureBullet = new Image[1];                       //массив картинок для снаряда

        public static void Init()
        {
            /*
            PictureGamerTank["Up"] = Image.FromFile(Path + "tank_v1G2_Up.png"); //грузим массив картинок для танка
            PictureGamerTank["Down"] = Image.FromFile(Path + "tank_v1G2_Down.png");
            PictureGamerTank["Left"] = Image.FromFile(Path + "tank_v1G2_Left.png");
            PictureGamerTank["Right"] = Image.FromFile(Path + "tank_v1G2_Right.png");
            */
            PictureGamerTank[0] = Image.FromFile(Path + "tank_v1G2_Up.png"); //грузим массив картинок для танка
            PictureGamerTank[1] = Image.FromFile(Path + "tank_v1G2_Down.png");
            PictureGamerTank[2] = Image.FromFile(Path + "tank_v1G2_Left.png");
            PictureGamerTank[3] = Image.FromFile(Path + "tank_v1G2_Right.png");

            PictureBullet[0] = Image.FromFile(Path + "bullet.png");  //грузим картинки для снаряда
        }

    }
    /// <summary>
    /// класс для отображения снаряда
    /// </summary>
    public class ViewBullet          //для отрисовки снаряда
    {
        public PictureBox VBullet;        //картинка для снарда (pictureBox)
        public Bullet ObjBullet;         //ссылка на объект снаряда
        public ViewBullet(Bullet b)
        {
            ObjBullet = b;                   //запоминаем связь с объектом
            VBullet = new PictureBox();
            VBullet.Size = new Size(GlobalConst.TankSize, GlobalConst.TankSize); //задаем размер
            VBullet.Image = GlobalConst.PictureBullet[0];                        //загружаем картинку
            VBullet.Location = new Point(ObjBullet.X, ObjBullet.Y); 
        }
        /// <summary>
        /// Метод изменения локации
        /// </summary>
        public void ChangeLocation()                         //смотрим изменения локации объекта
        {
            VBullet.Location = new Point(ObjBullet.X, ObjBullet.Y);             //изменение положения
            //VBullet.Image = GlobalConst.PictureGamerTank[(int)Game.GamerTank.direction]; //изменение направления
        }
    }
        
        
        
    /*              
           // Controls.Add(item);   //подумать что с эти делать
    */
    /// <summary>
    /// класс содержащий объекты для отображения
    /// </summary>
    public class View                                                              //объекты для отрисовки
    {
        public List<ViewBullet> ListViewBullets = new List<ViewBullet>();          //список снарядов ViewBullet

        /// <summary>
        /// Метод добавляющий снаряд ViewBullet в список снарядов ListViewBullets
        /// </summary>
        /// <param name="Vb"></param>
        public void AddBullet(ViewBullet Vb)                                      //метод добавления визуального снаряда
        {
            ListViewBullets.Add(Vb);
        }
        /// <summary>
        /// Метод добавляющий снаряд Bullet в список снарядов ListViewBullets
        /// </summary>
        /// <param name="b"></param>
        public void AddBullet(Bullet b)                                          //метод добавления снарядов
        {
            AddBullet(new ViewBullet(b));
        }

    }


    public partial class Form1 : Form
    {
        GameModel Game;               //игровая модель игра
        Controller ControlGame;       //контроллер игры

        PictureBox ViewGamerTank;         //отрисовка танка

        public View  ViewGame  = new View();  //отрисовка снарядов и объектов

        public Form1()
        {
            InitializeComponent();
            GlobalConst.Init();
            ClientSize = new Size(GlobalConst.WindowWidth, GlobalConst.WindowHight);   //задаем размер окна
            Game = new GameModel();                                                   //запускаем игровую модель
            ControlGame = new Controller(Game);                                      //запускаем контроллер 


            ViewGamerTank = new PictureBox();                                             //создаем графический образ танка
            ViewGamerTank.Size = new Size(GlobalConst.TankSize, GlobalConst.TankSize);
            //GamerTank.ImageLocation = GlobalConst.PictureGamerTank;
            ViewGamerTank.Image = GlobalConst.PictureGamerTank[0];

            ViewGamerTank.Location = new Point(Game.GamerTank.X, Game.GamerTank.Y);            
            Controls.Add(ViewGamerTank);

            Game.GamerTank.onNewBullet += (bullet) =>  //подписываемся на появление нового снаряда
            {
                ViewBullet vbullet = new ViewBullet(bullet);
                ViewGame.AddBullet(vbullet);
                Controls.Add(vbullet.VBullet);
            };
            

            /*
            Paint += (sender, args) =>
            {
                for (int i = 0; i <= time; i++)
                {
                    args.Graphics.TranslateTransform(centerX, centerY);
                    args.Graphics.RotateTransform(i * 360f / 10);
                    args.Graphics.FillEllipse(Brushes.Blue, radius - size / 2, -size / 2, size, size);
                    args.Graphics.ResetTransform();
                }
            };*/

            //зацикливаем игру
            int time = 0;                                         //количество циклов таймера 
            Timer timer = new Timer();                            //создаем таймер
            timer.Interval = GlobalConst.TimerInterval;           //период таймера
            timer.Tick += (sender, args) =>             //
            {
                //изменения снарядов
                foreach (ViewBullet item in ViewGame.ListViewBullets)
                {
                    item.ObjBullet.move();
                    item.ChangeLocation();
                }

                time++;
                Invalidate();                 //перерисовка графики
            };
            timer.Start();                   //запуск таймера
            //
        }

        protected override void OnKeyDown(KeyEventArgs e)       //обработка кнопок
        {
            ControlGame.ControlKey(e.KeyCode);                 //передача в контроллер

            ViewGamerTank.Location = new Point(Game.GamerTank.X, Game.GamerTank.Y);             //изменение положения
            ViewGamerTank.Image = GlobalConst.PictureGamerTank[(int) Game.GamerTank.direction]; //изменение направления


        }
    }
}
