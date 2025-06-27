# Get GPS from images
Scans through a file or directory and outputs a CSV containing the file and GPS location for each.

Run the application with the path as an argument, or enter it when prompted.

Drag and drop a file or folder onto the exe to supply the path automatically.

The CSV file is saved to C:\tmp\

Make sure this folder exists before running the scan.

## Example output

File;LatRef;LatDeg;LatMin;LatSec;LonRef;LonDeg;LonMin;LonSec;Altitude;latDecimal;lonDecimal
c:\tmp\1.jpg;N;50;7;48;E;11;13;32;7;50.13;10.2256;https://www.google.com/maps/place/50.13,11.2256

The CSV Delimiter is semicolon to avoid comma issues in coordinates and links

## Information in the CSV output
 -File path
- GPS Latitude North or South, degrees, minutes, seconds
- GPS Longitude East or West, degrees, minutes, seconds
- Altitude above sea level
- Latitude, Longitude in decimal format for use in Google maps
- Google maps URL to the GPS location

If there's no GPS info in the file EXIF, LatRef and LonRef are set to X, and all numbers are 0
