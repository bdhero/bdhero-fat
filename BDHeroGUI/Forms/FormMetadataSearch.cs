using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BDHeroGUI.Forms
{
    public partial class FormMetadataSearch : Form
    {
        public string SearchQuery
        {
            get { return textBoxSearchQuery.Text; }
            set { textBoxSearchQuery.Text = value; }
        }

        public FormMetadataSearch(string searchQuery)
        {
            InitializeComponent();
            SearchQuery = searchQuery;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
