using System;
using System.Collections;
using System.Collections.Generic;
using enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] 
    private MovementType _movementType = MovementType.TransformBased;
    
    [SerializeField] 
    private Animator _animator;

    [SerializeField]
    private float speed = 1f;
    
    private Vector3 moveBy;
    private bool jump;

    private String currentWalkingDirection = "";
    private String previousWalkingDirection = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void ExecuteMovement()
    {
        if (_movementType == MovementType.TransformBased)
        {
            //transform.position += new Vector3(moveBy.x, moveBy.z, moveBy.y) * speed;
            //transform.Translate(moveBy * (speed * Time.deltaTime));

            Vector3 moveByFixed = new Vector3(moveBy.x, moveBy.z, moveBy.y);
            
            if (moveByFixed.magnitude != 0)
            {
                if (moveByFixed == Vector3.left && currentWalkingDirection != "left") currentWalkingDirection = "left";
                else if(moveByFixed == -Vector3.left && currentWalkingDirection != "right") currentWalkingDirection = "right";
                else if (moveByFixed == Vector3.forward && currentWalkingDirection != "up") currentWalkingDirection = "up";
                else if (moveByFixed == -Vector3.forward && currentWalkingDirection != "down") currentWalkingDirection = "down";
            }
            else currentWalkingDirection = "";
            
            
            // if there was a change
            if(!currentWalkingDirection.Equals(previousWalkingDirection))
                PerformAnimation();

            previousWalkingDirection = currentWalkingDirection;

            transform.position += moveByFixed * moveByFixed.magnitude * speed * Time.deltaTime;
        }
        else if (_movementType == MovementType.PhysicalBased)
        {
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            
            Vector3 rotatedVector = Camera.main.transform.rotation * new Vector3(moveBy.x, jump ? 100 : 0, moveBy.y);
            rotatedVector = new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z );
            
            rigidBody.AddForce(rotatedVector * 3, ForceMode.Force);
        } 
        
    }

    private void PerformAnimation()
    {

        switch (currentWalkingDirection)
        {
            case "left":
                _animator.SetBool("idle", false);
                _animator.SetBool("walk_left", true);
                _animator.SetBool("walk_right", false);
                _animator.SetBool("walk_front", false);
                _animator.SetBool("walk_back", false);
                break;
            case "right":
                _animator.SetBool("idle", false);
                _animator.SetBool("walk_right", true);
                _animator.SetBool("walk_left", false);
                _animator.SetBool("walk_front", false);
                _animator.SetBool("walk_back", false);
                break; 
            case "up":
                _animator.SetBool("idle", false);
                _animator.SetBool("walk_front", true);
                _animator.SetBool("walk_right", false);
                _animator.SetBool("walk_left", false);
                _animator.SetBool("walk_back", false);
                break;
            case "down":
                _animator.SetBool("idle", false);
                _animator.SetBool("walk_back", true);
                _animator.SetBool("walk_right", false);
                _animator.SetBool("walk_front", false);
                _animator.SetBool("walk_left", false);
                break;
            default:
                _animator.SetBool("idle", true);
                _animator.SetBool("walk_right", false);
                _animator.SetBool("walk_front", false);
                _animator.SetBool("walk_back", false);
                _animator.SetBool("walk_left", false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        ExecuteMovement();
    }
    
    void OnMovement(InputValue inputValue)
    {
        Vector2 newPos = inputValue.Get<Vector2>();
        moveBy = newPos;
    }
}
