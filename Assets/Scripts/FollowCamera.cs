using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Controller2D Player;
    public Vector2 FocusAreaSize;

    public float VerticalOffset;
    public float LookAheadDistanceX;
    public float lookSmoothTimeX;
    public float VerticalSmoothTime;

    FocusArea CamFocusArea;

    float _currentLookAheadX;
    float _playerLookAheadX;
    float _lookAheadDirX;
    float _smoothLookVelocityX;
    float _smoothVelocityY;

    bool lookAheadStopped;

    void Start()
    {
        CamFocusArea = new FocusArea(Player._collider.bounds, FocusAreaSize);
    }

    void LateUpdate()
    {
        CamFocusArea.Update(Player._collider.bounds);

        Vector2 focusPosition = CamFocusArea.Center + Vector2.up * VerticalOffset;

        if (CamFocusArea.Velocity.x != 0)
        {
            _lookAheadDirX = Mathf.Sign(CamFocusArea.Velocity.x);
            if (Mathf.Sign(Player.PlayerInput.x) == Mathf.Sign(CamFocusArea.Velocity.x) && Player.PlayerInput.x != 0)
            {
                lookAheadStopped = false;
                _playerLookAheadX = _lookAheadDirX * LookAheadDistanceX;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    _playerLookAheadX = _currentLookAheadX + (_lookAheadDirX * LookAheadDistanceX - _currentLookAheadX) / 4f;
                }
            }
        }

  
        _currentLookAheadX = Mathf.SmoothDamp(_currentLookAheadX, _playerLookAheadX, ref _smoothLookVelocityX, lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref _smoothVelocityY, VerticalSmoothTime);
        focusPosition += Vector2.right * _currentLookAheadX;

        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(CamFocusArea.Center, FocusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 Center;
        float Left, Right;
        float Top, Bottom;

        public Vector2 Velocity;

        public FocusArea(Bounds bounds, Vector2 size)
        {
            Left = bounds.center.x - size.x / 2;
            Right = bounds.center.x + size.x / 2;

            Bottom = bounds.min.y;
            Top = bounds.min.y + size.y;

            Velocity = Vector2.zero;
            Center = new Vector2((Left + Right) / 2, (Top + Bottom) / 2);
        }

        public void Update(Bounds bounds)
        {
            float shiftX = 0;
            if (bounds.min.x < Left)
                shiftX = bounds.min.x - Left;
            else if (bounds.max.x > Right)
                shiftX = bounds.max.x - Right;

            Left += shiftX;
            Right += shiftX;

            float shiftY = 0;
            if (bounds.min.y < Bottom)
                shiftY = bounds.min.y - Bottom;
            else if (bounds.max.y > Top)
                shiftY = bounds.max.y - Top;

            Top += shiftY;
            Bottom += shiftY;

            Center = new Vector2((Left + Right) / 2, (Top + Bottom) / 2);

            Velocity = new Vector2(shiftX, shiftY);
        }
    }
}
