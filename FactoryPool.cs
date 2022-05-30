using System;

namespace Natori.Pooling
{
    /// <summary>
    /// インスタンス生産もできるプール
    /// </summary>
    public sealed class FactoryPool<T> : IPoolReturnAcceptor<T>
    {
        private readonly Pool<T> _pool = new Pool<T>();

        private readonly Func<T> _factoryMethod;

        public int InstanceCount => _pool.InstanceCount;

        public bool IsEmpty => _pool.IsEmpty;

        public FactoryPool(Func<T> factoryMethod)
        {
            _factoryMethod = factoryMethod;
        }

        public void CreateAndPoolInstances(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _pool.ReturnInstance(_factoryMethod.Invoke());
            }
        }

        public T RentOrCreateInstance()
        {
            T instance = default(T);

            if (_pool.IsEmpty)
            {
                instance = _factoryMethod.Invoke();
            }
            else
            {
                instance = _pool.RentInstance();
            }

            return instance;
        }

        public void ReturnInstance(T instance)
        {
            _pool.ReturnInstance(instance);
        }

    }
}