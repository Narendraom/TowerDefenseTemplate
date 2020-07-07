using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading;

public static class ByteSerializer
{
    public static byte[] Serialize<T>(this T m)
    {
        using (var ms = new MemoryStream())
        {
            new BinaryFormatter().Serialize(ms, m);
            return ms.ToArray();
        }
    }

    public static T Deserialize<T>(this byte[] byteArray)
    {
        using (var ms = new MemoryStream(byteArray))
        {
            return (T)new BinaryFormatter().Deserialize(ms);
        }
    }
}

public static class XMLSerializer
{
    public static void Save<T>(string FileName, T targetObject)
    {
        using (var writer = new System.IO.StreamWriter(FileName))
        {
			//Debug.Log ("Xml = "+targetObject);
            var serializer = new XmlSerializer(targetObject.GetType());
            serializer.Serialize(writer, targetObject);
            writer.Flush();
        }
    }

    public static T Load<T>(string FileName)
    {
        using (var stream = System.IO.File.OpenRead(FileName))
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
        
    }
}

public class Alpha
{
    public void Beta()
    {
        while (true)
        {
            Debug.Log("running thread..");
        }
    }
}

