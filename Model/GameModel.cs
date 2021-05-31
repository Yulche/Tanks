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
        public int X { get; set; }         //локация сущности
        public int Y { get; set; }

        public Direction direction { get; set; }              //направление сущности

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
            BorderCollision();            
        }
        /// <summary>
        /// метод проверяющий выход за границы
        /// </summary>
        void BorderCollision()                              //проверка на выход за границы
        {
            if (X < 0) X = 0;
            if (X > GlobalConst.WindowWidth - GlobalConst.TankSize) X = GlobalConst.WindowWidth - GlobalConst.TankSize;
            if (Y < 0) Y = 0;
            if (Y > GlobalConst.WindowHight - GlobalConst.TankSize) Y = GlobalConst.WindowHight - GlobalConst.TankSize;
        }

    }  
    
    /// <summary>
    /// класс снарядов
    /// </summary>
    public class Bullet : Spirit                                  //сущность снаряда
    {     
        public bool distroy { get; set; }                      //уничтожен снаряд
        public Bullet(int x, int y, Direction direction) :base(x, y)        //создаем снаряд в заданной позиции
        {
            this.direction = direction;                                    //и берем направление у танка
            distroy = false;
        }
        /// <summary>
        /// метод проверяющий выход за границы
        /// </summary>
        void BorderCollision()                              //проверка на выход за границы
        {
            if ((X < 0) || (X > GlobalConst.WindowWidth - GlobalConst.TankSize) ||
                (Y < 0) || (Y > GlobalConst.WindowHight - GlobalConst.TankSize))
            { distroy = true; }
        }

    }
    /// <summary>
    /// класс танков
    /// </summary>
    public class Tank : Spirit                                     //сущность танка
    {          
        public List<Bullet> bullets = new List<Bullet>();   //список снарядов танка

        public Tank(int x, int y) :base(x,y)                //создаем танк в заданной позиции
        {                                               
            direction = Direction.Up;                       //направлени танка            
        }

        public delegate void NewBullet(Bullet b);      //создаеи делегат для события
        public event NewBullet onNewBullet;       //создаем событие с типом NewBullet
        public void Shot()                                  //стрельба танка
        {
            Bullet b = new Bullet(X, Y, direction);
            bullets.Add(b);       //добавляем в список снарядов
            onNewBullet(b);        //событие новый снаряд
                                          //для отправки снаряда в список снарядов
        }
    }
    /// <summary>
    /// класс бизнес логики игры
    /// </summary>
    class GameModel                                //игровая модель
    {
        public Tank GamerTank;
        public GameModel() 
        {   
            //создаем танк с необходимыми кординатами
            GamerTank = new Tank(GlobalConst.WindowWidth / 2 - 2*GlobalConst.TankSize, GlobalConst.WindowHight - GlobalConst.TankSize);
               
        }
    }
}
