using UnityEngine;
using System.Collections;

public class BuildingFadeManager : MonoBehaviour
{
    public SpriteRenderer Player;

    private SpriteRenderer _buildingRenderer;
    private BoxCollider2D _buildingBoxCollider;

    void Start()
    {
        _buildingRenderer = this.GetComponent<SpriteRenderer>();
        _buildingBoxCollider = this.GetComponentInChildren<BoxCollider2D>();
    }

    void Update()
    {
        Rect PlayerRect = new Rect(Player.transform.position.x - Player.sprite.bounds.extents.x, Player.transform.position.y - Player.sprite.bounds.extents.y, Player.sprite.bounds.extents.x * 2, Player.sprite.bounds.extents.y * 2),
             BuildingRect = new Rect(_buildingBoxCollider.transform.position.x - _buildingBoxCollider.bounds.extents.x, _buildingBoxCollider.transform.position.y - _buildingBoxCollider.bounds.extents.y, _buildingBoxCollider.bounds.extents.x * 2, _buildingBoxCollider.bounds.extents.y * 2);

        if (PlayerRect.Overlaps(BuildingRect))
        {
            if (_buildingRenderer.color.a > 0.1)
                _buildingRenderer.color = new Color(_buildingRenderer.color.r, _buildingRenderer.color.g, _buildingRenderer.color.b, _buildingRenderer.color.a - Time.deltaTime);
        }
        else
        {
            if(_buildingRenderer.color.a < 1)
                _buildingRenderer.color = new Color(_buildingRenderer.color.r, _buildingRenderer.color.g, _buildingRenderer.color.b, _buildingRenderer.color.a + Time.deltaTime);
        }
    }
}