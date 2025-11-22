using UnityEngine;

public sealed class InterpolatedTransform : MonoBehaviour
{
    private Vector3 _targetPosition;
    private Vector3 _velocity;

    // Time (in seconds) it takes to reach ~95% of the target
    private const float SmoothTime = 0.1f;

    public Vector3 Position => transform.position;

    public Vector3 TargetPosition
    {
        get => _targetPosition;
        set => _targetPosition = value;
    }

    public void SnapTo(Vector3 newPosition)
    {
        transform.position = newPosition;
        _targetPosition = newPosition;
        _velocity = Vector3.zero;
    }

    public void Update()
    {
        var deltaTime = Time.deltaTime;
        
        transform.position = Vector3.SmoothDamp(
            transform.position,
            _targetPosition,
            ref _velocity,
            SmoothTime,
            Mathf.Infinity,
            deltaTime
        );
    }
}