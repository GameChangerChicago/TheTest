using UnityEngine;
using System.Collections;

public class FireFlyCameraManager : MonoBehaviour
{
    private Camera _secondaryCamera;
    private BoxCollider2D _detectionBox;
    private float _goalSize;

    void Start()
    {
        _goalSize = Camera.main.orthographicSize;
        _detectionBox = this.GetComponent<BoxCollider2D>();
        _secondaryCamera = this.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (_secondaryCamera.orthographicSize < _goalSize - 0.1f)
        {
            Camera.main.orthographicSize += Time.deltaTime * 0.5f;
            _secondaryCamera.orthographicSize += Time.deltaTime * 0.5f;
        }
        else if(_secondaryCamera.orthographicSize > _goalSize + 0.1f)
        {
            Camera.main.orthographicSize -= Time.deltaTime * 0.5f;
            _secondaryCamera.orthographicSize -= Time.deltaTime * 0.5f;
        }
        else
        {
            Camera.main.orthographicSize = _goalSize;
            _secondaryCamera.orthographicSize = _goalSize;
        }
    }
    
    public void IncreaseSize()
    {
        float preGoalSize = _goalSize;
        _goalSize += 0.3f;
        _detectionBox.size = new Vector2(_detectionBox.size.x * (_goalSize / preGoalSize), _detectionBox.size.y * (_goalSize / preGoalSize));
    }
}