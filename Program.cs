using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CDS_API_FIREBASE
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Cadastrar Novo Usuario.");
                Console.WriteLine("2 - Buscar Usuario.");
                Console.WriteLine("3 - Excluir Usuario.");
                Console.WriteLine("4 - Sair.");
                Console.Write("OP: ");
                String op = Console.ReadLine();
                switch (op)
                {
                    case "1":
                        User aux = new User();
                        Console.Write("ID: ");
                        aux.id = Console.ReadLine();
                        Console.Write("Nome: ");
                        aux.nome = Console.ReadLine();
                        Console.Write("Telefone: ");
                        aux.telefone = Console.ReadLine();
                        Console.Write("Endereco: ");
                        aux.endereco = Console.ReadLine();
                        Console.Write("Email: ");
                        aux.email = Console.ReadLine();
                        cadastrarUser(aux);
                        break;

                    case "2":
                        Console.Write("ID: ");
                        User busca = buscarUser(Console.ReadLine());
                        if(busca != null)
                        {
                            Console.WriteLine("ID: " + busca.id);
                            Console.WriteLine("Nome: " + busca.nome);
                            Console.WriteLine("Telefone: " + busca.telefone);
                            Console.WriteLine("Endereco: " + busca.endereco);
                            Console.WriteLine("Email: " + busca.email);
                        }
                        else
                        {
                            Console.WriteLine("Usuario Inexistente");
                        }
                        break;

                    case "3":
                        Console.Write("ID: ");
                        String id = Console.ReadLine();
                        busca = buscarUser(id);
                        if (busca != null)
                        {
                            apagarUser(id);
                        }
                        else
                        {
                            Console.WriteLine("Usuario Inexistente");
                        }
                       break;
                    case "4":
                        return;
                    
                    default:
                        Console.WriteLine("Opcao Invalida.");
                        break;

                }
                
            }
        }

        public static void cadastrarUser(User user)
        {
            string userJson = JsonSerializer.Serialize(user);
            var request = WebRequest.CreateHttp("https://cds-firebase.firebaseio.com/" + user.id + ".json");
            request.Method = "PUT";
            request.ContentType = "application/json";
            var buffer = Encoding.UTF8.GetBytes(userJson);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            request.GetResponse();
            Console.WriteLine("Usuário Cadastrado com Sucesso.");

        }

        public static User buscarUser(String id)
        {
            WebClient wc = new WebClient();
            String resultado = wc.DownloadString("https://cds-firebase.firebaseio.com/" + id + ".json");
            User aux = JsonSerializer.Deserialize<User>(resultado);
            return aux;
        }

        public static void apagarUser(String id)
        {
            var request = WebRequest.CreateHttp("https://cds-firebase.firebaseio.com/" + id + ".json");
            request.Method = "DELETE";
            request.ContentType = "application/json";
            request.GetResponse();
            Console.WriteLine("Usuário Apagado com Sucesso.");
        }
    }
}
