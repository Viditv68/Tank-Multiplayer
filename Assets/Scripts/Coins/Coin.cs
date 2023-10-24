using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    protected int coinValue = 10;
    protected bool alreadyCollected;

    public abstract int Collect();

    public void SetValue(int _value)
    {
        coinValue = _value;
    }

    protected void Show(bool _show)
    {
        spriteRenderer.enabled = _show;

    }
}
