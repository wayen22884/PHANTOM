using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public bool IsForceMove = true;
    public bool allowAirDash = true;
    private float moveForceFactor = 0.001f;
    [Range(0, 100f)] [SerializeField] private int moveForce = 35;
    [Range(0, 20f)] [SerializeField] private int jumpForce = 5;

    [Range(0, 100f)] [SerializeField] private float airForce = 0.7F;
    [Range(0, 100f)] [SerializeField] private float gravity = 45;


    [SerializeField] private bool isGround;
    private bool istouchLeft;
    private bool istouchRight;
    private float mass = 1;
    [SerializeField] private Vector3 velocity;
    [Range(0, 20f)] [SerializeField] private float boundaryFactor = 1;
    [Range(0, 20f)] [SerializeField] private float stachDistance = 3;
    [Range(0, 5f)] [SerializeField] private float stachUseTime = 0.6F;
    [Range(0, 20f)] [SerializeField] private float maxXDirctionSpeed = 20;
    private int groundLayer = 6;

    public Vector3 collider;

    public Action attackAction;
    public Func<float,bool> SetFace;
    public Func<bool> ReturnIsRight;
    public Action<string> OnChangeState;

    private List<Func<bool>> checkList = new List<Func<bool>>();

    public Func<bool> AttackInput;
    public Func<bool> DashInput;
    public Func<float> MoveInput;
    public Func<bool> JumpInput;
    public void StartInput()
    {
        checkList.Add(() => CheckAction(AttackInput, AttackAction));
        checkList.Add(() => CheckAction(() => DashInput() && (isGround || allowAirDash), DashAction));
        checkList.Add(() => CheckAction(() => true, MoveAction));
        Observable.EveryUpdate().Subscribe(_ => Controll());
    }

    private void Controll()
    {
        foreach (var checkItem in checkList)
        {
            if (checkItem == null)
            {
                continue;
            }

            if (checkItem())
            {
                break;
            }
        }
        velocity = CheckPhysics(velocity);
    }

    private void MoveAction()
    {
        var xAxis = MoveInput();
        SetFace?.Invoke(xAxis);
        var force = new Vector3(xAxis * moveForce * moveForceFactor, 0, 0);
        if (JumpInput() && isGround)
        {
            force.y += jumpForce;
            isGround = false;
        }

        var a = force / mass;
        velocity += a;
    }

    private void DashAction()
    {
        var isRight = ReturnIsRight?.Invoke() ?? false;
        velocity.x = 0;
        var distance = isRight ? stachDistance : -stachDistance;
        transform.DOMoveX(transform.position.x + distance, stachUseTime);
    }

    private Vector3 CheckPhysics(Vector3 tempVelocity)
    {
        tempVelocity.x = DealXDirectionVelocity(tempVelocity.x, airForce * moveForceFactor);
        tempVelocity.y = DealYDirectionVelocity(tempVelocity.y);

        if (tempVelocity.magnitude > 0.001f)
        {
            if (IsForceMove)
            {
                var move = new Vector3(tempVelocity.x, tempVelocity.y * Time.deltaTime, 0);
                transform.position += move;
                tempVelocity.x = 0;
            }
            else
            {
                transform.position += tempVelocity * Time.deltaTime;
            }
        }


        var layerMaskGround = 1 << 6;
        isGround = CheckHit(transform.position, collider.x - 0.1f, Vector3.down, collider.y / 2, layerMaskGround);

        if (CheckHit(transform.position, collider.x - 0.1f, Vector3.down, collider.y / 2, 1 << 7))
        {
            tempVelocity = AddBoundaryForce(tempVelocity);
        }

        istouchLeft = CheckHit(transform.position, collider.y, Vector3.left, collider.x / 2, layerMaskGround);
        istouchRight = CheckHit(transform.position, collider.y, Vector3.right, collider.x / 2, layerMaskGround);
        return tempVelocity;
    }

    private bool CheckAction(Func<bool> ifCondition, Action action)
    {
        if (ifCondition == null)
        {
            return false;
        }

        if (ifCondition())
        {
            action?.Invoke();
            return true;
        }

        return false;
    }

    private void AttackAction()
    {
        attackAction?.Invoke();
        OnChangeState?.Invoke("Smash");
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

    private Vector3 AddBoundaryForce(Vector3 tempVelocity)
    {
        tempVelocity.y += (-tempVelocity.y + -tempVelocity.y * boundaryFactor);
        return tempVelocity;
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
}
