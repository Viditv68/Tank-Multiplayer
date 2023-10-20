using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId;

    public void SetOwner(ulong _ownerClientId)
    { 
        ownerClientId = _ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.attachedRigidbody == null)
            return;

        if(_collision.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (netObj.OwnerClientId == ownerClientId)
                return;
        }

        if(_collision.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
