using UnityEngine;
using System.Collections;

public class BuildingFadeManager : MonoBehaviour
{
    public SpriteRenderer Player,
                          Building;

    void Update()
    {
        //Debug.Log(Player.sprite.textureRect.Overlaps(Building.sprite.textureRect));
        Rect PlayerRect = new Rect(Player.transform.position.x - Player.sprite.bounds.extents.x, Player.transform.position.y - Player.sprite.bounds.extents.y, Player.sprite.bounds.extents.x * 2, Player.sprite.bounds.extents.y * 2),
             BuildingRect = new Rect(Building.transform.position.x - Building.sprite.bounds.extents.x, Building.transform.position.y - Building.sprite.bounds.extents.y, Building.sprite.bounds.extents.x * 2, Building.sprite.bounds.extents.y * 2);
        
        if (PlayerRect.Overlaps(BuildingRect))
        {
            if (Building.color.a > 0.1)
                Building.color = new Color(Building.color.r, Building.color.g, Building.color.b, Building.color.a - Time.deltaTime);
        }
        else
        {
            if(Building.color.a < 1)
                Building.color = new Color(Building.color.r, Building.color.g, Building.color.b, Building.color.a + Time.deltaTime);
        }
    }
}