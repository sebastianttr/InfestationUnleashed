using System;
using System.Collections;
using System.Collections.Generic;
using enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Player : MonoBehaviour
{


    [SerializeField] private float rotationSpeed = 40f;

    [SerializeField] 
    private GameObject cursorObject;

    [SerializeField] 
    private GameObject cameraParentObject;

    [SerializeField] 
    private LayerMask _layerMask;
    
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private Vector3 cameraStartPosition; 
    
    private Rigidbody _rigidBody;
    
    private Vector3 moveBy;
    private bool jump;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        previousPosition = transform.position;
        cameraStartPosition = cameraParentObject.transform.position;
    }

    void OnJumping(InputValue inputValue)
    {
        Debug.Log("I am jumping");
        // change the y value of the moveBy Variable. 
        jump = true;
    }

    /*void ExecuteMovement()
    {
        if (_movementType == MovementType.TransformBased)
        {
            previousPosition = _rigidBody.position;
            //transform.position += new Vector3(moveBy.x, moveBy.z, moveBy.y) * speed;
            //transform.Translate(moveBy * (speed * Time.deltaTime));

            Vector3 moveByFixed = new Vector3(moveBy.x, moveBy.z, moveBy.y);
            
            _rigidBody.MovePosition(transform.position + moveByFixed * moveByFixed.magnitude * speed * Time.deltaTime);
        }
        else if (_movementType == MovementType.PhysicalBased)
        {
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            
            Vector3 rotatedVector = Camera.main.transform.rotation * new Vector3(moveBy.x, jump ? 100 : 0, moveBy.y);
            rotatedVector = new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z );
            
            rigidBody.AddForce(rotatedVector * 3, ForceMode.Force);
        } 
        
    }*/

    private void FixedUpdate()
    {
        //ExecuteMovement();
        GetMouseCursorWorldPosition();
        //MoveCamera();

        
        // always set it to false
        jump = false;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with object.");
    }

    private void MoveCamera()
    {
        Vector3 cameraMovement = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 moveByFixed = new Vector3(moveBy.x, moveBy.z, moveBy.y);
        
        Vector3 velocity = _rigidBody.position - previousPosition;      

        Debug.Log($"{Time.time} : {velocity}");

        //cameraParentObject.transform.position += (moveByFixed * moveByFixed.magnitude +  velocity * velocity.magnitude * speed * Time.deltaTime);
        //cameraParentObject.transform.Translate(velocity.normalized * speed * Time.deltaTime);
    }

    private void GetMouseCursorWorldPosition()
    {
        Vector2 mouse = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouse);

        if(Physics.Raycast(ray,out var hit, Mathf.Infinity,_layerMask))
        {
            Vector3 cursorPosition = hit.point;
            cursorPosition.Set(cursorPosition.x, 0, cursorPosition.z);
            cursorObject.transform.position = cursorPosition;
            
            var lookPos = cursorPosition - transform.position;
            lookPos.y = 0;
           
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    void OnMovement(InputValue inputValue)
    {
        Vector2 newPos = inputValue.Get<Vector2>();
        moveBy = newPos;
    }
}
