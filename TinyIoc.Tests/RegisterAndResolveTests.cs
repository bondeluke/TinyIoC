using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TinyIoc.Tests
{
    [TestClass]
    public class RegisterAndResolveTests
    {
        [TestMethod]
        public void Class()
        {
            var tiny = new TinyContainer();

            tiny.Register<Simple>();

            Assert.IsNotNull(tiny.Resolve<Simple>());
        }

        [TestMethod]
        public void Interface()
        {
            var tiny = new TinyContainer();

            tiny.Register<ISimple, Simple>();

            Assert.IsNotNull(tiny.Resolve<ISimple>());
        }

        [TestMethod]
        public void WithFactory()
        {
            var tiny = new TinyContainer();

            tiny.Register(factory => new Simple());

            Assert.IsNotNull(tiny.Resolve<Simple>());
        }

        [TestMethod]
        public void Delegate()
        {
            var tiny = new TinyContainer();

            tiny.Register<Simple>()
                .Register<Func<Simple>>(factory => factory.Resolve<Simple>);

            Assert.IsNotNull(tiny.Resolve<Func<Simple>>()());
        }

        [TestMethod]
        public void ComplexClass()
        {
            var tiny = new TinyContainer();

            tiny.Register<Simple>()
                .Register<ISimple, Simple>()
                .Register<Func<Simple>>(factory => () => new Simple())
                .Register<IComplex, Complex>();

            var simpleInstance = tiny.Resolve<IComplex>();

            Assert.IsNotNull(simpleInstance);
        }

        [TestMethod]
        public void ThrowsOnReRegister()
        {
            var tiny = new TinyContainer();

            tiny.Register<Simple>();

            Assert.ThrowsException<TinyError>(() => tiny.Register<Simple>());
            Assert.ThrowsException<TinyError>(() => tiny.Register(factory => new Simple()));
        }
    }

    public interface ISimple { }

    public class Simple : ISimple { }

    public interface IComplex { }

    public class Complex : IComplex
    {
        public Complex(Simple simple, ISimple iSimple, Func<Simple> complexClassFactory) { }
    }
}
