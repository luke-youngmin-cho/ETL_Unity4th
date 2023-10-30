using Platformer.Controllers;

namespace Platformer.GameElements
{
    public class PoolOfEnemyController : GameObjectPool<EnemyController>
    {
        protected override void OnGetFromPool(EnemyController item)
        {
            base.OnGetFromPool(item);
            item.SetUp();
        }
    }
}