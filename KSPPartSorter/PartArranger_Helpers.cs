using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TonyPartArranger
{
    public partial class PartArranger : MonoBehaviour
    {
        /// <summary>
        /// Sort the editor's part list and refresh the view
        /// </summary>
        public void RefreshPartList()
        {
            PartLoader.LoadedPartsList.Sort((part1, part2) => (
                SortedPart.FindByName(part1.name).Position.CompareTo(SortedPart.FindByName(part2.name).Position)
            ));

            if (lockedEditor) // Locking the editor prevents the view from changing so we have to unlock it, refresh the view, then lock it again
            {
                EditorLogic.fetch.Unlock("PartArrangerLock");
                EditorPartList.Instance.Refresh();
                EditorLogic.fetch.Lock(true, false, true, "PartArrangerLock");
            }
        }

        /// <summary>
        /// Changes the editor's selected category
        /// </summary>
        /// <param name="category"></param>
        public void ShowCategory(PartCategories category)
        {
            EditorPartList.Instance.categorySelected = (int)category;
            EditorPartList.Instance.Refresh();
        }

        /// <summary>
        /// Returns whether the given PartCategories is valid (one of the editor tabs)
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Boolean IsCategoryValid(PartCategories category)
        {
            return validCategories.Contains(category);
        }

        /// <summary>
        /// Returns the currently selected PartCategories
        /// </summary>
        /// <returns></returns>
        public PartCategories CurrentCategory()
        {
            return (PartCategories)EditorPartList.Instance.categorySelected;
        }

        /// <summary>
        /// Moves the given SortedPart up/down in its category
        /// </summary>
        /// <param name="part"></param>
        /// <param name="direction"></param>
        public void MovePart(SortedPart part, MoveDirection direction)
        {
            part.Move(direction); // Move the part itself
            RefreshPartList();    // Refresh the editor view
            needToScroll = true;  // Tell the GUI that it'll need to scroll the ScrollView next frame
        }

        /// <summary>
        /// Resets a category and redraws the part list
        /// </summary>
        /// <param name="category"></param>
        public void ResetCategory(PartCategories category)
        {
            SortedPart.ResetCategory(category);
            selectedPart = SortedPart.FindByCategory(category).First();
            RefreshPartList();
            SaveSettings();
            needToScroll = true;  // Tell the GUI that it'll need to scroll the ScrollView next frame
        }

        /// <summary>
        /// Save settings to the config file
        /// </summary>
        public void SaveSettings()
        {
            ConfigNode node = new ConfigNode();

            node.AddValue("WindowX", windowRect.xMin);
            node.AddValue("WindowY", windowRect.yMin);
            node.AddValue("WindowHeight", windowRect.height);
            node.AddValue("WindowLocked", windowLocked);
            node.AddValue("WindowShown", windowShown);
            node.AddValue("UseBlizzyToolbar", settings["UseBlizzyToolbar"]);

            foreach (PartCategories category in validCategories)
            {
                foreach (SortedPart part in SortedPart.FindByCategory(category))
                    node.AddValue(category.ToString(), part.Name);
            }

            node.Save(KSPUtil.ApplicationRootPath + "GameData/" + pluginDir + "/config.txt");
        }

        /// <summary>
        /// Load settings from the config file
        /// </summary>
        public void LoadSettings()
        {
            try // ConfigNode.Load throws a NullReferenceException on a missing or blank file
            {
                ConfigNode node = ConfigNode.Load(KSPUtil.ApplicationRootPath + "Gamedata/" + pluginDir + "/config.txt");
                String readValue;

                readValue = node.GetValue("UseBlizzyToolbar");
                if (readValue != null && readValue.ToLower() == "true")
                    settings["UseBlizzyToolbar"] = true;
                else
                    settings["UseBlizzyToolbar"] = false;

                readValue = node.GetValue("WindowLocked");
                if (readValue != null && readValue.ToLower() == "true")
                    windowLocked = true;
                else
                    windowLocked = false;

                readValue = node.GetValue("WindowShown");
                if (readValue != null && readValue.ToLower() == "true")
                    windowShown = true;
                else
                    windowShown = false;

                readValue = node.GetValue("WindowX");
                if (readValue != null)
                {
                    try
                    {
                        windowRect.x = Convert.ToInt32(readValue);
                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                }

                readValue = node.GetValue("WindowY");
                if (readValue != null)
                {
                    try
                    {
                        windowRect.y = Convert.ToInt32(readValue);
                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                }

                readValue = node.GetValue("WindowHeight");
                if (readValue != null)
                {
                    try
                    {
                        windowRect.height = Convert.ToInt32(readValue);
                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                }

                foreach (PartCategories category in validCategories)
                {
                    List<string> partList = node.GetValues(category.ToString()).ToList();
                    SortedPart.ApplyOrder(category, partList);
                }
            }
            catch (NullReferenceException)
            {
                print("[PartArranger] Missing or blank config file");
            }
        }
    }
}