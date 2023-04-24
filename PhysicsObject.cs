using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject
{
    [HideInInspector]
    public Vector3 position = Vector3.zero;
    public float mass = 1;
    public Vector3 currentVelocity = Vector3.zero;

    private float G;

    public PhysicsObject(Vector3 _position, float _mass, Vector3 _initialVelocity, float _G)
    {
        position = _position;
        mass = _mass;
        currentVelocity = _initialVelocity;
        G = _G;
    }

    public Vector3 UpdatePosition()
    {
        position += currentVelocity * Time.fixedDeltaTime;
        return position;
    }

    public Vector3 UpdateVelocity(PhysicsObject[] objects)
    {
        currentVelocity = CalculateGravity(objects);
        return currentVelocity;
    }

    Vector3 CalculateGravity(PhysicsObject[] objects)
    {
        // F = (G * m1 * m2) / (r * r)
        // G = gravitational constant
        // r = distance between objects

        Vector3 result = currentVelocity;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != this)
            {
                float deltaX = objects[i].position.x - position.x;
                float deltaY = objects[i].position.y - position.y;
                float deltaZ = objects[i].position.z - position.z;
                float r = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

                float F = (G * mass * objects[i].mass) / (r * r);
                Vector3 forceVector = (objects[i].position - position).normalized * F;
                Vector3 accelerationDueToGravity = forceVector / mass;

                result += accelerationDueToGravity * Time.fixedDeltaTime;
            }
        }

        return result;
    }
}
