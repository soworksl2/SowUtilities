using System.IO;
using System.Xml.Serialization;

namespace SowUtilities.Serialization
{
    /// <summary>
    /// Proporciona metodos estaticos para la serializacion y deserializacion con Xml
    /// </summary>
    public static class Xml
    {
        /// <summary>
        /// Serializa el objeto especificado en la ruta especificada.
        /// </summary>
        /// <typeparam name="T">El tipo del objeto a serializar</typeparam>
        /// <param name="objectToSerialize">Objeto que se va a serializar</param>
        /// <param name="fileName">El nombre completo del archivo donde se serializara el objeto</param>
        public static void Serialize<T>(T objectToSerialize, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                serializer.Serialize(fileStream, objectToSerialize);
        }
        /// <summary>
        /// Serializa un objeto y lo devuelve en un string
        /// </summary>
        /// <typeparam name="T">El tipo del objeto que se serializara</typeparam>
        /// <param name="objectToSerialize">el objeto que se desea serializar</param>
        /// <returns>El objeto serializado</returns>
        public static string Serialize<T>(T objectToSerialize)
        {
            string output;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, objectToSerialize);
                output = stringWriter.ToString();
            }

            return output;
        }
        /// <summary>
        /// Deserializa un objeto
        /// </summary>
        /// <typeparam name="T">El tipo del objeto a deserializar</typeparam>
        /// <param name="fileName">El nombre completo donde se encuentra el archivo del objeto serializado</param>
        /// <returns>El objeto deseriaizado</returns>
        public static T Deserialize<T>(string fileName)
        {
            T output;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                output = (T)serializer.Deserialize(stream);

            return output;
        }
        /// <summary>
        /// Deserializa un objeto xml
        /// </summary>
        /// <typeparam name="T">El tipo del objeto que se deserializara</typeparam>
        /// <param name="xmlObject">La cadena de texto que representa el objeto serializado mediante xml</param>
        /// <returns>El objeto deserializado</returns>
        public static T DeserializeString<T>(string xmlObject)
        {
            T output;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xmlObject))
                output = (T)serializer.Deserialize(reader);

            return output;
        }
    }
}
