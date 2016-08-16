using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public float AmmoutToChangeLight;    

    protected bool lightIncreasing
    {
        get
        {
            return _lightIncreasing;
        }
        set
        {
            if (value != _lightIncreasing)
            {
                if(value)
                {
                    _targetLightSize = _myLight.LightRadius + AmmoutToChangeLight;
                }
                else
                {
                    Debug.Log(_myLight.LightRadius - AmmoutToChangeLight);
                    _targetLightSize = _myLight.LightRadius - AmmoutToChangeLight;
                }

                _lightIncreasing = value;
            }
        }
    }
    private bool _lightIncreasing = true;

    private DynamicLight _myLight;
    private float _targetLightSize;

    void Start()
    {
        _myLight = this.GetComponent<DynamicLight>();
        _targetLightSize = _myLight.LightRadius + AmmoutToChangeLight;
    }
    
    void Update()
    {
        LightPulse();
    }

    private void LightPulse()
    {
        if (lightIncreasing)
        {
            if(_myLight.LightRadius < _targetLightSize - 0.1f)
            {
                _myLight.LightRadius += Time.deltaTime * (_targetLightSize - _myLight.LightRadius);
            }
            else
            {
                Debug.Log("sadf");
                lightIncreasing = false;
            }
        }
        else
        {
            Debug.Log(_targetLightSize);
            if (_myLight.LightRadius > _targetLightSize + 0.1f)
            {
                _myLight.LightRadius -= Time.deltaTime * (AmmoutToChangeLight - (_myLight.LightRadius - AmmoutToChangeLight));
            }
            else
            {
                lightIncreasing = true;
            }
        }
    }
}