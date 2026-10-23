using System.Collections.Generic;
using UnityEngine;

public sealed class FlockManager : MonoBehaviour
{
    [SerializeField] private Boid boidPrefab;
    [SerializeField] private int boidCount = 150;
    [SerializeField] private float boidSpawnRadius = 8f;

    [SerializeField] private float sepWeight = 1.6f;
    [SerializeField] private float aliWeight = 1f;
    [SerializeField] private float cohWeight = 0.9f;

    [SerializeField] private float boundsRadius = 18f;
    [SerializeField, Min(0)] private float boundsWeight = 1f;

    [SerializeField] private float lookAhead = 2.5f;
    [SerializeField, Min(0)] private float obstacleWeight = 2f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Obstacle Spawn Infos")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int obstacleCount = 10;
    [SerializeField] private int obstacleMaxSize = 10;
    [SerializeField] private float obstacleSpawnRadius = 8f;

    private Vector3 Center => transform.position;

    public readonly List<Boid> boids = new();

    private void Awake()
    {
        SpawnBoids();
        SpawnObstacles();
    }

    private void SpawnBoids()
    {
        if (boidPrefab == null)
            return;

        for (int i = 0; i < boidCount; i++)
        {
            Vector3 pos = Center + Random.insideUnitSphere * boidSpawnRadius;
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            Boid boid = Instantiate(boidPrefab, pos, rotation);

            boid.Flock = this;
            boids.Add(boid);
        }
    }

    private void SpawnObstacles()
    {
        if (obstaclePrefab == null)
            return;

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 pos = Center + Random.insideUnitSphere * obstacleSpawnRadius;
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            GameObject obstacle = Instantiate(obstaclePrefab, pos, rotation);

            float scale = Random.Range(1f, Mathf.Max(1, obstacleMaxSize));
            obstacle.transform.localScale = Vector3.one * scale;
        }
    }

    public Vector3 ComputeForce(Boid boid, Collider[] near, int count)
    {
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 cohesionCenter = Vector3.zero;
        int seen = 0;

        for (int i = 0; i < count; i++)
        {
            if (count >= near.Length)
                break;

            Collider col = near[i];
            if (col?.attachedRigidbody == null || col.gameObject == boid.gameObject)
                continue;

            Vector3 toMe = boid.transform.position
                         - col.transform.position;

            if (toMe.sqrMagnitude > 0.0001f)
                separation += toMe / toMe.sqrMagnitude;

            alignment += col.attachedRigidbody.linearVelocity;
            cohesionCenter += col.transform.position;
            seen++;
        }

        Vector3 totalForce = StayInBounds(boid) * boundsWeight;
        totalForce += AvoidObstacle(boid) * obstacleWeight;

        if (seen > 0)
        {
            alignment /= seen;
            cohesionCenter = cohesionCenter / seen - boid.transform.position;
   
            Vector3 separationForce = separation * boid.MaxSpeed;
            Vector3 alignmentForce = alignment.normalized * boid.MaxSpeed;
            Vector3 cohesionForce = cohesionCenter.normalized * boid.MaxSpeed;
            
            if (separationForce.sqrMagnitude > 0.0001)
                totalForce += (separationForce - boid.Velocity) * sepWeight;
            
            if (alignmentForce.sqrMagnitude > 0.0001)
                totalForce += (alignmentForce - boid.Velocity) * aliWeight;

            if (cohesionForce.sqrMagnitude > 0.0001)
                totalForce += (cohesionForce - boid.Velocity) * cohWeight;
        }

        return Vector3.ClampMagnitude(totalForce, boid.MaxForce);
    }

    public Vector3 StayInBounds(Boid boid)
    {
        Vector3 toCenter = Center
            - boid.transform.position;

        if (toCenter.sqrMagnitude < boundsRadius * boundsRadius)
            return Vector3.zero;

        return toCenter.normalized * boid.MaxSpeed
             - boid.Velocity;
    }

    // desvia de obstáculo à frente
    public Vector3 AvoidObstacle(Boid boid)
    {
        Vector3 dir = boid.Velocity.normalized;

        if (!Physics.SphereCast(boid.transform.position, 0.4f, dir, out RaycastHit hit, lookAhead, obstacleMask))
            return Vector3.zero;

        Vector3 away = Vector3.Reflect(dir, hit.normal);
        return (away - boid.Velocity).normalized
             * boid.MaxForce;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, boidSpawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, boundsRadius);

    }
}

