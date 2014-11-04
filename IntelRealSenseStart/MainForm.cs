using System;
using System.Windows.Forms;
using IntelRealSenseStart.Code;

namespace IntelRealSenseStart
{
    public partial class MainForm : Form
    {
        private readonly RealSenseHandsDeterminer realSenseHandsDeterminer;

        public MainForm()
        {
            InitializeComponent();
            realSenseHandsDeterminer = new RealSenseHandsDeterminer();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!realSenseHandsDeterminer.Started)
            {
                realSenseHandsDeterminer.Start();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (realSenseHandsDeterminer.Started)
            {
                realSenseHandsDeterminer.Stop();
            }
        }
    }
}