using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FolderProcces
{
    class Program
    {

        static void Main(string[] args)
        {
            var selectMenu = "";

            Console.WriteLine("1. ---- Crear archivos de lectura ----");

            Console.WriteLine("2. ---- Proceso de Orden ----");

            Console.WriteLine("3. Crear txt con archivos no deseados");

            selectMenu = Console.ReadLine();

            switch (selectMenu)
            {
                case "1":

                    writeMainProcces();
                    Console.WriteLine("¿Deseas escribir otro archivo? Y / N");
                    var continuar = Console.ReadLine();
                    while (!continuar.Equals("N"))
                    {
                        writeMainProcces();
                        Console.WriteLine("¿Deseas escribir otro archivo? Y / N");
                        continuar = Console.ReadLine();
                    }
                    break;
                case "2":
                    OrderedParallelQuery();

                    Console.WriteLine("¿Deseas acomodar mas archivos? Y / N");
                    var continuarOrder = Console.ReadLine();

                    while (!continuarOrder.Equals("N"))
                    {
                        OrderedParallelQuery();
                        Console.WriteLine("¿Deseas acomodar mas archivos? Y / N");
                        continuarOrder = Console.ReadLine();
                    }

                    Console.WriteLine("El proceso a finalizado");
                    System.Console.ReadLine();

                    break;
                case "3":
                    CreatTxtWithFilesDontWant();

                    break;
                default:
                    break;
            }


            
            
            
           

        }

        public static void CreatTxtWithFilesDontWant() {
            Console.WriteLine("Escriba la carpeta de origen");

            var sourceFolder = System.Console.ReadLine();

            // Obtiene archvos

            string pathRead = string.Format(@"{0}", sourceFolder);
            var nameFile = "DeleteFiles";

            var pathFileTxt = string.Format(@"{0}\{1}.txt", pathRead, nameFile);

            writeFileName(pathRead, pathFileTxt);


            // Crea txt con el nombre de los archivos leidos


        }

        public static void OrderedParallelQuery()
        {

            Console.WriteLine("Escriba la carpeta de origen");

            var sourceFolder = System.Console.ReadLine();


            try
            {
                var txtFiles = Directory.EnumerateFiles(sourceFolder, "*.txt");
                foreach (string currentFile in txtFiles)
                {
                    orderFiles(sourceFolder, currentFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
            }
            
        }

        public static void writeMainProcces() 
        {
            Console.WriteLine("1. ---- Proceso de Escritura ----");

            Console.WriteLine("Escribe la direccion de la carpeta");

            var path = Console.ReadLine();
            string pathRead = string.Format(@"{0}", path);
            var nameFile = Path.GetFileName(pathRead);

            var pathFileTxt = string.Format(@"{0}\{1}.txt", pathRead, nameFile);


            writeFileFolder(pathRead,
                            pathFileTxt);

        }


        public static void writeFileFolder(string pathRead, string pathWrite)
        {
            Console.WriteLine("Iniciando prooceso");

            if (!Directory.Exists(pathRead))
            {
                Console.WriteLine(string.Format("La direccion {0} no existe", pathRead));
            }
            string[] dirs = Directory.GetDirectories(pathRead);
            List<string> singleDirNames = dirs.Select(x => Path.GetFileName(x)).ToList();

            Console.WriteLine(string.Format("se encontraron {0} folders", singleDirNames.Count()));

            File.WriteAllLines(pathWrite, singleDirNames);

        }

        public static void writeFileName(string pathRead, string pathWrite)
        {
            Console.WriteLine("Iniciando prooceso de escritura txt de archivos no deseados");

            string line;


            if (!Directory.Exists(pathRead))
            {
                Console.WriteLine(string.Format("La direccion {0} no existe", pathRead));
            }

            // Lee Archivo delelete.txt existente 

            var txtFiles = Directory.EnumerateFiles(pathRead, "*.txt").FirstOrDefault();

            var pathFiletxt = System.IO.Path.Combine(pathRead);


            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(string.Format(@"{0}", pathFiletxt));

            var dirRead = new List<string>();

            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                dirRead.Add(line);
            }

            file.Close();

            // Lee archivos existentes en la carpeta

            string[] files = Directory.GetFiles(pathRead);

            List<string> singleFilesNames = files.Select(x => Path.GetFileName(x)).ToList();

            if (files.Length > 0)
            {
                Console.WriteLine(string.Format("se encontraron {0} archivos", singleFilesNames.Count()));

                File.WriteAllLines(pathWrite, singleFilesNames);
            }
            else
            {
                Console.WriteLine("La carpeta esta vacia");

                Console.ReadLine();
            }


        }

        public static void orderFiles(string sourceFolder, string sourceFile)
        {

            int counter = 0;
            string line;

            var pathFiletxt = System.IO.Path.Combine(sourceFile);


            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(string.Format(@"{0}", pathFiletxt));

            var dirRead = new List<string>();

            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                dirRead.Add(line);
                counter++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);

            //Crea carpeta con nombre del archivo
            var newDestination = System.IO.Path.Combine(sourceFolder, Path.ChangeExtension(pathFiletxt, null));

            if (!Directory.Exists(sourceFolder))
            {
                System.IO.Directory.CreateDirectory(sourceFolder);
            }

            Console.WriteLine(string.Format( "Acomodando Archvos de {0} en subcarpetas", pathFiletxt));

            foreach (var phFolder in dirRead)
            {

                string[] files = Directory.GetFiles(sourceFolder, string.Format("*{0}*", phFolder), SearchOption.TopDirectoryOnly);

                if (files.Length > 0)
                {
                    foreach (var fileMove in files)
                    {
                        string pathString = System.IO.Path.Combine(newDestination, phFolder);
                        string pathFile = string.Format(@"{0}", fileMove);
                        string fileName = Path.GetFileName(fileMove);
                        string destinationdirectory = System.IO.Path.Combine(string.Format(@"{0}", pathString), fileName);

                        try
                        {
                            if (!Directory.Exists(pathString))
                            {
                                System.IO.Directory.CreateDirectory(pathString);
                            }

                            //System.IO.File.Copy(pathFile, destinationdirectory);
                            System.IO.File.Move(pathFile, destinationdirectory,true);
                            //System.IO.File.Delete(pathFile);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        System.Console.WriteLine(fileName);
                        counter--;

                    }

                }
            }
        }
    }

}
