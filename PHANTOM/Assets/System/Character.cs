using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.GameCenter;

public class Character : MonoBehaviour
{
    private float moveForceFactor = 0.001f;

    [SerializeField] private int moveForce;
    [SerializeField] private int jumpForce;

    [SerializeField] private float airForce;
    [SerializeField] private float gravity;


    [SerializeField] private bool isGround;
    private bool istouchLeft;
    private bool istouchRight;
    private float mass = 1;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float boundaryFactor;
    private int groundLayer = 6;

    public Vector3 collider;

    void Awake()
    {
        Observable.EveryUpdate().Subscribe(_ => MoveInput());
        Observable.EveryUpdate().Subscribe(_ => Boundary());
        Observable.EveryUpdate().Subscribe(_ => Move());
        Observable.EveryUpdate().Subscribe(_ => CheckGround());
    }

    private void Boundary()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddBoundaryForce();
        }
    }

    private void Move()
    {
        velocity.x = AddContraryForce(velocity.x, airForce * moveForceFactor);

        if (velocity.x > 0 && istouchRight)
        {
            velocity.x = 0;
        }

        if (velocity.x < 0 && istouchLeft)
        {
            velocity.x = 0;
        }

        if (!isGround)
        {
            velocity.y += -(gravity * moveForceFactor);
        }
        else
        {
            velocity.y = 0;
        }

        if (velocity.magnitude > 0.001f)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }

    private float AddContraryForce(float value, float force)
    {
        float tempValue = 0;
        bool positive = value >= 0;
        var absValue = math.abs(value);
        if (absValue > 0.001)
        {
            var tempX = math.clamp(absValue - force, 0, absValue);
            tempValue = positive ? tempX : -tempX;
        }

        return tempValue;
    }

    private void MoveInput()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var force = new Vector3(xAxis * moveForce * moveForceFactor, 0, 0);
        if (Input.GetKeyDown(KeyCode.S))
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
}