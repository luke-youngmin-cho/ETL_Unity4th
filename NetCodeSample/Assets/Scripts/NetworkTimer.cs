public class NetworkTimer
{
    float _timer;
    public float minPeriod { get; }
    public uint ticks { get; private set; }

    public NetworkTimer(float tickRate)
    {
        minPeriod = 1f / tickRate;
    }

    public bool TryUpdate(float deltaTime)
    {
        bool ticked = false;
        if (_timer >= minPeriod)
        {
            _timer -= minPeriod;
            ticks++;
            ticked = true;
        }

        _timer += deltaTime;
        return ticked;
    }
}