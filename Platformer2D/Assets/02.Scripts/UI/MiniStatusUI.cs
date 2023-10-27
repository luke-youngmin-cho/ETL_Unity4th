using System;
using Platformer.Stats;
using UnityEngine;
using UnityEngine.UI;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.UI
{
    public class MiniStatusUI : MonoBehaviour
    {
        [SerializeField] private Slider _hpBar;

        private void Start()
        {
            IHp hp = transform.root.GetComponent<IHp>();
            _hpBar.minValue = hp.hpMin;
            _hpBar.maxValue = hp.hpMax;
            _hpBar.value = hp.hpValue;

            hp.onHpChanged += (value) => _hpBar.value = value;

            // is 키워드 
            // 객체가 어떤 타입으로 참조할 수 있는지 확인하고 bool 결과를 반환하는 키워드
            if (hp is CharacterController)
            {
                Vector3 originScale = transform.localScale;
                ((CharacterController)hp).onDirectionChanged += (value) =>
                {
                    transform.localScale = value < 0 ?
                        new Vector3(-originScale.x, originScale.y, originScale.z) :
                        new Vector3(+originScale.x, originScale.y, originScale.z);
                };
            }
        }
    }
}

// is as 예시
class A
{

}

class B : A
{
    public int id;
}

class IsAsTest
{
    void Test(A a)
    {
        // is 는 bool 체크 한번만 하면 되고 , as 는 주소를 저장할 지역변수가 필요하면서 null 체크를 해야하므로 
        // 아래 상황에서는 보통 is 를 주로 씀
        // as 는 null 체크 따로 안해도 되면서(타입 보장된상황) 멤버변수에 결과를 대입해두어야 할때 더 용이함

        if (a is B)
        {
            Console.WriteLine(((B)a).id); 
        }

        B b = a as B; // as 키워드 : 캐스팅 실패시 null 반환, 성공시 해당 객체 참조 반환
        if (b != null)
        {
            Console.WriteLine(b.id);
        }

        Console.WriteLine(b?.id);

        Type type = typeof(B); // B 라는 타입에 대한 정보를 가진 객체의 참조를 반환
    }
}


