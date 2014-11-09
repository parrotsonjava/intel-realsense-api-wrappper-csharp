using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace IntelRealSenseStart.Code
{
    internal class HandBitmapBuilder
    {
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
        public HandBitmapBuilder addSegmentationImage(PXCMImage segmentationImage, byte userId)
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

        private static unsafe void CopySegmentationImageData(Bitmap bitmap, PXCMImage segmentationImage, byte userId,
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

        public Bitmap Build()
        {
            return bitmap;
        }
    }
}