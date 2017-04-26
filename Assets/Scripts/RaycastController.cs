using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

    public const float SkinWidth = .015f;
    const float _distanceBetweenRays = 0.25f;



    public LayerMask CollisionMask;

    [HideInInspector]
    public int HorizontalRayCount;
    [HideInInspector]
    public int VerticalRayCount;

    [HideInInspector]
    public float _horizontalRaySpacing;
    [HideInInspector]
    public float _verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D _collider;
    public RaycastOrigins _raycastOrigins;

    public virtual void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(SkinWidth * -2);

        _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(SkinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        HorizontalRayCount = Mathf.RoundToInt(boundsHeight / _distanceBetweenRays);
        VerticalRayCount = Mathf.RoundToInt(boundsWidth / _distanceBetweenRays);

        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        _horizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
    }
    
}
