using System;
using System.Drawing;
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
            realSenseHandsDeterminer = RealSenseFactory.GetHandsDeterminer();

            realSenseHandsDeterminer.SegmentationImage += realSenseHandsDeterminer_SegmentationImage;
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

        private void realSenseHandsDeterminer_SegmentationImage(Bitmap bitmap)
        {
            BeginInvoke(
                new RealSenseHandsDeterminer.NewBitmapDelegate(realSenseHandsDeterminer_SegmentationImage_Synchronized),
                bitmap);
        }

        private void realSenseHandsDeterminer_SegmentationImage_Synchronized(Bitmap bitmap)
        {
            pictureBoxHand.Image = bitmap;
        }
    }
}