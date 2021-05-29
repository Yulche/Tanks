using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;//

namespace Tanks.Model
{
    enum Direction               //направление
    {
        Up,
        Down,
        Left,
        Right
    }
    abstract class Spirit               //базовый класс сущность (дух)
    {
        public int X { get; set; }         //локация сущности
        public int Y { get; set; }

        public Direction direction { get; set; }              //направлени сущности
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

        void BorderCollision() //проверка на выход за границы
        {
            if (X < 0) X = 0;
            if (X > GlobalConst.WindowWidth - GlobalConst.TankSize) X = GlobalConst.WindowWidth - GlobalConst.TankSize;
            if (Y < 0) Y = 0;
            if (Y > GlobalConst.WindowHight - GlobalConst.TankSize) Y = GlobalConst.WindowHight - GlobalConst.TankSize;
        }

    }  
    
    
    class Bullet : Spirit                           //сущность снаряда
    {
        public Bullet(int x, int y)
        {
            X = x; Y = y;                            //создаем снаряд в заданной позиции          

        }

    }

    class Tank : Spirit                              //сущность танка
    {          
        public List<Bullet> bullets = new List<Bullet>();    //список снарядов танка

        public Tank(int x, int y) 
        {
            X = x; Y = y;                           //создаем танк в заданной позиции            
            direction = Direction.Up;                //направлени танка            
        }
        public void Shot()                        //стрельба танка
        {
            bullets.Add(new Bullet(X,Y));         //добавляем в список снаряды
        }
    }

    class GameModel                      //игровая модель
    {
        public Tank GamerTank;
        public GameModel() 
        {
            GamerTank = new Tank(GlobalConst.WindowWidth / 2 - 2*GlobalConst.TankSize, GlobalConst.WindowHight - GlobalConst.TankSize); //создаем танк с необходимыми кординатами
           
        }
    }
}
