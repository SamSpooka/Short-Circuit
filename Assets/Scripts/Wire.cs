using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private WireTheme wireTheme;
    [SerializeField] private bool powered;
    [SerializeField] private Node nodeA, nodeB;
    private LineRenderer lineRenderer;
    private Vector2 currentDirection;
    private Vector2 prevPoint;
    private Vector2 prevEndPoint;
    [SerializeField] private float WIRE_SEGMENT_LENGTH_THRESHOLD = 3f;
    [SerializeField] private float WIRE_SEGMENT_BREAK_THRESHOLD = 1f;
    [SerializeField] private float WIRE_SEGMENT_BACKTRACK_THRESHOLD = 0.5f;
    private float GRID_SIZE = 0.1f;
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        currentDirection = Vector2.down;
        if (WIRE_SEGMENT_BACKTRACK_THRESHOLD >= WIRE_SEGMENT_BREAK_THRESHOLD)
            Debug.Log("WARNING: BACKTRACK_THRESHOLD should not be less than or equal to BREAK_THRESHOLD for best experience");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetPower()
    {
        return powered;
    }

    public void SetPower(bool powerRequest)
    {
        if (powered == powerRequest)
            return;

        powered = powerRequest;
        Color color;
        if (powered)
            color = wireTheme.onColor;
        else
            color = wireTheme.offColor;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        if (nodeA.GetPower() != powered)
            nodeA.SetPower(powered);
        if (nodeB.GetPower() != powered)
            nodeB.SetPower(powered);
    }

    public bool SetStartNode(Node newNode)
    {
        nodeA = newNode;
        lineRenderer.SetPosition(0, nodeA.transform.position);
        lineRenderer.SetPosition(1, nodeA.transform.position);
        return true;
    }

    public bool SetEndNode(Node newNode)
    {
        nodeB = newNode;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newNode.transform.position);
        return true;
    }

    //Set the visual end point of the line depending on the Vector2 argument
    //Also handles changing direction and removing previously set points on the line
    //if the user is moving backwards (trying to remove the wire)
    public void SetEndPoint(Vector2 endPoint)
    {
        prevPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        if (prevPoint == null)
        {
            prevPoint = endPoint; //Initialize prevPoint
            Debug.Log("ERROR: Start Point is null");
        }

        Vector2 lastSetPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 2); //Get the last drawn position
        if (lastSetPoint == null)
            Debug.Log("LAST SET POINT NULL");

        float absVertShift = Mathf.Abs(lastSetPoint.y - prevPoint.y);
        float absHorizShift = Mathf.Abs(lastSetPoint.x - prevPoint.x);
        //Debug.Log("Direction: " + currentDirection);

        if (currentDirection == Vector2.up || currentDirection == Vector2.down) //Currently moving vertically
        {
            if ((absVertShift >= WIRE_SEGMENT_LENGTH_THRESHOLD || lineRenderer.positionCount == 2) && Mathf.Abs(prevPoint.x - endPoint.x) >= WIRE_SEGMENT_BREAK_THRESHOLD) //Should be moving horizontally
            {
                CreateNewLinePosition(new Vector2(lastSetPoint.x, endPoint.y)); //Create a new point only shifted in the y direction
                currentDirection = Vector2.right * Mathf.Sign(endPoint.x - lastSetPoint.x); //Now moving either left or right
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector2(endPoint.x, endPoint.y));
            }
            else if (absVertShift <= WIRE_SEGMENT_BACKTRACK_THRESHOLD && lineRenderer.positionCount > 2) //Should backtrack (change to horizontal movement)
            {
                Debug.Log("Removing Vertical, switching to horizontal");
                RemoveLastLinePosition();
                Vector2 previousLastSetPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 2);
                if (previousLastSetPoint.x < endPoint.x)
                    currentDirection = Vector2.right;
                else
                    currentDirection = Vector2.left;
            }
            else
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector2(lastSetPoint.x, endPoint.y));
            
        }
        else if (currentDirection == Vector2.left || currentDirection == Vector2.right) //Currently moving horizontally
        {
            if ((absHorizShift >= WIRE_SEGMENT_LENGTH_THRESHOLD || lineRenderer.positionCount == 2) && Mathf.Abs(prevPoint.y - endPoint.y) >= WIRE_SEGMENT_BREAK_THRESHOLD) //Should be moving vertically
            {
                CreateNewLinePosition(new Vector2(endPoint.x, lastSetPoint.y)); //Create a new point only shifted in the x direction
                currentDirection = Vector2.up * Mathf.Sign(endPoint.y - lastSetPoint.y); //Now moving either up or down
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector2(endPoint.x, endPoint.y));
            }
            else if (absHorizShift <= WIRE_SEGMENT_BACKTRACK_THRESHOLD && lineRenderer.positionCount > 2) //Should backtrack (change to vertical movement)
            {
                Debug.Log("Removing Horizontal, switching to vertical");
                Debug.Log("Prev,last" + prevPoint + ", " + lastSetPoint);
                RemoveLastLinePosition();
                Vector2 previousLastSetPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 2);
                if (previousLastSetPoint.y < endPoint.y)
                    currentDirection = Vector2.up;
                else
                    currentDirection = Vector2.down;
            }
            else
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector2(endPoint.x, lastSetPoint.y));
        } 

        prevEndPoint = endPoint;
    }

    private void CreateNewLinePosition(Vector2 newPos)
    {
        Vector3[] newPositions = new Vector3[lineRenderer.positionCount + 1]; //Create a new array with 1 more position than the lineRenderer had previously
        for (int i = 0; i < lineRenderer.positionCount; i++) //Add all existing positions to the array
            newPositions[i] = lineRenderer.GetPosition(i);

        newPositions[lineRenderer.positionCount - 1] = newPos; //Add the new position at the end of the array (with 1 buffer space for the point to be moved)
        lineRenderer.positionCount++;
        lineRenderer.SetPositions(newPositions); //Update the line renderer
    }

    private void RemoveLastLinePosition()
    {
        Vector3[] newPositions = new Vector3[lineRenderer.positionCount - 1]; //Create new array with 1 fewer position than the lineRenderer had previously
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
            newPositions[i] = lineRenderer.GetPosition(i);

        lineRenderer.positionCount--;
        lineRenderer.SetPositions(newPositions);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lineRenderer.GetPosition(lineRenderer.positionCount - 2), 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lineRenderer.GetPosition(lineRenderer.positionCount - 1), 0.1f);
    }
}
