using UnityEngine;

public class PickUpAndPlace : MonoBehaviour
{
    public GameObject wirePrefab;
    public GameObject usedWireList;         //Empty Object to save used wire objects
    private Camera _camera;
    private GameObject _currentObject;
    private bool _isHolding;
    private Vector3 _distanceToCam;    //Irgendwas ausprobieren
    private int _plugAmount;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        _distanceToCam = new Vector3(1f, 0.1f, 0.3f);
    }

    private int CheckWireHeld()
    {
        return _currentObject?.transform.childCount ?? 0;
    }

    public void Place()
    {
        if (Input.GetMouseButtonDown(0) && CheckWireHeld() > 0)
        {
            Debug.Log($"Place called. CurrentObject: {_currentObject?.name}, ChildCount: {CheckWireHeld()}");
            Transform plugTransform = _currentObject.transform.GetChild(0);
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.gameObject.CompareTag("Socket") && plugTransform.CompareTag("Plug"))
                {
                    Socket socket = hit.transform.GetComponent<Socket>();
                    if (socket.hasWire && CheckWireHeld() < 2) PickPlugFromSocket(hit);
                    else if (!socket.hasWire)
                    {
                        Debug.Log("Located Socket");
                        plugTransform.SetParent(hit.transform);
                        plugTransform.position = hit.transform.position;
                        if (_currentObject.GetComponent<Wire>().startSocket == null)
                        {
                            _currentObject.GetComponent<Wire>().startSocket = socket;
                            socket.currentPlug = plugTransform.gameObject;
                            Debug.Log(socket.currentPlug);
                            socket.currentWire = _currentObject.GetComponent<Wire>();
                            socket.hasWire = true;
                        }
                        else
                        {
                            _currentObject.GetComponent<Wire>().endSocket = socket;
                            socket.currentPlug = plugTransform.gameObject;
                            socket.currentWire = _currentObject.GetComponent<Wire>();
                            socket.hasWire = true;
                            Drop();
                        }
                    }
                }
            }
        }
        else if (CheckWireHeld() == 0)
        {
            Debug.Log("Somehow Empty Object in Hand?!?!?!?");
            Drop();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DestroyWire();
        }
    }

    public void TryPickUp()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.Log("Located PickUp Obj");
                if (hit.transform.gameObject.CompareTag("PickUpObject"))
                {
                    Debug.Log("Picking Up Object!");
                    _currentObject = Instantiate(wirePrefab);
                    _currentObject.transform.SetParent(_camera.transform);
                    _currentObject.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;
                    _currentObject.transform.GetChild(1).GetComponent<Rigidbody>().isKinematic = true;
                    _currentObject.transform.localPosition = _distanceToCam;
                    _currentObject.transform.rotation = _camera.transform.rotation;
                    GetComponent<KeyboardController>().isHoldingItem = true;
                    _isHolding = true;
                    Debug.Log($"Instantiated Wire: {_currentObject.name}, Child Count: {_currentObject.transform.childCount}");
                }
                else if (hit.transform.gameObject.CompareTag("Socket"))
                {
                    PickPlugFromSocket(hit);
                }
            }
        }
    }

    private void PickPlugFromSocket(RaycastHit hit)
    {
        Socket socket = hit.transform.GetComponent<Socket>();
        if (CheckWireHeld() < 1)
        {
            if (!socket.hasWire) return;
            if (socket.currentPlug == null) return;
                    
            Wire wire = socket.currentWire;
            GameObject plug = socket.currentPlug;
            
            //Remove the right Socket from the Wire
            wire.startSocket = (socket == wire.startSocket) ? null : wire.startSocket;
            wire.endSocket = (socket == wire.endSocket) ? null : wire.endSocket;
            
            plug.transform.SetParent(wire.transform);
            _currentObject = wire.transform.gameObject;
            _currentObject.transform.SetParent(_camera.transform);
            _currentObject.transform.localPosition = _distanceToCam;
            _currentObject.transform.rotation = _camera.transform.rotation;
            plug.transform.localPosition = new Vector3(-0.2f, 0.0f, 0.8f); 
            socket.currentPlug = null;
            socket.currentWire = null;
            socket.hasWire = false;
            _isHolding = true;
        }
        else if (CheckWireHeld() == 1)
        {
            if (!socket.hasWire) return;
            if (socket.currentPlug == null) return;
            
            if (socket.currentWire.transform.gameObject == _currentObject)
            {
                Wire wire = _currentObject.GetComponent<Wire>();
                GameObject plug = socket.currentPlug;
                
                //Remove the right Socket from the Wire
                wire.startSocket = (socket == wire.startSocket) ? null : wire.startSocket;
                wire.endSocket = (socket == wire.endSocket) ? null : wire.endSocket;
                    
                plug.transform.SetParent(wire.transform);
                plug.transform.localPosition = new Vector3(-0.3f, 0.0f, 0.8f); 
                socket.currentPlug = null;
                socket.currentWire = null;
                socket.hasWire = false; 
            }
        }
    }

    //Drops _currentObject/currentWire when all children/Plugs are set
    //All used Wires get put into usewWireList for Further checks
    private void Drop()
    {
        _currentObject.transform.SetParent(usedWireList.transform);
        _currentObject = null;
        _isHolding = false;
    }

    //Destroys _currentObject/currentWire when using RMB
    private void DestroyWire()
    {
        if (_currentObject.GetComponent<Wire>())
        {
            Socket socket1 = _currentObject.GetComponent<Wire>().startSocket;
            Socket socket2 = _currentObject.GetComponent<Wire>().endSocket;
            if (socket1 != null)
            {
                socket1.currentPlug = null;
                socket1.currentWire = null;
                socket1.hasWire = false;
            }
            if (socket2 != null)
            {
                socket2.currentPlug = null;
                socket2.currentWire = null;
                socket2.hasWire = false;
            }
            
            Destroy(_currentObject.GetComponent<Wire>().startPlug);
            Destroy(_currentObject.GetComponent<Wire>().endPlug);
            Destroy(_currentObject);
            _isHolding = false;
            _currentObject = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isHolding)
        {
            TryPickUp();
        }
        else
        {
            Place();
        }
    }
}
