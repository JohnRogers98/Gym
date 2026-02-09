namespace Gym.Application.Aspects
{
    internal interface ILockedRequest
    {
        String GetLockId();
        String GetLockOperation();
    }
}
