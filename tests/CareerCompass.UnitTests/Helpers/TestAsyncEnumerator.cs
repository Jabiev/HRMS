using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    public ValueTask DisposeAsync() => default;

    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
}
