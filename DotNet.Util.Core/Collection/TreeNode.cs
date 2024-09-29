using System.Collections;

namespace Xin.DotnetUtil.Collection
{

    public class TreeNode<T> : ITreeNode<T>, ITreeRoot<T> where T : struct
    {
        public ITree<T> parent { get; set; }
        public T Value { get; set; }
        public List<ITreeNode<T>> Children { get; set; }

        public TreeNode(T value)
        {
            this.Value = value;
            Children = new List<ITreeNode<T>>();
        }

        public TreeNode()
        {
            Children = new List<ITreeNode<T>>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TreeEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// 添加孩子
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(ITreeNode<T> child)
        {
            child.parent = this;
            Children.Add(child);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void RemoveChild(ITreeNode<T> child)
        {
            try
            {
                var treeNode = Children.First(entity => EqualityComparer<T>.Default.Equals(entity.Value, child.Value) &&
             EqualityComparer<List<ITreeNode<T>>>.Default.Equals(entity.Children, child.Children));
                if (treeNode == null) throw new Exception("当前节点中不包含这个子节点");
                Children.Remove(treeNode);
            }
            catch(Exception ex)
            {
                throw ex;
            }


        }
        /// <summary>
        /// DFS先序遍历
        /// </summary>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void FindChildDFSPreOrder(T value, out List<T> findPath)
        {
            findPath = new List<T>();
            DFSPreOrder(this, value, findPath); ;
        }
        /// <summary>
        /// DFS中序遍历
        /// </summary>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void FindChildDFSInOrder(T value, out List<T> findPath)
        {
            findPath = new List<T>();
            DFSInOrder(this, value, findPath);
        }
        /// <summary>
        /// DFS后序遍历
        /// </summary>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void FindChildDFSPostOrder(T value, out List<T> findPath)
        {
            findPath = new List<T>();
            DFSPostOrder(this, value, findPath);
        }
        /// <summary>
        /// DFS先序
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        /// <returns></returns>
        private bool DFSPreOrder(ITreeNode<T> node, T value, List<T> findPath)
        {
            findPath.Add(node.Value);
            if (EqualityComparer<T>.Default.Equals(node.Value, value))
            {
                return true;
            }

            foreach (var child in node.Children)
            {
                if (DFSPreOrder(child, value, findPath))
                {
                    return true;
                }
                findPath.RemoveAt(findPath.Count - 1);
            }

            return false;
        }
        /// <summary>
        /// DFS中序
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        /// <returns></returns>
        private bool DFSInOrder(ITreeNode<T> node, T value, List<T> findPath)
        {
            int mid = node.Children.Count / 2;

            for (int i = 0; i < mid; i++)
            {
                if (DFSInOrder(node.Children[i], value, findPath))
                {
                    return true;
                }
            }

            findPath.Add(node.Value);
            if (EqualityComparer<T>.Default.Equals(node.Value, value))
            {
                return true;
            }

            for (int i = mid; i < node.Children.Count; i++)
            {
                if (DFSInOrder(node.Children[i], value, findPath))
                {
                    return true;
                }
                if (i == mid)
                {
                    findPath.RemoveAt(findPath.Count - 1);
                }
            }

            return false;
        }
        /// <summary>
        /// DFS后序
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        /// <returns></returns>
        private bool DFSPostOrder(ITreeNode<T> node, T value, List<T> findPath)
        {
            foreach (var child in node.Children)
            {
                if (DFSPostOrder(child, value, findPath))
                {
                    return true;
                }
            }

            findPath.Add(node.Value);
            if (EqualityComparer<T>.Default.Equals(node.Value, value))
            {
                return true;
            }

            findPath.RemoveAt(findPath.Count - 1);
            return false;
        }

    }
    /// <summary>
    /// 对树进行DFS遍历的迭代实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeEnumerator<T> : IEnumerator<T> where T : struct
    {
        private readonly ITree<T> _root;
        private Queue<ITree<T>> _queue;
        private ITree<T> _current;

        public TreeEnumerator(ITree<T> root)
        {
            _root = root;
            _queue = new Queue<ITree<T>>();
            _queue.Enqueue(_root);
        }

        public T Current => _current.Value;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_queue.Count == 0)
                return false;

            _current = _queue.Dequeue();

            foreach (var child in _current.Children)
            {
                _queue.Enqueue(child);
            }

            return true;
        }

        public void Reset()
        {
            _queue.Clear();
            _queue.Enqueue(_root);
        }

        public void Dispose()
        {
            _queue = null;
            _current = null;
        }

    }
}
