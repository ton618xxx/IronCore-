using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform axeVisial;


    private Vector3 direction;
    private Transform player;
    private float flySpeed;
    private float rotationSpeed;
    private float timer = 1;

    public void AxeSetup(float flySpeed, Transform player, float timer)
    {
        this.flySpeed = flySpeed;
        this.player = player;   
        this.timer = timer;
    }


    private void Update()
    {
        axeVisial.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        timer -= Time.deltaTime;

        if (timer > 0)
            direction = player.position + Vector3.up - transform.position;

        rb.linearVelocity = direction.normalized * flySpeed;

        transform.forward = rb.linearVelocity;


    }
}






