using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;//

namespace Tanks.Model
{
    /// <summary>
    /// направление
    /// </summary>
    public enum Direction                       
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// базовый класс сущность (дух)
    /// </summary>
    public abstract class Spirit                 
    {
        public int X { get; set; }                              //локация сущности
        public int Y { get; set; }
        public bool IsEnemy { get; set; }                      //вражеский или дружеский

        public Direction direction { get; set; }              //направление 
        public bool distroy { get; set; }                      //уничтожен 

        public Spirit(int x, int y) 
        {
            X = x; Y = y;                                     //создаем сущность с задаными координатами 
        }
        /// <summary>
        /// метод перемещения
        /// </summary>
        public void move()
        {
            switch (direction)
            {
                case Direction.Up:
                    Y -= GlobalConst.MoveStep;
                    break;
                case Direction.Down:
                    Y += GlobalConst.MoveStep;
                    break;
                case Direction.Left:
                    X -= GlobalConst.MoveStep;
                    break;
                case Direction.Right:
                    X += GlobalConst.MoveStep;
                    break;
            }
            this.BorderCollision();            
        }
        /// <summary>
        /// метод проверяющий выход за границы
        /// </summary>
        public abstract void BorderCollision()                             //проверка на выход за границы
       ;/* {
            if (X < 0) X = 0;
            if (X > GlobalConst.WindowWidth - GlobalConst.TankSize) X = GlobalConst.WindowWidth - GlobalConst.TankSize;
            if (Y < 0) Y = 0;
            if (Y > GlobalConst.WindowHight - GlobalConst.TankSize) Y = GlobalConst.WindowHight - GlobalConst.TankSize;
        }*/
    }  
    //
    /// <summary>
    /// класс снарядов
    /// </summary>
    public class Bullet : Spirit                                  //сущность снаряда
    {              
        public Bullet(int x, int y, Direction direction, bool isEnemy) :base(x, y)        //создаем снаряд в заданной позиции
        {
            this.direction = direction;                                    //и берем направление у танка
            IsEnemy = isEnemy;                                             //и тип друг/враг
            distroy = false;
        }
        /// <summary>
        /// метод проверяющий выход за границы
        /// </summary>
        public override void BorderCollision()                              //проверка на выход за границы
        {
            if ((X < 0) || (X > GlobalConst.WindowWidth - GlobalConst.TankSize) ||
                (Y < 0) || (Y > GlobalConst.WindowHight - GlobalConst.TankSize))
            {                 
                distroy = true;
                ModelsGame.listRemoveBullets.Add(this);    //добавляем в список на удаление
            }

            if (X < 0) X = 0;
            if (X > GlobalConst.WindowWidth - GlobalConst.TankSize) X = GlobalConst.WindowWidth - GlobalConst.TankSize;
            if (Y < 0) Y = 0;
            if (Y > GlobalConst.WindowHight - GlobalConst.TankSize) Y = GlobalConst.WindowHight - GlobalConst.TankSize;
        }
        //
        //обработка попаданий
        /// <summary>
        /// Метод проверки поподаний снаряда по другому снаряду
        /// </summary>
        /// <param name="b">снаряд</param>
        public void bump(Bullet b)
        {
            if ((IsEnemy != b.IsEnemy) && (!distroy))    //если снаряды враги друг другу и не уничтожен
            {
                int dx = Math.Abs(X - b.X);          //растояние по модулю, на осях
                int dy = Math.Abs(Y - b.Y);
                if ((dx < GlobalConst.Definition) && (dy < GlobalConst.Definition)) //если меньше заданного
                {
                    distroy = true;                            //этому снаряду - метку: уничтожен
                    ModelsGame.listRemoveBullets.Add(this);    //добавляем этот снаряд в лист на удаление
                    b.distroy = true;                          //тому снаряду - метку: уничтожен
                    ModelsGame.listRemoveBullets.Add(b);      //добавляем тот снаряд в список на удаление
                }
            }           
        }
        /// <summary>
        /// Метод проверки попаданий снаряда по вражескому танку
        /// </summary>
        /// <param name="b"></param>
        public void bump(EnemyTank b)
        {
            if ((IsEnemy != b.IsEnemy) && (!distroy))   //если враги друг другу и не уничтожен
            {
                int dx = Math.Abs(X - b.X);          //растояние по модулю, на осях
                int dy = Math.Abs(Y - b.Y);
                if ((dx < GlobalConst.Definition) && (dy < GlobalConst.Definition))
                {
                    distroy = true;
                    ModelsGame.listRemoveBullets.Add(this);
                    b.distroy = true;
                    ModelsGame.listRemoveTanks.Add(b);         //добавляем этот снаряд в список на удаление
                    GameStatistics.AddScore();
                }
            }
        }
        /// <summary>
        /// метод проверки попаданий снарядов по танку игрока
        /// </summary>
        /// <param name="b">танк игрока</param>
        public void bump(GamerTank b)
        {
            if ((IsEnemy != b.IsEnemy) && (!distroy))   //если враги друг другу и не уничтожен
            {
                int dx = Math.Abs(X - b.X);          //растояние по модулю, на осях
                int dy = Math.Abs(Y - b.Y);
                if ((dx < GlobalConst.Definition) && (dy < GlobalConst.Definition))
                {
                    distroy = true;
                    ModelsGame.listRemoveBullets.Add(this);
                    //b.distroy = true;
                    //ModelsGame.listRemoveTanks.Add(b);                    
                    GameStatistics.DeleteGameLife();
                }
            }
        }
    }
    //
    /// <summary>
    /// класс танков
    /// </summary>
    public class Tank : Spirit                               //сущность танка
    {
        public Tank(int x, int y) :base(x,y)                //создаем танк в заданной позиции
        {                                               
            direction = Direction.Up;                       //направлени танка            
        }

        public override  void BorderCollision()                             //проверка на выход за границы
        {
            if (X < 0) X = 0;
            if (X > GlobalConst.WindowWidth - GlobalConst.TankSize) X = GlobalConst.WindowWidth - GlobalConst.TankSize;
            if (Y < 0) Y = 0;
            if (Y > GlobalConst.WindowHight - GlobalConst.TankSize) Y = GlobalConst.WindowHight - GlobalConst.TankSize;
        }        
        
        public void Shot()                                     //стрельба танка
        {
            if (!distroy)   //если танк не уничтожен
            {
                Bullet b = new Bullet(X, Y, direction, IsEnemy);            
                ModelsGame.listBullets.Add(b);          //добавляем в список снарядов для отображения
            }
                  
        }
    }
    /// <summary>
    /// класс вражеских танков
    /// </summary>
    public class EnemyTank : Tank 
    {
        public bool IsMove { get; set; }                         //двигается танк или стоит
        public EnemyTank(int x, int y) : base(x, y)              //создаем танк в заданной позиции
        {
            direction = Direction.Down;                          //направлени танка  
            IsEnemy = true;
        }
        //
        //управление танком
        public void MoveTo() 
        {
            if (IsMove) move();
        }
    }
    //
    /// <summary>
    /// класс танк игрока
    /// </summary>
    public class GamerTank : Tank
    {
        public GamerTank(int x, int y) : base(x, y)
        {
            direction = Direction.Up;
            IsEnemy = false;
        }
    }
    //
    /// <summary>
    /// Общий класс, в котором хранятся объекты игры
    /// </summary>
    public class ModelsGame
    {
        //объекты игры
        public static List<Bullet> listBullets = new List<Bullet>();             //снаряды
        public static GamerTank gamerTnk;                                        //танк игрока
        public static List<EnemyTank> listEnemyTnks = new List<EnemyTank>();        //танки врага
        //
        //объекты для удаления
        public static List<Bullet> listRemoveBullets = new List<Bullet>();
        public static List<EnemyTank> listRemoveTanks = new List<EnemyTank>();
        //
        public static void AddEnemyTanks()
        {
            if (listEnemyTnks.Count < GlobalConst.CountEnemyTanks) 
            {
                Random rnd = new Random();
                int x=0;
                switch (rnd.Next(0,3))
                {
                    case 0: x = 0;
                        break;
                    case 1: x = GlobalConst.WindowWidth/2-GlobalConst.TankSize;
                        break;
                    case 2: x = GlobalConst.WindowWidth-GlobalConst.TankSize;
                        break;
                }
                ModelsGame.listEnemyTnks.Add(new EnemyTank(x, 0));
            }
        }
        //
        /// <summary>
        /// метод удаления уничтоженных снарядов из списка
        /// </summary>        
        public static void RemoveBullet() 
        {
            foreach (Bullet b in listRemoveBullets) 
            { 
                listBullets.Remove(b);
            }
            listRemoveBullets.Clear();
        }
        /// <summary>
        /// метод удаления уничтоженных танков из списка
        /// </summary>
        public static void RemoveTank()
        {
            foreach (EnemyTank t in listRemoveTanks)
            {
                listEnemyTnks.Remove(t);
            }
            listRemoveTanks.Clear();            
        }
        /// <summary>
        /// метод для удаление уничтоженных объектов
        /// </summary>
        public static void RemoveAll()
        {
            RemoveBullet();
            RemoveTank();
        }        
    }
    //
    /// <summary>
    /// класс для игровой статистики
    /// </summary>
    public class GameStatistics
    {
        public static int Score = 0;
        public static int GameLife = GlobalConst.GameLife;
        
        public delegate void GameOver();      //создаеи делегат для события
        public static event GameOver onGameOver;            //создаем событие с типом NewBullet  */
        public static void AddScore() 
        {
            if (++Score % GlobalConst.CountBonusLife == 0) GameLife++;
        }
        public static void DeleteGameLife() 
        {
            if (--GameLife <=0) onGameOver();        //событие новый снаряд  для отправки снаряда в список снарядов;
        }
    }
    //
    //
    /// <summary>
    /// класс бизнес логики игры
    /// </summary>
    class Game                               //игровая модель
    {
        public Game() 
        {   
            //создаем танк с необходимыми кординатами
            ModelsGame.gamerTnk = new GamerTank(GlobalConst.WindowWidth / 2 - 2*GlobalConst.TankSize, GlobalConst.WindowHight - GlobalConst.TankSize);
            ModelsGame.listEnemyTnks.Add( new EnemyTank(0, 0));
            ModelsGame.listEnemyTnks.Add(new EnemyTank(GlobalConst.WindowWidth - GlobalConst.TankSize, 0));
            ModelsGame.listEnemyTnks.Add(new EnemyTank(GlobalConst.WindowWidth /2 - GlobalConst.TankSize, 0));
        }

    }
}
