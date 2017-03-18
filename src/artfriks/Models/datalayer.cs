using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace artfriks.Models
{
    public class datalayer
    {
        SqlConnection con = new SqlConnection(@"Server=cocoaws.cew0codqdrhj.us-west-2.rds.amazonaws.com,1433;Initial Catalog=Cocospices_Dev1;User ID=cocoadmin;Password=cocospices");
        SqlDataReader dr = null;
        public void CloseConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public void OpenConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }
        public SqlDataReader GetDataReaderByProc(string Procedure, string ParamName, string ParamValue)
        {
            try
            {
                string[] SplitName = ParamName.Split(',');
                string[] SplitValue = ParamValue.Split(',');

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Procedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < SplitName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(SplitName[i], SplitValue[i]);
                }
                //cmd.Parameters.AddWithValue(ParamName, ParamValue);
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                CloseConnection();
            }
            return dr;
        }
    }
}
