using System.Collections;
using System.Collections.Generic; 
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int healthPoints = 20; 
 
    [Header("Idle data")]
    public float idleTime;
    public float aggresionRange;

    [Header("Move data")]
    public float moveSpeed;
    public float chaseSpeed;
    public float turnSpeed;
    private bool manualMovement;
    private bool manualRotation; 

    [SerializeField] private Transform[] patrolPoints;
    private Vector3[] patrolPointsPosition; 

    private int currentPatrolIndex; 

    public bool inBattleMode {  get; private set; } 

    public Transform player {  get; private set; }  
    public Animator anim {  get; private set; } 
    public NavMeshAgent agent {  get; private set; }    

    

    
    public EnemyStateMachine stateMachine { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").GetComponent<Transform>();  
      
    }   

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }



    protected virtual void Update()
    {
        
    }

    protected bool ShouldEnterBattleMode()
    {
        bool inAggressionRange = Vector3.Distance(transform.position, player.position) < aggresionRange;
        
        if (inAggressionRange && !inBattleMode)
        {
            EnterBattleMode();
            return true;    
        }

        return false;
    }
    
    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }
    
    public virtual void GetHit()
    {
        EnterBattleMode(); 
        healthPoints--; 
    }

    public virtual void HitImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        StartCoroutine(HitImpactCourutine(force, hitPoint, rb));
    }

    private IEnumerator HitImpactCourutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(.1f); 
        rb.AddForceAtPosition(force, hitPoint,ForceMode.Impulse);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggresionRange);      
    }

    public void ActivateManualMovement(bool manualMovement) => this.manualMovement = manualMovement; 
    
    public bool ManualMovementActive() => manualMovement;

    public void ActivateManualRotation(bool manualRotation) => this.manualRotation = manualRotation;      
    public bool ManualRotationActive() => manualRotation;   


    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger(); 

    public virtual void AbilityTrigger()
    {
        stateMachine.currentState.AbilityTrigger();
    }
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPosition[currentPatrolIndex];
        Debug.Log(destination);

        currentPatrolIndex++;
        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0; 

        return destination;
    }
    private void InitializePatrolPoints()
    {
        patrolPointsPosition = new Vector3[patrolPoints.Length];   
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);    
        }
    }

    public void FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currentEulerAngels = transform.rotation.eulerAngles;
        float yRotation = Mathf.LerpAngle(currentEulerAngels.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);   

        transform.rotation = Quaternion.Euler(currentEulerAngels.x, yRotation, currentEulerAngels.z);
    }


}

  

