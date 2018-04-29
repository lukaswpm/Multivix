using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace Server
{
    public class MSConnection
    {
        public static void Open()
        {
            try
            {
                Global.connection = new MySqlConnection(Global.strConnection);
                Global.connection.Open();
                Console.WriteLine("Connection to DB chatserver successfully!");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Errors.Manage_Errors(ex);
            }
        }
        public static void Close()
        {
            try
            {
                if (Global.connection.State == ConnectionState.Open)
                    Global.connection.Close();
            }
            catch (MySqlException ex)
            {
                Errors.Manage_Errors(ex);
            }
        }
        public MySqlCommand ExecuteQuery(string Query)
        {
            try
            {
                Open();
                MySqlCommand cmd = new MySqlCommand(Query, Global.connection);
                return cmd;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Errors.Manage_Errors(ex);
            }
            return null;
        }
        public string MySqlResult(MySqlCommand Query, int Column)
        {
            try
            {
                MSConnection mSConnection = new MSConnection();
                MySqlDataReader reader = Query.ExecuteReader();
                while (reader.Read())
                {
                    return (string)reader[Column];
                }
            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex);
                Errors.Manage_Errors(ex);
            }
            return null;
        }
        public string MySqlResult(MySqlCommand Query, string Column)
        {
            try
            {
                MSConnection mSConnection = new MSConnection();
                MySqlDataReader reader = Query.ExecuteReader();
                while (reader.Read())
                {
                    return (string)reader[Column];
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                Errors.Manage_Errors(ex);
            }
            return null;
        }
        public static bool Connection_State()
        {
            if (Global.connection.State == ConnectionState.Open)
                return true;
            else return false;
        }
    }
}
