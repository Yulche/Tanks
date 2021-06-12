namespace Tanks.Model
{
    /// <summary>
    /// тип стены
    /// </summary>
    public enum WallType      
    {
        Brick,        //кирпич
        Concrete,      //бетон
        None,         //пусто
        Base          //штаб
    }
    /// <summary>
    /// класс стена
    /// </summary>
    public class Wall
    {
        public int width = GlobalConst.WindowWidth / GlobalConst.BrickSize;  //размеры карты, пропорционально кирпичу
        public int higth = GlobalConst.WindowHight / GlobalConst.BrickSize;

        public WallType[,] mapWall; //карта стен

        public delegate void GameOver();      //создаеи делегат для события
        public event GameOver onGameOver;            //создаем событие с типом GameOver */

        public Wall()
        {
            mapWall = new WallType[width, higth];      //карта стен  пропорционально размеру кирпича

            for (int j = 0; j < higth; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    mapWall[i, j] = WallType.None;
                }
            }
        }
        /// <summary>
        /// Метод возвращающий тип стены в указанных координатах
        /// </summary>
        /// <param name="x">реальные координаты</param>
        /// <param name="y"></param>
        /// <returns></returns>
        public WallType GetMapWall(int x, int y)
        {
            x /= GlobalConst.BrickSize;       //координаты пропорционально размеру кирпича
            y /= GlobalConst.BrickSize;

            if ((x >= 0) && (x < width) && (y >= 0) && (y < higth)) //проверка границ массива
            {
                return mapWall[x, y];       //координаты пропорционально размеру кирпича
            }
            else return WallType.None;                
        }
        /// <summary>
        /// Метод добавляющий в карту стену с координатами xy
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        public bool Change(int x, int y, WallType type)
        {
            x /= GlobalConst.BrickSize;       //координаты пропорционально размеру кирпича
            y /= GlobalConst.BrickSize;
            if ((x >= 0) && (x < width) && (y >= 0) && (y < higth)) //проверка границ массива
            {
                WallType lastType = mapWall[x, y] ;
                if (lastType == WallType.Base) onGameOver();
                if (lastType != WallType.Concrete) mapWall[x, y] = type;   //если был не бетон, то меняем
                return lastType != WallType.None;                         //если бало пусто, то вернем ложь (т.е. не взаимодействовали ни с чем)
            }
            return false;
        }
        /// <summary>
        /// Метод добавляющий в карту стену от xy до x1y1
        /// </summary>
        /// <param name="x">координаты</param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="type">тип стены</param>
        public bool Change(int x, int y, int x1, int y1, WallType type)
        {
            x /= GlobalConst.BrickSize;
            y /= GlobalConst.BrickSize;
            x1 /= GlobalConst.BrickSize;
            y1 /= GlobalConst.BrickSize;
            if (x > x1)
            {
                x += x1;
                x1 = x - x1;
                x -= x1;
            }
            if (y > y1)
            {
                y += y1;
                y1 = y - y1;
                y -= y1;
            }
            bool result = false;
            for (int i = x; i < x1; i++)
            {
                for (int j = y; j < y1; j++)
                {
                    if (Change(i * GlobalConst.BrickSize, j * GlobalConst.BrickSize, type)) result = true;
                }
            }
            return result;
        }
    }
}
