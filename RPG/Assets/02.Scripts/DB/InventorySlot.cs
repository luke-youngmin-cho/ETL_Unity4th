using System.ComponentModel.DataAnnotations.Schema;

namespace RPG.DB
{
    [Table("InventorySlot")]
    public class InventorySlot
    {
        [Column("slotID")]
        public int slotID { get; set; }
        [Column("itemID")]
        public int itemID { get; set; }
        [Column("itemNum")]
        public int itemNum { get; set; }
    }
}