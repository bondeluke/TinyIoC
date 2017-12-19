namespace TinyIoc
{
    public static class TinyExtensions
    {
        public static ITinyRegistry RegisterModule(this ITinyRegistry registry, ITinyModule module)
        {
            module.RegisterServices(registry);
            return registry;
        }

        public static ITinyRegistry RegisterModule<T>(this ITinyRegistry registry) where T : ITinyModule, new() => registry.RegisterModule(new T());
    }
}
