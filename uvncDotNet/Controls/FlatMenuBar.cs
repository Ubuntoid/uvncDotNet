// Filename: FlatMenuBar.cs
// Desc: FlatMenuBar and FlatPopupMenu derived classes.
// 2005-11-07 nschan Initial revision.
// 2005-11-10 nschan Added DrawDesignMode().
// 2005-11-14 nschan Changed FlatPopupMenu Visible property to false by default.
// 2005-11-17 nschan Use EnableBorderDrawing, EnableHoverBackDrawing,
//                   and HoverTextFont properties. Also, added
//                   DrawMenuItemTextR() method.
// 2006-01-29 nschan Added support for check and radio styles.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace uvncDotNet.Controls
{
    /// <summary>
    /// FlatPopupMenu inherits from FlatMenu in order to
    /// implement a flat-style popup menu.
    /// FlatPopupMenu is also used by FlatMenuBar to show
    /// cascading submenus. FlatPopupMenu can be used
    /// independently of FlatMenuBar to create standalone
    /// context menus.
    /// </summary>
    public class FlatPopupMenu : FlatMenu
    {
        #region Constants
        private static readonly int checkAreaWidth = 15;
        #endregion Constants

        #region Constructor
        /// <summary>
        /// FlatPopupMenu constructor.
        /// </summary>
        public FlatPopupMenu()
        {
            this.alwaysShowPopup = true;
            this.isPopupStyleMenu = true;
            this.MenuTimerInterval = 300;
            this.Size = new Size(80, 10);
            this.Visible = false;
            //this.BackColor = SystemColors.Control;
        }

        #endregion Constructor

        #region Public methods
        /// <summary>
        /// Show the popup menu (like a context menu).
        /// </summary>
        /// <param name="parent">Parent form or outer control.</param>
        /// <param name="alignX">Horizontal alignment of x,y location relative
        /// to the top-left corner of the menu control.</param>
        /// <param name="alignY">Vertical alignment of x,y location relative
        /// to the top-left corner of the menu control.</param>
        /// <param name="x">The x-coordinate of the location of the menu, relative to the parent form.</param>
        /// <param name="y">The y-coordinate of the location of the menu, relative to the parent form.</param>
        public void TrackPopupMenu(Control parent, FlatMenuAlignment alignX, FlatMenuAlignment alignY, int x, int y)
        {
            if (parent == null || this.MenuItems.Count == 0)
                return;

            this.Visible = false;
            this.Parent = parent;
            this.ParentMenu = null;

            this.Visible = true;
            this.ParentMenuItem = null;

            int x2 = x;
            int y2 = y;
            switch (alignX)
            {
                case FlatMenuAlignment.LeftAlign:
                    x2 = x - 1;
                    break;
                case FlatMenuAlignment.CenterAlign:
                    x2 = x - this.ClientRectangle.Width / 2;
                    break;
                case FlatMenuAlignment.RightAlign:
                    x2 = x - this.ClientRectangle.Width + 1;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            switch (alignY)
            {
                case FlatMenuAlignment.TopAlign:
                    y2 = y - 1;
                    break;
                case FlatMenuAlignment.VCenterAlign:
                    y2 = y - this.ClientRectangle.Height / 2;
                    break;
                case FlatMenuAlignment.BottomAlign:
                    y2 = y - this.ClientRectangle.Height + 1;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            this.Location = new Point(x2, y2);

            this.BringToFront();
        }

        /// <summary>
        /// Overloaded version of TrackPopupMenu which uses left
        /// and top alignment by default (e.g., standard context
        /// menu semantics).
        /// </summary>
        /// <param name="parent">Parent form or control.</param>
        /// <param name="x">The x-coordinate of the location of the menu, relative to the parent form.</param>
        /// <param name="y">The y-coordinate of the location of the menu, relative to the parent form.</param>
        public void TrackPopupMenu(Control parent, int x, int y)
        {
            this.TrackPopupMenu(parent, FlatMenuAlignment.LeftAlign, FlatMenuAlignment.TopAlign, x, y);
        }
        #endregion Public methods

        #region Protected Dispose method
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources.
            }
            base.Dispose(disposing);
        }
        #endregion Protected Dispose method

        #region Private methods
        /// <summary>
        /// Expand the given rectangle a bit. This is used
        /// to create the hover rectangle for a menu item.
        /// </summary>
        /// <param name="rect">Rectangle to adjust.</param>
        private void AdjustMenuItemRect(ref Rectangle rect)
        {
            rect.Y -= 2;
            rect.Height += 4;
            rect.X -= 3;
            rect.Width += 7;
        }

        /// <summary>
        /// Re-calculate the size of the menu based on the
        /// menu item rects. Then do the resizing as well.
        /// </summary>
        private void AdjustMenuSize()
        {
            if (this.MenuItems.Count == 0)
                return;

            // Compute a new width and height for the menu.
            int newWidth = 80;
            if (this.ParentMenu != null && !this.ParentMenu.IsPopupMenu)
                newWidth = Math.Max(newWidth, this.ParentMenuItem.ClientRectangle.Width + 10);
            int newHeight = 0;

            int xpad = 9;
            int ypad = 4;
            bool hasSubMenu = false;

            if (this.HasCheckOrRadioItems())
                xpad += FlatPopupMenu.checkAreaWidth + 3;

            for (int i = 0; i < this.MenuItems.Count; ++i)
            {
                FlatMenuItem item = this.MenuItems.GetAt(i);
                newWidth = Math.Max(newWidth, item.ClientRectangle.Width + xpad);
                if (item.MenuItems.Count > 0)
                    hasSubMenu = true;
                newHeight = Math.Max(newHeight, item.ClientRectangle.Bottom);
            }
            if (hasSubMenu)
                newWidth += 10;
            newHeight += ypad;

            // Resize ourselves if needed.
            if (this.ClientRectangle.Width != newWidth ||
                 this.ClientRectangle.Height != newHeight)
            {
                this.Size = new Size(newWidth, newHeight);
            }
        }

        /// <summary>
        /// Draw the menu item text centered vertically but
        /// left-aligned in the given rectangle.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="text">Text string to draw.</param>
        /// <param name="font">Text font.</param>
        /// <param name="color">Text color.</param>
        /// <param name="r">Rectangle bounds to draw in.</param>
        private void DrawMenuItemTextR(Graphics g, string text, Font font, Color color, Rectangle r)
        {
            using (Brush brush = new SolidBrush(color))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;

                Rectangle rnew = new Rectangle(r.X + 3, r.Y, r.Width, r.Height);
                g.DrawString(text, font, brush, (RectangleF)rnew, sf);
            }
        }

        /// <summary>
        /// Draw the given menu item.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="item">Menu item to draw.</param>
        private void DrawMenuItem(Graphics g, FlatMenuItem item)
        {
            if (item.ClientRectangle.Width < 2)
                return;

            if (item.Style == FlatMenuItemStyle.Separator)
            {
                // Draw separator.
                int y = (item.ClientRectangle.Top + item.ClientRectangle.Bottom) / 2;
                int x1 = item.ClientRectangle.Left;
                int x2 = item.ClientRectangle.Right - 1;
                this.DrawSeparator(g, this.SeparatorColor, x1, y, x2, y);
            }
            else
            {
                // Draw checkmark or radio indicator if needed.
                Color tcolor = this.TextColor;
                if (!item.Enabled)
                    tcolor = this.DisabledTextColor;
                if (item.Style == FlatMenuItemStyle.Check)
                    this.DrawCheck(g, tcolor, item);
                else if (item.Style == FlatMenuItemStyle.Radio)
                    this.DrawRadio(g, tcolor, item);

                // Draw the text portion of menu item.
                if (this.currMenuItem == item)
                {
                    // Draw text in hover state.
                    if (this.EnableHoverBackDrawing)
                        this.DrawBackground(g, item.ClientRectangle, this.HoverBackColor);
                    if (this.EnableHoverBorderDrawing)
                        this.DrawBorder(g, item.ClientRectangle, this.HoverBorderColor);
                    this.DrawMenuItemTextR(g, item.Text, this.HoverFont, this.HoverTextColor,
                        item.ClientRectangle);
                    if (item.MenuItems.Count > 0)
                        this.DrawSubMenuArrow(g, item, this.HoverTextColor);
                }
                else
                {
                    // Draw text in regular, unhighlighted state.
                    this.DrawMenuItemTextR(g, item.Text, this.Font, tcolor,
                        item.ClientRectangle);
                    if (item.MenuItems.Count > 0)
                        this.DrawSubMenuArrow(g, item, tcolor);
                }
            }
        }

        /// <summary>
        /// Draw a partial line at the top of the popup menu
        /// that is the same width as the parent menu item.
        /// This method is used for a block-out effect when
        /// a popup menu is opened on top of a FlatMenuBar.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        private void DrawPartialLine(Graphics g)
        {
            if (this.ParentMenuItem == null)
                return;

            //var color = this.popupMenu.BackColor;
            var color = this.BackColor;
            using (var pen = new Pen(color, 0))
            {
                g.DrawLine(pen, 1, 0, this.ParentMenuItem.ClientRectangle.Width - 2, 0);
            }
        }

        /// <summary>
        /// Draw a small arrow indicating the menu item
        /// has a submenu (e.g., child menu items of its own).
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="item">Menu item for which to draw the arrow.</param>
        /// <param name="color">Arrow color.</param>
        private void DrawSubMenuArrow(Graphics g, FlatMenuItem item, Color color)
        {
            int x = item.ClientRectangle.Right - 8;
            int y = item.ClientRectangle.Top + item.ClientRectangle.Height / 2 - 2;
            using (Pen pen = new Pen(color, 0))
            {
                g.DrawLine(pen, x, y, x, y + 4);
                g.DrawLine(pen, x + 1, y + 1, x + 1, y + 3);
                g.DrawLine(pen, x + 2, y + 2, x + 1, y + 2);
            }
        }

        /// <summary>
        /// Draw a checkmark.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="color">Color for the checkmark.</param>
        /// <param name="item">Checked menu item.</param>
        private void DrawCheck(Graphics g, Color color, FlatMenuItem item)
        {
            const int size = 11;

            int x = item.ClientRectangle.X - FlatPopupMenu.checkAreaWidth - 3;
            int y = item.ClientRectangle.Y + item.ClientRectangle.Height / 2 - size / 2;

            Rectangle r = new Rectangle(x, y, size, size);
            this.DrawBackground(g, r, this.BackColor);

            using (Pen pen = new Pen(color, 0))
            {
                g.DrawRectangle(pen, r);
                if (item.Checked)
                {
                    SmoothingMode oldMode = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawLine(pen, x + 2, y + 7, x + 4, y + 9);
                    g.DrawLine(pen, x + 4, y + 9, x + 9, y + 4);
                    g.SmoothingMode = oldMode;
                }
            }
        }

        /// <summary>
        /// Draw a radio button (if needed).
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="color">Radio button color.</param>
        /// <param name="item">Radio menu item.</param>
        private void DrawRadio(Graphics g, Color color, FlatMenuItem item)
        {
            if (!item.Radio)
                return;

            const int size = 5;

            int x = item.ClientRectangle.X - FlatPopupMenu.checkAreaWidth / 2 - size / 2 - 5;
            int y = item.ClientRectangle.Y + item.ClientRectangle.Height / 2 - size / 2;

            Rectangle r = new Rectangle(x, y, size, size);
            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, r);
            }
        }

        /// <summary>
        /// Detect if the mouse is near the submenu arrow.
        /// </summary>
        /// <param name="x">The x-coordinate of the mouse location to test.</param>
        /// <param name="y">The y-coordinate of the mouse location to test.</param>
        /// <returns>true if the mouse is near the submenu arrow.</returns>
        private bool IsMouseNearSubMenuArrow(int x, int y)
        {
            if (this.currMenuItem == null || this.currMenuItem.MenuItems.Count == 0)
                return false;

            int xtol = 15;
            Rectangle rect = this.currMenuItem.ClientRectangle;
            if (rect.Width < xtol)
                return false;
            rect.X = rect.Right - xtol;
            rect.Width = xtol;
            if (rect.Contains(x, y))
                return true;

            return false;
        }
        #endregion Private methods

        #region FlatMenu overrides
        /// <summary>
        /// Draw the popup menu.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        protected override void DrawMenu(Graphics g)
        {
            // Draw the menu background.
            DrawBackground(g, this.ClientRectangle, this.BackColor);
            if (this.HasCheckOrRadioItems())
            {
                // Draw background for a left column area.
                Rectangle r = new Rectangle(this.ClientRectangle.X, this.ClientRectangle.Y,
                    FlatPopupMenu.checkAreaWidth + 4, this.ClientRectangle.Height);

                var color = this.BackColor;
                DrawBackground(g, r, color);
            }

            // Draw the menu border.
            if (this.EnableBorderDrawing)
                DrawBorder(g, this.ClientRectangle, this.BorderColor);
            if (this.ParentMenu != null && !this.ParentMenu.IsPopupMenu)
                DrawPartialLine(g);

            // Draw each menu item.
            for (int i = 0; i < this.MenuItems.Count; ++i)
            {
                FlatMenuItem item = this.MenuItems.GetAt(i);
                Debug.Assert(item != null);

                DrawMenuItem(g, item);
            }
        }

        /// <summary>
        /// Re-calculate menu item rects for this menu.
        /// Also adjusts the menu size if needed.
        /// </summary>
        /// <param name="g">Graphics object needed to calculate
        /// text rects using MeasureString.</param>
        protected override void RepositionMenuItems(Graphics g)
        {
            int xoffset = this.LeftMargin;
            int yoffset = this.TopMargin;

            if (this.HasCheckOrRadioItems())
                xoffset += FlatPopupMenu.checkAreaWidth + 3;

            // Set the client rect for each menu item. The rect will
            // be a bit larger than the text rect based on MeasureString.
            for (int i = 0; i < this.MenuItems.Count; ++i)
            {
                FlatMenuItem item = this.MenuItems.GetAt(i);
                Debug.Assert(item != null);

                string text = item.Text;
                if (text == String.Empty)
                    text = " ";
                SizeF textSize = g.MeasureString(text, this.Font);
                SizeF hoverTextSize = g.MeasureString(text, this.HoverFont);
                Rectangle rect = new Rectangle(xoffset, yoffset,
                    (int)(Math.Max(textSize.Width, hoverTextSize.Width) + 0.5),
                    (int)(Math.Max(textSize.Height, hoverTextSize.Height) + 0.5));

                AdjustMenuItemRect(ref rect);
                if (item.Style == FlatMenuItemStyle.Separator)
                {
                    rect.Offset(0, -this.MenuItemSpacing / 2);
                    rect.Height = 3;
                }
                item.ClientRectangle = rect;

                if (item.Style == FlatMenuItemStyle.Separator)
                    yoffset += rect.Height;
                else
                    yoffset += (this.MenuItemSpacing + (int)rect.Height);
            }

            // Now adjust the overall menu size given the updated item sizes.
            AdjustMenuSize();

            // Now adjust each menu item again so that each rect is the
            // same width and extends towards the right border of the menu.
            for (int j = 0; j < this.MenuItems.Count; ++j)
            {
                FlatMenuItem item = this.MenuItems.GetAt(j);
                Rectangle rect = item.ClientRectangle;
                rect.Width = this.ClientRectangle.Width - rect.X - 4;
                item.ClientRectangle = rect;
            }
        }

        /// <summary>
        /// Create a popup menu.
        /// </summary>
        protected override void CreatePopupMenu()
        {
            if (this.popupMenu == null)
                this.popupMenu = new FlatPopupMenu();

            this.popupMenu.BackColor = this.BackColor;
            this.popupMenu.BorderColor = this.BorderColor;
            this.popupMenu.SeparatorColor = this.SeparatorColor;
            this.popupMenu.TextColor = this.TextColor;

            this.popupMenu.HoverBackColor = this.HoverBackColor;
            this.popupMenu.HoverBorderColor = this.HoverBorderColor;
            this.popupMenu.HoverTextColor = this.HoverTextColor;

            this.popupMenu.DisabledTextColor = this.DisabledTextColor;

            this.popupMenu.Font = this.Font.Clone() as Font;
            this.popupMenu.HoverFont = this.HoverFont.Clone() as Font;

            this.popupMenu.EnableBorderDrawing = this.EnableBorderDrawing;
            this.popupMenu.EnableHoverBorderDrawing = this.EnableHoverBorderDrawing;
            this.popupMenu.EnableHoverBackDrawing = this.EnableHoverBackDrawing;

            this.popupMenu.MenuTimerInterval = this.MenuTimerInterval;

            this.popupMenu.Cursor = this.Cursor;
        }

        /// <summary>
        /// Calculate the location for the popup menu in screen coordinates.
        /// </summary>
        /// <param name="ptScreen">Screen location to compute.</param>
        protected override void GetPopupMenuScreenLocation(ref Point ptScreen)
        {
            if (this.currMenuItem == null)
                return;

            Point ptClient = new Point(this.ClientRectangle.Right - 1, this.currMenuItem.ClientRectangle.Top - 4);
            if (!this.EnableBorderDrawing)
                ptClient.X += 1;
            ptScreen = this.PointToScreen(ptClient);
        }
        #endregion FlatMenu overrides

        #region Mouse event handlers
        /// <summary>
        /// Override mouse movement to detect if mouse
        /// is near submenu arrow.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.IsMouseNearSubMenuArrow(e.X, e.Y))
            {
                if (this.popupMenu != null && !this.popupMenu.Visible)
                {
                    this.ShowPopupMenu();
                }
            }
        }
        #endregion Mouse event handlers
    }

    /// <summary>
    /// FlatMenuBar inherits from FlatMenu and implements
    /// a flat-style menu bar control. FlatMenuBar uses
    /// FlatPopupMenu in order to implement cascading
    /// submenus.
    /// </summary>
    public class FlatMenuBar : FlatMenu
    {
        #region Properties
        /// <summary>
        /// Set this property to true if you want menus to
        /// be popped open without having to click on the
        /// menu bar first (e.g., for simulating menus that
        /// you see on web pages).
        /// </summary>
        [Category("Behavior"),
         DefaultValue(false),
         Description("Enable or disable auto-showing of top-level popup menus.")]
        public bool AlwaysShowPopupMenu
        {
            get { return this.alwaysShowPopup; }
            set { this.alwaysShowPopup = value; }
        }
        #endregion Properties

        #region Constructor
        public FlatMenuBar()
        {
            this.isPopupStyleMenu = false;
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
            }
            base.Dispose(disposing);
        }
        #endregion Protected Dispose method

        #region Private methods
        /// <summary>
        /// Do design mode-only drawing.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        private void DrawDesignMode(Graphics g)
        {
            if (!this.DesignMode)
                return;

            string text = "FlatMenuBar";
            this.DrawMenuItemText(g, text, this.Font,
                this.TextColor, this.LeftMargin, this.TopMargin);
        }

        /// <summary>
        /// Draw the menu item text centered in the given
        /// rectangle.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="text">Text string to draw.</param>
        /// <param name="font">Text font.</param>
        /// <param name="color">Text color.</param>
        /// <param name="r">Rectangle bounds to draw in.</param>
        private void DrawMenuItemTextR(Graphics g, string text, Font font, Color color, Rectangle r)
        {
            using (Brush brush = new SolidBrush(color))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                Rectangle rnew = new Rectangle(r.X - 1, r.Y, r.Width, r.Height);
                g.DrawString(text, font, brush, (RectangleF)rnew, sf);
            }
        }

        /// <summary>
        /// Draw the given menu item.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        /// <param name="item">Menu item to draw.</param>
        private void DrawMenuItem(Graphics g, FlatMenuItem item)
        {
            if (item.ClientRectangle.Width < 2)
                return;

            if (item.Style == FlatMenuItemStyle.Separator)
            {
                int x = (item.ClientRectangle.Left + item.ClientRectangle.Right) / 2;
                int y1 = item.ClientRectangle.Top + 3;
                int y2 = item.ClientRectangle.Bottom - 3;
                this.DrawSeparator(g, this.SeparatorColor, x, y1, x, y2);
            }
            else if (this.currMenuItem == item && !this.ignoreNextMouseMove)
            {
                if (this.IsPopupMenuVisible() && this.currMenuItem.MenuItems.Count > 0)
                {
                    this.DrawBackground(g, item.ClientRectangle, this.popupMenu.BackColor);
                    if (this.EnableBorderDrawing)
                        this.DrawBorder(g, item.ClientRectangle, this.popupMenu.BorderColor);
                    this.DrawMenuItemTextR(g, item.Text, this.Font, this.popupMenu.TextColor,
                        item.ClientRectangle);
                }
                else
                {
                    if (this.EnableHoverBackDrawing)
                        this.DrawBackground(g, item.ClientRectangle, this.HoverBackColor);
                    if (this.EnableHoverBorderDrawing)
                        this.DrawBorder(g, item.ClientRectangle, this.HoverBorderColor);
                    this.DrawMenuItemTextR(g, item.Text, this.HoverFont, this.HoverTextColor,
                        item.ClientRectangle);
                }
            }
            else
            {
                Color color = this.TextColor;
                if (!item.Enabled)
                    color = this.DisabledTextColor;

                this.DrawMenuItemTextR(g, item.Text, this.Font, color,
                    item.ClientRectangle);
            }
        }

        /// <summary>
        /// Expand the given rectangle a bit. Used to create
        /// the hover rectangle for a highlighted menu item.
        /// </summary>
        /// <param name="rect">Rectangle to expand.</param>
        private void AdjustMenuItemRect(ref Rectangle rect)
        {
            rect.Y -= 2;
            rect.Height += 4;
            rect.X -= 3;
            rect.Width += 7;

            // Increase width if either font is bold.
            if (this.Font != null && this.Font.Bold)
                rect.Width += 3;
            else if (this.HoverFont != null && this.HoverFont.Bold)
                rect.Width += 3;
        }
        #endregion Private methods

        #region FlatMenu overrides
        /// <summary>
        /// Draw the menu control.
        /// </summary>
        /// <param name="g">Graphics object to draw with.</param>
        protected override void DrawMenu(Graphics g)
        {
            // Draw the background for the menu bar.
            DrawBackground(g, this.ClientRectangle, this.BackColor);

            // Draw the border for the menu bar.
            if (this.EnableBorderDrawing)
                DrawBorder(g, this.ClientRectangle, this.BorderColor);

            // Check for design mode.
            if (this.DesignMode)
            {
                this.DrawDesignMode(g);
                return;
            }

            // Draw each menu item.
            for (int i = 0; i < this.MenuItems.Count; ++i)
            {
                var item = this.MenuItems.GetAt(i);
                Debug.Assert(item != null);

                DrawMenuItem(g, item);
            }
        }

        /// <summary>
        /// Re-calculate the menu item rects.
        /// </summary>
        /// <param name="g">Graphics object needed to use
        /// MeasureString.</param>
        protected override void RepositionMenuItems(Graphics g)
        {
            int xoffset = this.LeftMargin;
            int yoffset = this.TopMargin;

            for (int i = 0; i < this.MenuItems.Count; ++i)
            {
                FlatMenuItem item = this.MenuItems.GetAt(i);
                Debug.Assert(item != null);

                string text = item.Text;
                if (text == String.Empty)
                    text = " ";
                SizeF textSize = g.MeasureString(text, this.Font);
                SizeF hoverTextSize = g.MeasureString(text, this.HoverFont);
                Rectangle rect = new Rectangle(xoffset, yoffset,
                    (int)(Math.Max(textSize.Width, hoverTextSize.Width) + 0.5),
                    (int)(Math.Max(textSize.Height, hoverTextSize.Height) + 0.5));

                AdjustMenuItemRect(ref rect);
                if (item.Style == FlatMenuItemStyle.Separator)
                {
                    rect.Offset(-this.MenuItemSpacing / 2, 0);
                    rect.Width = 3;
                }
                item.ClientRectangle = rect;

                if (item.Style == FlatMenuItemStyle.Separator)
                    xoffset += rect.Width;
                else
                    xoffset += (this.MenuItemSpacing + rect.Width);
            }
        }

        /// <summary>
        /// Create a popup menu.
        /// </summary>
        protected override void CreatePopupMenu()
        {
            if (this.popupMenu == null)
                this.popupMenu = new FlatPopupMenu();

            this.popupMenu.BorderColor = this.HoverBorderColor; // Special case - this is not an error.
            this.popupMenu.SeparatorColor = this.SeparatorColor;
            this.popupMenu.TextColor = this.TextColor;

            this.popupMenu.HoverBackColor = this.HoverBackColor;
            this.popupMenu.HoverBorderColor = this.HoverBorderColor;
            this.popupMenu.HoverTextColor = this.HoverTextColor;

            this.popupMenu.DisabledTextColor = this.DisabledTextColor;

            this.popupMenu.Font = this.Font.Clone() as Font;
            this.popupMenu.HoverFont = this.HoverFont.Clone() as Font;

            this.popupMenu.EnableHoverBorderDrawing = this.EnableHoverBorderDrawing;
            this.popupMenu.EnableHoverBackDrawing = this.EnableHoverBackDrawing;

            this.popupMenu.MenuTimerInterval = this.MenuTimerInterval;

            this.popupMenu.Cursor = this.Cursor;
        }

        /// <summary>
        /// Determine the screen location for positioning the popup menu.
        /// </summary>
        /// <param name="ptScreen">Screen location to be computed.</param>
        protected override void GetPopupMenuScreenLocation(ref Point ptScreen)
        {
            if (this.currMenuItem == null)
                return;

            Rectangle rect = this.currMenuItem.ClientRectangle;
            Point ptClient = rect.Location;
            ptClient.Y += rect.Height - 1;
            ptScreen = this.PointToScreen(ptClient);
        }
        #endregion FlatMenu overrides

        #region Mouse event handlers
        /// <summary>
        /// Override mouse down event.
        /// </summary>
        /// <param name="e">Mouse event argument.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (this.currMenuItem == null ||
                 this.currMenuItem.MenuItems.Count == 0 ||
                 this.AlwaysShowPopupMenu)
                return;

            // Toggle showing of popup menu.
            if (this.needShowPopupMenu)
            {
                this.needShowPopupMenu = false;
                this.Invalidate();
                this.HidePopupMenu();
            }
            else
            {
                this.needShowPopupMenu = true;
                this.Invalidate();
                this.ShowPopupMenu();
            }
        }
        #endregion Mouse event handlers
    }
}

// END
