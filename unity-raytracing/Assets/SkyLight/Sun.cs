using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunSimulation
{
    public struct Location
    {
        public float latitude;
        public float longitude;
        public int timeZone;
    }

    public struct Time
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
    }

    public static class SunPosition
    {
        private const double Deg2Rad = Math.PI / 180.0;
        private const double Rad2Deg = 180.0 / Math.PI;

        public static void CalculateSunPosition(Location location, Time time, out double outAzimuth, out double outAltitude)
        {
            // Convert latitude and longitude to radians
            double rlat = location.latitude * Deg2Rad;
            double rlon = location.longitude * Deg2Rad;

            // Decimal hour of the day at Greenwich
            double greenwichtime = time.hour - location.timeZone + time.minute / 60.0;

            // Number of days from J2000.0.  
            double daynum = 367 * time.year - (int)((7.0  * (time.year +(int)((time.month + 9.0) / 12.0))) / 4.0) +
                (int)((275.0 * time.month) / 9.0) + time.day - 730531.5 + greenwichtime / 24;

            // Solar Coordinates  
            // Mean longitude of the sun
            double mean_long = daynum * 0.01720279239 + 4.894967873;
            // Mean anomaly of the sun
            double mean_anom = daynum * 0.01720197034 + 6.240040768;
            // Ecliptic longitude of the sun 
            double eclip_long =  mean_long + 0.03342305518 * Math.Sin(mean_anom) + 0.0003490658504 * Math.Sin(2 * mean_anom);

            // Obliquity of the ecliptic
            double obliquity = 0.4090877234 - 0.000000006981317008 * daynum;
            // Right ascension of the sun
            double rasc = Math.Atan2(Math.Cos(obliquity) * Math.Sin(eclip_long), Math.Cos(eclip_long));
            // Declination of the sun
            double decl = Math.Asin(Math.Sin(obliquity) * Math.Sin(eclip_long));
            // Local sidereal time
            double sidereal = 4.894961213 + 6.300388099 * daynum + rlon;
            // Hour angle of the sun
            double hour_ang = sidereal - rasc;

            // Local elevation of the sun
            double elevation = Math.Asin(Math.Sin(decl) * Math.Sin(rlat) +
                Math.Cos(decl) * Math.Cos(rlat) * Math.Cos(hour_ang));
            // Local azimuth of the sun
            double azimuth = Math.Atan2(
                -Math.Cos(decl) * Math.Cos(rlat) * Math.Sin(hour_ang),
                Math.Sin(decl) - Math.Sin(rlat) * Math.Sin(elevation));
            azimuth = azimuth * Rad2Deg;
            double shiftedx = azimuth - 0;
            double delta = 360;
            azimuth = (((shiftedx % delta) + delta) % delta);
            elevation = elevation * Rad2Deg;
            shiftedx = elevation + 180;
            elevation = (((shiftedx % delta) + delta) % delta) - 180;

            // # Refraction correction 
            double targ = ((elevation + (10.3 / (elevation + 5.11)))) * Deg2Rad;
            elevation += (1.02 / Math.Tan(targ)) / 60;

            //# Return azimuth and elevation in degrees
            azimuth = Math.Round(azimuth, 2);
            elevation = Math.Round(elevation, 2);

            outAltitude = elevation;
            outAzimuth = azimuth;
        }
    }
}