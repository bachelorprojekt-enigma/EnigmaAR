
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private GameObject keyboardPokeInteractor;
    [SerializeField] private GameObject keyboardDirectInteractor;
    [SerializeField] private GameObject keyboardInteractorGroup;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float pokeSpeed = 0.5f;
    private Vector2 _moveDirection;
    private Vector2 _lookDirection;
    private Camera _camera;
    private float _originalY;
    private float _rotationX;
    private float _rotationY;
    private bool _isPressed;
    private bool _isHoveringButton;
    private bool _isHoveringSocket;
    private Socket litSocket;
    public bool isHoldingItem;
    private void Start()
    {
      
        keyboardPokeInteractor.SetActive(false);
        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        MovePlayer();
        RotateCamera();
        
    }

    private void FixedUpdate()
    {
        ShootRayToCursor();
        //ShootRayToCursor();    Moved to Update, to hopefully fix stuttering of dragging the Plugs
        //                       Maybe need to change it back if it results in worse performance
    }

    public void GetMouseClick(InputAction.CallbackContext context)
    {
        
        if (_isHoveringButton && context.performed)
        {
            _isPressed = true;
            _originalY = keyboardPokeInteractor.transform.position.y;
            Debug.Log("Pressed");
            keyboardPokeInteractor.SetActive(true);
            StartCoroutine(PushButtonDown());
        } 
        else if (context.canceled)
        {
            _isPressed = false;
            Debug.Log("Canceled");
            keyboardPokeInteractor.transform.position = new Vector3(keyboardPokeInteractor.transform.position.x, _originalY, keyboardPokeInteractor.transform.position.z);
            keyboardPokeInteractor.SetActive(false);
        }    
    }

    private IEnumerator PushButtonDown()
    {
        while (_isPressed)
        {
            Debug.Log("Pressing Button");
            keyboardPokeInteractor.transform.position += Vector3.up * (pokeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void GetMoveDirection(InputAction.CallbackContext context) 
    {
        _moveDirection = context.ReadValue<Vector2>();

    }

    public void GetLookDirection(InputAction.CallbackContext context)
    {
        _lookDirection = context.ReadValue<Vector2>();
    }
    

    private void MovePlayer()
    {
        Vector2 movementDelta = new Vector2(_moveDirection.x*moveSpeed*Time.deltaTime, _moveDirection.y*moveSpeed*Time.deltaTime); 
        // transform forward * movement delta 
        
       var position = transform.position + _camera.transform.right * _moveDirection.x * moveSpeed ;
       position += _camera.transform.forward * _moveDirection.y * moveSpeed;
       
       transform.position = Vector3.Lerp(transform.position, position, moveSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        _rotationX += _lookDirection.x * mouseSensitivity;
        _rotationY -= _lookDirection.y * mouseSensitivity;
  
        _camera.transform.rotation = Quaternion.Euler(_rotationY, _rotationX, 0);
    }

    private void ShootRayToCursor()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.gameObject.CompareTag("Button"))
            {
                _isHoveringButton = true;
                keyboardInteractorGroup.transform.position = hit.point;
            } 
            else if (hit.transform.gameObject.CompareTag("Socket"))
            {
                litSocket = hit.transform.GetComponent<Socket>();
                hit.transform.GetComponent<Socket>().HighlightSocket();
            }
            else
            {
                if(litSocket != null)
                {
                    litSocket.DisableHighlightSocket();
                    litSocket = null;
                }
                _isHoveringButton = false;
            }
        }
    }
}
