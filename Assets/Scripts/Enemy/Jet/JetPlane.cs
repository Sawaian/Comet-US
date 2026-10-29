
using UnityEngine;

public class JetPlane : MonoBehaviour
{
    [Header("Flight")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float loopRadius = 2.5f;
    [SerializeField] private Vector2 loopCenter = new Vector2(0f, 2f);
    [SerializeField] private float leftEdge = -12f;
    [SerializeField] private float rightEdge = 12f;

    [Header("Phase 3")]
    [SerializeField] private float phase3SpeedMultiplier = 2f;

    [Header("Shooting")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float fireInterval = 1.5f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;

    private enum FlightState
    {
        Entering,
        Looping,
        Exiting
    }

    private FlightState state = FlightState.Entering;

    private float angle;
    private float nextShotTime;
    private float currentSpeed;
    private bool isDead = false;

    private void Awake()
    {
        currentSpeed = speed;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (health == null)
            health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (health == null)
            return;

        health.OnDamaged += PlayHit;
        health.OnDeath += PlayExplosion;
    }

    private void OnDisable()
    {
        if (health == null)
            return;

        health.OnDamaged -= PlayHit;
        health.OnDeath -= PlayExplosion;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject target =
                GameObject.FindWithTag("Player");

            if (target != null)
                player = target.transform;
        }

        ResetFlight();
    }

    public void SetPhase(int phase)
    {
        currentSpeed = phase == 3
            ? speed * phase3SpeedMultiplier
            : speed;
    }

    private void Update()
    {
        if (isDead)
            return;

        MoveJet();

        if (player != null &&
            projectile != null &&
            Time.time >= nextShotTime)
        {
            Fire();
            nextShotTime = Time.time + fireInterval;
        }
    }

    private void MoveJet()
    {
        Vector3 position = transform.position;

        switch (state)
        {
            case FlightState.Entering:
                {
                    float targetX =
                        loopCenter.x - loopRadius;

                    position.x = Mathf.MoveTowards(
                        position.x,
                        targetX,
                        currentSpeed * Time.deltaTime
                    );

                    // Works even when the jet enters
                    // from farther offscreen.
                    if (Mathf.Abs(position.x - targetX) < 0.01f)
                    {
                        state = FlightState.Looping;
                        angle = Mathf.PI;
                    }

                    break;
                }

            case FlightState.Looping:
                {
                    float angularSpeed =
                        currentSpeed / loopRadius;

                    angle += angularSpeed * Time.deltaTime;

                    position.x =
                        loopCenter.x +
                        Mathf.Cos(angle) * loopRadius;

                    position.y =
                        loopCenter.y +
                        Mathf.Sin(angle) * loopRadius;

                    if (angle >= 3f * Mathf.PI)
                    {
                        state = FlightState.Exiting;

                        position.x =
                            loopCenter.x - loopRadius;

                        position.y = loopCenter.y;
                    }

                    break;
                }

            case FlightState.Exiting:
                {
                    position.x +=
                        currentSpeed * Time.deltaTime;

                    if (position.x >= rightEdge)
                    {
                        ResetFlight();
                        return;
                    }

                    break;
                }
        }

        transform.position = position;
    }

    private void ResetFlight()
    {
        transform.position = new Vector3(
            leftEdge,
            loopCenter.y,
            transform.position.z
        );

        state = FlightState.Entering;
        angle = Mathf.PI;
    }

    private void Fire()
    {
        if (firePoint == null ||
            projectile == null ||
            player == null)
            return;

        Vector3 direction =
            (player.position -
             firePoint.position).normalized;

        GameObject bullet = Instantiate(
            projectile,
            firePoint.position,
            Quaternion.identity
        );

        JetProjectile jetBullet =
            bullet.GetComponent<JetProjectile>();

        if (jetBullet != null)
        {
            jetBullet.SetDirection(
                direction,
                bulletSpeed
            );
        }
    }

    private void PlayHit()
    {
        if (animator == null)
            return;

        animator.SetTrigger("Jet_hit");
    }

    private void PlayExplosion(GameObject jet)
    {
        isDead = true;

        if (animator == null)
            return;

        animator.ResetTrigger("Jet_hit");
        animator.SetTrigger("Jet_explosion");
    }
}
