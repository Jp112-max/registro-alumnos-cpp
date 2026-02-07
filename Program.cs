using System;
using System.Numerics;

public class Program
{
    public static void Main()
    {
        string path = "datos.txt";

        if (!File.Exists(path))
        {
            StreamWriter sw = File.CreateText(path);
            sw.WriteLine("id,nombre,carrera,promedio");
            sw.Close();
            Console.WriteLine("Archivo Creado!");
        }

        Console.WriteLine("1-Agregar alumno");
        Console.WriteLine("2-Leer Info");
        Console.WriteLine("3-Editar");
        Console.WriteLine("4-Filtrar");
        Console.WriteLine("5-Login");
        Console.WriteLine("6-Eliminar");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Ingresa el nuevo estudiante en formato:");
                Console.WriteLine("id,nombre,carrera,promedio");
                string usuario = Console.ReadLine();
                StreamWriter agregar = File.AppendText(path);
                agregar.WriteLine(usuario);
                agregar.Close();
                break;

            case "2":
                StreamReader lectura = File.OpenText(path);
                string contenido = lectura.ReadToEnd();
                lectura.Close();
                Console.WriteLine(contenido);
                break;

            case "3":
                string[,] matriz = CrearMat(path);

                Console.Write("Ingresa el Id del usuario a modificar: ");
                string idUsuario = Console.ReadLine();

                int filaUsuario = EncontrarFila(matriz);

                if (filaUsuario >= 0)
                {
                    Console.WriteLine("Ingresa lo que deseas cambiar:");
                    Console.WriteLine("1-Nombre\n2-Carrera\n3-Prom");
                    int indiceCambio = Convert.ToInt32(Console.ReadLine());

                    if (indiceCambio > 0 && indiceCambio <= 3)
                    {
                        Console.WriteLine("Ingresa el nuevo valor: ");
                        matriz[filaUsuario, indiceCambio] = Console.ReadLine();
                        Console.WriteLine("Cambio realizado con éxito!");
                    }

                    Guardar(matriz, path);
                }
                else
                {
                    Console.WriteLine("Id no encontrado!");
                }
                break;

            case "4":
                string[,] data = CrearMat(path);
                Console.WriteLine("Filtrar por:\n1-Nombre\n2-Carrera\n3-Rango de promedio");
                string opc = Console.ReadLine();

                if (opc == "1")
                {
                    Console.Write("Ingresa el nombre: ");
                    string nombre = Console.ReadLine();
                    for (int i = 1; i < data.GetLength(0) - 1; i++)
                    {
                        if (data[i, 1].Trim().ToLower() == nombre.ToLower())
                            Console.WriteLine($"{data[i, 0]},{data[i, 1]},{data[i, 2]},{data[i, 3]}");
                    }
                }
                else if (opc == "2")
                {
                    Console.Write("Ingresa la carrera: ");
                    string carrera = Console.ReadLine();
                    for (int i = 1; i < data.GetLength(0) - 1; i++)
                    {
                        if (data[i, 2].Trim().ToLower() == carrera.ToLower())
                            Console.WriteLine($"{data[i, 0]},{data[i, 1]},{data[i, 2]},{data[i, 3]}");
                    }
                }
                else if (opc == "3")
                {
                    Console.Write("Promedio mínimo: ");
                    double min = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Promedio máximo: ");
                    double max = Convert.ToDouble(Console.ReadLine());

                    for (int i = 1; i < data.GetLength(0) - 1; i++)
                    {
                        double prom = Convert.ToDouble(data[i, 3]);
                        if (prom >= min && prom <= max)
                            Console.WriteLine($"{data[i, 0]},{data[i, 1]},{data[i, 2]},{data[i, 3]}");
                    }
                }
                break;

            case "5":
                string[,] datos = CrearMat(path);
                int fila = EncontrarFila(datos);
                if (fila >= 0)
                {
                    Console.WriteLine("Ingresa tu contraseña:");
                    string psw = Console.ReadLine();
                    if (psw == datos[fila, 2])
                        Console.WriteLine("Credenciales correctas!");
                    else
                        Console.WriteLine("Credenciales incorrectas!!");
                }
                else Console.WriteLine("No se encontró el dato!");
                break;


            case "6":
                string[,] m = CrearMat(path);

                Console.Write("Ingresa el Id del usuario a eliminar: ");
                string idEliminar = Console.ReadLine();

                int filaEliminar = -1;
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    if (m[i, 0] == idEliminar)
                    {
                        filaEliminar = i;
                        break;
                    }
                }

                if (filaEliminar >= 0)
                {
                    for (int j = 0; j < m.GetLength(1); j++)
                    {
                        m[filaEliminar, j] = "";
                    }
                    Console.WriteLine("Usuario eliminado con éxito!");
                    Guardar(m, path);
                }
                else
                {
                    Console.WriteLine("ID no encontrado!");
                }
                break;
        }
    }
    public static void Guardar(string[,] matriz, string path)
    {
        StreamWriter sw = File.CreateText(path);
        for (int i = 0; i < matriz.GetLength(0) - 1; i++)
        {
            if (string.IsNullOrWhiteSpace(matriz[i, 0])) continue;

            for (int j = 0; j < matriz.GetLength(1); j++)
            {
                if (j == matriz.GetLength(1) - 1) sw.WriteLine(matriz[i, j].Trim());
                else sw.Write(matriz[i, j] + ",");
            }
        }
        sw.Close();
    }
    public static string[,] CrearMat(string path)
    {
        StreamReader reader = File.OpenText(path);
        string texto = reader.ReadToEnd();
        reader.Close();

        string[] filas = texto.Split('\n');
        string[,] matriz = new string[filas.Length, 4];

        for (int i = 0; i < matriz.GetLength(0) - 1; i++)
        {
            string[] columnas = filas[i].Split(",");
            for (int j = 0; j < matriz.GetLength(1); j++)
            {
                matriz[i, j] = columnas[j];
            }
        }
        return matriz;
    }
    public static int EncontrarFila(string[,] matriz)
    {
        Console.Write("Ingresa el Id del usuario: ");
        string idUsuario = Console.ReadLine();
        int filaUsuario = -1;

        for (int i = 0; i < matriz.GetLength(0); i++)
        {
            if (matriz[i, 0] == idUsuario)
            {
                filaUsuario = i;
                break;
            }
        }
        return filaUsuario;
    }

    public static void Imprimir(string[,] matriz)
    {
        for (int i = 0; i < matriz.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < matriz.GetLength(1); j++)
            {
                Console.Write(matriz[i, j] + ",");
            }
            Console.WriteLine();
        }
    }
}
