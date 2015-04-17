﻿using System;
using System.Data.SqlClient;
using com.WanderingTurtle.Common;

namespace com.WanderingTurtle.DataAccess
{
    public class UserLoginAccessor
    {
        //Summary: creates a new variable to allow other programs to use the
        //methods in this form
        //
        //Parameters: none
        //
        //Exceptions: none
        public UserLoginAccessor()
        { }

        //Summary: allows the user to get their login information so that they
        //can use the program as an authorized user
        //
        //Parameters: string password - the password needed to identify the
        //registered user
        //
        //Exceptions: SqlException - when the program cannot access the database
        //            ApplicationException - when there are no entries in database
        public Employee getUserLogin(int id, string password)
        {
            var conn = DatabaseConnection.GetDatabaseConnection(); //find out the class where the database string will be held and place it in here
            var query = @"spUserLoginGet";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@original_userID", id);
            cmd.Parameters.AddWithValue("@original_userPassword", password);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    return null;
                    //    new Employee(
                    //    (int)reader.GetValue(0), //EmployeeID
                    //);
                    //loginUser.Password = reader.GetValue(1).ToString();
                    //loginUser.Level = (RoleData)reader.GetInt32(2);
                }
                else
                {
                    throw new ApplicationException("This password does not go with any users.");
                }

                //loginUser = tempGetUserLogin();
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        //Summary: temporary using this to retrieve data for getUserLogin until the database is hooked up
        //
        //Parameters: none
        //
        //Exceptions: none
        //private UserLogin tempGetUserLogin()
        //{
        //    return new UserLogin(123, "fairy", 'a');
        //}

        //private List<UserLogin> tempGetUserLoginList()
        //{
        //    List<UserLogin> loginList = new List<UserLogin>();

        //    loginList.Add(new UserLogin(123, "fairy", 'a'));
        //    loginList.Add(new UserLogin(145, "PrettyCure", 'b'));
        //    loginList.Add(new UserLogin(200, "FireEmblem", 'c'));

        //    return loginList;
        //}
    }
}