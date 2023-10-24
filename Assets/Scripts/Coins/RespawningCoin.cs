using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollecetd;
    private Vector3 prevoiusPosition;
    private void Update()
    {
        if (prevoiusPosition != transform.position)
            Show(true);

        prevoiusPosition = transform.position;
    }

    public override int Collect()
    {
        if(!IsServer)
        {
            Show(false);
            return 0;
        }

        if (alreadyCollected)
            return 0;
        
        alreadyCollected = true;

        OnCollecetd?.Invoke(this);
        return coinValue;

    }

    public void Reset()
    {
        alreadyCollected = false;
    }
}
