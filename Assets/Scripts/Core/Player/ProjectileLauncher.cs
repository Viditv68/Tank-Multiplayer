using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private CoinWallet coinWallet;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 2f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float muzzeFlashDuration = 2f;
    [SerializeField] private int costToFire = 2;

    private bool shouldFire;
    private float timer;
    private float muzzleFlashTimer = 2f;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) 
            return;

        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) 
            return;

        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }


    private void HandlePrimaryFire(bool _shouldFire)
    {
        shouldFire = _shouldFire;
    }

    void Update()
    {

        if(muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;
            if(muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
            }
        }


        if (!IsOwner)
            return;

        if (timer  > 0)
        {
            timer -= Time.deltaTime;
        }

        if (!shouldFire)
            return;

        if (timer > 0)
            return;

        if (coinWallet.totalCoins.Value < costToFire)
            return;


        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);

        timer = 1/fireRate;

    }


    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 _spawnPos, Vector3 _direction)
    {
        if (coinWallet.totalCoins.Value < costToFire)
            return;

        coinWallet.SpendCoins(costToFire);

        GameObject projectileInstance = Instantiate(serverProjectilePrefab, _spawnPos, Quaternion.identity);

        projectileInstance.transform.up = _direction;

        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }

        SpawnDummyProjectileClientRpc(_spawnPos, _direction);

    }



    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 _spawnPos, Vector3 _direction)
    {
        if (IsOwner)
            return;

        SpawnDummyProjectile(_spawnPos, _direction);
    }


    private void SpawnDummyProjectile(Vector3 _spawnPos, Vector3 _direction)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzeFlashDuration;
        GameObject projectileInstance = Instantiate(clientProjectilePrefab, _spawnPos, Quaternion.identity);

        projectileInstance.transform.up = _direction;

        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }
}
