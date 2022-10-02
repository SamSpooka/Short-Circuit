using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Node node;
    [SerializeField] private bool toggleState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        toggleState = !toggleState;
        node.SetPower(toggleState);
    }
}
