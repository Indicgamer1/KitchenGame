using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateRemoved;
    public event EventHandler OnPlateSpawned;
    [SerializeField] private KitchenObjectsSO plateKitchenObjectSO;
    private float spawnplateTimer;
    private float spawnTimerMax = 4f;
    private int platesSpawnedAmount = 0;
    private int platesSpawnedAmountMax = 4;
    private void Update()
    {
        spawnplateTimer += Time.deltaTime;
        if (spawnplateTimer > spawnTimerMax)
        {
            spawnplateTimer = 0f;

            if (KitchenGameManager.Instance.IsGamePlaying()  && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
