public interface ISpeedCalculator
{
    float CurrentSpeed { get; }
    void RecalculateSpeed();
}