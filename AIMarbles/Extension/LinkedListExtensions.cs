using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Extension
{
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Copies all elements from the source LinkedList up to and including the specified target element
        /// into a new LinkedList.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="sourceList">The source LinkedList to iterate through.</param>
        /// <param name="targetElement">The element to stop at and include in the new list.</param>
        /// <returns>
        /// A new LinkedList containing elements from the beginning of the sourceList up to and including
        /// the targetElement. If the targetElement is not found, all elements from the sourceList
        /// will be copied to the new list, and a warning will be printed to the console.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if sourceList is null.</exception>
        public static LinkedList<T> GetAllBefore<T>(this LinkedList<T> sourceList, T targetElement)
        {
            if (sourceList == null)
            {
                throw new ArgumentNullException(nameof(sourceList), "The source list cannot be null.");
            }
            // Note: targetElement itself can be null if T is a reference type or Nullable<T>,
            // EqualityComparer<T>.Default.Equals handles this correctly.

            LinkedList<T> newList = new LinkedList<T>();
            bool foundTarget = false;

            foreach (T element in sourceList)
            {
                newList.AddLast(element);

                if (EqualityComparer<T>.Default.Equals(element, targetElement))
                {
                    foundTarget = true;
                    break;
                }
            }

            if (!foundTarget && sourceList.Count > 0)
            {
                Console.WriteLine($"Warning (GetAllBefore): Target element '{targetElement}' was not found in the original list. All elements were copied.");
            }

            return newList;
        }

        /// <summary>
        /// Copies all elements from the source LinkedList from (and including) the specified target element
        /// to the end of the list into a new LinkedList.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="sourceList">The source LinkedList to iterate through.</param>
        /// <param name="targetElement">The element from which to start copying.</param>
        /// <returns>
        /// A new LinkedList containing elements from the targetElement to the end of the sourceList.
        /// If the targetElement is not found, an empty LinkedList will be returned, and a warning will be printed.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if sourceList is null.</exception>
        public static LinkedList<T> GetAllAfter<T>(this LinkedList<T> sourceList, T targetElement)
        {
            if (sourceList == null)
            {
                throw new ArgumentNullException(nameof(sourceList), "The source list cannot be null.");
            }

            LinkedList<T> newList = new LinkedList<T>();
            bool foundTarget = false;

            // Iterate through the list nodes to easily get the 'Next' elements
            LinkedListNode<T>? currentNode = sourceList.First;

            while (currentNode != null)
            {
                // If we've already found the target, or if the current node is the target
                if (foundTarget || EqualityComparer<T>.Default.Equals(currentNode.Value, targetElement))
                {
                    foundTarget = true; // Set flag to true if this is the target or if we're already past it
                    newList.AddLast(currentNode.Value); // Add the current element
                }
                currentNode = currentNode.Next; // Move to the next node
            }

            // Handle the case where the target element was not found
            if (!foundTarget && sourceList.Count > 0)
            {
                Console.WriteLine($"Warning (GetAllAfter): Target element '{targetElement}' was not found in the original list. An empty list was returned.");
            }

            return newList;
        }
    }
}
