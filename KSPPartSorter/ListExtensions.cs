using System.Collections.Generic;

namespace TonyPartArranger
{
    /// <summary>
    /// Controls which direction to move an element with Move()
    /// </summary>
    public enum MoveDirection
    {
        Up,
        Down,
        Top,
        Bottom
    }

    /// <summary>
    /// Extends IList with a method to move an element up or down
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Moves an element up or down in a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="indexToMove"></param>
        /// <param name="direction"></param>
        public static void Move<T>(this IList<T> list, int indexToMove, MoveDirection direction)
        {
            if (direction == MoveDirection.Up)
            {
                if (indexToMove == 0)
                    return;

                var item = list[indexToMove];
                list.RemoveAt(indexToMove);
                list.Insert(indexToMove - 1, item);
            }

            else if (direction == MoveDirection.Down)
            {
                if (indexToMove == list.Count - 1)
                    return;

                var item = list[indexToMove];
                list.RemoveAt(indexToMove);
                list.Insert(indexToMove + 1, item);
            }

            else if (direction == MoveDirection.Top)
            {
                if (indexToMove == 0)
                    return;

                var item = list[indexToMove];
                list.RemoveAt(indexToMove);
                list.Insert(0, item);
            }

            else if (direction == MoveDirection.Bottom)
            {
                if (indexToMove == list.Count - 1)
                    return;

                var item = list[indexToMove];
                list.RemoveAt(indexToMove);
                list.Add(item);
            }
        }
    }
}