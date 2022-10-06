using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBase : MonoBehaviour
{
    [SerializeField] protected Node[] inputNodes;
    [SerializeField] protected Node outputNode;
    [SerializeField] protected bool powered;

    protected void Start()
    {
        foreach (Node inputNode in inputNodes)
        {
            if (inputNode != null)
                inputNode.OnPowerChanged += InputPowerChanged;
        }
        outputNode.OnPowerChanged += OutputPowerChanged;
    }

    protected virtual void InputPowerChanged()
    {
        //Implementation (gate specific)
    }

    protected virtual void OutputPowerChanged()
    {
        //Typical Implementation (can be overridden)
        if (outputNode.GetPower() != powered && powered)
            outputNode.SetPower(true);
    }

    protected void OnDestroy()
    {
        foreach (Node inputNode in inputNodes)
        {
            if (inputNode != null)
                inputNode.OnPowerChanged -= InputPowerChanged;
        }
    }
}
