/*
Copyright (c) 2014, Anthony Myre

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TonyPartArranger
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)] // Makes the plugin only load in the VAB and SPH
    public partial class PartArranger : MonoBehaviour
    {
        private String pluginVersion = "1.1.1";
        private SortedPart selectedPart;
        private Rect windowRect = new Rect((Screen.width) / 2 - 358 / 2, Screen.height / 2 - 600 / 2, 347, 444);
        private Vector2 partScrollPosition = new Vector2(0, 0);
        private Boolean windowShown = false;
        private Boolean windowShown_Astro = false;
        private Boolean windowLocked = false;
        private Boolean lockedEditor = false;
        private Boolean needToScroll = false;
        private Dictionary<String, Texture> buttonTextures = new Dictionary<String, Texture>();
        private float buttonClickTime = 0.0F;
        private Boolean isButtonDelayed = true;
        private String tooltipText = "";
        public List<PartCategories> validCategories = new List<PartCategories>();
        private Boolean usingBlizzyToolbar;
        private IButton blizzyButton;
        private ApplicationLauncherButton appButton;
        private Dictionary<String, GUIStyle> buttonStyles = new Dictionary<String, GUIStyle>();
        private Dictionary<String, GUILayoutOption[]> buttonLayouts = new Dictionary<String, GUILayoutOption[]>();
        private Dictionary<String, int> cursorIn = new Dictionary<String, int>();
        private int originalWindowHeight;
        private int originalMousePosition;
        private Boolean resizing = false;
        private Dictionary<String, object> settings = new Dictionary<String, object>();

        private static List<AvailablePart> stockPartList;

        // Configurable variables
        private readonly float moveRepeatDelay = 0.5F;
        private readonly float moveRepeatInterval = 0.1F;
        private readonly String pluginDir = "PartArranger";
        private readonly int minimumHeight = 267;
        
        /// <summary>
        /// Runs when the plugin is initialized
        /// </summary>
        public void Start()
        {
            validCategories.Add(PartCategories.Pods);
            validCategories.Add(PartCategories.Propulsion);
            validCategories.Add(PartCategories.Control);
            validCategories.Add(PartCategories.Structural);
            validCategories.Add(PartCategories.Aero);
            validCategories.Add(PartCategories.Utility);
            validCategories.Add(PartCategories.Science);

            SortedPart.Init();
            LoadSettings();
            selectedPart = SortedPart.FindByCategory(PartCategories.Pods).First();
            RefreshPartList();

            usingBlizzyToolbar = (Boolean)settings["UseBlizzyToolbar"] && ToolbarManager.ToolbarAvailable;

            if (!(Boolean)settings["HidePlugin"])
            {
                buttonTextures.Add("CatPods", GameDatabase.Instance.GetTexture(pluginDir + "/icons/CatPods", false));
                buttonTextures.Add("CatPropulsion", GameDatabase.Instance.GetTexture(pluginDir + "/icons/CatPropulsion", false));
                buttonTextures.Add("CatControl", GameDatabase.Instance.GetTexture(pluginDir + "/icons/CatControl", false));
                buttonTextures.Add("CatStructural", GameDatabase.Instance.GetTexture(pluginDir + "/icons/CatStructural", false));
                buttonTextures.Add("CatAero", GameDatabase.Instance.GetTexture(pluginDir + "/icons/CatAero", false));
                buttonTextures.Add("CatUtility", GameDatabase.Instance.GetTexture(pluginDir + "/icons/CatUtility", false));
                buttonTextures.Add("CatScience", GameDatabase.Instance.GetTexture(pluginDir + "/icons/CatScience", false));
                buttonTextures.Add("MoveTop", GameDatabase.Instance.GetTexture(pluginDir + "/icons/MoveTop", false));
                buttonTextures.Add("MoveUp", GameDatabase.Instance.GetTexture(pluginDir + "/icons/MoveUp", false));
                buttonTextures.Add("MoveDown", GameDatabase.Instance.GetTexture(pluginDir + "/icons/MoveDown", false));
                buttonTextures.Add("MoveBottom", GameDatabase.Instance.GetTexture(pluginDir + "/icons/MoveBottom", false));
                buttonTextures.Add("MoveReset", GameDatabase.Instance.GetTexture(pluginDir + "/icons/MoveReset", false));
                buttonTextures.Add("Toolbar", GameDatabase.Instance.GetTexture(pluginDir + "/icons/ToolbarIcon", false));
                buttonTextures.Add("Locked", GameDatabase.Instance.GetTexture(pluginDir + "/icons/Locked", false));
                buttonTextures.Add("Unlocked", GameDatabase.Instance.GetTexture(pluginDir + "/icons/Unlocked", false));
                buttonTextures.Add("Close", GameDatabase.Instance.GetTexture(pluginDir + "/icons/Close", false));
                buttonTextures.Add("ButtonBGOn", GameDatabase.Instance.GetTexture(pluginDir + "/icons/ButtonBGOn", false));
                buttonTextures.Add("ButtonBGOff", GameDatabase.Instance.GetTexture(pluginDir + "/icons/ButtonBGOff", false));
                buttonTextures.Add("ButtonBGActive", GameDatabase.Instance.GetTexture(pluginDir + "/icons/ButtonBGActive", false));
                buttonTextures.Add("ButtonBGHover", GameDatabase.Instance.GetTexture(pluginDir + "/icons/ButtonBGHover", false));
                buttonTextures.Add("CursorResizeNS", GameDatabase.Instance.GetTexture(pluginDir + "/icons/CursorResizeNS", false));

                GameEvents.onGUIAstronautComplexSpawn.Add(OnAstroComplexShown);
                GameEvents.onGUIAstronautComplexDespawn.Add(OnAstroComplexHidden);

                if (usingBlizzyToolbar)
                {
                    blizzyButton = ToolbarManager.Instance.add("TonyPartArranger", "PartArrangerButton");
                    blizzyButton.TexturePath = pluginDir + "/icons/BlizzyIconOff";
                    blizzyButton.ToolTip = "Show or hide PartArranger";
                    blizzyButton.Visibility = new GameScenesVisibility(GameScenes.EDITOR, GameScenes.SPH);
                    blizzyButton.OnClick += (e) =>
                    {
                        if (windowShown)
                            ToolbarHide();
                        else
                            ToolbarShow();
                    };

                    if (windowShown)
                        blizzyButton.TexturePath = pluginDir + "/icons/BlizzyIconOn";
                }
                else
                    GameEvents.onGUIApplicationLauncherReady.Add(ApplicationLauncherReady);
            }

            print("[PartArranger] Loaded version " + pluginVersion);
        }

        /// <summary>
        /// Runs each frame
        /// </summary>
        public void Update()
        {
            if ((Boolean)settings["HidePlugin"] || !windowShown)
                return; // Don't handle resizing if the plugin is hidden or if the window isn't being shown

            // Fix reversed y position in mouse coordinates
            Vector3 mousePos = Input.mousePosition;
            mousePos.y = Screen.height - mousePos.y;

            Boolean cursorInZone = new Rect(windowRect.x, windowRect.yMax - 5, windowRect.width, 5).Contains(mousePos);

            if (Input.GetMouseButtonDown(0) && !windowLocked && cursorInZone)
            {
                resizing = true;
                originalWindowHeight = (int)windowRect.height;
                originalMousePosition = (int)Mouse.screenPos.y;
                Cursor.SetCursor((Texture2D)buttonTextures["CursorResizeNS"], new Vector2(11, 11), CursorMode.ForceSoftware);
            }
            else if (Input.GetMouseButtonUp(0) && resizing)
            {
                resizing = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }

            if (cursorInZone && !windowLocked)
                Cursor.SetCursor((Texture2D)buttonTextures["CursorResizeNS"], new Vector2(11, 11), CursorMode.ForceSoftware);
            else if (!cursorInZone && !resizing)
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            if (resizing && windowRect.height >= minimumHeight)
                windowRect.height = originalWindowHeight - (originalMousePosition - Mouse.screenPos.y);

            if (windowRect.height < minimumHeight)
                windowRect.height = minimumHeight;
        }

        /// <summary>
        /// Runs when the AppLauncher toolbar is ready to accept new buttons
        /// </summary>
        public void ApplicationLauncherReady()
        {
            appButton = ApplicationLauncher.Instance.AddModApplication(ToolbarShow, ToolbarHide, null, null, null, null, ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB, buttonTextures["Toolbar"]);

            if (windowShown)
                appButton.SetTrue();
        }

        /// <summary>
        /// Runs when the plugin is unloaded
        /// </summary>
        public void OnDestroy()
        {
            SaveSettings();

            if (!(Boolean)settings["HidePlugin"])
            {
                EditorLogic.fetch.Unlock("PartArrangerLock"); // Unlock the editor, just in case

                GameEvents.onGUIAstronautComplexSpawn.Remove(OnAstroComplexShown);
                GameEvents.onGUIAstronautComplexDespawn.Remove(OnAstroComplexHidden);

                if (usingBlizzyToolbar)
                    blizzyButton.Destroy();
                else
                {
                    ApplicationLauncher.Instance.RemoveModApplication(appButton);
                    GameEvents.onGUIApplicationLauncherReady.Remove(ApplicationLauncherReady);
                }
            }
        }

        /// <summary>
        /// Runs when the toolbar button is clicked to show the window
        /// </summary>
        public void ToolbarShow()
        {
            windowShown = true;

            if (usingBlizzyToolbar)
                blizzyButton.TexturePath = pluginDir + "/icons/BlizzyIconOn";
        }

        /// <summary>
        /// Runs when the toolbar button is clicked to hide the window
        /// </summary>
        public void ToolbarHide()
        {
            windowShown = false;
            SaveSettings();

            if (usingBlizzyToolbar)
                blizzyButton.TexturePath = pluginDir + "/icons/BlizzyIconOff";
        }

        /// <summary>
        /// Runs when the Astronaut Complex is shown inside the editor
        /// </summary>
        public void OnAstroComplexShown()
        {
            windowShown_Astro = windowShown;
            windowShown = false;
        }

        /// <summary>
        /// Runs when the Astronaut Complex is hidden inside the editor
        /// </summary>
        public void OnAstroComplexHidden()
        {
            windowShown = windowShown_Astro;
        }
    }
}