using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PatternTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            var Oyucnu = FluentFacroty<Player>.Init(new Player()).GiveAValue(x=>x.Name, "Yunus").GiveAValue(x=>x.Age, 50).Take();

        }
    }

    interface IFactory<T>
    {
        //IFactory<T> GiveAValue(string PropertyName, object Value);

        IFactory<T> GiveAValue(Expression<Func<T, object>> Property, object Value);

        T Take();
    }

    class Factory<T> : IFactory<T> where T : class
    {
        T _instance;
        public Factory(T instance)
        {
            _instance = instance;
        }

        //public IFactory<T> GiveAValue(string PropertyName, object Value)
        //{
        //    var pInfo = _instance.GetType().GetProperty(PropertyName);
        //    if (pInfo != null)
        //    {
        //        pInfo.SetValue(_instance, Value);
        //    }
        //    return this;
        //}

        public IFactory<T> GiveAValue(Expression<Func<T, object>> Property, object Value)
        {
            PropertyInfo pInfo = null;

            if (Property.Body is MemberExpression)
            {
                pInfo = (Property.Body as MemberExpression).Member as PropertyInfo;
            }
            else
            {
                pInfo = (((UnaryExpression)(Property.Body)).Operand as MemberExpression).Member as PropertyInfo;
            }

            pInfo.SetValue(_instance, Value);

            return this;
        }

        public T Take()
        {
            return _instance;
        }
    }

    static class FluentFacroty<T> where T : class
    {
        public static IFactory<T> Init(T Instance)
        {
            return new Factory<T>(Instance);
        }
    }

    class Player
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Tank
    {
        public int Id { get; set; }
        public string Side { get; set; }

    }

}
