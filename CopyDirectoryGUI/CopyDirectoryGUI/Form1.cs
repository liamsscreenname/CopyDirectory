using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace CopyDirectoryGUI
{
    public partial class Form1 : Form
    {
        private string source = null, destination = null;
        private CopyDirectory cDir;
        private Form3 frm3;

        public Form1()
        {
            InitializeComponent();
            button1.Click += new EventHandler(button1_Click);
            button2.Click += new EventHandler(button2_Click);
            copy.Click += new EventHandler(copy_Click);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void setSource(string s)
        {
            source = s;
        }

        public void setDestination(string d)
        {
            destination = d;
        }

        private void copy_Click(object sender, EventArgs args)
        {
            if (source != null && destination != null)
            {
                cDir = new CopyDirectory(source, destination);
                //Run the copy function in the background
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(bg_DoWork);
                bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
                bg.RunWorkerAsync();

                frm3 = new Form3();
                frm3.Show();
                //Update Form3 in background while copy function is running to show currently copying file
                BackgroundWorker bg2 = new BackgroundWorker();
                bg2.DoWork += new DoWorkEventHandler(bg2_DoWork);
                bg2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg2_RunWorkerCompleted);
                bg2.RunWorkerAsync();
            }
            else
                MessageBox.Show("Please select a valid source and Destination", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void bg_DoWork(object sender, DoWorkEventArgs args)
        {
            cDir.Copy();
        }

        private void bg2_DoWork(object sender, DoWorkEventArgs args)
        {
            while (cDir.copying)
            {
                Invoke(new MethodInvoker(delegate() { frm3.setLabel(cDir.current); }));
            }
        }

        //Run this when copy is complete - prompt user to copy again or quit program
        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            Console.WriteLine("Copying Done");
            if (MessageBox.Show("Done! Copy More Files?", "Done!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                source = null;
                destination = null;
                updateGUI();
            }
            else
                Close();
        }

        private void bg2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            Console.WriteLine("Copying Done 2");
            frm3.Hide();
        }

        private void button1_Click(object sender, EventArgs args)
        {
            Form2 frm2 = new Form2(this, true);
            frm2.ShowDialog();
            while (frm2.selecting)
            {

            }
            updateGUI();
        }

        private void button2_Click(object sender, EventArgs args)
        {
            Form2 frm2 = new Form2(this, false);
            frm2.ShowDialog();
            while (frm2.selecting)
            {

            }
            updateGUI();
        }

        private void updateGUI()
        {
            if (source != null)
            {
                if (File.Exists(source))
                    pictureBox1.Image = imageList1.Images[0];
                else
                    pictureBox1.Image = imageList1.Images[1];
                textBox1.Text = source;
            }
            else
            {
                pictureBox1.Image = null;
                textBox1.Text = "None Selected";
            }
            if (destination != null)
            {
                pictureBox2.Image = imageList1.Images[1];
                textBox2.Text = destination;
            }
            else
            {
                pictureBox2.Image = null;
                textBox2.Text = "None Selected";
            }
        }
    }
}
