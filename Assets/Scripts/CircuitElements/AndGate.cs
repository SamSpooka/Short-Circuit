using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndGate : GateBase
{
    protected override void InputPowerChanged()
    {
        if (inputNodes[0].GetPower() && inputNodes[1].GetPower())
            powered = true;
        else if (outputNode.GetPower())
            powered = false;

        outputNode.SetPower(powered);
    }
}
