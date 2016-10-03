using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public float AmoutToChangeLight,
                 MaxLightSize;
    public int FriendCountGoal;
    public bool StrongLight;

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
                    if (StrongLight)
                    {
                        _targetLightSize = _myLight.LightRadius + AmoutToChangeLight;
                    }
                    else
                    {
                        _targetLightSize = Mathf.Clamp(_myLight.LightRadius + (AmoutToChangeLight * (Random.Range(0.1f, 0.5f))), 0.1f, MaxLightSize);
                    }
                }
                else
                {
                    if (StrongLight)
                    {
                        _targetLightSize = _myLight.LightRadius - AmoutToChangeLight;
                    }
                    else
                    {
                        _targetLightSize = Mathf.Clamp(_myLight.LightRadius - (AmoutToChangeLight * (Random.Range(0.5f, 0.9f))), 0.1f, MaxLightSize);
                    }

                    //if (this.transform.parent.name == "Player")
                    //    Debug.Log("Current Light: " + _myLight.LightRadius + " Change Ammount: " + AmoutToChangeLight);
                }

                _lightIncreasing = value;
            }
        }
    }
    private bool _lightIncreasing = true;

    private DynamicLight _myLight;
    private FireFlyCameraManager _theCameraManager;
    private float _targetLightSize;
    private int _friendCount;
    private bool _fadingOut,
                 _pulsing;

    void Start()
    {
        _myLight = this.GetComponent<DynamicLight>();
        _theCameraManager = FindObjectOfType<FireFlyCameraManager>();
        _targetLightSize = _myLight.LightRadius + AmoutToChangeLight;
        if (this.transform.parent.name == "Player")
        {
            StartPulsing();
        }
        else
        {
            Invoke("StartPulsing", Random.Range(0.5f, 1.5f));
        }
    }
    
    void Update()
    {
        if (_pulsing)
            LightPulse();

        if(_friendCount == FriendCountGoal)
        {
            //Begin FadeOut
        }
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

    private void StartPulsing()
    {
        _pulsing = true;
    }

    public void GainedAFriend()
    {
        lightIncreasing = true;
        _friendCount++;
        _targetLightSize += 0.5f;
        _theCameraManager.IncreaseSize();
    }

    public IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(1);
        
        lightIncreasing = false;
        _targetLightSize = 0;
        _fadingOut = true;
    }
}