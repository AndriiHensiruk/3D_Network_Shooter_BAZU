using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private int damage;
    public GameObject hitEffectPrefab;

    public void Init(int dmg)
    {
        damage = dmg;
        Invoke(nameof(DestroySelf), 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (hitEffectPrefab != null)
        {
            GameObject vfx = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 1f); 
        }

        Health hp = other.GetComponent<Health>();
        if (hp != null)
            hp.ApplyDamageServerRpc(damage);

            DestroySelf();
    }

    void DestroySelf()
    {
        if (IsServer)
            NetworkObject.Despawn();
    }
}
