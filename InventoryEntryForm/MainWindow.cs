using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryEntryForm
{
    public partial class MainWindow : Form
    {
        private Form1 invEntry;
        private Companies compWindow;
        
        public MainWindow()
        {
            InitializeComponent();
            invEntry = new Form1();
            invEntry.MdiParent = this;
            invEntry.Show();
            compWindow = new Companies();
            compWindow.MdiParent = this;
        }

        private void inventoryEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (invEntry.IsDisposed)
            {
                invEntry = new Form1();
                invEntry.MdiParent = this;
            }
            invEntry.Show();
            compWindow.Hide();
        }

        private void companiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (compWindow.IsDisposed)
            {
                compWindow = new Companies();
                compWindow.MdiParent = this;
            }
            compWindow.Show();
            invEntry.Hide();
        }
    }
}
