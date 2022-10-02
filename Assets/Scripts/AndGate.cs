using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndGate : MonoBehaviour
{
    [SerializeField] private Node inputA, inputB, output;

    private void Start()
    {
        if (inputA != null)
            inputA.OnPowerChanged += InputPowerChanged;
        if (inputB != null)
            inputB.OnPowerChanged += InputPowerChanged;
    }
    private void InputPowerChanged()
    {
        Debug.Log("Input Power Changed");
        if (inputA.GetPower() && inputB.GetPower())
            output.SetPower(true);
        else if (output.GetPower())
            output.SetPower(false);
    }
}
