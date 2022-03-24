using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [FormerlySerializedAs("moveSpeed")] [SerializeField]
    private float moveForceFactor = 0.001f;

    [SerializeField] private int moveForce;
    [SerializeField] private int jumpForce;

    [SerializeField] private float airForce;
    [SerializeField] private float gravity;


    [SerializeField] private bool isGround;
    private float mass = 1;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float boundaryFactor;

    void Awake()
    {
        Observable.EveryUpdate().Subscribe(_ => MoveInput());
        Observable.EveryUpdate().Subscribe(_ => Move());
        Observable.EveryUpdate().Subscribe(_ => Boundary());
        Observable.EveryUpdate().Subscribe(_ => Jump());
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddForce(new Vector3(0, jumpForce, 0));
        }
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
        velocity.x = AddContraryForce(velocity.x, airForce);
        velocity.y = AddGravityForce(velocity.y, gravity);

        if (velocity.magnitude > 0.001f)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }

    private float AddGravityForce(float value, float force)
    {
        float tempValue = value;
        if (!isGround)
        {
            tempValue += -force * moveForceFactor;
        }
        else
        {
            tempValue = tempValue > 0 ? tempValue : 0;
        }

        return tempValue;
    }

    private float AddContraryForce(float value, float force)
    {
        float tempValue = 0;
        float tempX = value;
        if (math.abs(value) > 0.001)
        {
            tempX += (value > 0 ? -force : force) * moveForceFactor;
            var min = value > 0 ? 0 : value;
            var max = value > 0 ? value : 0;
            tempValue = math.clamp(tempX, min, max);
        }

        return tempValue;
    }

    private void MoveInput()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");
        var force = new Vector3(xAxis * moveForce * moveForceFactor, yAxis * jumpForce * moveForceFactor, 0);
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
}