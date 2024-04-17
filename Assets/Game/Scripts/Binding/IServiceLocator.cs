namespace Game.Scripts.Binding
{
    public interface IServiceLocator
    {
        void AddService<TService, TInstance>(TInstance instance)
            where TService : IService
            where TInstance : TService;
        void RemoveService<T>(T service) where T : IService;
        T GetService<T>() where T : IService;
    }
}