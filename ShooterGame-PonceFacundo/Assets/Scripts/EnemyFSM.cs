using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    public enum TypeEnemy
    {
        Ghost,
        Bomb
    }
    public TypeEnemy myType;

    [SerializeField]
    public enum States
    {
        Idle,
        Attack,
        GoBack
    }
    public States myCurrentState;

    [SerializeField][Tooltip("Only if type enemy is: BOMB")]
    public float timeUntilExplode;
    private float timer = 0;

    [SerializeField]
    [Range(5,20)]
    [Tooltip("Only if type enemy is: GHOST")]
    private float speedEnemy;

    [SerializeField] private float damageEnemy;

    public void CheckState()
    {
        switch (myCurrentState)
        {
            case States.Idle:
                MakeBehaviour(States.Idle, myType);
                break;
            case States.Attack:
                MakeBehaviour(States.Attack, myType);
                break;
            case States.GoBack:
                MakeBehaviour(States.GoBack, myType);
                break;
        }
    }
    public void MakeBehaviour(States whatState, TypeEnemy whatType)
    {
        switch (whatType)
        {
            case TypeEnemy.Ghost:


                break;
            case TypeEnemy.Bomb:

                if(whatState == States.Idle)
                {
                    transform.position = transform.position;
                }

                break;
        }
    }
}
