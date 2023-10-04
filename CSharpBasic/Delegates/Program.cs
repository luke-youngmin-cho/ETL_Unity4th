namespace Delegates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayerUI playerUI = new PlayerUI();
            PlayerUI2 playerUI2 = new PlayerUI2();
            Player player = new Player(); //new Player(playerUI, playerUI2);

            // 대리자에 함수 참조 대입하기
            player.onHpChanged = playerUI.Refresh;

            // 대리자에 함수 구독하기
            player.onHpChanged += playerUI2.Refresh;
            player.onHpChanged += playerUI2.Refresh;
            player.onHpChanged += playerUI2.Refresh;
            player.onHpChanged += playerUI2.Refresh;
            player.onHpChanged += playerUI2.Refresh;

            // 대리자에 함수 구독취소하기
            player.onHpChanged -= playerUI2.Refresh;

            player.onHpChanged = playerUI.Refresh;

            while (true)
            {
                // todo -> 고블린과의 모의전투중... hp 깎일 예정
                //playerUI.Refresh(player.hp);
            }
        }
    }
}