using computershop.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace computershop.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel user)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection()) 
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [user] where username = @username and [password] = @password";
                command.Parameters.Add("@username",System.Data.SqlDbType.NVarChar).Value=credential.UserName;
                command.Parameters.Add("@password",System.Data.SqlDbType.NVarChar).Value=credential.Password;
                validUser = command.ExecuteScalar() != null;
            }
            return validUser;
        }

        public void Edit(UserModel user)
        {
            throw new NotImplementedException();
        }

        IEnumerable<UserModel> IUserRepository.GetAll()
        {
            throw new NotImplementedException();
        }

        

        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
               connection.Open();
               command.Connection = connection;
               command.CommandText = "select * from [user] where @username = Username";
               command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
               using (var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        user = new UserModel()
                        {
                            ID = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            Name = reader[3].ToString(),
                            lastName = reader[4].ToString(),
                            Email = reader[5].ToString()
                        };
                    }
                }
            }
            return user;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
