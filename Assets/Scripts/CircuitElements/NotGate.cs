using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotGate : GateBase
{
    protected override void InputPowerChanged()
    {
        powered = !inputNodes[0].GetPower();
        outputNode.SetPower(powered); //Invert incoming power
    }
}
