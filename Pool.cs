using System;

namespace Natori.Pooling
{
    /// <summary>
    /// 純粋なプール（インスタンスの生成を行わない）
    /// </summary>
    public sealed class Pool<T>
    {
        private T[] _queue;

        private int _length;

        public int InstanceCount => _length;

        public bool IsEmpty => 0 == _length;

        private readonly int _additionalBufferSizeWhenResizing;

        public Pool(int initialBufferSize = 16,int additionalBufferSizeWhenResizing = 16)
        {
            _queue = new T[initialBufferSize];
            _additionalBufferSizeWhenResizing = additionalBufferSizeWhenResizing;
        }

        public T RentInstance()
        {
            var lengthMinusOne = _length - 1;
            var instance = _queue[lengthMinusOne];
            _queue[lengthMinusOne] = default;//ムムー
            _length = lengthMinusOne;
            return instance;
        }

        public void ReturnInstance(T instance)
        {
            if (_queue.Length <= _length)
            {
                Array.Resize(ref _queue,_queue.Length + _additionalBufferSizeWhenResizing);
            }
            _queue[_length] = instance;
            _length++;
        }
    }
}