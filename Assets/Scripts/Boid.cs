using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float maxForce = 24f;
    [SerializeField] private float perception = 2.5f;
    [SerializeField] private LayerMask boidMask;

    public FlockManager Flock { get; set; }
    public Vector3 Velocity { get; private set; }
    public float MaxSpeed => maxSpeed;
    public float MaxForce => maxForce;

    private Rigidbody rb;
    private readonly Collider[] near = new Collider[32];

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, perception, near, boidMask);
        Vector3 force = Flock.ComputeForce(this, near, count);
        Velocity = Vector3.ClampMagnitude(Velocity + force * Time.fixedDeltaTime, maxSpeed);

        rb.linearVelocity = Velocity;

        if (Velocity.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(Velocity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, perception);
    }
}