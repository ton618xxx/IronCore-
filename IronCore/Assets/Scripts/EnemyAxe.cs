using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    public Rigidbody rb;
    public Transform axeVisial;
    public Transform player;
    public float flySpeed;
    public float rotationSpeed;

    public Vector3 direction;

    private void Update()
    {
       axeVisial.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
       direction = player.position + Vector3.up - transform.position; 
       rb.linearVelocity = direction.normalized * flySpeed;

       transform.forward = rb.linearVelocity;

    }
}
