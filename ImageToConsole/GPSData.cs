using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ImageToConsole
{
    public class GPSData
    {
        public char LatitudeRef;
        public char LongitudeRef;
        public uint AltitudeRef;
        public AltitudeType RelativeToSeaLevel;

        public double LatitudeDegrees;
        public double LatitudeMinutes;
        public double LatitudeSeconds;

        public double LongitudeDegrees;
        public double LongitudeMinutes;
        public double LongitudeSeconds;

        public double Altitude;

        public string FilePath = "";

        public GPSData(string path)
        {
            FilePath = path;
            LatitudeRef = 'X';
            LongitudeRef = 'X';
            AltitudeRef = 0;
            RelativeToSeaLevel = AltitudeType.AboveSeaLevel;

            LatitudeDegrees = 0;
            LatitudeMinutes = 0;
            LatitudeSeconds = 0;

            LongitudeDegrees = 0;
            LongitudeMinutes = 0;
            LongitudeSeconds = 0;
        }

        public GPSData(char latRef, byte[] latBytes, char lonRef, byte[] lonBytes, uint altRef, byte[] altBytes, string path = "")
        {
            LatitudeRef = latRef;
            LongitudeRef = lonRef;
            (LatitudeDegrees, LatitudeMinutes, LatitudeSeconds) = ExifInfo.ParseGPSbytes(latBytes);
            (LongitudeDegrees, LongitudeMinutes, LongitudeSeconds) = ExifInfo.ParseGPSbytes(lonBytes);
            AltitudeRef = altRef;
            FilePath = path;

            if (altBytes.Length == 8)
            {
                double Numerator = BitConverter.ToInt32(altBytes, 0);
                double Denominator = BitConverter.ToInt32(altBytes, 4);
                if (Denominator != 0)
                {
                    Altitude = Numerator / Denominator;
                }
            }

            FilePath = path;
        }

        //0 = Above Sea Level
        //1 = Below Sea Level
        //2 = Positive Sea Level(sea-level ref)
        //3 = Negative Sea Level(sea-level ref)



        public enum AltitudeType
        {
            AboveSeaLevel,
            BelowSeaLevel,
            PositiveSeaLevel,
            NegativeSeaLevel
        }
    }
}
