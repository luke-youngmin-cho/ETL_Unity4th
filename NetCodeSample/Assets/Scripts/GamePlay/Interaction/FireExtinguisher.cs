using Unity.Netcode;

namespace CopycatOverCooked.GamePlay
{
    public class FireExtinguisher : Pickable
    {
        public override void Use(NetworkObject user)
        {
            // todo -> 분사하기
        }

        [ServerRpc]
        private void ExtinguishFireServerRpc()
        {
            // todo -> deplete hp of fire
        }

        [ClientRpc]
        private void ShotEffectClientRpc()
        {
            // todo -> Play particle system
        }
    }
}
