using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    [Header("Lanes")]
    public float LaneWidth = 3f;
    public int LaneCount = 3;

    [Header("Movement")]
    public float SmoothTime = 0.3f;

    [Header("Steering")]
    public float MaxSteerAngle = 30f;
    public float SteerSpeed = 5f;
    public float SteerResetThreshold = 0.5f;

    [Header("Body Tilt")]
    public Transform CarBody;
    public float BodyTiltAngle = 10f;

    [Header("Wheel Meshes (вращение)")]
    public Transform FrontLeftWheel;
    public Transform FrontRightWheel;
    public Transform RearLeftWheel;
    public Transform RearRightWheel;
    public float WheelRotationMultiplier = 15f;

    [Header("References")]
    public ChunkManager ChunkManager;

    private int _currentLane;
    private float _targetX;
    private float _targetSteerAngle;
    private float _currentSteerAngle = 0f;
    private float _velocity = 0f;

    private float _frontLeftWheelRotation = 0f;
    private float _frontRightWheelRotation = 0f;

    private void Awake()
    {
        _currentLane = LaneCount / 2;
        _targetX = GetLaneX(_currentLane);
        transform.position = new Vector3(_targetX, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        HandleInput();
        MoveToTargetLane();
        RotateWheels();
    }

    private void OnTriggerEnter(Collider other)
    {
        Cone cone = other.GetComponent<Cone>();
        if (cone != null)
        {
            cone.FlyBack(transform.forward);
            CameraShake.Instance?.Shake();
        }
    }

    private void HandleInput()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            MoveLeft();
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            MoveRight();
        }
    }

    private void MoveLeft()
    {
        if (_currentLane > 0)
        {
            _currentLane--;
            _targetX = GetLaneX(_currentLane);
            _targetSteerAngle = -MaxSteerAngle;
            AnimateBodyTilt(-BodyTiltAngle);
        }
    }

    private void MoveRight()
    {
        if (_currentLane < LaneCount - 1)
        {
            _currentLane++;
            _targetX = GetLaneX(_currentLane);
            _targetSteerAngle = MaxSteerAngle;
            AnimateBodyTilt(BodyTiltAngle);
        }
    }

    private void AnimateBodyTilt(float angle)
    {
        if (CarBody == null) return;
        CarBody.DOKill();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(CarBody.DOLocalRotate(new Vector3(CarBody.localEulerAngles.x, angle, CarBody.localEulerAngles.z), SmoothTime * 0.4f));
        sequence.Append(CarBody.DOLocalRotate(new Vector3(CarBody.localEulerAngles.x, 0f, CarBody.localEulerAngles.z), SmoothTime * 0.4f));
    }

    private void MoveToTargetLane()
    {
        float newX = Mathf.SmoothDamp(transform.position.x, _targetX, ref _velocity, SmoothTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if (Mathf.Abs(transform.position.x - _targetX) < SteerResetThreshold)
        {
            _targetSteerAngle = 0f;
        }
    }

    private void RotateWheels()
    {
        float speed = ChunkManager != null ? ChunkManager.CurrentSpeed : 0f;
        float rotationAmount = speed * WheelRotationMultiplier * Time.deltaTime;

        _currentSteerAngle = Mathf.Lerp(_currentSteerAngle, _targetSteerAngle, SteerSpeed * Time.deltaTime);

        _frontLeftWheelRotation += rotationAmount;
        _frontRightWheelRotation += rotationAmount;

        FrontLeftWheel.localRotation = Quaternion.Euler(_frontLeftWheelRotation, _currentSteerAngle, 0);
        FrontRightWheel.localRotation = Quaternion.Euler(_frontRightWheelRotation, _currentSteerAngle, 0);

        RearLeftWheel.Rotate(rotationAmount, 0, 0, Space.Self);
        RearRightWheel.Rotate(rotationAmount, 0, 0, Space.Self);
    }

    private float GetLaneX(int lane)
    {
        float totalWidth = (LaneCount - 1) * LaneWidth;
        return -totalWidth / 2f + lane * LaneWidth;
    }
}