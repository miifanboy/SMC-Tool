using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using Gameloop.Vdf.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Security.Principal;
using Mayerch1.GithubUpdateCheck;
namespace smctool
{

    public partial class SMCTool : MaterialForm
    {
        public SMCTool()
        {
            
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
        }
        
        
        private string GetCSGODir()
        {
            string steamPath = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", "");
            string pathsFile = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");

            if (!File.Exists(pathsFile))
                return null;

            List<string> libraries = new List<string>();
            libraries.Add(Path.Combine(steamPath));

            var pathVDF = File.ReadAllLines(pathsFile);
            // Okay, this is not a full vdf-parser, but it seems to work pretty much, since the 
            // vdf-grammar is pretty easy. Hopefully it never breaks. I'm too lazy to write a full vdf-parser though. 
            Regex pathRegex = new Regex(@"\""(([^\""]*):\\([^\""]*))\""");
            foreach (var line in pathVDF)
            {
                if (pathRegex.IsMatch(line))
                {
                    string match = pathRegex.Matches(line)[0].Groups[1].Value;

                    // De-Escape vdf. 
                    libraries.Add(match.Replace("\\\\", "\\"));
                }
            }

            foreach (var library in libraries)
            {
                string csgoPath = Path.Combine(library, "steamapps\\common\\Counter-Strike Global Offensive\\csgo");
                if (Directory.Exists(csgoPath))
                {
                    return csgoPath;
                }
            }

            return null;
        }
        public class outwp
        {
            public Dictionary<string, string> attributes = new Dictionary<string, string>();
        }
        public class weapon
        {
            public string name { get; set; }
            public string id { get; set; }
            public Dictionary<string, string> attributes = new Dictionary<string, string>();  
        }
        public class update
        {
            public string newversion { get; set; }
            public bool updateavailable { get; set; }
        }
        List<outwp> outwps;
        List<weapon> wplist;
        List<MaterialCheckbox> boxlist;
        List<MaterialTextBox2> textlist;
        List<MaterialTextBox2> textfloatlist;
        ModFiles modFiles;
        Search search;
        Encoding utf8WithoutBom;
        bool IsChanged = false;
        bool CanStart = false;
        string csgopath;
        string rootpath;
        [DllImport("kernel32.dll")]
        static extern int CreateSymbolicLink(string lpSymlinkFileName,
        string lpTargetFileName, int dwFlags);
        public static void LinkDirectory(string sourceDirName, string destDirName,string path)
        {
            if (CreateSymbolicLink(sourceDirName, destDirName, 0x1) != 1)
            {
                MessageBox.Show("Error: Unable to create symbolic link. " +
                    "(Error Code: " + Marshal.GetLastWin32Error() + ")" + " - Please launch this app as administrator!");
                if(Directory.Exists(path + "smc\\"))
                {
                    Directory.Delete(path + "smc\\",true);
                    
                }
  
            }
        }
        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            if(CanStart)
            {
                IsChanged = true;
                if (IsChanged)
                {
                    this.Text = "SMC Tool v" + Application.ProductVersion + " *Unapplied Changes";
                }
                else
                {
                    this.Text = "SMC Tool v" + Application.ProductVersion;
                }
            }
              
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {



        }
        void LoadTextboxes()
        {
            weapon wp = wplist.Find(i => i.name == selectedweapon.Text);
            
            var keys = wp.attributes;
            foreach (MaterialTextBox2 box in textlist)
            {
                box.Text = "";
                foreach (var key in keys)
                {
                    if (box.Name == key.Key)
                    {
                        box.Text = Convert.ToString(Convert.ToInt32(key.Value));
                    }
                }
            }
        }
        void CreateTextBox(JObject attributes)
        {
            foreach (KeyValuePair<string, JToken> attr in attributes)
            {
                int y = 0;
                foreach (string name in attr.Value["int"])
                {
                    MaterialTextBox2 tempbox = new MaterialTextBox2();
                    MaterialLabel label = new MaterialLabel();
                    label.Name = name;
                    label.Text = name;
                    label.AutoSize = true;
                    label.Location = new Point(250, y + 7);
                    tempbox.Name = name;
                    tempbox.AutoSize = true;
                    tempbox.Location = new Point(450, y + 7);
                    tempbox.UseTallSize = false;
                    tempbox.TextChanged += txtBox_TextChanged;
                    textlist.Add(tempbox);
                    tabPage1.Controls.Add(label);
                    tabPage1.Controls.Add(tempbox);
                    y += 40;   
                }
            }    
        }
        void SaveTextboxes()
        {
            List<weapon> newsave = new List<weapon>();
            foreach (weapon wp in wplist)
            {
                if (wp.name == selectedweapon.Text)
                {
                    foreach (MaterialTextBox2 box in textlist)
                    {
                        try
                        {
                            if (wp.attributes.ContainsKey(box.Name))
                            {
                                wp.attributes[box.Name] = Convert.ToString(Convert.ToInt32(box.Text));
                            }
                            else
                            {
                                wp.attributes.Add(box.Name, Convert.ToString(Convert.ToInt32(box.Text)));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                newsave.Add(wp);
            }
            wplist = newsave;
        }
        void LoadTextboxesFloat()
        {
            weapon wp = wplist.Find(i => i.name == selectedweapon.Text);

            var keys = wp.attributes;
            foreach (MaterialTextBox2 box in textfloatlist)
            {
                box.Text = "";
                foreach (var key in keys)
                {
                    if (box.Name == key.Key)
                    {
                        box.Text = Convert.ToString(key.Value);
                    }
                }
            }
            CanStart = true;
        }
        void CreateTextBoxFloat(JObject attributes)
        {
            int count = 0;
            foreach (KeyValuePair<string, JToken> attr in attributes)
            {
                int y = 0;
                foreach (string name in attr.Value["float"])
                {
                    
                    MaterialTextBox2 tempbox = new MaterialTextBox2();
                    MaterialLabel label = new MaterialLabel();
                    label.Name = name;
                    label.Text = name;
                    label.AutoSize = true;
                    if(count <= 5)
                    {
                        label.Location = new Point(250, y + 167);
                        tempbox.Location = new Point(450, y + 167);
                    }
                    if (count > 5)
                    {
                        
                        label.Location = new Point(730, y + 7);
                        tempbox.Location = new Point(930, y + 7);
                        
                    }

                    tempbox.Name = name;
                    tempbox.AutoSize = true;
                    
                    tempbox.UseTallSize = false;
                    tempbox.TextChanged += txtBox_TextChanged;
                    textfloatlist.Add(tempbox);
                    tabPage1.Controls.Add(label);
                    tabPage1.Controls.Add(tempbox);
                    y += 40;
                    count++;
                    if(count == 6)
                    {
                        y = 0;
                    }

                }
            }
        }
        void SaveTextboxesFloat()
        {
            List<weapon> newsave = new List<weapon>();
            foreach (weapon wp in wplist)
            {
                if (wp.name == selectedweapon.Text)
                {
                    foreach (MaterialTextBox2 box in textfloatlist)
                    {
                        try
                        {
                            if (wp.attributes.ContainsKey(box.Name))
                            {
                                wp.attributes[box.Name] = Convert.ToString(box.Text);
                            }
                            else
                            {
                                wp.attributes.Add(box.Name, Convert.ToString(box.Text));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                newsave.Add(wp);
            }
            wplist = newsave;
        }
        void LoadCheckboxes()
        {
            weapon wp = wplist.Find(i => i.name == selectedweapon.Text);
            var keys = wp.attributes;
            foreach (MaterialCheckbox box in boxlist)
            {
                box.Checked = false;
                foreach (var key in keys)
                {

                    if (box.Name == key.Key)
                    {
                        box.Checked = Convert.ToBoolean(Convert.ToInt32(key.Value));
                    }
                }
            }
        }
        void CreateCheckboxes(JObject attributes)
        {
            foreach (KeyValuePair<string, JToken> attr in attributes)
            {
                int y = 0;
                foreach (string name in attr.Value["bool"])
                {
                    MaterialCheckbox tempbox = new MaterialCheckbox();
                    tempbox.Name = name;
                    tempbox.Text = name;
                    tempbox.AutoSize = true;
                    tempbox.Location = new Point(10, y + 7);
                    boxlist.Add(tempbox);
                    tabPage1.Controls.Add(tempbox);
                    y += 36;
                }
            }
        }
        void ReloadCheckboxes(JObject data)
        {
            List<weapon> newlist = new List<weapon>();
            foreach (KeyValuePair<string, JToken> b in data)
            {
                var val = b.Value;
                var obj = (JObject)val;
               
                JObject pairs = (JObject)val;
                foreach (KeyValuePair<string, JToken> a in pairs)
                {
                    weapon temp = wplist.Find(i => i.name == selectedweapon.Text);
                    if (a.Key == temp.id)
                    {
                    var chk = a.Value["attributes"];
                    weapon wp = wplist.Find(i => i.id == Convert.ToString(a.Key));
                        //temp.name = Convert.ToString(a.Value["name"]);
                        //temp.id = Convert.ToString(a.Key);
                        if (wp != null)
                        {
                            wp.attributes.Clear();
                            foreach (JProperty j in chk)
                            {
                                //string[] tempattr = { j.Name, Convert.ToString(j.Value) };
                                //temp.attributes.Add(tempattr);
                                try
                                {

                                    wp.attributes.Add(j.Name, Convert.ToString(j.Value));
                                }
                                catch
                                {
                                    if (wp.attributes[j.Name] != null)
                                    {
                                        wp.attributes[j.Name] = Convert.ToString(j.Value);
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            LoadCheckboxes();
            LoadTextboxes();
            LoadTextboxesFloat();
        }
        void CreateWeaponObjects(JObject data, JObject def)
        {
            foreach (KeyValuePair<string, JToken> a in data)
            {
                weapon temp = new weapon();
                
                var chk = a.Value["attributes"];
                temp.name = Convert.ToString(a.Value["name"]);
                temp.id = Convert.ToString(a.Key);
                if (chk != null)
                {
                    foreach (JProperty j in chk)
                    {
                        //string[] tempattr = { j.Name, Convert.ToString(j.Value) };
                        //temp.attributes.Add(tempattr);
                        temp.attributes.Add(j.Name, Convert.ToString(j.Value));

                    }

                }

                wplist.Add(temp);
                selectedweapon.Items.Add(temp.name);

            }
            selectedweapon.SelectedIndex = 0;
        }
        void MigiExport(string modname,string rootpath)
        {
            SaveToFile("items_game_"+modname + ".txt");
            Directory.CreateDirectory(rootpath + "migi\\csgo\\addons\\"+"m_"+modname+"\\scripts\\items\\");
            modFiles.Move("items_game_" + modname + ".txt", rootpath + "migi\\csgo\\addons\\" + "m_" + modname + "\\scripts\\items\\" + "items_game_" + modname + ".txt");
            if(File.Exists(rootpath + "migi\\csgo\\addons\\" + "m_" + modname + "\\scripts\\items\\" + "items_game_" + modname + ".txt"))
            {
                MessageBox.Show("Exported successfully!", "SMC Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        void SaveCheckboxes()
        {
            List<weapon> newsave = new List<weapon>();
            foreach (weapon wp in wplist)
            {
                if (wp.name == selectedweapon.Text)
                {
                    foreach (CheckBox box in boxlist)
                    {
                        try
                        {
                            if (wp.attributes.ContainsKey(box.Name))
                            {
                                wp.attributes[box.Name] = Convert.ToString(Convert.ToInt32(box.Checked));
                            }
                            else
                            {
                                wp.attributes.Add(box.Name, Convert.ToString(Convert.ToInt32(box.Checked)));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                newsave.Add(wp);
            }
            wplist = newsave;
        }
        string RemoveBr(string txt)
        {
            var aStringBuilder = new StringBuilder(txt);
            aStringBuilder.Remove(txt.Length-3, 2);
            aStringBuilder.Remove(0, 1);
            txt = aStringBuilder.ToString();
            return txt;
        }
        void SaveToFile(string filename)
        {
            JObject obj = new JObject();
            foreach (var wp in wplist)
            {
                outwp outw = new outwp();
                outw.attributes = wp.attributes;
                obj[wp.id] = JObject.FromObject(outw);
            }
            var serialized = JsonConvert.SerializeObject(new { items = obj });
            var deserialize = JsonConvert.DeserializeObject(serialized);
            
            var serialize2 = JsonConvert.SerializeObject(new { items_game = deserialize});
         
            var deserialize2 = (JObject)JsonConvert.DeserializeObject(serialize2);
            
            var vdf = deserialize2.ToVdf();
            var final = RemoveBr(VdfConvert.Serialize(vdf));
            var load = VdfConvert.Deserialize(final);
            var realfinal = VdfConvert.Serialize(load);
            File.WriteAllText(filename,realfinal,utf8WithoutBom);
        }
        public void ShowMyDialogBox()
        {
            migiexport testDialog = new migiexport();

            
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                MigiExport(testDialog.materialTextBox21.Text, rootpath);
                
            }
            else
            {
                testDialog.Close();
            }
            testDialog.Dispose();
        }
        void UpdateChecker()
        {
            GithubUpdateCheck update = new GithubUpdateCheck("miifanboy", "SMC-Tool");
            update newupdate = new update();            
            newupdate.updateavailable = update.IsUpdateAvailable(Application.ProductVersion, VersionChange.Revision);
            newupdate.newversion = update.Version();
            if(newupdate.updateavailable)
            {
                MessageBox.Show("New version " + "v" + newupdate.newversion + " is out on github.com! \nPlease install the newest version.","SMC Tool",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bool IsElevated = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            if (!IsElevated)
            {
                MessageBox.Show("Please run the app as administrator", "SMC Tool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            UpdateChecker();
            this.Text = "SMC Tool v" + Application.ProductVersion;
            csgopath = GetCSGODir();
            rootpath = csgopath.Remove(csgopath.Length - 4, 4);
            outwps = new List<outwp>();
            wplist = new List<weapon>();
            boxlist = new List<MaterialCheckbox>();
            textlist = new List<MaterialTextBox2>();
            textfloatlist = new List<MaterialTextBox2>();
            JProperty weaponsp = VdfConvert.Deserialize(File.ReadAllText("weapons.txt")).ToJson();
            JObject attributes = JObject.Parse(File.ReadAllText("attributes.json"));
            JProperty defaultvalues = VdfConvert.Deserialize(File.ReadAllText("items_game_default.txt")).ToJson();
            JToken weapons = weaponsp.Value;
            JToken defaults = defaultvalues.Value;
            CreateCheckboxes(attributes);
            CreateTextBox(attributes);
            CreateTextBoxFloat(attributes);
            CreateWeaponObjects((JObject)weapons, (JObject)defaults);
            modFiles = new ModFiles();
            search = new Search();
        }

        private void selectedweapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            applytime.ForeColor = Color.Black;
            applytime.Text = "Never";
            IsChanged = false;
            CanStart = false;
            LoadCheckboxes();
            LoadTextboxes();
            LoadTextboxesFloat();
            utf8WithoutBom = new UTF8Encoding(false);
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text Files | *.txt";
            fileDialog.DefaultExt = "txt";
            fileDialog.FileName = "items_game_default.txt";
            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                JProperty defaultvalues = VdfConvert.Deserialize(File.ReadAllText(fileDialog.InitialDirectory + fileDialog.FileName,utf8WithoutBom)).ToJson();
                JToken defaults = defaultvalues.Value;
                ReloadCheckboxes((JObject)defaults);
            }
            

        }
        private void button1_Click(object sender, EventArgs e)
        {
            SaveCheckboxes();
            SaveTextboxes();
            SaveTextboxesFloat();
            DateTime time = DateTime.Now;
            applytime.ForeColor = Color.Green;
            applytime.Text = time.ToLocalTime().ToString();
            IsChanged = false;
            this.Text = "SMC Tool v" + Application.ProductVersion;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToFile("items_game_smcmod.txt");
            if (File.Exists("items_game_smcmod.txt"))
            {
                MessageBox.Show("Saved Successfully!", "SMC Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            string targetpath = rootpath + "smc\\csgo\\";
            if(Directory.Exists(targetpath))
            {
                File.Copy("items_game_smcmod.txt", Directory.GetCurrentDirectory() + "\\addons\\m_smcmod\\scripts\\items\\items_game_smcmod.txt", true);
                modFiles.CreateVPK(rootpath);
                modFiles.CopyFiles(targetpath);
                modFiles.LaunchMod();
            }
            else
            {
                //create files and directories
                Directory.CreateDirectory(targetpath);
                Directory.CreateDirectory(targetpath + "\\addons\\m_smcmod\\scripts\\items\\");
                File.Copy("items_game_smcmod.txt", Directory.GetCurrentDirectory() + "\\addons\\m_smcmod\\scripts\\items\\items_game_smcmod.txt",true);
                modFiles.SetupVPK(csgopath + "\\scripts\\items\\items_game.txt");
                modFiles.CreateVPK(rootpath);
                LinkDirectory(targetpath + "\\maps", csgopath + "\\maps",rootpath);
                LinkDirectory(targetpath + "\\panorama", csgopath + "\\panorama",rootpath);
                if(!Directory.Exists(rootpath + "smc\\"))
                {
                    this.Close();
                }
                modFiles.CopyFiles(targetpath);
                modFiles.LaunchMod();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            JProperty defaultvalues = VdfConvert.Deserialize(File.ReadAllText("items_game_default.txt")).ToJson();
            JToken defaults = defaultvalues.Value;
            ReloadCheckboxes((JObject)defaults);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Text Files | *.txt";
            fileDialog.DefaultExt = "txt";
            fileDialog.FileName = "items_game_smcmod";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveToFile(fileDialog.InitialDirectory + fileDialog.FileName);
                if(File.Exists(fileDialog.InitialDirectory + fileDialog.FileName))
                {
                    MessageBox.Show("Saved Successfully", "SMC Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {

            search.Show();

        }

        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if(materialSwitch1.Checked)
            {
                var materialSkinManager = MaterialSkinManager.Instance;
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            }
            else
            {
                var materialSkinManager = MaterialSkinManager.Instance;
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            }
            
        }

        private void exportForMIGIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMyDialogBox();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure ?", "Gaben", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                rickroll rickroll = new rickroll();
                rickroll.Show();
            }
        }
    }
}
