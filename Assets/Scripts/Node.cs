using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private bool powered;
    [SerializeField] private List<Wire> wires;
    [SerializeField] private int connectionLimit = 1;
    public event Action OnPowerChanged;

    public bool GetPower()
    {
        return powered;
    }

    public void SetPower(bool powerRequest)
    {
        if (powerRequest == powered) //already set to that power
            return;
        
        powered = powerRequest;

        if (OnPowerChanged != null) 
            OnPowerChanged(); //Trigger event for all delegates
        
        foreach (Wire wire in wires)
        {
            if (wire.GetPower() != powered)
                wire.SetPower(powered);
        }
    }

    public bool AddNewConnection(Wire newWire)
    {
        if (wires.Count >= connectionLimit)
            return false;

        wires.Add(newWire);
        return true;
    }
}
