namespace DocumentLab.ImageProcessor.Utility
{
  using Extensions;
  using ImageMagick;
  using System.Drawing;
  public static class BitmapUtils
  {
    public static Bitmap Resample(Bitmap image, int dpix, int dpiy)
    {
      using (var resampled = new MagickImage(image))
      {
        resampled.Resample(dpix, dpiy);
        return resampled.ToBitmap();
      }
    }
  }
}
