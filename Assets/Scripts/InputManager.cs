using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Node firstSelectedNode, secondSelectedNode;
    [SerializeField] private Wire selectedWire;
    [SerializeField] private GameObject wirePrefab;
    private bool drawingWire = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node newNode = MouseOverNode();
            if (newNode != null)
            {
                if (newNode != firstSelectedNode && !drawingWire) //First node selected, start drawing from that starting point
                {
                    Wire newWire = GameObject.Instantiate(wirePrefab).GetComponent<Wire>();
                    if (newNode.AddNewConnection(newWire)) //If the wire is able to be added to the node
                    {
                        Debug.Log("Success");
                        firstSelectedNode = newNode;
                        newWire.SetStartNode(firstSelectedNode);
                        selectedWire = newWire;
                        drawingWire = true;
                    }
                    else
                        Destroy(newWire.gameObject);
                }
                else if (newNode != firstSelectedNode)
                {
                    if (newNode.AddNewConnection(selectedWire)) //Try connecting other end of wire to node
                    {
                        selectedWire.SetEndNode(newNode);
                        drawingWire = false;
                        selectedWire = null;
                        firstSelectedNode = null;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (drawingWire)
        {
            selectedWire.SetEndPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("Mouse Pos: " + mousePos.x + ", " + mousePos.y);
    }

    private Node MouseOverNode()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Node"))
                return hit.transform.gameObject.GetComponent<Node>();
        }
        return null;
    }
}
