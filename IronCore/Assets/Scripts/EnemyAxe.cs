using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    public Rigidbody rb;
    public Transform axeVisial;
    public Transform player;
    public float flySpeed;
    public float rotationSpeed;

    public Vector3 direction;

    private float timer = 1;


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






