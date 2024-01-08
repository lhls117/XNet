using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNet.Foundation.Collections
{
    /// <summary>
    /// Represents a node in a Tree structure, with a parent node and zero or more child nodes.
    /// </summary>
    public class TreeNode<T> : ITreeNode<T> where T : class, ITreeNode<T>
    {
        private readonly IList<T> _children;
        private T _parent;

        public TreeNode()
        {
            _children = new List<T>();
        }

        #region Implementation of ITreeNode<T>

        public event EventHandler<T> NodeAdded;
        public event EventHandler<CancelEventArgs<T>> NodeAdding;
        public event EventHandler<T> NodeRemoved;
        public event EventHandler<CancelEventArgs<T>> NodeRemoving;

        /// <inheritdoc />
        public IEnumerable<T> Ancestors
        {
            get
            {
                if (Parent == null) yield break;
                yield return Parent;
                foreach (var node in Parent.Ancestors)
                    yield return node;
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> Children => _children;

        /// <inheritdoc />
        public int Depth => Parent?.Depth + 1 ?? 0;

        /// <inheritdoc />
        public IEnumerable<T> Descendants
        {
            get
            {
                foreach (var node in Children)
                {
                    yield return node;
                    foreach (var descendant in node.Descendants)
                        yield return descendant;
                }
            }
        }

        /// <inheritdoc />
        public int Height
        {
            get { return !Children.Any() ? 0 : Children.Max(n => n.Height) + 1; }
        }

        /// <inheritdoc />
        public IEnumerable<T> Nodes
        {
            get
            {
                yield return this as T;
                foreach (var node in Descendants)
                    yield return node;
            }
        }

        /// <inheritdoc />
        public T Parent
        {
            get => _parent;
            set
            {
                if (_parent == value) return;
                _parent?.Remove(this as T);
                _parent = value;
                _parent?.Add(this as T);
            }
        }

        /// <inheritdoc />
        public T Root => Parent == null ? this as T : Parent.Root;

        /// <inheritdoc />
        public void Add(T node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (_children.Contains(node))
                throw new ArgumentException(string.Format("{0} have exist.", nameof(node)));

            if (node.Descendants.Contains(this as T))
                throw new ArgumentException(string.Format("{0} recycle reference.", nameof(node)));

            var eventArgs = new CancelEventArgs<T> { Target = node };
            NodeAdding?.Invoke(this, eventArgs);
            if (eventArgs.Cancel) return;

            _children.Add(node);
            node.SetParent(this as T);
            NodeAdded?.Invoke(this, node);
        }

        /// <inheritdoc />
        public void Add(params T[] nodes)
        {
            foreach (var node in nodes)
            {
                Add(node);
            }
        }

        /// <inheritdoc />
        public void Remove(T node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            // if we don't have it, we can't remove it
            if (!_children.Contains(node))
                throw new ArgumentException(string.Format("{0} not exist.", nameof(node)));

            var eventArgs = new CancelEventArgs<T> { Target = node };
            NodeRemoving?.Invoke(this, eventArgs);
            if (eventArgs.Cancel) return;

            _children.Remove(node);
            node.SetParent(null);
            NodeRemoved?.Invoke(this, node);
        }

        /// <inheritdoc />
        public void Remove(params T[] nodes)
        {
            foreach (var node in nodes)
            {
                Remove(node);
            }
        }

        /// <inheritdoc />
        public void SetParent(T parent)
        {
            _parent = parent;
        }

        /// <inheritdoc />
        public void Traverse(Action<T> action, TraversalType traversalType = TraversalType.BreadthFirst)
        {
            if (traversalType == TraversalType.BreadthFirst)
            {
                foreach (var node in Nodes) action(node);
            }
            else if (traversalType == TraversalType.DepthFirst)
            {
            }
        }

        #endregion

        public override string ToString()
        {
            return "Depth=" + Depth + ", Height=" + Height + ", Children=" + _children.Count;
        }
    }
}
