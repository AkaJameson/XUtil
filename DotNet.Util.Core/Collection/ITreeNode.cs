using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.Collection
{
    public interface ITree<T>: IEnumerable<T> where T : struct
    {
        T Value { get; set; }
        List<ITreeNode<T>> Children { get; set; }
        void AddChild(ITreeNode<T> child);

        void RemoveChild(ITreeNode<T> child);

        void FindChildDFSPreOrder(T value, out List<T> findPath);

        void FindChildDFSInOrder(T value, out List<T> findPath);

        void FindChildDFSPostOrder(T value, out List<T> findPath);
    }
    public interface ITreeNode<T>:ITree<T> where T:struct
    {
        public ITree<T> parent { get; set; }

    }

    public interface ITreeRoot<T>: ITree<T> where T : struct
    {

    }

}
