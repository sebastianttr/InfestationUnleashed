using System.Collections;
using System.Collections.Generic;
using enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] 
    private MovementType _movementType = MovementType.TransformBased;
    
        
    [SerializeField]
    private float speed = 1f;
    
    private Vector3 moveBy;
    private bool jump;

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
