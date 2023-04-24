using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public Vector3 initialVelocity;
    public bool drawOrbit;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public LineRenderer line;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
    }
}
