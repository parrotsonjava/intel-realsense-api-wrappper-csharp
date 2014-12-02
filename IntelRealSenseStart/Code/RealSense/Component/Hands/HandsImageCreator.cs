using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IntelRealSenseStart.Code.RealSense.Config;
using IntelRealSenseStart.Code.RealSense.Config.HandsImage;
using IntelRealSenseStart.Code.RealSense.Data;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Hands
{
    public class HandsImageCreator
    {
        private const int CONFIDENCE_THRESHOLD = 80;
        private static readonly byte[] LUT;

        private Pen bonePen;

        private readonly PXCMCapture.Device device;
        private readonly HandsData handsData;
        private readonly HandsImageConfiguration imageConfiguration;
        private readonly ImageData imageData;

        private readonly Configuration realSenseConfiguration;

        private PXCMProjection projection;
        private PXCMPointF32[] uvMap;

        static HandsImageCreator()
        {


            LUT = Enumerable.Repeat((byte) 0, 256).ToArray();
            LUT[255] = 1;
        }

        private HandsImageCreator(PXCMCapture.Device device, HandsData handsData, ImageData imageData,
            Configuration realSenseConfiguration, HandsImageConfiguration imageConfiguration)
        {
            this.device = device;
            this.handsData = handsData;
            this.imageData = imageData;

            this.realSenseConfiguration = realSenseConfiguration;
            this.imageConfiguration = imageConfiguration;

            Configure();

        }

        private void Configure()
        {
            bonePen = new Pen(Color.SlateBlue, 3.0f * BackgroundImageResolution.Width / imageConfiguration.Resolution.Width);
        }

        public Bitmap Create()
        {
            Bitmap bitmap = CreateBitmap();
            CreateProjection();
            SetBackground(bitmap);
            OverlayBitmap(bitmap);

            CleanUp();

            return (Bitmap) bitmap.ResizeImage(imageConfiguration.Resolution);
        }

        private Bitmap CreateBitmap()
        {
            var backgroundImageSize = BackgroundImageResolution;
            return new Bitmap(backgroundImageSize.Width, backgroundImageSize.Height, PixelFormat.Format32bppRgb);
        }

        private void CreateProjection()
        {
            projection = device.CreateProjection();
            uvMap = new PXCMPointF32[imageData.DepthImage.info.width * imageData.DepthImage.info.height];
            projection.QueryUVMap(imageData.DepthImage, uvMap);
        }

        private void SetBackground(Bitmap bitmap)
        {
            if (imageConfiguration.BackgroundImage == HandsImageBackground.ColorImage)
            {
                CopyImageDataToBitmap(bitmap, imageData.ColorImage);
            }
            else if (imageConfiguration.BackgroundImage == HandsImageBackground.DepthImage)
            {
                CopyImageDataToBitmap(bitmap, imageData.DepthImage);
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
            if (imageConfiguration.Overlays.Contains(HandsImageOverlay.DepthCoordinateHandsSegmentationImage))
            {
                OverlayBitmapWithHandsSegmentationImages(bitmap);
            }
            if (imageConfiguration.Overlays.Contains(HandsImageOverlay.DepthCoordinateHandJoints))
            {
                OverlayBitmapWithHandJoints(bitmap, projectColorToDepthCoordinates: false);
            }
            if (imageConfiguration.Overlays.Contains(HandsImageOverlay.ColorCoordinateHandJoints))
            {
                OverlayBitmapWithHandJoints(bitmap, projectColorToDepthCoordinates: true);
            }
        }

        private void OverlayBitmapWithHandsSegmentationImages(Bitmap bitmap)
        {
            foreach (HandData hand in handsData.Hands)
            {
                OverlayBitmapWithHandsSegmentationImage(bitmap, hand.SegmentationImage, (byte) hand.BodySide);
            }
        }

        private unsafe void OverlayBitmapWithHandsSegmentationImage(Bitmap bitmap, PXCMImage segmentationImage,
            byte userId)
        {
            PXCMImage.ImageData data;
            pxcmStatus status = segmentationImage.AcquireAccess(PXCMImage.Access.ACCESS_READ,
                PXCMImage.PixelFormat.PIXEL_FORMAT_Y8, out data);

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

        private void OverlayBitmapWithHandJoints(Bitmap bitmap, bool projectColorToDepthCoordinates)
        {
            foreach (HandData hand in handsData.Hands)
            {
                AddJointData(bitmap, projectColorToDepthCoordinates, hand.Joints.Values.ToArray());
            }
        }

        public void AddJointData(Bitmap bitmap, bool projectColorToDepthCoordinates, PXCMHandData.JointData[] nodes)
        {
            Graphics graphics = Graphics.FromImage(bitmap);

            DrawLines(graphics, nodes, projectColorToDepthCoordinates, 0.To(1).ToArray());
            DrawLines(graphics, nodes, projectColorToDepthCoordinates, 0.AsRange().Add(2.To(5)).ToArray());
            DrawLines(graphics, nodes, projectColorToDepthCoordinates, 0.AsRange().Add(6.To(9)).ToArray());
            DrawLines(graphics, nodes, projectColorToDepthCoordinates, 0.AsRange().Add(10.To(13)).ToArray());
            DrawLines(graphics, nodes, projectColorToDepthCoordinates, 0.AsRange().Add(14.To(17)).ToArray());
            DrawLines(graphics, nodes, projectColorToDepthCoordinates, 0.AsRange().Add(18.To(21)).ToArray());
        }

        public void DrawLines(Graphics graphics, PXCMHandData.JointData[] nodes, bool projectColorToDepthCoordinates,
            int[] indexes, bool closed = false)
        {
            var detectedPositions = nodes.Select((node, index) => new {Node = node, Index = index})
                .Where(node => node.Node.confidence > CONFIDENCE_THRESHOLD && indexes.Contains(node.Index))
                .Select(node =>
                    projectColorToDepthCoordinates
                        ? MapDepthPositionToColorPositionInBackgroundImage(node.Node.positionImage)
                        : MapDepthPositionToBackgroundImage(node.Node.positionImage))
                .Where(image => image.x > 0 && image.y > 0)
                .ToArray();

            0.To(detectedPositions.Length - (closed ? 1 : 2))
                .ToArray()
                .Select(startIndex =>
                    new
                    {
                        Start = detectedPositions[startIndex],
                        End = detectedPositions[(startIndex + 1)%detectedPositions.Length]
                    })
                .Do(line => DrawLine(graphics, line.Start, line.End));
        }

        private PXCMPoint3DF32 MapDepthPositionToColorPositionInBackgroundImage(PXCMPoint3DF32 depthPosition)
        {
            if (imageConfiguration.BackgroundImage == HandsImageBackground.ColorImage)
            {
                return new PXCMPoint3DF32
                {
                    x = uvMap[(int) depthPosition.y*imageData.DepthImage.info.width + (int) depthPosition.x].x*
                        BackgroundImageResolution.Width,
                    y = uvMap[(int)depthPosition.y * imageData.DepthImage.info.width + (int)depthPosition.x].y *
                        BackgroundImageResolution.Height
                };
            }

            return depthPosition;
        }

        private PXCMPoint3DF32 MapDepthPositionToBackgroundImage(PXCMPoint3DF32 depthPosition)
        {
            var depthResolution = realSenseConfiguration.DepthImage.Resolution;
            if (depthResolution.Equals(BackgroundImageResolution))
            {
                return depthPosition;
            }

            return new PXCMPoint3DF32
            {
                x = (depthPosition.x / depthResolution.Width) * BackgroundImageResolution.Width,
                y = (depthPosition.y / depthResolution.Height) * BackgroundImageResolution.Height
            };
        }

        private void DrawLine(Graphics graphics, PXCMPoint3DF32 start, PXCMPoint3DF32 end)
        {
            graphics.DrawLine(bonePen, new Point((int) start.x, (int) start.y), new Point((int) end.x, (int) end.y));
        }

        private void CleanUp()
        {
            projection.Dispose();
        }

        private PXCMImage BackgroundImage
        {
            get
            {
                switch (imageConfiguration.BackgroundImage)
                {
                    case HandsImageBackground.ColorImage:
                        return imageData.ColorImage;
                    case HandsImageBackground.DepthImage:
                        return imageData.DepthImage;
                    default:
                        return null;
                }
            }
        }

        private Size BackgroundImageResolution
        {
            get
            {
                var backgroundImage = BackgroundImage;
                return backgroundImage != null
                    ? new Size(backgroundImage.info.width, backgroundImage.info.height)
                    : imageConfiguration.Resolution;
            }
        }

        public class Builder
        {
            public HandsImageCreator Build(PXCMCapture.Device device, HandsData handsData, ImageData imageData,
                Configuration realSenseConfiguration, HandsImageConfiguration imageConfiguration)
            {
                return new HandsImageCreator(device, handsData, imageData, realSenseConfiguration, imageConfiguration);
            }
        }
    }
}