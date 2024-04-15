using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public PlayerData playerData;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {

        playerData = new PlayerData(100, 100, 0, 100, false, 25, 200, false);
        /*
        playerData.playerCurrentHealth = 100;
        playerData.playerMaxHealth = 200;
        playerData.playerCurrentArmor = 0;
        playerData.playerMaxArmor = 200;
        playerData.hasArmor = false;
        playerData.playerMaxMana = 100;
        playerData.playerCurrentMana = 25;
        playerData.isHit = false;
        */

        audioSource = GetComponent<AudioSource>();

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerData = new PlayerData(100, 100, 0, 100, false, 25, 200, false);
        }
    }

    void Start()
    {
        if (GameManager.Instance.currentLevel == Enumerators.Levels.Level1)
        {
            playerData = new PlayerData(100, 100, 0, 100, false, 25, 200, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Una forma rápida de controlar la salud del jugador
        /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            setHasArmor();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ArmorRecovery(10);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GiveMana(50);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            setDamage(20);
        }*/
    }

    #region Metodos relacionados con la salud del jugador
    /// <summary>
    /// Recuperar el dato de salud del jugador
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        return playerData.playerCurrentHealth;
    }
    /// <summary>
    /// Modifica el dato salud del jugador
    /// </summary>
    /// <param name="newHealth"></param>
    public void SetHealth(int newHealth)
    {
        playerData.playerCurrentHealth = newHealth;
    }
    /// <summary>
    /// Suma al dato salud del jugador
    /// </summary>
    /// <param name="healthRec"></param>
    public void HealthRecovery(int healthRec)
    {
        playerData.playerCurrentHealth += healthRec;
        if (playerData.playerCurrentHealth > playerData.playerMaxHealth)
        {
            playerData.playerCurrentHealth = playerData.playerMaxHealth;
        }
        HUDManager.Instance.UpdateHealthNumber();
    }
    /// <summary>
    /// Aplicar daño al jugador
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(int damage)
    {
        SetCurrentHealthOnArmorState(damage);

        SetArmorStatus();

        if (playerData.playerCurrentArmor < 0)playerData.playerCurrentArmor = 0;
        
        HUDManager.Instance.UpdateHealthNumber();
        HUDManager.Instance.UpdateCurrentArmor();

    }
    /// <summary>
    /// Establece el estado del dato hasArmor para comprobar si el jugador tiene un valor de armadura superior a 0
    /// </summary>
    private void SetArmorStatus()
    {
        if (playerData.playerCurrentArmor <= 0)
        {
            playerData.hasArmor = false;
        }
        else if (playerData.playerCurrentArmor > 0)
        {
            playerData.hasArmor = true;
        }
    }
    /// <summary>
    /// Un calculo de daño segun la armadura del jugador
    /// </summary>
    /// <param name="damage"></param>
    private void SetCurrentHealthOnArmorState(int damage)
    {
        HUDManager.Instance.IsHit();
        IsHit();

        if (!playerData.hasArmor)
        {
            playerData.playerCurrentHealth -= damage;
        }
        else if (playerData.hasArmor)
        {
            playerData.playerCurrentHealth = playerData.playerCurrentHealth - (damage / 2);
            playerData.playerCurrentArmor -= damage;
        }
        if (playerData.playerCurrentHealth < 0)
        {
            playerData.playerCurrentHealth = 0;
        }
        else if (playerData.playerCurrentHealth == 0)
        {
            GameManager.Instance.SetStatus(Enumerators.GameStatus.GameOver);
        }
    }
    private void IsHit()
    {
        audioSource.enabled = true;
        StartCoroutine(ActivateAndDeactivateHitSound(0.9f));
    }
    public bool GetIsHit()
    {
        return playerData.isHit;
    }
    #endregion

    #region Metodos relacionados con el mana del jugador
    public int GetCurrentMana()
    {
        return playerData.playerCurrentMana;
    }
    /// <summary>
    /// Reduce el mana del jugador
    /// </summary>
    /// <param name="cost"></param>
    public void SetManaCost(int cost)
    {
        playerData.playerCurrentMana -= cost;
        if (playerData.playerCurrentMana < 0)
        {
            playerData.playerCurrentMana = 0;
        }
        else if (playerData.playerCurrentMana > playerData.playerMaxMana)
        {
            playerData.playerCurrentMana = playerData.playerMaxMana;
        }
        HUDManager.Instance.UpdateCurrentMana();
    }
    /// <summary>
    /// Comprobar si el jugador tiene suficiente mana
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool EnoughMana(int cost)
    {
        if (playerData.playerCurrentMana >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Recuperar mana
    /// </summary>
    /// <param name="num"></param>
    public void GiveMana(int num)
    {
        playerData.playerCurrentMana += num;
        HUDManager.Instance.UpdateCurrentMana();
    }
    #endregion

    #region Metodos relacionados con la armadura del jugador
    public int GetArmor()
    {
        return playerData.playerCurrentArmor;
    }
    public void setHasArmor()
    {
        if (playerData.hasArmor)
        {
            playerData.hasArmor = false;
        }
        else if (!playerData.hasArmor)
        {
            playerData.hasArmor = true;
        }
    }

    public void ArmorRecovery(int armor)
    {
        playerData.playerCurrentArmor += armor;
        if (!playerData.hasArmor)
        {
            playerData.hasArmor = true;
        }
        if (playerData.playerCurrentArmor > playerData.playerMaxArmor)
        {
            playerData.playerCurrentHealth = playerData.playerMaxHealth;
        }
        HUDManager.Instance.UpdateCurrentArmor();
    }
    #endregion
    IEnumerator ActivateAndDeactivateHitSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.enabled = false;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
[Serializable]
public class PlayerData
{
    public int playerCurrentHealth;
    public int playerMaxHealth;
    public int playerCurrentArmor;
    public int playerMaxArmor;
    public bool hasArmor;
    public int playerCurrentMana;
    public int playerMaxMana;
    public bool isHit;

    public PlayerData(int health, int maxHealth, int armor, int maxArmor, bool hasArmor, int mana, int maxMana, bool isHit)
    {
        this.playerCurrentHealth = health;
        this.playerMaxHealth = maxHealth;
        this.playerCurrentArmor = armor;
        this.playerMaxArmor = maxArmor;
        this.hasArmor = hasArmor;
        this.playerCurrentMana = mana;
        this.playerMaxMana = maxMana;
        this.isHit = isHit;
    }
}
