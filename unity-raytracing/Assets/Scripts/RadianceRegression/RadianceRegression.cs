using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PBRT
{
    public abstract class DataType
    {
        public DataType()
        {
        }

        public override string ToString()
        {
            return "";
        }
    }

    // basic Data Type
    public class Point2 : DataType
    {
        float x = 0.0f;
        float y = 0.0f;

        public Point2(float x, float y)
            : base()
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Point2 other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as Point2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static bool operator ==(Point2 left, Point2 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point2 left, Point2 right)
        {
            return !Equals(left, right);
        }
        public override string ToString()
        {
            return string.Format("{0:0.0000} {1:0.0000}", x, y);
        }
    }

    public class Point3 : DataType
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        public Point3(float x, float y, float z)
            : base()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals(Point3 other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as Point3);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static bool operator ==(Point3 left, Point3 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point3 left, Point3 right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("{0:0.0000} {1:0.0000} {2:0.0000}", x, y, z);
        }
    }

    public class Vector2 : DataType
    {
        float x = 0.0f;
        float y = 0.0f;

        public Vector2(float x, float y)
            : base()
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Vector2 other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as Vector2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !Equals(left, right);
        }
        public override string ToString()
        {
            return string.Format("{0:0.0000} {1:0.0000}", x, y);
        }
    }

    public class Vector3 : DataType
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        public Vector3(float x, float y, float z)
            : base()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals(Vector3 other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as Vector3);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !Equals(left, right);
        }
        public override string ToString()
        {
            return string.Format("{0:0.0000} {1:0.0000} {2:0.0000}", x, y, z);
        }
    }

    public class Normal3 : DataType
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        public Normal3(float x, float y, float z)
            : base()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals(Normal3 other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as Normal3);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static bool operator ==(Normal3 left, Normal3 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Normal3 left, Normal3 right)
        {
            return !Equals(left, right);
        }
        public override string ToString()
        {
            return string.Format("{0:0.0000} {1:0.0000} {2:0.0000}", x, y, z);
        }
    }

    public class RGBSpectrum : DataType
    {
        float r = 0.0f;
        float g = 0.0f;
        float b = 0.0f;

        public RGBSpectrum(float r, float g, float b)
            : base()
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public bool Equals(RGBSpectrum other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return r == other.r && g == other.g && b == other.b;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as RGBSpectrum);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(r, g, b);
        }

        public static bool operator ==(RGBSpectrum left, RGBSpectrum right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RGBSpectrum left, RGBSpectrum right)
        {
            return !Equals(left, right);
        }
        public override string ToString()
        {
            return string.Format("{0:0.0000} {1:0.0000} {2:0.0000}", r, g, b);
        }
    }

    public class LerpSpectrum : DataType
    {
        List<float> lamdas = new List<float>();
        List<float> values = new List<float>();

        public LerpSpectrum(float lamda0, float value0, float lamda1, float value1)
            : base()
        {
            this.lamdas.Add(lamda0);
            this.lamdas.Add(lamda1);
            this.values.Add(value0);
            this.values.Add(value1);
        }

        public LerpSpectrum(List<float> lamdas, List<float> values)
        {
            this.lamdas = lamdas;
            this.values = values;
        }

        public bool Equals(LerpSpectrum other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return lamdas.SequenceEqual(other.lamdas) && values.SequenceEqual(other.values);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as LerpSpectrum);
        }

        public override int GetHashCode()
        {
            return (lamdas, values).GetHashCode();
        }

        public static bool operator ==(LerpSpectrum left, LerpSpectrum right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LerpSpectrum left, LerpSpectrum right)
        {
            return !Equals(left, right);
        }
        public override string ToString()
        {
            string str = "";

            Debug.Assert(lamdas.Count == values.Count);
            for (int i = 0; i < values.Count; i++)
            {
                str += string.Format("{0:0.0000} {1:0.0000}", lamdas[i], values[i]);
                if (i != values.Count - 1)
                    str += " ";
            }

            return str;
        }
    }

    public class BlackbodySpectrum : DataType
    {
        float k = 0.0f;

        public BlackbodySpectrum(float k)
           : base()
        {
            this.k = k;
        }

        public bool Equals(BlackbodySpectrum other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return k == other.k;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as BlackbodySpectrum);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(k);
        }

        public static bool operator ==(BlackbodySpectrum left, BlackbodySpectrum right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BlackbodySpectrum left, BlackbodySpectrum right)
        {
            return !Equals(left, right);
        }
        public override string ToString()
        {
            string str = "";

            str += string.Format("{0:0.0000}", k);

            return str;
        }
    }

    public class FileSpectrum : DataType
    {
        string filename;

        public FileSpectrum(string filename)
            : base()
        {
            this.filename = filename;
        }

        public bool Equals(FileSpectrum other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return filename.Equals(other.filename);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as FileSpectrum);
        }

        public override int GetHashCode()
        {
            return filename.GetHashCode();
        }

        public static bool operator ==(FileSpectrum left, FileSpectrum right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileSpectrum left, FileSpectrum right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            string str = "";

            str += string.Format("{0}", filename);

            return str;
        }
    }

    // Parameter Type
    public class Parameter<T>
    {
        static Dictionary<System.Type, string> typeNames = new Dictionary<System.Type, string>()
        {
            { typeof(int), "integer" },
            { typeof(float), "float" },
            { typeof(PBRT.Point2), "point2" },
            { typeof(PBRT.Point3), "point3" },
            { typeof(PBRT.Vector2), "vector2" },
            { typeof(PBRT.Vector3), "vector3" },
            { typeof(PBRT.Normal3), "normal" },
            { typeof(PBRT.LerpSpectrum), "spectrum" },
            { typeof(PBRT.FileSpectrum), "spectrum" },
            { typeof(PBRT.RGBSpectrum), "rgb" },
            { typeof(PBRT.BlackbodySpectrum), "blackbody" },
            { typeof(PBRT.Texture), "texture" },
            { typeof(bool), "bool" },
            { typeof(string), "string" }
        };

        private List<T> values = new List<T>();

        private List<T> defaults = new List<T>();

        private bool outputDefault;

        public Parameter(T def, bool outputDefault = false)
        {
            this.values.Add(def);

            this.defaults.Add(def);

            this.outputDefault = outputDefault;
        }

        public Parameter(T value, T def, bool outputDefault = false)
        {
            this.values.Add(value);

            this.defaults.Add(def);

            this.outputDefault = outputDefault;
        }

        public Parameter(List<T> defs, bool outputDefault = false)
        {
            this.values = defs.GetRange(0, defs.Count);

            this.defaults = defs.GetRange(0, defs.Count);

            this.outputDefault = outputDefault;
        }

        public Parameter(List<T> values, List<T> defs, bool outputDefault = false)
        {
            this.values = values.GetRange(0, values.Count);

            this.defaults = defs.GetRange(0, defs.Count);

            this.outputDefault = outputDefault;
        }

        public void ResetValue()
        {
            this.values = defaults.GetRange(0, defaults.Count);
        }

        public void SetValue(T value)
        {
            values[0] = value;
        }

        public void SetValues(List<T> values)
        {
            this.values = values.GetRange(0, values.Count);
        }

        public T GetValue()
        {
            return values[0];
        }

        public List<T> GetValues()
        {
            return values;
        }

        public T GetDefault()
        {
            return defaults[0];
        }

        public List<T> GetDefaults()
        {
            return defaults;
        }

        public string GetTypeName()
        {
            return typeNames[typeof(T)];
        }

        public new System.Type GetType()
        {
            return typeof(T);
        }

        public bool EqualDefault()
        {
            return values.SequenceEqual(defaults);
        }

        public bool Equals(Parameter<T> other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return values.SequenceEqual(other.values);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as Parameter<T>);
        }

        public override int GetHashCode()
        {
            return (values, defaults).GetHashCode();
        }

        public static bool operator ==(Parameter<T> left, Parameter<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Parameter<T> left, Parameter<T> right)
        {
            return !Equals(left, right);
        }

        public List<T> ToList()
        {
            List<T> values = new List<T>();

            foreach (var v in values)
                values.Add(v);

            return values;
        }

        public string ToString(string name)
        {
            string str = "";

            if (outputDefault || !this.EqualDefault())
            {
                str += string.Format("    \"{0} {1}\"", typeNames[typeof(T)], name);

                str += string.Format(" [ ");
                for (int i = 0; i <values.Count; i++)
                {
                    var value = values[i];

                    if (typeof(T) == typeof(int))
                    {
                        str += string.Format("{0}", value);
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        str += string.Format("{0:0.0000}", value);
                    }
                    else if (typeof(T) == typeof(bool))
                    {
                        str += string.Format("{0}", value.ToString().ToLower());
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        str += string.Format("\"{0}\"", value);
                    }
                    else if (typeof(T) == typeof(Texture))
                    {
                        str += string.Format("\"{0}\"", value.ToString());
                    }
                    else
                    {
                        str += string.Format("{0}", value.ToString());
                    }

                    if(i < values.Count - 1)
                        str += " ";
                }

                str += string.Format(" ]\n");

            }

            return str;
        }
    };

    public class ParameterList
    {
        Dictionary<string, Parameter<int>> integers = new Dictionary<string, Parameter<int>>();
        Dictionary<string, Parameter<float>> floats = new Dictionary<string, Parameter<float>>();
        Dictionary<string, Parameter<Point2>> point2s = new Dictionary<string, Parameter<Point2>>();
        Dictionary<string, Parameter<Vector2>> vector2s = new Dictionary<string, Parameter<Vector2>>();
        Dictionary<string, Parameter<Point3>> point3s = new Dictionary<string, Parameter<Point3>>();
        Dictionary<string, Parameter<Vector3>> vector3s = new Dictionary<string, Parameter<Vector3>>();
        Dictionary<string, Parameter<Normal3>> normal3s = new Dictionary<string, Parameter<Normal3>>();
        Dictionary<string, Parameter<RGBSpectrum>> rgbSpectrums = new Dictionary<string, Parameter<RGBSpectrum>>();
        Dictionary<string, Parameter<LerpSpectrum>> lerpSpectrums = new Dictionary<string, Parameter<LerpSpectrum>>();
        Dictionary<string, Parameter<FileSpectrum>> fileSpectrums = new Dictionary<string, Parameter<FileSpectrum>>();
        Dictionary<string, Parameter<BlackbodySpectrum>> blackbodySpectrums = new Dictionary<string, Parameter<BlackbodySpectrum>>();
        Dictionary<string, Parameter<Texture>> textures = new Dictionary<string, Parameter<Texture>>();
        Dictionary<string, Parameter<bool>> bools = new Dictionary<string, Parameter<bool>>();
        Dictionary<string, Parameter<string>> strings = new Dictionary<string, Parameter<string>>();

        public ParameterList()
        {
        }

        public void AddParameter(string name, int def, bool outputDefault = false)
        {
            integers.Add(name, new Parameter<int>(def, outputDefault));
        }

        public void AddParameter(string name, float def, bool outputDefault = false)
        {
            floats.Add(name, new Parameter<float>(def, outputDefault));
        }

        public void AddParameter(string name, Point2 def, bool outputDefault = false)
        {
            point2s.Add(name, new Parameter<Point2>(def, outputDefault));
        }

        public void AddParameter(string name, Vector2 def, bool outputDefault = false)
        {
            vector2s.Add(name, new Parameter<Vector2>(def, outputDefault));
        }

        public void AddParameter(string name, Point3 def, bool outputDefault = false)
        {
            point3s.Add(name, new Parameter<Point3>(def, outputDefault));
        }

        public void AddParameter(string name, Vector3 def, bool outputDefault = false)
        {
            vector3s.Add(name, new Parameter<Vector3>(def, outputDefault));
        }

        public void AddParameter(string name, Normal3 def, bool outputDefault = false)
        {
            normal3s.Add(name, new Parameter<Normal3>(def, outputDefault));
        }

        public void AddParameter(string name, LerpSpectrum def, bool outputDefault = false)
        {
            lerpSpectrums.Add(name, new Parameter<LerpSpectrum>(def, outputDefault));
        }

        public void AddParameter(string name, FileSpectrum def, bool outputDefault = false)
        {
            fileSpectrums.Add(name, new Parameter<FileSpectrum>(def, outputDefault));
        }

        public void AddParameter(string name, BlackbodySpectrum def, bool outputDefault = false)
        {
            blackbodySpectrums.Add(name, new Parameter<BlackbodySpectrum>(def, outputDefault));
        }

        public void AddParameter(string name, Texture def, bool outputDefault = false)
        {
            textures.Add(name, new Parameter<Texture>(def, outputDefault));
        }

        public void AddParameter(string name, bool def, bool outputDefault = false)
        {
            bools.Add(name, new Parameter<bool>(def, outputDefault));
        }

        public void AddParameter(string name, string def, bool outputDefault = false)
        {
            strings.Add(name, new Parameter<string>(def, outputDefault));
        }

        public void AddParameter(string name, RGBSpectrum def, bool outputDefault = false)
        {
            rgbSpectrums.Add(name, new Parameter<RGBSpectrum>(def, outputDefault));
        }

        public void SetParameter(string name, int value)
        {
            integers[name].SetValue(value);
        }

        public void SetParameter(string name, float value)
        {
            floats[name].SetValue(value);
        }

        public void SetParameter(string name, Point2 value)
        {
            point2s[name].SetValue(value);
        }

        public void SetParameter(string name, Vector2 value)
        {
            vector2s[name].SetValue(value);
        }

        public void SetParameter(string name, Point3 value)
        {
            point3s[name].SetValue(value);
        }

        public void SetParameter(string name, Vector3 value)
        {
            vector3s[name].SetValue(value);
        }

        public void SetParameter(string name, Normal3 value)
        {
            normal3s[name].SetValue(value);
        }

        public void SetParameter(string name, RGBSpectrum value)
        {
            rgbSpectrums[name].SetValue(value);
        }

        public void SetParameter(string name, LerpSpectrum value)
        {
            lerpSpectrums[name].SetValue(value);
        }

        public void SetParameter(string name, FileSpectrum value)
        {
            fileSpectrums[name].SetValue(value);
        }

        public void SetParameter(string name, BlackbodySpectrum value)
        {
            blackbodySpectrums[name].SetValue(value);
        }

        public void SetParameter(string name, Texture def)
        {
            textures[name].SetValue(def);
        }

        public void SetParameter(string name, bool value)
        {
            bools[name].SetValue(value);
        }

        public void SetParameter(string name, string value)
        {
            strings[name].SetValue(value);
        }

        /// Getter
        public void GetParameter(string name, ref int value)
        {
            value = integers[name].GetValue();
        }

        public void GetParameter(string name, ref float value)
        {
            value = floats[name].GetValue();
        }

        public void GetParameter(string name, ref Point2 value)
        {
            value = point2s[name].GetValue();
        }

        public void GetParameter(string name, ref Vector2 value)
        {
            value = vector2s[name].GetValue();
        }

        public void GetParameter(string name, ref Point3 value)
        {
            value = point3s[name].GetValue();
        }

        public void GetParameter(string name, ref Vector3 value)
        {
            value = vector3s[name].GetValue();
        }

        public void GetParameter(string name, ref Normal3 value)
        {
            value = normal3s[name].GetValue();
        }

        public void GetParameter(string name, ref RGBSpectrum value)
        {
            value = rgbSpectrums[name].GetValue();
        }

        public void GetParameter(string name, ref LerpSpectrum value)
        {
            value = lerpSpectrums[name].GetValue();
        }

        public void GetParameter(string name, ref FileSpectrum value)
        {
            value = fileSpectrums[name].GetValue();
        }

        public void GetParameter(string name, ref BlackbodySpectrum value)
        {
            value = blackbodySpectrums[name].GetValue();
        }

        public void GetParameter(string name, ref Texture value)
        {
            value = textures[name].GetValue();
        }

        public void GetParameter(string name, ref bool value)
        {
            value = bools[name].GetValue();
        }

        public void GetParameter(string name, ref string value)
        {
            value = strings[name].GetValue();
        }

        /// Parameters List
        public void AddParameters(string name, List<int> defs, bool outputDefault = false)
        {
            integers.Add(name, new Parameter<int>(defs, outputDefault));
        }

        public void AddParameters(string name, List<float> defs, bool outputDefault = false)
        {
            floats.Add(name, new Parameter<float>(defs, outputDefault));
        }

        public void AddParameters(string name, List<Point2> defs, bool outputDefault = false)
        {
            point2s.Add(name, new Parameter<Point2>(defs, outputDefault));
        }

        public void AddParameters(string name, List<Vector2> defs, bool outputDefault = false)
        {
            vector2s.Add(name, new Parameter<Vector2>(defs, outputDefault));
        }

        public void AddParameters(string name, List<Point3> defs, bool outputDefault = false)
        {
            point3s.Add(name, new Parameter<Point3>(defs, outputDefault));
        }

        public void AddParameters(string name, List<Vector3> defs, bool outputDefault = false)
        {
            vector3s.Add(name, new Parameter<Vector3>(defs, outputDefault));
        }

        public void AddParameters(string name, List<Normal3> defs, bool outputDefault = false)
        {
            normal3s.Add(name, new Parameter<Normal3>(defs, outputDefault));
        }

        public void AddParameters(string name, List<LerpSpectrum> defs, bool outputDefault = false)
        {
            lerpSpectrums.Add(name, new Parameter<LerpSpectrum>(defs, outputDefault));
        }

        public void AddParameters(string name, List<FileSpectrum> defs, bool outputDefault = false)
        {
            fileSpectrums.Add(name, new Parameter<FileSpectrum>(defs, outputDefault));
        }

        public void AddParameters(string name, List<BlackbodySpectrum> defs, bool outputDefault = false)
        {
            blackbodySpectrums.Add(name, new Parameter<BlackbodySpectrum>(defs, outputDefault));
        }

        public void AddParameters(string name, List<Texture> defs, bool outputDefault = false)
        {
            textures.Add(name, new Parameter<Texture>(defs, outputDefault));
        }

        public void AddParameters(string name, List<bool> defs, bool outputDefault = false)
        {
            bools.Add(name, new Parameter<bool>(defs, outputDefault));
        }

        public void AddParameters(string name, List<string> defs, bool outputDefault = false)
        {
            strings.Add(name, new Parameter<string>(defs, outputDefault));
        }

        public void AddParameters(string name, List<RGBSpectrum> defs, bool outputDefault = false)
        {
            rgbSpectrums.Add(name, new Parameter<RGBSpectrum>(defs, outputDefault));
        }

        /// Setters
        public void SetParameters(string name, List<int> values)
        {
            integers[name].SetValues(values);
        }

        public void SetParameters(string name, List<float> values)
        {
            floats[name].SetValues(values);
        }

        public void SetParameters(string name, List<Point2> values)
        {
            point2s[name].SetValues(values);
        }

        public void SetParameters(string name, List<Vector2> values)
        {
            vector2s[name].SetValues(values);
        }

        public void SetParameters(string name, List<Point3> values)
        {
            point3s[name].SetValues(values);
        }

        public void SetParameters(string name, List<Vector3> values)
        {
            vector3s[name].SetValues(values);
        }

        public void SetParameters(string name, List<Normal3> values)
        {
            normal3s[name].SetValues(values);
        }

        public void SetParameters(string name, List<RGBSpectrum> values)
        {
            rgbSpectrums[name].SetValues(values);
        }

        public void SetParameters(string name, List<LerpSpectrum> values)
        {
            lerpSpectrums[name].SetValues(values);
        }

        public void SetParameters(string name, List<FileSpectrum> values)
        {
            fileSpectrums[name].SetValues(values);
        }

        public void SetParameters(string name, List<BlackbodySpectrum> values)
        {
            blackbodySpectrums[name].SetValues(values);
        }

        public void SetParameters(string name, List<Texture> values)
        {
            textures[name].SetValues(values);
        }

        public void SetParameters(string name, List<bool> values)
        {
            bools[name].SetValues(values);
        }

        public void SetParameters(string name, List<string> values)
        {
            strings[name].SetValues(values);
        }

        public void GetParameters(string name, ref List<int> values)
        {
            values = integers[name].ToList();
        }

        public void GetParameters(string name, ref List<float> values)
        {
            values = floats[name].ToList();
        }

        public void GetParameters(string name, ref List<Point2> values)
        {
            values = point2s[name].ToList();
        }

        public void GetParameters(string name, ref List<Vector2> values)
        {
            values = vector2s[name].ToList();
        }

        public void GetParameters(string name, ref List<Point3> values)
        {
            values = point3s[name].ToList();
        }

        public void GetParameters(string name, ref List<Vector3> values)
        {
            values = vector3s[name].ToList();
        }

        public void GetParameters(string name, ref List<Normal3> values)
        {
            values = normal3s[name].ToList();
        }

        public void GetParameters(string name, ref List<RGBSpectrum> values)
        {
            values = rgbSpectrums[name].ToList();
        }

        public void GetParameters(string name, ref List<LerpSpectrum> values)
        {
            values = lerpSpectrums[name].ToList();
        }

        public void GetParameters(string name, ref List<FileSpectrum> values)
        {
            values = fileSpectrums[name].ToList();
        }

        public void GetParameters(string name, ref List<BlackbodySpectrum> values)
        {
            values = blackbodySpectrums[name].ToList();
        }

        public void GetParameters(string name, ref List<Texture> values)
        {
            values = textures[name].ToList();
        }

        public void GetParameters(string name, ref List<bool> values)
        {
            values = bools[name].ToList();
        }

        public void GetParameters(string name, ref List<string> values)
        {
            values = strings[name].ToList();
        }

        public string GetParametersString()
        {
            string str = "";
            foreach (var v in integers)
                str += v.Value.ToString(v.Key);
            foreach (var v in floats)
                str += v.Value.ToString(v.Key);
            foreach (var v in point2s)
                str += v.Value.ToString(v.Key);
            foreach (var v in vector2s)
                str += v.Value.ToString(v.Key);
            foreach (var v in point3s)
                str += v.Value.ToString(v.Key);
            foreach (var v in vector3s)
                str += v.Value.ToString(v.Key);
            foreach (var v in normal3s)
                str += v.Value.ToString(v.Key);
            foreach (var v in rgbSpectrums)
                str += v.Value.ToString(v.Key);
            foreach (var v in lerpSpectrums)
                str += v.Value.ToString(v.Key);
            foreach (var v in fileSpectrums)
                str += v.Value.ToString(v.Key);
            foreach (var v in blackbodySpectrums)
                str += v.Value.ToString(v.Key);
            foreach (var v in textures)
                str += v.Value.ToString(v.Key);
            foreach (var v in bools)
                str += v.Value.ToString(v.Key);
            foreach (var v in strings)
                str += v.Value.ToString(v.Key);

            return str;
        }
    }

    // Directives
    public abstract class Directive : DataType
    {
        private ParameterList parameterList = new ParameterList();

        public Directive()
            : base()
        {
        }

        public void AddParameter(string name, int def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, float def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, Point2 def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, Vector2 def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, Point3 def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, Vector3 def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, Normal3 def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, RGBSpectrum def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, FileSpectrum def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, BlackbodySpectrum def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, Texture def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, bool def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, string def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void AddParameter(string name, LerpSpectrum def, bool outputDefault = false)
        {
            parameterList.AddParameter(name, def, outputDefault);
        }

        public void SetParameter(string name, int value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, float value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, Point2 value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, Vector2 value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, Point3 value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, Vector3 value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, Normal3 value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, RGBSpectrum value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, LerpSpectrum value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, FileSpectrum value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, BlackbodySpectrum value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, Texture value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, bool value)
        {
            parameterList.SetParameter(name, value);
        }

        public void SetParameter(string name, string value)
        {
            parameterList.SetParameter(name, value);
        }

        /// Getter
        public void GetParameter(string name, ref int value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref float value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref Point2 value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref Vector2 value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref Point3 value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref Vector3 value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref Normal3 value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref RGBSpectrum value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref LerpSpectrum value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref FileSpectrum value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref BlackbodySpectrum value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref Texture value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref bool value)
        {
            parameterList.GetParameter(name, ref value);
        }

        public void GetParameter(string name, ref string value)
        {
            parameterList.GetParameter(name, ref value);
        }

        /// Parameters List
        public void AddParameters(string name, List<int> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<float> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<Point2> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<Vector2> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<Point3> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<Vector3> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<Normal3> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<RGBSpectrum> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<LerpSpectrum> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<FileSpectrum> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<BlackbodySpectrum> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<Texture> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<bool> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        public void AddParameters(string name, List<string> defs)
        {
            parameterList.AddParameters(name, defs);
        }

        /// Setters
        public void SetParameters(string name, List<int> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<float> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<Point2> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<Vector2> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<Point3> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<Vector3> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<Normal3> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<RGBSpectrum> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<LerpSpectrum> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<FileSpectrum> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<BlackbodySpectrum> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<Texture> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<bool> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void SetParameters(string name, List<string> values)
        {
            parameterList.SetParameters(name, values);
        }

        public void GetParameters(string name, ref List<int> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<float> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<Point2> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<Vector2> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<Point3> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<Vector3> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<Normal3> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<RGBSpectrum> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<LerpSpectrum> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<FileSpectrum> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<BlackbodySpectrum> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<Texture> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<bool> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        public void GetParameters(string name, ref List<string> values)
        {
            parameterList.GetParameters(name, ref values);
        }

        protected string GetParametersString()
        {
            return parameterList.GetParametersString();
        }

        public override string ToString()
        {
            return "";
        }

        public abstract string ToDirectiveString();
    }

    public class Accelerator : Directive
    {
        public enum Type
        {
            BVH,
            KDTREE
        };

        public enum SplitMethod
        {
            SAH,
            MIDDLE,
            EQUAL,
            HLBVH
        };

        Type type = Type.BVH;
        SplitMethod splitMethod = SplitMethod.SAH;

        public Accelerator(Type type)
            : base()
        {
            this.type = type;

            if (type == Type.BVH)
            {
                AddParameter("maxnodeprims", 4);

                splitMethod = SplitMethod.SAH;
            }

            else if (type == Type.KDTREE)
            {
                AddParameter("intersectcost", 5);
                AddParameter("traversalcost", 1);
                AddParameter("emptybonus", 0.5f);
                AddParameter("maxprims", 1);
                AddParameter("maxdepth", -1);
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Accelerator \"{0}\"\n", type.ToString().ToLower());
            str += "    " + string.Format("\"string splitMethod\" \"{0}\"\n", splitMethod.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };


    // transformation, material, ReverseOrientation
    public class AttributeBegin : Directive
    {
        public AttributeBegin()
            : base()
        {
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("\n");
            str += string.Format("AttributeBegin\n");
            str += GetParametersString();

            return str;
        }
    }

    public class AttributeEnd : Directive
    {
        public AttributeEnd()
            : base()
        {
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("AttributeEnd\n");
            str += string.Format("\n");
            str += GetParametersString();

            return str;
        }
    }

    public class Attribute : Directive
    {
        public enum Type
        {
            Shape,
            Light,
            Material,
            Medium,
            Texture
        };

        // all
        Type type;

        public Attribute(Type type)
            : base()
        {
            this.type = type;

            if (type == Type.Shape)
            {
            }
            else if (type == Type.Light)
            {
            }
            else if (type == Type.Material)
            {
            }
            else if (type == Type.Medium)
            {
            }
            else if (type == Type.Texture)
            {
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Attribute \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };

    public class AreaLightSource : Directive
    {
        public enum Type
        {
            Diffuse
        };

        // all
        Type type = Type.Diffuse;

        public AreaLightSource(Type type)
            : base()
        {
            this.type = type;

            if (type == Type.Diffuse)
            {
                AddParameter("filename", "");
                AddParameter("l", new LerpSpectrum(300.0f, 1.0f, 800.0f, 1.0f));
                AddParameter("twosided", false);
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("AreaLightSource \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };

    public class ActiveTransform : Directive
    {
        public enum Type
        {
            StartTime,
            EndTime,
            All
        };

        Type type;

        public ActiveTransform(Type type)
            : base()
        {
            this.type = type;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("ActiveTransform {0}\n", type.ToString());

            return str;
        }
    }

    public class ColorSpace : Directive
    {
        public enum Type
        {
            ACES2065_1, //	The standard color space defined in the Academy Color Encoding System.
            REC2020,    // The ITU-R Recommendation BT.2020 color space.
            DCI_P3,     // The DCI-P3 color space, widely used in current displays.
            SRGB       // The venerable sRGB color space; it has the smallest gamut of pbrt-v4's color spaces, but is still widely used.
        };

        Type type;

        public ColorSpace(Type type = Type.SRGB)
            : base()
        {
            this.type = type;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            if (type == Type.ACES2065_1)
                str += string.Format("ColorSpace \"{0}\"\n", "aces2065-1");
            else if (type == Type.REC2020)
                str += string.Format("ColorSpace \"{0}\"\n", "rec2020");
            else if (type == Type.DCI_P3)
                str += string.Format("ColorSpace \"{0}\"\n", "dci-p3");
            else// if (type == Type.SRGB)
                str += string.Format("ColorSpace \"{0}\"\n", "srgb");

            return str;
        }
    }

    public class Camera : Directive
    {
        public enum Type
        {
            Orthographic,   //	OrthographicCamera
            Perspective,    //	PerspectiveCamera
            Realistic,      //	RealisticCamera
            Spherical,      //	SphericalCamera
            Cube,           //  CubeCamera
            Sample          //  SampleCamera
        };

        Type type;

        public Camera(Type type)
            : base()
        {
            this.type = type;

            AddParameter("shutteropen", 0.0f);                                               //	The time at which the virtual camera shutter opens.
            AddParameter("shutterclose", 1.0f);                                              //	The time at which the virtual camera shutter closes.

            if (type == Type.Perspective || type == Type.Orthographic)
            {
                // Perspective, Orthographic only
                AddParameter("frameaspectratio", -1.0f);                                         //  (see description)   The aspect ratio of the film.By default, this is computed from the x and y resolutions of the film, but it can be overridden if desired.
                AddParameters("screenwindow", new List<float> { -1.0f, 1.0f, -1.0f, 1.0f });      //  (see description) The bounds of the film plane in screen space. By default, this is [-1,1] along the shorter image axis and is set proportionally along the longer axis.
                AddParameter("lensradius", 0.0f);                                                //	The radius of the lens. The default value yields a pinhole camera.
                AddParameter("focaldistance", 1e6f);                                             //	The focal distance of the lens. If "lensradius" is zero, this has no effect. Otherwise, it specifies the distance from the camera origin to the focal plane.
            }

            if (type == Type.Perspective)
            {
                // Perspective only
                AddParameter("fov", 90.0f);                                                      //	Specifies the field of view for the perspective camera. This is the spread angle of the viewing frustum along the narrower of the image's width and height.
            }

            if (type == Type.Spherical)
            {
                // SphericalCamera only
                AddParameter("mapping", "equalarea");                                            //	Specifies the field of view for the perspective camera. This is the spread angle of the viewing frustum along the narrower of the image's width and height.
            }

            if (type == Type.Realistic)
            {
                // RealisticCamera only
                AddParameter("lensfile", "");                                                    //	Specifies the name of a lens description file that gives the collection of lens elements in the lens system.A number of such lenses are available in the lenses directory in the pbrt-v4 scenes distribution.
                AddParameter("aperturediameter", 1.0f);                                          //	Diameter of the lens system's aperture, specified in mm. The smaller the aperture, the less light reaches the film plane, but the greater the range of distances that are in focus.
                AddParameter("focusdistance", 10.0f);                                            //	Distance in meters at which the lens system is focused.
                AddParameter("aperture", "");                                                    //  Allows specifying the shape of the camera aperture, which is circular by default. The values of "gaussian", "square", "pentagon", and "star" are associated with built-in aperture shapes; other values are interpreted as filenames specifying an image to be used to specify the shape.
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Camera \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }


    public class CoordinateSystem : Directive
    {
        public enum Type
        {
            World,
            Camera
        };

        Type type;

        public CoordinateSystem(Type type)
            : base()
        {
            this.type = type;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("CoordinateSystem \"{0}\"\n", type.ToString().ToLower());

            return str;
        }
    };

    public class CoordSysTransform : Directive
    {
        public enum Type
        {
            World,
            Camera
        };

        Type type;

        public CoordSysTransform(Type type)
            : base()
        {
            this.type = type;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("CoordSysTransform \"{0}\"\n", type.ToString().ToLower());

            return str;
        }
    };


    public class ConcatTransform : Directive
    {
        float m00, m01, m02, m03;
        float m10, m11, m12, m13;
        float m20, m21, m22, m23;
        float m30, m31, m32, m33;

        public ConcatTransform(float m00, float m01, float m02, float m03,
                         float m10, float m11, float m12, float m13,
                         float m20, float m21, float m22, float m23,
                         float m30, float m31, float m32, float m33)
            : base()
        {
            this.m00 = m00; this.m01 = m01; this.m02 = m02; this.m03 = m03;
            this.m10 = m10; this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m20 = m20; this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m30 = m30; this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("ConcatTransform [ {0:0.0000} {1:0.0000} {2:0.0000} {3:0.0000} {4:0.0000} {5:0.0000} {6:0.0000} {7:0.0000} {8:0.0000} {9:0.0000} {10:0.0000} {11:0.0000} {12:0.0000} {13:0.0000} {14:0.0000} {15:0.0000} ]\n",
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23,
                m30, m31, m32, m33);

            return str;
        }
    };


    public class Film : Directive
    {
        public enum Type
        {
            RGB,        // corresponding to RGBFilm: the default film if none is specified; stores RGB images using the current color space when the Film directive is encountered.
            GBuffer,    // corresponding to GBufferFilm: in addition to RGB, stores multiple additional channels that encode information about the visible geometry in each pixel.Images must be written in OpenEXR format with this film.The User's Guide has further documentation about the format of the information stored by this film implementation.
            Spectral    // corresponding toSpectralFilm: stores a discretized spectral distribution at each pixel, in addition to RGB (for convenience when viewing images.) Images must be written in OpenEXR format with this film.Spectral data is stored using the format described by Fichet et al
        };

        public enum Sensor
        {
            CIE1931,
            Canon_eos_100d,
            Canon_eos_1dx_mkii,
            Canon_eos_200d,
            Canon_eos_200d_mkii,
            Canon_eos_5d,
            Canon_eos_5d_mkii,
            Canon_eos_5d_mkiii,
            Canon_eos_5d_mkiv,
            Canon_eos_5ds,
            Canon_eos_m,
            Hasselblad_l1d_20c,
            Nikon_d810,
            Nikon_d850,
            Sony_ilce_6400,
            Sony_ilce_7m3,
            Sony_ilce_7rm3,
            Sony_ilce_9
        };

        public enum CoordinateSystemType
        {
            Camera,
            World
        };

        Type type = Type.RGB;
        Sensor sensor = Sensor.CIE1931;
        CoordinateSystemType coordinateSystem = CoordinateSystemType.Camera;

        public Film(Type type)
            : base()
        {
            this.type = type;

            AddParameter("xresolution", 1280, true);
            AddParameter("yresolution", 720, true);
            AddParameters("cropwindow", new List<float> { 0.0f, 1.0f, 0.0f, 1.0f });
            AddParameters("pixelbounds", new List<int> { 0, 1, 0, 1 });
            AddParameter("diagonal", 35.0f);
            AddParameter("filename", "pbrt.exr");
            AddParameter("savefp16", true);
            AddParameter("iso", 100.0f);
            AddParameter("whitebalance", 0);
            sensor = Sensor.CIE1931;

            AddParameter("maxcomponentvalue", float.MaxValue);

            if (type == Type.GBuffer)
            {
                // GBuffer 
                coordinateSystem = CoordinateSystemType.World;
            }

            if (type == Type.Spectral)
            {
                // SpectralFilm 
                AddParameter("nbuckets", 16);
                AddParameter("lambdamin", 360);
                AddParameter("lambdamax", 830);
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Film \"{0}\"\n", type.ToString().ToLower());
            //str += string.Format("    \"string sensor\" [ \"{0}\" ]\n", sensor.ToString().ToLower());
            if(type == Type.GBuffer)
                str += string.Format("    \"string coordinatesystem\" [ \"{0}\" ]\n", coordinateSystem.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }

    public class Include : Directive
    {
        string path;
        public Include(string path)
            : base()
        {
            this.path = path;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Include \"{0}\"\n", path.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }

    public class Import : Directive
    {
        string path;

        public Import(string path)
            : base()
        {
            this.path = path;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Import \"{0}\"\n", path.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }

    public class Identity : Directive
    {
        public Identity()
            : base()
        {
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Identity\n");
            str += GetParametersString();

            return str;
        }
    }
    public class Integrator : Directive
    {
        public enum Type
        {
            Ambientocclusion,    //	 Ambient occlusion (accessibility over the hemisphere)
            BDPT,                //	 Bidirectional path tracing
            Lightpath,           //	 Path tracing starting from the light sources
            Mlt,                 //	 Metropolis light transport using bidirectional path tracing
            Path,                //	 Path tracing
            Randomwalk,          //	 Rendering using a simple random walk without any explicit light sampling
            Simplepath,          //	 Path tracing with very basic sampling algorithms
            Simplevolpath,       //	 Volumetric path tracing with very basic sampling algorithms
            Sppm,                //	 Stochastic progressive photon mapping
            Volpath              //	 Volumetric path tracing
        };

        public enum LightSamplerType
        {
            BVH,
            Uniform,
            Power
        };

        // all
        Type type;
        LightSamplerType lightSamplerType;

        public Integrator(Type type)
            : base()
        {
            this.type = type;

            if (type == Type.Ambientocclusion)
            {
            }
            else if (type == Type.BDPT)
            {
                AddParameter("maxdepth", 5, true);

                AddParameter("regularize", false);
            }
            else if (type == Type.Lightpath)
            {
                AddParameter("maxdepth", 5, true);
            }
            else if (type == Type.Mlt)
            {
                AddParameter("maxdepth", 5, true);

                AddParameter("regularize", false);
            }
            else if (type == Type.Path)
            {
                AddParameter("maxdepth", 5, true);

                lightSamplerType = LightSamplerType.BVH;

                AddParameter("regularize", false);
            }
            else if (type == Type.Randomwalk)
            {
                AddParameter("maxdepth", 5, true);
            }
            else if (type == Type.Simplepath)
            {
                AddParameter("maxdepth", 5, true);
            }
            else if (type == Type.Simplevolpath)
            {
                AddParameter("maxdepth", 5, true);

                lightSamplerType = LightSamplerType.BVH;

                AddParameter("regularize", false);
            }
            else if (type == Type.Sppm)
            {
                AddParameter("maxdepth", 5, true);
            }
            else //if (type==Type.Volpath)
            {
                AddParameter("maxdepth", 5, true);

                AddParameter("regularize", false);
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Integrator \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };

    public class Light : Directive
    {
        public enum Type
        {
            Distant,
            Goniometric,
            Infinite,
            Point,
            Projection,
            Spot
        };

        // all
        Type type = Type.Distant;

        public Light(Type type)
            : base()
        {
            this.type = type;

            AddParameter("power", 1.0f);
            AddParameter("illuminance", 1.0f);
            AddParameter("scale", 1.0f);

            if (type == Type.Distant)
            {
                AddParameter("l", new LerpSpectrum(300.0f, 1.0f, 800.0f, 1.0f));
                AddParameter("from", new Point3(0.0f, 0.0f, 0.0f));
                AddParameter("to", new Point3(0.0f, 0.0f, 1.0f));
            }
            else if (type == Type.Goniometric)
            {
                AddParameter("filename", "");
                AddParameter("l", new LerpSpectrum(300.0f, 1.0f, 800.0f, 1.0f));
            }
            else if (type == Type.Infinite)
            {
                AddParameter("filename", "");
                AddParameters("portal", new List<Point3> { new Point3(0.0f, 0.0f, 0.0f), new Point3(0.0f, 0.0f, 0.0f), new Point3(0.0f, 0.0f, 0.0f) });
                AddParameter("l", new LerpSpectrum(300.0f, 1.0f, 800.0f, 1.0f));
            }
            else if (type == Type.Point)
            {
                AddParameter("from", new Point3(0.0f, 0.0f, 0.0f));
                AddParameter("l", new LerpSpectrum(300.0f, 1.0f, 800.0f, 1.0f));
            }
            else if (type == Type.Projection)
            {
                AddParameter("l", new LerpSpectrum(300.0f, 1.0f, 800.0f, 1.0f));
                AddParameter("fov", 90.0f);
                AddParameter("filename", "");
            }
            else if (type == Type.Spot)
            {
                AddParameter("l", new LerpSpectrum(300.0f, 1.0f, 800.0f, 1.0f));
                AddParameter("from", new Point3(0.0f, 0.0f, 0.0f));
                AddParameter("to", new Point3(0.0f, 0.0f, 1.0f));
                AddParameter("coneangle", 30.0f);
                AddParameter("conedeltaangle", 5.0f);
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Light \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };

    public class LightSource : Directive
    {
        public enum Type
        {
            Distant,
            Goniometric,	
            Infinite,
            point,
            projection,
            spot
        };

        // all
        Type type;

        public LightSource(Type type)
            : base()
        {
            this.type = type;

            if (type == Type.Distant)
            {
                AddParameter("from", new Point3(0, 0, 0));
            }
            else if(type == Type.Goniometric)
            {

            }
            else if (type == Type.Infinite)
            {
                AddParameter("filename", string.Empty);
                AddParameters("date_time_location_timezone", new List<float> { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f});
                AddParameter("power", 1.0f, true);
                AddParameter("sunsize", 0.1f, true);
                AddParameter("sunsizeconvergence", 1.0f, true);
                AddParameter("atmospherethickness", 1.0f, true);
                AddParameter("skyexposure", 1.0f, true);
                AddParameter("imagesize", 512, true);

                AddParameter("L", new RGBSpectrum(0.0f, 0.0f, 0.0f));
            }
            else if (type == Type.point)
            {
                AddParameter("scale", 1.0f);
                AddParameter("from", new Point3(0.0f, 0.0f, 0.0f));
                AddParameter("I", new RGBSpectrum(1.0f, 1.0f, 1.0f), true);
            }
            else if (type == Type.projection)
            {

            }
            else //if (type == Type.spot)
            {

            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("LightSource \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };

    public class LookAt : Directive
    {
        float eye_x;
        float eye_y;
        float eye_z;
        float look_x;
        float look_y;
        float look_z;
        float up_x;
        float up_y;
        float up_z;

        public LookAt(float eye_x, float eye_y, float eye_z, float look_x, float look_y, float look_z, float up_x, float up_y, float up_z)
            : base()
        {
            this.eye_x = eye_x;
            this.eye_y = eye_y;
            this.eye_z = eye_z;
            this.look_x = look_x;
            this.look_y = look_y;
            this.look_z = look_z;
            this.up_x = up_x;
            this.up_y = up_y;
            this.up_z = up_z;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("LookAt {0:0.0000} {1:0.0000} {2:0.0000}\n", eye_x, eye_y, eye_z);
            str += string.Format("    {0:0.0000} {1:0.0000} {2:0.0000}\n", look_x, look_y, look_z);
            str += string.Format("    {0:0.0000} {1:0.0000} {2:0.0000}\n", up_x, up_y, up_z);

            return str;
        }
    };

    public class Material : Directive
    {
        public enum Type
        {
            CoatedDiffuse,
            CoatedConductor,
            Conductor,
            Dielectric,
            Diffuse,
            DiffuseTransmission,
            Hair,
            Interface,
            Measured,
            Mix,
            SubSurface,
            ThinDielectric,
            Disney,
            Uber
        };

        // all
        Type type = Type.Diffuse;

        public Material(Type type)
            : base()
        {
            this.type = type;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Material \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };

    public class MakeNamedMaterial : Directive
    {
        string name = "";
        Material.Type type = Material.Type.Diffuse;

        //public static MakeNamedMaterial none = new MakeNamedMaterial(Material.Type.None, "(none)");

        public string GetName()
        {
            return name;
        }

        public MakeNamedMaterial(Material.Type type, string name)
            : base()
        {
            this.type = type;
            this.name = name;

            //if (type == Material.Type.None)
            //{
            //    return;
            //}

            if (!(type == Material.Type.Interface || type == Material.Type.Mix))
            {
                AddParameter("displacement", Texture.none);
                AddParameter("normalmap", "");
            }

            AddParameter("roughness", Texture.none);
            AddParameter("roughness", 0.5f);
            AddParameter("uroughness", Texture.none);
            AddParameter("uroughness", 0.0f);
            AddParameter("vroughness", Texture.none);
            AddParameter("vroughness", 0.0f);
            AddParameter("remaproughness", true);

            if (type == Material.Type.CoatedDiffuse)
            {
                AddParameter("albedo", Texture.none);
                AddParameter("albedo", 0.0f);
                AddParameter("g", Texture.none);
                AddParameter("g", 0.0f);
                AddParameter("maxdepth", 10);
                AddParameter("nsamples", 1);
                AddParameter("thickness", 0.01f);

                AddParameter("reflectance", Texture.none);
                AddParameter("reflectance", 0.0f);
                AddParameter("reflectance", new PBRT.RGBSpectrum(0.5f, 0.5f, 0.5f));
            }
            else if (type == Material.Type.CoatedConductor)
            {
            }
            else if (type == Material.Type.Conductor)
            {
                AddParameter("eta", new PBRT.RGBSpectrum(0, 0, 0));
                AddParameter("k", new PBRT.RGBSpectrum(0, 0, 0));
            }
            else if (type == Material.Type.Dielectric)
            {
                AddParameter("eta", 0.0f);
            }
            else if (type == Material.Type.Diffuse)
            {
                AddParameter("reflectance", Texture.none);
                AddParameter("reflectance", 0.0f);
                AddParameter("reflectance", new PBRT.RGBSpectrum(0.5f, 0.5f, 0.5f));
            }
            else if (type == Material.Type.DiffuseTransmission)
            {
            }
            else if (type == Material.Type.Hair)
            {
            }
            else if (type == Material.Type.Interface)
            {
            }
            else if (type == Material.Type.Measured)
            {
            }
            else if (type == Material.Type.Mix)
            {
            }
            else if (type == Material.Type.SubSurface)
            {
            }
            else if (type == Material.Type.ThinDielectric)
            {
            }
            else if (type == Material.Type.Disney)
            {
                AddParameter("color", new PBRT.RGBSpectrum(0.5f, 0.5f, 0.5f));
                AddParameter("color", PBRT.Texture.none);

                AddParameter("anisotropic", 0.0f, true);
                AddParameter("clearcoat", 0.0f, true);
                AddParameter("clearcoatgloss", 1.0f, true);
                AddParameter("eta", 1.5f, true);
                AddParameter("metallic", 0.0f, true);
                AddParameter("scatterdistance", new PBRT.RGBSpectrum(0.0f, 0.0f, 0.0f), true);
                AddParameter("sheen", 0.0f, true);
                AddParameter("sheentint", 0.5f, true);
                AddParameter("spectrans", 0.0f, true);
                AddParameter("speculartint", 0.0f, true);

                AddParameter("collect", 0.0f);
            }
        }

        public bool Equals(MakeNamedMaterial other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return name == other.name && type == other.type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as MakeNamedMaterial);
        }

        public override int GetHashCode()
        {
            return (name, type).GetHashCode();
        }

        public static bool operator ==(MakeNamedMaterial left, MakeNamedMaterial right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MakeNamedMaterial left, MakeNamedMaterial right)
        {
            return !Equals(left, right);
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("MakeNamedMaterial \"{0}\"\n", name);
            str += string.Format("    \"string type\" [ \"{0}\" ]\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }

    public class NamedMaterial : Directive
    {
        // all
        string name = "";

        public NamedMaterial(string name)
            : base()
        {
            this.name = name;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("NamedMaterial \"{0}\"\n", name);
            str += GetParametersString();

            return str;
        }
    };

    /*
    // TODO
    public class MakeNamedMedium : Directive
    {
        public enum Type
        {
            Diffuse
        };

        // all
        Type type = Type.Diffuse;

        public MakeNamedMedium(Type type)
            : base()
        {
            this.type = type;

            if (type == Type.Diffuse)
            {
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("MakeNamedMedium \"{0}\"", type.ToString().ToLower());
            str += GetParametersString();
            str += "\n";

            return str;
        }
    };

    public class MediumInterface : Directive
    {
        public enum Type
        {
            Diffuse
        };

        // all
        Type type = Type.Diffuse;

        public MediumInterface(Type type)
            : base()
        {
            this.type = type;

            if (type == Type.Diffuse)
            {
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("MediumInterface \"{0}\"", type.ToString().ToLower());
            str += GetParametersString();
            str += "\n";

            return str;
        }
    };
    */

    public class ObjectBegin : Directive
    {
        string id;

        public ObjectBegin(string id)
            : base()
        {
            this.id = id;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("ObjectBegin \"{0}\"\n", id.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }

    public class ObjectEnd : Directive
    {
        public ObjectEnd()
            : base()
        {
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("ObjectEnd\n");
            str += GetParametersString();

            return str;
        }
    }

    public class ObjectInstance : Directive
    {
        string id;

        public ObjectInstance(string id)
            : base()
        {
            this.id = id;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("ObjectInstance \"{0}\"\n", id.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }
    public class Option : Directive
    {
        public Option()
            : base()
        {
            AddParameter("disablepixeljitter", false);             // Forces all pixel samples to be through the center of the pixel area.Enabling this can be useful when computing reference images and then computing error with respect to them in that it eliminates differences due to geometric sampling that may not be of interest.
            AddParameter("disabletexturefiltering", false);        // Forces point sampling at the finest MIP level for all texture lookups. (Rarely useful.)
            AddParameter("disablewavelengthjitter", false);        // Forces all samples within each pixel to sample the same wavelengths.RGB images will generally have objectionable color error but this can also be useful when computing error with respect to reference images when error due to random wavelength sampling shouldn't be included.
            AddParameter("displacementedgescale", 1.0f);           // Global scale factor applied to triangle edge lengths before evaluating the edge length test for refinement when applying displacement mapping.Increasing this value will generally reduce memory use and startup time when rendering scenes with displacement mapping.
            AddParameter("msereferenceimage", "");                 // (none) Specifies the filename of an image to use when computing mean squared error versus the number of pixel samples taken (see "msereferenceout" below).
            AddParameter("msereferenceout", "");                   // Filename for per-sample mean squared error results.When both this and "msereferenceimage" are specified, the mean squared error of the current image and the reference image is computed after each sample is taken and the results are stored in text format in this file.
            AddParameter("rendercoordsys", "cameraworld");         // Specifies the coordinate system to use for rendering computation.The default, "cameraworld" translates the scene so that the camera is at the origin. "camera" uses camera space (performance may suffer due to the scene being rotated) and "world" uses world space(accuracy may suffer due to floating-point precision).
            AddParameter("seed", 0);                               // Seed to use for pseudo-random number generation during rendering.Rendering a scene with different seed values will give independent results, which can be useful for statistical analysis.
            AddParameter("forcediffuse", false);                   // Force all materials to be diffuse. (Note: not currently supported with the --wavefront and --gpu integrators.)
            AddParameter("pixelstats", false);                     // Write out images after rendering that encode per-pixel statistics including time spent rendering each pixel and any other per-pixel statistics added using STAT_PIXEL_COUNTER or STAT_PIXEL_RATIO in the pbrt source code.
            AddParameter("wavefront", false);                      // Enables the "wavefront" integrator (i.e., the integrator used for GPU rendering, but running on the CPU.)
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Option\n");
            str += GetParametersString();

            return str;
        }
    };


    public class PixelFilter : Directive
    {
        public enum Type
        {
            Box,
            Gussian,
            Mitchell,
            Sinc,
            Triangle
        };

        // all
        Type type;

        public PixelFilter(Type type)
            : base()
        {
            this.type = type;

            if (type == Type.Box)
            {
                AddParameter("xradius", 0.5f);
                AddParameter("yradius", 0.5f);
            }
            else if (type == Type.Gussian)
            {
                AddParameter("xradius", 1.5f);
                AddParameter("yradius", 1.5f);

                // gaussian only
                AddParameter("sigma", 0.5f);
            }
            else if (type == Type.Mitchell)
            {
                AddParameter("xradius", 2.0f);
                AddParameter("yradius", 2.0f);

                // mitchell only
                AddParameter("B", 1.0f / 3.0f);
                AddParameter("C", 1.0f / 3.0f);
            }
            else if (type == Type.Sinc)
            {
                AddParameter("xradius", 4.0f);
                AddParameter("yradius", 4.0f);

                // sinc only
                AddParameter("tau", 3.0f);
            }
            else// if (type == Type.Triangle)
            {
                AddParameter("xradius", 2.0f);
                AddParameter("yradius", 2.0f);
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("PixelFilter \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }


    public class ReverseOrientation : Directive
    {
        public ReverseOrientation()
            : base()
        {
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("ReverseOrientation\n");
            str += GetParametersString();

            return str;
        }
    }


    public class Rotate : Directive
    {
        float angle;
        float x;
        float y;
        float z;

        public Rotate(float angle, float x, float y, float z)
            : base()
        {
            this.angle = angle;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Rotate [ {0:0.0000} {1:0.0000} {2:0.0000} {3:0.0000} ]\n", angle, x, y, z);

            return str;
        }
    };

    public class Shape : Directive
    {
        public enum Type
        {
            BilinearMesh,
            Curve,
            Cylinder,
            Disk,
            Sphere,
            TriangleMesh,
            LoopSubdiv,
            PLYMesh
        };

        // all
        Type type = Type.TriangleMesh;

        public Shape(Type type)
            : base()
        {
            this.type = type;

            AddParameter("alpha", 1.0f);
            AddParameter("alpha", Texture.none);

            if (type == Type.BilinearMesh)
            {
            }
            else if (type == Type.Curve)
            {
                AddParameters("P", new List<Point3> { new Point3(0.0f, 0.0f, 0.0f), new Point3(0.0f, 0.0f, 0.0f), new Point3(0.0f, 0.0f, 0.0f), new Point3(0.0f, 0.0f, 0.0f) });
                AddParameter("basis", "bezier"); // only support bezier for now, others are bspline
                AddParameter("degree", 3);
                AddParameter("type", "flat"); // only support flat for now, others are cylinder, ribbon
                AddParameters("N", new List<Normal3> { new Normal3(1.0f, 0.0f, 0.0f), new Normal3(1.0f, 0.0f, 0.0f) });
                AddParameter("width", 1.0f);
                AddParameter("width0", 1.0f);
                AddParameter("width1", 1.0f);
                AddParameter("splitdepth", 3);
            }
            else if (type == Type.Cylinder)
            {
                AddParameter("radius", 1.0f);
                AddParameter("zmin", 1.0f);
                AddParameter("zmax", 1.0f);
                AddParameter("phimax", 360.0f);
            }
            else if (type == Type.Disk)
            {
                AddParameter("height", 0.0f);
                AddParameter("radius", 1.0f);
                AddParameter("innerradius", 0.0f);
                AddParameter("phimax", 360.0f);
            }
            else if (type == Type.Sphere)
            {
                AddParameter("radius", 1.0f);
                AddParameter("zmin", -1.0f);
                AddParameter("zmax", 1.0f);
                AddParameter("phimax", 360.0f);
            }
            else if (type == Type.TriangleMesh)
            {
                AddParameters("indices", new List<int>());
                AddParameters("P", new List<Point3>());
                AddParameters("N", new List<Normal3>());
                AddParameters("S", new List<Vector3>());
                AddParameters("uv", new List<Point2>());
            }
            else if (type == Type.LoopSubdiv)
            {
                AddParameter("levels", 3);
                AddParameters("indices", new List<int>());
                AddParameters("P", new List<Point3>());
            }
            else if (type == Type.PLYMesh)
            {
                AddParameter("filename", "");
                AddParameter("displacement", Texture.none);
                AddParameter("edgelength", 1.0f);
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Shape \"{0}\"\n", type.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };

    public class Sampler : Directive
    {
        public enum Type
        {
            Halton,
            Independent,
            PaddedSobol,
            Sobol,
            Stratified,
            ZSobol
        };

        public enum Randomization
        {
            None,
            Permutedigits,
            Owen,
            FastOwen
        };

        Type type = Type.ZSobol;
        Randomization randomization = Randomization.FastOwen;

        public Sampler(Type type)
            : base()
        {
            this.type = type;

            // all
            AddParameter("seed", 0);

            if (type != Type.Stratified)
            {
                // All of the samplers other than the StratifiedSampler
                AddParameter("pixelsamples", 16, true);
            }

            if (type == Type.Halton)
            {
                // HaltonSampler
                randomization = Randomization.Permutedigits;
            }

            if (type == Type.PaddedSobol || type == Type.Sobol || type == Type.ZSobol)
            {
                // HaltonSampler, PaddedSobolSampler, SobolSampler, and ZSobolSampler
                randomization = Randomization.FastOwen;
            }

            if (type == Type.Stratified)
            {
                // StratifiedSampler 
                AddParameter("jitter", true);
                AddParameter("xsamples", 4);
                AddParameter("ysamples", 4);
            }
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Sampler \"{0}\"\n", type.ToString().ToLower());
            //str += string.Format("    \"string randomization\" \"{0}\"\n", randomization.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    };


    public class Scale : Directive
    {
        float x;
        float y;
        float z;

        public Scale(float x, float y, float z)
            : base()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Scale {0:0.0000} {1:0.0000} {2:0.0000}\n", x, y, z);

            return str;
        }
    };


    public class Translate : Directive
    {
        float x;
        float y;
        float z;

        public Translate(float x, float y, float z)
            : base()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Translate [{0:0.0000} {1:0.0000} {2:0.0000}]\n", x, y, z);

            return str;
        }
    };

    public class TransformBegin : Directive
    {
        public TransformBegin(string id)
            : base()
        {
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("TransformBegin\n");
            str += GetParametersString();

            return str;
        }
    }

    public class TransformEnd : Directive
    {
        public TransformEnd()
            : base()
        {
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("TransformEnd\n");
            str += GetParametersString();

            return str;
        }
    }

    public class TransformTimes : Directive
    {
        float start, end;

        public TransformTimes(float start, float end)
            : base()
        {
            this.start = start;
            this.end = end;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("TransformTimes [ {0:0.0000} {1:0.0000} ]\n", start, end);

            return str;
        }
    };

    public class Transform : Directive
    {
        float m00, m01, m02, m03;
        float m10, m11, m12, m13;
        float m20, m21, m22, m23;
        float m30, m31, m32, m33;

        public Transform(float m00, float m01, float m02, float m03,
                         float m10, float m11, float m12, float m13,
                         float m20, float m21, float m22, float m23,
                         float m30, float m31, float m32, float m33)
            : base()
        {
            this.m00 = m00; this.m01 = m01; this.m02 = m02; this.m03 = m03;
            this.m10 = m10; this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m20 = m20; this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m30 = m30; this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Transform [ {0:0.0000} {1:0.0000} {2:0.0000} {3:0.0000} {4:0.0000} {5:0.0000} {6:0.0000} {7:0.0000} {8:0.0000} {9:0.0000} {10:0.0000} {11:0.0000} {12:0.0000} {13:0.0000} {14:0.0000} {15:0.0000} ]\n",
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23,
                m30, m31, m32, m33);

            return str;
        }
    };

    public class Texture : Directive
    {
        public enum DataType
        {
            None,
            Float,
            Spectrum
        };

        public enum ClassType
        {
            None,
            Bilerp,
            CheckerBoard,
            Constant,
            DirectionMix,
            Dots,
            FBM,
            ImageMap,
            Marble,
            Mix,
            Ptex,
            Scale,
            Windy,
            Wrinkled
        };

        public enum Mapping
        {
            UV,
            Spherical,
            Cylindrical,
            Planar
        };

        string name = "";
        DataType dataType = DataType.Float;
        ClassType classType = ClassType.ImageMap;
        Mapping mapping = Mapping.UV;

        public static Texture none = new Texture(DataType.None, ClassType.None, Mapping.UV, "(none)");

        public string GetName()
        {
            return name;
        }

        public Texture(DataType dataType, ClassType classType, Mapping mapping, string name)
            : base()
        {
            this.name = name;
            this.dataType = dataType;
            this.classType = classType;
            this.mapping = mapping;

            if (dataType == DataType.None || classType == ClassType.None)
            {
                return;
            }

            if (mapping == Mapping.UV)
            {
                AddParameter("uscale", 1.0f);
                AddParameter("vscale", 1.0f);
                AddParameter("udelta", 0.0f);
                AddParameter("vdelta", 0.0f);
            }
            else if (mapping == Mapping.Spherical)
            {
            }
            else if (mapping == Mapping.Cylindrical)
            {
            }
            else if (mapping == Mapping.Planar)
            {
                AddParameter("udelta", 0.0f);
                AddParameter("vdelta", 0.0f);

                AddParameter("v1", new PBRT.Vector3(1.0f, 0.0f, 0.0f));
                AddParameter("v2", 0.0f);
            }

            if (dataType == DataType.Float)
            {
                if (classType == ClassType.None)
                {
                }
                else if (classType == ClassType.Bilerp)
                {
                    AddParameter("v00", Texture.none);
                    AddParameter("v01", Texture.none);
                    AddParameter("v10", Texture.none);
                    AddParameter("v11", Texture.none);

                    AddParameter("v00", 0.0f);
                    AddParameter("v01", 1.0f);
                    AddParameter("v10", 0.0f);
                    AddParameter("v11", 1.0f);
                }
                else if (classType == ClassType.CheckerBoard)
                {
                    AddParameter("dimension", 2);
                    AddParameter("tex1", Texture.none);
                    AddParameter("tex2", Texture.none);
                    AddParameter("tex1", 1.0f);
                    AddParameter("tex2", 0.0f);
                }
                else if (classType == ClassType.Constant)
                {
                    AddParameter("value", Texture.none);
                    AddParameter("value", 1.0f);
                }
                else if (classType == ClassType.DirectionMix)
                {
                    AddParameter("tex1", Texture.none);
                    AddParameter("tex2", Texture.none);
                    AddParameter("tex1", 0.0f);
                    AddParameter("tex2", 1.0f);
                    AddParameter("dir", new Vector3(0.0f, 1.0f, 0.0f));
                }
                else if (classType == ClassType.Dots)
                {
                    AddParameter("inside", Texture.none);
                    AddParameter("outside", Texture.none);
                    AddParameter("inside", 1.0f);
                    AddParameter("outside", 0.0f);
                }
                else if (classType == ClassType.FBM)
                {
                    AddParameter("octaves", 8);
                    AddParameter("roughness", 0.5f);
                }
                else if (classType == ClassType.ImageMap)
                {
                    AddParameter("filename", "");
                    AddParameter("wrap", "repeat"); // TODO
                    AddParameter("maxanisotropy", 8);
                    AddParameter("filter", "bilinear"); // TODO
                    AddParameter("encoding", "sRGB"); // TODO
                    AddParameter("scale", 1);
                    AddParameter("invert", false);
                }
                else if (classType == ClassType.Marble)
                {
                    AddParameter("octaves", 8);
                    AddParameter("roughness", 0.5f);
                    AddParameter("scale", 1.0f);
                    AddParameter("variation", 0.2f);
                }
                else if (classType == ClassType.Mix)
                {
                    AddParameter("tex1", Texture.none);
                    AddParameter("tex2", Texture.none);
                    AddParameter("tex1", 0.0f);
                    AddParameter("tex2", 1.0f);
                    AddParameter("amount", 0.5f);
                }
                else if (classType == ClassType.Ptex)
                {
                    AddParameter("encoding", "gamma 2.2");
                    AddParameter("filename", "");
                    AddParameter("scale", 1.0f);
                }
                else if (classType == ClassType.Scale)
                {
                    AddParameter("tex", Texture.none);
                    AddParameter("scale", Texture.none);
                    AddParameter("tex", 0.0f);
                    AddParameter("scale", 1.0f);
                }
                else if (classType == ClassType.Windy)
                {
                    AddParameter("octaves", 8);
                    AddParameter("roughness", 0.5f);
                }
                else if (classType == ClassType.Wrinkled)
                {
                    AddParameter("octaves", 8);
                    AddParameter("roughness", 0.5f);
                }
            }
            else if (dataType == DataType.Spectrum)
            {
                if (classType == ClassType.None)
                {
                }
                else if (classType == ClassType.Bilerp)
                {
                    AddParameter("v00", Texture.none);
                    AddParameter("v01", Texture.none);
                    AddParameter("v10", Texture.none);
                    AddParameter("v11", Texture.none);

                    AddParameter("v00", 0.0f);
                    AddParameter("v01", 1.0f);
                    AddParameter("v10", 0.0f);
                    AddParameter("v11", 1.0f);
                }
                else if (classType == ClassType.CheckerBoard)
                {
                    AddParameter("dimension", 2);
                    AddParameter("tex1", Texture.none);
                    AddParameter("tex2", Texture.none);
                    AddParameter("tex1", 1.0f);
                    AddParameter("tex2", 0.0f);
                    AddParameter("tex1", new PBRT.RGBSpectrum(0.0f, 0.0f, 0.0f));
                    AddParameter("tex2", new PBRT.RGBSpectrum(0.0f, 0.0f, 0.0f));
                }
                else if (classType == ClassType.Constant)
                {
                    AddParameter("value", Texture.none);
                    AddParameter("value", 1.0f);
                }
                else if (classType == ClassType.DirectionMix)
                {
                    AddParameter("tex1", Texture.none);
                    AddParameter("tex2", Texture.none);
                    AddParameter("tex1", 0.0f);
                    AddParameter("tex2", 1.0f);
                    AddParameter("tex1", new PBRT.RGBSpectrum(0.0f, 0.0f, 0.0f));
                    AddParameter("tex2", new PBRT.RGBSpectrum(0.0f, 0.0f, 0.0f));
                    AddParameter("dir", new Vector3(0.0f, 1.0f, 0.0f));
                }
                else if (classType == ClassType.Dots)
                {
                    AddParameter("inside", Texture.none);
                    AddParameter("outside", Texture.none);
                    AddParameter("inside", 1.0f);
                    AddParameter("outside", 0.0f);
                }
                else if (classType == ClassType.FBM)
                {
                    AddParameter("octaves", 8);
                    AddParameter("roughness", 0.5f);
                }
                else if (classType == ClassType.ImageMap)
                {
                    AddParameter("filename", "");
                    AddParameter("wrap", "repeat"); // TODO
                    AddParameter("maxanisotropy", 8);
                    AddParameter("filter", "bilinear"); // TODO
                    AddParameter("encoding", "sRGB"); // TODO
                    AddParameter("scale", 1);
                    AddParameter("invert", false);
                }
                else if (classType == ClassType.Marble)
                {
                    AddParameter("octaves", 8);
                    AddParameter("roughness", 0.5f);
                    AddParameter("scale", 1.0f);
                    AddParameter("variation", 0.2f);
                }
                else if (classType == ClassType.Mix)
                {
                    AddParameter("tex1", Texture.none);
                    AddParameter("tex2", Texture.none);
                    AddParameter("tex1", 0.0f);
                    AddParameter("tex2", 1.0f);
                    AddParameter("tex1", new PBRT.RGBSpectrum(0.0f, 0.0f, 0.0f));
                    AddParameter("tex2", new PBRT.RGBSpectrum(0.0f, 0.0f, 0.0f));
                    AddParameter("amount", 0.5f);
                }
                else if (classType == ClassType.Ptex)
                {
                    AddParameter("encoding", "gamma 2.2");
                    AddParameter("filename", "");
                    AddParameter("scale", 1.0f);
                }
                else if (classType == ClassType.Scale)
                {
                    AddParameter("tex", Texture.none);
                    AddParameter("scale", Texture.none);
                    AddParameter("tex", 0.0f);
                    AddParameter("scale", 1.0f);
                }
                else if (classType == ClassType.Windy)
                {
                    AddParameter("octaves", 8);
                    AddParameter("roughness", 0.5f);
                }
                else if (classType == ClassType.Wrinkled)
                {
                    AddParameter("octaves", 8);
                    AddParameter("roughness", 0.5f);
                }
            }
        }

        public bool Equals(Texture other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return name == other.name && dataType == other.dataType && classType == other.classType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as Texture);
        }

        public override int GetHashCode()
        {
            return (name, dataType, classType).GetHashCode();
        }

        public static bool operator ==(Texture left, Texture right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Texture left, Texture right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("{0}", name);
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Texture \"{0}\" \"{1}\" \"{2}\"\n", name, dataType.ToString().ToLower(), classType.ToString().ToLower());
            str += GetParametersString();

            return str;
        }
    }

    public class WorldBegin : Directive
    {
        public WorldBegin()
            : base()
        {
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("\n");
            str += string.Format("WorldBegin\n");
            str += string.Format("# --------------------------------------------------\n");
            str += GetParametersString();

            return str;
        }
    }

    public class IncludeFile : Directive
    {
        public string fileName;

        public IncludeFile(string fileName)
            : base()
        {
            this.fileName = fileName;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += string.Format("Include \"{0}\"\n", fileName);
            str += GetParametersString();

            return str;
        }
    }

    public class Custom : Directive
    {
        public string content;

        public Custom(string content)
            : base()
        {
            this.content = content;
        }

        public override string ToDirectiveString()
        {
            string str = "";

            str += content;
            str += string.Format("\n");

            return str;
        }
    }

    public class PBRTScene
    {
        List<Directive> directives = new List<Directive>();

        Dictionary<string, MakeNamedMaterial> namedMaterials = new Dictionary<string, MakeNamedMaterial>();
        Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public MakeNamedMaterial GetNamedMaterial(string name)
        {
            return namedMaterials[name];
        }

        public Texture GetTexture(string name)
        {
            return textures[name];
        }

        public PBRTScene()
        {
        }

        public Accelerator Accelerator()
        {
            var instance = new Accelerator(PBRT.Accelerator.Type.BVH);
            directives.Add(instance);

            return instance;
        }

        public AttributeBegin AttributeBegin()
        {
            var instance = new AttributeBegin();
            directives.Add(instance);

            return instance;
        }

        public AttributeEnd AttributeEnd()
        {
            var instance = new AttributeEnd();
            directives.Add(instance);

            return instance;
        }

        public Attribute Attribute(Attribute.Type type)
        {
            var instance = new Attribute(type);
            directives.Add(instance);

            return instance;
        }

        public AreaLightSource AreaLightSource(AreaLightSource.Type type)
        {
            var instance = new AreaLightSource(type);
            directives.Add(instance);

            return instance;
        }

        public ActiveTransform ActiveTransform(ActiveTransform.Type type)
        {
            var instance = new ActiveTransform(type);
            directives.Add(instance);

            return instance;
        }

        public ColorSpace ColorSpace(ColorSpace.Type type)
        {
            var instance = new ColorSpace(type);
            directives.Add(instance);

            return instance;
        }

        public Camera Camera(Camera.Type type)
        {
            var instance = new Camera(type);
            directives.Add(instance);

            return instance;
        }

        public CoordinateSystem CoordinateSystem(CoordinateSystem.Type type)
        {
            var instance = new CoordinateSystem(type);
            directives.Add(instance);

            return instance;
        }

        public CoordSysTransform CoordSysTransform(CoordSysTransform.Type type)
        {
            var instance = new CoordSysTransform(type);
            directives.Add(instance);

            return instance;
        }

        public ConcatTransform ConcatTransform(float m00, float m01, float m02, float m03,
                                            float m10, float m11, float m12, float m13,
                                            float m20, float m21, float m22, float m23,
                                            float m30, float m31, float m32, float m33)
        {
            var instance = new ConcatTransform(m00, m01, m02, m03,
                                               m10, m11, m12, m13,
                                               m20, m21, m22, m23,
                                               m30, m31, m32, m33);
            directives.Add(instance);

            return instance;
        }

        public Film Film(Film.Type type)
        {
            var instance = new Film(type);
            directives.Add(instance);

            return instance;
        }

        public Include Include(string path)
        {
            var instance = new Include(path);
            directives.Add(instance);

            return instance;
        }

        public Import Import(string path)
        {
            var instance = new Import(path);
            directives.Add(instance);

            return instance;
        }

        public Identity Identity()
        {
            var instance = new Identity();
            directives.Add(instance);

            return instance;
        }

        public Integrator Integrator(Integrator.Type type)
        {
            var instance = new Integrator(type);
            directives.Add(instance);

            return instance;
        }

        public Light Light(Light.Type type)
        {
            var instance = new Light(type);
            directives.Add(instance);

            return instance;
        }

        public LightSource LightSource(LightSource.Type type)
        {
            var instance = new LightSource(type);
            directives.Add(instance);

            return instance;
        }

        public LookAt LookAt(float m00, float m01, float m02,
                         float m10, float m11, float m12,
                         float m20, float m21, float m22)
        {
            var instance = new LookAt(m00, m01, m02,
                                      m10, m11, m12,
                                      m20, m21, m22);
            directives.Add(instance);

            return instance;
        }

        public MakeNamedMaterial MakeNamedMaterial(Material.Type type, string name)
        {
            var instance = new MakeNamedMaterial(type, name);
            directives.Add(instance);

            namedMaterials.Add(name, instance);

            return instance;
        }

        public Material Material(Material.Type type)
        {
            var instance = new Material(type);
            directives.Add(instance);

            return instance;
        }

        public NamedMaterial NamedMaterial(string name)
        {
            var instance = new NamedMaterial(name);
            directives.Add(instance);

            return instance;
        }

        /*
        public MakeNamedMedium MakeNamedMedium(MakeNamedMedium.Type type)
        {
            var instance = new MakeNamedMedium(type);
            directives.Add(instance);

            return instance;
        }

        public MediumInterface MediumInterface(MediumInterface.Type type)
        {
            var instance = new MediumInterface(type);
            directives.Add(instance);

            return instance;
        }
        */

        public ObjectBegin ObjectBegin(string id)
        {
            var instance = new ObjectBegin(id);
            directives.Add(instance);

            return instance;
        }

        public ObjectEnd ObjectEnd()
        {
            var instance = new ObjectEnd();
            directives.Add(instance);

            return instance;
        }

        public ObjectInstance ObjectInstance(string id)
        {
            var instance = new ObjectInstance(id);
            directives.Add(instance);

            return instance;
        }

        public Option Option()
        {
            var instance = new Option();
            directives.Add(instance);

            return instance;
        }

        public PixelFilter PixelFilter(PixelFilter.Type type)
        {
            var instance = new PixelFilter(type);
            directives.Add(instance);

            return instance;
        }

        public ReverseOrientation ReverseOrientation()
        {
            var instance = new ReverseOrientation();
            directives.Add(instance);

            return instance;
        }

        public Rotate Rotate(float angle, float x, float y, float z)
        {
            var instance = new Rotate(angle, x, y, z);
            directives.Add(instance);

            return instance;
        }

        public Shape Shape(Shape.Type type)
        {
            var instance = new Shape(type);
            directives.Add(instance);

            return instance;
        }

        public Sampler Sampler(Sampler.Type type)
        {
            var instance = new Sampler(type);
            directives.Add(instance);

            return instance;
        }

        public Scale Scale(float x, float y, float z)
        {
            var instance = new Scale(x, y, z);
            directives.Add(instance);

            return instance;
        }

        public Translate Translate(float x, float y, float z)
        {
            var instance = new Translate(x, y, z);
            directives.Add(instance);

            return instance;
        }

        public TransformBegin TransformBegin(string id)
        {
            var instance = new TransformBegin(id);
            directives.Add(instance);

            return instance;
        }

        public TransformEnd TransformEnd()
        {
            var instance = new TransformEnd();
            directives.Add(instance);

            return instance;
        }

        public TransformTimes TransformTimes(float startTime, float endTime)
        {
            var instance = new TransformTimes(startTime, endTime);
            directives.Add(instance);

            return instance;
        }

        public Texture Texture(Texture.DataType dataType, Texture.ClassType classType, Texture.Mapping mapping, string name)
        {
            var instance = new Texture(dataType, classType, mapping, name);
            directives.Add(instance);

            textures.Add(name, instance);

            return instance;
        }

        public Transform Transform(float m00, float m01, float m02, float m03,
                               float m10, float m11, float m12, float m13,
                               float m20, float m21, float m22, float m23,
                               float m30, float m31, float m32, float m33)
        {
            var instance = new Transform(m00, m01, m02, m03,
                                         m10, m11, m12, m13,
                                         m20, m21, m22, m23,
                                         m30, m31, m32, m33);
            directives.Add(instance);

            return instance;
        }

        public WorldBegin WorldBegin()
        {
            var instance = new WorldBegin();
            directives.Add(instance);

            return instance;
        }

        public IncludeFile IncludeFile(string fileName)
        {
            var instance = new IncludeFile(fileName);
            directives.Add(instance);

            return instance;
        }

        public Custom Custom(string content)
        {
            var instance = new Custom(content);
            directives.Add(instance);

            return instance;
        }

        public string ToDirectiveString()
        {
            string str = "";

            foreach (var directive in directives)
            {
                str += directive.ToDirectiveString();
            }

            return str;
        }

        public void Save(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                string str = ToDirectiveString();

                writer.Write(str);
                writer.Flush();
                writer.Close();
            }
        }
    }

    public class RadianceRegressionTool : EditorWindow
    {
        UnityEngine.Vector3 numCameras;
        UnityEngine.Vector3 numLights;

        //[MenuItem("RadianceRegression/RadianceRegressionTool")]
        public static void ShowWindow()
        {
            GetWindow<RadianceRegressionTool>().Show();
        }


        public void DoParameterUnitTest()
        {
            Parameter<bool> bool_test1 = new Parameter<bool>(false);
            Parameter<bool> bool_test2 = new Parameter<bool>(false);

            Parameter<int> int_test1 = new Parameter<int>(1);
            Parameter<int> int_test2 = new Parameter<int>(1);

            Parameter<float> float_test1 = new Parameter<float>(1.0f);
            Parameter<float> float_test2 = new Parameter<float>(1.0f);

            Parameter<string> string_test1 = new Parameter<string>("a");
            Parameter<string> string_test2 = new Parameter<string>("a");
            Parameter<string> string_test3 = new Parameter<string>("a");

            Parameter<int> v0 = new Parameter<int>(1, 1);
            Parameter<float> v1 = new Parameter<float>(1, 1);
            Parameter<PBRT.Point2> v2 = new Parameter<PBRT.Point2>(new Point2(1.0f, 2.0f));
            Parameter<PBRT.Point3> v3 = new Parameter<PBRT.Point3>(new Point3(1.0f, 2.0f, 3.0f));
            Parameter<PBRT.Vector2> v4 = new Parameter<PBRT.Vector2>(new Vector2(1.0f, 2.0f));
            Parameter<PBRT.Vector3> v5 = new Parameter<PBRT.Vector3>(new Vector3(1.0f, 2.0f, 3.0f));
            Parameter<PBRT.Normal3> v6 = new Parameter<PBRT.Normal3>(new Normal3(1.0f, 2.0f, 3.0f));
            Parameter<PBRT.LerpSpectrum> v7 = new Parameter<PBRT.LerpSpectrum>(new PBRT.LerpSpectrum(0.1f, 0.2f, 0.3f, 0.4f));
            Parameter<PBRT.RGBSpectrum> v8 = new Parameter<PBRT.RGBSpectrum>(new PBRT.RGBSpectrum(0.1f, 0.2f, 0.3f));
            Parameter<PBRT.BlackbodySpectrum> v9 = new Parameter<PBRT.BlackbodySpectrum>(new PBRT.BlackbodySpectrum(3200.0f));
            Parameter<PBRT.FileSpectrum> v10 = new Parameter<PBRT.FileSpectrum>(new PBRT.FileSpectrum("a"));
            Parameter<bool> v11 = new Parameter<bool>(true);
            Parameter<string> v12 = new Parameter<string>("a");
        }

        public void DoDirectiveUnitTest()
        {
            PBRTScene pbrtScene = new PBRTScene();
            try
            {

                var accelerator = pbrtScene.Accelerator();
                var attributeBegin = pbrtScene.AttributeBegin();
                var attributeEnd = pbrtScene.AttributeEnd();
                var attribute0 = pbrtScene.Attribute(PBRT.Attribute.Type.Shape);

                attribute0.AddParameter("radius1", 5.0f);
                attribute0.AddParameter("radius", 5.0f);

                attribute0.SetParameter("radius1", 5.0f);


                attribute0.SetParameter("radius", 5.0f);
                var attribute1 = pbrtScene.Attribute(PBRT.Attribute.Type.Light);
                var attribute2 = pbrtScene.Attribute(PBRT.Attribute.Type.Material);
                var attribute3 = pbrtScene.Attribute(PBRT.Attribute.Type.Medium);
                var attribute4 = pbrtScene.Attribute(PBRT.Attribute.Type.Texture);


                var areaLightSource0 = pbrtScene.AreaLightSource(PBRT.AreaLightSource.Type.Diffuse);
                var activeTransform0 = pbrtScene.ActiveTransform(PBRT.ActiveTransform.Type.StartTime);
                var activeTransform1 = pbrtScene.ActiveTransform(PBRT.ActiveTransform.Type.EndTime);
                var activeTransform2 = pbrtScene.ActiveTransform(PBRT.ActiveTransform.Type.All);

                var colorSpace0 = pbrtScene.ColorSpace(PBRT.ColorSpace.Type.ACES2065_1);
                var colorSpace1 = pbrtScene.ColorSpace(PBRT.ColorSpace.Type.REC2020);
                var colorSpace2 = pbrtScene.ColorSpace(PBRT.ColorSpace.Type.DCI_P3);
                var colorSpace3 = pbrtScene.ColorSpace(PBRT.ColorSpace.Type.SRGB);

                var camera0 = pbrtScene.Camera(PBRT.Camera.Type.Orthographic);
                var camera1 = pbrtScene.Camera(PBRT.Camera.Type.Perspective);
                var camera2 = pbrtScene.Camera(PBRT.Camera.Type.Realistic);
                var camera3 = pbrtScene.Camera(PBRT.Camera.Type.Spherical);

                var coordinateSystem0 = pbrtScene.CoordinateSystem(PBRT.CoordinateSystem.Type.World);
                var coordinateSystem1 = pbrtScene.CoordinateSystem(PBRT.CoordinateSystem.Type.Camera);

                var coordSysTransform0 = pbrtScene.CoordSysTransform(PBRT.CoordSysTransform.Type.World);
                var coordSysTransform1 = pbrtScene.CoordSysTransform(PBRT.CoordSysTransform.Type.Camera);
                var concatTransform = pbrtScene.ConcatTransform(11.0f, 12.0f, 13.0f, 14.0f,
                                                          21.0f, 22.0f, 23.0f, 24.0f,
                                                          31.0f, 32.0f, 33.0f, 34.0f,
                                                          41.0f, 42.0f, 43.0f, 44.0f);
                var film0 = pbrtScene.Film(PBRT.Film.Type.RGB);
                var film1 = pbrtScene.Film(PBRT.Film.Type.GBuffer);
                var film2 = pbrtScene.Film(PBRT.Film.Type.Spectral);

                var include = pbrtScene.Include("1");
                var import = pbrtScene.Import("2");
                var identity = pbrtScene.Identity();
                var integrator0 = pbrtScene.Integrator(PBRT.Integrator.Type.Ambientocclusion);
                var integrator1 = pbrtScene.Integrator(PBRT.Integrator.Type.BDPT);
                var integrator2 = pbrtScene.Integrator(PBRT.Integrator.Type.Lightpath);
                var integrator3 = pbrtScene.Integrator(PBRT.Integrator.Type.Mlt);
                var integrator4 = pbrtScene.Integrator(PBRT.Integrator.Type.Path);
                var integrator5 = pbrtScene.Integrator(PBRT.Integrator.Type.Randomwalk);
                var integrator6 = pbrtScene.Integrator(PBRT.Integrator.Type.Simplepath);
                var integrator7 = pbrtScene.Integrator(PBRT.Integrator.Type.Simplevolpath);
                var integrator8 = pbrtScene.Integrator(PBRT.Integrator.Type.Sppm);
                var integrator9 = pbrtScene.Integrator(PBRT.Integrator.Type.Volpath);

                var light0 = pbrtScene.Light(PBRT.Light.Type.Distant);
                var light1 = pbrtScene.Light(PBRT.Light.Type.Goniometric);
                var light2 = pbrtScene.Light(PBRT.Light.Type.Infinite);
                var light3 = pbrtScene.Light(PBRT.Light.Type.Point);
                var light4 = pbrtScene.Light(PBRT.Light.Type.Projection);
                var light5 = pbrtScene.Light(PBRT.Light.Type.Spot);

                var lightSource = pbrtScene.LightSource(PBRT.LightSource.Type.Infinite);
                var lookAt = pbrtScene.LookAt(11.0f, 12.0f, 13.0f,
                                        21.0f, 22.0f, 23.0f,
                                        31.0f, 32.0f, 33.0f);
                var transform = pbrtScene.Transform(11.0f, 12.0f, 13.0f, 14.0f,
                                                          21.0f, 22.0f, 23.0f, 24.0f,
                                                          31.0f, 32.0f, 33.0f, 34.0f,
                                                          41.0f, 42.0f, 43.0f, 44.0f);
                //var makeNamedMedium = pbrtScene.MakeNamedMedium(PBRT.MakeNamedMedium.Type.Diffuse);
                //var mediumInterface = pbrtScene.MediumInterface(PBRT.MediumInterface.Type.Diffuse);
                var objectBegin = pbrtScene.ObjectBegin("asd");
                var objectEnd = pbrtScene.ObjectEnd();
                var objectInstance = pbrtScene.ObjectInstance("asd");
                var option = pbrtScene.Option();
                option.SetParameter("disablepixeljitter", true);
                var pixelFilter0 = pbrtScene.PixelFilter(PBRT.PixelFilter.Type.Box);
                var pixelFilter1 = pbrtScene.PixelFilter(PBRT.PixelFilter.Type.Gussian);
                var pixelFilter2 = pbrtScene.PixelFilter(PBRT.PixelFilter.Type.Mitchell);
                var pixelFilter3 = pbrtScene.PixelFilter(PBRT.PixelFilter.Type.Sinc);
                var pixelFilter4 = pbrtScene.PixelFilter(PBRT.PixelFilter.Type.Triangle);
                var reverseOrientation = pbrtScene.ReverseOrientation();
                var rotate = pbrtScene.Rotate(0.0f, 1.0f, 2.0f, 3.0f);
                var shape0 = pbrtScene.Shape(PBRT.Shape.Type.BilinearMesh);
                var shape1 = pbrtScene.Shape(PBRT.Shape.Type.Curve);
                var shape2 = pbrtScene.Shape(PBRT.Shape.Type.Cylinder);
                var shape3 = pbrtScene.Shape(PBRT.Shape.Type.Disk);
                var shape4 = pbrtScene.Shape(PBRT.Shape.Type.Sphere);
                var shape5 = pbrtScene.Shape(PBRT.Shape.Type.TriangleMesh);
                var shape7 = pbrtScene.Shape(PBRT.Shape.Type.LoopSubdiv);
                var shape8 = pbrtScene.Shape(PBRT.Shape.Type.PLYMesh);

                var sampler0 = pbrtScene.Sampler(PBRT.Sampler.Type.Halton);
                var sampler1 = pbrtScene.Sampler(PBRT.Sampler.Type.Independent);
                var sampler2 = pbrtScene.Sampler(PBRT.Sampler.Type.PaddedSobol);
                var sampler3 = pbrtScene.Sampler(PBRT.Sampler.Type.Sobol);
                var sampler4 = pbrtScene.Sampler(PBRT.Sampler.Type.Stratified);
                var sampler5 = pbrtScene.Sampler(PBRT.Sampler.Type.ZSobol);

                var scale = pbrtScene.Scale(1.0f, 2.0f, 3.0f);
                var translate = pbrtScene.Translate(1.0f, 2.0f, 3.0f);
                var transformBegin = pbrtScene.TransformBegin("test");
                var transformEnd = pbrtScene.TransformEnd();
                var transformTimes = pbrtScene.TransformTimes(1.0f, 2.0f);

                // Texture.DataType dataType = Texture.DataType.Float;
                Texture.DataType dataType = Texture.DataType.Spectrum;

                Texture.Mapping mapping = Texture.Mapping.UV;
                //Texture.Mapping mapping = Texture.Mapping.Spherical;
                //Texture.Mapping mapping = Texture.Mapping.Cylindrical;
                //Texture.Mapping mapping = Texture.Mapping.Planar;
                var textureFloatBilerp = pbrtScene.Texture(dataType, Texture.ClassType.Bilerp, mapping, "textureFloatBilerp");
                var textureFloatCheckerBoard = pbrtScene.Texture(dataType, Texture.ClassType.CheckerBoard, mapping, "textureFloatCheckerBoard");
                var textureFloatConstant = pbrtScene.Texture(dataType, Texture.ClassType.Constant, mapping, "textureFloatConstant");
                var textureFloatDirectionMix = pbrtScene.Texture(dataType, Texture.ClassType.DirectionMix, mapping, "textureFloatDirectionMix");
                var textureFloatDots = pbrtScene.Texture(dataType, Texture.ClassType.Dots, mapping, "textureFloatDots");
                var textureFloatFBM = pbrtScene.Texture(dataType, Texture.ClassType.FBM, mapping, "textureFloatFBM");
                var textureFloatImageMap = pbrtScene.Texture(dataType, Texture.ClassType.ImageMap, mapping, "textureFloatImageMap");
                var textureFloatMarble = pbrtScene.Texture(dataType, Texture.ClassType.Marble, mapping, "textureFloatMarble");
                var textureFloatMix = pbrtScene.Texture(dataType, Texture.ClassType.Mix, mapping, "textureFloatMix");
                var textureFloatPtex = pbrtScene.Texture(dataType, Texture.ClassType.Ptex, mapping, "textureFloatPtex");
                var textureFloatScale = pbrtScene.Texture(dataType, Texture.ClassType.Scale, mapping, "textureFloatScale");
                var textureFloatWindy = pbrtScene.Texture(dataType, Texture.ClassType.Windy, mapping, "textureFloatWindy");
                var textureFloatWrinkled = pbrtScene.Texture(dataType, Texture.ClassType.Wrinkled, mapping, "textureFloatWrinkled");

                Material.Type materialType = Material.Type.CoatedDiffuse;
                // Material.Type materialType = Material.Type.CoatedConductor;
                // Material.Type materialType = Material.Type.Conductor;
                // Material.Type materialType = Material.Type.Dielectric;
                // Material.Type materialType = Material.Type.Diffuse;
                // Material.Type materialType = Material.Type.DiffuseTransmission;
                // Material.Type materialType = Material.Type.Hair;
                // Material.Type materialType = Material.Type.Interface;
                // Material.Type materialType = Material.Type.Measured;
                // Material.Type materialType = Material.Type.Mix;
                // Material.Type materialType = Material.Type.SubSurface;
                // Material.Type materialType = Material.Type.ThinDielectric;
                var testMaterial = pbrtScene.MakeNamedMaterial(materialType, "testMaterial");
                testMaterial.SetParameter("displacement", textureFloatWindy);
                testMaterial.SetParameter("displacement", pbrtScene.GetTexture("textureFloatWindy"));
                testMaterial.SetParameter("roughness", textureFloatWindy);
                testMaterial.SetParameter("uroughness", textureFloatWindy);
                testMaterial.SetParameter("vroughness", textureFloatWindy);
                testMaterial.SetParameter("albedo", textureFloatBilerp);
                testMaterial.SetParameter("albedo", 0.0f);
                testMaterial.SetParameter("g", textureFloatBilerp);
                testMaterial.SetParameter("reflectance", 1.0f);
                testMaterial.SetParameter("reflectance", textureFloatBilerp);

                var testMaterial2 = pbrtScene.MakeNamedMaterial(materialType, "testMaterial2");
                testMaterial2.SetParameter("displacement", textureFloatWindy);
                testMaterial2.SetParameter("roughness", textureFloatWindy);
                testMaterial2.SetParameter("uroughness", textureFloatWindy);
                testMaterial2.SetParameter("vroughness", textureFloatWindy);
                testMaterial2.SetParameter("albedo", textureFloatBilerp);
                testMaterial2.SetParameter("albedo", 0.0f);
                testMaterial2.SetParameter("g", textureFloatBilerp);
                testMaterial2.SetParameter("reflectance", 1.0f);
                testMaterial2.SetParameter("reflectance", textureFloatBilerp);

                
                MakeNamedMaterial someMaterialInScene = pbrtScene.GetNamedMaterial("testMaterial");
                var namedMaterial = pbrtScene.NamedMaterial(someMaterialInScene.GetName());
                
                var material = pbrtScene.Material((Material.Type)materialType);

                var worldBegin = pbrtScene.WorldBegin();

                var str = pbrtScene.ToDirectiveString();
                Debug.Log(str);

                pbrtScene.Save(@"C:\test.pbrt");
            }
            catch (KeyNotFoundException e)
            {
                Debug.Log("KeyNotFoundException");
            }
            finally
            {
            }
        }

        private void OnGUI()
        {
            numCameras = EditorGUILayout.Vector3Field("Cameras", numCameras);
            numLights = EditorGUILayout.Vector3Field("Lights", numLights);

            if (GUILayout.Button("Export"))
            {
                Variant varint1 = 2;
                Variant varint2 = 1;
                Variant varint3 = varint2;
                Variant varfloat = 1.1f;
                int a = varfloat;
                float b = varfloat;

                varfloat = "a";
                Variant varPoint2 = new Variant(new Point2(1.1f, 1.1f));
                Variant varPoint3 = new Variant(new Point3(1.1f, 1.1f, 1.1f));
                Variant varVector2 = new Variant(new Vector2(1.1f, 1.1f));
                Variant varVector3 = new Variant(new Vector3(1.1f, 1.1f, 1.1f));
                Variant varNormal3 = new Variant(new Normal3(1.1f, 1.1f, 1.1f));
                Variant varLerpSpectrum = new Variant(new LerpSpectrum(1.1f, 1.1f, 1.1f, 1.1f));
                Variant varRGBSpectrum = new Variant(new RGBSpectrum(1.1f, 1.1f, 1.1f));
                Variant varBlackbodySpectrum = new Variant(new BlackbodySpectrum(1.1f));
                Variant varbool = true;
                Variant varstring = "";

                Variant varfloatstring1 = new Variant(Texture.DataType.Float);
                System.Type aa = varfloatstring1.GetType();

                //DoParameterUnitTest();
                DoDirectiveUnitTest();
            }
        }
    }
};