using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 movementDirection;
    private Animator bulletAnimator;
    private float originalAnimatorSpeed = 1f;
    void Start()
    {
        bulletAnimator = GetComponent<Animator>();
        if (bulletAnimator != null)
        {
            originalAnimatorSpeed = bulletAnimator.speed;
        }

        Destroy(gameObject, 5f);
    }
    private void Update()
    {
        if (BlueCharacter.IsEnemyTimeFrozen)
        {
            SetBulletAnimationFrozen(true);
            return;
        }

        SetBulletAnimationFrozen(false);

        if (movementDirection == Vector3.zero) return;
        transform.position += movementDirection * Time.deltaTime;
    }
    private void SetBulletAnimationFrozen(bool isFrozen)
    {
        if (bulletAnimator == null)
        {
            return;
        }

        bulletAnimator.speed = isFrozen ? 0f : originalAnimatorSpeed;
    }

    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }
}