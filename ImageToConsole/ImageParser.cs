using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using System.Text;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1416
#pragma warning restore IDE0079 // Remove unnecessary suppression

namespace ImageToConsole

{
    public class ImageParser
    {
        [SupportedOSPlatform("windows")]

        //[DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow", SetLastError = true)]
        //private static extern IntPtr GetConsoleHandle();
        //private IntPtr handler;
        private readonly Bitmap? bmp = null;

        public ImageParser(Image image)
        {
            bmp = (Bitmap)image;
        }

        public ImageParser(string filePath)
        {
            if (filePath == null)
            {

                Debug.WriteLine("WriteImageToConsole error: image path is null");
                return;
            }

            filePath = Path.GetFullPath(filePath);

            if (File.Exists(filePath) == false)
            {
                Debug.WriteLine($"WriteImageToConsole error: file does not exist: {filePath}");
                return;
            }
            bmp = (Bitmap)Image.FromFile(filePath);
        }

        //public void DrawImageToConsole(int startx, int starty, int width, int height, bool scaleToFit)
        //{
        //    if (bmp is null) return;
        //    using (var graphics = Graphics.FromHwnd(handler))
        //    {
        //        graphics.DrawImage(bmp, new PointF(100, 100));
        //        Pen pen = new Pen(Color.Red, 5f);
        //        graphics.DrawEllipse(pen, new RectangleF(200, 200, 100,100));
        //    }
        //    Console.WriteLine($"Drew image {bmp.Width}x{bmp.Height}");
        //    Console.ReadKey();
        //}

        public void WriteImageToConsole(int startx, int starty, int width, int height, bool scaleToFit)
        {
            if (bmp == null)
            {
                Debug.WriteLine($"WriteImageToConsole error: bitmap is null");
                return;
            }

            int finalWidth = bmp.Width;
            int finalHeight = bmp.Height;


            if (scaleToFit)
            {
                float ratioX = (float)width / bmp.Width;
                float ratioY = (float)height / bmp.Height;
                float ratioFinal = Math.Min(ratioX, ratioY);
                finalWidth = (int)Math.Min((bmp.Width * ratioFinal * 2), width);
                finalHeight = (int)(bmp.Height * ratioFinal);
                Debug.WriteLine($"Img {bmp.Width}x{bmp.Height}, frame {width}x{height} Ratios: x {ratioX} y {ratioY}, using {ratioFinal}");
                Debug.WriteLine($"final {finalWidth} {finalHeight}");
            }

            using Bitmap bitmap = new(bmp, finalWidth, finalHeight);
            for (int y = 0; y < height - 1 && y < bitmap.Height; y++)
            {
                Console.SetCursorPosition(startx, starty + y);
                for (int x = 0; x < width - 1 && x < bitmap.Width; x++)
                {
                    Console.Write(ColorChar('█', bitmap.GetPixel(x, y)));
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        public static string ColorChar(char symbol, Color color)
        {
            return $"\x1B[38;2;{color.R};{color.G};{color.B}m{symbol}";
        }



        public static Dictionary<int, byte[]> ReadExifDataToDictionary(string path)
        {
            //Debug.WriteLine($"Read exif, file: {path}");
            Dictionary<int, byte[]> ExifProperties = [];
            //ExifInfo exifInfo = new();
            PropertyItem[] propItems;
            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                // IDs: https://learn.microsoft.com/en-us/dotnet/api/system.drawing.imaging.propertyitem.id?view=windowsdesktop-9.0

                Image img = Image.FromStream(stream, false, false);
                propItems = img.PropertyItems;
            }
            foreach (PropertyItem item in propItems)
            {
                if (item.Value != null)
                    ExifProperties.Add(item.Id, item.Value);
            }
            return ExifProperties;
        }

        public static GPSData? GetGPSData(string path)
        {
            var props = ReadExifDataToDictionary(path);
            props.TryGetValue(0x1, out byte[] latRef);
            props.TryGetValue(0x2, out byte[] latBytes);
            props.TryGetValue(0x3, out byte[] lonRef);
            props.TryGetValue(0x4, out byte[] lonBytes);
            props.TryGetValue(0x5, out byte[] altRef);
            props.TryGetValue(0x6, out byte[] altBytes);

            if (latRef == null || latBytes == null || lonRef == null || lonBytes == null || altRef == null || altBytes == null)
            {
                return null;
            }

            ASCIIEncoding encoding = new();
            char latR = encoding.GetString(latRef)[0];
            char lonR = encoding.GetString(lonRef)[0];

            GPSData gps = new(latR, latBytes, lonR, lonBytes, altRef[0], altBytes, path);
            return gps;
        }

        public static void PrintGPSData(string path)
        {
            GPSData? gps = GetGPSData(path);
            if (gps == null)
            {
                Console.WriteLine("No GPS data");
            }
            else
            {
                Console.WriteLine($"Lat: {gps.LatitudeRef} {gps.LatitudeDegrees} {gps.LatitudeMinutes} {gps.LatitudeSeconds}");
                Console.WriteLine($"Lon: {gps.LongitudeRef} {gps.LongitudeDegrees} {gps.LongitudeMinutes} {gps.LongitudeSeconds}");
                Console.WriteLine($"Alt: {Math.Round(gps.Altitude, 1)}m {gps.RelativeToSeaLevel}");
            }
        }

        public static string GPSDataToCsv(List<GPSData> gpsList)
        {
            StringBuilder sb = new();
            sb.AppendLine("File;LatRef;LatDeg;LatMin;LatSec;LonRef;LonDeg;LonMin;LonSec;Altitude;latDecimal;lonDecimal");
            
            foreach (GPSData gps in gpsList)
            {
                sb.Append($"{gps.FilePath.Replace(',', '-')};");
                sb.Append($"{gps.LatitudeRef};{gps.LatitudeDegrees};{gps.LatitudeMinutes};{gps.LatitudeSeconds};");
                sb.Append($"{gps.LongitudeRef};{gps.LongitudeDegrees};{gps.LongitudeMinutes};{gps.LongitudeSeconds};");
                sb.Append($"{Math.Floor(gps.Altitude)};");

                double latDecimal = gps.LatitudeDegrees + (gps.LatitudeMinutes / 60) + (gps.LatitudeSeconds / 3600);
                double lonDecimal = gps.LongitudeDegrees + (gps.LongitudeMinutes / 60) + (gps.LongitudeSeconds / 3600);
                string latText = Math.Round(latDecimal, 4).ToString().Replace(',', '.');
                string lonText = Math.Round(lonDecimal, 4).ToString().Replace(',', '.');

                sb.Append($"{latText};{lonText};");
                sb.Append($"https://www.google.com/maps/place/{latText},{lonText}");


                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static void ReadImageExifData(string path)
        {
            ExifInfo exifInfo = new();
            PropertyItem[] propItems;
            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                // IDs: https://learn.microsoft.com/en-us/dotnet/api/system.drawing.imaging.propertyitem.id?view=windowsdesktop-9.0

                Image img = Image.FromStream(stream, false, false);
                propItems = img.PropertyItems;
            }
            if (propItems is null) return;

            int count = 0;
            Console.WriteLine("--- Image properties ---");
            foreach (PropertyItem propItem in propItems)
            {
                if (propItem.Value is null) continue;

                string propertyName = exifInfo.GetPropertyName(propItem.Id).Replace("PropertyTag", "");
                string propertyType = exifInfo.GetPropertyTypeDescription(propItem.Type);

                //if (propItem.Type != 2 && propertyName == "")
                //{
                //    Debug.WriteLine($"Skipping property 0x{propItem.Id:x} Type: {propertyType}, Length: {propItem.Len}");
                //    continue;
                //}

                Console.WriteLine("-----------------");
                Console.WriteLine($"0x{propItem.Id:x} Type: {propItem.Type}, {propertyType}, Length: {propItem.Len}");
                Console.WriteLine(propertyName);


                /*
                    1	is an array of bytes.
                    2	is a null-terminated ASCII string. If you set the type data member to ASCII type, you should set the Len property to the length of the string including the null terminator. For example, the string "Hello" would have a length of 6.
                    3	is an array of unsigned short (16-bit) integers.
                    4	is an array of unsigned long (32-bit) integers.
                    5	data member is an array of pairs of unsigned long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
                    6	is an array of bytes that can hold values of any data type.
                    7	is an array of signed long (32-bit) integers.
                    10	is an array of pairs of signed long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator. 
                 */
                
                if (propItem.Type == 2)
                {
                    System.Text.ASCIIEncoding encoding = new();
                    string text = encoding.GetString(propItem.Value);
                    Console.WriteLine(text.Trim());
                }
                else if (propItem.Type == 5 && (propItem.Id == 0x2 || propItem.Id == 0x4))
                {
                    (double deg, double min, double sec) = ExifInfo.ParseGPSbytes(propItem.Value);
                    Console.WriteLine($"{deg}° {min}m {sec}s");
                }
                else if (propItem.Len < 500)
                {
                    if (propItem.Type == 1 || propItem.Type == 6)
                    {
                        // 1	is an array of bytes.
                        // 6	is an array of bytes that can hold values of any data type.
                        Console.Write("Bytes: ");
                        foreach (byte b in propItem.Value)
                        {
                            Console.Write(b + " ");
                        }
                        Console.WriteLine();
                    }
                    else if (propItem.Type == 3 || propItem.Type == 4)
                    {
                        // 3 is an array of unsigned short (16-bit) integers.
                        // 4 is an array of unsigned long(32 - bit) integers.
                        Console.Write("Bytes: ");
                        foreach (object o in propItem.Value)
                        {
                            Console.Write(o + " ");
                        }
                        
                        Console.WriteLine();
                        Console.WriteLine($"length {propItem.Value.Length}");
                        if (propItem.Value.Length == 2)
                        {
                            Console.Write("Integer: ");
                            Console.WriteLine(BitConverter.ToUInt16(propItem.Value, 0));
                        }
                        else if (propItem.Value.Length == 4)
                        {
                            Console.Write("Integer: ");
                            Console.WriteLine(BitConverter.ToUInt32(propItem.Value, 0));
                        }
                        else if (propItem.Value.Length == 8)
                        {
                            Console.Write("Integer: ");
                            Console.WriteLine(BitConverter.ToInt64(propItem.Value, 0));
                        }
                    }
                    else if (propItem.Type == 5 || propItem.Type == 10)
                    {
                        Console.Write("Bytes: ");
                        foreach (object o in propItem.Value)
                        {
                            Console.Write(o + " ");
                        }
                        Console.WriteLine();
                        // 5 data member is an array of pairs of unsigned long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
                        // 10 is an array of pairs of signed long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator. 
                        if (propItem.Len == 8)
                        {
                            byte[] bytes = propItem.Value;
                            double Numerator = BitConverter.ToInt32(bytes, 0);
                            double Denominator = BitConverter.ToInt32(bytes, 4);
                            //double Seconds = BitConverter.ToInt32(bytes, 16) / BitConverter.ToInt32(bytes, 20);
                            Console.WriteLine($"  {Numerator}/{Denominator}");
                            if (Denominator != 0)
                            {
                                Console.WriteLine($"= {Numerator / Denominator}");
                            }
                        }
                        Console.WriteLine();
                    }
                    else if (propItem.Type == 7)
                    {
                        // 7 is an array of signed long (32-bit) integers.
                        
                        Console.Write("Bytes: ");
                        foreach (object o in propItem.Value)
                        {
                            Console.Write(o + " ");
                        }
                        Console.WriteLine();
                        if (propItem.Value.Length == 4)
                        {
                            Console.Write("Integer: ");
                            Console.WriteLine(BitConverter.ToInt32(propItem.Value, 0));
                        }
                        if (propItem.Value.Length == 8)
                        {
                            Console.Write("Integer: ");
                            Console.WriteLine(BitConverter.ToInt64(propItem.Value, 0));
                        }
                        Console.WriteLine();
                    }
                    //else if (propItem.Type == 10)
                    //{
                    //    // 10	is an array of pairs of signed long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator. 

                    //}
                }

                count++;
            }

        }
    }
}
