namespace PathwayViewer
{
    using System;
    using System.Drawing.Drawing2D;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// This class is used to repaint controls with a specified format. It's comparable with an .css file used by webpages.
    /// </summary>
    public class Style
    {
        #region PROPERTIES

        private FormPathwayViewer MainForm = null;

        public Color AlternatingRow = ColorTranslator.FromHtml("#E8F5FE");
        public Color GradientLight = ColorTranslator.FromHtml("#90AEF9");
        public Color GradientMedium = ColorTranslator.FromHtml("#3465DF");
        public Color GradientDark = ColorTranslator.FromHtml("#1B3475");
        public Color ReadOnly = ColorTranslator.FromHtml("#CFCEDE");
        public Color Black = ColorTranslator.FromHtml("#000000");
        public Color White = ColorTranslator.FromHtml("#FFFFFF");
        public Color Question = ColorTranslator.FromHtml("#FDC400");
        public Color Error = ColorTranslator.FromHtml("#FF0033");
        public Color Info = ColorTranslator.FromHtml("#00CC00");
        public Color Warning = ColorTranslator.FromHtml("#FF9E35");

        #endregion

        #region CONSTRUCTOR

        public Style(FormPathwayViewer mainForm)
        {
            this.MainForm = mainForm;
        }

        #endregion

        #region PUBLIC METHODS

        public void ApplyStyle(Control control)
        {
            try
            {
                SetDoubleBuffering(control, true);
                
                if (control.Tag != null && control.Tag.ToString().ToUpper() == "CUSTOM")
                {
                    // Use a custom style
                }
                else
                {
                    switch (control.GetType().Name)
                    {
                        case "Form":
                            Form F = (Form)control;
                            F.BackColor = GradientDark;
                            F.ForeColor = Black;
                            break;

                        case "TabPage":
                            TabPage Tp = (TabPage)control;
                            Tp.Font = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                            Tp.BackColor = GradientDark;
                            Tp.ForeColor = Black;
                            break;

                        case "TabControl":
                            TabControl Tc = (TabControl)control;
                            Tc.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Bold);
                            Tc.BackColor = GradientDark;
                            Tc.ForeColor = Black;
                            break;

                        case "Panel":
                            Panel P = (Panel)control;
                            P.BackColor = GradientDark;
                            P.ForeColor = Black;
                            break;

                        case "GroupBox":
                            GroupBox Gb = (GroupBox)control;
                            Gb.Paint += new PaintEventHandler(GroupBox_Paint);
                            Gb.BackColor = GradientDark;
                            Gb.ForeColor = Black;
                            break;

                        case "DataGridView":
                            DataGridView DG = (DataGridView)control;
                            DG.ForeColor = Color.Black;
                            DG.CellPainting += new DataGridViewCellPaintingEventHandler(DG_CellPainting);
                            DG.BackgroundColor = White;
                            DG.ColumnHeadersDefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Bold);
                            DG.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                            DG.AlternatingRowsDefaultCellStyle.BackColor = AlternatingRow;
                            DG.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
                            DG.BorderStyle = BorderStyle.Fixed3D;
                            break;

                        case "Label":
                            Label L = (Label)control;
                            L.BackColor = GradientDark;
                            L.ForeColor = GradientLight;
                            L.BorderStyle = BorderStyle.None;
                            L.TextAlign = ContentAlignment.MiddleLeft;
                            L.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Bold);
                            break;

                        case "TextBox":
                            TextBox Tb = (TextBox)control;
                            if (Tb.BackColor != Question)
                            {
                                if (Tb.Enabled && !Tb.ReadOnly)
                                {
                                    Tb.BackColor = White;
                                }
                                else
                                {
                                    Tb.BackColor = ReadOnly;
                                }
                            }
                            Tb.ForeColor = Black;
                            Tb.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                            break;

                        case "TextBoxBase":
                            TextBoxBase Tbb = (TextBoxBase)control;
                            if (Tbb.BackColor != Question)
                            {
                                if (Tbb.Enabled && !Tbb.ReadOnly)
                                {
                                    Tbb.BackColor = White;
                                }
                                else
                                {
                                    Tbb.BackColor = ReadOnly;
                                }
                            }
                            Tbb.ForeColor = Black;
                            Tbb.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                            break;

                        case "ComboBox":
                            ComboBox Cb = (ComboBox)control;
                            if (Cb.BackColor != Question)
                            {
                                if (Cb.Enabled)
                                {
                                    Cb.BackColor = White;
                                }
                                else
                                { 
                                    Cb.BackColor = ReadOnly;
                                }
                            }
                            Cb.ForeColor = Black;
                            Cb.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                            break;

                        case "ListControl":
                            ListControl Lc = (ListControl)control;
                            if (Lc.BackColor != Question)
                            {
                                if (Lc.Enabled)
                                {
                                    Lc.BackColor = White;
                                }
                                else 
                                {
                                    Lc.BackColor = ReadOnly;
                                }
                            }
                            Lc.ForeColor = Black;
                            Lc.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                            break;

                        case "Button":
                            Button B = (Button)control;
                            B.FlatStyle = FlatStyle.Standard;
                            B.BackColor = GradientLight;
                            B.BackgroundImageLayout = ImageLayout.Center;
                            break;

                        case "ButtonBase":
                            ButtonBase Bb = (ButtonBase)control;
                            Bb.FlatStyle = FlatStyle.Standard;
                            Bb.BackColor = GradientLight;
                            Bb.BackgroundImageLayout = ImageLayout.Center;
                            break;

                        case "CheckBox":
                            CheckBox Ch = (CheckBox)control;
                            Ch.BackColor = GradientDark;
                            Ch.ForeColor = GradientLight;
                            Ch.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Bold);
                            break;

                        case "PictureBox":
                            PictureBox Pb = (PictureBox)control;
                            Pb.BackColor = GradientDark;
                            break;

                        case "SplitContainer":
                            SplitContainer Sc = (SplitContainer)control;
                            Sc.BackColor = GradientDark;
                            break;

                        case "SplitterPanel":
                            SplitterPanel Sp = (SplitterPanel)control;
                            Sp.BackColor = GradientDark;
                            break;

                        case "NumericUpDown":
                            NumericUpDown nud = (NumericUpDown)control;
                            nud.Font = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                            break;

                        default:
                            break;
                    }
                }

                foreach (Control Child in control.Controls)
                {
                    ApplyStyle(Child);
                }
            }
            catch (Exception ex)
            {
                this.MainForm.ShowMessage(FormPathwayViewer.MessageType.Error, string.Format("An error occurred while adjusting control properties.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion

        #region PRIVATE METHODS

        private void SetDoubleBuffering(Control control, bool doubleBuffered)
        {
            try
            {
                PropertyInfo property = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                property.SetValue(control, doubleBuffered, null);
            }
            catch (Exception ex)
            {
                this.MainForm.ShowMessage(FormPathwayViewer.MessageType.Error, string.Format("An error occurred while applying the doubleBuffered property on control: {4}.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name, control.Name));
            }
        }
        
        private void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            DrawGroupBox(sender as GroupBox, e.Graphics);
        }

        private void DrawGroupBox(GroupBox box, Graphics g)
        {
            try
            {
                if (box != null)
                {
                    Brush brush = new SolidBrush(White);
                    Pen pen = new Pen(brush);
                    SizeF strSize = g.MeasureString(box.Text, box.Font);
                    Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                                   box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                                   box.ClientRectangle.Width - 1,
                                                   box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                    // Clear text and border
                    g.Clear(box.BackColor);

                    // Draw text
                    g.DrawString(box.Text, box.Font, brush, box.Padding.Left, 0);

                    // Drawing Border
                    //Left
                    g.DrawLine(pen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                    //Right
                    g.DrawLine(pen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                    //Bottom
                    g.DrawLine(pen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                    //Top1
                    g.DrawLine(pen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left - 1, rect.Y));
                    //Top2
                    g.DrawLine(pen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width + 3), rect.Y), new Point(rect.X + rect.Width, rect.Y));
                }
            }
            catch (Exception ex)
            {
                this.MainForm.ShowMessage(FormPathwayViewer.MessageType.Error, string.Format("An error occurred while painting a groupBox.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        private void DG_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1 || e.ColumnIndex == -1)
                {
                    LinearGradientBrush br = new LinearGradientBrush(e.CellBounds, GradientDark, White, 90, true);
                    ColorBlend cb = new ColorBlend();
                    cb.Positions = new[] { 0, (float)0.5, 1 };
                    cb.Colors = new[] { GradientDark, GradientLight, White };
                    br.InterpolationColors = cb;

                    e.Graphics.FillRectangle(br, e.CellBounds);
                    e.Graphics.DrawRectangle(new Pen(Color.White), e.CellBounds);
                    e.PaintContent(e.ClipBounds);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                this.MainForm.ShowMessage(FormPathwayViewer.MessageType.Error, string.Format("An error occurred while painting a datagrid cell.{0}Method: {1}.{2}{0}Exception: {3}",
                    Environment.NewLine, ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion
    }
}
