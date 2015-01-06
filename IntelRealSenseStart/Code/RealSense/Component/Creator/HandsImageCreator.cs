using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using IntelRealSenseStart.Code.RealSense.Config.Image;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Data.Determiner;
using IntelRealSenseStart.Code.RealSense.Exception;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Creator
{
    public class HandsImageCreator : ImageCreator
    {
        private const int CONFIDENCE_THRESHOLD = 80;
        private static readonly byte[] LUT;

        private readonly RealSenseConfiguration realSenseConfiguration;

        static HandsImageCreator()
        {
            LUT = Enumerable.Repeat((byte) 0, 256).ToArray();
            LUT[255] = 1;
        }

        private HandsImageCreator(RealSenseConfiguration realSenseConfiguration)
        {
            this.realSenseConfiguration = realSenseConfiguration;
        }

        public Bitmap Create(Bitmap bitmap, DeterminerData determinerData,
            ImageCreatorConfiguration imageCreatorConfiguration)
        {
            return new HandsImageCreatorRun(bitmap, determinerData, realSenseConfiguration, imageCreatorConfiguration)
                .Create();
        }

        public class HandsImageCreatorRun
        {
            private readonly Pen bonePen;

            private readonly Bitmap bitmap;
            private readonly DeterminerData determinerData;

            private readonly RealSenseConfiguration realSenseConfiguration;
            private readonly ImageCreatorConfiguration imageCreatorConfiguration;

            private PXCMProjection projection;
            private PXCMPointF32[] uvMap;

            public HandsImageCreatorRun(Bitmap bitmap, DeterminerData determinerData,
                RealSenseConfiguration realSenseConfiguration, ImageCreatorConfiguration imageCreatorConfiguration)
            {
                this.bitmap = bitmap;
                this.determinerData = determinerData;

                this.realSenseConfiguration = realSenseConfiguration;
                this.imageCreatorConfiguration = imageCreatorConfiguration;

                bonePen = new Pen(Color.SlateBlue, 3.0f*bitmap.Width/imageCreatorConfiguration.Resolution.Width);
            }

            public Bitmap Create()
            {
                CreateProjection();
                OverlayBitmap();
                CleanUp();

                return bitmap;
            }

            private void CreateProjection()
            {
                projection = determinerData.Device.CreateProjection();
                uvMap =
                    new PXCMPointF32[
                        determinerData.ImageData.DepthImage.info.width*determinerData.ImageData.DepthImage.info.height];
                projection.QueryUVMap(determinerData.ImageData.DepthImage, uvMap);
            }

            private void OverlayBitmap()
            {
                if (imageCreatorConfiguration.Overlays.Contains(ImageOverlay.DepthCoordinateHandsSegmentationImage))
                {
                    OverlayBitmapWithHandsSegmentationImages();
                }
                if (imageCreatorConfiguration.Overlays.Contains(ImageOverlay.DepthCoordinateHandJoints))
                {
                    OverlayBitmapWithHandJoints(projectColorToDepthCoordinates: false);
                }
                if (imageCreatorConfiguration.Overlays.Contains(ImageOverlay.ColorCoordinateHandJoints))
                {
                    OverlayBitmapWithHandJoints(projectColorToDepthCoordinates: true);
                }
            }

            private void OverlayBitmapWithHandsSegmentationImages()
            {
                foreach (HandData hand in determinerData.HandsData.Hands)
                {
                    OverlayBitmapWithHandsSegmentationImage(hand.SegmentationImage, (byte) hand.BodySide);
                }
            }

            private unsafe void OverlayBitmapWithHandsSegmentationImage(PXCMImage segmentationImage, byte userId)
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

            private void OverlayBitmapWithHandJoints(bool projectColorToDepthCoordinates)
            {
                foreach (HandData hand in determinerData.HandsData.Hands)
                {
                    AddJointData(projectColorToDepthCoordinates, hand.Joints.Values.ToArray());
                }
            }

            public void AddJointData(bool projectColorToDepthCoordinates, PXCMHandData.JointData[] nodes)
            {
                var graphics = Graphics.FromImage(bitmap);

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
                if (imageCreatorConfiguration.BackgroundImage == ImageBackground.ColorImage)
                {
                    Size backgroundImageResolution = bitmap.Size;
                    return new PXCMPoint3DF32
                    {
                        x =
                            uvMap[
                                (int) depthPosition.y*determinerData.ImageData.DepthImage.info.width +
                                (int) depthPosition.x
                                ].x*backgroundImageResolution.Width,
                        y =
                            uvMap[
                                (int) depthPosition.y*determinerData.ImageData.DepthImage.info.width +
                                (int) depthPosition.x
                                ].y*backgroundImageResolution.Height
                    };
                }

                return depthPosition;
            }

            private PXCMPoint3DF32 MapDepthPositionToBackgroundImage(PXCMPoint3DF32 depthPosition)
            {
                Size backgroundImageResolution = bitmap.Size;
                var depthResolution = realSenseConfiguration.DepthImage.Resolution;

                if (depthResolution.Equals(backgroundImageResolution))
                {
                    return depthPosition;
                }

                return new PXCMPoint3DF32
                {
                    x = (depthPosition.x/depthResolution.Width)*backgroundImageResolution.Width,
                    y = (depthPosition.y/depthResolution.Height)*backgroundImageResolution.Height
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
        }

        public class Builder
        {
            private RealSenseConfiguration realSenseConfiguration;

            public Builder WithRealSenseConfiguration(RealSenseConfiguration realSenseConfiguration)
            {
                this.realSenseConfiguration = realSenseConfiguration;
                return this;
            }

            public HandsImageCreator Build()
            {
                return new HandsImageCreator(realSenseConfiguration);
            }
        }
    }
}