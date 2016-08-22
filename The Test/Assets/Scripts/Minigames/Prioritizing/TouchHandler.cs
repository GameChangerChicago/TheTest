using UnityEngine;
using System.Collections;

public class TouchHandler : MonoBehaviour
{
    public BoxCollider2D[] TouchColliders;
    private SubController _theSub;

    void Start()
    {
        _theSub = FindObjectOfType<SubController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Touch();
        }
    }

    private void Touch()
    {
        for (int i = 0; i < TouchColliders.Length; i++)
        {
            if (TouchColliders[i].OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                switch (i)
                {
                    case 0:
                        _theSub.currentSubmarineState = SubmarineStates.LEFTROW;
                        break;
                    case 1:
                        _theSub.currentSubmarineState = SubmarineStates.MIDDLEROW;
                        break;
                    case 2:
                        _theSub.currentSubmarineState = SubmarineStates.RIGHTROW;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}