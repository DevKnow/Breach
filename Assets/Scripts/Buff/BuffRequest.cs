public struct BuffRequest
{
    public BuffKeyword id;
    public int stack;
    public int duration;
    public int delay;

    public BuffRequest(BuffKeyword id, int stack, int duration, int delay = 0)
    {
        this.id = id;
        this.stack = stack;
        this.duration = duration;
        this.delay = delay;
    }
}
