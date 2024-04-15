using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    public EnemyDataManager Instance;

    public EnemyData EnemyData;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHealth(int health)
    {
        EnemyData.EnemyHealth = health;
    }

    public int GetHealth()
    {
        return EnemyData.EnemyHealth;
    }

    public void ReduceHealth(int damage)
    {
        EnemyData.EnemyHealth -= damage;
    }
}
[Serializable]
public class EnemyData
{
    public int EnemyHealth;
}
