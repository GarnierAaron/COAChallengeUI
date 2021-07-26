using COAConsoleApp.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace COAConsoleApp
{
    class Program
    {
        private static HttpClient httpClient = new HttpClient();
        private static string URLbase = "https://localhost:44397/";
        static void Main(string[] args)
        {
            string texto="";
            do
            {
                Console.Clear();
                Console.WriteLine("------------- COA Challenge -----------------");
                GetUsuarios();
                Console.WriteLine("\n¿Que desea realizar?");
                

                int opcionElegida = OpcionCorrecta();

                switch (opcionElegida)
                {
                    case 1:
                        CrearUsuario();
                        break;
                    case 2:
                        EliminarUsuario();
                        break;
                    case 3:
                        ModificarUsuario();
                        break;
                }

                Console.WriteLine("\n¿Desea realizar otra operacion? (Y)");
                texto = Console.ReadLine();
            } while (texto == "Y");

            Console.WriteLine("\nGracias por utilizar COA consoleApp");
            Console.ReadLine();
        }
        private static void GetUsuarios()
        {

            var stringResponse = httpClient.GetAsync(URLbase + "Usuario/Lista").Result.Content.ReadAsStringAsync().Result;
            var respuestaListaUsuarios = JsonConvert.DeserializeObject<ResponseDTO>(stringResponse);

            if (respuestaListaUsuarios.Success)
            {
                Console.WriteLine("\nLista de Usuarios\n");
                var listaUsuario=((JArray)respuestaListaUsuarios.Result).ToObject<List<UsuarioDTO>>();

                if(listaUsuario.Count == 0)
                {
                    Console.WriteLine("\nActualmente no hay Usuarios Activos\n");
                }
                else
                {
                    foreach (var usuario in listaUsuario)
                    {
                        Console.WriteLine($"UserName: {usuario.UserName} - Nombre: {usuario.Nombre} - Email: {usuario.Email} - Telefono: {usuario.Telefono} .");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nOcurrio un Error inesperado.\n");
            }
            
            
        }
        private static void CrearUsuario()
        {
            Console.Clear();
            Console.WriteLine("------------- Crear Usuario -----------------");
            GetUsuarios();
            Console.WriteLine("\nIngrese el -UserName- del Nuevo Usuario");
            var userNameCrear = Console.ReadLine();

            var stringResponse = httpClient.GetAsync(URLbase + "Usuario/Lista").Result.Content.ReadAsStringAsync().Result;
            var respuestaListaUsuarios = JsonConvert.DeserializeObject<ResponseDTO>(stringResponse);

            if (respuestaListaUsuarios.Success)
            {
                var listaUsuario = ((JArray)respuestaListaUsuarios.Result).ToObject<List<UsuarioDTO>>();
               
                if(listaUsuario.Any(x => x.UserName.ToUpper() == userNameCrear.ToUpper()))
                {
                    Console.WriteLine("\nYa existe un -UserName- Activo con ese Nombre\n");
                }
                else
                {
                    Console.WriteLine("\nIngrese el nuevo Nombre");
                    var nombreCrear = Console.ReadLine();                    
                    
                    Console.WriteLine("\nIngrese el nuevo Email");
                    var emailCrear = Console.ReadLine();                
                    
                    Console.WriteLine("\nIngrese el nuevo Telefono");
                    var telefonoCrear = IngresarTelefono();

                    var usuarioDTOCrear = new UsuarioDTO()
                    {
                        UserName = userNameCrear,
                        Nombre = nombreCrear,
                        Email = emailCrear,
                        Telefono = telefonoCrear
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(usuarioDTOCrear), System.Text.Encoding.UTF8, "application/json");
                    string stringResponseCrear = httpClient.PostAsync(URLbase + "Usuario/Agregar", content).Result.Content.ReadAsStringAsync().Result;
                    var responseUsuarioCrear = JsonConvert.DeserializeObject<ResponseDTO>(stringResponseCrear);

                    if (!responseUsuarioCrear.Success)
                    {
                        Console.WriteLine("\nOcurrio un Error inesperado.\n");
                    }
                }             
                             
                
            }
            else
            {
                Console.WriteLine("\nOcurrio un Error inesperado.\n");
            }
        }
        private static void EliminarUsuario()
        {
            Console.Clear();
            Console.WriteLine("------------- Eliminar Usuario -----------------");
            GetUsuarios();
            Console.WriteLine("\nIngrese el -UserName- del Usuario que desea Eliminar");
            var userNameEliminar = Console.ReadLine();

            var stringResponse = httpClient.GetAsync(URLbase + "Usuario/Lista").Result.Content.ReadAsStringAsync().Result;
            var respuestaListaUsuarios = JsonConvert.DeserializeObject<ResponseDTO>(stringResponse);

            if (respuestaListaUsuarios.Success)
            {
                var listaUsuario = ((JArray)respuestaListaUsuarios.Result).ToObject<List<UsuarioDTO>>();

                if (listaUsuario.Count == 0)
                {
                    Console.WriteLine("\nActualmente no hay Usuarios Activos\n");
                }
                else
                {
                    if (listaUsuario.Any(x => x.UserName.ToUpper() == userNameEliminar.ToUpper()))
                    {
                 
                        string stringResponseEliminar = httpClient.DeleteAsync(URLbase + "Usuario/Eliminar/"+userNameEliminar).Result.Content.ReadAsStringAsync().Result;
                        var responseUsuarioEliminar = JsonConvert.DeserializeObject<ResponseDTO>(stringResponseEliminar);

                        if (!responseUsuarioEliminar.Success)
                        {
                            Console.WriteLine("\nOcurrio un Error inesperado.\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nNo existe un -UserName- Activo con ese Nombre\n");
                    }
                        
                }
            }
            else
            {
                Console.WriteLine("\nOcurrio un Error inesperado.\n");
            }
        }
        private static void ModificarUsuario()
        {
            Console.Clear();
            Console.WriteLine("------------- Modificar Usuario -----------------");
            GetUsuarios();
            Console.WriteLine("\nIngrese el -UserName- del Usuario que desea Modificar");
            var userNameModificar = Console.ReadLine();

            var stringResponse = httpClient.GetAsync(URLbase + "Usuario/Lista").Result.Content.ReadAsStringAsync().Result;
            var respuestaListaUsuarios = JsonConvert.DeserializeObject<ResponseDTO>(stringResponse);

            if (respuestaListaUsuarios.Success)
            {              
                var listaUsuario = ((JArray)respuestaListaUsuarios.Result).ToObject<List<UsuarioDTO>>();

                if (listaUsuario.Count == 0)
                {
                    Console.WriteLine("\nActualmente no hay Usuarios Activos\n");
                }
                else
                {
                    if (listaUsuario.Any(x => x.UserName.ToUpper() == userNameModificar.ToUpper()))
                    {
                        Console.WriteLine("\nIngrese el nuevo Nombre");
                        var nombreModificar = Console.ReadLine();

                        Console.WriteLine("\nIngrese el nuevo Email");
                        var emailModificar = Console.ReadLine();

                        Console.WriteLine("\nIngrese el nuevo Telefono");
                        var telefonoModificar = IngresarTelefono();

                        var usuarioDTOModificar = new UsuarioDTO()
                        {
                            UserName = userNameModificar,
                            Nombre = nombreModificar,
                            Email = emailModificar,
                            Telefono = telefonoModificar
                        };

                        var content = new StringContent(JsonConvert.SerializeObject(usuarioDTOModificar), System.Text.Encoding.UTF8, "application/json");
                        string stringResponseModificar = httpClient.PutAsync(URLbase + "Usuario/Modificar", content).Result.Content.ReadAsStringAsync().Result;
                        var responseUsuarioModificar = JsonConvert.DeserializeObject<ResponseDTO>(stringResponseModificar);

                        if (!responseUsuarioModificar.Success)
                        {
                            Console.WriteLine("\nOcurrio un Error inesperado.\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nNo existe un -UserName- Activo con ese Nombre\n");
                    }
                        
                }
            }
            else
            {
                Console.WriteLine("\nOcurrio un Error inesperado.\n");
            }
            

        }
        static int OpcionCorrecta()
        {
            bool estado = false;
            int numero = 0;

            while (!estado)
            {
                Console.WriteLine("\nIngrese el Numero de la opcion que desea:");
                Console.WriteLine("\n1 - Crear Usuario");
                Console.WriteLine("2 - Eliminar Usuario");
                Console.WriteLine("3 - Modificar Usuario");
                

                if (int.TryParse(Console.ReadLine(), out numero) & (numero < 4) & (numero > 0))
                {
                    estado = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nIngresó una Opcion Incorrecta. Vuelva a intentarlo nuevamente.");
                }

            }
            return numero;
        }
        static long IngresarTelefono()
        {
            bool estado = false;
            long numero = 0;
            while (!estado)
            {
                if (long.TryParse(Console.ReadLine(), out numero) & (numero > 0))
                {
                    estado = true;
                }
                else
                {
                    Console.WriteLine("\nDebe ingresar un valor numerico para el Telefono.");
                }
            }
            return numero;
        }
        
    }
}
