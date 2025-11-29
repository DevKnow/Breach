public abstract class BuffModel
{
    public abstract DurationTrigger DurationTrigger { get; }
   
    public BuffData Data => _data;
    protected BuffData _data;
    
    public bool IsExpired => _duration <= 0;
    protected int _duration;

    protected int _stack;

    public void Initialize(BuffData data)
    {
        _data = data;
    }

    public void StackUp(BuffRequest request)
    {
        _stack += request.stack;

        if (!_data.isVolatile)
        {
            _duration += request.duration;
        }
    }

    public void ReduceDuration()
    {
        _duration--;
    }
}
