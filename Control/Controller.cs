using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Tanks.Model;

namespace Tanks.Control
{
    class Controller
    {
        GameModel Game;
        public Controller(GameModel Game) 
        {
            this.Game = Game;
        }
        public void ControlKey (Keys key) //обработка кнопок
        {
            switch (key)
            {
                case  Keys.Up:
                    Game.GamerTank.direction = Direction.Up;
                    break;
                case  Keys.Down:
                    Game.GamerTank.direction = Direction.Down; 
                    break;
                case  Keys.Left:
                    Game.GamerTank.direction = Direction.Left;
                    break;
                case  Keys.Right:
                    Game.GamerTank.direction = Direction.Right;
                    break;                
            }
            if (key != Keys.Space)           //обработка стрельбы
                Game.GamerTank.move();      //перемещени танка
            else Game.GamerTank.Shot();
        }
    }
}
