using System;
using System.IO;
using System.Xml.Serialization;

public static class PersistenciaXML{
    public static void Guardar<T>(string rutaArchivo, T objeto)
    {
        XmlSerializer serializador = new(typeof(T));

        using (StreamWriter escritor = new(rutaArchivo))
        {
            serializador.Serialize(escritor, objeto);
        }
    }

    public static T Cargar<T>(string rutaArchivo)
    {
        if (!File.Exists(rutaArchivo))
        {
            throw new FileNotFoundException();
        }

        XmlSerializer serializador = new(typeof(T));

        using (StreamReader lector = new(rutaArchivo))
        {
            var resultado = serializador.Deserialize(lector) ?? throw new Exception();

            return (T) resultado;
        }
    }
}