using System;
using System.Collections.Generic;
using UnityEngine;

namespace TonyPartArranger
{
    public partial class PartArranger : MonoBehaviour
    {
        /// <summary>
        /// Maintains a sorted list of parts that can be manipulated by the user
        /// </summary>
        public class SortedPart
        {
            private static Dictionary<PartCategories, List<SortedPart>> sortedPartCategories = new Dictionary<PartCategories, List<SortedPart>>();
            private String name;
            private String title;
            private PartCategories category;

            /*** Getters/setters ***/

            /// <summary>
            /// Internal name of the part
            /// </summary>
            public String Name
            {
                get { return name; }
                set { name = value; }
            }

            /// <summary>
            /// Display name of the part
            /// </summary>
            public String Title
            {
                get { return this.title; }
            }

            /// <summary>
            /// PartCategories this part belongs to
            /// </summary>
            public PartCategories Category
            {
                get { return category; }
                set { category = value; }
            }

            /// <summary>
            /// Position of this part in its category
            /// </summary>
            public int Position
            {
                get { return sortedPartCategories[this.Category].IndexOf(this); }
            }

            /// <summary>
            /// Returns what this part's position would be if its button were positioned at the cursor
            /// </summary>
            public int CursorPosition
            {
                get
                {
                    int mouseY = (int)Mouse.screenPos.y - (int)windowRect.y + (int)partScrollPosition.y - 68;
                    int position = mouseY / 23;
                    int thisCatCount = SortedPart.FindByCategory(this.Category).Count;


                    return (position < 0) ? 0 : (position > thisCatCount - 1) ? thisCatCount - 1 : position;
                }
            }

            /// <summary>
            /// Returns this part's button's position relative to the ScrollView
            /// </summary>
            public Rect ButtonPosition
            {
                get { return new Rect(3, (this.Position * 23) + 3, 268, 20); }
            }

            /// <summary>
            /// Returns this part's button's position in screen coordinates
            /// </summary>
            public Rect ButtonScreenPosition
            {
                get
                {

                    int x = 3;
                    int y = this.Position * 23 + 3;

                    // Handle scrollview
                    y -= (int)partScrollPosition.y;

                    // Convert to windowRect coordinates
                    x += 8;
                    y += 68;

                    // Convert to screen coordinates
                    x += (int)windowRect.x;
                    y += (int)windowRect.y;

                    return new Rect(x, y, 268, 20);
                }
            }

            /// <summary>
            /// Returns the internal part sort dictionary
            /// </summary>
            public static Dictionary<PartCategories, List<SortedPart>> SortedPartCategories
            {
                get { return sortedPartCategories; }
            }

            /// <summary>
            /// Creates a SortedPart
            /// </summary>
            /// <param name="part"></param>
            public SortedPart(AvailablePart part)
            {
                this.name = part.name;
                this.category = part.category;
                this.title = part.title;
                sortedPartCategories[category].Add(this);
            }

            /*** Static methods ***/

            /// <summary>
            /// Initialize the sorted part system, loading categories and parts
            /// </summary>
            public static void Init()
            {
                if (PartArranger.stockPartList == null)
                    PartArranger.stockPartList = new List<AvailablePart>(PartLoader.LoadedPartsList); // Clone the original part order so we can reset to defaults

                LoadCategories();
                LoadParts();
            }

            /// <summary>
            /// Initialize categories
            /// </summary>
            private static void LoadCategories()
            {
                foreach (PartCategories category in Enum.GetValues(typeof(PartCategories)))
                {
                    sortedPartCategories[category] = new List<SortedPart>();
                }
            }

            /// <summary>
            /// Loads all available parts
            /// </summary>
            private static void LoadParts()
            {
                List<AvailablePart> availableParts = PartLoader.LoadedPartsList;
                foreach (var part in availableParts)
                {
                    new SortedPart(part);
                }
            }

            /// <summary>
            /// Resets the order of the given category to default
            /// </summary>
            /// <param name="category"></param>
            public static void ResetCategory(PartCategories category)
            {
                sortedPartCategories[category] = new List<SortedPart>();

                foreach (var part in PartArranger.stockPartList)
                {
                    if (part.category == category)
                        new SortedPart(part);
                }
            }

            /// <summary>
            /// Applies an order to a category
            /// </summary>
            /// <param name="category"></param>
            /// <param name="partList"></param>
            public static void ApplyOrder(PartCategories category, List<String> partList)
            {
                List<String> reversedList = partList;
                reversedList.Reverse();

                foreach (string partName in reversedList)
                {
                    SortedPart part = SortedPart.FindByName(partName);

                    if (part != null)
                        SortedPart.FindByName(partName).Move(MoveDirection.Top);
                }
            }

            /// <summary>
            /// Returns a part by name
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public static SortedPart FindByName(String name)
            {
                return SortedPart.All().Find(x => x.Name == name);
            }

            /// <summary>
            /// Returns a list of parts by category
            /// </summary>
            /// <param name="category"></param>
            /// <returns></returns>
            public static List<SortedPart> FindByCategory(PartCategories category)
            {
                return SortedPart.All().FindAll(x => x.Category == category);
            }

            /// <summary>
            /// Returns all parts
            /// </summary>
            /// <returns></returns>
            public static List<SortedPart> All()
            {
                List<SortedPart> list = new List<SortedPart>();

                foreach (PartCategories category in Enum.GetValues(typeof(PartCategories)))
                {
                    list.AddRange(sortedPartCategories[category]);
                }

                return list;
            }

            /*** Instance methods ***/

            /// <summary>
            /// Moves the part up or down in its category
            /// </summary>
            /// <param name="direction"></param>
            public void Move(MoveDirection direction)
            {
                sortedPartCategories[this.Category].Move(this.Position, direction);
            }
        }
    }
}