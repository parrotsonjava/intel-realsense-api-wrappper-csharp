using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace IntelRealSenseStart.Code
{
    internal class HandBitmapBuilder
    {
        private const int CONFIDENCE_THRESHOLD = 80;
        private static readonly Pen BONE_PEN = new Pen(Color.SlateBlue, 3.0f);

        private static readonly byte[] LUT;
        private Bitmap bitmap;

        static HandBitmapBuilder()
        {
            LUT = Enumerable.Repeat((byte) 0, 256).ToArray();
            LUT[255] = 1;
        }

        public static HandBitmapBuilder Create()
        {
            return new HandBitmapBuilder();
        }


        public HandBitmapBuilder AddRGBImage(PXCMImage image)
        {
            if (image != null)
            {
                Bitmap bitmap = QueryForBitmap(image.info.width, image.info.height);
                CopyImageDataToBitmap(bitmap, image);
            }
            return this;
        }

        public HandBitmapBuilder AddSegmentationImage(PXCMImage segmentationImage, byte userId)
        {
            Bitmap bitmap = QueryForBitmap(segmentationImage.info.width, segmentationImage.info.height);
            AddSegmentationInfoTo(bitmap, segmentationImage, userId);

            return this;
        }

        private Bitmap QueryForBitmap(int width, int height)
        {
            if (bitmap == null)
            {
                bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                return bitmap;
            }
            if (bitmap.Width != width || bitmap.Height != height)
            {
                throw new Exception("Wrong image dimensions");
            }

            return bitmap;
        }

        private void AddSegmentationInfoTo(Bitmap bitmap, PXCMImage segmentationImage, byte userId)
        {
            PXCMImage.ImageData data;
            pxcmStatus status = segmentationImage.AcquireAccess(PXCMImage.Access.ACCESS_READ,
                PXCMImage.PixelFormat.PIXEL_FORMAT_Y8, out data);

            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new Exception("Error acquiring the image data");
            }

            CopySegmentationImageData(bitmap, segmentationImage, userId, data);
        }


        private void CopyImageDataToBitmap(Bitmap bitmap, PXCMImage sourceImage)
        {
            PXCMImage.ImageData imageData;
            pxcmStatus status = sourceImage.AcquireAccess(PXCMImage.Access.ACCESS_READ,
                PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out imageData);

            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                throw new Exception("Error retrieving image data");
            }

            int width = sourceImage.info.width;
            int height = sourceImage.info.height;
            byte[] pixels = imageData.ToByteArray(0, imageData.pitches[0]*height);

            sourceImage.ReleaseAccess(imageData);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);

            Marshal.Copy(pixels, 0, data.Scan0, width*height*4);
            bitmap.UnlockBits(data);
        }

        private unsafe void CopySegmentationImageData(Bitmap bitmap, PXCMImage segmentationImage, byte userId,
            PXCMImage.ImageData data)
        {
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

        public HandBitmapBuilder AddJointData(PXCMHandData.JointData[] nodes)
        {
            Graphics graphics = Graphics.FromImage(bitmap);

            DrawLines(graphics, nodes, 0.To(1).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(2.To(5)).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(6.To(9)).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(10.To(13)).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(14.To(17)).ToArray());
            DrawLines(graphics, nodes, 0.AsRange().Add(18.To(21)).ToArray());

            return this;
        }

        public void DrawLines(Graphics graphics, PXCMHandData.JointData[] nodes, int[] indexes, bool closed = false)
        {
            PXCMPoint3DF32[] detectedPositions = nodes.Select((node, index) => new {Node = node, Index = index})
                .Where(node => node.Node.confidence > CONFIDENCE_THRESHOLD && indexes.Contains(node.Index))
                .Select(node => node.Node.positionImage)
                .ToArray();

            0.To(detectedPositions.Length - (closed ? 1 : 2))
                .ToArray()
                .Select(
                    startIndex => new
                    {
                        Start = detectedPositions[startIndex],
                        End = detectedPositions[(startIndex + 1)%detectedPositions.Length]
                    })
                .Do(line => DrawLine(graphics, line.Start, line.End));
        }

        private void DrawLine(Graphics graphics, PXCMPoint3DF32 start, PXCMPoint3DF32 end)
        {
            graphics.DrawLine(BONE_PEN, new Point((int) start.x, (int) start.y), new Point((int) end.x, (int) end.y));
            ;
        }

        public Bitmap Build()
        {
            return bitmap;
        }
    }
}