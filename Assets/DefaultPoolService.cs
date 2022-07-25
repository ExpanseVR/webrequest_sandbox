using System;
using System.Collections.Generic;

    public class DefaultPoolService : IPoolService
    {
        private readonly Dictionary<Type, Queue<PoolBase>> allAvailable = new Dictionary<Type, Queue<PoolBase>>();
        private readonly Dictionary<Type, HashSet<PoolBase>> allUsed = new Dictionary<Type, HashSet<PoolBase>>();

        public T Get<T>() where T : PoolBase, new()
        {
            Type type = typeof(T);
            var available = GetAvailable(type);
            T poolInstance;
            if (available.Count > 0)
            {
                poolInstance = available.Dequeue() as T;
                while (poolInstance == null)
                {
                    //TODO: log error!
                    poolInstance = available.Dequeue() as T;
                }
                poolInstance.Reset();
            }
            else
            {
                poolInstance = new T();
            }

            var used = GetUsed(type);
            used.Add(poolInstance);

            return poolInstance;
        }

        public void Return(PoolBase instance)
        {
            Type type = instance.GetType();
            InternalReturn(type, instance);

        }

        public void Return<T>(T instance) where T : PoolBase, new()
        {
            Type type = typeof(T);
            InternalReturn(type, instance);
        }

        private Queue<PoolBase> GetAvailable(Type type)
        {
            if (!allAvailable.TryGetValue(type, out var queue))
            {
                queue = new Queue<PoolBase>();
                allAvailable.Add(type, queue);
            }

            return queue;
        }

        private HashSet<PoolBase> GetUsed(Type type)
        {
            if (!allUsed.TryGetValue(type, out var set))
            {
                set = new HashSet<PoolBase>();
                allUsed.Add(type, set);
            }

            return set;
        }

        private void InternalReturn(Type type, PoolBase instance)
        {
            var used = GetUsed(type);
            if (used.Contains(instance))
            {
                used.Remove(instance);
            }
            else
            {
                //TODO: Log error
            }

            var available = GetAvailable(type);
            available.Enqueue(instance);
        }
    }
