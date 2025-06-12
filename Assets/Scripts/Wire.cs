using System;
using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;


public class Wire : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject startPlug;
    public GameObject endPlug;
    private Rope rope;
    private Color startColor;
    private float speed = 10f;
    private string connectedChars = "";

    public bool startIsPlugged;
    public bool endIsPlugged;

    public Socket startSocket;
    public Socket endSocket;
    
    void Start()
    {
        rope = GetComponent<Rope>();
        Transform startTransform = rope.StartPoint;
        Transform endTransform = rope.EndPoint;
        if (startTransform != null && endTransform != null)
        {
            startPlug = startTransform.gameObject;
            endPlug = endTransform.gameObject;
        }
    }

    public string ConnectedChars()
    {
        return connectedChars;
    }

    public string updateConnection()
    {
        if (startSocket != null && endSocket != null)
        {
            if(!connectedChars.Contains(startSocket.socketChar + endSocket.socketChar + ","))
            {
                connectedChars += startSocket.socketChar + endSocket.socketChar + ",";
                Debug.Log("connected Chars " + connectedChars);
                return connectedChars;
            }

            return connectedChars;
        }
        else
        {
            connectedChars = "";
            return connectedChars;
        }
    }

    private void FixedUpdate()
    {
        updateConnection();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(startPlug.transform.position, endPlug.transform.position);
        speed = distance / 3f;
        if (speed < 10)
        {
            speed = 10;
        }
        if (distance > rope.ropeLength + 500)
        {
            float step = speed * Time.deltaTime;
            startPlug.transform.position = Vector3.MoveTowards(startPlug.transform.position,
                endPlug.transform.position, step);
        }
    }
}
