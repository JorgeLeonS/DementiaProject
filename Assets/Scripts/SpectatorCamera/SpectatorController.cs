using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorController : MonoBehaviour
{
    [Range(0f, 20f)]
    [SerializeField] float speed;

    InputCameraController droneController;
    Vector2 move;
    Vector2 rotation;
    

    private void Awake()
    {
        droneController = new InputCameraController();
        droneController.Drone.Move.performed += ctxt => move = ctxt.ReadValue<Vector2>();
        droneController.Drone.Move.canceled += cntxt => move = Vector2.zero;
        droneController.Drone.Look.performed += ctxt => rotation = ctxt.ReadValue<Vector2>();
        droneController.Drone.Look.canceled += ctxt => rotation = Vector2.zero;
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
    }

    private void MoveDrone()
    {
        if (move == Vector2.zero)
            return;
        Debug.Log(move);

        if(move!= Vector2.zero)
        {
            float moveX = move.x * speed * Time.deltaTime;
            float moveZ = move.y * speed * Time.deltaTime;
            Vector3 movPos = new Vector3(moveX, transform.position.y, moveZ);

            transform.Translate(movPos,Space.Self);
        }
    }

    private void RotateDrone()
    {
        if (rotation == Vector2.zero)
            return;
        Debug.Log(rotation);
        
    }

}
