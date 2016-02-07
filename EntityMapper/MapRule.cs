using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMapper
{
    internal abstract class MapRule
    {
        public Type Source { get; private set; }
        public Type Target { get; private set; }
        protected MapRule(Type source, Type target)
        {
            Source = source;
            Target = target;
        }
    }

    internal class MapRule<TSource, TTarget>: MapRule
    {
        private Action<TSource, TTarget> map;
        public Action<TSource, TTarget> Map
        {
            get { return map; }
            set
            {
                if (value == null)
                    map = (source, target) => { };
                else
                    map = value;
            }
        }

        public MapRule(Action<TSource, TTarget> map)
            : base(typeof(TSource), typeof(TTarget))
        {
            Map = map;
        }
    }
}
