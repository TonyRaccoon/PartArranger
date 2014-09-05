using System.Collections.Generic;
using UnityEngine;

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

    public static class RectExtensions
    {
        /// <summary>
        /// Expands a Rect
        /// </summary>
        /// <param name="rect">The Rect to expand</param>
        /// <param name="left">How far (in pixels) to expand to the left</param>
        /// <param name="right">How far (in pixels) to expand to the right</param>
        /// <param name="up">How far (in pixels) to expand upward</param>
        /// <param name="down">How far (in pixels) to expand downward</param>
        /// <returns>Expanded Rect</returns>
        public static Rect Expand(this Rect rect, float left, float right, float up, float down)
        {
            Rect newRect = rect;

            newRect.x -= left;
            newRect.width += left + right;
            newRect.y -= up;
            newRect.height += up + down;

            return newRect;
        }

        /// <summary>
        /// Expands a Rect
        /// </summary>
        /// <param name="rect">The Rect to expand</param>
        /// <param name="horizontal">How far (in pixels) to expand left and right</param>
        /// <param name="vertical">How far (in pixels) to expand up and down</param>
        /// <returns>Expanded Rect</returns>
        public static Rect Expand(this Rect rect, float horizontal, float vertical)
        {
            Rect newRect = rect;

            newRect.x -= horizontal;
            newRect.width += horizontal * 2;
            newRect.y -= vertical;
            newRect.height += vertical * 2;

            return newRect;
        }

        /// <summary>
        /// Expands a Rect equally in all directions
        /// </summary>
        /// <param name="rect">The Rect to expand</param>
        /// <param name="amount">How far (in pixels) to expand all sides</param>
        /// <returns>Expanded Rect</returns>
        public static Rect Expand(this Rect rect, float amount)
        {
            Rect newRect = rect;

            newRect.x -= amount;
            newRect.width += amount * 2;
            newRect.y -= amount;
            newRect.height += amount*2;

            return newRect;
        }
    }
}