using UnityEngine;
using System.Collections;

public class FireFlyCameraManager : MonoBehaviour
{
    private Camera _secondaryCamera;
    private float _goalSize;

    void Start()
    {
        _goalSize = Camera.main.orthographicSize;
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
        _goalSize += 0.3f;
    }
}