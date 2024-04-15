using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSelectorManager : MonoBehaviour
{
    public Enumerators.Spells selectedSpell;

    public GameObject[] weapons;

    public string[] unlockedSpells;
    private void Awake()
    {
        selectedSpell = Enumerators.Spells.Missile;

        weapons[1].SetActive(true);

        unlockedSpells = new string[weapons.Length];

        //unlockSpell(0, "Test");
        unlockSpell(1, "Missile");
        unlockSpell(2, "Ice");
        unlockSpell(3, "Fire");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Obtener el valor de la tecla pulsada como un string, más facil de tratar a traves de un switch
        string _sKey = Input.inputString;

        //Una simple comprobacion de que la tecla pulsada es un numero
        if (int.TryParse(_sKey, out int num))
        {
            if (num >= 0 && num < weapons.Length)
            {
                foreach (GameObject weapon in weapons)
                {
                    weapon.SetActive(false);
                }

                switch (num)
                {
                    case 1:
                        selectedSpell = Enumerators.Spells.Missile;
                        if (isUnlocked(1, "Missile"))
                        {
                            weapons[1].SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Hechizo no desbloqueado");
                        }
                        break;
                    case 2:
                        selectedSpell = Enumerators.Spells.Ice;
                        if (isUnlocked(2, "Ice"))
                        {
                            weapons[2].SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Hechizo no desbloqueado");
                        }
                        break;
                    case 3:
                        selectedSpell = Enumerators.Spells.Fire;
                        if (isUnlocked(3, "Fire"))
                        {
                            weapons[3].SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Hechizo no desbloqueado");
                        }
                        break;
                    case 4:
                        selectedSpell = Enumerators.Spells.Lightning;
                        if (isUnlocked(4, "Lightning"))
                        {
                            weapons[4].SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Hechizo no desbloqueado");
                        }
                        break;
                    case 5:
                        selectedSpell = Enumerators.Spells.Illusion;
                        if (isUnlocked(5, "Illusion"))
                        {
                            weapons[5].SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Hechizo no desbloqueado");
                        }
                        break;
                    case 6:
                        selectedSpell = Enumerators.Spells.TransforSpell;
                        if (isUnlocked(6, "Transform"))
                        {
                            weapons[6].SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Hechizo no desbloqueado");
                        }
                        break;
                    case 0:
                        selectedSpell = Enumerators.Spells.Test;
                        if (isUnlocked(0, "Test"))
                        {
                            weapons[0].SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Hechizo no desbloqueado");
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Debug.Log("No hay tantos hechizos");
            }
        }
    }
    public void unlockSpell(int arrayIndex, string spell)
    {
        if (arrayIndex >= 0 && arrayIndex < unlockedSpells.Length)
        {
            unlockedSpells[arrayIndex] = spell;
        }
    }
    public bool isUnlocked(int arrayIndex, string spell)
    {
        if (unlockedSpells[arrayIndex] == spell)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
