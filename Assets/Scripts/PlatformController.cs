using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController {
    
    public LayerMask PassengersMask;

    public Vector3[] LocalWaypoints;
    Vector3[] _globalWaypoints;

    public bool Clyclic = false;
    public float WaitTime;

    [Range(0,2)]
    public float EaseAmount;
    float _nextMoveTime;

    public float Speed;
    int _fromWayPointIndex;
    float _percentBetweenWayPoints;

    List<PassengerMovement> _passengerMovement;
    Dictionary<Transform, Controller2D> _passengerDictionary = new Dictionary<Transform, Controller2D>();

    public override void Start () {
        base.Start();

        _globalWaypoints = new Vector3[LocalWaypoints.Length];

        for (int i = 0; i < LocalWaypoints.Length; i++)
        {
            _globalWaypoints[i] = LocalWaypoints[i] + transform.position;
        }

    }
	
	// Update is called once per frame
	void Update () {
        UpdateRaycastOrigins();

        Vector3 velocity = CalculatePlatformMovement();

        CalculatePassengerMovement(velocity);

        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    float Ease(float x)
    {
        float a = EaseAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1-x, a));
    }

    Vector3 CalculatePlatformMovement()
    {
        if(Time.time < _nextMoveTime)
        {
            return Vector3.zero;
        }

        _fromWayPointIndex %= _globalWaypoints.Length;
        int toWayPointIndex = (_fromWayPointIndex + 1) % _globalWaypoints.Length;

        float distanceBetweenWaypoints = Vector3.Distance(_globalWaypoints[_fromWayPointIndex], _globalWaypoints[toWayPointIndex]);

        _percentBetweenWayPoints += Time.deltaTime * Speed / distanceBetweenWaypoints;
        _percentBetweenWayPoints = Mathf.Clamp01(_percentBetweenWayPoints);

        float easedPercent = Ease(_percentBetweenWayPoints);

        Vector3 newPos = Vector3.Lerp(_globalWaypoints[_fromWayPointIndex], _globalWaypoints[toWayPointIndex], easedPercent);

        if(_percentBetweenWayPoints >= 1)
        {
            _percentBetweenWayPoints = 0;
            _fromWayPointIndex++;

            if (!Clyclic)
            {
                if (_fromWayPointIndex >= _globalWaypoints.Length - 1)
                {
                    _fromWayPointIndex = 0;
                    System.Array.Reverse(_globalWaypoints);
                }
            }
            _nextMoveTime = Time.time + WaitTime;
        }

        return newPos - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in _passengerMovement)
        {
            if (!_passengerDictionary.ContainsKey(passenger.PTransform))
            {
                _passengerDictionary.Add(passenger.PTransform, passenger.PTransform.GetComponent<Controller2D>());
            }

            if (passenger.MoveBeforePlatform == beforeMovePlatform)
            {
                _passengerDictionary[passenger.PTransform].Move(passenger.Velocity, passenger.StandingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement(Vector3 velocity)
    {
        _passengerMovement = new List<PassengerMovement>();
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.TopLeft;
                rayOrigin += Vector2.right * (_verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, PassengersMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - SkinWidth) * directionY;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight;
                rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, PassengersMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - SkinWidth) * directionX;
                        float pushY = -SkinWidth;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }

            }
        }

        if (directionY == -1 || velocity.y == 0 && velocity.x != 0){
            float rayLength = SkinWidth * 2;

            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = _raycastOrigins.TopLeft + Vector2.right * (_verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, PassengersMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    struct PassengerMovement
    {
        public Transform PTransform;
        public Vector3 Velocity;
        public bool StandingOnPlatform;
        public bool MoveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            PTransform = _transform;
            Velocity = _velocity;
            StandingOnPlatform = _standingOnPlatform;
            MoveBeforePlatform = _moveBeforePlatform;
        }
    }

    void OnDrawGizmos()
    {
        if (LocalWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < LocalWaypoints.Length; i++)
            {
                Vector3 globalWaypoints = (Application.isPlaying) ? _globalWaypoints[i] : LocalWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypoints - Vector3.up * size, globalWaypoints + Vector3.up * size);
                Gizmos.DrawLine(globalWaypoints - Vector3.left * size, globalWaypoints + Vector3.left * size);
            }
        }    
    }
}
