using RPG.Singleton;

namespace RPG.DB
{
    public class Repositories : SingletonBase<Repositories>
    {
        public InventoryRepository inventory { get; private set; }

        public void SaveChanges()
        {
            GameDbContext.instance.SaveChanges();
        }


        protected override void Init()
        {
            base.Init();
            inventory = new InventoryRepository(GameDbContext.instance);
        }
    }
}