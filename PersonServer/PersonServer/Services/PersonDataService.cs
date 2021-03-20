using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonServer.Services
{
    public class PersonDataService : PersonService.PersonServiceBase
    {
        private Person.Types.Gender GetGender(string cnp)
        {
            switch (cnp.ElementAt(0))
            {
                case '1':
                    return Person.Types.Gender.Male;
                case '2':
                    return Person.Types.Gender.Female;
                case '5':
                    return Person.Types.Gender.Male;
                case '6':
                    return Person.Types.Gender.Female;
                default:
                    return Person.Types.Gender.Unknown;
            }

        }

        private int GetAge(string cnp)
        {
            DateTime today = DateTime.Now;
            int year = Int32.Parse(cnp.Substring(1, 2));
            if ((cnp.ElementAt(0) == '5' || cnp.ElementAt(0) == '6') && year > today.Year % 100)
            {
                return -1;
            }
            int month = Int32.Parse(cnp.Substring(3, 2));
            if (month < 1 || month > 12)
            {
                return -1;
            }
            int day = Int32.Parse(cnp.Substring(5, 2));
            if (day < 1 || day > 31)
            {
                return -1;
            }

            if (year <= today.Year % 100 && (cnp.ElementAt(0) == '5' || cnp.ElementAt(0) == '6'))
            {
                year += 2000;
            }
            else
            {
                year += 1900;
            }
            DateTime birthDate = new DateTime(year, month, day);
            int age = (today - birthDate).Days / 365;
            return age;
        }
        public override Task<PersonalDataResponse> SendPersonalData(PersonalDataRequest request, ServerCallContext context)
        {
            var person = new Person()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = GetAge(request.Cnp),
                Gender = GetGender(request.Cnp)
            };
            if (person.Age == -1)
            {
                Console.WriteLine("\n---------------" +
               "\n|  CONNECTED  |" +
               "\n---------------" +
               "\nFirst name: " + person.FirstName +
               "\nLast name: " + person.LastName +
               "\nAge: " + "INVALID" +
               "\nGender: " + person.Gender +
               "\n---------------\n");
                return Task.FromResult(new PersonalDataResponse() { Status = PersonalDataResponse.Types.Status.Error });
            }
            else
            {
                Console.WriteLine("\n---------------" +
                    "\n|  CONNECTED  |" +
                    "\n---------------" +
                    "\nFirst name: " + person.FirstName +
                    "\nLast name: " + person.LastName +
                    "\nAge: " + person.Age +
                    "\nGender: " + person.Gender +
                    "\n---------------\n");
                return Task.FromResult(new PersonalDataResponse() { Status = PersonalDataResponse.Types.Status.Success });
            }

        }
    }
}
