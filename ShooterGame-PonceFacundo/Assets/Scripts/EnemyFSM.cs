using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    public enum STATES
    {
        Idle,
        Attack,
        GoBack
    }
    public STATES myState;
    [SerializeField]
    public enum TypeEnemy
    {
        Ghost,
        Bomb
    }
    public TypeEnemy myType;


}
