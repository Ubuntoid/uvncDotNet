// Filename: FlatMenu.cs
// Desc: Flat menu framework code.
// 2005-11-07 nschan Initial revision.
// 2005-11-10 nschan MenuId property is now non-browsable.
// 2005-11-12 nschan Added InvalidateMenu() to refresh control
//                   when certain properties are changed.
// 2005-11-14 nschan Set TabStop property to false by default.
// 2005-11-17 nschan Added EnableBorderDrawing, EnableHoverBackDrawing,
//                   and HoverTextFont properties.
// 2006-01-27 nschan Removed SecondaryBackColor property, added
//                   BackGradientColor, HoverBackGradientColor,
//                   EnableHoverBorderDrawing, and PopupMenu properties.
// 2016-08-24 Ubuntoid Removed GradientColor

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace uvncDotNet.Controls
{
    /// <summary>
    /// FlatMenuAlignment is an enum defining alignment values
    /// for use with the FlatPopupMenu.TrackPopupMenu() method.
    /// Value names are borrowed from VC++ TrackPopupMenu usage.
    /// </summary>
    public enum FlatMenuAlignment
    {
        CenterAlign,
        LeftAlign,
        RightAlign,
        BottomAlign,
        TopAlign,
        VCenterAlign
    }

    /// <summary>
    /// FlatMenuItemList is a collection of menu items
    /// that appear on the same menu.
    /// </summary>
    public class FlatMenuItemList
    {
        #region Constants
        private static readonly int MaxItemCount = 30;
        #endregion Constants

        #region Private fields
        private ArrayList menuItems = new ArrayList();
        #endregion Private fields

        #region Properties
        /// <summary>
        /// Get the number of menu items in the list.
        /// </summary>
        public int Count
        {
            get { return this.menuItems.Count; }
        }
        #endregion Properties

        #region Constructor
        public FlatMenuItemList()
        {
        }
        #endregion Constructor

        #region Public methods
        /// <summary>
        /// Add the given menu item to the list. The menu item
        /// is appended to the existing list.
        /// </summary>
        /// <param name="item">The menu item to add.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Add(FlatMenuItem item)
        {
            if (!CanAddItem())
                return false;

            this.menuItems.Add(item);
            return true;
        }

        /// <summary>
        /// Add a new menu item given the text and
        /// Click event handler.
        /// </summary>
        /// <param name="text">Menu item text string.</param>
        /// <param name="clickHandler">Click event handler. Can be null.</param>
        /// <returns>The newly created menu item if successful, null otherwise.</returns>
        public FlatMenuItem Add(string text, System.EventHandler clickHandler)
        {
            if (!CanAddItem())
                return null;

            var item = new FlatMenuItem {Text = text};
            if (clickHandler != null)
                item.Click += clickHandler;
            if (!this.Add(item))
                return null;

            return item;
        }

        /// <summary>
        /// Add a new separator menu item to the list.
        /// Does the work of creating a new FlatMenuItem,
        /// setting its properties to indicate that it is
        /// a separator item, and then appending it to the
        /// current list of menu items.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool AddSeparator()
        {
            if (!CanAddItem())
                return false;

            var item = new FlatMenuItem
            {
                Selectable = false,
                Style = FlatMenuItemStyle.Separator,
                Text = string.Empty
            };
            return this.Add(item);
        }

        /// <summary>
        /// Remove all menu items from the list.
        /// </summary>
        public void Clear()
        {
            this.menuItems.Clear();
        }

        /// <summary>
        /// Get the menu item at the given index.
        /// </summary>
        /// <param name="index">The index of the menu item in the list.</param>
        /// <returns>The menu item at the specified index, or null if the index is out of bounds.</returns>
        public FlatMenuItem GetAt(int index)
        {
            FlatMenuItem item = null;
            if (0 <= index && index < this.menuItems.Count)
            {
                item = this.menuItems[index] as FlatMenuItem;
            }
            return item;
        }

        /// <summary>
        /// Remove the specified menu item from the list.
        /// </summary>
        /// <param name="item">The menu item to remove.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Remove(FlatMenuItem item)
        {
            if (!this.menuItems.Contains(item))
                return false;

            this.menuItems.Remove(item);
            return true;
        }

        /// <summary>
        /// Remove the menu item at the specified index.
        /// </summary>
        /// <param name="index">The index of the menu item to remove.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= this.menuItems.Count)
                return false;

            this.menuItems.RemoveAt(index);
            return true;
        }
        #endregion Public methods

        #region Private methods
        /// <summary>
        /// Check if we can add a new menu item.
        /// </summary>
        /// <returns>true if we can add, false otherwise.</returns>
        private bool CanAddItem()
        {
            return this.menuItems.Count < FlatMenuItemList.MaxItemCount;
        }

        #endregion Private methods

        #region ToString override
        /// <summary>
        /// Print out the ToString() value for each
        /// menu item in the list. Useful for debugging.
        /// </summary>
        /// <returns>List of menu item text values.</returns>
        public override string ToString()
        {
            if (this.menuItems.Count == 0)
            {
                return "FlatMenuItemList: Empty";
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append("FlatMenuItemList:");
                foreach (var obj in this.menuItems)
                {
                    var item = obj as FlatMenuItem;
                    sb.Append('\n');
                    sb.Append(item);
                }
                return sb.ToString();
            }
        }
        #endregion ToString override
    }

    /// <summary>
    /// FlatMenuItemStyle is an enumeration defining
    /// different menu item styles.
    /// </summary>
    public enum FlatMenuItemStyle
    {
        Regular,
        Separator,
        Check,
        Radio
    }

    /// <summary>
    /// FlatMenuItem is a simple data class that stores
    /// information about a single menu item. A FlatMenuItem
    /// instance itself may also contain one or more child
    /// menu items as part of a sub-menu which extends off
    /// that instance.
    /// </summary>
    public class FlatMenuItem
    {
        #region Events
        public event System.EventHandler Click;
        #endregion Events

        #region Private fields
        private FlatMenuItemStyle style = FlatMenuItemStyle.Regular;

        private Rectangle clientRectangle;

        private bool enabled = true;
        private bool selectable = true;

        private bool isChecked = false;
        private bool radio = false;

        private string text = "Item";

        private FlatMenuItemList menuItems = new FlatMenuItemList();
        #endregion Private fields

        #region Properties
        /// <summary>
        /// Get or set the menu item style.
        /// </summary>
        public FlatMenuItemStyle Style
        {
            get { return this.style; }
            set { this.style = value; }
        }

        /// <summary>
        /// Get or set the client rectangle for the menu item.
        /// </summary>
        public Rectangle ClientRectangle
        {
            get { return this.clientRectangle; }
            set { this.clientRectangle = value; }
        }

        /// <summary>
        ///  Get or set the enabled state of the menu item.
        ///  If a menu item is disabled, it cannot be highlighted
        ///  or clicked. It is also drawn with a different text
        ///  color. If a disabled menu item has child items, the
        ///  submenu containing those items will not be accessible.
        /// </summary>
        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        /// <summary>
        ///  Get or set the selectable state of the menu item.
        ///  Unlike the disabled state, if a menu item is not
        ///  selectable, it does not appear with a different
        ///  text color. Used primarily to indicate that
        ///  separator items are not selectable.
        /// </summary>
        public bool Selectable
        {
            get { return this.selectable; }
            set { this.selectable = value; }
        }

        /// <summary>
        /// Get or set the checked state of the menu item.
        /// </summary>
        public bool Checked
        {
            get { return this.isChecked; }
            set { this.isChecked = value; }
        }

        /// <summary>
        /// Get or set the radio state of the menu item.
        /// </summary>
        public bool Radio
        {
            get { return this.radio; }
            set { this.radio = value; }
        }

        /// <summary>
        /// Get or set the displayable text string for the menu item.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        /// <summary>
        /// Get or set the list of child menu items.
        /// </summary>
        public FlatMenuItemList MenuItems
        {
            get { return this.menuItems; }
            set { this.menuItems = value; }
        }

        /// <summary>
        /// Check if the menu item has a Click event handler.
        /// </summary>
        public bool HasClickHandler
        {
            get { return this.Click != null; }
        }
        #endregion Properties

        #region Constructor
        public FlatMenuItem()
        {
        }
        #endregion Constructor

        #region Public methods
        /// <summary>
        /// Helper function to send a menu item Click event
        /// if appropriate.
        /// </summary>
        public void NotifyClickEvent()
        {
            if (this.Enabled && this.Selectable)
            {
                var tmpClick = this.Click;
                tmpClick?.Invoke(this, System.EventArgs.Empty);
            }
        }
        #endregion Public methods

        #region ToString override
        /// <summary>
        /// Return the Text property value.
        /// </summary>
        /// <returns>The Text property value.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (this.Style == FlatMenuItemStyle.Separator)
                sb.Append("Separator");
            else
                sb.Append(this.Text);

            return sb.ToString();
        }
        #endregion ToString override
    }

    /// <summary>
    /// CurrentMenuItemChangeEventArgs is a custom event argument
    /// class used to store the current FlatMenuItem that is
    /// being highlighted as a user navigates a menu. When no
    /// menu item is being highlighted, the current menu item
    /// property of this class will be set to null.
    /// </summary>
    public class CurrentMenuItemChangeEventArgs : System.EventArgs
    {
        #region Private fields
        private FlatMenuItem currentMenuItem;
        #endregion Private fields

        #region Properties
        /// <summary>
        /// Get or set the currently highlighted menu item.
        /// If no menu item is currently highlighted, this
        /// value should be set to null.
        /// </summary>
        public FlatMenuItem CurrentMenuItem
        {
            get { return this.currentMenuItem; }
            set { this.currentMenuItem = value; }
        }
        #endregion Properties

        #region Constructor
        public CurrentMenuItemChangeEventArgs()
        {
        }
        #endregion Constructor
    }

    /// <summary>
    /// CurrentMenuItemChangeEventHandler is a delegate that
    /// defines the method prototype for a current menu item
    /// change event handler.
    /// </summary>
    public delegate void CurrentMenuItemChangeEventHandler(object sender, CurrentMenuItemChangeEventArgs e);

    /// <summary>
    /// FlatMenuDesigner is used in design-mode to filter out
    /// properties which aren't used by the control.
    /// </summary>
    public class FlatMenuDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        #region PreFilterProperties method
        /// <summary>
        /// Remove properties from VS designer which don't apply to FlatMenu controls.
        /// The properties are still there, they just won't appear in the designer.
        /// </summary>
        /// <param name="properties">Collection of properties to adjust.</param>
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            //properties.Remove("BackColor");
            properties.Remove("ForeColor");
            properties.Remove("Text");
        }
        #endregion PreFilterProperties method
    }

    /// <summary>
    /// FlatMenu is a base custom control class that implements
    /// most of the functionality for a flat-style menu bar or
    /// popup menu. Both FlatMenuBar and FlatPopupMenu inherit
    /// from this class. Note that FlatMenu is abstract as it
    /// contains four abstract methods which derived classes
    /// must implement.
    /// </summary>
    [Designer(typeof(FlatMenuDesigner))]
    public abstract class FlatMenu : System.Windows.Forms.Control
    {
        #region Events
        /// <summary>
        /// Event sent by a child popup menu to request that it
        /// be hidden (popped down). Not intended for use by
        /// client code.
        /// </summary>
        protected event System.EventHandler RequestHide;

        /// <summary>
        /// Event to indicate a new current menu item is in effect.
        /// Used by client code to register a callback function
        /// to receive notification of when a new current menu item
        /// is set (in order to display a status bar message,
        /// for example, as the user navigates a menu).
        /// </summary>
        public event CurrentMenuItemChangeEventHandler CurrentMenuItemChange;
        #endregion Events

        #region Static fields
        private static int nextMenuId = 1;
        private static readonly Color defaultBackColor = Color.FromArgb(50, 81, 118);
        private static readonly Color defaultHoverBackColor = Color.FromArgb(68, 139, 186);
        #endregion Static fields

        #region Private fields
        private int menuId;

        private Color backColor;
        private Color borderColor;
        private Color separatorColor;
        private Color textColor;

        private Color hoverBackColor;
        private Color hoverBorderColor;
        private Color hoverTextColor;

        private Color disabledTextColor;

        private Font hoverFont;

        private bool enableBorderDrawing;
        private bool enableHoverBorderDrawing;
        private bool enableHoverBackDrawing;

        private int leftMargin;
        private int topMargin;
        private int menuItemSpacing;

        private bool isMouseEntered;
        private bool isMousePressed;

        private int menuTimerInterval;

        private FlatMenu parentMenu;
        private FlatMenuItem parentMenuItem;

        private FlatMenuItemList menuItems;
        #endregion Private fields

        #region Protected fields
        protected System.Windows.Forms.Timer menuTimer;

        protected FlatMenuItem currMenuItem;

        protected bool alwaysShowPopup;
        protected bool needShowPopupMenu;
        protected FlatMenu popupMenu;
        protected bool isPopupStyleMenu;

        protected bool ignoreNextMouseMove;
        #endregion Protected fields

        #region Properties
        /// <summary>
        /// Get the menu ID for this instance. The ID is
        /// assigned dynamically during run-time and can
        /// be used to help in debugging/identifying menu
        /// instances.
        /// </summary>
        [Browsable(false)]
        public int MenuId
        {
            get { return this.menuId; }
        }

        /// <summary>
        /// Set or get the background color
        /// for the menu.
        /// </summary>
        [Category("Appearance"),
         Description("The background color for the menu.")]
        public override Color BackColor
        {
            get { return this.backColor; }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    this.InvalidateMenu();
                }
            }
        }

        /// <summary>
        /// Get or set the border color for this menu.
        /// </summary>
        [Category("Appearance"),
         Description("The border color for the menu.")]
        public Color BorderColor
        {
            get { return this.borderColor; }
            set { this.UpdateColorProperty(ref this.borderColor, value); }
        }

        /// <summary>
        /// Get or set the color for a separator menu item.
        /// A separator is displayed as just a horizontal or
        /// vertical line that divides groupings of menu items.
        /// </summary>
        [Category("Appearance"),
         Description("The color used to draw separator menu items.")]
        public Color SeparatorColor
        {
            get { return this.separatorColor; }
            set { this.UpdateColorProperty(ref this.separatorColor, value); }
        }

        /// <summary>
        /// Get or set the normal text color for drawing menu item
        /// text strings.
        /// </summary>
        [Category("Appearance"),
         Description("The color used to draw menu item text.")]
        public Color TextColor
        {
            get { return this.textColor; }
            set { this.UpdateColorProperty(ref this.textColor, value); }
        }

        /// <summary>
        /// Set or get the hover background color
        /// for the menu.
        /// </summary>
        [Category("Appearance"),
         Description("The background color for the hover state.")]
        public Color HoverBackColor
        {
            get { return this.hoverBackColor; }
            set
            {
                if (this.hoverBackColor != value)
                {
                    this.hoverBackColor = value;
                    this.InvalidateMenu();
                }
            }
        }

        /// <summary>
        /// Get or set the border color of a highlighted
        /// menu item.
        /// </summary>
        [Category("Appearance"),
         Description("The border color of a menu item in the hover (highlighted) state.")]
        public Color HoverBorderColor
        {
            get { return this.hoverBorderColor; }
            set { this.UpdateColorProperty(ref this.hoverBorderColor, value); }
        }

        /// <summary>
        /// Get or set the text color for a highlighted
        /// menu item.
        /// </summary>
        [Category("Appearance"),
         Description("The text color for a menu item in the hover (highlighted) state.")]
        public Color HoverTextColor
        {
            get { return this.hoverTextColor; }
            set { this.UpdateColorProperty(ref this.hoverTextColor, value); }
        }

        /// <summary>
        /// Get or set the text color for a disabled menu item.
        /// </summary>
        [Category("Appearance"),
         Description("The text color for a disabled menu item.")]
        public Color DisabledTextColor
        {
            get { return this.disabledTextColor; }
            set { this.UpdateColorProperty(ref this.disabledTextColor, value); }
        }

        /// <summary>
        /// Get or set a new text font to use for drawing
        /// menu item text in the hover state. By default,
        /// MS Sans Serif 8pt is used.
        /// </summary>
        [Category("Appearance"),
         Description("The font for drawing menu item text while in the hover (highlighted) state.")]
        public Font HoverFont
        {
            get { return this.hoverFont; }
            set
            {
                if (this.hoverFont != null)
                    this.hoverFont.Dispose();
                this.hoverFont = value;
                this.InvalidateMenu();
            }
        }

        /// <summary>
        /// Enable or disable drawing of menu border.
        /// It's useful to turn this property off when
        /// you want only the full background
        /// (if any) to show through.
        /// </summary>
        [Category("Appearance"),
         DefaultValue(true),
         Description("Enable or disable drawing of menu border.")]
        public bool EnableBorderDrawing
        {
            get { return this.enableBorderDrawing; }
            set { this.UpdateBoolProperty(ref this.enableBorderDrawing, value); }
        }

        /// <summary>
        /// Enable or disable drawing of hover rectangle border.
        /// </summary>
        [Category("Appearance"),
         DefaultValue(true),
         Description("Enable or disable drawing of hover rectangle border.")]
        public bool EnableHoverBorderDrawing
        {
            get { return this.enableHoverBorderDrawing; }
            set { this.UpdateBoolProperty(ref this.enableHoverBorderDrawing, value); }
        }

        /// <summary>
        /// Specify whether to draw the hover back color or not.
        /// Turning off this property allows you to draw hover
        /// rectangles which consist of just the border, thus
        /// it won't obscure the background (if any).
        /// </summary>
        [Category("Appearance"),
         DefaultValue(true),
         Description("Enable or disable drawing of the hover state background color.")]
        public bool EnableHoverBackDrawing
        {
            get { return this.enableHoverBackDrawing; }
            set { this.UpdateBoolProperty(ref this.enableHoverBackDrawing, value); }
        }

        /// <summary>
        /// Get or set an offset in pixels from the left
        /// side of the menu control for drawing the text
        /// of the first menu item.
        /// </summary>
        [Category("Appearance"),
         DefaultValue(7),
         Description("Specify the x-offset in pixels to begin drawing text for the first menu item.")]
        public int LeftMargin
        {
            get { return this.leftMargin; }
            set
            {
                if (this.leftMargin != value)
                {
                    this.leftMargin = value;
                    this.InvalidateMenu();
                }
            }
        }

        /// <summary>
        /// Get or set an offset in pixels from the top
        /// of the menu control for drawing the text of the
        /// first menu item.
        /// </summary>
        [Category("Appearance"),
         DefaultValue(6),
         Description("Specify the y-offset in pixels to begin drawing text for the first menu item.")]
        public int TopMargin
        {
            get { return this.topMargin; }
            set
            {
                if (this.topMargin != value)
                {
                    this.topMargin = value;
                    this.InvalidateMenu();
                }
            }
        }

        /// <summary>
        /// Get or set a value in pixels to be used for
        /// the spacing in between two menu items.
        /// </summary>
        [Category("Appearance"),
         DefaultValue(4),
         Description("Specify the spacing between two menu items in pixels.")]
        public int MenuItemSpacing
        {
            get { return this.menuItemSpacing; }
            set
            {
                if (this.menuItemSpacing != value)
                {
                    this.menuItemSpacing = value;
                    this.InvalidateMenu();
                }
            }
        }

        /// <summary>
        /// Is the mouse inside of the menu control?
        /// </summary>
        [Browsable(false)]
        protected bool IsMouseEntered
        {
            get { return this.isMouseEntered; }
        }

        /// <summary>
        /// Is the left mouse button pressed?
        /// </summary>
        [Browsable(false)]
        protected bool IsMousePressed
        {
            get { return this.isMousePressed; }
        }

        /// <summary>
        /// Get or set a timer interval used for auto-closing
        /// the menu when the mouse is moved out of bounds.
        /// </summary>
        [Category("Behavior"),
         DefaultValue(500),
         Description("The timer interval in milliseconds for auto-closing a menu.")]
        public int MenuTimerInterval
        {
            get { return this.menuTimerInterval; }
            set { this.menuTimerInterval = value; }
        }

        /// <summary>
        /// Is this menu a popup-style menu?
        /// </summary>
        [Browsable(false)]
        public bool IsPopupMenu
        {
            get { return this.isPopupStyleMenu; }
        }

        /// <summary>
        /// Return the popup menu. If null, create it first.
        /// </summary>
        [Browsable(false)]
        public FlatMenu Popup
        {
            get
            {
                if (this.popupMenu == null)
                {
                    this.CreatePopupMenu();
                    if (this.popupMenu != null)
                        this.popupMenu.RequestHide += new System.EventHandler(this.popupMenu_RequestHide);
                }
                return this.popupMenu;
            }
            set { this.popupMenu = value; }
        }

        /// <summary>
        /// Get or set the parent menu for this menu.
        /// For a top-level menu, this property is null.
        /// </summary>
        [Browsable(false)]
        public FlatMenu ParentMenu
        {
            get { return this.parentMenu; }
            set { this.parentMenu = value; }
        }

        /// <summary>
        /// Get or set the parent menu item for this menu.
        /// For a top-level menu, this property is null.
        /// </summary>
        [Browsable(false)]
        public FlatMenuItem ParentMenuItem
        {
            get { return this.parentMenuItem; }
            set { this.parentMenuItem = value; }
        }

        /// <summary>
        /// Get or set the list of child menu items.
        /// </summary>
        [Browsable(false)]
        public FlatMenuItemList MenuItems
        {
            get { return this.menuItems; }
            set { this.menuItems = value; }
        }
        #endregion Properties

        #region ShouldSerialize property methods
        /// <summary>
        /// Indicate whether or not the BackColor
        /// property should be serialized or not.
        /// </summary>
        /// <returns>true if property should be serialized, false otherwise.</returns>
        private bool ShouldSerializeBackColor()
        {
            // Don't serialize default value.
            if (this.BackColor == FlatMenu.defaultBackColor)
                return false;

            return true;
        }

        /// <summary>
        /// Indicate whether or not the HoverBackColor
        /// property should be serialized or not.
        /// </summary>
        /// <returns>true if property should be serialized, false otherwise.</returns>
        private bool ShouldSerializeHoverBackColor()
        {
            // Don't serialize default value.
            if (this.HoverBackColor == FlatMenu.defaultHoverBackColor)
                return false;

            return true;
        }
        #endregion ShouldSerialize property methods

        #region Reset property methods
        /// <summary>
        /// Reset BackGradientColor property back to default value.
        /// </summary>
        public override void ResetBackColor()
        {
            this.BackColor = FlatMenu.defaultBackColor;
        }

        /// <summary>
        /// Reset HoverBackGradientColor property back to default value.
        /// </summary>
        private void ResetHoverBackColor()
        {
            this.HoverBackColor = FlatMenu.defaultHoverBackColor;
        }
        #endregion Reset property methods

        #region Constructor
        /// <summary>
        /// FlatMenu constructor.
        /// </summary>
        protected FlatMenu()
        {
            // Initialize private fields.
            this.menuId = FlatMenu.nextMenuId++;

            this.backColor = FlatMenu.defaultBackColor;
            this.borderColor = Color.Black;
            this.separatorColor = Color.Black;
            this.textColor = Color.Black;

            this.hoverBackColor = FlatMenu.defaultHoverBackColor;
            this.hoverBorderColor = this.TextColor;
            this.hoverTextColor = this.TextColor;

            this.disabledTextColor = Color.Gray;

            this.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular);
            this.hoverFont = new Font("Segoe UI", 9.75f, FontStyle.Regular);

            this.enableBorderDrawing = false;
            this.enableHoverBorderDrawing = false;
            this.enableHoverBackDrawing = true;

            this.leftMargin = 7;
            this.topMargin = 6;
            this.menuItemSpacing = 4;

            this.isMouseEntered = false;
            this.isMousePressed = false;

            this.menuTimerInterval = 500;

            this.parentMenu = null;
            this.parentMenuItem = null;

            this.menuItems = new FlatMenuItemList();

            // Initialize protected fields.
            this.menuTimer = new System.Windows.Forms.Timer();
            this.menuTimer.Tick += new System.EventHandler(this.MenuTimerCallback);

            this.currMenuItem = null;

            this.alwaysShowPopup = false;
            this.needShowPopupMenu = false;
            this.popupMenu = null;
            this.isPopupStyleMenu = false;

            this.ignoreNextMouseMove = false;

            this.RequestHide = null;
            this.CurrentMenuItemChange = null;

            // Turn off tab-stop.
            this.TabStop = false;

            // Enable double-buffering.
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }
        #endregion Constructor

        #region Protected Dispose method
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources.
                if (this.hoverFont != null)
                {
                    this.hoverFont.Dispose();
                    this.hoverFont = null;
                }
                if (this.menuTimer != null)
                {
                    this.menuTimer.Dispose();
                    this.menuTimer = null;
                }
                if (this.popupMenu != null)
                {
                    this.popupMenu.Dispose();
                    this.popupMenu = null;
                }
            }
            base.Dispose(disposing);
        }
        #endregion Protected Dispose method

        #region Protected helper methods
        /// <summary>
        /// Recursive method to test whether the mouse is
        /// in the popup menu or one of the child menus of
        /// that popup menu.
        /// </summary>
        /// <returns>Returns true or false as per summary.</returns>
        protected bool IsMouseInPopupMenu()
        {
            if (this.popupMenu == null)
                return false;

            if (this.popupMenu.IsMouseEntered)
                return true;

            return this.popupMenu.IsMouseInPopupMenu();
        }

        /// <summary>
        /// Show the popup menu for the currently highlighted
        /// menu item.
        /// </summary>
        protected void ShowPopupMenu()
        {
            // Hide existing popup menu first.
            this.HidePopupMenu();

            // Check if we can actually show a popup menu.
            if (this.currMenuItem == null ||
                 this.currMenuItem.MenuItems.Count == 0 ||
                 !this.currMenuItem.Enabled ||
                 !this.currMenuItem.Selectable ||
                 this.Parent == null)
            {
                return;
            }

            // Create the popup menu if needed.
            if (this.popupMenu == null)
            {
                this.CreatePopupMenu();
                if (this.popupMenu == null)
                    return;
                this.popupMenu.RequestHide += new System.EventHandler(this.popupMenu_RequestHide);
            }
            if (this.popupMenu.Parent != this.Parent)
                this.popupMenu.Parent = this.Parent;
            if (this.popupMenu.ParentMenu != this)
                this.popupMenu.ParentMenu = this;

            // Load menu items into the popup menu.
            this.popupMenu.MenuItems.Clear();
            for (int i = 0; i < this.currMenuItem.MenuItems.Count; ++i)
            {
                FlatMenuItem item = this.currMenuItem.MenuItems.GetAt(i);
                this.popupMenu.MenuItems.Add(item);
            }

            // Compute the screen location for the popup menu.
            Point ptScreen = new Point(0, 0);
            this.GetPopupMenuScreenLocation(ref ptScreen);

            // Show the popup menu.
            this.popupMenu.Visible = true;
            this.popupMenu.ParentMenuItem = this.currMenuItem;
            this.popupMenu.Location = this.popupMenu.Parent.PointToClient(ptScreen);
            this.popupMenu.BringToFront();
        }

        /// <summary>
        /// Hide the popup menu and any of its descendant menus.
        /// This method uses recursion.
        /// </summary>
        protected void HidePopupMenu()
        {
            if (this.popupMenu == null)
                return;

            // Hide descendant menus first.
            this.popupMenu.HidePopupMenu();

            // Hide the popup menu.
            if (this.popupMenu.Visible)
            {
                this.popupMenu.Visible = false;
            }
            this.popupMenu.MenuItems.Clear();
        }

        /// <summary>
        /// Draw a rectangle with specified area and color.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="rect">Rectangle to draw in.</param>
        /// <param name="color">Color to draw rectangle with.</param>
        protected void DrawBackground(Graphics g, Rectangle rect, Color color)
        {
            if (rect.Width == 0 || rect.Height == 0)
                return;

            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, rect);
            }
        }

        /// <summary>
        /// Draw a rectangular 1-pixel border using the given
        /// area and color.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="rect">Rectangle defining the border.</param>
        /// <param name="color">Border color.</param>
        protected void DrawBorder(Graphics g, Rectangle rect, Color color)
        {
            if (rect.Width < 2 || rect.Height < 2)
                return;

            using (Pen pen = new Pen(color, 0))
            {
                Rectangle r = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                g.DrawRectangle(pen, r);
            }
        }

        /// <summary>
        /// Draw the given text string at the specified x,y
        /// location and using the given font and color for the text.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="text">Text string to draw.</param>
        /// <param name="font">Font to use for drawing the text string.</param>
        /// <param name="color">Text color.</param>
        /// <param name="x">The x-location of top-left corner of text.</param>
        /// <param name="y">The y-location of top-left corner of text.</param>
        protected void DrawMenuItemText(Graphics g, string text, Font font, Color color, int x, int y)
        {
            using (Brush brush = new SolidBrush(color))
            {
                g.DrawString(text, font, brush, x, y);
            }
        }

        /// <summary>
        /// Draw a separator line.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="color">Color of the separator line.</param>
        /// <param name="x1">The x-coordinate of the first end point of the separator line.</param>
        /// <param name="y1">The y-coordinate of the first end point of the separator line.</param>
        /// <param name="x2">The x-coordinate of the second end point of the separator line.</param>
        /// <param name="y2">The y-coordinate of the second end point of the separator line.</param>
        protected void DrawSeparator(Graphics g, Color color, int x1, int y1, int x2, int y2)
        {
            using (Pen pen = new Pen(color, 0))
            {
                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        /// <summary>
        /// Start the menu timer which is used to implement
        /// a timeout for checking if the user has moved the
        /// mouse out of the menu control.
        /// </summary>
        protected void StartMenuTimer()
        {
            if (this.menuTimer != null)
            {
                this.menuTimer.Stop();
                this.menuTimer.Interval = this.menuTimerInterval;
                this.menuTimer.Start();
            }
        }

        /// <summary>
        /// Stop the menu timer.
        /// </summary>
        protected void StopMenuTimer()
        {
            if (this.menuTimer != null)
            {
                this.menuTimer.Stop();
            }
        }

        /// <summary>
        /// Update the old bool value if it is different than
        /// the new value.
        /// </summary>
        /// <param name="oldValue">Existing bool value to check/update.</param>
        /// <param name="newValue">The new value of the bool property.</param>
        protected void UpdateBoolProperty(ref bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                oldValue = newValue;
                this.InvalidateMenu();
            }
        }

        /// <summary>
        /// Update the old color if it is different than
        /// the new color.
        /// </summary>
        /// <param name="oldColor">Old color to adjust.</param>
        /// <param name="newColor">New color to apply.</param>
        protected void UpdateColorProperty(ref Color oldColor, Color newColor)
        {
            if (oldColor != newColor)
            {
                oldColor = newColor;
                this.InvalidateMenu();
            }
        }

        /// <summary>
        /// Determine the current menu item given the current mouse
        /// location.
        /// </summary>
        /// <param name="x">The x-coordinate of the current mouse location.</param>
        /// <param name="y">The y-coordinate of the current mouse location.</param>
        protected void UpdateCurrentMenuItem(int x, int y)
        {
            FlatMenuItem newItem = null;

            // Determine a new current menu item given the
            // x,y mouse location.
            for (int i = 0; i < this.MenuItems.Count; ++i)
            {
                FlatMenuItem item = this.MenuItems.GetAt(i);
                Debug.Assert(item != null);

                if (item.Selectable &&
                     item.Enabled &&
                     item.ClientRectangle.Contains(x, y))
                {
                    newItem = item;
                    break;
                }
            }

            // We actually only set a new current menu item
            // if newItem is not null. We do this to avoid turning
            // off the highlighting as the user transitions from
            // one menu item to an adjacent one. By doing this
            // check, the transition will be direct from one menu
            // item to the next. You might wonder how then will
            // the current menu item ever be set back to null.
            // This is done elsewhere in the code (e.g., when the
            // user moves the mouse outside of the menu control).
            if (newItem != null)
            {
                this.currMenuItem = newItem;
                this.NotifyCurrentMenuItemChange();
            }
        }

        /// <summary>
        /// Check if the popup menu is visible or not.
        /// </summary>
        /// <returns>true or false as per summary.</returns>
        protected bool IsPopupMenuVisible()
        {
            if (this.popupMenu == null)
                return false;

            return this.popupMenu.Visible;
        }

        /// <summary>
        /// Return the top-level parent menu of the current menu.
        /// </summary>
        /// <returns>The top-level parent menu.</returns>
        protected FlatMenu GetTopMenu()
        {
            FlatMenu topMenu = null;
            FlatMenu currMenu = this;
            while (currMenu != null)
            {
                topMenu = currMenu;
                currMenu = currMenu.ParentMenu;
            }

            return topMenu;
        }

        /// <summary>
        /// Return the top-most parent menu which is also
        /// a popup menu.
        /// </summary>
        /// <returns>The top-most parent menu which is also
        /// a popup menu.</returns>
        protected FlatMenu GetTopPopupMenu()
        {
            FlatMenu topPopup = null;
            FlatMenu currMenu = this;
            while (currMenu != null && currMenu.IsPopupMenu)
            {
                topPopup = currMenu;
                currMenu = currMenu.ParentMenu;
            }

            return topPopup;
        }

        /// <summary>
        /// Notify receivers of a change in the currently
        /// highlighted menu item for this menu.
        /// </summary>
        protected void NotifyCurrentMenuItemChange()
        {
            FlatMenu topMenu = this.GetTopMenu();
            Debug.Assert(topMenu != null);

            CurrentMenuItemChangeEventHandler evh = topMenu.CurrentMenuItemChange;
            if (evh != null)
            {
                CurrentMenuItemChangeEventArgs e = new CurrentMenuItemChangeEventArgs();
                e.CurrentMenuItem = this.currMenuItem;
                evh(topMenu, e);
            }
        }

        /// <summary>
        /// Reset the current menu item back to null.
        /// Send out a CurrentMenuItemChangeEvent if needed.
        /// </summary>
        protected void ResetCurrentMenuItem()
        {
            FlatMenuItem prevItem = this.currMenuItem;
            this.currMenuItem = null;
            if (prevItem != null)
                this.NotifyCurrentMenuItemChange();
        }

        /// <summary>
        /// Call Invalidate() if parent is set.
        /// </summary>
        protected void InvalidateMenu()
        {
            if (this.Parent == null)
                return;

            this.Invalidate();
        }

        /// <summary>
        /// Determine if this menu has any immediate child
        /// menu items which are either Check or Radio style.
        /// </summary>
        /// <returns></returns>
        protected bool HasCheckOrRadioItems()
        {
            for (int i = 0; i < this.MenuItems.Count; ++i)
            {
                FlatMenuItem item = this.MenuItems.GetAt(i);
                if (item.Style == FlatMenuItemStyle.Check ||
                     item.Style == FlatMenuItemStyle.Radio)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion Protected helper methods

        #region Mouse event handlers
        /// <summary>
        /// Override mouse entry into the menu control.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this.isMouseEntered = true;
            this.StopMenuTimer();
        }

        /// <summary>
        /// Override mouse exit from the menu control.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.isMouseEntered = false;
            this.StartMenuTimer();
        }

        /// <summary>
        /// Override mouse movement.
        /// </summary>
        /// <param name="e">Mouse event argument.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            FlatMenuItem prevItem = this.currMenuItem;
            UpdateCurrentMenuItem(e.X, e.Y);
            if (prevItem != this.currMenuItem)
            {
                this.Invalidate();
                if (this.needShowPopupMenu || this.alwaysShowPopup)
                    this.ShowPopupMenu();
            }
        }

        /// <summary>
        /// Override pressing of a mouse button.
        /// </summary>
        /// <param name="e">Mouse event argument.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            this.isMousePressed = true;
        }

        /// <summary>
        /// Override release of a mouse button.
        /// </summary>
        /// <param name="e">Mouse event argument.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button != MouseButtons.Left)
                return;

            this.isMousePressed = false;
            this.ignoreNextMouseMove = false;

            if (this.currMenuItem == null)
                return;

            // If the current menu item has children, then
            // it is not a leaf menu item and hence cannot
            // send out a Click event.
            if (this.currMenuItem.MenuItems.Count != 0)
                return;

            // If the current menu item has no Click event
            // handler, do nothing.
            if (!this.currMenuItem.HasClickHandler)
                return;

            // Record the current menu item being clicked
            // before resetting it back to null.
            FlatMenuItem currItem = this.currMenuItem;
            this.ResetCurrentMenuItem();
            this.Invalidate();

            // Close any popup menus if needed.
            if (this.parentMenuItem != null)
            {
                FlatMenu topPopup = this.GetTopPopupMenu();
                if (topPopup != null)
                {
                    topPopup.HidePopupMenu();

                    // Top popup menu tries to make request to its parent
                    // for closure, or else it closes itself.
                    System.EventHandler evh = topPopup.RequestHide;
                    if (evh != null)
                        evh(topPopup, EventArgs.Empty);
                    else
                        topPopup.Visible = false;
                }
            }
            else if (this.ParentMenu == null && this.IsPopupMenu)
            {
                // We have to close ourselves.
                this.HidePopupMenu();
                this.Visible = false;
            }
            else
            {
                this.ignoreNextMouseMove = true;
            }

            // Notify any registered Click event handlers.
            currItem.NotifyClickEvent();
            this.needShowPopupMenu = false;
        }
        #endregion Mouse event handlers

        #region Paint event handler
        /// <summary>
        /// Override painting of menu control.
        /// </summary>
        /// <param name="pe">Paint event argument.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the base class OnPaint.
            base.OnPaint(e);

            // Update menuitem text rects.
            Graphics g = e.Graphics;
            RepositionMenuItems(g);

            // Draw the menu.
            DrawMenu(g);
            this.ignoreNextMouseMove = false;
        }
        #endregion Paint event handler

        #region Visibility event handler
        /// <summary>
        /// Reset various fields when visibility of menu
        /// control is changed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            this.StopMenuTimer();
            this.isMouseEntered = false;
            this.isMousePressed = false;
            this.ResetCurrentMenuItem();
            this.ignoreNextMouseMove = false;
        }
        #endregion Visibility event handler

        #region Menu timer callback
        /// <summary>
        /// Menu timer callback function.
        /// </summary>
        /// <param name="obj">Timer callback object.</param>
        /// <param name="e">Timer callback event argument.</param>
        protected void MenuTimerCallback(object obj, EventArgs e)
        {
            if (this.menuTimer == null)
                return;

            this.StopMenuTimer();

            // Check if the mouse is in this menu control or
            // in a popup menu.
            if (!this.IsMouseEntered && !this.IsMouseInPopupMenu())
            {
                this.ResetCurrentMenuItem();
                this.needShowPopupMenu = false;
                this.Invalidate();
                this.HidePopupMenu();

                System.EventHandler evh = this.RequestHide;
                if (this.parentMenuItem != null && evh != null)
                {
                    // Make a request to hide ourselves.
                    evh(this, EventArgs.Empty);
                }
                else if (this.IsPopupMenu)
                {
                    // Handles the case when using FlatPopupMenu directly
                    // without a FlatMenuBar (e.g., a context menu).
                    this.Visible = false;
                }
            }
        }
        #endregion Menu timer callback

        #region RequestHide event handler
        /// <summary>
        /// Receive request to hide popup menu.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        protected void popupMenu_RequestHide(object obj, System.EventArgs e)
        {
            // Check if we should override the request to hide
            // the popup menu. This is the case when the current
            // menu item corresponds to the popup menu.
            if (!this.IsPopupMenu && this.IsMouseEntered)
            {
                if (this.popupMenu != null &&
                     this.popupMenu.ParentMenuItem == this.currMenuItem)
                {
                    return;
                }
            }

            this.needShowPopupMenu = false;

            if (this.ParentMenuItem == null && !this.IsPopupMenu)
                this.ResetCurrentMenuItem();
            this.Invalidate();

            this.HidePopupMenu();

            // Propagate the event to parent menu if needed.
            if (!this.IsMouseEntered)
            {
                System.EventHandler evh = this.RequestHide;
                if (evh != null)
                    evh(this, System.EventArgs.Empty);
                else if (this.IsPopupMenu)
                    this.Visible = false;
            }
        }
        #endregion RequestHide event handler

        #region ToString override
        /// <summary>
        /// Output the menu ID to a string.
        /// </summary>
        /// <returns>A string containing the menu ID.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Menu");
            sb.Append(this.MenuId);

            return sb.ToString();
        }
        #endregion ToString override

        #region Abstract methods
        protected abstract void DrawMenu(Graphics g);
        protected abstract void RepositionMenuItems(Graphics g);
        protected abstract void CreatePopupMenu();
        protected abstract void GetPopupMenuScreenLocation(ref Point ptScreen);
        #endregion Abstract methods

    }
}
