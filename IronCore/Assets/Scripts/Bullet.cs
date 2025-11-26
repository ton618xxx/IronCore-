using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BoxCollider cd;
    private Rigidbody rb;
    private TrailRenderer trailRenderer;
    private MeshRenderer meshRenderer;

    [SerializeField] private GameObject bulletImpactFX;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;


    private void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void BulletSetup(float flyDistance)
    {
        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = .25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + .5f; //magic number .5f is a length of tip of the laser (Check Method UpdateAimVisual() on PlayerAim script) ; 
    }

    private void Update()
    {
        FadeTrailIfNeeded();
        DisableBulletIfNeeded();
        ReturnToPoolIfNeeded();
    }
    private void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0)
            ReturnBulletToPool();   
    }

    private void ReturnBulletToPool() => ObjectPool.instance.ReturnObject(gameObject);   


    private void DisableBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    private void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
            trailRenderer.time -= 2 * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        if (enemy != null)
            enemy.GetHit(); 

        CreateImpactFx(collision);
        ReturnBulletToPool();   

    }

    private void CreateImpactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newImpactFx = ObjectPool.instance.GetObject(bulletImpactFX);
            newImpactFx.transform.position = contact.point;

            ObjectPool.instance.ReturnObject(newImpactFx, 1);   
        }
    }
}
          









