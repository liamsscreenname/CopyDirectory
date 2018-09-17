using System.Windows.Forms;

namespace CopyDirectoryGUI
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public void setLabel(string s)
        {
            label2.Text = s;
            label1.Refresh();
            label2.Refresh();
        }
    }
}
