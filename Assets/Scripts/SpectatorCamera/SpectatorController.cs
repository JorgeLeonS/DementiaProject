using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectatorController : MonoBehaviour
{
    [Range(0f, 20f)]
    [SerializeField] float movementSpeed;

    [Range(0f, 20f)]
    [SerializeField] float rotationSpeed;

    InputCameraController droneController;
    Vector2 move;
    Vector2 rotation;
    float height;

    float xRotation = 0f;
    float yRotation = 0f;

    private void Awake()
    {
        droneController = new InputCameraController();
        droneController.Drone.Move.performed += ctxt => move = ctxt.ReadValue<Vector2>();
        droneController.Drone.Move.canceled += cntxt => move = Vector2.zero;
        droneController.Drone.Look.performed += ctxt => rotation = ctxt.ReadValue<Vector2>();
        droneController.Drone.Look.canceled += ctxt => rotation = Vector2.zero;

        droneController.Drone.Fly.performed += ctxt => height = ctxt.ReadValue<float>();
        droneController.Drone.Fly.canceled += cftxt => height = 0f;

        droneController.Drone.ResetHorizontal.performed += _ => ResetHorizontal();
    }

    private void OnEnable()
    {
        droneController.Drone.Enable();
        
    }

    private void OnDisable()
    {
        droneController.Drone.Disable();
        
    }

    private void Update()
    {
        MoveDrone();
        RotateDrone();
        Fly();
    }

    private void MoveDrone()
    {
        if (move == Vector2.zero)
            return;
        Debug.Log(move);

        if(move!= Vector2.zero)
        {
            float moveX = move.x * movementSpeed * Time.deltaTime;
            float moveZ = move.y * movementSpeed * Time.deltaTime;
            Vector3 movPos = new Vector3(moveX, 0f, moveZ);

            transform.Translate(movPos,Space.Self);
        }
    }

    private void RotateDrone()
    {
        if (rotation == Vector2.zero)
            return;

        float rotationX = rotation.x * rotationSpeed * Time.deltaTime;
        float rotationY = rotation.y * rotationSpeed * Time.deltaTime;
        Debug.Log(rotation);

        xRotation -= rotationY;
        //xRotation = Mathf.Clamp(xRotation,-90f, 90f);

        yRotation += rotationX;
        //yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void Fly()
    {
        if (height == 0)
            return;
        float newHeight = height * movementSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * newHeight);
    }

    private void ResetHorizontal()
    {
        Debug.Log("Reset button clicked");
        transform.rotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
    }

}
