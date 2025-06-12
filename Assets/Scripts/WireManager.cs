using System.Collections.Generic;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    private Transform[] _wireChildren;
    private Wire[] _wireObjects;
    private string _pluggedLetters;

    private void GetUsedWires()
    {
        _wireChildren = transform.GetComponentsInChildren<Transform>();
        var wireList = new List<Wire>();

        for (int i = 1; i < _wireChildren.Length; i++)
        {
            Wire wire = _wireChildren[i].GetComponent<Wire>();
            if (wire != null)
            {
                wireList.Add(wire);
            }
            else
            {
                Debug.LogWarning($"Child {_wireChildren[i].name} has no Wire component.");
            }
        }

        _wireObjects = wireList.ToArray();
    }

    private void BuildWiredString()
    {
        if (_wireObjects == null)
        {
            Debug.LogWarning("No wire objects found.");
            return;
        }

        _pluggedLetters = "";

        foreach (var wire in _wireObjects)
        {
            if (wire != null)
            {
                _pluggedLetters += wire.updateConnection();
            }
            else
            {
                Debug.LogWarning("Null wire detected in _wireObjects.");
            }
        }
        Debug.Log("Connected Letters: " + _pluggedLetters);
    }

    private void OnTransformChildrenChanged()
    {
        Debug.Log("Child added or removed in the hierarchy");
        GetUsedWires();
        BuildWiredString();
    }
}