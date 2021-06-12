using System;

namespace Tanks.Model
{
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
            GlobalConst.SoundBullet.Play();
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
        public void Bump(Bullet b)
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
        public void Bump(EnemyTank b)
        {
            if ((IsEnemy != b.IsEnemy) && (!distroy))   //если враги друг другу и не уничтожен
            {
                int dx = Math.Abs(X - b.X);          //растояние по модулю, на осях
                int dy = Math.Abs(Y - b.Y);
                if ((dx < GlobalConst.Definition) && (dy < GlobalConst.Definition))
                {
                    GlobalConst.SoundBump.Play();
                    //с увеличением убитых танков увеличивается количество ранений у танка
                    if (++b.Injury > GameStatistics.Score / GlobalConst.CountInjury) 
                    { 
                        b.distroy = true;
                        ModelsGame.listRemoveTanks.Add(b);         //добавляем этот танк в список на удаление
                        GameStatistics.AddScore();
                    } 
                    distroy = true;
                    ModelsGame.listRemoveBullets.Add(this);//добавляем этот снаряд в список на удаление
                    
                }
            }
        }
        /// <summary>
        /// метод проверки попаданий снарядов по танку игрока
        /// </summary>
        /// <param name="b">танк игрока</param>
        public void Bump(GamerTank b)
        {
            if ((IsEnemy != b.IsEnemy) && (!distroy))   //если враги друг другу и не уничтожен
            {
                int dx = Math.Abs(X - b.X);          //растояние по модулю, на осях
                int dy = Math.Abs(Y - b.Y);
                if ((dx < GlobalConst.Definition) && (dy < GlobalConst.Definition))
                {
                    GlobalConst.SoundBump.Play();
                    distroy = true;
                    ModelsGame.listRemoveBullets.Add(this);
                    b.distroy = true;
                    //ModelsGame.listRemoveTanks.Add(b);                    
                    GameStatistics.DeleteGameLife();
                }
            }
        }
        /// <summary>
        /// Метод проверки попаданий по стене
        /// </summary>
        /// <param name="wall"></param>
        public void BumpWall(Wall wall) 
        {
            if (!distroy)
            {            
                switch (direction) 
                {
                    case Direction.Up:
                        distroy = wall.Change(X,Y,
                            X+GlobalConst.TankSize,Y-GlobalConst.BrickSize * GlobalConst.PowerBreakWall,
                            WallType.None);
                        break;
                    case Direction.Down:                    
                        distroy = wall.Change(X, Y,
                            X + GlobalConst.TankSize, Y + GlobalConst.BrickSize * GlobalConst.PowerBreakWall,
                            WallType.None);
                        break;
                    case Direction.Left:
                        distroy = wall.Change(X, Y,
                            X - GlobalConst.BrickSize * GlobalConst.PowerBreakWall, Y + GlobalConst.TankSize,
                            WallType.None);
                        break;
                    case Direction.Right:
                        distroy = wall.Change(X, Y,
                            X + GlobalConst.BrickSize * GlobalConst.PowerBreakWall, Y + GlobalConst.TankSize,
                            WallType.None);
                        break;           
                } 
            }
            if (distroy) ModelsGame.listRemoveBullets.Add(this); //если снаряд уничтожен то в список на уничтожение
        }
    }
}
