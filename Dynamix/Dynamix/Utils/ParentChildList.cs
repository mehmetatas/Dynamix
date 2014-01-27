using System.Collections;
using System.Collections.Generic;

namespace Dynamix.Utils
{
    public class ParentChildList<TParent, TChild> : IParentChildList<TParent, TChild>
        where TParent : class, IParent<TParent, TChild>
        where TChild : class, IChild<TParent>
    {
        private readonly List<TChild> _children;
        private readonly TParent _parent;

        public ParentChildList(TParent parent)
        {
            _children = new List<TChild>();
            _parent = parent;
        }

        public void Add(TChild child)
        {
            if (child.Parent != _parent && child.Parent != null)
                child.Parent.Children.Remove(child);

            if (_children.Contains(child)) 
                return;

            _children.Add(child);
            child.Parent = _parent;
        }

        public bool Remove(TChild child)
        {
            if (!_children.Remove(child))
                return false;
            child.Parent = null;
            return true;
        }

        public TChild this[int index]
        {
            get { return _children[index]; }
        }

        public int Count
        {
            get { return _children.Count; }
        }

        public IEnumerator<TChild> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_children as IEnumerable).GetEnumerator();
        }
    }

    public class Child<TParent, TChild> : IChild<TParent>
        where TParent : class, IParent<TParent, TChild>
        where TChild : class, IChild<TParent>
    {
        private readonly TChild _child;

        public Child(TChild child)
        {
            _child = child;
        }

        private TParent _parent;
        public TParent Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;

                if (_parent != null)
                    _parent.Children.Remove(_child);

                _parent = value;

                if (_parent != null)
                    _parent.Children.Add(_child);
            }
        }
    }

    public interface IParentChildList<TParent, TChild> : IEnumerable<TChild>
        where TParent : class, IParent<TParent, TChild>
        where TChild : class, IChild<TParent>
    {
        void Add(TChild child);
        bool Remove(TChild child);
        TChild this[int index] { get; }
        int Count { get; }
    }

    public interface IParent<TParent, TChild>
        where TParent : class, IParent<TParent, TChild>
        where TChild : class, IChild<TParent>
    {
        IParentChildList<TParent, TChild> Children { get; }
    }

    public interface IChild<TParent>
    {
        TParent Parent { get; set; }
    }
}
