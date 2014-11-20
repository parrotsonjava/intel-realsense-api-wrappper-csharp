using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Config.HandsImage;
using IntelRealSenseStart.Code.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Exception;

namespace IntelRealSenseStart.Code.RealSense.Component.Hands
{
    public class HandsImageCreator
    {
        private const int CONFIDENCE_THRESHOLD = 80;
        private static readonly Pen BONE_PEN = new Pen(Color.SlateBlue, 3.0f);

        private static readonly byte[] LUT;

        private readonly HandsData handsData;
        private readonly HandsImageConfiguration imageConfiguration;
        private readonly ImageData imageData;

        private readonly Configuration realSenseConfiguration;

        static HandsImageCreator()
        {
            LUT = Enumerable.Repeat((byte)0, 256).ToArray();
            LUT[255] = 1;
        }

        private HandsImageCreator(HandsData handsData, ImageData imageData,
            Configuration realSenseConfiguration, HandsImageConfiguration imageConfiguration)
        {
            this.handsData = handsData;
            this.imageData = imageData;

            this.realSenseConfiguration = realSenseConfiguration;
            this.imageConfiguration = imageConfiguration;
        }

        public Bitmap Create()
        {
            Bitmap bitmap = CreateBitmap();
            SetBackground(bitmap);
            OverlayBitmap(bitmap);

            return bitmap;
        }

        private Bitmap CreateBitmap()
        {
            int width = realSenseConfiguration.ColorImage.Resolution.Width;
            int height = realSenseConfiguration.ColorImage.Resolution.Height;
            return new Bitmap(width, height, PixelFormat.Format32bppRgb);
        }

        private void SetBackground(Bitmap bitmap)
        {
            if (imageConfiguration.BackgroundImage == HandsImageBackground.ColorImage)
            {
                CopyImageDataToBitmap(bitmap, imageData.ColorImage);
            }
        }

        private void CopyImageDataToBitmap(Bitmap bitmap, PXCMImage sourceImage)
        {
            PXCMImage.ImageData sourceImageData;
            pxcmStatus status = sourceImage.AcquireAccess(PXCMImage.Access.ACCESS_READ,
                PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out sourceImageData);

            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseException("Error retrieving colorImage data");
            }

            int width = sourceImage.info.width;
            int height = sourceImage.info.height;
            byte[] pixels = sourceImageData.ToByteArray(0, sourceImageData.pitches[0]*height);

            sourceImage.ReleaseAccess(sourceImageData);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);

            Marshal.Copy(pixels, 0, data.Scan0, width*height*4);
            bitmap.UnlockBits(data);
        }

        private void OverlayBitmap(Bitmap bitmap)
        {
            if (imageConfiguration.Overlays.Contains(HandsImageOverlay.HandsSegmentationImage))
            {
                OverlayBitmapWithHandsSegmentationImages(bitmap);
            }
            if (imageConfiguration.Overlays.Contains(HandsImageOverlay.HandJoints))
            {
                OverlayBitmapWithHandJoints(bitmap);
            }
        }

        private void OverlayBitmapWithHandsSegmentationImages(Bitmap bitmap)
        {
            foreach (var hand in handsData.Hands)
            {
                OverlayBitmapWithHandsSegmentationImage(bitmap, hand.SegmentationImage, (byte) hand.BodySide);
            }
        }

        private unsafe void OverlayBitmapWithHandsSegmentationImage(Bitmap bitmap, PXCMImage segmentationImage, byte userId)
        {
            PXCMImage.ImageData data;
            var status = segmentationImage.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_Y8, out data);

            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new RealSenseException("Error retrieving image data");
            }

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var destinationPointer = (byte*) bitmapData.Scan0;
            var sourcePointer = (byte*) data.planes[0];
            int imagesize = bitmap.Width*bitmap.Height;

            byte tmp;
            for (int i = 0; i < imagesize; i++, destinationPointer += 4, sourcePointer++)
            {
                tmp = (byte) (LUT[sourcePointer[0]]*userId*100);
                destinationPointer[0] = (Byte) (tmp | destinationPointer[0]);
                destinationPointer[1] = (Byte) (tmp | destinationPointer[1]);
                destinationPointer[2] = (Byte) (tmp | destinationPointer[2]);
                destinationPointer[3] = 0xff;
            }

            bitmap.UnlockBits(bitmapData);
            segmentationImage.ReleaseAccess(data);
        }

        private void OverlayBitmapWithHandJoints(Bitmap bitmap)
        {
            foreach (var hand in handsData.Hands)
            {
                AddJointData(bitmap, hand.Joints.Values.ToArray());
            }
        }

        public void AddJointData(Bitmap bitmap, PXCMHandData.JointData[] nodes)
        {
            Graphics graphics = Graphics.FromImage(bitmap);

            DrawLines(graphics, nodes, 0.To(1).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(2.To(5)).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(6.To(9)).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(10.To(13)).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(14.To(17)).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(18.To(21)).ToArray());
        }

        public void DrawLines(Graphics graphics, PXCMHandData.JointData[] nodes, int[] indexes, bool closed = false)
        {
            PXCMPoint3DF32[] detectedPositions = nodes.Select((node, index) => new { Node = node, Index = index })
                .Where(node => node.Node.confidence > CONFIDENCE_THRESHOLD && indexes.Contains(node.Index))
                .Select(node => node.Node.positionImage)
                .ToArray();

            0.To(detectedPositions.Length - (closed ? 1 : 2))
                .ToArray()
                .Select(
                    startIndex => new
                    {
                        Start = detectedPositions[startIndex],
                        End = detectedPositions[(startIndex + 1) % detectedPositions.Length]
                    })
                .Do(line => DrawLine(graphics, line.Start, line.End));
        }

        private void DrawLine(Graphics graphics, PXCMPoint3DF32 start, PXCMPoint3DF32 end)
        {
            graphics.DrawLine(BONE_PEN, new Point((int)start.x, (int)start.y), new Point((int)end.x, (int)end.y));
        }

        public class Builder
        {
            public HandsImageCreator Build(HandsData handsData, ImageData imageData,
                Configuration realSenseConfiguration, HandsImageConfiguration imageConfiguration)
            {
                return new HandsImageCreator(handsData, imageData, realSenseConfiguration, imageConfiguration);
            }
        }
    }
}