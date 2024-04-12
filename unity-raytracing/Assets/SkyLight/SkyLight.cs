using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SunSimulation
{

    [ExecuteInEditMode]

    public class SkyColor : MonoBehaviour
    {
        Material material;
        Light light;

        [Serializable]
        public class SkyColorSettings
        {
            // SkyColor Settings ---------------------------------------
            //sunSize = 0.04f;
            //sunSizeConvergence = 5.0f;
            //atmosphereThickness = 1.0f;
            //skyTint = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            //groundColor = new Color(0.369f, 0.349f, 0.341f, 1.0f);
            //exposure = 1.3f;
            // ---------------------------------------------------------
            [Range(0.0f, 1.0f)]
            public float sunSize;

            [Range(0.0f, 10.0f)]
            public float sunSizeConvergence;

            [Range(0.0f, 5.0f)]
            public float atmosphereThickness;

            public Color skyTint;

            public Color groundColor;

            [Range(0.0f, 8.0f)]
            public float exposure;

            [Range(1.0f, 10.0f)]
            public float power;
        }
        
        [Serializable]
        public class SunRotationSettings
        {
            // location
            [Range(-89.99f, 89.99f)]
            public float latitude;
            [Range(-180.0F, 180.0F)]
            public float longitude;
            [Range(-11, +12)]
            public int timeZone;

            // time
            public int year;
            [Range(1, 12)]
            public int month;
            [Range(1, 31)]
            public int day;
            [Range(0, 24)]
            public int hour;
            [Range(0, 60)]
            public int minute;

            // results
            public float calAzi;
            public float calAlt;
            public float calLightIntensity;
        }

        public SkyColorSettings skyColor = new SkyColorSettings();
        public SunRotationSettings sunRotation = new SunRotationSettings();
    

        // Start is called before the first frame update
        void Start()
        {
            material = RenderSettings.skybox;
            light = RenderSettings.sun;
        }

        // Update is called once per frame
        void Update()
        {
            material.SetFloat("_SunSize", skyColor.sunSize);
            material.SetFloat("_SunSizeConvergence", skyColor.sunSizeConvergence);
            material.SetFloat("_AtmosphereThickness", skyColor.atmosphereThickness);
            material.SetVector("_SkyTint", skyColor.skyTint);
            material.SetVector("_GroundColor", skyColor.groundColor);
            material.SetFloat("_Exposure", skyColor.exposure);
            material.SetFloat("_Power", skyColor.power);

            SetPosition();
        }

        Vector3 SphericalToDirection(float theta, float phi)
        {
            float sintheta = Mathf.Sin(theta * Mathf.Deg2Rad);
            float costheta = Mathf.Cos(theta * Mathf.Deg2Rad);
            float sinphi = Mathf.Sin(phi * Mathf.Deg2Rad);
            float cosphi = Mathf.Cos(phi * Mathf.Deg2Rad);

            Vector3 eyeRay = new Vector3();
            eyeRay.x = sintheta * cosphi;
            eyeRay.y = costheta;
            eyeRay.z = sintheta * sinphi;

            return eyeRay;
        }

        void SetPosition()
        {
            Location location = new Location();
            location.latitude = sunRotation.latitude;
            location.longitude = sunRotation.longitude;
            location.timeZone = sunRotation.timeZone;

            Time time = new Time();
            time.year = sunRotation.year;
            time.month = sunRotation.month;
            time.day = sunRotation.day;
            time.hour = sunRotation.hour;
            time.minute = sunRotation.minute;


            double azimuth;    // 方位角 :  0北, 90東, 180南, 270西
            double altitude;   // 仰角   :  0水平, 望天90, 望地 -90
            SunPosition.CalculateSunPosition(location, time, out azimuth, out altitude);

            // spherical coordinate
            double phi = 90.0 - azimuth;    //
            double theta = 90.0 - altitude; //
            Debug.LogFormat("{0} {1} {2} {3}", azimuth, altitude, phi, theta);

            // Unity Coordinate
            Vector3 sunDir = SphericalToDirection((float)theta, (float)phi);

            light.transform.localRotation = Quaternion.FromToRotation(Vector3.back, sunDir);
            // light.intensity = Mathf.InverseLerp(-12, 0, (float)altitude);
            Debug.Log(light.transform.position);

            sunRotation.calAzi = (float)azimuth;
            sunRotation.calAlt = (float)altitude;            
            sunRotation.calLightIntensity = light.intensity;
        }
    }
}
