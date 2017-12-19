using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TinyIoc.Tests
{
    public class MyModule : ITinyModule
    {
        public void RegisterServices(ITinyRegistry registry)
        {
            registry.Register<Simple>();
        }
    }

    public class MyOtherModule : ITinyModule
    {
        public void RegisterServices(ITinyRegistry registry)
        {
            registry.Register<ISimple, Simple>();
        }
    }

    [TestClass]
    public class TinyModuleTests
    {
        [TestMethod]
        public void Basic()
        {
            var container = new TinyContainer();

            new MyModule().RegisterServices(container);

            Assert.IsNotNull(container.Resolve<Simple>());
        }

        [TestMethod]
        public void ExtensionMethods()
        {
            var container = new TinyContainer();

            new MyModule().RegisterServices(container);
            // or
            container.RegisterModule(new MyModule());
            // or
            container.RegisterModule<MyOtherModule>();

            Assert.IsNotNull(container.Resolve<Simple>());
            Assert.IsNotNull(container.Resolve<ISimple>());
        }
    }
}
