using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tobii.Eyetracking.Sdk;

using System.Drawing;
using System.Drawing.Imaging;

namespace SensorLib.Tobii.Calibration
{

    using TobiiCalibration = global::Tobii.Eyetracking.Sdk.Calibration;


    static class CalibrationViewer
    {


        public static Image GetCalibrationImage(TobiiCalibration calibration)
        {

            // Create image to draw to
            Bitmap bitmap = new Bitmap(250, 250, PixelFormat.Format32bppArgb);
            Graphics graphicsDevice = Graphics.FromImage(bitmap);
            graphicsDevice.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;


            foreach(CalibrationPlotItem plotItem in calibration.Plot)
            {
                using(Pen pen = new Pen(Color.DarkGray))
                {

                    // Draw calibration points
                    Rectangle r = GetCalibrationCircleBounds(new PointF(plotItem.TrueX, plotItem.TrueY), circleRadius);
                    graphicsDevice.DrawEllipse(pen,r);
                

                    // Draw bounds
                    pen.Color = Color.LightGray;
                    Rectangle canvasBounds = GetCanvasBounds();
                    graphicsDevice.DrawRectangle(pen,canvasBounds);


                    // Draw errors
                    if (plotItem.ValidityLeft == 1)
                    {
                        pen.Color = Color.Red;

                        Point p1 = PixelPointFromNormalizedPoint(new PointF(plotItem.TrueX, plotItem.TrueY));
                        Point p2 = PixelPointFromNormalizedPoint(new PointF(plotItem.MapLeftX, plotItem.MapLeftY));

                        graphicsDevice.DrawLine(pen, p1, p2);
                    }

                    if (plotItem.ValidityRight == 1)
                    {
                        pen.Color = Color.Lime;

                        Point p1 = PixelPointFromNormalizedPoint(new PointF(plotItem.TrueX, plotItem.TrueY));
                        Point p2 = PixelPointFromNormalizedPoint(new PointF(plotItem.MapRightX, plotItem.MapRightY));

                        graphicsDevice.DrawLine(pen, p1, p2);
                    }
                                            
    
                }
            }


            return bitmap;
        }







        static Rectangle GetCalibrationCircleBounds(PointF center,int radius)
        {
            Point pixelCenter = PixelPointFromNormalizedPoint(center);
            int d = 2*radius;

            return new Rectangle(pixelCenter.X - radius,pixelCenter.Y - radius,d,d);
        }

        static Point PixelPointFromNormalizedPoint(PointF normalizedPoint)
        {
            int xPadding = (int)(paddingRatio * width);
            int yPadding = (int)(paddingRatio * height);

            int canvasWidth = width - 2*xPadding;
            int canvasHeight = height - 2*yPadding;

            Point pixelPoint = new Point();
            pixelPoint.X = xPadding + (int)(normalizedPoint.X * canvasWidth);
            pixelPoint.Y = yPadding + (int)(normalizedPoint.Y * canvasHeight);

            return pixelPoint;
        }

        static Rectangle GetCanvasBounds()
        {
            Point upperLeft = PixelPointFromNormalizedPoint(new PointF(0F, 0F));
            Point lowerRight = PixelPointFromNormalizedPoint(new PointF(1F, 1F));
            
            Rectangle bounds = new Rectangle();

            bounds.Location = upperLeft;
            bounds.Width = lowerRight.X - upperLeft.X;
            bounds.Height = lowerRight.Y - upperLeft.Y;

            return bounds;
        }


        const float paddingRatio = 0.07F;
        const int circleRadius = 5;
        const int width = 250;
        const int height = 250;

    }

    
}
