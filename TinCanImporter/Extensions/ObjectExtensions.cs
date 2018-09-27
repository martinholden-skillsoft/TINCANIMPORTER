using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TinCanImporter.Extensions
{
    public static class ObjectExtensions
    {
        public static T CloneObject<T>(this object source)
        {
            T result = Activator.CreateInstance<T>();
            //// **** made things  
            return result;
        }

        public static T CloneObjectSerializable<T>(this T obj) where T : class
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);
            ms.Position = 0;
            object result = bf.Deserialize(ms);
            ms.Close();
            return (T)result;
        }
    }
}
