using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using MaterialSkinDataTable.MaterialSkin2DotNet.Controls;
using MaterialSkinDataTable;
namespace smctool
{
    public partial class Search : MaterialForm
    {
        public Search()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
        }
        DataTable dttable;
        private void Search_Load(object sender, EventArgs e)
        {
            JObject attributes = JObject.Parse(File.ReadAllText("search.json"));
            int count = attributes.Count;
            dttable = new DataTable();
            dttable.Columns.Add("Attribute");
            dttable.Columns.Add("Description");
            MaterialDataTable datatable = new MaterialDataTable();
            datatable.AllowUserToAddRows = false;
            datatable.AllowUserToDeleteRows = false;
            datatable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            datatable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            datatable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            datatable.Location = new System.Drawing.Point(11, 94);
            datatable.Name = "datatable";
            datatable.ReadOnly = true;
            datatable.Size = new System.Drawing.Size(875, 456);
            datatable.TabIndex = 2;
            this.Controls.Add(datatable);
            textBox1.TextChanged += searchtxt_changed;
            foreach (var val in attributes)
            {

                string[] temp = { val.Key, (string)val.Value };
                dttable.Rows.Add(temp);
            }
            datatable.DataSource = dttable;
            Font font = new Font(SystemFonts.DefaultFont,FontStyle.Regular);

            datatable.Font = font;
            datatable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
        }
        private void searchtxt_changed(object sender, EventArgs e)
        {
            dttable.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '%{1}%'", "Attribute", textBox1.Text);
        }
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private void backButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void search_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void search_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void search_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        private void materialButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
