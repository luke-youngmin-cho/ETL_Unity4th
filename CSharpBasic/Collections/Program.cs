using System.Collections;
using System.Collections.Generic;

namespace Collections
{
    enum ItemID
    {
        RedPotion = 20,
        BluePotion = 21,
    }

    interface IComparable<T>
    {
        int CompareTo(T other);
    }

    class SlotData : IComparable<SlotData>
    {
        public bool isEmpty => id == 0 && num == 0;

        public int id;
        public int num;

        public void DoSomething<T>()
        {
            Console.WriteLine(typeof(T).Name);
        }

        public void DoSomethingOfInt()
        {
            Console.WriteLine(typeof(int).Name);
        }

        public void DoSomethingOfFloat()
        {
            Console.WriteLine(typeof(float).Name);
        }

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
            #region Dynamic Array

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

            if (inventory2.Remove(new SlotData(0, 0)))
            {
                Console.WriteLine("지워질리가없는데 이게...");
            }

            for (int i = 0; i < 10; i++)
            {
                inventory2.Add(new SlotData(0, 0));
            }
            inventory2[0].DoSomething<SlotData>();
            inventory2[0].DoSomethingOfInt();
            inventory2[0].DoSomethingOfFloat();

            inventory2[0].DoSomething<int>();
            inventory2[0].DoSomething<float>();
            inventory2[0].DoSomething<double>();

            inventory2[0] = new SlotData((int)ItemID.RedPotion, 40);
            inventory2[1] = new SlotData((int)ItemID.BluePotion, 99);
            inventory2[2] = new SlotData((int)ItemID.BluePotion, 50);

            Console.WriteLine("인벤토리 정보 :");
            for (int i = 0; i < inventory2.Count; i++)
            {
                Console.WriteLine($"슬롯 {i} : [{(ItemID)(inventory2[i]).id}] , [{(inventory2[i]).num}]");
            }


            // todo -> 파란포션 5개를 획득
            // 1. 파란포션 5개가 들어갈 수 있는 슬롯을 찾아야함. 
            availableSlotIndex = inventory2.FindIndex(slotData => (slotData).isEmpty ||
                                                                     ((slotData).id == (int)ItemID.BluePotion &&
                                                                      (slotData).num <= 99 - 5));

            // 2. 해당 슬롯의 아이템 갯수에다가 내가 추가하려는 갯수를 더한 만큼의 수정예상값을 구함.
            expected = inventory2[availableSlotIndex].num + 5;

            // 3. 새로운 아이템 데이터를 만들어서 슬롯 데이터를 교체 해줌.
            newSlotData = new SlotData((int)ItemID.BluePotion, expected);
            inventory2[availableSlotIndex] = newSlotData;

            Console.WriteLine("인벤토리 정보 :");
            for (int i = 0; i < inventory2.Count; i++)
            {
                Console.WriteLine($"슬롯 {i} : [{(ItemID)(inventory2[i]).id}] , [{(inventory2[i]).num}]");
            }

            Console.WriteLine("for - each");
            // using 구문 : IDisposable 객체의 Dispose() 호출을 보장하는 구문
            using (IEnumerator<SlotData> enumerator = inventory2.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Console.WriteLine($"[{(ItemID)(enumerator.Current.id)}] , [{enumerator.Current.num}]");
                }
                enumerator.Reset();
            }

            foreach (var item in inventory2)
            {
                Console.WriteLine((ItemID)item.id);
                inventory2[0] = new SlotData(0, 0); // 이런거(순회중에 콜렉션 수정) 하믄 안댐 
            }

            // 둘 이상의 콜렉션 순회
            MyDynamicArray<SlotData> inventory1 = new MyDynamicArray<SlotData>();
            inventory1.Add(new SlotData(0, 0));
            inventory1.Add(new SlotData(0, 0));
            inventory1.Add(new SlotData(0, 0));

            using (IEnumerator<SlotData> e1 = inventory1.GetEnumerator())
            using (IEnumerator<SlotData> e2 = inventory2.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    // Something to do with e1.Current , e2.Current
                    Console.WriteLine("!!");
                }
                e1.Reset();
                e2.Reset();
            }


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

            ArrayList arrayList = new ArrayList();
            object name = "철수";

            arrayList.Add(name);
            arrayList.Add(1);
            arrayList.Remove("철수");
            arrayList.IndexOf(name);

            List<string> list = new List<string>();
            list.Add("철수");
            list.Remove("철수");
            list.Find(x => x == "철수");
            list.Add("영희");
            Console.WriteLine(list[0]);
            #endregion

            #region Queue
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(3); // 삽입
            if (queue.Peek() > 0) // 탐색
                queue.Dequeue(); // 삭제

            while (queue.Count > 0)
            {
                Console.WriteLine(queue.Dequeue());
            }
            #endregion

            #region Stack
            Stack<int> stack = new Stack<int>();
            stack.Push(3);
            if (stack.Peek() > 0)
                stack.Pop();
            #endregion

            #region Linked List
            MyLinkedList<int> linkedList = new MyLinkedList<int>();
            linkedList.AddFirst(3);
            linkedList.AddFirst(4);
            Console.WriteLine(linkedList.Find(x => x > 0).Value);
            linkedList.Find(1);
            linkedList.Find(x => x > 0);

            #endregion

            #region Hashtable
            MyHashtable<string, float> myHashtable = new MyHashtable<string, float>();
            myHashtable.Add("Luke", 90.0f);
            myHashtable.Add("Carm", 80.0f);

            foreach (var item in myHashtable)
            {
                Console.WriteLine($"{item.Key} 의 점수 : {item.Value}");
            }

            foreach (var item in myHashtable.Keys)
            {
                Console.WriteLine($"이름 : {item}");
            }

            foreach (var item in myHashtable.Values)
            {
                Console.WriteLine($"점수들 : {item}");
            }

            // C# 제공 Hashtable
            Hashtable hashtable = new Hashtable();
            hashtable.Add("철수", 90.0f);

            Dictionary<string, float> dictionary = new Dictionary<string, float>();
            dictionary.Add("Luke", 30.0f);

            foreach (var item in dictionary)
            {
                Console.WriteLine($"{item.Key}의 점수 : {item.Value}");
            }

            foreach (var item in dictionary.Keys)
            {
                Console.WriteLine($"이름 : {item}");
            }

           
            HashSet<int> set = new HashSet<int>();
            set.Add(1);
            set.Add(2);
            #endregion

        }
    }
}