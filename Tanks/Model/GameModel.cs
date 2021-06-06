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
        public bool distroy { get; set; }                      //уничтожен снаряд
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
            if ((X <= 0) || (X >= GlobalConst.WindowWidth - GlobalConst.TankSize) ||
                (Y <= 0) || (Y >= GlobalConst.WindowHight - GlobalConst.TankSize))
            { //distroy = true;
            }

            if (X < 0) X = 0;
            if (X > GlobalConst.WindowWidth - GlobalConst.TankSize) X = GlobalConst.WindowWidth - GlobalConst.TankSize;
            if (Y < 0) Y = 0;
            if (Y > GlobalConst.WindowHight - GlobalConst.TankSize) Y = GlobalConst.WindowHight - GlobalConst.TankSize;
        }
    }
    
    /// <summary>
    /// класс танков
    /// </summary>
    public class Tank : Spirit                                     //сущность танка
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
        //
        public delegate void NewBullet(Bullet b);      //создаеи делегат для события
        public event NewBullet onNewBullet;            //создаем событие с типом NewBullet        
        public Bullet Shot()                                  //стрельба танка
        {
            Bullet b = new Bullet(X, Y, direction, IsEnemy);
            onNewBullet(b);        //событие новый снаряд  для отправки снаряда в список снарядов
            return b;
        }
    }
    /// <summary>
    /// класс вражеских танков
    /// </summary>
    public class EnemyTank : Tank 
    {        
        public EnemyTank(int x, int y) : base(x, y)                //создаем танк в заданной позиции
        {
            direction = Direction.Down;                         //направлени танка  
            IsEnemy = true;
        }
    }

    public class GamerTank : Tank
    {
        public GamerTank(int x, int y) : base(x, y)
        {
            direction = Direction.Up;
            IsEnemy = false;
        }
    }
    //
    //
    /// <summary>
    /// класс бизнес логики игры
    /// </summary>
    class GameModel                                //игровая модель
    {
        public GamerTank gamerTnk;                //танк игрока
        public List<EnemyTank> ListEnemyTnk;      //танки врага
        public List<Bullet> listBullets;          //снаряды

        public GameModel() 
        {   
            //создаем танк с необходимыми кординатами
            gamerTnk = new GamerTank(GlobalConst.WindowWidth / 2 - 2*GlobalConst.TankSize, GlobalConst.WindowHight - GlobalConst.TankSize);
            ListEnemyTnk.Add( new EnemyTank(GlobalConst.WindowWidth / 2 - 2 * GlobalConst.TankSize, (GlobalConst.WindowHight - GlobalConst.TankSize)/2));


        }
    }
}
