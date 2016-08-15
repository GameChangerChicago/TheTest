using UnityEngine;
using System.Collections;

public class CookieManager : MonoBehaviour
{
    public GameObject[] CookieSprites;
    public float PercentEaten;

    protected int currentCookieIndex
    {
        get
        {
            return _currentCookieIndex;
        }
        set
        {
            if(_currentCookieIndex != value)
            {
                for (int i = 0; i < CookieSprites.Length; i++)
                {
                    if(i != value)
                    {
                        CookieSprites[i].SetActive(false);
                    }
                    else
                    {
                        CookieSprites[i].SetActive(true);
                    }
                }

                //AntController[] allAnts = FindObjectsOfType<AntController>();

                //for (int i = 0; i < allAnts.Length; i++)
                //{
                //    allAnts[i].StartMoving();
                //}

                _currentCookieIndex = value;
            }
        }
    }
    private int _currentCookieIndex;

    void Update()
    {
        if(PercentEaten > 99)
        {
            currentCookieIndex = 5;
        }
        else if(PercentEaten > 75)
        {
            currentCookieIndex = 4;
        }
        else if(PercentEaten > 50)
        {
            currentCookieIndex = 3;
        }
        else if (PercentEaten > 25)
        {
            currentCookieIndex = 2;
        }
        else if (PercentEaten > 10)
        {
            currentCookieIndex = 1;
        }
    }
}