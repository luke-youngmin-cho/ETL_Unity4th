using RPG.Singleton;

namespace RPG.DB
{
    public class Repositories : SingletonBase<Repositories>
    {
        public InventoryRepository inventory { get; private set; }


        protected override void Init()
        {
            base.Init();
            inventory = new InventoryRepository(GameDbContext.instance);
        }
    }
}