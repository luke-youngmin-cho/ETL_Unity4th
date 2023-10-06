namespace Collections
{
    enum ItemID
    {
        RedPotion = 20,
        BluePotion = 21,
    }

    class SlotData : IComparable<SlotData>
    {
        public bool isEmpty => id == 0 && num == 0;

        public int id;
        public int num;

        public SlotData(int id, int num)
        {
            this.id = id;
            this.num = num;
        }

        public int CompareTo(SlotData? other)
        {
            return this.id == other?.id && this.num == other.num ? 0 : -1;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MyDynamicArray inventory = new MyDynamicArray();

            for (int i = 0; i < 10; i++)
            {
                inventory.Add(new SlotData(0, 0));                
            }

            inventory[0] = new SlotData((int)ItemID.RedPotion, 40);
            inventory[1] = new SlotData((int)ItemID.BluePotion, 99);
            inventory[2] = new SlotData((int)ItemID.BluePotion, 50);

            Console.WriteLine("인벤토리 정보 :");
            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine($"슬롯 {i} : [{(ItemID)((SlotData)inventory[i]).id}] , [{((SlotData)inventory[i]).num}]");
            }

            
            // todo -> 파란포션 5개를 획득
            // 1. 파란포션 5개가 들어갈 수 있는 슬롯을 찾아야함. 
            int availableSlotIndex = inventory.FindIndex(slotData => ((SlotData)slotData).isEmpty ||
                                                                     (((SlotData)slotData).id == (int)ItemID.BluePotion &&
                                                                      ((SlotData)slotData).num <= 99 - 5));

            // 2. 해당 슬롯의 아이템 갯수에다가 내가 추가하려는 갯수를 더한 만큼의 수정예상값을 구함.
            int expected = ((SlotData)inventory[availableSlotIndex]).num + 5;

            // 3. 새로운 아이템 데이터를 만들어서 슬롯 데이터를 교체 해줌.
            SlotData newSlotData = new SlotData((int)ItemID.BluePotion, expected);
            inventory[availableSlotIndex] = newSlotData;

            Console.WriteLine("인벤토리 정보 :");
            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine($"슬롯 {i} : [{(ItemID)((SlotData)inventory[i]).id}] , [{((SlotData)inventory[i]).num}]");
            }


            MyDynamicArray<SlotData> inventory2 = new MyDynamicArray<SlotData>();

            // 숙제 : MyDynamicArray 기반으로 만들었던 예제를 inventory2 가지고 구현해보기

            /*
            inventory[1] = "철수";
            Console.WriteLine($"동적배열의 0번째 인덱스 값 : {inventory[0]}");

            int idx = inventory.FindIndex(x => (string)x == "철수");
            idx = inventory.FindIndex(x => (int)x > 3);

            int 파란포션5개추가가능한인덱스 = inventory.FindIndex(x => x.isEmpty == true ||
                                                                (x.id == 파란포션아이디 &&
                                                                 x.num <= 파란포션최대갯수 - 5))
            int 수정될갯수 = inventory[파란포션5개추가가능한인덱스].num + 5;
            슬롯데이터 수정될슬롯데이터 = new 슬롯데이터(파란포션아이디, 수정될갯수);
            inventory[파란포션5개추가가능한인덱스] = 수정될슬롯데이터;
            */
        }
    }
}