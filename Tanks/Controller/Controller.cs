using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Tanks.Model;

namespace Tanks.Control
{
    class Controller
    {
        public void ControlKey (Keys key) //обработка кнопок
        {
            switch (key)
            {
                case  Keys.Up:
                    ModelsGame.gamerTnk.direction = Direction.Up;
                    break;
                case  Keys.Down:
                    ModelsGame.gamerTnk.direction = Direction.Down; 
                    break;
                case  Keys.Left:
                    ModelsGame.gamerTnk.direction = Direction.Left;
                    break;
                case  Keys.Right:
                    ModelsGame.gamerTnk.direction = Direction.Right;
                    break;                
            }
            if (key != Keys.Space)           //обработка стрельбы
                ModelsGame.gamerTnk.Move();      //перемещени танка
            else ModelsGame.gamerTnk.Shot();
            ModelsGame.gamerTnk.Barrier(ModelsGame.walls);
        }
        //
        //искусственный интелект для танка
        //
        Random rnd = new Random();
        /// <summary>
        /// Уровень разума
        /// </summary>
        /// <param name="">уровень безумства в %</param>
        /// <returns></returns>
        bool Mind(int x) 
        {
            return (rnd.Next(0, x)==0);
        }
        /// <summary>
        /// метод определяющий направление к цели
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="targetX">координаты цели</param>
        /// <param name="targetY">координаты</param>
        public Direction GoTo(int x, int y, int targetX, int targetY) 
        {
            int dx = targetX - x;
            int dy = targetY - y;
            if ((Math.Abs(dx) < Math.Abs(dy))&&(dx!=0)) //определяем наименьший координатный вектор, по нему и будем определять направление 
            {
                if (dx > 0) return Direction.Right; else return Direction.Left;
            }
            else 
            {
                if (dy > 0) return Direction.Down; else return Direction.Up;
            }            
        }

        /// <summary>
        /// Метод управления вражескими танками
        /// </summary>
        /// <param name="intelligence">уровень разума в %</param>
        public void ContolEnemyTanks(int intelligence, int time) 
        {
            int count = 0;
            foreach (EnemyTank tank in ModelsGame.listEnemyTnks) 
            {
                tank.Barrier(ModelsGame.walls);     //проверка границ

                if (count++ % 2 == 0)
                {
                    //цель танк игрока
                    if (Mind(intelligence)) tank.direction = GoTo(tank.X, tank.Y, ModelsGame.gamerTnk.X, ModelsGame.gamerTnk.Y);
                    else if (time % 50 == 0) tank.direction = (Direction)rnd.Next(0, 3);

                }
                else
                {
                    //цель штаб
                    if (Mind(intelligence)) tank.direction = 
                        GoTo(tank.X, tank.Y,
                        GlobalConst.WindowWidth/2,   
                        GlobalConst.WindowHight - GlobalConst.TankSize/2);
                    else if (time % 25 == 0) tank.direction = (Direction)rnd.Next(0, 3);
                }
                tank.IsMove = Mind(intelligence);  //танк едет, если умный 
                tank.MoveTo();
                if (time % 25 == 0) tank.Shot();                
            }
        }
    }
}
