using Core.EventAggregator.Interface;
using Zenject;

namespace Core.EventAggregator
{
    public class Binder : IBinder
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly DiContainer _container;
        // private static readonly Type EventAggregatorType = typeof(EventAggregator);
        // private static readonly Type PublishedType = typeof(IPublisher);

        public Binder(IEventAggregator eventAggregator, DiContainer container /*, List<IPublisher> publishers*/)
        {
            _eventAggregator = eventAggregator;
            _container = container;
            // foreach (var published in publishers)
            //     Bind(published);
        }

        // private void Bind(IPublisher published)
        // {
        //     var type = published.GetType();
        //     var properties = EventProperties(type);
        //     var method = EventAggregatorType.GetMethod("GetEvent", BindingFlags.Public | BindingFlags.Instance);
        //     foreach (var propertyInfo in properties)
        //     {
        //         var propertyType = propertyInfo.PropertyType;
        //         var returnType = propertyType.GetGenericArguments()[0];
        //         var eventResolve = method.MakeGenericMethod(returnType);
        //         var del = Delegate.CreateDelegate(propertyType, _eventAggregator, eventResolve);
        //         propertyInfo.SetValue(published, del);
        //     }
        // }

        // private static IEnumerable<PropertyInfo> EventProperties(Type type)
        // {
        //     var types = type.GetInterfaces().Where(i => i.IsGenericType && PublishedType.IsAssignableFrom(i));
        //     var list = new List<PropertyInfo>();
        //     foreach (var @interface in types)
        //     {
        //         var count = @interface.GenericTypeArguments.Length;
        //         for (var i = 1; i <= count; i++)
        //             list.Add(@interface.GetProperty($"Event{i}"));
        //     }
        //
        //     return list;
        // }

        public void Bind<TEvent>(IPublisher<TEvent> publisher) where TEvent : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2>(IPublisher<TEvent1, TEvent2> publisher)
            where TEvent1 : EventHubBase where TEvent2 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2, TEvent3>(IPublisher<TEvent1, TEvent2, TEvent3> publisher)
            where TEvent1 : EventHubBase where TEvent2 : EventHubBase where TEvent3 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event3 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent3>(_container);
                publisher.Event3 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2, TEvent3, TEvent4>(IPublisher<TEvent1, TEvent2, TEvent3, TEvent4> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event3 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent3>(_container);
                publisher.Event3 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event4 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent4>(_container);
                publisher.Event4 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5> publisher) where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event3 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent3>(_container);
                publisher.Event3 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event4 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent4>(_container);
                publisher.Event4 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event5 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent5>(_container);
                publisher.Event5 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6> publisher) where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event3 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent3>(_container);
                publisher.Event3 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event4 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent4>(_container);
                publisher.Event4 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event5 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent5>(_container);
                publisher.Event5 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event6 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent6>(_container);
                publisher.Event6 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
            where TEvent7 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event3 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent3>(_container);
                publisher.Event3 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event4 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent4>(_container);
                publisher.Event4 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event5 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent5>(_container);
                publisher.Event5 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event6 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent6>(_container);
                publisher.Event6 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event7 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent7>(_container);
                publisher.Event7 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
            where TEvent7 : EventHubBase
            where TEvent8 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event3 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent3>(_container);
                publisher.Event3 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event4 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent4>(_container);
                publisher.Event4 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event5 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent5>(_container);
                publisher.Event5 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event6 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent6>(_container);
                publisher.Event6 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event7 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent7>(_container);
                publisher.Event7 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event8 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent8>(_container);
                publisher.Event8 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9> publisher)
            where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
            where TEvent7 : EventHubBase
            where TEvent8 : EventHubBase
            where TEvent9 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event3 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent3>(_container);
                publisher.Event3 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event4 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent4>(_container);
                publisher.Event4 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event5 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent5>(_container);
                publisher.Event5 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event6 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent6>(_container);
                publisher.Event6 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event7 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent7>(_container);
                publisher.Event7 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event8 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent8>(_container);
                publisher.Event8 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event9 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent9>(_container);
                publisher.Event9 = () => eventAgg;
                return eventAgg;
            };
        }

        public void Bind<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10>(
            IPublisher<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10>
                publisher) where TEvent1 : EventHubBase
            where TEvent2 : EventHubBase
            where TEvent3 : EventHubBase
            where TEvent4 : EventHubBase
            where TEvent5 : EventHubBase
            where TEvent6 : EventHubBase
            where TEvent7 : EventHubBase
            where TEvent8 : EventHubBase
            where TEvent9 : EventHubBase
            where TEvent10 : EventHubBase
        {
            publisher.Event1 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent1>(_container);
                publisher.Event1 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event2 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent2>(_container);
                publisher.Event2 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event3 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent3>(_container);
                publisher.Event3 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event4 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent4>(_container);
                publisher.Event4 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event5 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent5>(_container);
                publisher.Event5 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event6 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent6>(_container);
                publisher.Event6 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event7 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent7>(_container);
                publisher.Event7 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event8 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent8>(_container);
                publisher.Event8 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event9 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent9>(_container);
                publisher.Event9 = () => eventAgg;
                return eventAgg;
            };
            publisher.Event10 = () =>
            {
                var eventAgg = _eventAggregator.GetEvent<TEvent10>(_container);
                publisher.Event10 = () => eventAgg;
                return eventAgg;
            };
        }
    }
}