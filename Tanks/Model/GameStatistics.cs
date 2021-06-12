namespace Tanks.Model
{
    //
    /// <summary>
    /// класс для игровой статистики
    /// </summary>
    public class GameStatistics
    {
        public static int Score = 0;
        public static int GameLife = GlobalConst.GameLife;
        
        public delegate void GameOver();                     //создаеи делегат для события
        public static event GameOver onGameOver;             //создаем событие с типом GameOver 
        /// <summary>
        /// Метод добавляющий очки
        /// </summary>
        public static void AddScore() 
        {
            if (++Score % GlobalConst.CountBonusLife == 0) GameLife++;   //бонусы
        }
        /// <summary>
        /// Метод еменьшающий жизни
        /// </summary>
        public static void DeleteGameLife() 
        {
            if (--GameLife <=0) onGameOver();        //событие конец игры
        }
    }
}
