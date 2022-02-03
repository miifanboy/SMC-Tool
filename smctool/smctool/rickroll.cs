using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smctool
{
    public partial class rickroll : MaterialForm
    {
        public rickroll()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
        }
        int i = 0;
        private void rickroll_Load(object sender, EventArgs e)
        {
            Screen scr = Screen.PrimaryScreen;
            Rectangle rec = scr.Bounds;
            this.Width = rec.Width;
            this.Height = rec.Height;
            this.Location = new Point(0, 0);
            this.TopMost = true;
            webBrowser1.Size = new System.Drawing.Size(rec.Width, rec.Height);
            webBrowser1.Location = new Point(0, 0);
            webBrowser1.Width = rec.Width;
            webBrowser1.Height = rec.Height;
            webBrowser1.Navigate("www.youtube.com/watch?v=QDia3e12czc&autoplay=1");
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if(i == 10)
            {
                this.Close();
            }
            i++;
        }
    }
}
