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
    [SerializeField] private Animator _animator;

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
    
    private Vector3 _lastPosition;
    private uint playEventId;
    private bool isPlaying;

    private Dictionary<uint, String> statesDict = new Dictionary<uint, string>()
    {
        { 0, "" },
        {906520814,	"gras"		}	,
        {4142189312,	"street"	},
        {144697359	,"outdoor"	}	,
        {340398852,	"indoor"	},		
        {565529991,	"final"		}	,
        { 748895195, "None"	},	
    };

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        previousPosition = transform.position;
        cameraStartPosition = cameraParentObject.transform.position;
        AkSoundEngine.SetState("footsteps_states", "outdoor");
    }

    void OnJumping(InputValue inputValue)
    {
        Debug.Log("I am jumping");
        // change the y value of the moveBy Variable. 
        jump = true;
    }

    private void FixedUpdate()
    {
        //ExecuteMovement();
        GetMouseCursorWorldPosition();
        //MoveCamera();
        AkPlayFootsteps();

        // always set it to false
        jump = false;
    }

    private void AkPlayFootsteps()
    {
        float velocity = (transform.position - _lastPosition).magnitude;

        if (velocity > 0.005f * 0.005f)
        {
            // Get the play event id; play sound event
            if (!isPlaying)
            {
                Debug.Log("Playing footsteps");
                AkSoundEngine.GetState("footsteps_states", out var ee);
                Debug.Log(statesDict[ee]);
                playEventId = AkSoundEngine.PostEvent("Play_Footsteps",gameObject);
                isPlaying = true;
            }
        }
        else 
        {
            // stop sound by sound id
            if (isPlaying)
            {
                AkSoundEngine.StopPlayingID(playEventId);
                isPlaying = false;
            }
        }
        
        _lastPosition = transform.position;
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
