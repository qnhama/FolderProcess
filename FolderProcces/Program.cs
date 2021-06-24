﻿using System;
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

            Console.WriteLine("1. ---- Crear archivo txt con nombre de folders ----");

            Console.WriteLine("2. ---- Proceso de Orden ----");

            Console.WriteLine("3. Crear txt con nombre de archivos");

            Console.WriteLine("4. Elimina archivos no deseados que se encuentre en el txt Delete archivos");

            Console.WriteLine("5. Crea txt con el nombre de los canales de los archivos en un txt");


            selectMenu = Console.ReadLine();

            switch (selectMenu)
            {
                case "1":

                    writeTxtWithFolderNames();
                    Console.WriteLine("¿Deseas escribir otro archivo? Y / N");
                    var continuar = Console.ReadLine();
                    while (!continuar.Equals("N"))
                    {
                        writeTxtWithFolderNames();
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
                    CreatTxtWithNameFiles();

                    break;
                case "4":
                    DeleteFilesFromTxtFilesDontWant();

                    break;
                case "5":
                    Console.WriteLine("Escriba la carpeta de origen");

                    
                    CreateTxtNameChanelFromTxtFilesDontWant();
                    break;
                default:
                    break;
            }

        }

        #region Menu Flow

        // ------ Meunu Select 1 Options ----------

        public static void writeTxtWithFolderNames()
        {
            Console.WriteLine("1. ---- Proceso de Escritura txt con nombre de folders----");

            Console.WriteLine("Escribe la direccion de la carpeta");

            var path = Console.ReadLine();
            string pathRead = string.Format(@"{0}", path);
            var nameFile = Path.GetFileName(pathRead);

            var pathFileTxt = string.Format(@"{0}\{1}.txt", pathRead, nameFile);

            writeFileTxtFromFolders(pathRead,
                            pathFileTxt);

        }


        // ------ Meunu Select 2 Options ----------


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

        // ------ Meunu Select 3 Options ----------


        public static void CreatTxtWithNameFiles()
        {
            Console.WriteLine("Escriba la carpeta de origen");

            var sourceFolder = System.Console.ReadLine();

            // Obtiene archvos

            string pathRead = string.Format(@"{0}", sourceFolder);
           
            // Crea txt con el nombre de los archivos leidos

            Console.WriteLine("Iniciando prooceso de escritura txt de archivos no deseados");

            var pathFileTxt = Directory.GetFiles(pathRead, "*.txt", SearchOption.TopDirectoryOnly).First();



            writeFileName(pathRead, pathFileTxt);

        }


        // ------ Meunu Select 4 Options ----------


        /// <summary>
        /// mediante un txt es posible gaurdar registro de los archivos eliminados
        /// </summary>
        public static void DeleteFilesFromTxtFilesDontWant()
        {
            Console.WriteLine("Escriba la carpeta de origen");

            var sourceFolder = System.Console.ReadLine();


            try
            {
                string pathRead = string.Format(@"{0}", sourceFolder);
                var pathFileTxt = Directory.GetFiles(pathRead, "*.txt", SearchOption.TopDirectoryOnly).First();


                DeleteFiles(sourceFolder, pathFileTxt);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
            }

        }

        // ------ Meunu Select 5 Options ----------

        /// <summary>
        /// Cada archivo contiene una clave que corresponde al nombre del canal al que pertenece
        /// </summary>
        public static void CreateTxtNameChanelFromTxtFilesDontWant()
        {
            // lee archivo txt

            var sourceFolder = System.Console.ReadLine();

            // Obtiene archvos

            string pathRead = string.Format(@"{0}", sourceFolder);
            var nameFile = "DeleteFiles";

            var pathDeleteFileTxt = string.Format(@"{0}\{1}.txt", pathRead, nameFile);


            List<string> dirRead = new List<string>();

            try
            {
                dirRead = readFile(pathDeleteFileTxt);

                if (dirRead.Count == 0)
                {
                    Console.WriteLine("La lista de nombres del txt esta vacia");
                }
                else
                {
                    // Extrae el nombre del canal de cada archivo leido

                    // Tag de busqueda del nombre del archivo
                    var tag = "_";

                    var chanelsNames = getChanelNames(dirRead, tag, tag);

                    // Escribe archivo txt

                    string pathReadChanelsNames = string.Format(@"{0}", pathRead);
                    nameFile = "ChanelsNames";

                    var pathFileTxt = string.Format(@"{0}\{1}.txt", pathReadChanelsNames, nameFile);

                    if (!File.Exists(pathFileTxt))
                    {
                        using (StreamWriter sw = File.CreateText(pathFileTxt))
                        {
                        }
                        File.WriteAllLines(pathFileTxt, chanelsNames);

                    }
                    else
                    {
                        //lee archivo ya existente


                        var txtFiles = from file in Directory.EnumerateFiles(sourceFolder, "*.txt")
                            where !file.ToLower().Contains(nameFile)
                            select file;

                        var chanelsNamesReadFiles = readFile(pathFileTxt);

                        foreach (string currentFile in txtFiles)
                        {

                            //function call to get the filename
                            var filename = Path.GetFileName(currentFile);

                            if (filename.Equals(nameFile))
                            {
                                var chanelsNamesResult = chanelsNamesReadFiles.Distinct().Except(chanelsNames.Distinct()).ToList();

                                if (chanelsNamesResult.Count() > 0)
                                {
                                    // guarda resultado en el mismo txt

                                    File.AppendAllLines(pathFileTxt, chanelsNamesResult);
                                }
                            }
                            else
                            {
                                var ReadFiles = readFile(pathFileTxt);

                                // Si en la lista de chanels leidos existen en la lista de los archvos que si son utiles entonces los elimina de la lista de canales no son utiles

                                var result = chanelsNames.Distinct().Except(ReadFiles.Distinct()).ToList();

                                if (result.Count() > 0)
                                {
                                    // guarda resultado en el mismo txt
                                    foreach (var ok in result)
                                    {
                                        chanelsNames.Remove(ok);
                                    }

                                    File.AppendAllLines(pathFileTxt, chanelsNames);
                                }

                            }

                           

                        }

                      
                    }

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
            }


        }

        #endregion


        #region Tools

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtReadPath"></param>
        /// <returns></returns>
        public static List<string> readFile(string txtReadPath)
        {
            var dirRead = new List<string>();

            try
            {

                if (!File.Exists(txtReadPath))
                {
                    Console.WriteLine(string.Format("La direccion {0} no existe", txtReadPath));
                }

                int counter = 0;
                string line;

                var pathFiletxt = System.IO.Path.Combine(txtReadPath);

                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(string.Format(@"{0}", pathFiletxt));


                while ((line = file.ReadLine()) != null)
                {
                    System.Console.WriteLine(line);
                    dirRead.Add(line);
                    counter++;
                }


                file.Close();

                return dirRead;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return dirRead;
            }
        }


        /// <summary>
        /// Elimina Archivos de una carpeta de acuerdo a un archivo txt si existe en el archivo se elinina
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="sourceFile"></param>
        public static void DeleteFiles(string sourceFolder, string sourceFile)
        {
            try
            {
                var dirReads = readFile(sourceFile);

                if (dirReads.Count == 0)
                {
                    Console.WriteLine("La lista de nombres del txt esta vacia");
                }


                System.Console.WriteLine("Existe  {0} registros.", dirReads.Count());


                Console.WriteLine(string.Format("Buacando Archvos en la carpeta {0}", sourceFolder));


                List<string> files = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories).ToList();



                files = dirReads.Where(t2 => files.Any(t1 => t2.Contains(t1))).ToList();
                Console.WriteLine(string.Format("Se encontraron {0}", files.Count()));


                Console.WriteLine(string.Format("¿Estas seguro que quieres eliminar {0} archivos?", files.Count()));

                Console.WriteLine("s/n");

                var confirmDelete = Console.ReadLine();

                if (confirmDelete.Equals("s"))
                {
                    foreach (var dileDelete in files)
                    {


                        try
                        {

                            //System.IO.File.Copy(pathFile, destinationdirectory);
                            System.IO.File.Delete(dileDelete);
                            //System.IO.File.Delete(pathFile);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.InnerException);
                        }
                        System.Console.WriteLine(dileDelete);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                
            }
        }

        /// <summary>
        /// De acuardo a un txt se crea otro mas con los nombres de los canales a cual pertenese de acuardo a tags open y close
        /// </summary>
        /// <param name="dirRead"></param>
        /// <param name="tagOpen"></param>
        /// <param name="tagClose"></param>
        /// <returns></returns>
        public static List<string> getChanelNames(List<string> dirRead, string tagOpen, string tagClose) 
        {
            List<string> chanelNameFiles = new List<string>();

            try
            {

                foreach (var fileName in dirRead)
                {

                    // El nombre del canal siempre esta en la parte final al abrir y cerrar con el tag

                    var endTagPosition = fileName.LastIndexOf(tagClose);

                    if (endTagPosition == -1)
                    {
                        Console.WriteLine(string.Format("No es un archivo con tagClose {0}", fileName));
                    }
                    else
                    {
                        // Obtenemos un substring para encontrar la siguiente ultima posición que corresponde a la posicion del tag de inicio del nombre del archivo

                        var subFilename = fileName.Substring(0, endTagPosition);

                        if (string.IsNullOrWhiteSpace(subFilename))
                        {
                            Console.WriteLine(string.Format("No fue posible realizar Substring {0}", fileName));
                        }

                        var startTagPosition = subFilename.LastIndexOf(tagOpen);

                        if (startTagPosition == -1)
                        {
                            Console.WriteLine(string.Format("No es un archivo con tagStart {0}", fileName));
                        }

                        var lenghtChaneleName = endTagPosition - (startTagPosition + 1);

                        var nameChanelFile = fileName.Substring(startTagPosition + 1, lenghtChaneleName);

                        chanelNameFiles.Add(nameChanelFile);
                    }

                    
                }

                return chanelNameFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return chanelNameFiles;
            }

            
        }

        /// <summary>
        /// Escribe un archivo txt con los nombres de archvios de la carpeta indicada
        /// </summary>
        /// <param name="pathRead"></param>
        /// <param name="pathWrite"></param>
        public static void writeFileTxtFromFolders(string pathRead, string pathWrite)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            
        }

        /// <summary>
        /// Escribe sobre un txt existente los archivos de una carpeta
        /// </summary>
        /// <param name="pathRead"></param>
        /// <param name="pathWrite"></param>
        public static void writeFileName(string pathRead, string pathWrite)
        {
            try
            {


                var dirRead = readFile(pathWrite);

                // Lee archivos existentes en la carpeta

                /// ---------------- Revisar codigo

                List<string> files = Directory.GetFiles(pathRead, "*",SearchOption.AllDirectories).Where(f => !f.EndsWith(".txt")).ToList();

                /// ---------------- Revisar codigo

                List<string> singleFilesNames = files.Select(x => Path.GetFileName(x)).ToList();

                if (files.Count() > 0)
                {
                    Console.WriteLine(string.Format("se encontraron {0} archivos", singleFilesNames.Count()));

                    // Compara las listas y encientras las diferencias 

                    var result = singleFilesNames.Except(dirRead).ToList();

                    // guarda resultado en el mismo txt

                    File.AppendAllLines(pathWrite, result);

                }
                else
                {
                    Console.WriteLine("La carpeta esta vacia");

                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            
        }



        /// <summary>
        /// Ordena los archivos de acuerdo a la informacion de un txtx
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="sourceFile"></param>

        public static void orderFiles(string sourceFolder, string sourceFile)
        {
            try
            {
                var pathFiletxt = System.IO.Path.Combine(sourceFile);


                var dirRead = readFile(sourceFile);


                System.Console.WriteLine("There were {0} lines.", dirRead.Count);

                //Crea carpeta con nombre del archivo
                var newDestination = System.IO.Path.Combine(sourceFolder, Path.ChangeExtension(pathFiletxt, null));

                if (!Directory.Exists(sourceFolder))
                {
                    System.IO.Directory.CreateDirectory(sourceFolder);
                }

                Console.WriteLine(string.Format("Acomodando Archvos de {0} en subcarpetas", pathFiletxt));

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
                                System.IO.File.Move(pathFile, destinationdirectory, true);
                                //System.IO.File.Delete(pathFile);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            System.Console.WriteLine(fileName);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }

        }

        #endregion

    }

}
