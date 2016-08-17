using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public float AmoutToChangeLight;    

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
                    _targetLightSize = _myLight.LightRadius + AmoutToChangeLight;
                }
                else
                {
                    _targetLightSize = _myLight.LightRadius - AmoutToChangeLight;

                    //if (this.transform.parent.name == "Player")
                    //    Debug.Log("Current Light: " + _myLight.LightRadius + " Change Ammount: " + AmoutToChangeLight);
                }

                _lightIncreasing = value;
            }
        }
    }
    private bool _lightIncreasing = true;

    private DynamicLight _myLight;
    private float _targetLightSize;
    private int _friendCount;
    private bool _fadingOut;

    void Start()
    {
        _myLight = this.GetComponent<DynamicLight>();
        _targetLightSize = _myLight.LightRadius + AmoutToChangeLight;
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
                lightIncreasing = false;
            }
        }
        else
        {
            if (_myLight.LightRadius > _targetLightSize + 0.1f)
            {
                float shrinkRate;

                if (_myLight.LightRadius - (_myLight.LightRadius - _targetLightSize) < 0.1f)
                    shrinkRate = 0.1f;
                else
                    shrinkRate = _myLight.LightRadius - (_myLight.LightRadius - _targetLightSize);

                if (_friendCount > 0)
                    _myLight.LightRadius -= Time.deltaTime * (shrinkRate * (5 / _friendCount));
                else
                    _myLight.LightRadius -= Time.deltaTime * (shrinkRate * 5);

            }
            else if(!_fadingOut)
            {
                lightIncreasing = true;
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    public void GainedAFriend()
    {
        lightIncreasing = true;
        _friendCount++;
        _targetLightSize += 0.5f;
    }

    public IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(1);
        
        lightIncreasing = false;
        _targetLightSize = 0;
        _fadingOut = true;
    }
}