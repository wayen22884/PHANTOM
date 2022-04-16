using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public bool IsForceMove;
    public bool allowAirDash;
    private float moveForceFactor = 0.001f;
    [Range(0, 100f)] [SerializeField] private int moveForce;
    [Range(0, 20f)] [SerializeField] private int jumpForce;

    [Range(0, 100f)] [SerializeField] private float airForce;
    [Range(0, 100f)] [SerializeField] private float gravity;


    [SerializeField] private bool isGround;
    private bool istouchLeft;
    private bool istouchRight;
    private float mass = 1;
    [SerializeField] private Vector3 velocity;
    [Range(0, 20f)] [SerializeField] private float boundaryFactor;
    [Range(0, 20f)] [SerializeField] private float stachDistance = 3;
    [Range(0, 5f)] [SerializeField] private float stachUseTime = 1;
    [Range(0, 20f)] [SerializeField] private float maxXDirctionSpeed = 3;
    private int groundLayer = 6;

    public Vector3 collider;

    private Action attackAction;


    public void Initialize(Action attackAction)
    {
        this.attackAction = attackAction;
    }

    public void StartInput()
    {
        Observable.EveryUpdate().Subscribe(_ => MoveInput());
        Observable.EveryUpdate().Subscribe(_ => AttackInput());

        Observable.EveryUpdate().Subscribe(_ => Move());
        Observable.EveryUpdate().Subscribe(_ => CheckGround());
        Observable.EveryUpdate().Subscribe(_ => Stash());
    }

    private void AttackInput()
    {
        if (Input.GetButtonDown("NormalAttack"))
        {
            attackAction?.Invoke();
        }
    }

    private void Stash()
    {
        if (Input.GetButtonDown("Dash") && (isGround || allowAirDash))
        {
            var isRight = Input.GetAxis("Horizontal") > 0;
            velocity.x = 0;
            var distance = isRight ? stachDistance : -stachDistance;
            transform.DOMoveX(transform.position.x + distance, stachUseTime);
        }
    }


    private void Move()
    {
        velocity.x = DealXDirectionVelocity(velocity.x, airForce * moveForceFactor);
        velocity.y = DealYDirectionVelocity(velocity.y);

        OnChangeState("Velocity",velocity.magnitude>0.001?0:1);
        if (velocity.magnitude > 0.001f)
        {
            if (IsForceMove)
            {
                var move = new Vector3(velocity.x, velocity.y * Time.deltaTime, 0);
                transform.position += move;
                velocity.x = 0;
            }
            else
            {
                transform.position += velocity * Time.deltaTime;
            }
        }
    }

    private float DealYDirectionVelocity(float velocityY)
    {
        var tempValue = velocityY;
        if (!isGround)
        {
            tempValue += -(gravity * moveForceFactor);
        }
        else
        {
            tempValue = 0;
        }

        return tempValue;
    }

    private float DealXDirectionVelocity(float xValue, float force)
    {
        float tempValue = xValue;
        bool rightDirection = xValue >= 0;
        if (xValue > 0 && istouchRight)
        {
            tempValue = 0;
        }

        if (xValue < 0 && istouchLeft)
        {
            tempValue = 0;
        }

        var absValue = Math.Abs(tempValue);
        absValue = absValue > maxXDirctionSpeed ? maxXDirctionSpeed : absValue;
        if (absValue > 0.001)
        {
            var tempX = Mathf.Clamp(absValue - force, 0, absValue);
            tempValue = rightDirection ? tempX : -tempX;
        }

        return tempValue;
    }

    private void MoveInput()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var force = new Vector3(xAxis * moveForce * moveForceFactor, 0, 0);
        if (Input.GetButtonDown("Jump") && isGround)
        {
            force.y += jumpForce;
            isGround = false;
        }

        AddForce(force);
    }

    public void AddBoundaryForce()
    {
        velocity.y += (-velocity.y + -velocity.y * boundaryFactor);
    }

    private void AddForce(Vector3 vector3)
    {
        var a = vector3 / mass;
        velocity += a;
    }

    private void CheckGround()
    {
        var layerMaskGround = 1 << 6;
        isGround = CheckHit(transform.position, collider.x - 0.1f, Vector3.down, collider.y / 2, layerMaskGround);

        if (CheckHit(transform.position, collider.x - 0.1f, Vector3.down, collider.y / 2, 1 << 7))
        {
            AddBoundaryForce();
        }

        istouchLeft = CheckHit(transform.position, collider.y, Vector3.left, collider.x / 2, layerMaskGround);
        istouchRight = CheckHit(transform.position, collider.y, Vector3.right, collider.x / 2, layerMaskGround);
        //Debug.Log($"right:{istouchRight}     left:{istouchLeft}");
    }

    private bool CheckHit(Vector3 center, float lineDistance, Vector3 direction, float maxDistance, int layerMask = 0)
    {
        var firstPoint = center;
        var lineDirction = Vector3.Cross(Vector3.forward, direction).normalized;
        firstPoint -= lineDistance / 2 * lineDirction;
        var hit = false;
        var interval = lineDistance / 10;
        for (int i = 0; i < 10 && !hit; i++)
        {
            var startPoint = firstPoint + lineDirction * interval * i;
            hit = Physics.Raycast(startPoint, direction, maxDistance, layerMask);
        }

        return hit;
    }

    public void OnDrawGizmosSelected()
    {
        DrawCollider();
    }

    private void DrawCollider()
    {
        Gizmos.color = new Color(1, 1, 1, 0.3f);
        Gizmos.DrawCube(transform.position, collider);
    }

    public event Action<string,float> OnChangeState;
}

public class CharacterAttr
{
    private ReactiveProperty<int> HP;
    private ReactiveProperty<int> ATK;
    private ReactiveProperty<int> Speed;
    private ReactiveProperty<int> AtkSpeed;
}