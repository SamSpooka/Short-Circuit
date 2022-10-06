using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb : MonoBehaviour
{
    [SerializeField] private Node node;
    [SerializeField] private SpriteRenderer bulbSprite;
    // Start is called before the first frame update
    void Start()
    {
        if (node != null)
            node.OnPowerChanged += SetPower;
    }

    private void SetPower()
    {
        Debug.Log("Changing Power to " + node.GetPower());
        if (node.GetPower())
            bulbSprite.color = Color.yellow;
        else
            bulbSprite.color = Color.black;
    }
}
