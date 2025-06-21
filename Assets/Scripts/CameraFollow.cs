using UnityEngine;
using Unity.Netcode;

public class CameraFollow : NetworkBehaviour
{
    public Transform target;
    public float distance = 4f;
    public float height = 2f;
    public LayerMask wallMask;

    private void LateUpdate()
    {
        if (!IsOwner || target == null) return;

        Vector3 desiredPos = target.position - target.forward * distance + Vector3.up * height;
        Vector3 lookPos = target.position + Vector3.up;

        if (Physics.Linecast(lookPos, desiredPos, out RaycastHit hit, wallMask))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = desiredPos;
        }

        transform.LookAt(lookPos);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            
            target = transform.parent;
            transform.SetParent(null);
        }
        else
        {
            gameObject.SetActive(false); 
        }
    }
}
