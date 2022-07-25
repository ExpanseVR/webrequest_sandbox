using System;

public abstract class PoolBase : IEquatable<PoolBase>
{
    private static int _instanceNumber = 0;

    private readonly int instanceNumber;

    protected IPoolService PoolService {  get; private set; }

    public PoolBase()
    {
        instanceNumber = _instanceNumber++;
    }

    public abstract void Reset();

    public bool Equals(PoolBase other)
    {
        if (other == null) return false;
        return other.instanceNumber == instanceNumber;
    }

    public override int GetHashCode()
    {
        return instanceNumber;
    }

    public void SetPoolService(IPoolService poolService)
    {
        PoolService = PoolService ?? poolService;
    }

    public void Return()
    {
        PoolService.Return(this);
    }
}

public interface IPoolService
{
    T Get<T>() where T : PoolBase, new();
    void Return<T>(T instance) where T : PoolBase, new();

    void Return(PoolBase instance);
}
