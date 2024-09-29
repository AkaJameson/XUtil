namespace Xin.DotnetUtil.Collection
{
    public interface ITree<T>: IEnumerable<T> where T : struct
    {
        T Value { get; set; }

        List<ITreeNode<T>> Children { get; set; }

        void AddChild(ITreeNode<T> child);

        void RemoveChild(ITreeNode<T> child);

        public void FindChildDFSPreOrder(T value, out List<T> findPath);

        public void FindChildDFSInOrder(T value, out List<T> findPath);

        public void FindChildDFSPostOrder(T value, out List<T> findPath);

    }
    public interface ITreeNode<T>:ITree<T> where T:struct
    {
        public ITree<T> parent { get; set; }

    }

    public interface ITreeRoot<T>: ITree<T> where T : struct
    {

    }

}
