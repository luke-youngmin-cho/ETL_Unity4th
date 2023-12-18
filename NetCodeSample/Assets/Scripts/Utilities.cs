using System;
using Unity.VisualScripting;

public abstract class Timer
{
    public uint ticks { get; private set; }
    protected float initTime;
    protected float time { get; set; }
    public bool isRunning { get; protected set; }
    public float progress => time / initTime;

    public Action onStart = delegate { };
    public Action onStop = delegate { };

    protected Timer(float initTime)
    {
        this.initTime = initTime;
        this.isRunning = false;
    }

    public void Start()
    {
        time = initTime;
        if (!isRunning)
        {
            isRunning = true;
            onStart.Invoke();
        }
    }

    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            time = 0f;
            onStop.Invoke();
        }
    }

    public void Pause() => isRunning = false;
    public void Resume() => isRunning = true;

    public virtual void Tick(float deltaTime)
    {
        ticks++;
    }
}

public class CountdownTimer : Timer
{
    public CountdownTimer(float initTime) : base(initTime)
    {
    }


    public bool isFinished => time <= 0;

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (isRunning)
        {
            if (time > 0)
                time -= deltaTime;

            if (time <= 0)
                Stop();
        }
    }

    public void Reset() => time = initTime;
    public void Reset(float newTime) => time = newTime;
}