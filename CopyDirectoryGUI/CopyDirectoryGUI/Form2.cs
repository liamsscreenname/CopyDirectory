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

namespace CopyDirectoryGUI
{
    public partial class Form2 : Form
    {
        public bool selecting = true, isSource;
        private List<string> paths = new List<string>();
        private string selected = null;
        private Form1 mainForm;
        private DirectoryInfo currentDir = null;
        private Stack<DirectoryInfo> prevDirs = new Stack<DirectoryInfo>();

        public Form2(Form1 frm1, bool source)
        {
            InitializeComponent();
            mainForm = frm1;
            isSource = source;
            currentDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            showFiles(currentDir);
            listView1.ItemActivate += new EventHandler(listView1_ItemActivate);
            FormClosed += new FormClosedEventHandler(form2_FormClosed);
            button1.Click += new EventHandler(button1_Click);
            button2.Click += new EventHandler(button2_Click);
        }

        private void showFiles(DirectoryInfo cDir)
        {
            currentDir = cDir;
            //Clear lists
            listView1.Clear();
            paths.Clear();
            //Add Folders
            try
            {
                foreach (DirectoryInfo dir in currentDir.GetDirectories())
                {
                    listView1.Items.Add(dir.Name, 1);
                    paths.Add(dir.FullName);
                }
                //Add Files
                foreach (FileInfo file in currentDir.GetFiles())
                {
                    listView1.Items.Add(file.Name, 0);
                    paths.Add(file.FullName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs args)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int selIndex = listView1.SelectedItems[0].Index;
                //Display contents of double-clicked directory
                if (!File.Exists(paths[selIndex]))
                    prevDirs.Push(currentDir);
                    showFiles(new DirectoryInfo(paths[selIndex]));
            }
        }

        private void button1_Click(object sender, EventArgs args)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                selected = paths[listView1.SelectedItems[0].Index];
                if (isSource)
                {
                    mainForm.setSource(selected);
                    Close();
                    return;
                }
                //Ensure that a folder is selected as the Destination
                if (!File.Exists(selected))
                {
                    mainForm.setDestination(selected);
                    Close();
                    return;
                }
                else
                    MessageBox.Show("Please Select a Folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs args)
        {
            if (prevDirs.Count > 0)
                showFiles(prevDirs.Pop());
        }

        //Ensure program does not get stuck when user closes Form2
        private void form2_FormClosed(object sender, FormClosedEventArgs args)
        {
            selecting = false;
        }
    }
}
