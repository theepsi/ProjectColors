using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller2D : RaycastController {

    public CollisionInfo Collisions;

    [HideInInspector]
    public Vector2 PlayerInput;


    public override void Start()
    {
        base.Start();
        
    }

    public void Move(Vector2 movement, bool standingOnPlatform)
    {
        Move(movement, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 movement, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        Collisions.Reset();

        PlayerInput = input;

        if (movement.x != 0)
            HorizontalCollisions(ref movement);

        if (movement.y != 0)
            VerticalCollisions(ref movement);
        
        transform.Translate(movement);

        if (standingOnPlatform)
        {
            Collisions.Below = true;
        }
    }

    void HorizontalCollisions(ref Vector2 movement)
    {
        float directionX = Mathf.Sign(movement.x);
        float rayLength = Mathf.Abs(movement.x) + SkinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {

                if (hit.distance == 0)
                {
                    continue;
                }
                movement.x = (hit.distance - SkinWidth) * directionX;
                rayLength = hit.distance;

                Collisions.Left = directionX == -1;
                Collisions.Right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector2 movement)
    {
        float directionY = Mathf.Sign(movement.y);
        float rayLength = Mathf.Abs(movement.y) + SkinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (_verticalRaySpacing * i + movement.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                movement.y = (hit.distance - SkinWidth) * directionY;
                rayLength = hit.distance;

                Collisions.Below = directionY == -1;
                Collisions.Above = directionY == 1;
            }
        }
    }

    public struct CollisionInfo
    {
        public bool Above, Below;
        public bool Left, Right;
        
        public void Reset()
        {
            Above = Below = false;
            Left = Right = false;
        }
    }

}
