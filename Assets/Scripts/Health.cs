using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> health = new NetworkVariable<int>(100);

    [ServerRpc(RequireOwnership = false)]
    public void ApplyDamageServerRpc(int dmg)
    {
        if (health.Value <= 0) return;

        health.Value -= dmg;
        if (health.Value <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (IsServer)
            NetworkObject.Despawn();
    }
}
