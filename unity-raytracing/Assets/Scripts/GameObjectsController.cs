using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsController : MonoBehaviour
{
    public float acceleration = 50; // how fast you accelerate
    public float accSprintMultiplier = 4; // how much faster you go when "sprinting"
    public float lookSensitivity = 1; // mouse look sensitivity
    public float dampingCoefficient = 5; // how quickly you break to a halt after you stop your input
    public float scale = 0.1f;
    Vector3 velocity; // current velocity

    public List<GameObject> gameObjects = new List<GameObject>();
    int objectIndex = 0;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            objectIndex += 1;
        }
        if (objectIndex >= gameObjects.Count)
            objectIndex = 0;

        // Input
        UpdateInput();
        
        // Physics
        velocity = Vector3.Lerp(velocity, Vector3.zero, dampingCoefficient * Time.deltaTime);
        gameObjects[objectIndex].transform.position += velocity * Time.deltaTime;
    }

    void UpdateInput()
    {
        // Position
        velocity += GetAccelerationVector() * Time.deltaTime;

        // Rotation
        Vector2 mouseDelta = lookSensitivity * new Vector2(Input.GetAxis("Horizontal2") * scale, -Input.GetAxis("Vertical2") * scale);
        Quaternion rotation = gameObjects[objectIndex].transform.rotation;
        Quaternion horiz = Quaternion.AngleAxis(mouseDelta.x, Vector3.up);
        Quaternion vert = Quaternion.AngleAxis(mouseDelta.y, Vector3.right);
        gameObjects[objectIndex].transform.rotation = horiz * rotation * vert;
    }

    Vector3 GetAccelerationVector()
    {
        Vector3 moveInput = default;

        moveInput += Input.GetAxis("Horizontal") * Vector3.right * scale * 0.1f;
        moveInput += -Input.GetAxis("Vertical") * Vector3.forward * scale * 0.1f;

        Vector3 direction = gameObjects[objectIndex].transform.TransformVector(moveInput.normalized);

        if (Input.GetButton("Fire1"))
            return direction * (acceleration * accSprintMultiplier);
        else
            return direction * acceleration;
    }
}