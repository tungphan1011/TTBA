using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public int currVal;
    public int maxVal;    

    public Stat(int curr, int max)
    {
        currVal = curr;
        maxVal = max;       
    }

    internal void Subtract(int amount)
    {
        currVal -= amount;

        if (currVal < 0) { currVal = 0; }
    }

    internal void Add(int amount)
    {
        currVal += amount;

        if (currVal > maxVal) { currVal = maxVal; }
    }

    internal void SetToMax()
    {
        currVal = maxVal;
    }
}

public class Character : MonoBehaviour, IDamageable
{
    public Stat hp;
    [SerializeField] StatusBar hpBar;
    public Stat stamina;
    [SerializeField] StatusBar staminaBar;

    public bool isDead;
    public bool isExhausted;

    DisableControls disableControls;
    PlayerRespawn playerRespawn;

    private void Awake()
    {
        disableControls = GetComponent<DisableControls>();
        playerRespawn = GetComponent<PlayerRespawn>();
    }

    private void Start()
    {
        LoadPlayerStats();
        UpdateHPBar();
        UpdateStaminaBar();
        StartCoroutine(RegenerateStamina());
        StartCoroutine(RegenerateHP());
    }

    private void UpdateHPBar()
    {
        hpBar.Set(hp.currVal, hp.maxVal);
    }

    private void UpdateStaminaBar()
    {
        staminaBar.Set(stamina.currVal, stamina.maxVal);
    }

    public void TakeDamage(int amount)
    {
        if (isDead == true) { return; }
        hp.Subtract(amount);
        if (hp.currVal <= 0)
        {
            Dead();
        }
        UpdateHPBar();
        SavePlayerStats();
    }

    private void Dead()
    {
        isDead = true;
        disableControls.DisableControl();
        playerRespawn.StartRespawn();
    }

    public void Heal(int amount)
    {
        hp.Add(amount);
        UpdateHPBar();
        SavePlayerStats();
    }

    public void FullHeal()
    {
        hp.SetToMax();
        UpdateHPBar();
        SavePlayerStats();
    }

    public void GetTired(int amount)
    {
        stamina.Subtract(amount);
        if (stamina.currVal <= 0)
        {
            Exhausted();
        }
        UpdateStaminaBar();
        SavePlayerStats();
    }

    private void Exhausted()
    {
        isExhausted = true;
        disableControls.DisableControl();
        StartCoroutine(ExhaustedRecoveryDelay());
    }

    private IEnumerator ExhaustedRecoveryDelay()
    {
        yield return new WaitForSeconds(5f); // Player cannot act for 5 seconds
        isExhausted = false;
        disableControls.EnableControl();
    }

    public void Rest(int amount)
    {
        stamina.Add(amount);
        UpdateStaminaBar();
        SavePlayerStats();
    }


    public void FullRest(int amount)
    {
        stamina.SetToMax();
        UpdateStaminaBar();
        SavePlayerStats();
    }

    private IEnumerator RegenerateStamina()
    {
        while (true)
        {
            float delay = CalculateRegenerationDelay();
            yield return new WaitForSeconds(delay);
            if (stamina.currVal < stamina.maxVal)
            {
                int recoveryAmount = CalculateRecoveryAmount();
                stamina.Add(recoveryAmount); // Adjust the amount as needed
                if (isExhausted && stamina.currVal > 0)
                {
                    ExhaustedRecoveryDelay();
                }
                UpdateStaminaBar();
                SavePlayerStats();
            }
        }
    }

    private float CalculateRegenerationDelay()
    {
        if (stamina.currVal <= 0)
        {
            return 0.5f; // Faster regeneration when stamina is 0
        }
        else if (stamina.currVal <= stamina.maxVal / 2)
        {
            return 1f; // Medium regeneration rate when stamina is below half
        }
        else
        {
            return 2f; // Slower regeneration when stamina is above half
        }
    }

    private int CalculateRecoveryAmount()
    {
        if (stamina.currVal <= stamina.maxVal / 2)
        {
            return 5; // Recover 2 stamina when stamina is below half
        }
        else
        {
            return 2; // Recover 1 stamina when stamina is above half
        }
    }

    private IEnumerator RegenerateHP()
    {
        while (true)
        {
            yield return new WaitForSeconds(hpRegenDelay());
            if (!isDead && hp.currVal < hp.maxVal)
            {
                hp.Add(hpRegenAmount());
                UpdateHPBar();
                SavePlayerStats();
            }
        }
    }

    private float hpRegenDelay()
    {
        if (hp.currVal <= 0)
        {
            return 0.5f; // Faster regeneration when hp is 0
        }
        else if (hp.currVal <= hp.maxVal / 2)
        {
            return 1f; // Medium regeneration rate when hp is below half
        }
        else
        {
            return 2f; // Slower regeneration when hp is above half
        }
    }

    private int hpRegenAmount()
    {
        if (hp.currVal <= hp.maxVal / 2)
        {
            return 5; // Recover 2 hp when hp is below half
        }
        else
        {
            return 2; // Recover 1 hp when hp is above half
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            TakeDamage(10);
            SavePlayerStats();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Heal(10);
            SavePlayerStats();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetTired(10);
            SavePlayerStats();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Rest(10);
            SavePlayerStats();
        }
    }

    public void CalculateDamage(ref int damage)
    {
        
    }

    public void ApplyDamage(int damage)
    {
        TakeDamage(damage);
    }

    public void CheckState()
    {
        
    }

    private void SavePlayerStats()
    {
        PlayerPrefs.SetInt("PlayerHP", hp.currVal);
        PlayerPrefs.SetInt("PlayerStamina", stamina.currVal);
        PlayerPrefs.Save();
    }

    private void LoadPlayerStats()
    {
        if (PlayerPrefs.HasKey("PlayerHP"))
        {
            hp.currVal = PlayerPrefs.GetInt("PlayerHP");
        }
        if (PlayerPrefs.HasKey("PlayerStamina"))
        {
            stamina.currVal = PlayerPrefs.GetInt("PlayerStamina");
        }
    }
}
