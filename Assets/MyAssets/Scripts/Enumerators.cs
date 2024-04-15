using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enumerators : MonoBehaviour
{
    public enum GameStatus
    {
        None,
        MainMenu,
        Running,
        Pause,
        EndLevel,
        GameOver
    }

    public enum PlayerStatus
    {
        Idle,
        Moving,
        Running,
        Dead
    }

    public enum EnemyStatus
    {
        Idle,
        Moving,
        PlayerDetected,
        Figthing,
        Shooting,
        Hurt,
        Dead
    }

    public enum Spells
    {
        Test,
        Missile,
        Ice,
        Fire,
        Lightning,
        Illusion,
        TransforSpell
    }
    public enum Levels
    {
        Menu,
        Level1,
        Level2
    }
}
