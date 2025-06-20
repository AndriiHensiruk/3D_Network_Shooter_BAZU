using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private int damage;

    public void Init(int dmg)
    {
        damage = dmg;
        Invoke(nameof(DestroySelf), 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.ApplyDamageServerRpc(damage);
        }

        DestroySelf();
    }

    void DestroySelf()
    {
        if (IsServer)
            NetworkObject.Despawn();
    }
}
