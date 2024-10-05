namespace XUtil.Core.Collection.Tree
{
    public class TreeFactory
    {
        /// <summary>
        /// 创建树根
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ITreeRoot<T> CreateRoot<T>(T value) where T : struct
        {
            return new TreeNode<T> { Value = value };
        }
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ITreeNode<T> CreateNode<T>(T value) where T : struct
        {
            return new TreeNode<T> { Value = value };
        }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void AddChild<T>(ITree<T> parent, ITreeNode<T> child) where T : struct
        {
            parent.AddChild(child);
        }
        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void RemoveChild<T>(ITree<T> parent, ITreeNode<T> child) where T : struct
        {
            parent.RemoveChild(child);
        }
        /// <summary>
        /// DFS先序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        public static void FindChildDFSPreOrder<T>(ITree<T> root, T value, out List<T> findPath) where T : struct
        {
            root.FindChildDFSPreOrder(value, out findPath);
        }
        /// <summary>
        /// DFS中序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        public static void FindChildDFSInOrder<T>(ITree<T> root, T value, out List<T> findPath) where T : struct
        {
            root.FindChildDFSInOrder(value, out findPath);
        }
        /// <summary>
        /// DFS后序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="value"></param>
        /// <param name="findPath"></param>
        public static void FindChildDFSPostOrder<T>(ITree<T> root, T value, out List<T> findPath) where T : struct
        {
            root.FindChildDFSPostOrder(value, out findPath);
        }


    }
}
