using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinyIoc
{
    public interface ITinyRegistry
    {
        ITinyRegistry Register<T>();
        ITinyRegistry Register<TService, TImplementation>() where TImplementation : TService;
        ITinyRegistry Register<T>(Func<ITinyResolver, T> factory);
    }

    public interface ITinyResolver
    {
        T Resolve<T>();
    }

    public class TinyContainer : ITinyRegistry, ITinyResolver
    {
        private readonly IDictionary<Type, Type> _bindings;
        private readonly IDictionary<Type, Func<ITinyResolver, object>> _instanceFactory;

        public TinyContainer()
        {
            _bindings = new Dictionary<Type, Type>();
            _instanceFactory = new Dictionary<Type, Func<ITinyResolver, object>>();
        }

        public ITinyRegistry Register<T>() => Register<T, T>();

        public ITinyRegistry Register<TService, TImplementation>() where TImplementation : TService
        {
            var serviceKey = typeof(TService);

            ThrowIfContains(serviceKey);

            _bindings.Add(serviceKey, typeof(TImplementation));
            return this;
        }

        public ITinyRegistry Register<T>(Func<ITinyResolver, T> factory)
        {
            var serviceKey = typeof(T);

            ThrowIfContains(serviceKey);

            _instanceFactory.Add(serviceKey, f => factory(f));

            return this;
        }

        public T Resolve<T>() => (T)ResolveInternal(typeof(T));

        private object ResolveInternal(Type t)
        {
            if (_bindings.ContainsKey(t))
            {
                var ctrs = _bindings[t].GetConstructors();

                if (ctrs.Length == 0)
                {
                    return Activator.CreateInstance(t);
                }

                if (ctrs.Length > 1)
                {
                    throw new TinyMistake("Too many constructors.");
                }

                var instances = ctrs[0]
                    .GetParameters()
                    .Select(p => ResolveInternal(p.ParameterType))
                    .ToArray();

                return ctrs[0].Invoke(instances);
            }

            if (_instanceFactory.ContainsKey(t))
            {
                return _instanceFactory[t](this);
            }

            throw new TinyMistake($"Oh no. You didn't register {t.FullName}.");
        }

        private void ThrowIfContains(Type t)
        {
            if (_bindings.ContainsKey(t) || _instanceFactory.ContainsKey(t))
            {
                throw new TinyMistake($"You already registered {t.Name}.");   
            }
        }
    }

    public class TinyMistake : Exception
    {
        public TinyMistake(string message) : base(message) { }
    }
}
