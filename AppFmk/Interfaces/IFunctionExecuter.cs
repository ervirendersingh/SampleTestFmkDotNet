using System;

namespace AppFmk.Interfaces
{
    public interface IFunctionExecuter
    {
        void Execute(Action action);
        T Execute<T>(Func<T> action);
    }
}
