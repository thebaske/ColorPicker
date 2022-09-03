using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InjectModule
{
    public static class InjectorMaster<T>
    {
        //klase koje su dependency nekom 
        public static Dictionary<Type, T> dependencies = new Dictionary<Type, T>();

        //klase koje koriste dependencije
        private static List<IInjectable<T>> dependencyUsers = new List<IInjectable<T>>();

        private static bool dependencyAccepted = false;

        
        //klase koje se prijave kao dependency ( npr, neki manager )
        public static void AcceptDependency(Type tpe, T obj)
        {
            if (!dependencyAccepted)
            {
                dependencyAccepted = true;
                ServiceTicker.st.OnDisabled += Clearreferences;
            }
            dependencies.Add(tpe, obj);
           
        }

        public static void Clearreferences()
        {
            dependencyAccepted = false;
            ServiceTicker.st.OnDisabled -= Clearreferences;
            dependencies.Clear();
            dependencyUsers.Clear();
        }


        //Klase koje traze svoj dependency
        public static void GetDependency(Type requestedType, IInjectable<T> sender)
        {
            if (dependencies.ContainsKey(requestedType))
            {
                dependencies.TryGetValue(requestedType, out T result);
                sender.Inject(result);
            }
            else
            {
                dependencyUsers.Add(sender);
                ServiceTicker.st.AddSubscriber(QuerySenders);
            }
        }

        public static void QuerySenders()
        {
            bool queryComplete = false;
            for (int i = 0; i < dependencyUsers.Count; i++)
            {
                if (dependencies.ContainsKey(typeof(T)))
                {
                    dependencies.TryGetValue(typeof(T), out T result);
                    dependencyUsers[i].Inject(result);
                    queryComplete = true;
                }
            }

            if (queryComplete)
            {
                ServiceTicker.st.RemoveSubscriber(QuerySenders);
            }
        }
    }

    public interface IInjectable<T>
    {
        void Inject(T reference);
    }
}

