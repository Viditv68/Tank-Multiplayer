using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;

    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    private bool isDead = false;

    public Action<Health> OnDie;

    public override void OnNetworkSpawn()
    {
        if(!IsServer) 
            return;

        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int _damage)
    {
        ModifyHealth(-_damage);
    }

    public void RestoreHealth(int _healValue)
    {
        ModifyHealth(_healValue);
    }

    private void ModifyHealth(int _health)
    {
        if(isDead) 
            return;
    
        int newHealth = CurrentHealth.Value + _health;
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);

        if(CurrentHealth.Value == 0)
        {
            OnDie?.Invoke(this);
            isDead = true;
        }
    }
}
