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
    public partial class Form1 : Form
    {
        Game game;               //игровая модель игра
        Controller ControlGame;       //контроллер игры
        //Statistics GameStatistics;    //статистика
        bool IsGameOver = false;
        public Form1()
        {
            InitializeComponent();
            GlobalConst.Init();
            DoubleBuffered = true; //чтоб картинка не мигала
            
            BackgroundImage = GlobalConst.background;
            ClientSize = new Size(GlobalConst.WindowWidth + GlobalConst.WinStatisticsWidth, GlobalConst.WindowHight);   //задаем размер окна
            game = new Game();                                                   //запускаем игровую модель
            ControlGame = new Controller();                                     //запускаем контроллер                  
            GlobalConst.SoundBackround.PlayLooping();
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
                        b.Bump(b1); 
                    foreach (EnemyTank t in ModelsGame.listEnemyTnks)  //снаряд с танком
                        b.Bump(t);
                    b.BumpWall(ModelsGame.walls);   //проверяем попадания по стенам
                    b.Bump(ModelsGame.gamerTnk);   //проверяем попадания по танку игрока
                    b.Move();                    //изменения локации
                }
                ModelsGame.AddEnemyTanks();
                ControlGame.ContolEnemyTanks(3,time);    //чем менбше, тем умнеее            
                //
                //удаление уничтоженных объектов
                if (time % 10 == 0)  //задержка удаления
                    ModelsGame.RemoveAll();     
                
                time++;
                Invalidate();                 //перерисовка графики
                if (IsGameOver) 
                {
                    GlobalConst.soundGameOver.Play();
                    MessageBox.Show("Game over");       //вызываем окно с текстом: конец игры
                }   
            };
            timer.Start();                   //запуск таймера
            //
            //подписываемся на событие: конец игры
            GameStatistics.onGameOver += () => 
            { 
                timer.Stop();                       //останавливаем игру
                IsGameOver = true;
                //MessageBox.Show("Game over");       //вызываем окно с текстом: конец игры         
            };
            //
            ModelsGame.walls.onGameOver += () =>
            {
                timer.Stop();                       //останавливаем игру
                IsGameOver = true;
                //MessageBox.Show("Game over");       //вызываем окно с текстом: конец игры         
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
            if (gtank.distroy) 
            {
                gtank.distroy = false;
                gtank.X = GlobalConst.WindowWidth / 2 - 2 * GlobalConst.TankSize;
                gtank.Y = GlobalConst.WindowHight - GlobalConst.TankSize;
            }
            g.DrawImage(GlobalConst.PictureGamerTank[(int)gtank.direction], gtank.X, gtank.Y, GlobalConst.TankSize, GlobalConst.TankSize);
            //
            //отрисовка стен
            for (int i=0; i<ModelsGame.walls.width; i++) 
            {
                for (int j=0; j<ModelsGame.walls.higth; j++) 
                {
                    int t = (int) ModelsGame.walls.mapWall[i, j]; //преобразуем тип стены в int
                    if (t < 2) g.DrawImage(
                        GlobalConst.PictureWall[t],
                        i*GlobalConst.BrickSize,
                        j*GlobalConst.BrickSize,
                        GlobalConst.BrickSize,
                        GlobalConst.BrickSize);
                }
            }
            //рисуем штаб
            g.DrawImage(GlobalConst.PictureBase[0],
                (GlobalConst.WindowWidth-GlobalConst.TankSize)/2,
                GlobalConst.WindowHight-GlobalConst.TankSize,
                GlobalConst.TankSize,
                GlobalConst.TankSize);
            //
            //отрисовка поля статистики
            //g.DrawRectangle(new Pen(Color.Red, 5), GlobalConst.WindowWidth, 0, GlobalConst.WinStatisticsWidth, GlobalConst.WindowHight);
			g.FillRectangle(Brushes.Gray, 
                GlobalConst.WindowWidth, 0, 
                GlobalConst.WinStatisticsWidth, GlobalConst.WindowHight);
            Font font = new Font(FontFamily.GenericSansSerif, 26);
            Brush brush = Brushes.Blue;
            g.DrawString("   Игровая \n cтатистика", font, brush, GlobalConst.WindowWidth, 30);
            int y = 150;
            g.DrawString("  Бонус: ", font, brush, GlobalConst.WindowWidth, y);
            g.DrawString(" "+GlobalConst.CountBonusLife +" x      = ", font, brush, GlobalConst.WindowWidth, y+50);
            g.DrawImage(                                                        //иконка танка врага
                GlobalConst.PictureEnemyTank[0],
                GlobalConst.WindowWidth + 70,
                y + 50);
            g.DrawImage(                                                          //иконка танка игрока
               GlobalConst.PictureGamerTank[0],
               GlobalConst.WindowWidth + 145,
               y + 50);

            y = 300;
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
        }
    }
}
