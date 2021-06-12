using System;
using System.Collections.Generic;

namespace Tanks.Model
{
    /// <summary>
    /// Общий класс, в котором хранятся объекты игры
    /// </summary>
    public class ModelsGame
    {
        //объекты игры
        public static List<Bullet> listBullets = new List<Bullet>();             //снаряды
        public static GamerTank gamerTnk;                                        //танк игрока
        public static List<EnemyTank> listEnemyTnks = new List<EnemyTank>();     //танки врага
        public static Wall walls = new Wall();                                  //стены
        //
        //объекты для удаления
        public static List<Bullet> listRemoveBullets = new List<Bullet>();
        public static List<EnemyTank> listRemoveTanks = new List<EnemyTank>();
        //
        /// <summary>
        /// добавляются танки-враги, если их меньше, чем положено
        /// </summary>
        public static void AddEnemyTanks()
        {
            if (listEnemyTnks.Count < GlobalConst.CountEnemyTanks) 
            {
                Random rnd = new Random();
                int x=0;
                switch (rnd.Next(0,3))
                {
                    case 0: x = 0;
                        break;
                    case 1: x = GlobalConst.WindowWidth/2-GlobalConst.TankSize;
                        break;
                    case 2: x = GlobalConst.WindowWidth-GlobalConst.TankSize;
                        break;
                }
                ModelsGame.listEnemyTnks.Add(new EnemyTank(x, 0));
            }
        }
        //
        /// <summary>
        /// метод удаления уничтоженных снарядов из списка
        /// </summary>        
        public static void RemoveBullet() 
        {
            foreach (Bullet b in listRemoveBullets) 
            { 
                listBullets.Remove(b);
            }
            listRemoveBullets.Clear();
        }
        /// <summary>
        /// метод удаления уничтоженных танков из списка
        /// </summary>
        public static void RemoveTank()
        {
            foreach (EnemyTank t in listRemoveTanks)
            {
                listEnemyTnks.Remove(t);
            }
            listRemoveTanks.Clear();            
        }
        /// <summary>
        /// метод для удаление уничтоженных объектов
        /// </summary>
        public static void RemoveAll()
        {
            RemoveBullet();
            RemoveTank();
        }        
    }
}
