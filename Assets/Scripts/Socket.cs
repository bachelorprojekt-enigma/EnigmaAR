using System;
// using Unity.VisualScripting;
using UnityEngine;

public class Socket : MonoBehaviour
{
    public GameObject currentPlug;
    private Rigidbody plugRigidbody;
    public Wire currentWire;
    private Color startColor = Color.white; //Tried to switch to this Color in DisableHighlight but it doesnt work somehow
    public bool hasWire = false;

    private float speed = 5f;

    private CapsuleCollider _capsuleCollider;
    
    [SerializeField] public string socketChar = "A";

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    //Only needed for drag and drop i recon
    private void OnTriggerEnter(Collider other)
    {
        return;
        if ((currentPlug != null && other.transform.gameObject != currentPlug) || currentPlug == null) return;
        Debug.Log("Socket trigger entered");
        if (other.CompareTag("Plug"))
        {
            Debug.Log("Plug entered Trigger");
            currentPlug = other.gameObject;
            currentWire = currentPlug.GetComponentInParent<Wire>();
            plugRigidbody = currentPlug.GetComponent<Rigidbody>();
        }
        
    }

    //Only needed for drag and drop i recon
    private void OnTriggerExit(Collider other)
    {
        return;
        Debug.Log("DoingSMTH3");
        if (other.CompareTag("Plug") && currentPlug != null)
        {
            Debug.Log("Plug exit pull radius, should stay still");
            plugRigidbody.linearVelocity = Vector3.zero;
            plugRigidbody.angularVelocity = Vector3.zero;
            currentPlug = null;
        }
    }

    public void HighlightSocket()
    {
        startColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.green;
    }

    public void DisableHighlightSocket()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    
    
    void Update()
    {
        //Currently unused, This code was for the Drag and Drop action of the Plugs/Wire
       // if (currentPlug != null && !currentPlug.GetComponent<DragAndDrop>().isDragging)
       // {
         //   Vector3 triggerCenter = _capsuleCollider.transform.TransformPoint(_capsuleCollider.center);
         //   float distance = Vector3.Distance(triggerCenter, currentPlug.transform.position);
         //   if (distance > 0.4f)
        //    {   
          //      float step = speed * Time.deltaTime;
          //      currentPlug.transform.position = Vector3.MoveTowards(currentPlug.transform.position,
         //           triggerCenter, step);
       //     }
    //    }
    }
}
