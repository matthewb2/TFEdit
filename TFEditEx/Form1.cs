using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TFEditEx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textEditorControl1.ShowLineNumbers = true;
            //textEditorControl1.OptionsChanged();

        }
    }
}
