using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] private float lookAhead = 2.5f;
    [SerializeField] private LayerMask obstacleMask;

    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int obstacleCount = 10;
    [SerializeField] private int obstacleMaxSize = 10;
    [SerializeField] private float obstacleSpawnRadius = 8f;

    private Vector3 center => transform.position;

    public readonly List<Boid> boids = new();

    private void Awake()
    {
        // Instancia n° boids em uma determinada área.
        for (int i = 0; i < boidCount; i++)
        {
            Vector3 pos = transform.position 
                + Random.insideUnitSphere * boidSpawnRadius;

            Boid boid = Instantiate(boidPrefab, pos, Quaternion.Euler(0f, Random.value * 360f, 0f));

            boid.Flock = this;
            boids.Add(boid);
        }

        // Instancia n° obstacles em uma determinada área.
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 pos = transform.position
                + Random.insideUnitSphere * obstacleSpawnRadius;

            GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.Euler(0f, Random.value * 360f, 0f));
            obstacle.transform.localScale = Vector3.one * Random.Range(1, obstacleMaxSize);
        }
    }

    public Vector3 ComputeForce(Boid boid, Collider[] near, int count)
    {
        Vector3 sep = Vector3.zero;
        Vector3 ali = Vector3.zero;
        Vector3 coh = Vector3.zero;
        int seen = 0;

        foreach (Collider col in near)
        {
            if (col?.attachedRigidbody == null || col.gameObject == boid.gameObject)
                continue;

            Vector3 toMe = boid.transform.position
                         - col.transform.position;

            sep += toMe / (toMe.sqrMagnitude + 0.0001f);
            ali += col.attachedRigidbody.linearVelocity;
            coh += col.transform.position;
            seen++;
        }

        if (seen == 0) 
            return Vector3.zero;

        ali /= seen;
        coh = coh / seen - boid.transform.position;

        Vector3 force = StayInBounds(boid) * 30f + AvoidObstacle(boid) * 2f;

        return force 
             + (sep.normalized * boid.MaxSpeed - boid.Velocity) * sepWeight
             + (ali.normalized * boid.MaxSpeed - boid.Velocity) * aliWeight
             + (coh.normalized * boid.MaxSpeed - boid.Velocity) * cohWeight;
    }

    // campos: center, boundsRadius = 18f,
    //         lookAhead = 2.5f, obstacleMask
    // volta para o centro quando sai do raio
    public Vector3 StayInBounds(Boid boid)
    {
        Vector3 toCenter = center
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
        if (!Physics.SphereCast(boid.transform.position,
                0.4f, dir, out RaycastHit hit,
                lookAhead, obstacleMask))
            return Vector3.zero;

        Vector3 away = Vector3.Reflect(dir, hit.normal);
        return (away - boid.Velocity).normalized
             * boid.MaxForce;
    }
    // dentro de ComputeForce, antes de retornar:
    //  force += StayInBounds(boid)
    //        + AvoidObstacle(boid) * 2f;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, boidSpawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, boundsRadius);

    }
}

