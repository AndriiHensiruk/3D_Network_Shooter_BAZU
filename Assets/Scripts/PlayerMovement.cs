using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController ctrl;

    void Start()
    {
        ctrl = GetComponent<CharacterController>();
        if (!IsOwner) Destroy(GetComponentInChildren<Camera>().gameObject);
    }

    void Update()
    {
        if (!IsOwner) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        ctrl.Move(move * moveSpeed * Time.deltaTime);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 1, -5);
            Camera.main.transform.localEulerAngles = Vector3.zero;
        }
    }
}
