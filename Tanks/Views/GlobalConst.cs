using System.Drawing;

namespace Tanks
{
    /// <summary>
    /// глобальные параметры
    /// </summary>
    public class GlobalConst 
    {
        public static int WindowWidth = 800;      //размеры окна
        public static int WindowHight = 600;
        public static int WinStatisticsWidth = 200; //ширина области статистики

        public static int TimerInterval = 50;     //время между тактами (чем больше, тем медленее)
        public static int GameLife = 10;        //количество жизней
        public static int CountBonusLife = 5;   //количество танков, чтобы получить бонусную жизнь
        public static int CountInjury = 2;      //количество танков, через которое начнет расти количество ран у танка
        public static int CountEnemyTanks = 4;   //количество вражеских танков одновременно

        public static int TankSize = 80;              //размер танка
        public static int BrickSize = TankSize / 8;   //размер кирпича
        public static int PowerBreakWall = TankSize / BrickSize /4;//глубина рузрушения стены  (4-кол выстрелов разрушиь стену размером с танк)
        public static int MoveStep = 20;             //шаг перемещения
        public static int Definition = TankSize / 2;  //точность (попаданий)

        public static string Path = "pic\\";// "..\\..\\pic\\";                  //путь для файлов, ..- уровень выше
                                                                      //public static string ImageGamerTank = Path + "tank_v1G2.png";   //изображение танка игрока
        public static Image background = Image.FromFile(Path + "marble.jpg");      //фон
        
        public static Image[] PictureGamerTank = new Image[4];                    //массив картинок для танка игрока GamerTank
        //public static Dictionary<string, Image> PictureGamerTank;               
        public static Image[] PictureEnemyTank = new Image[4];                    //массив картинок для вражеского танка
        public static Image[] PictureBullet = new Image[3];                       //массив картинок для снаряда
        public static Image[] PictureWall = new Image[2];                         //картинки для стен
        public static Image[] PictureBase = new Image[2];                         //для штаба
        //
        //звук
        public static System.Media.SoundPlayer soundGameOver = new System.Media.SoundPlayer("sound\\goodbye.wav");
        public static System.Media.SoundPlayer SoundBump = new System.Media.SoundPlayer("sound\\vystr1.wav");
        public static System.Media.SoundPlayer SoundBullet = new System.Media.SoundPlayer("sound\\vystr_2.wav");
        public static System.Media.SoundPlayer SoundBackround = new System.Media.SoundPlayer("sound\\fon2.wav");
        public static void Init()
        {         
            //загрузка массив картинок для танка игрока GamerTank
            PictureGamerTank[0] = Image.FromFile(Path + "tank_v1G2_Up.png"); 
            PictureGamerTank[1] = Image.FromFile(Path + "tank_v1G2_Down.png");
            PictureGamerTank[2] = Image.FromFile(Path + "tank_v1G2_Left.png");
            PictureGamerTank[3] = Image.FromFile(Path + "tank_v1G2_Right.png");
            //
            //загрузка массив картинок для вражеского танка EnemyTank
            PictureEnemyTank[0] = Image.FromFile(Path + "tank_Enemy_Up.png");
            PictureEnemyTank[1] = Image.FromFile(Path + "tank_enemy_Down.png");
            PictureEnemyTank[2] = Image.FromFile(Path + "tank_Enemy_Left.png");
            PictureEnemyTank[3] = Image.FromFile(Path + "tank_Enemy_Right.png");
            //
            //загрузка картинок для снаряда
            PictureBullet[0] = Image.FromFile(Path + "bullet_0.png");   //снаряд игрока
            PictureBullet[1] = Image.FromFile(Path + "bullet_1.png");   //вражеский снаряд
            PictureBullet[2] = Image.FromFile(Path + "bullet_2.png");   //взрыв
            //
            //загрузка картинок для стен
            PictureWall[0] = Image.FromFile(Path + "brick_0.png");   //кирпич
            PictureWall[1] = Image.FromFile(Path + "brick_1.png");   //бетон
            //
            //загрузка картинок штаба
            PictureBase[0] = Image.FromFile(Path + "eagle3.png"); 
        }
    }
}
