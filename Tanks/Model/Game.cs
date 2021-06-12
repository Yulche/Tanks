using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;//

namespace Tanks.Model
{
    /// <summary>
    /// класс бизнес логики игры
    /// </summary>
    class Game                               //игровая модель
    {
        public Game() 
        {   
            //создаем танк с необходимыми кординатами
            ModelsGame.gamerTnk = new GamerTank(GlobalConst.WindowWidth / 2 - 2*GlobalConst.TankSize, GlobalConst.WindowHight - GlobalConst.TankSize);
            ModelsGame.listEnemyTnks.Add( new EnemyTank(0, 0));
            ModelsGame.listEnemyTnks.Add(new EnemyTank(GlobalConst.WindowWidth - GlobalConst.TankSize, 0));
            ModelsGame.listEnemyTnks.Add(new EnemyTank(GlobalConst.WindowWidth /2 - GlobalConst.TankSize, 0));
            //создаем стены
            ModelsGame.walls.Change(0, 100, 800, 400,WallType.Brick);
            ModelsGame.walls.Change(0, 400, 100, 500, WallType.Concrete);
            ModelsGame.walls.Change(700, 400, 800, 500, WallType.Concrete);
            //ModelsGame.walls.Change(350, 400, 450, 500, WallType.Concrete);
            //создаем штаб
            int x =( GlobalConst.WindowWidth - GlobalConst.TankSize)/ 2;
            int y = GlobalConst.WindowHight - GlobalConst.TankSize;
            ModelsGame.walls.Change(
                x,   //середина
                y,   //внизу
                x+ GlobalConst.TankSize/2,
                y+GlobalConst.TankSize,
                WallType.Base);
            //стены вокруг штаба
            ModelsGame.walls.Change(
                x - GlobalConst.TankSize / 2,
                y - GlobalConst.TankSize / 2,
                x ,
                GlobalConst.WindowHight,
                WallType.Brick);
            ModelsGame.walls.Change(
                x - GlobalConst.TankSize / 2,   
                y - GlobalConst.TankSize / 2,
                x + GlobalConst.TankSize * 3 / 2,
                y ,
                WallType.Brick);
            ModelsGame.walls.Change(
                x + GlobalConst.TankSize ,
                y - GlobalConst.TankSize / 2,
                x + GlobalConst.TankSize * 3 / 2,
                GlobalConst.WindowHight,
                WallType.Brick);
        }
    }
}
