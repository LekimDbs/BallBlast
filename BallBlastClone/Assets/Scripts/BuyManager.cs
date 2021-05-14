using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyManager : MonoBehaviour
{
    public static int Coins;

    public static float FireSpeed=3;
    public static float FireSpeedCost=1;
    public static float FirePower=1;
    public static float FirePowerCost=1;
    public static float CoinsPower=1;
    public static float CoinsPowerCost=1;

    public static BuyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadValues();
    }

    public void LoadValues() {
        BulletSpawner.Instance.BPS = FireSpeed;

    }
    public void AddCoins(int count)
    {
        Coins += count;
        UIManager.Instance.UpdateCoinsText();
    }
    public void UpgradeFireSpeed()
    {
        if (Coins >= FireSpeedCost)
        {
            FireSpeed += 1;
            BulletSpawner.Instance.BPS = FireSpeed;
            RemoveCoins((int)FireSpeedCost);
            SetFireSpeedCost();
            
        }
    }
    public void SetFireSpeedCost()
    {
        FireSpeedCost = (FireSpeed-2) * 1.5f;
        FireSpeedCost = (int)FireSpeedCost;
    }
    public void UpgradeFirePower()
    {
        if (Coins >= FirePowerCost)
        {
            FirePower += 0.1f;
            RemoveCoins((int)FirePowerCost);
            SetFirePowerCost();
        }
    }
    public void SetFirePowerCost()
    {
        FirePowerCost =(FirePower-1)*10*1.5f;
        FirePowerCost = (int)FirePowerCost;
    }
    public void UpgradeCoinsPower()
    {
        if (Coins >= CoinsPowerCost)
        {
            CoinsPower += 0.1f;
            RemoveCoins((int)CoinsPowerCost);
            SetCoinsPowerCost();
        }
    }
    public void SetCoinsPowerCost()
    {
        CoinsPowerCost = (CoinsPower - 1) * 10 * 1.5f;
        CoinsPowerCost = (int)CoinsPowerCost;
    }
    public void RemoveCoins(int count)
    {
        Coins -= count;
        UIManager.Instance.UpdateCoinsText();
    }
}
