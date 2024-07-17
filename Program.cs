using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ImagenCopiadora
{
  public static void CopiarImagen(string rutaOrigen, string rutaDestino, string nombreArchivo)
  {
    // Copiar el archivo desde la ruta de origen a la ubicación deseada
    File.Copy(rutaOrigen, rutaDestino, true);
    Console.WriteLine($"Copiada: {nombreArchivo}");
  }

  public static void Main()
  {
    // Obtener lista de archivos en el directorio local_Images
    string[] archivos = Directory.GetFiles("local_Images");
    List<Task> tareas = new List<Task>();
    List<string> nombresArchivos = new List<string>();

    foreach (string archivo in archivos)
    {
      // Procesar solo archivos de imagen
      if (archivo.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
          archivo.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
          archivo.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
          archivo.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
      {
        string rutaDestino = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(archivo));
        string nombreArchivo = Path.GetFileName(archivo);

        // Crear una tarea para cada copia de imagen
        Task tarea = Task.Run(() => CopiarImagen(archivo, rutaDestino, nombreArchivo));
        tareas.Add(tarea);
        nombresArchivos.Add(nombreArchivo);
      }
    }

    // Esperar a que todas las tareas terminen
    Task.WaitAll(tareas.ToArray());

    // Registrar los nombres de los archivos copiados en un archivo JSON
    File.WriteAllText("descargas.json", JsonConvert.SerializeObject(nombresArchivos, Formatting.Indented));
  }
}
