using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XORGate : GateBase
{
    protected override void InputPowerChanged()
    {
        if (inputNodes[0].GetPower() ^ inputNodes[1].GetPower())
            powered = true;
        else
            powered = false;

        outputNode.SetPower(powered);
    }
}
