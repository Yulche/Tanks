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
        public void Move()
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
}
