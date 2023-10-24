using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> totalCoins= new NetworkVariable<int>();

    public void SpendCoins(int _costToFire)
    {
        totalCoins.Value -= _costToFire;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(!_collision.TryGetComponent<Coin>(out Coin coin))
            return;

        int coinValue = coin.Collect();
        if (!IsServer)
            return;

        totalCoins.Value += coinValue;

    }
}
