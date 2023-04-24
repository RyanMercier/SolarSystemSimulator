using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    // G = gravitational constant = 0.0000000000667
    public float G = 0.0000000000667f;
    public int futureSteps = 1;
    public float orbitLineWidth = 0.5f;
    public CelestialBody[] celestialBodies;
    private PhysicsObject[] physObjects;
    private Queue<Vector3>[] futurePositions;


    void Awake()
    {
        Time.fixedDeltaTime = 0.01f;
    }

    void Start()
    {
        // Create an array of qeueus which each hold the future positions of each planet
        futurePositions = Enumerable.Range(0, celestialBodies.Length).Select(i => new Queue<Vector3>()).ToArray();
        physObjects = new PhysicsObject[celestialBodies.Length];

        // Initialize the PhysicsObjects
        for (int i = 0; i < celestialBodies.Length; i++)
        {
            celestialBodies[i].line.SetWidth(orbitLineWidth, orbitLineWidth);
            physObjects[i] = new PhysicsObject(celestialBodies[i].rb.position, celestialBodies[i].rb.mass, celestialBodies[i].initialVelocity, G);
        }

        InitializeCelestialBodyPath();
    }

    void InitializeCelestialBodyPath()
    {
        for (int i = 0; i < celestialBodies.Length; i++)
        {
            futurePositions[i].Enqueue(physObjects[i].position);
        }

        // Calculate futureSteps number of future positions of the celestial bodies and save them into our qeueue
        for (int i = 0; i < futureSteps; i++)
        {
            for (int j = 0; j < celestialBodies.Length; j++)
            {
                physObjects[j].UpdateVelocity(physObjects);
            }

            for (int j = 0; j < celestialBodies.Length; j++)
            {
                futurePositions[j].Enqueue(physObjects[j].UpdatePosition());
            }
        }
    }

    void FixedUpdate()
    {
        // Move the celestial body's GameObject
        for (int i = 0; i < celestialBodies.Length; i++)
        {
            celestialBodies[i].rb.position = futurePositions[i].Dequeue();
        }

        // Calculate the next velocities
        for (int i = 0; i < celestialBodies.Length; i++)
        {
            physObjects[i].UpdateVelocity(physObjects);
        }

        // Update the future positions table
        for (int i = 0; i < celestialBodies.Length; i++)
        {
            futurePositions[i].Enqueue(physObjects[i].UpdatePosition());
        }

        DrawOrbits();
    }

    void DrawOrbits()
    {
        for (int i = 1; i < celestialBodies.Length; i++)
        {
            if (celestialBodies[i].drawOrbit)
            {
                celestialBodies[i].line.positionCount = futurePositions[i].Count;
                celestialBodies[i].line.SetPositions(futurePositions[i].ToArray());
            }

            else
            {
                celestialBodies[i].line.positionCount = 0;
            }
        }
    }
}
