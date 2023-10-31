using Platformer.Controllers;

namespace Platformer.GameElements.Pool.Generic
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