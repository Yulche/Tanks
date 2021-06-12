namespace Tanks.Model
{
    /// <summary>
    /// класс танков
    /// </summary>
    public class Tank : Spirit                               //сущность танка
    {
        public int Injury;                                         //ранение танка
        public Tank(int x, int y) :base(x,y)                //создаем танк в заданной позиции
        {                                               
            direction = Direction.Up;                       //направлени танка
            Injury = 0;                                                //
        }

        public override  void BorderCollision()                             //проверка на выход за границы
        {
            if (X < 0) X = 0;
            if (X > GlobalConst.WindowWidth - GlobalConst.TankSize) X = GlobalConst.WindowWidth - GlobalConst.TankSize;
            if (Y < 0) Y = 0;
            if (Y > GlobalConst.WindowHight - GlobalConst.TankSize) Y = GlobalConst.WindowHight - GlobalConst.TankSize;           
        }

        /// <summary>
        /// Метод проверяющий преграды
        /// </summary>
        /// <param name="wall">стены</param>
        public void Barrier(Wall wall) 
        {
            bool isWall = false ;

            for (int i = 0; i < GlobalConst.TankSize; i++)            //проверяем по ширене танка
            {
                switch (direction)
                {
                    case Direction.Up:
                        //ширина над танком
                        if (wall.GetMapWall(X + i, Y  ) < WallType.None) isWall = true;
                        break;
                    case Direction.Down:
                        //ширина под танком
                        if (wall.GetMapWall(X + i, Y + (GlobalConst.TankSize-GlobalConst.BrickSize)) != WallType.None) isWall = true;
                        break;
                    case Direction.Left:
                        //ширина слева
                        if (wall.GetMapWall(X , Y + i) != WallType.None) isWall = true;
                        break;
                    case Direction.Right:
                        //справа
                        if (wall.GetMapWall(X + (GlobalConst.TankSize - GlobalConst.BrickSize), Y + i) != WallType.None) isWall = true;
                        break;                
                }
                if (isWall) break; //если стена, нет смысла проверять дальше
            }
            if (isWall) 
            {
                switch (direction)
                {
                    case Direction.Up:
                        Y += GlobalConst.MoveStep;
                        break;
                    case Direction.Down:
                        Y -= GlobalConst.MoveStep;
                        break;
                    case Direction.Left:
                        X += GlobalConst.MoveStep;
                        break;
                    case Direction.Right:
                        X -= GlobalConst.MoveStep;
                        break;
                }
            }
        }
        /// <summary>
        /// выстрел
        /// </summary>
        public void Shot()                                     //стрельба танка
        {
            if (!distroy)   //если танк не уничтожен
            {
                Bullet b = new Bullet(X, Y, direction, IsEnemy);            
                ModelsGame.listBullets.Add(b);          //добавляем в список снарядов для отображения
            }                  
        }
    }

    //
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
            if (IsMove) Move();
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
}