using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.Xevprm
{
    public enum TreeTraversalType
    {
        DepthFirst,
        BreadthFirst
    }

    public enum TreeTraversalDirection
    {
        TopDown,
        BottomUp
    }

    /// <summary>
    /// Represents a hierarchy of objects or data. Tree is a root-level alias for TreeNode and SubtreeNode.
    /// </summary>
    public class Tree<T> : TreeNode<T> where T : new()
    {
        public Tree() { }
    }

    /// <summary>
    /// Represents a hierarchy of objects or data. Subtree is a root-level alias for Tree and TreeNode.
    /// </summary>
    public class Subtree<T> : TreeNode<T> where T : new()
    {
        public Subtree() { }
    }

    /// <summary>
    /// Represents a node in a Tree structure, with a parent node and zero or more child nodes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T> : IDisposable where T : new()
    {
        private TreeNode<T> _Parent;
        public TreeNode<T> Parent
        {
            get { return _Parent; }
            set
            {
                if (value == _Parent)
                    return;

                if (_Parent != null)
                    _Parent.Children.Remove(this);

                if (value != null && !value.Children.Contains(this))
                    value.Children.Add(this);

                _Parent = value;
            }
        }

        public TreeNode<T> Root
        {
            get
            {
                //return (Parent == null) ? this : Parent.Root;

                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
                return node;
            }
        }

        private TreeNodeList<T> _Children;
        public TreeNodeList<T> Children
        {
            get { return _Children; }
            private set { _Children = value; }
        }

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    _value = new T();
                }
                return _value;
            }
            set { _value = value; }
        }

        private TreeTraversalDirection _disposeTraversalDirection = TreeTraversalDirection.BottomUp;
        public TreeTraversalDirection DisposeTraversalDirection
        {
            get { return _disposeTraversalDirection; }
            set { _disposeTraversalDirection = value; }
        }

        private TreeTraversalType _disposeTraversalType = TreeTraversalType.DepthFirst;
        public TreeTraversalType DisposeTraversalType
        {
            get { return _disposeTraversalType; }
            set { _disposeTraversalType = value; }
        }

        public TreeNode()
        {
            Parent = null;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode(T Value)
        {
            this.Value = Value;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode(TreeNode<T> Parent)
        {
            this.Parent = Parent;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode(TreeNodeList<T> Children)
        {
            Parent = null;
            this.Children = Children;
            Children.Parent = this;
        }

        public TreeNode(TreeNode<T> Parent, TreeNodeList<T> Children)
        {
            this.Parent = Parent;
            this.Children = Children;
            Children.Parent = this;
        }

        /// <summary>
        /// Reports a depth of nesting in the tree, starting at 0 for the root.
        /// </summary>
        public int Depth
        {
            get
            {
                //return (Parent == null ? -1 : Parent.Depth) + 1;

                int depth = 0;
                var node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                    depth++;
                }
                return depth;
            }
        }

        public IEnumerable<TreeNode<T>> GetEnumerable(TreeTraversalType traversalType, TreeTraversalDirection traversalDirection)
        {
            switch (traversalType)
            {
                case TreeTraversalType.DepthFirst: return GetDepthFirstEnumerable(traversalDirection);
                case TreeTraversalType.BreadthFirst: return GetBreadthFirstEnumerable(traversalDirection);
                default: return null;
            }
        }

        private IEnumerable<TreeNode<T>> GetDepthFirstEnumerable(TreeTraversalDirection traversalDirection)
        {
            if (traversalDirection == TreeTraversalDirection.TopDown)
                yield return this;

            foreach (TreeNode<T> child in Children)
            {
                var e = child.GetDepthFirstEnumerable(traversalDirection).GetEnumerator();
                while (e.MoveNext())
                {
                    yield return e.Current;
                }
            }

            if (traversalDirection == TreeTraversalDirection.BottomUp)
                yield return this;
        }

        // TODO: adjust for traversal direction
        private IEnumerable<TreeNode<T>> GetBreadthFirstEnumerable(TreeTraversalDirection traversalDirection)
        {
            if (traversalDirection == TreeTraversalDirection.BottomUp)
            {
                var stack = new Stack<TreeNode<T>>();
                foreach (var item in GetBreadthFirstEnumerable(TreeTraversalDirection.TopDown))
                {
                    stack.Push(item);
                }
                while (stack.Count > 0)
                {
                    yield return stack.Pop();
                }
                yield break;
            }

            var queue = new Queue<TreeNode<T>>();
            queue.Enqueue(this);

            while (0 < queue.Count)
            {
                TreeNode<T> node = queue.Dequeue();

                foreach (TreeNode<T> child in node.Children)
                {
                    queue.Enqueue(child);
                }

                yield return node;
            }
        }

        public override string ToString()
        {
            string Description = "[" + (Value == null ? "<null>" : Value.ToString()) + "] ";

            Description += "Depth=" + Depth.ToString() + ", Children=" + Children.Count.ToString();

            if (Root == this)
                Description += " (Root)";

            return Description;
        }

        #region IDisposable

        private bool _IsDisposed;
        private T _value;

        public bool IsDisposed
        {
            get { return _IsDisposed; }
        }

        // TODO: update this to use GetEnumerator once that's working
        public virtual void Dispose()
        {
            CheckDisposed();

            // clean up contained objects (in Value property)
            if (DisposeTraversalDirection == TreeTraversalDirection.BottomUp)
            {
                foreach (var node in Children)
                {
                    node.Dispose();
                }
            }

            OnDisposing();

            if (DisposeTraversalDirection == TreeTraversalDirection.TopDown)
            {
                foreach (var node in Children)
                {
                    node.Dispose();
                }
            }

            _IsDisposed = true;
        }

        public event EventHandler Disposing;

        protected void OnDisposing()
        {
            if (Disposing != null)
            {
                Disposing(this, EventArgs.Empty);
            }
        }

        protected void CheckDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion
    }

    /// <summary>
    /// Contains a list of TreeNode (or TreeNode-derived) objects, with the capability of linking parents and children in both directions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNodeList<T> : List<TreeNode<T>> where T : new()
    {
        public TreeNode<T> Parent;

        public TreeNodeList(TreeNode<T> parent)
        {
            this.Parent = parent;
        }

        public new TreeNode<T> Add(TreeNode<T> node)
        {
            base.Add(node);
            node.Parent = Parent;
            return node;
        }

        public TreeNode<T> Add(T value)
        {
            var Node = new TreeNode<T>(Parent);
            Node.Value = value;
            return Node;
        }

        public override string ToString()
        {
            return "Count=" + Count.ToString();
        }
    }
}
