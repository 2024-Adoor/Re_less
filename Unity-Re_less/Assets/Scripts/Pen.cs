using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField]
    private GameObject lineSegmentPrefab;
    
    [SerializeField]
    private LineRenderer currentLineSegment;
    
    [SerializeField]
    private GameObject lineContainer;
    
    
    private bool _isPressed = false;
    private readonly float _minInkDist = 0.01f;
    List<Vector3> _pointPositions = new List<Vector3>();
    
    void Update()
    {
        
        
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && !_isPressed)
        {
            StartLine(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch)  +
                      OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward * 0.1f);
            _isPressed = true;
        }
        
        if (_isPressed)
        {
            UpdateLine(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));

            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                _isPressed = false;
            }
        }
    }
    
    private void StartLine(Vector3 position)
    {
        GameObject newLine = Instantiate(lineSegmentPrefab, position, Quaternion.identity);
        currentLineSegment = newLine.GetComponent<LineRenderer>();
        currentLineSegment.positionCount = 1;
        currentLineSegment.SetPosition(0, position);
        _pointPositions.Clear();
        _pointPositions.Add(position);
        newLine.transform.parent = lineContainer.transform;
    }

    private void UpdateLine(Vector3 position)
    {
        float segmentLength = (position - _pointPositions.Last()).magnitude;
        if (segmentLength >= _minInkDist)
        {
            _pointPositions.Add(position);
            currentLineSegment.positionCount = _pointPositions.Count;
            currentLineSegment.SetPositions(_pointPositions.ToArray());
        }
    }
    
    public void ClearLines()
    {
        for (int i = 0; i < lineContainer.transform.childCount; i++)
        {
            Destroy(lineContainer.transform.GetChild(i).gameObject);
        }
    }
}
