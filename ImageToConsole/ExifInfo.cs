namespace ImageToConsole
{
    public class ExifInfo
    {
        // https://exiftool.org/TagNames/EXIF.html
        // https://nicholasarmstrong.com/2010/02/exif-quick-reference/
        // https://exiftool.org/TagNames/EXIF.html
        // https://exiftool.org/TagNames/GPS.html

        private readonly Dictionary<int, string> PropertyNames = [];
        private readonly Dictionary<int, string> PropertyTypeDescription = [];

        public ExifInfo()
        {
            FillDictionary();
        }

        public static (double deg, double min, double sec) ParseGPSbytes(byte[] bytes)
        {
            // converted from https://stackoverflow.com/questions/45136895/extracting-gps-numerical-values-from-byte-array-using-powershell
            if (bytes.Length < 20) return (0, 0, 0);
            double Degrees = BitConverter.ToInt32(bytes, 0) / BitConverter.ToInt32(bytes, 4);
            double Minutes = BitConverter.ToInt32(bytes, 8) / BitConverter.ToInt32(bytes, 12);
            double Seconds = BitConverter.ToInt32(bytes, 16) / BitConverter.ToInt32(bytes, 20);
            return (Degrees, Minutes, Seconds);
        }

        public string GetPropertyName(int id)
        {
            if (PropertyNames.TryGetValue(id, out string? value))
            {
                return value;
            }
            else return "";
        }

        public string GetPropertyTypeDescription(int id)
        {
            if (PropertyTypeDescription.TryGetValue(id, out string? value))
            {
                return value;
            }
            else return "";
        }

        private void FillDictionary()
        {
            PropertyNames.Add(0x0000, "PropertyTagGpsVer");
            PropertyNames.Add(0x0001, "PropertyTagGpsLatitudeRef");
            PropertyNames.Add(0x0002, "PropertyTagGpsLatitude");
            PropertyNames.Add(0x0003, "PropertyTagGpsLongitudeRef");
            PropertyNames.Add(0x0004, "PropertyTagGpsLongitude");
            PropertyNames.Add(0x0005, "PropertyTagGpsAltitudeRef");
            PropertyNames.Add(0x0006, "PropertyTagGpsAltitude");
            PropertyNames.Add(0x0007, "PropertyTagGpsGpsTime");
            PropertyNames.Add(0x0008, "PropertyTagGpsGpsSatellites");
            PropertyNames.Add(0x0009, "PropertyTagGpsGpsStatus");
            PropertyNames.Add(0x000A, "PropertyTagGpsGpsMeasureMode");
            PropertyNames.Add(0x000B, "PropertyTagGpsGpsDop");
            PropertyNames.Add(0x000C, "PropertyTagGpsSpeedRef");
            PropertyNames.Add(0x000D, "PropertyTagGpsSpeed");
            PropertyNames.Add(0x000E, "PropertyTagGpsTrackRef");
            PropertyNames.Add(0x000F, "PropertyTagGpsTrack");
            PropertyNames.Add(0x0010, "PropertyTagGpsImgDirRef");
            PropertyNames.Add(0x0011, "PropertyTagGpsImgDir");
            PropertyNames.Add(0x0012, "PropertyTagGpsMapDatum");
            PropertyNames.Add(0x0013, "PropertyTagGpsDestLatRef");
            PropertyNames.Add(0x0014, "PropertyTagGpsDestLat");
            PropertyNames.Add(0x0015, "PropertyTagGpsDestLongRef");
            PropertyNames.Add(0x0016, "PropertyTagGpsDestLong");
            PropertyNames.Add(0x0017, "PropertyTagGpsDestBearRef");
            PropertyNames.Add(0x0018, "PropertyTagGpsDestBear");
            PropertyNames.Add(0x0019, "PropertyTagGpsDestDistRef");
            PropertyNames.Add(0x001A, "PropertyTagGpsDestDist");
            PropertyNames.Add(0x00FE, "PropertyTagNewSubfileType");
            PropertyNames.Add(0x00FF, "PropertyTagSubfileType");
            PropertyNames.Add(0x0100, "PropertyTagImageWidth");
            PropertyNames.Add(0x0101, "PropertyTagImageHeight");
            PropertyNames.Add(0x0102, "PropertyTagBitsPerSample");
            PropertyNames.Add(0x0103, "PropertyTagCompression");
            PropertyNames.Add(0x0106, "PropertyTagPhotometricInterp");
            PropertyNames.Add(0x0107, "PropertyTagThreshHolding");
            PropertyNames.Add(0x0108, "PropertyTagCellWidth");
            PropertyNames.Add(0x0109, "PropertyTagCellHeight");
            PropertyNames.Add(0x010A, "PropertyTagFillOrder");
            PropertyNames.Add(0x010D, "PropertyTagDocumentName");
            PropertyNames.Add(0x010E, "PropertyTagImageDescription");
            PropertyNames.Add(0x010F, "PropertyTagEquipMake");
            PropertyNames.Add(0x0110, "PropertyTagEquipModel");
            PropertyNames.Add(0x0111, "PropertyTagStripOffsets");
            PropertyNames.Add(0x0112, "PropertyTagOrientation");
            PropertyNames.Add(0x0115, "PropertyTagSamplesPerPixel");
            PropertyNames.Add(0x0116, "PropertyTagRowsPerStrip");
            PropertyNames.Add(0x0117, "PropertyTagStripBytesCount");
            PropertyNames.Add(0x0118, "PropertyTagMinSampleValue");
            PropertyNames.Add(0x0119, "PropertyTagMaxSampleValue");
            PropertyNames.Add(0x011A, "PropertyTagXResolution");
            PropertyNames.Add(0x011B, "PropertyTagYResolution");
            PropertyNames.Add(0x011C, "PropertyTagPlanarConfig");
            PropertyNames.Add(0x011D, "PropertyTagPageName");
            PropertyNames.Add(0x011E, "PropertyTagXPosition");
            PropertyNames.Add(0x011F, "PropertyTagYPosition");
            PropertyNames.Add(0x0120, "PropertyTagFreeOffset");
            PropertyNames.Add(0x0121, "PropertyTagFreeByteCounts");
            PropertyNames.Add(0x0122, "PropertyTagGrayResponseUnit");
            PropertyNames.Add(0x0123, "PropertyTagGrayResponseCurve");
            PropertyNames.Add(0x0124, "PropertyTagT4Option");
            PropertyNames.Add(0x0125, "PropertyTagT6Option");
            PropertyNames.Add(0x0128, "PropertyTagResolutionUnit");
            PropertyNames.Add(0x0129, "PropertyTagPageNumber");
            PropertyNames.Add(0x012D, "PropertyTagTransferFunction");
            PropertyNames.Add(0x0131, "PropertyTagSoftwareUsed");
            PropertyNames.Add(0x0132, "PropertyTagDateTime");
            PropertyNames.Add(0x013B, "PropertyTagArtist");
            PropertyNames.Add(0x013C, "PropertyTagHostComputer");
            PropertyNames.Add(0x013D, "PropertyTagPredictor");
            PropertyNames.Add(0x013E, "PropertyTagWhitePoint");
            PropertyNames.Add(0x013F, "PropertyTagPrimaryChromaticities");
            PropertyNames.Add(0x0140, "PropertyTagColorMap");
            PropertyNames.Add(0x0141, "PropertyTagHalftoneHints");
            PropertyNames.Add(0x0142, "PropertyTagTileWidth");
            PropertyNames.Add(0x0143, "PropertyTagTileLength");
            PropertyNames.Add(0x0144, "PropertyTagTileOffset");
            PropertyNames.Add(0x0145, "PropertyTagTileByteCounts");
            PropertyNames.Add(0x014C, "PropertyTagInkSet");
            PropertyNames.Add(0x014D, "PropertyTagInkNames");
            PropertyNames.Add(0x014E, "PropertyTagNumberOfInks");
            PropertyNames.Add(0x0150, "PropertyTagDotRange");
            PropertyNames.Add(0x0151, "PropertyTagTargetPrinter");
            PropertyNames.Add(0x0152, "PropertyTagExtraSamples");
            PropertyNames.Add(0x0153, "PropertyTagSampleFormat");
            PropertyNames.Add(0x0154, "PropertyTagSMinSampleValue");
            PropertyNames.Add(0x0155, "PropertyTagSMaxSampleValue");
            PropertyNames.Add(0x0156, "PropertyTagTransferRange");
            PropertyNames.Add(0x0200, "PropertyTagJPEGProc");
            PropertyNames.Add(0x0201, "PropertyTagJPEGInterFormat");
            PropertyNames.Add(0x0202, "PropertyTagJPEGInterLength");
            PropertyNames.Add(0x0203, "PropertyTagJPEGRestartInterval");
            PropertyNames.Add(0x0205, "PropertyTagJPEGLosslessPredictors");
            PropertyNames.Add(0x0206, "PropertyTagJPEGPointTransforms");
            PropertyNames.Add(0x0207, "PropertyTagJPEGQTables");
            PropertyNames.Add(0x0208, "PropertyTagJPEGDCTables");
            PropertyNames.Add(0x0209, "PropertyTagJPEGACTables");
            PropertyNames.Add(0x0211, "PropertyTagYCbCrCoefficients");
            PropertyNames.Add(0x0212, "PropertyTagYCbCrSubsampling");
            PropertyNames.Add(0x0213, "PropertyTagYCbCrPositioning");
            PropertyNames.Add(0x0214, "PropertyTagREFBlackWhite");
            PropertyNames.Add(0x0301, "PropertyTagGamma");
            PropertyNames.Add(0x0302, "PropertyTagICCProfileDescriptor");
            PropertyNames.Add(0x0303, "PropertyTagSRGBRenderingIntent");
            PropertyNames.Add(0x0320, "PropertyTagImageTitle");
            PropertyNames.Add(0x5001, "PropertyTagResolutionXUnit");
            PropertyNames.Add(0x5002, "PropertyTagResolutionYUnit");
            PropertyNames.Add(0x5003, "PropertyTagResolutionXLengthUnit");
            PropertyNames.Add(0x5004, "PropertyTagResolutionYLengthUnit");
            PropertyNames.Add(0x5005, "PropertyTagPrintFlags");
            PropertyNames.Add(0x5006, "PropertyTagPrintFlagsVersion");
            PropertyNames.Add(0x5007, "PropertyTagPrintFlagsCrop");
            PropertyNames.Add(0x5008, "PropertyTagPrintFlagsBleedWidth");
            PropertyNames.Add(0x5009, "PropertyTagPrintFlagsBleedWidthScale");
            PropertyNames.Add(0x500A, "PropertyTagHalftoneLPI");
            PropertyNames.Add(0x500B, "PropertyTagHalftoneLPIUnit");
            PropertyNames.Add(0x500C, "PropertyTagHalftoneDegree");
            PropertyNames.Add(0x500D, "PropertyTagHalftoneShape");
            PropertyNames.Add(0x500E, "PropertyTagHalftoneMisc");
            PropertyNames.Add(0x500F, "PropertyTagHalftoneScreen");
            PropertyNames.Add(0x5010, "PropertyTagJPEGQuality");
            PropertyNames.Add(0x5011, "PropertyTagGridSize");
            PropertyNames.Add(0x5012, "PropertyTagThumbnailFormat");
            PropertyNames.Add(0x5013, "PropertyTagThumbnailWidth");
            PropertyNames.Add(0x5014, "PropertyTagThumbnailHeight");
            PropertyNames.Add(0x5015, "PropertyTagThumbnailColorDepth");
            PropertyNames.Add(0x5016, "PropertyTagThumbnailPlanes");
            PropertyNames.Add(0x5017, "PropertyTagThumbnailRawBytes");
            PropertyNames.Add(0x5018, "PropertyTagThumbnailSize");
            PropertyNames.Add(0x5019, "PropertyTagThumbnailCompressedSize");
            PropertyNames.Add(0x501A, "PropertyTagColorTransferFunction");
            PropertyNames.Add(0x501B, "PropertyTagThumbnailData");
            PropertyNames.Add(0x5020, "PropertyTagThumbnailImageWidth");
            PropertyNames.Add(0x5021, "PropertyTagThumbnailImageHeight");
            PropertyNames.Add(0x5022, "PropertyTagThumbnailBitsPerSample");
            PropertyNames.Add(0x5023, "PropertyTagThumbnailCompression");
            PropertyNames.Add(0x5024, "PropertyTagThumbnailPhotometricInterp");
            PropertyNames.Add(0x5025, "PropertyTagThumbnailImageDescription");
            PropertyNames.Add(0x5026, "PropertyTagThumbnailEquipMake");
            PropertyNames.Add(0x5027, "PropertyTagThumbnailEquipModel");
            PropertyNames.Add(0x5028, "PropertyTagThumbnailStripOffsets");
            PropertyNames.Add(0x5029, "PropertyTagThumbnailOrientation");
            PropertyNames.Add(0x502A, "PropertyTagThumbnailSamplesPerPixel");
            PropertyNames.Add(0x502B, "PropertyTagThumbnailRowsPerStrip");
            PropertyNames.Add(0x502C, "PropertyTagThumbnailStripBytesCount");
            PropertyNames.Add(0x502D, "PropertyTagThumbnailResolutionX");
            PropertyNames.Add(0x502E, "PropertyTagThumbnailResolutionY");
            PropertyNames.Add(0x502F, "PropertyTagThumbnailPlanarConfig");
            PropertyNames.Add(0x5030, "PropertyTagThumbnailResolutionUnit");
            PropertyNames.Add(0x5031, "PropertyTagThumbnailTransferFunction");
            PropertyNames.Add(0x5032, "PropertyTagThumbnailSoftwareUsed");
            PropertyNames.Add(0x5033, "PropertyTagThumbnailDateTime");
            PropertyNames.Add(0x5034, "PropertyTagThumbnailArtist");
            PropertyNames.Add(0x5035, "PropertyTagThumbnailWhitePoint");
            PropertyNames.Add(0x5036, "PropertyTagThumbnailPrimaryChromaticities");
            PropertyNames.Add(0x5037, "PropertyTagThumbnailYCbCrCoefficients");
            PropertyNames.Add(0x5038, "PropertyTagThumbnailYCbCrSubsampling");
            PropertyNames.Add(0x5039, "PropertyTagThumbnailYCbCrPositioning");
            PropertyNames.Add(0x503A, "PropertyTagThumbnailRefBlackWhite");
            PropertyNames.Add(0x503B, "PropertyTagThumbnailCopyRight");
            PropertyNames.Add(0x5090, "PropertyTagLuminanceTable");
            PropertyNames.Add(0x5091, "PropertyTagChrominanceTable");
            PropertyNames.Add(0x5100, "PropertyTagFrameDelay");
            PropertyNames.Add(0x5101, "PropertyTagLoopCount");
            PropertyNames.Add(0x5102, "PropertyTagGlobalPalette");
            PropertyNames.Add(0x5103, "PropertyTagIndexBackground");
            PropertyNames.Add(0x5104, "PropertyTagIndexTransparent");
            PropertyNames.Add(0x5110, "PropertyTagPixelUnit");
            PropertyNames.Add(0x5111, "PropertyTagPixelPerUnitX");
            PropertyNames.Add(0x5112, "PropertyTagPixelPerUnitY");
            PropertyNames.Add(0x5113, "PropertyTagPaletteHistogram");
            PropertyNames.Add(0x8298, "PropertyTagCopyright");
            PropertyNames.Add(0x829A, "PropertyTagExifExposureTime");
            PropertyNames.Add(0x829D, "PropertyTagExifFNumber");
            PropertyNames.Add(0x8769, "PropertyTagExifIFD");
            PropertyNames.Add(0x8773, "PropertyTagICCProfile");
            PropertyNames.Add(0x8822, "PropertyTagExifExposureProg");
            PropertyNames.Add(0x8824, "PropertyTagExifSpectralSense");
            PropertyNames.Add(0x8825, "PropertyTagGpsIFD");
            PropertyNames.Add(0x8827, "PropertyTagExifISOSpeed");
            PropertyNames.Add(0x8828, "PropertyTagExifOECF");
            PropertyNames.Add(0x9000, "PropertyTagExifVer");
            PropertyNames.Add(0x9003, "PropertyTagExifDTOrig");
            PropertyNames.Add(0x9004, "PropertyTagExifDTDigitized");
            PropertyNames.Add(0x9101, "PropertyTagExifCompConfig");
            PropertyNames.Add(0x9102, "PropertyTagExifCompBPP");
            PropertyNames.Add(0x9201, "PropertyTagExifShutterSpeed");
            PropertyNames.Add(0x9202, "PropertyTagExifAperture");
            PropertyNames.Add(0x9203, "PropertyTagExifBrightness");
            PropertyNames.Add(0x9204, "PropertyTagExifExposureBias");
            PropertyNames.Add(0x9205, "PropertyTagExifMaxAperture");
            PropertyNames.Add(0x9206, "PropertyTagExifSubjectDist");
            PropertyNames.Add(0x9207, "PropertyTagExifMeteringMode");
            PropertyNames.Add(0x9208, "PropertyTagExifLightSource");
            PropertyNames.Add(0x9209, "PropertyTagExifFlash");
            PropertyNames.Add(0x920A, "PropertyTagExifFocalLength");
            PropertyNames.Add(0x927C, "PropertyTagExifMakerNote");
            PropertyNames.Add(0x9286, "PropertyTagExifUserComment");
            PropertyNames.Add(0x9290, "PropertyTagExifDTSubsec");
            PropertyNames.Add(0x9291, "PropertyTagExifDTOrigSS");
            PropertyNames.Add(0x9292, "PropertyTagExifDTDigSS");
            PropertyNames.Add(0xA000, "PropertyTagExifFPXVer");
            PropertyNames.Add(0xA001, "PropertyTagExifColorSpace");
            PropertyNames.Add(0xA002, "PropertyTagExifPixXDim");
            PropertyNames.Add(0xA003, "PropertyTagExifPixYDim");
            PropertyNames.Add(0xA004, "PropertyTagExifRelatedWav");
            PropertyNames.Add(0xA005, "PropertyTagExifInterop");
            PropertyNames.Add(0xA20B, "PropertyTagExifFlashEnergy");
            PropertyNames.Add(0xA20C, "PropertyTagExifSpatialFR");
            PropertyNames.Add(0xA20E, "PropertyTagExifFocalXRes");
            PropertyNames.Add(0xA20F, "PropertyTagExifFocalYRes");
            PropertyNames.Add(0xA210, "PropertyTagExifFocalResUnit");
            PropertyNames.Add(0xA214, "PropertyTagExifSubjectLoc");
            PropertyNames.Add(0xA215, "PropertyTagExifExposureIndex");
            PropertyNames.Add(0xA217, "PropertyTagExifSensingMethod");
            PropertyNames.Add(0xA300, "PropertyTagExifFileSource");
            PropertyNames.Add(0xA301, "PropertyTagExifSceneType");
            PropertyNames.Add(0xA302, "PropertyTagExifCfaPattern");

            PropertyTypeDescription.Add(1, "byte array");
            PropertyTypeDescription.Add(2, "ASCII, null terminated");
            PropertyTypeDescription.Add(3, "ushort array");
            PropertyTypeDescription.Add(4, "ulong array");
            PropertyTypeDescription.Add(5, "ulong pairs array");
            PropertyTypeDescription.Add(6, "byte array, any data");
            PropertyTypeDescription.Add(7, "long array");
            PropertyTypeDescription.Add(10, "long pairs array");



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
        }
    }
}
