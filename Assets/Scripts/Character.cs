using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningToEnemy,
        RunningFromEnemy,
        BeginAttack,
        Attack,
        BeginShoot,
        Shoot,
        Death
    }

    public enum Weapon
    {
        Pistol,
        Bat,
    }

    public Weapon weapon;
    public float runSpeed;
    public float distanceFromEnemy;
    public Transform target;
    State state;
    Animator animator;
    Vector3 originalPosition;
    Quaternion originalRotation;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        state = State.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    [ContextMenu("Attack")]
    void AttackEnemy()
    {
        switch (weapon) {
            case Weapon.Bat:
                state = State.RunningToEnemy;
                break;

            case Weapon.Pistol:
                state = State.BeginShoot;
                break;
        }
    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f) {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                animator.SetFloat("Speed", 0.0f);
                transform.rotation = originalRotation;
                break;

            case State.RunningToEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(target.position, distanceFromEnemy))
                    state = State.BeginAttack;
                break;

            case State.BeginAttack:
                animator.SetTrigger("MeleeAttack");
                state = State.Attack;
                break;

            case State.Attack:
                break;

            case State.BeginShoot:
                animator.SetTrigger("Shoot");
                state = State.Shoot;
                break;

            case State.Shoot:
                break;

            case State.RunningFromEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    state = State.Idle;
                break;
            
            case State.Death:
                animator.SetTrigger("Death");
                gameObject.GetComponent<Character>().enabled = false;
                break;
        }
    }
}
