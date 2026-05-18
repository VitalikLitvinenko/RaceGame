using UnityEngine;

public class SpeedCalculator : ISpeedCalculator
{
    public float CurrentSpeed { get; private set; } = 0;

    private float _startMoveSpeed;
    private float _maxSpeed;
    private float _speedIncreasePerSecond;

    public SpeedCalculator(float startMoveSpeed, float maxSpeed, float speedIncreasePerSecond)
    {
        _startMoveSpeed = startMoveSpeed;
        _maxSpeed = maxSpeed;
        _speedIncreasePerSecond = speedIncreasePerSecond;
    }

    public void RecalculateSpeed()
    {
        if (CurrentSpeed < _startMoveSpeed)
        {
            CurrentSpeed += _startMoveSpeed * Time.deltaTime;
            if (CurrentSpeed > _startMoveSpeed)
            {
                CurrentSpeed = _startMoveSpeed;
            }
        }
        else
        {
            if (CurrentSpeed < _maxSpeed)
            {
                CurrentSpeed += _speedIncreasePerSecond * Time.deltaTime;
            }
        }

        if (CurrentSpeed > _maxSpeed)
        {
            CurrentSpeed = _maxSpeed;
        }
    }
}