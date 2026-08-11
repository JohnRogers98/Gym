namespace Gym.WebApplication.Operations
{
    public class AsyncOperationStateNotifier<TRequest, TResponse>
    {
        public event Action<AsyncOperation<TResponse>>? Executed;

        public virtual void Notify(AsyncOperation<TResponse> state)
        {
            Executed?.Invoke(state);
        }
    }
}
