using System.IO;
using System.Xml.Serialization;

namespace SowUtilities.Serialization
{
    public static class Serializer
    {

        public static void SaveToXml<T>(T objectToSerialize, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                serializer.Serialize(fileStream, objectToSerialize);
            }
        }

        public static object LoadToXml<T>(string fileName)
        {
            object output;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using(FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                output = serializer.Deserialize(fileStream);
            }

            return output;
        }

    }
}
