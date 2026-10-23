using UnityEngine;

public class Predator : MonoBehaviour
{
    [SerializeField] FlockManager flock;
    [SerializeField] float speed = 6f;

    private void Update()
    {
        if (flock.boids.Count == 0) 
            return;

        Vector3 c = Vector3.zero;

        foreach (var b in flock.boids) 
            c += b.transform.position;

        c /= flock.boids.Count;

        transform.position = Vector3.MoveTowards(transform.position, c, speed * Time.deltaTime);
    }
}