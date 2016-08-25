using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireFlyBackgroundManager : MonoBehaviour
{
    public GameObject Background1,
                      Background2;
    private List<GameObject> _allBackgrounds;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void BackgroundHandler()
    {
        if (_allBackgrounds.Count > 1)
        {
            for (int i = 0; i < _allBackgrounds.Count; i++)
            {
                //Things happen to these _allBackground[i]
            }
        }
        else
        {
            //Do things to this ---> _allBackgrounds[0]
        }
    }
}