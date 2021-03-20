using Grpc.Net.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new PersonService.PersonServiceClient(channel);
            Console.WriteLine("Do you want to enter your data? Y/N");
            var option = Console.ReadLine();
            while (option.ToLower().Equals("Y".ToLower()))
            {
                Console.WriteLine("\nFirst name: ");
                var firstName = Console.ReadLine();
                while (firstName.All(char.IsDigit))
                {
                    Console.WriteLine("Incorrect first name!\nFirst name: ");
                    firstName = Console.ReadLine();
                }
                Console.WriteLine("Last name: ");
                var lastName = Console.ReadLine();
                while (lastName.All(char.IsDigit))
                {
                    Console.WriteLine("Incorrect last name!\nLast name: ");
                    lastName = Console.ReadLine();
                }

                Console.WriteLine("CNP: ");
                var cnp = Console.ReadLine();
                while (!cnp.All(char.IsDigit) || cnp.Length != 13)
                {
                    Console.WriteLine("Incorrect CNP!\nCNP: ");
                    cnp = Console.ReadLine();
                }

                var response = await client.SendPersonalDataAsync(new PersonalDataRequest()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Cnp = cnp
                });
                Console.WriteLine("Status: " + response.Status);

                Console.WriteLine("\nDo you want to enter other data? Y/N");
                option = Console.ReadLine();
            }
            await channel.ShutdownAsync();
        }
    }
}

