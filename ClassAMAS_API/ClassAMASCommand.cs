using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using iAnywhere.Data.SQLAnywhere;
using System.Data.SqlTypes;
using System.IO;
using CommonValues;


namespace AMAS_DBI
{
    public static class AMASCommand
    {
        private static AMAS_DBI.Class_syb_acc SyB_Acc;

        public static void AccessCommands(AMAS_DBI.Class_syb_acc acc)
        {
            SyB_Acc = acc;
            SyB_Acc.MyRights = Roles_Employee_list(SyB_Acc.Current_User);
        }

        public static AMAS_DBI.Class_syb_acc Access
        {
            get { return SyB_Acc; }
        }

        public static int AddDelo(string Delo, string Descr)
        {
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@delo", SqlDbType.Char, 8);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = Delo;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.Char, 120);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = Descr;

                SyB_Acc.SQLCommand.CommandText = "insert into dbo.rkk_delo (delo,description_) values (@delo, @description_)";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 8.0", e.StackTrace);
            }

            return SeekDelo(Delo);
        }

        public static int SeekDelo(string delo)
        {
            int ret = -1;

            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                if (delo.Length < 8) delo = delo.PadRight(8);
                SyB_Acc.SQLCommand.CommandText = "select cod from dbo.rkk_delo where delo like '" + delo + "'";
                reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        ret = (int)reader["cod"];
                }

            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 8.1", e.StackTrace);
                ret = -1;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static int SeekPrefixCount(int Id)
        {
            int ret = 0;

            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "select counter from dbo.rkk_prefix where kod = " + Id.ToString();
                reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        ret = (int)reader["counter"];
                }

            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 8.2", e.StackTrace);
                ret = 0;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static string SeekDeloDescription(string delo)
        {
            string ret = "";

            if (delo.Length<8) delo.PadRight(8,' ');
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@delo", SqlDbType.Char, 8);
                SyB_Acc.SQLCommand.Parameters[0].Value = delo;
                SyB_Acc.SQLCommand.CommandText = "select description_ from dbo.rkk_delo where delo like @delo" ;
                reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        ret = (string)reader["description_"];
                }

            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 8.2.1", e.StackTrace);
                ret = "";
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static int SeekSuffixCount(int Id)
        {
            int ret = 0;

            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "select counter from dbo.rkk_suffix where id = " + Id.ToString();
                reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        ret = (int)reader["counter"];
                }

            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 8.3", e.StackTrace);
                ret = 0;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        // registers

        public static bool Seek_Juridic(string Fullname, string Shortname, int flat, int house, int street, int district, int city, int areal, int trc, int state)
        {
            bool b = false;
            try
            {
                SyB_Acc.refresh_execomm();
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.ORG_the_org ";
                int i = 0;
                if (Fullname.Trim().Length > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@full_name", SqlDbType.VarChar, 80);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = Fullname;
                    i++;
                }
                if (Shortname.Trim().Length > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@organ", SqlDbType.VarChar, 18);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = Shortname;
                    i++;
                }
                if (flat > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@flat", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = flat;
                    i++;
                }
                if (house > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@house", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = house;
                    i++;
                }
                if (street > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@street", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = street;
                    i++;
                }
                if (district > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@district", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = district;
                    i++;
                }
                if (city > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@city", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = city;
                    i++;
                }
                if (areal > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@areal", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = areal;
                    i++;
                }
                if (trc > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@trc", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = trc;
                    i++;
                }
                if (state > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@state", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = state;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 1", e.StackTrace);
                SyB_Acc.SQLCommand.Cancel(); }
            return b;
        }


        public static bool UpdateManAddress(int man_id, int fla_id)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.pop_people set fla_id=" + fla_id.ToString() + " where id=" + man_id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 2", e.StackTrace);
            }
            return b;
        }

        public static bool UpdateOrgAddress(int org_id, int fla_id)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.org_jrd_juridic set fla_id=" + fla_id.ToString() + " where id=" + org_id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 2", e.StackTrace);
            }
            return b;
        }

        public static bool ADD_KLADR_Address(string address, string cod)
        {
            bool res = false;
            System.Data.SqlClient.SqlParameter Param = null;

            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.ADR_KLADR_set_address";

                Param = SyB_Acc.SQLCommand.Parameters.Add("@adr", SqlDbType.VarChar);
                Param.Direction = ParameterDirection.Input;
                Param.Value = address;

                Param = SyB_Acc.SQLCommand.Parameters.Add("@adrCOD", SqlDbType.VarChar);
                Param.Direction = ParameterDirection.Input;
                Param.Value = cod;

                SyB_Acc.SQLCommand.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex) { res = false; SyB_Acc.EBBLP.AddError(ex.Message, "Command - 3", ex.StackTrace); }
            return res;
        }

        public static bool ADD_Address(string state, string trc, string areal, string city, string district, string street, string house, string flat)
        {
            bool b = false;
            System.Data.SqlClient.SqlParameter Param = null;
  
            try
            {
                state = state.Trim();
                city = city.Trim();
                street = street.Trim();
                house = house.Trim();
                flat = flat.Trim();
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.ADR_add_address";
                if (state.Length > 0)
                {
                    Param=SyB_Acc.SQLCommand.Parameters.Add("@state", SqlDbType.VarChar, 40);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = state;
                }
                if (trc.Length > 0)
                {
                    Param=SyB_Acc.SQLCommand.Parameters.Add("@trc", SqlDbType.VarChar, 40);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = trc;
                }
                if (areal.Length > 0)
                {
                    Param=SyB_Acc.SQLCommand.Parameters.Add("@areal", SqlDbType.VarChar, 40);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = areal;
                }
                if (city.Length > 0)
                {
                    Param=SyB_Acc.SQLCommand.Parameters.Add("@city", SqlDbType.VarChar, 40);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = city;
                }
                if (district.Length > 0)
                {
                    Param=SyB_Acc.SQLCommand.Parameters.Add("@district", SqlDbType.VarChar, 40);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = district;
                }
                if (street.Length > 0)
                {
                    Param=SyB_Acc.SQLCommand.Parameters.Add("@street", SqlDbType.VarChar, 40);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = street;
                }
                if (house.Length > 0)
                {
                    Param=SyB_Acc.SQLCommand.Parameters.Add("@house", SqlDbType.VarChar, 40);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = house;
                }
                if (flat.Length > 0)
                {
                    Param=SyB_Acc.SQLCommand.Parameters.Add("@flat", SqlDbType.VarChar, 40);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = flat;
                }

                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 4", e.StackTrace);
            }
            return b;
        }

        public static bool add_ORG(string org, string shortName, int flat)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.org_jrd_juridic (full_name,short_name, fla_id) values('" + org.Trim() + "','" + shortName.Trim() + "'," + flat.ToString() + ")";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 5", e.StackTrace);
            }
            return b;
        }

        public static bool ADD_Man(string family, string name, string father, int flat)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.pop_add_man";
                SyB_Acc.SQLCommand.Parameters.Add("@family", SqlDbType.VarChar, 40);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = family;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar, 40);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@father", SqlDbType.VarChar, 40);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = father;
                SyB_Acc.SQLCommand.Parameters.Add("@fla_id", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[3].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[3].Value = flat;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 5", e.StackTrace);

            }
            return b;
        }

        public static bool ADD_Employee(string family, string name, string father, string degree, int juridic)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.emp_add_leader";
                SyB_Acc.SQLCommand.Parameters.Add("@family", SqlDbType.VarChar, 40);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = family;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar, 40);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@father", SqlDbType.VarChar, 40);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = father;
                SyB_Acc.SQLCommand.Parameters.Add("@degree", SqlDbType.VarChar, 40);
                SyB_Acc.SQLCommand.Parameters[3].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[3].Value = degree;
                SyB_Acc.SQLCommand.Parameters.Add("@org", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[4].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[4].Value = juridic;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 6", e.StackTrace);
            }
            return b;
        }

        public static bool Seek_People(string family, string name, string father, int flat, int house, int street, int city)
        {
                        bool b = false;
            try
            {
                SyB_Acc.refresh_execomm();
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.pop_the_man";
                int i = 0;
                if (family.Trim().Length > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@family", SqlDbType.Char,40);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = family.Trim();
                    i++;
                }
                if (name.Trim().Length > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.Char, 40);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = name.Trim();
                    i++;
                }
                if (father.Trim().Length > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@father", SqlDbType.Char, 40);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = father.Trim();
                    i++;
                }
                if (flat > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@flat", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = flat;
                    i++;
                }
                if (house > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@house", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = house;
                    i++;
                }
                if (street > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@street", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = street;
                    i++;
                }
                /*if (city > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@city", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = city;
                }*/
                
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 7", e.StackTrace);
            }
            return b;
        }

        // Structure

        public static long append_DEP(string name, long under)
        {
            int Id = -1;
            System.Data.SqlClient.SqlDataReader reader=null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.org_jrd_department ([name], under,juridic) values ('" + name.Trim() + "', " + under.ToString() + ", " + SyB_Acc.My_Organization.ToString() + ")";
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select department from dbo.org_jrd_department where [name] like '" + name.Trim() + "' and [under]= " + under.ToString() + " and juridic= " + SyB_Acc.My_Organization.ToString() + " and user_=dbo.user_ident()";
                reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        Id = (int)reader["department"];
                }

            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 8", e.StackTrace);
                Id = -1;
            }
            finally
            {
                if (reader!=null) reader.Close();
            }
            return Id;
        }

        public static bool rename_DEP(string name, long ident)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.org_jrd_department set [name] ='" + name.Trim() + "' where department=" + ident.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 1", e.StackTrace);
            }
            return b;
        }

        public static bool reOrder_DEP(long under, long org)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                if(under>0)
                    SyB_Acc.SQLCommand.CommandText = "update dbo.org_jrd_department set under =" + under.ToString() + " where department=" + org.ToString();
                else
                    SyB_Acc.SQLCommand.CommandText = "update dbo.org_jrd_department set under =null where department=" + org.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 9", e.StackTrace);
            }
            return b;
        }

        public static bool delete_DEP(long DEP)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.org_delete_department";
                SyB_Acc.SQLCommand.Parameters.Add("@dep", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value =(int) DEP;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 10", e.StackTrace);
            }
            return b;
        }
        
        //groups

        public static int Add_Group(string name)
        {
            int Id = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.org_groups (denote) values ('" + name.Trim() + "' )";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 11", e.StackTrace);
                Id = -11; 
            }

            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select id from dbo.org_groups where denote like '" + name.Trim() + "' ";
                System.Data.SqlClient.SqlDataReader reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        Id = (int)reader["id"];
                }
                reader.Close();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 12", e.StackTrace);
                Id = -2; 
            }
            return Id;
        }

        public static bool Delete_Group(int id)
        {
            bool b=false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "delete dbo.org_groups where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 13", e.StackTrace);
                b = false; 
            }
            return b;
        }

        public static bool Delete_Own_Document(int kod)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_own_Docs where kod=" + kod.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 13.1", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool Rename_Group(int id, string name)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.org_groups set denote='" + name.Trim() + "' where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 14", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static int Group_For_vizing(int id)
        {
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.org_groups set vizing=1,executing=0 where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 15", e.StackTrace);
            }
            return grp_MoVI(id);
        }
        public static int Group_For_moving(int id)
        {
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.org_groups set executing=1,vizing=0 where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 15", e.StackTrace);
            }
            return grp_MoVI(id);
        }

        // contragents

        public static ArrayList Doc_contragents(int document)
        {
            ArrayList agents =new ArrayList();
            SyB_Acc.SQLCommand.Parameters.Clear();
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.agent_contragents_of_doc @docid = " + document.ToString();
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.Default);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        agents.Add(new CommonClass.Arrayagents((string)reader.GetValue(1), (int)reader.GetValue(0), (int)reader.GetValue(2), (int)reader.GetValue(3)));
                    }
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 15.1", e.StackTrace);
                agents = null;
            }
            if (reader != null) reader.Close();
            return agents;
        }

        public static bool Contragent_append(int document, int agent)
        {
            bool b = false;
            SyB_Acc.SQLCommand.Parameters.Clear();
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.agent_add @docid = " + document.ToString()+",@agent="+agent.ToString();
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                    if ((int)reader.GetValue(0) > 0) b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 15.2", e.StackTrace);
                b = false;
            }
            if (reader != null) reader.Close();
            return b;
        }

        public static bool Contragent_remove(int document, int agent)
        {
            bool b = false;
            SyB_Acc.SQLCommand.Parameters.Clear();
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.agent_remove @docid = " + document.ToString() + ",@agent=" + agent.ToString();
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                    if ((int)reader.GetValue(0) == 0) b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 15.3", e.StackTrace);
                b = false;
            }
            if (reader != null) reader.Close();
            return b;
        }

        public static int Group_For_all(int id)
        {
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.org_groups set executing=1,vizing=1 where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 16", e.StackTrace);
            }
            return grp_MoVI(id);
        }

        private static int grp_MoVI(int id)
        {
            int b = 0;
            bool moving = false;
            bool vizing = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "select vizing, executing from dbo.org_groups where id=" + id.ToString();
                System.Data.SqlClient.SqlDataReader reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        moving = (bool)reader["executing"];
                        vizing = (bool)reader["vizing"];
                    }
                }
                reader.Close();
                if (moving) b = 1;
                if (vizing) b = b | 2;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 17", e.StackTrace);
                b = 0; 
            }
            return b;
        }

        public static int Employee_to_Group(int grp,long emp)
        {
            bool b = false;
            int id = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.org_workgroup (group_w,rank) values(" + grp.ToString()+"," +emp.ToString()+")";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 18", e.StackTrace);
                b = false; 
            }
            if (b)
                try
                {
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.CommandText = "select id from dbo.org_workgroup  where group_w=" + grp.ToString() + " and rank=" + emp.ToString();
                    System.Data.SqlClient.SqlDataReader reader = SyB_Acc.SQLCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            id = (int)reader["id"];
                        }
                    }
                    reader.Close();
                }
                catch (Exception e) 
                { 
                    SyB_Acc.EBBLP.AddError(e.Message, "Command - 19", e.StackTrace);
                    id = -1; 
                }
            return id;
        }

        public static bool Employee_from_Group(int wemp)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "delete dbo.org_workgroup where id=" + wemp.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 20", e.StackTrace);
                b = false; 
            }
            return b;
        }
        //degree

        public static int Add_Degree(string name)
        {
            int Id = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.org_jrd_degree (name,juridic) values ('" + name.Trim() + "' ," + SyB_Acc.My_Organization.ToString() + ")";
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select degree from dbo.org_jrd_degree where [name] like '" + name.Trim() + "' and juridic= " + SyB_Acc.My_Organization.ToString() + " and user_=dbo.user_ident()";
                System.Data.SqlClient.SqlDataReader reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        Id = (int)reader["degree"];
                }
                reader.Close();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 21", e.StackTrace);
            }
            return Id;
        }

        public static bool Locate_Degree(long dep, int degree)
        {
            bool b=false;
            try
            {
               SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.emp_dep_degrees (department,degree,leader) values (" + dep.ToString() + "," + degree.ToString() + ",0)";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 22", e.StackTrace);
            }
            return b;
        }

        public static bool Delete_L_degree(string cod)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "delete from amas.emp_dep_degrees where cod=" + cod;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 23", e.StackTrace);
            }
            return b;
        }
        
        public static bool assing_leader(string cod)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update amas.emp_dep_degrees set leader = 0 where department in (select department from amas.emp_dep_degrees where cod=" + cod+")";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update amas.emp_dep_degrees set leader = 1 where cod=" + cod;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 24", e.StackTrace);
            }
            return b;
        }

        public static bool Erise_Degree(int degree)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "delete dbo.org_jrd_degree where degree="+degree.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 25", e.StackTrace);
            }
            return b;
        }

        public static bool Rename_Degree(int degree,string name)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.org_jrd_degree set name='" + name .Trim()+ "' where degree=" + degree.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 26", e.StackTrace);
                b = false;
            }
            return b;
        }

        // instrustions

        public static int add_dep_instruction(int dep, string instruction)
        {
            SyB_Acc.SQLCommand.Parameters.Clear();
            SyB_Acc.SQLCommand.CommandType = CommandType.Text;
            Int32 newID = 0;
            SyB_Acc.SQLCommand.CommandText =
              "insert into dbo.org_department_instructions (department,line) values (@dep,@instr); "
                + "SELECT CAST(max(cod) AS int) from dbo.org_department_instructions";
            SyB_Acc.SQLCommand.Parameters.Add("@dep", SqlDbType.Int);
            SyB_Acc.SQLCommand.Parameters["@dep"].Value = dep;
            SyB_Acc.SQLCommand.Parameters.Add("@instr", SqlDbType.VarChar);
            SyB_Acc.SQLCommand.Parameters["@instr"].Value = instruction;
            try
                {
                    newID = (Int32)SyB_Acc.SQLCommand.ExecuteScalar();
                }
            catch (Exception ex)
                {
                    SyB_Acc.EBBLP.AddError(ex.Message, "Command - 27", ex.StackTrace);
                }
            return (int)newID;
        }

        public static int add_rank_instruction(int degree, string instruction)
        {
            SyB_Acc.SQLCommand.Parameters.Clear();
            SyB_Acc.SQLCommand.CommandType = CommandType.Text;
            Int32 newID = 0;
            SyB_Acc.SQLCommand.CommandText =
              "insert into dbo.org_rank_instructions (rank,line) values (@degree,@instr); "
                + "SELECT CAST(max(cod) AS int) from dbo.org_rank_instructions";
            SyB_Acc.SQLCommand.Parameters.Add("@degree", SqlDbType.Int);
            SyB_Acc.SQLCommand.Parameters["@degree"].Value = degree;
            SyB_Acc.SQLCommand.Parameters.Add("@instr", SqlDbType.VarChar);
            SyB_Acc.SQLCommand.Parameters["@instr"].Value = instruction;
            try
            {
                newID = (Int32)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 28", ex.StackTrace);
            }
            return (int)newID;
        }

        public static bool remove_dept_instruction(int cod)
        {
            bool good = true;
            SyB_Acc.SQLCommand.Parameters.Clear();
            SyB_Acc.SQLCommand.CommandType = CommandType.Text;
            SyB_Acc.SQLCommand.CommandText =
              "delete dbo.org_department_instructions where cod=" + cod.ToString();
            try
            {
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 29", ex.StackTrace);
                ; good = false;
            }
            return good;
        }

        public static bool remove_rank_instruction(int cod)
        {
            bool good = true;
            SyB_Acc.SQLCommand.Parameters.Clear();
            SyB_Acc.SQLCommand.CommandType = CommandType.Text;
            SyB_Acc.SQLCommand.CommandText =
              "delete dbo.org_rank_instructions where cod=" + cod.ToString();
            try
            {
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 30", ex.StackTrace);
                good = false;
            }
            return good;
        }

        //personnel


        public static bool Reserve_Employee(int man)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = " insert into dbo.emp_f2 (employee) values (" + man.ToString() + ")";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 31", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool Reserve_Employee_with_degree(int man, long degree)
        {
            bool b=false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = " insert into dbo.emp_f2 (employee) values (" + man.ToString() + ")";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.emp_dep_degrees set employee = " + man.ToString() + " where cod= " + degree.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 32", e.StackTrace);
            }
            return b;
        }

        public static bool remove_Employee(int id)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.emp_dep_degrees set employee = Null where employee = "+id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "delete from dbo.emp_F2 where EMPLOYEE ="+id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.emp_employees set deleted=getdate() where EMPLOYEE ="+id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 34", e.StackTrace);
            }
            return b;
        }

        public static bool Assign_Employee(int man,long degree)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.emp_dep_degrees set employee = " + man.ToString() + " where cod= " + degree.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 35", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool Deassign_Employee(long degree)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.emp_dep_degrees set employee = null where cod= " + degree.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 36", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool Anketa_Employee_write(int employee,byte[] anketa,string ext)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.CommandText = "dbo.emp_anketa";
                SyB_Acc.SQLCommand.Parameters.Add("@man", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = employee;
                SyB_Acc.SQLCommand.Parameters.Add("@anketa", SqlDbType.Image);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = anketa;
                SyB_Acc.SQLCommand.Parameters.Add("@ext", SqlDbType.VarChar,10);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = ext;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 37", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool Photo_Employee_write(int employee, byte[] photo)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.CommandText = "dbo.emp_photo";
                SyB_Acc.SQLCommand.Parameters.Add("@man", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = employee;
                SyB_Acc.SQLCommand.Parameters.Add("@photo", SqlDbType.Image);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = photo;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 38", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static string Anketa_Employee_read(int employee)
        {
            string filename = "";
            System.Data.SqlClient.SqlDataReader reader=null;
            try
            {
                int bufferSize = 100;
                byte[] outByte = new byte[bufferSize];
                long retval;
                long startIndex = 0;
                string ext = "";
                BinaryWriter writer; 
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select ext,anketa from dbo.emp_f2 where  employee= " + employee.ToString();
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        try
                        {
                            ext = (string)reader["ext"];
                        }
                        catch { ext = "";}
                        //CommonValues.CommonClass.tempFiles.Delete();
                        if (ext.Length > 0) ext = "." + ext;
                        filename="anketa" + ext;
                        //CommonValues.CommonClass.tempFiles.AddFile(filename, false);
                        filename = CommonValues.CommonClass.TempDirectory + filename;
                        bool goodFile=false;
                        int ifl = 1;
                        do
                        {
                            
                            try
                            {
                                goodFile = true;
                                if (File.Exists(filename))
                                {
                                    goodFile = false;
                                    ifl++;
                                    File.Delete(filename);
                                    goodFile = true;
                                }
                            }
                            catch { filename = CommonValues.CommonClass.TempDirectory + "anketa" + ifl.ToString() + ext; ifl++; }
                        }
                        while (!goodFile);
                        FileStream stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                        writer = new BinaryWriter(stream);
                        retval = reader.GetBytes(1, startIndex, outByte, 0, bufferSize);
                        while (retval == bufferSize)
                        {
                            writer.Write(outByte);
                            writer.Flush();
                            startIndex += bufferSize;
                            retval = reader.GetBytes(1, startIndex, outByte, 0, bufferSize);
                        }
                        writer.Write(outByte, 0, (int)retval );
                        writer.Flush();
                        writer.Close();
                        stream.Close();
                    }
                }
                //reader.Close();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 39", e.StackTrace);
                filename = "";
            }
            if (reader != null) reader.Close(); 
            return filename;
        }

        // security

        public static void ADDEmployeeLogin(string password, string login,int employee)
        {
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "CREATE LOGIN " + login.Trim() + " WITH PASSWORD = '" + password.Trim() + "' , DEFAULT_DATABASE =" + SyB_Acc.DataBaseName.Trim(); // AMAS"; 
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                SyB_Acc.SQLCommand.CommandText = "create user " + login.Trim() + " for login " + login.Trim();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.emp_employees (employee, indoor_name) values(" + employee.ToString() + ",'" + login.Trim() + "')";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 40", e.StackTrace);
            }
        }

        public static bool roles_EmployeeAddRemove(string login, string ROLE, bool check)
        {
            bool b = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.sec_RoleAddRem @role='"+ROLE.Trim()+"', @user = '" + login.Trim() + "' ";
                if (check)
                    SyB_Acc.SQLCommand.CommandText += ", @assign=1";
                else
                    SyB_Acc.SQLCommand.CommandText += ", @assign=0";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 41", e.StackTrace);
                b = false;
            }
            if (ROLE.ToLower().Trim().CompareTo("security") == 0)
                try
                {
                    if (check)
                    {
                        SyB_Acc.SQLCommand.CommandText = "exec sys.sp_addrolemember @rolename='db_owner', @membername='" + login.Trim() + "'";
                        SyB_Acc.SQLCommand.ExecuteNonQuery();
                        SyB_Acc.SQLCommand.CommandText = "EXEC master..sp_addsrvrolemember @loginame = N'" + login.Trim() + "', @rolename = N'securityadmin'";
                                           SyB_Acc.SQLCommand.ExecuteNonQuery();
} 
                    else
                    {
                        SyB_Acc.SQLCommand.CommandText = "exec sys.sp_droprolemember 'db_owner', '" + login.Trim() + "'";
                        SyB_Acc.SQLCommand.ExecuteNonQuery();
                        SyB_Acc.SQLCommand.CommandText = "EXEC master..sp_dropsrvrolemember @loginame = N'" + login.Trim() + "', @rolename = N'securityadmin'";
                        SyB_Acc.SQLCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception e) 
                {
                    SyB_Acc.EBBLP.AddError(e.Message, "Command - 41.1", e.StackTrace);
                }
            return b;
       }

        public static void roles_InstructionAddRemove(int instruction, int ROLE, bool check)
        {
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                if (check)
                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.sec_roles_for_instructions (instruction,role) values("+instruction.ToString()+","+ROLE.ToString()+")";
                else
                    SyB_Acc.SQLCommand.CommandText = "delete dbo.sec_roles_for_instructions where instruction =" + instruction.ToString() + " and role=" + ROLE.ToString() ;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42", e.StackTrace);
            }
        }

        public static bool Change_password(string password)
        {
            bool res = true;
            try
            {
                
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                //SyB_Acc.SQLCommand.CommandText = "Alter LOGIN " + SyB_Acc.Login_name + " WITH password=@password";
                //System.Data.SqlClient.SqlParameter Par = SyB_Acc.SQLCommand.Parameters.Add("@password", SqlDbType.VarChar);
                //Par.Value = password;
                SyB_Acc.SQLCommand.CommandText = "Alter LOGIN " + SyB_Acc.Login_name + " WITH password='" + password + "'";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.1", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool FastSendAnswer( int answer)
        {
            bool ret = false;
            try
            {
                 SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_get_answer";
                SyB_Acc.SQLCommand.Parameters.Add("@answer", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = answer;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch ( Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.1-1", e.StackTrace);
                ret = false; 
            }
            return ret;
        }

        public static DateTime AlterDateExecuting(int moving, DateTime dat)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_moving set when_m=@when_m where moving=@moving";
                SyB_Acc.SQLCommand.Parameters.Add("@when_m", SqlDbType.DateTime);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = dat;
                SyB_Acc.SQLCommand.Parameters.Add("@moving", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = moving;
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "select when_m from dbo.rkk_moving where moving=@moving";
                SyB_Acc.SQLCommand.Parameters.Add("@moving", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = moving;
                System.Data.SqlClient.SqlDataReader reader =  SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                    if(reader.Read())
                    dt = reader.GetDateTime(0);
                reader.Close();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.1-2", e.StackTrace);
                dt = DateTime.MinValue;
            }
            return dt;
        }

        public static bool KillAnswer(int moving)
        {
            bool res = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_moving set exe_doc = null, executed = null where moving=@moving ";
                SyB_Acc.SQLCommand.Parameters.Add("@moving", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = moving;

                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.1-3", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool KillSend(int moving)
        {
            bool res = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "delete dbo.rkk_moving where moving=@moving ";
                SyB_Acc.SQLCommand.Parameters.Add("@moving", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = moving;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.1-4", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool KillSendViza(int id)
        {
            bool res = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "delete dbo.rkk_vizing where id=@id ";
                SyB_Acc.SQLCommand.Parameters.Add("@id", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = id;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.1-4.1", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool KillSendNews(int news)
        {
            bool res = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "delete dbo.rkk_news where news=@news ";
                SyB_Acc.SQLCommand.Parameters.Add("@news", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = news;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.1-4.2", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool SetMainExecutor(int moving)
        {
            bool res = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_set_main_executor ";
                    SyB_Acc.SQLCommand.Parameters.Add("@moving", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[0].Value = moving;

                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.1-5", e.StackTrace);
                res = false;
            }
        return res;
        }

        
        public static bool RankDegree(int Employee, int degree)
        {
            bool res = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.EMP_Rank_replace " ;
                    SyB_Acc.SQLCommand.Parameters.Add("@new_employee", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[0].Value = Employee;
                    SyB_Acc.SQLCommand.Parameters.Add("@rank", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[1].Value = degree;

                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.2", e.StackTrace);
                res = false;
            }
        return res;
    }

        public static bool UnrankDegree(int rank)
        {
            bool res = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.Emp_rank_deplace "  ;
                SyB_Acc.SQLCommand.Parameters.Add("@rank", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = rank;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 42.3", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static Array Roles_Employee_list(string employee)
        {
            Array roles=null;
            SyB_Acc.SQLCommand.Parameters.Clear();
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.sec_allroles @user = '" + employee + "'";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int rows = 0;
                    while (reader.Read())
                    {
                        rows++;
                    }
                    roles = Array.CreateInstance(typeof(string), rows, 4);
                    reader.Close();
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    rows = 0;
                    while (reader.Read())
                    {
                        try { roles.SetValue(reader.GetValue(0), rows, 0); }
                        catch { }
                        try {roles.SetValue(reader.GetValue(1), rows, 1);}
                        catch { }
                        try { roles.SetValue(reader.GetValue(3).ToString(), rows, 2); }
                        catch { }
                        try { roles.SetValue(reader.GetValue(4).ToString(), rows, 3); }
                        catch { }
                        rows++;
                    }
                }
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 43", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static ArrayList SubKindTema_list(int kind)
        {
            ArrayList roles = null;
            CommonClass.ArrayThree Ath = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select RKK_tema.tema, RKK_tema.description_,dbo.RKK_FileToTema(RKK_tema.tema," + kind.ToString() + ",0) from dbo.RKK_tema join dbo.RKK_tema_for_kind on RKK_tema.tema=RKK_tema_for_kind.tema where RKK_tema_for_kind.kind=" + kind.ToString() + " order by description_";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int id;
                    string name;
                    roles = new ArrayList();
                    while (reader.Read())
                    {
                        id = (int)reader.GetValue(0);
                        name = (string)reader.GetValue(1);
                        Ath = new CommonClass.ArrayThree(name.Trim(), id);
                        Ath.FId = (int)reader.GetValue(2);
                        roles.Add(Ath);
                    }
                }
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 44", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static ArrayList MySubKindTema_list(int kind)
        {
            ArrayList roles = null;
            CommonClass.ArrayThree Ath = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select EMP_temy.tema, EMP_temy.description_,dbo.RKK_FileToTema(EMP_temy.tema," + kind.ToString() + ",1) from dbo.EMP_temy join dbo.RKK_tema_for_kind on EMP_temy.tema=RKK_tema_for_kind.tema where RKK_tema_for_kind.kind=" + kind.ToString() + " order by description_";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int id;
                    string name;
                    roles = new ArrayList();
                    while (reader.Read())
                    {
                        id = (int)reader.GetValue(0);
                        name = (string)reader.GetValue(1);
                        Ath = new CommonClass.ArrayThree(name.Trim(), id);
                        Ath.FId = (int)reader.GetValue(2);
                        roles.Add(Ath);
                    }
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 44-1", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static byte[] GetFromDotLibrary(int kind, int tema, bool MyLib)
        {
            byte[] res=null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                int cnt = 0;
                int len = 50000;
                byte[] buff = new byte[len];
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@kind", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = kind;
                if (MyLib)
                {
                    if (tema == 0)
                        SyB_Acc.SQLCommand.CommandText = "select ole_docum from dbo.RKK_Mydots_library where kind= @kind and tema is null";
                    else
                    {
                        SyB_Acc.SQLCommand.CommandText = "select ole_docum from dbo.RKK_Mydots_library where kind= @kind and tema =@tema";
                        SyB_Acc.SQLCommand.Parameters.Add("@tema", SqlDbType.Int);
                        SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[1].Value = tema;
                    }
                }
                else
                {
                    if (tema == 0)
                        SyB_Acc.SQLCommand.CommandText = "select ole_docum from dbo.RKK_dot_library where kind= @kind and tema is null";
                    else
                    {
                        SyB_Acc.SQLCommand.CommandText = "select ole_docum from dbo.RKK_dot_library where kind= @kind and tema =@tema";
                        SyB_Acc.SQLCommand.Parameters.Add("@tema", SqlDbType.Int);
                        SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[1].Value = tema;
                    }
                }
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    int i = 0;
                    byte[] newres;
                    do
                    {
                        cnt = (int)reader.GetBytes(0, i * len, buff, 0, len);
                        i++;
                        if (res != null)
                        {
                            newres = new byte[res.Length + cnt];
                            res.CopyTo(newres, 0);
                            if ((int)buff.LongLength == cnt)
                                buff.CopyTo(newres, res.Length);
                            else
                                for (int n = 0; n < cnt; n++)
                                    newres[n + res.Length] = buff[n];
                        }
                        else
                        {
                            newres = new byte[cnt];
                            if ((int)buff.LongLength == cnt)
                                buff.CopyTo(newres, 0);
                            else
                                for (int n = 0; n < cnt; n++)
                                    newres[n] = buff[n];
                        }
                        res = newres;
                    }
                    while (cnt == len);
                }
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 45", ex.StackTrace);
                res = null; if (reader != null) reader.Close();
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return res;
        }

        public static bool AddtoDOTLibrary(int kind, int tema, byte[] File)
        {
            bool b = true;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@kind", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = kind;
                int cnt = 0;
                if (tema == 0)
                {
                    SyB_Acc.SQLCommand.CommandText = "select count(*) as cnt from dbo.RKK_dot_library where kind= "+kind.ToString()+" and tema is null";
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    reader.Read();
                    cnt = (int)reader.GetValue(0);
                    reader.Close();
                    if (cnt == 0)
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_dot_library (kind,ole_docum) values (@kind,@ole_docum)";
                    else
                        SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_dot_library set ole_docum=@ole_docum where kind= @kind and tema is null";
                    SyB_Acc.SQLCommand.Parameters.Add("@ole_docum", SqlDbType.Image);
                    SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[1].Value = File;
                }
                else
                {
                    SyB_Acc.SQLCommand.CommandText = "select count(*) as cnt from dbo.RKK_dot_library where kind= @kind and tema=@tema";
                    SyB_Acc.SQLCommand.Parameters.Add("@tema", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[1].Value = tema;
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    reader.Read();
                    cnt = (int)reader.GetValue(0);
                    reader.Close();
                    if (cnt == 0)
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_dot_library (kind,ole_docum,tema) values (@kind,@ole_docum,@tema)";
                    else
                        SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_dot_library set ole_docum=@ole_docum where kind=@kind and tema=@tema";
                    SyB_Acc.SQLCommand.Parameters.Add("@ole_docum", SqlDbType.Image);
                    SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[2].Value = File;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception ex) 
            { 
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 45", ex.StackTrace);
                b = false; if (reader!=null) reader.Close();
            }
            return b;
        }

        public static bool AddtoMYDOTLibrary(int kind, int tema, byte[] File)
        {
            bool b = true;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@kind", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = kind;
                int cnt = 0;
                if (tema == 0)
                {
                    SyB_Acc.SQLCommand.CommandText = "select count(*) as cnt from dbo.RKK_MYdots_library where kind= " + kind.ToString() + " and tema is null";
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    reader.Read();
                    cnt = (int)reader.GetValue(0);
                    reader.Close();
                    if (cnt == 0)
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_MYdots_library (kind,ole_docum) values (@kind,@ole_docum)";
                    else
                        SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_MYdots_library set ole_docum=@ole_docum where kind= @kind and tema is null";
                    SyB_Acc.SQLCommand.Parameters.Add("@ole_docum", SqlDbType.Image);
                    SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[1].Value = File;
                }
                else
                {
                    SyB_Acc.SQLCommand.CommandText = "select count(*) as cnt from dbo.RKK_MYdots_library where kind= @kind and tema=@tema";
                    SyB_Acc.SQLCommand.Parameters.Add("@tema", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[1].Value = tema;
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    reader.Read();
                    cnt = (int)reader.GetValue(0);
                    reader.Close();
                    if (cnt == 0)
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_MYdots_library (kind,ole_docum,tema) values (@kind,@ole_docum,@tema)";
                    else
                        SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_MYdots_library set ole_docum=@ole_docum where kind=@kind and tema=@tema";
                    SyB_Acc.SQLCommand.Parameters.Add("@ole_docum", SqlDbType.Image);
                    SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[2].Value = File;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 45", ex.StackTrace);
                b = false; if (reader != null) reader.Close();
            }
            return b;
        }

        public static Array TemaForKind_list(int kind)
        {
            Array roles = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select RKK_tema.tema, RKK_tema.description_, 0 as assg from dbo.RKK_tema  where tema not in (select tema from dbo.RKK_tema_for_kind  where RKK_tema_for_kind.kind=" + kind.ToString() + " )";
                SyB_Acc.SQLCommand.CommandText += " union ";
                SyB_Acc.SQLCommand.CommandText += "select RKK_tema.tema, RKK_tema.description_, 1 as assg from dbo.RKK_tema join dbo.RKK_tema_for_kind on RKK_tema.tema=RKK_tema_for_kind.tema where RKK_tema_for_kind.kind=" + kind.ToString() ;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int id;
                    int rows = 0;
                    while (reader.Read()) rows++;
                    roles = Array.CreateInstance(typeof(string), rows, 3);
                    reader.Close();
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    rows = 0;
                    while (reader.Read())
                    {
                        id = (int)reader.GetValue(0);
                        roles.SetValue(id.ToString(), rows, 0);
                        roles.SetValue((string)reader.GetValue(1), rows, 1);
                        if ((int)reader.GetValue(2) == 0) roles.SetValue("0", rows, 2);
                        else roles.SetValue("1", rows, 2);
                        rows++;
                    }
                }
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 46", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static void ADDelTemaForKind(int kind,int tema, bool check)
        {
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                if (check)
                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_tema_for_kind (kind,tema) values(" + kind.ToString() + "," + tema.ToString() + ")";
                else
                    SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_tema_for_kind where kind =" + kind.ToString() + " and tema=" + kind.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 47", e.StackTrace);
            }

        }

        public static void MultiSendChange(int kind)
        {
            SyB_Acc.SQLCommand.CommandType = CommandType.Text;
            try
            {
                SyB_Acc.SQLCommand.CommandText = "exec dbo.RKK_one_answer_change @kind=" + kind.ToString() ;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 48", e.StackTrace);
            }
        }
        public static Array MultiSendKind()
        {
            Array roles = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select kod, kind, one_answer from dbo.RKK_kind";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int rows = 0;
                    while (reader.Read())
                    {
                        rows++;
                    }
                    roles = Array.CreateInstance(typeof(string), rows, 3);
                    reader.Close();
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    rows = 0;
                    while (reader.Read())
                    {
                        roles.SetValue(reader.GetValue(0).ToString(), rows, 0);
                        roles.SetValue(reader.GetValue(1), rows, 1);
                        roles.SetValue(reader.GetValue(2).ToString(), rows, 2);
                        rows++;
                    }
                }
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 49", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static int AddSign(string sign)
        {
            int res = 0;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "declare @dt datetime ;set @dt=getdate(); insert into dbo.RKK_wfl_resolution(sign) values(@sign); ";
                SyB_Acc.SQLCommand.CommandText += "select top 1 id from dbo.RKK_wfl_sign order by id desc; ";
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("sign", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value=sign;
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 49-1", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static bool DeleteSign(int id)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = " delete dbo.RKK_wfl_resolution where id= " + id.ToString();
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                 res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 49-1", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static ArrayList A_resolutions_list(int enumerator)
        {
            ArrayList roles = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Archive:
                        SyB_Acc.SQLCommand.CommandText = "select id, goal from dbo.RKK_wfl_outdocs order by goal";
                        break;
                    case (int)CommonClass.Lists.Kind:
                        SyB_Acc.SQLCommand.CommandText = "select kod, kind from dbo.RKK_kind order by kind";
                        break;
                    case (int)CommonClass.Lists.WellKind:
                        SyB_Acc.SQLCommand.CommandText = "select kod, kind from dbo.RKK_kind where WelInOut=1 order by kind";
                        break;
                    case (int)CommonClass.Lists.OutKind:
                        SyB_Acc.SQLCommand.CommandText = "select kod, kind from dbo.RKK_kind where WelInOut=2 order by kind";
                        break;
                    case (int)CommonClass.Lists.Tema:
                        SyB_Acc.SQLCommand.CommandText = "select tema, description_ from dbo.RKK_tema order by description_";
                        break;
                    case (int)CommonClass.Lists.Coming:
                        SyB_Acc.SQLCommand.CommandText = "select cod, Coming from dbo.RKK_Coming order by Coming";
                        break;
                    case (int)CommonClass.Lists.Prefix:
                        SyB_Acc.SQLCommand.CommandText = "select RKK_prefix.kod, case when rkk_delo.delo is not null then  RKK_prefix.prefix+  '      [ журнал '+rtrim(rkk_delo.delo)+']' else RKK_prefix.prefix end from dbo.RKK_prefix left join dbo.rkk_delo on RKK_prefix.delo=rkk_delo.cod order by prefix";
                        break;
                    case (int)CommonClass.Lists.Suffix:
                        SyB_Acc.SQLCommand.CommandText = "select id, suffix from dbo.RKK_suffix order by suffix";
                        break;
                    case (int)CommonClass.Lists.Viza:
                        SyB_Acc.SQLCommand.CommandText = "select id, viza_yes + ' / '+ viza_no as viza  from dbo.RKK_vizing_list order by viza_yes";
                        break;
                    case (int)CommonClass.Lists.Delo:
                        SyB_Acc.SQLCommand.CommandText = "select cod as id, delo  from dbo.RKK_delo order by delo";
                        break;
                }
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int id;
                    string name;
                    roles = new ArrayList();
                    while (reader.Read())
                    {
                        id = (int)reader.GetValue(0);
                        name=(string)reader.GetValue(1);
                        roles.Add(new CommonClass.Arraysheet(name.Trim(), id));
                    }
                }
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 50", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static ArrayList Resolutions_Refresh()
        {
            ArrayList roles = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select id, sign from dbo.RKK_wfl_sign order by sign";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int id;
                    string name;
                    roles = new ArrayList();
                    while (reader.Read())
                    {
                        id = (int)reader.GetValue(0);
                        name = (string)reader.GetValue(1);
                        roles.Add(new CommonClass.Arraysheet(name.Trim(), id));
                    }
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 50.0.1", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static bool MeAnswer(int document)
        {
            bool ret = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select count (*) from dbo.rkk_moving where parent=dbo.user_ident() and rkk_moving.exe_doc= " + document.ToString();
                int cnt = (int)SyB_Acc.SQLCommand.ExecuteScalar();
                if (cnt > 0)
                    ret = true;
                else ret = false;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 50.1", e.StackTrace);
                ret = false; 
            }
            return ret;
        }

        public static bool DocCorrecting(int document)
        {
            bool ret = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select count (*) from dbo.rkk_document_correct where document = " + document.ToString();
                int cnt = (int)SyB_Acc.SQLCommand.ExecuteScalar();
                if (cnt > 0)
                    ret = true;
                else ret = false;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 50.1.1", e.StackTrace);
                ret = false;
            }
            return ret;
        }

        public static ArrayList MyKinds_list()
        {
            ArrayList roles = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                        SyB_Acc.SQLCommand.CommandText = "select kod, kind from dbo.EMP_kinds order by kind";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int id;
                    string name;
                    roles = new ArrayList();
                    while (reader.Read())
                    {
                        id = (int)reader.GetValue(0);
                        name = (string)reader.GetValue(1);
                        roles.Add(new CommonClass.Arraysheet(name.Trim(), id));
                    }
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 50.2", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }


        public static bool AddThe_resolution(int enumerator, string goal)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@goal", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = goal;
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Archive:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_wfl_outdocs (goal) values (@goal)";
                        break;
                    case (int)CommonClass.Lists.Kind:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_kind (kind) values (@goal)";
                        break;
                    case (int)CommonClass.Lists.Tema:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_tema (description_) values (@goal)";
                        break;
                    case (int)CommonClass.Lists.Coming:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_Coming (Coming) values (@goal)";
                        break;
                    case (int)CommonClass.Lists.Prefix:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_prefix (prefix) values (@goal)";
                        break;
                    case (int)CommonClass.Lists.Suffix:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_suffix (suffix) values (@goal)";
                        break;
                    case (int)CommonClass.Lists.Viza:
                        string vizaY = goal.Substring(0, goal.IndexOf('/')-1).Trim();
                        if (goal.Length > vizaY.Length + 1)
                            goal = goal.Substring(vizaY.Length + 1).Trim();
                        else goal = "";
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_vizing_list (viza_yes, viza_no) values (@VizaY,@goal)";
                        SyB_Acc.SQLCommand.Parameters.Add("@VizaY", SqlDbType.VarChar);
                        SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[0].Value = goal;
                        SyB_Acc.SQLCommand.Parameters[1].Value = vizaY;
                        break;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 51", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool DeleteThe_resolution(int enumerator, int id)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Archive:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_wfl_outdocs where id="+id.ToString();
                        break;
                    case (int)CommonClass.Lists.Kind:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_kind where kod=" + id.ToString();
                        break;
                    case (int)CommonClass.Lists.Tema:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_tema where tema=" + id.ToString();
                        break;
                    case (int)CommonClass.Lists.Coming:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_Coming where cod=" + id.ToString();
                        break;
                    case (int)CommonClass.Lists.Prefix:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_prefix where kod=" + id.ToString();
                        break;
                    case (int)CommonClass.Lists.Suffix:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_suffix where id=" + id.ToString();
                        break;
                    case (int)CommonClass.Lists.Viza:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.RKK_vizing_list where id=" + id.ToString();
                        break;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 52", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static Array AssignWFL_list(int enumerator, int Degree)
        {
            Array roles = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Kind:
                        SyB_Acc.SQLCommand.CommandText = "select kod, kind, 0 as b from dbo.RKK_kind where kod not in (select kind from dbo.emp_kind where employee =" + Degree.ToString() + ")";
                        SyB_Acc.SQLCommand.CommandText += " union ";
                        SyB_Acc.SQLCommand.CommandText += "select kod, kind, 1 as b from dbo.RKK_kind where kod in (select kind from dbo.emp_kind where employee =" + Degree.ToString() + ")";
                        break;
                    case (int)CommonClass.Lists.Tema:
                        SyB_Acc.SQLCommand.CommandText = "select tema, description_, 0 as b from dbo.RKK_tema where tema not in (select tema from dbo.emp_tema where employee =" + Degree.ToString() + ")";
                        SyB_Acc.SQLCommand.CommandText += " union ";
                        SyB_Acc.SQLCommand.CommandText += "select tema, description_, 1 as b from dbo.RKK_tema where tema in (select tema from dbo.emp_tema where employee =" + Degree.ToString() + ")";
                        break;
                    case (int)CommonClass.Lists.Coming:
                        SyB_Acc.SQLCommand.CommandText = "select cod, Coming, 0 as b from dbo.RKK_Coming where cod not in (select Coming from dbo.emp_Coming where employee =" + Degree.ToString() + ")";
                        SyB_Acc.SQLCommand.CommandText += " union ";
                        SyB_Acc.SQLCommand.CommandText += "select cod, Coming, 1 as b from dbo.RKK_Coming where cod in (select Coming from dbo.emp_Coming  where employee =" + Degree.ToString() + ")";
                        break;
                }
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int rows = 0;
                    while (reader.Read())
                    {
                        rows++;
                    }
                    roles = Array.CreateInstance(typeof(string), rows, 3);
                    reader.Close();
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    rows = 0;
                    while (reader.Read())
                    {
                        roles.SetValue(reader.GetValue(0).ToString(), rows, 0);
                        roles.SetValue(reader.GetValue(1), rows, 1);
                        roles.SetValue(reader.GetValue(2).ToString(), rows, 2);
                        rows++;
                    }
                }
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 53", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static bool AddWFL_assign(int enumerator, int assign, int employee)
        {
            bool b = false;
            try
            {
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Kind:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.emp_kind (kind,employee) values (" + assign.ToString() + "," + employee.ToString() + ")";
                        break;
                    case (int)CommonClass.Lists.Tema:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.emp_tema (tema,employee) values (" + assign.ToString() + "," + employee.ToString() + ")";
                        break;
                    case (int)CommonClass.Lists.Coming:
                        SyB_Acc.SQLCommand.CommandText = "insert into dbo.emp_Coming (Coming,employee) values (" + assign.ToString() + "," + employee.ToString()+ ")";
                        break;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 54", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool DeleteWFL_assign(int enumerator, int assign, int employee)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Kind:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.emp_kind where kind=" + assign.ToString() + " and employee=" + employee.ToString() ;
                        break;
                    case (int)CommonClass.Lists.Tema:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.emp_tema where tema=" + assign.ToString() + " and employee=" + employee.ToString() ;
                        break;
                    case (int)CommonClass.Lists.Coming:
                        SyB_Acc.SQLCommand.CommandText = "delete dbo.emp_Coming where Coming=" + assign.ToString() + " and employee=" + employee.ToString() ;
                        break;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 55", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static Array PS_list(int enumerator, int PS_ID, int WelInOut)
        {
            Array roles = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Prefix:
                        SyB_Acc.SQLCommand.CommandText = "select kod, kind, 0 as b from dbo.RKK_kind where prefix is null and WelInOut=" + WelInOut.ToString();
                        SyB_Acc.SQLCommand.CommandText += " union ";
                        SyB_Acc.SQLCommand.CommandText += "select kod, kind, 1 as b from dbo.RKK_kind where prefix=" + PS_ID.ToString() + " and WelInOut=" + WelInOut.ToString();
                        SyB_Acc.SQLCommand.CommandText += " union ";
                        SyB_Acc.SQLCommand.CommandText += "select kod, kind, 2 as b from dbo.RKK_kind where prefix is not null and prefix<>" + PS_ID.ToString() + " and WelInOut=" + WelInOut.ToString();
                        break;
                    case (int)CommonClass.Lists.Suffix:
                        SyB_Acc.SQLCommand.CommandText = "select tema, description_, 0 as b from dbo.RKK_tema where suffix is null" ;
                        //SyB_Acc.SQLCommand.CommandText = "select tema, description_, 0 as b from dbo.RKK_tema where suffix is null  and WelInOut=" + WelInOut.ToString();
                        //SyB_Acc.SQLCommand.CommandText += " union ";
                        //SyB_Acc.SQLCommand.CommandText += "select tema, description_, 1 as b from dbo.RKK_tema where suffix=" + PS_ID.ToString() + " and WelInOut=" + WelInOut.ToString();
                        //SyB_Acc.SQLCommand.CommandText += " union ";
                        //SyB_Acc.SQLCommand.CommandText += "select tema, description_, 2 as b from dbo.RKK_tema where suffix is not null and suffix<>" + PS_ID.ToString() + " and WelInOut=" + WelInOut.ToString();
                        break;
                }
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int rows = 0;
                    while (reader.Read())
                    {
                        rows++;
                    }
                    roles = Array.CreateInstance(typeof(string), rows, 3);
                    reader.Close();
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    rows = 0;
                    while (reader.Read())
                    {
                        roles.SetValue(reader.GetValue(0).ToString(), rows, 0);
                        roles.SetValue(reader.GetValue(1), rows, 1);
                        roles.SetValue(reader.GetValue(2).ToString(), rows, 2);
                        rows++;
                    }
                }
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 56", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        public static bool Add_PS(int enumerator, int KT, int PS)
        {
            bool b = false;
            try
            {
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Prefix:
                        SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_kind set prefix=" + PS.ToString() + " where kod=" + KT.ToString() ;
                        break;
                    case (int)CommonClass.Lists.Suffix:
                        SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_tema set suffix=" + PS.ToString() + " where tema=" + KT.ToString() ;
                        break;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 57", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool Remove_PS(int enumerator, int KT)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                switch (enumerator)
                {
                    case (int)CommonClass.Lists.Prefix:
                        SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_kind set prefix=null where kod=" + KT.ToString();
                        break;
                    case (int)CommonClass.Lists.Suffix:
                        SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_tema set suffix=null where tema=" + KT.ToString();
                        break;
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 58", e.StackTrace);
                b = false;
            }
            return b;
        }

        public static Array Roles_Instruction_list(int instruction)
        {
            Array roles = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select id,RoleDescription, checked from dbo.sec_roles_in_instruction where  instruction = " + instruction.ToString();
                SyB_Acc.SQLCommand.CommandText += " union ";
                SyB_Acc.SQLCommand.CommandText += "select id,RoleDescription, 0 as checked from dbo.sec_roles where id not in ( select id from dbo.sec_roles_in_instruction where  instruction = " + instruction.ToString()+")";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    int rows = 0;
                    while (reader.Read())
                    {
                        rows++;
                    }
                    roles = Array.CreateInstance(typeof(string), rows, 3);
                    reader.Close();
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    rows = 0;
                    while (reader.Read())
                    {
                        roles.SetValue(reader.GetValue(0).ToString(), rows, 0);
                        roles.SetValue(reader.GetValue(1), rows, 1);
                        roles.SetValue(reader.GetValue(2).ToString(), rows, 2);
                        rows++;
                    }
                }
            }
            catch (Exception e) 
            { 
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 59", e.StackTrace);
                roles = null;
            }
            if (reader != null) reader.Close();
            return roles;
        }

        //document

        public static bool NullAutor_Recieved_document(int doc)
        {
            bool res = false;

            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "select count(*) from dbo.RKK_wellcome_document where autor is null and enterprice is null and leader is null and kod= "+doc.ToString();
                if((int)SyB_Acc.SQLCommand.ExecuteScalar()>0)
                    res = true;
                else res = false;
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 59.1", ex.StackTrace); res = false;
            }
            return res;
        }

        public static bool EditAutor_Recieved_document(int autor, int employee, int org, int doc)
        {
            bool res = false;
            int count = 0;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_wellcome_document set ";
                if (org > 0)
                {
                    SyB_Acc.SQLCommand.CommandText += " enterprice=@org";
                    SyB_Acc.SQLCommand.Parameters.Add("@org", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = org;
                    count++;
                    if (employee > 0)
                    {
                        if (count > 0) SyB_Acc.SQLCommand.CommandText += ",";
                        SyB_Acc.SQLCommand.CommandText += " leader=@leader";
                        SyB_Acc.SQLCommand.Parameters.Add("@leader", SqlDbType.Int);
                        SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[count].Value = employee;
                        count++;
                    }
                }
                if (autor > 0)
                {
                    if (count > 0) SyB_Acc.SQLCommand.CommandText += ",";
                    SyB_Acc.SQLCommand.CommandText += " autor=@autor";
                    SyB_Acc.SQLCommand.Parameters.Add("@autor", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = autor;
                    count++;
                }
                if (count > 0)
                {
                    SyB_Acc.SQLCommand.CommandText += " where kod=@doc";
                    SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = doc;
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 59.2", ex.StackTrace); res=false;
            }
            return res;
        }

        public static bool EditOutcoming_Recieved_document(string OutNumber, string dat,  int doc)
        {
            bool res = false;
            int count = 0;
            DateTime DatOut = DateTime.MinValue;
            if (dat.CompareTo("  .  .")!=0)
            try
            {
                DatOut = Convert.ToDateTime(dat);
            }
            catch
            {
                DatOut = DateTime.MaxValue;
            }
            if (OutNumber.Trim().Length > 0 || DatOut != DateTime.MinValue)
                try
                {
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.CommandText = "update dbo.RKK_wellcome_document set ";
                    if (OutNumber.Trim().Length > 0)
                    {
                        SyB_Acc.SQLCommand.CommandText += " outcoming=@outnum";
                        SyB_Acc.SQLCommand.Parameters.Add("@outnum", SqlDbType.Char);
                        SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[count].Value = OutNumber.Trim();
                        count++;
                    }
                    if (DatOut != DateTime.MinValue)
                    {
                        if (count > 0) SyB_Acc.SQLCommand.CommandText += ",";
                        SyB_Acc.SQLCommand.CommandText += " date_out=@datout";
                        SyB_Acc.SQLCommand.Parameters.Add("@datout", SqlDbType.DateTime);
                        SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[count].Value = DatOut;
                        count++;
                    }

                    SyB_Acc.SQLCommand.CommandText += " where kod=@doc";
                    SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = doc;
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                    res = true;
                }
                catch (Exception ex)
                {
                    SyB_Acc.EBBLP.AddError(ex.Message, "Command - 59.3", ex.StackTrace); res = false;
                }
            return res;
        }

        public static int Append_Recieved_document(int kind, int org, int employee, int autor, string outCod, string to_date,int tema, int coming, string annotation, int parentDoc)
        {
            int Ident = Add_FlowDocument(kind, parentDoc, annotation);
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                int count = 0;
                string sql = "";
                if (Ident > 0)
                {
                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_wellcome_document (kod";
                    sql = " values(@kod";
                    SyB_Acc.SQLCommand.Parameters.Add("@kod", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = Ident;
                    count++;
                    if (org > 0)
                    {
                        SyB_Acc.SQLCommand.CommandText += ",enterprice";
                        sql += ",@org";
                        SyB_Acc.SQLCommand.Parameters.Add("@org", SqlDbType.Int);
                        SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[count].Value = org;
                        count++;
                        if (employee > 0)
                        {
                            SyB_Acc.SQLCommand.CommandText += ",leader";
                            sql += ",@leader";
                            SyB_Acc.SQLCommand.Parameters.Add("@leader", SqlDbType.Int);
                            SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                            SyB_Acc.SQLCommand.Parameters[count].Value = employee;
                            count++;
                        }
                    }
                    if (autor > 0)
                    {
                        SyB_Acc.SQLCommand.CommandText += ",autor";
                        sql += ",@autor";
                        SyB_Acc.SQLCommand.Parameters.Add("@autor", SqlDbType.Int);
                        SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[count].Value = autor;
                        count++;
                    }
                    if (outCod.Length > 0)
                    {
                        SyB_Acc.SQLCommand.CommandText += ",outcoming";
                        sql += ",@outcoming";
                        SyB_Acc.SQLCommand.Parameters.Add("@outcoming", SqlDbType.VarChar);
                        SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[count].Value = outCod;
                        count++;
                    }
                    try
                    {
                        DateTime Dot;
                        Dot = (DateTime)Convert.ToDateTime(to_date);
                        SyB_Acc.SQLCommand.CommandText += ",date_out";
                        sql += ",@date";
                        SyB_Acc.SQLCommand.Parameters.Add("@date", SqlDbType.DateTime);
                        SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[count].Value = Dot;
                        count++;
                    }
                    catch {  }

                    SyB_Acc.SQLCommand.CommandText += ",tema";
                    sql += ",@tema";
                    SyB_Acc.SQLCommand.Parameters.Add("@tema", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = tema;
                    count++;

                    SyB_Acc.SQLCommand.CommandText += ",coming";
                    sql += ",@coming";
                    SyB_Acc.SQLCommand.Parameters.Add("@coming", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = coming;
                    count++;

                    SyB_Acc.SQLCommand.CommandText += ")" + sql + ")";

                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex) 
            { 
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 60", ex.StackTrace) ; Ident=-1;
            }
            return Ident;
        }

        public static int Append_Indoor_document(int kind, int tema, string annotation, int parentDoc)
        {
            int Ident = Add_FlowDocument(kind, parentDoc, annotation);
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                int count = 0;
                if (Ident > 0)
                {
                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_indoor_document (kod";
                    string sql = " values(@kod";
                    SyB_Acc.SQLCommand.Parameters.Add("@kod", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = Ident;
                    count++;
                    if (tema>0)
                    {
                        sql += ",@tema";
                        SyB_Acc.SQLCommand.CommandText += ",tema";
                        SyB_Acc.SQLCommand.Parameters.Add("@tema", SqlDbType.Int);
                        SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[count].Value = tema;
                    }
                    SyB_Acc.SQLCommand.CommandText += ")" + sql + ")";
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex) 
            { 
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 61", ex.StackTrace);
            }
            return Ident;
        }

        private static bool MainExecutor(int General_Document)
        {
            bool res=false;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select count(moving) as cnt from dbo.rkk_moving where rkk_moving.document=" + General_Document.ToString() + " and rkk_moving.pattern=dbo.user_ident() and rkk_moving.main_executor=1";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (!reader.HasRows) res = true;
                reader.Close();
            }
            catch 
            {
                res=false;
            }
            return res;
        }

        public static bool AnswerDocument(int document, int moving)
        {
            bool res=false;
            if (moving>0)
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_moving set exe_doc="+document.ToString()+", executed=getdate() where moving=" + moving.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 63", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static int Add_FlowDocument(int kind, int parentDoc, string annotation)
        {
            int Ident = 0;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                int count = 0;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.rkk_adding_flow_document ";
                //if (kind > 0)
                {
                    SyB_Acc.SQLCommand.CommandText += " @kind";
                    SyB_Acc.SQLCommand.Parameters.Add("@kind", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = kind;
                    count++;
                }
                //if (annotation.Trim().Length > 0)
                {
                    if (count > 0) SyB_Acc.SQLCommand.CommandText += " ,@annot";
                    else SyB_Acc.SQLCommand.CommandText += " @annot";
                    SyB_Acc.SQLCommand.Parameters.Add("@annot", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = annotation.Trim();
                    count++;
                }
                //if (parentDoc > 0)
                {
                    if (count > 0) SyB_Acc.SQLCommand.CommandText += " ,@parrentDocument";
                    else SyB_Acc.SQLCommand.CommandText += " @parrentDocument";
                    SyB_Acc.SQLCommand.Parameters.Add("@parrentDocument", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = parentDoc;
                    count++;
                }
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                reader.Read();
                Ident = (int)reader.GetValue(0);
                reader.Close();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 64", ex.StackTrace);
                Ident = 0;
            }
            try { reader.Close(); }
            catch {}
            return Ident;
        }

        public static bool Edit_FlowDocument(int doc,  string annotation)
        {
            bool ret = true;
            try
            {
                int count = 0;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.rkk_edit_flow_document ";
                //if (kind > 0)
                {
                    SyB_Acc.SQLCommand.CommandText += " @doc";
                    SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = doc;
                    count++;
                }
                //if (annotation.Trim().Length > 0)
                {
                    if (count > 0) SyB_Acc.SQLCommand.CommandText += " ,@annot";
                    else SyB_Acc.SQLCommand.CommandText += " @annot";
                    SyB_Acc.SQLCommand.Parameters.Add("@annot", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[count].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[count].Value = annotation.Trim();
                }
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 64-1", ex.StackTrace);
                ret = false;
            }
            return ret;
        }

        public static bool Append_formular(int document, Array Formular)
        {
            bool b=true;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters.Add("@docname", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters.Add("@lists", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters.Add("@denote", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[3].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.RKK_formular_append @doc, @docname, @denote, @lists";
                int count = Formular.Length / 3;
                for (int i = 0; i < count; i++)
                {
                    SyB_Acc.SQLCommand.Parameters[0].Value = document;
                    SyB_Acc.SQLCommand.Parameters[1].Value = Formular.GetValue(i,0);
                    SyB_Acc.SQLCommand.Parameters[2].Value = (int) Convert.ToInt32( Formular.GetValue(i, 1));
                    SyB_Acc.SQLCommand.Parameters[3].Value = Formular.GetValue(i, 2);
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }

            }
            catch (Exception ex) 
            { 
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 66", ex.StackTrace);
                b = false;
            }
            return b;
        }

        public static bool MyDocument(int document)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = document;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select count(*) as cnt from dbo.RKK_flow_document where kod=@doc and typist=dbo.user_ident()";
                int cnt = (int)SyB_Acc.SQLCommand.ExecuteScalar();
                if (cnt > 0) b = true;
                else b = false;
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 66-1", ex.StackTrace);
                b = false;
            }
            return b;
        }

        public static string Move_formular(int Id, string denote)
        {
            string man = "";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@id", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters.Add("@denote", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec dbo.RKK_Formular_send @id ,@denote ";
                SyB_Acc.SQLCommand.Parameters[0].Value = Id;
                SyB_Acc.SQLCommand.Parameters[1].Value = denote;
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                int typist = 0;
                try
                {
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.CommandText = "select top 1 who_have from dbo.RKK_flow_formular where id=" + Id.ToString();
                    System.Data.SqlClient.SqlDataReader reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    reader.Read();
                    if (reader.HasRows) typist = reader.GetInt32(0);
                    reader.Close();
                }
                catch //(Exception ex)
                {
                    typist = 0;
                }

                if (typist > 0)
                {
                    try
                    {
                        SyB_Acc.SQLCommand.Parameters.Clear();
                        SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                        SyB_Acc.SQLCommand.CommandText = "exec dbo.pop_the_man_by_id @id ";
                        SyB_Acc.SQLCommand.Parameters.Add("@id", SqlDbType.Int);
                        SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters[0].Value = typist;
                        System.Data.SqlClient.SqlDataReader reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                        reader.Read();
                        if (reader.HasRows) man = reader.GetString (0);
                        reader.Close();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 66.1", ex.StackTrace);
                man = "";
            }
            return man;
        }

        public static bool Append_Content(int document, byte[] stuff)
        {
            bool b = true;
            if(stuff!=null)
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.RKK_stuff (document,ole_doc) values (@doc, @content)";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters.Add("@content", SqlDbType.Image);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = document;
                SyB_Acc.SQLCommand.Parameters[1].Value = stuff;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception ex) 
            { 
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 67", ex.StackTrace);
                b = false;
            }
            else b = false;
            return b;
        }

        public static bool Append_Correct(int document, int page, byte[] stuff)
        {
            bool b = true;
            if (stuff != null)
                try
                {
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.rkk_document_correct (document,page,correct) values (@doc, @page, @correct)";
                    SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters.Add("@page", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters.Add("@correct", SqlDbType.Image);
                    SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[0].Value = document;
                    SyB_Acc.SQLCommand.Parameters[1].Value = page;
                    SyB_Acc.SQLCommand.Parameters[2].Value = stuff;
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SyB_Acc.EBBLP.AddError(ex.Message, "Command - 67.correct1", ex.StackTrace);
                    b = false;
                }
            else b = false;
            return b;
        }

        public static bool Edit_Content(int document, byte[] stuff)
        {
            bool b = true;
            if (stuff != null)
                try
                {
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                    SyB_Acc.SQLCommand.CommandText = "dbo.RKK_edit_stuff";
                    SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters.Add("@content", SqlDbType.Image);
                    SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[0].Value = document;
                    SyB_Acc.SQLCommand.Parameters[1].Value = stuff;
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SyB_Acc.EBBLP.AddError(ex.Message, "Command - 67-1", ex.StackTrace);
                    b = false;
                }
            else b = false;
            return b;
        }

        public static bool Update_Content(int document, byte[] stuff)
        {
            bool b = true;
            if (stuff != null)
                try
                {
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.CommandText = "update  dbo.RKK_stuff set ole_doc=@content where document=@doc";
                    SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters.Add("@content", SqlDbType.Image);
                    SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[0].Value = document;
                    SyB_Acc.SQLCommand.Parameters[1].Value = stuff;
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SyB_Acc.EBBLP.AddError(ex.Message, "Command - 67.1", ex.StackTrace);
                    b = false;
                }
            else b = false;
            return b;
        }

        public static int GetParentDoc(int doc)
        {
            int ret = 0;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select top 1 document from dbo.rkk_moving where exe_doc=" + doc.ToString();
                System.Data.SqlClient.SqlDataReader reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                reader.Read();
                if (reader.HasRows) ret = reader.GetInt32(0);
                reader.Close();
            }
            catch //(Exception ex)
            {
                //SyB_Acc.EBBLP.AddError(ex.Message, "Command - 68", ex.StackTrace);
                ret = 0;
            }
            return ret;
        }

        public static int DocumentTip(int document)
        {
            int tip =-1; 
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select cnt=count(*) from dbo.rkk_wellcome_document where kod=" + document.ToString();
                System.Data.SqlClient.SqlDataReader reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                reader.Read();
                if ((int)reader.GetValue(0) > 0) tip = (int)CommonClass.TypeofDocument.Wellcome;
                reader.Close();
                if (tip < 0)
                {
                    SyB_Acc.SQLCommand.CommandText = "select cnt=count(*)  from dbo.rkk_indoor_document where kod=" + document.ToString();
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    reader.Read();
                    if ((int)reader.GetValue(0) > 0) tip = (int)CommonClass.TypeofDocument.Indoor;
                    reader.Close();
                    if (tip < 0)
                    {
                        SyB_Acc.SQLCommand.CommandText = "select cnt=count(*)  from dbo.rkk_archive_document where kod=" + document.ToString();
                        reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                        reader.Read();
                        if ((int)reader.GetValue(0) > 0) tip = (int)CommonClass.TypeofDocument.Archive;
                        reader.Close();
                        if (tip < 0)
                        {
                            SyB_Acc.SQLCommand.CommandText = "select cnt=count(*)  from dbo.out_document where kod=" + document.ToString();
                            reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                            reader.Read();
                            if ((int)reader.GetValue(0) > 0) tip = (int)CommonClass.TypeofDocument.Outdoor;
                            reader.Close();
                            if (tip < 0)
                            {
                                SyB_Acc.SQLCommand.CommandText = "select cnt=count(*)  from dbo.rkk_alian_document where kod=" + document.ToString();
                                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                                reader.Read();
                                if ((int)reader.GetValue(0) > 0) tip = (int)CommonClass.TypeofDocument.Corporative;
                                reader.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                if(SyB_Acc!=null) SyB_Acc.EBBLP.AddError(ex.Message, "Command - 69", ex.StackTrace);
            }
            return tip;
        }

        public static string GetEmployeeById(int employee)
        {
            string FIO = "";
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.emp_get_employees_By_degree ";
                int i = 0;
                SyB_Acc.SQLCommand.Parameters.Add("@degree", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[i].Value = employee;
                reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        FIO = (string)reader["fio"] ;
                }
            }
            catch (Exception ex)
            {
                if (SyB_Acc != null) SyB_Acc.EBBLP.AddError(ex.Message, "Command - 69-1", ex.StackTrace);
                FIO = "";
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return FIO;

        }

        public static string GetDocumentTypist(int document)
        {
            string FIO = "";
            System.Data.SqlClient.SqlDataReader reader=null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_GetDocumenTypist ";
                int i = 0;
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[i].Value = document;
                reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        FIO = (string)reader["family"] + " " + (string)reader["name"] + " " + (string)reader["father"];
                }
            }
            catch (Exception ex)
            {
                if (SyB_Acc != null) SyB_Acc.EBBLP.AddError(ex.Message, "Command - 69-1.1", ex.StackTrace);
                FIO = "";
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return FIO;
        }

        public static int GetMasterDocument(int doc)
        {
            int Master = 0;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "select master_document from dbo.RKK_flow_document where kod=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                reader = SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                        Master =reader.GetInt32( 0);
                }
            }
            catch (Exception ex)
            {
                if (SyB_Acc != null) SyB_Acc.EBBLP.AddError(ex.Message, "Command - 69-2", ex.StackTrace);
                Master = 0;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return Master;
        }

        public static int Document_moving(bool Check_main_exe, long document, int department, int for_, DateTime when_m, string signing, long moving, int l_emp)
        {
            int res=0;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_doc_moving ";
                int i = 0;
                SyB_Acc.SQLCommand.Parameters.Add("@main_executor", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                if (Check_main_exe) SyB_Acc.SQLCommand.Parameters[i].Value = 1;
                else SyB_Acc.SQLCommand.Parameters[i].Value = 0;
                i++;
                if (document > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@document", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = (int)document;
                    i++;
                }
                if (for_ > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@for_", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = for_;
                    i++;
                }
                if (department > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@department", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = department;
                    i++;
                }
                SyB_Acc.SQLCommand.Parameters.Add("@when_m", SqlDbType.DateTime);
                SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[i].Value = when_m;
                i++;
                SyB_Acc.SQLCommand.Parameters.Add("@signing", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[i].Value = signing;
                i++;
                if (moving > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@moving", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = (int)moving;
                    i++;
                }
                if (l_emp > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@l_emp", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = (int)l_emp;
               }

                res=(int)SyB_Acc.SQLCommand.ExecuteScalar() ;
            }
            catch (Exception ex) 
            {
                res = 0; 
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 70", ex.StackTrace);
            }
            return res;
        }

        public static int Document_moving_short( int document, int for_, DateTime when_m)
        {
            int res = 0;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_doc_moving ";

                SyB_Acc.SQLCommand.Parameters.Add("@document", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = document;

                SyB_Acc.SQLCommand.Parameters.Add("@for_", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = for_;

                SyB_Acc.SQLCommand.Parameters.Add("@when_m", SqlDbType.DateTime);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = when_m;

                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                res = 0;
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 70.21", ex.StackTrace);
            }
            return res;
        }

        public static bool Document_Vized(int viza, bool Yes_no, int document, string denote )
        {
            bool b = true;
            int d ;
            int id = -1; ;
            if (Yes_no) d = 1; else d = 0;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_vizing set viza=" + viza.ToString() + ", Yes_no= " + d.ToString() + " where document=" + document.ToString()+ " and for_ in (select cod from dbo.emp_dep_degrees where employee=dbo.user_ident())";
                SyB_Acc.SQLCommand.ExecuteNonQuery() ;

                SyB_Acc.SQLCommand.CommandText = "select top 1 id from dbo.rkk_vizing where viza=" + viza.ToString() + " and document=" + document.ToString() + " and for_ in (select cod from dbo.emp_dep_degrees where employee=dbo.user_ident())";
                id = (int)SyB_Acc.SQLCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 69.4", ex.StackTrace);
                b=false;
            }

            if(id>0)
                try
                {
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.Parameters.Add("@denote", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[0].Value = denote;
                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.rkk_vizing_denote ( vizing,notes) values (" + id.ToString() + ", @denote)";
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SyB_Acc.EBBLP.AddError(ex.Message, "Command - 69.4.1", ex.StackTrace);
                }
            return b;
        }

        public static string GetViza(bool Yes_no, int viza)
        {

            string res = "";
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                string vizYN = "";
                if (Yes_no) vizYN = "viza_yes"; else vizYN = "viza_no";
                SyB_Acc.SQLCommand.CommandText = "select top 1 "+vizYN+" from dbo.rkk_vizing_list where id=" + viza.ToString() ;
                res=(string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 69.5", ex.StackTrace);
                res = "";
            }
            return res;
        }

        public static int GetTemaByDecription( string Descr)
        {

            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@descr", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = Descr.Trim();

                SyB_Acc.SQLCommand.CommandText = "select top 1 tema from dbo.rkk_tema where description_ like @descr";
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 69.8", ex.StackTrace);
                res = -1;
            }
            return res;
        }

        public static int Document_Vizing( long document, int department, int for_, DateTime when_v,  long moving, int l_emp)
        {
            int res=0;
            System.Data.SqlClient.SqlDataReader reader = null; 
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_doc_vizing ";
                int i = 0;
                if (document > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@document", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = (int)document;
                    i++;
                }
                if (for_ > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@for_", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = for_;
                    i++;
                }
                if (department > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@department", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = department;
                    i++;
                }
                SyB_Acc.SQLCommand.Parameters.Add("@when_v", SqlDbType.DateTime);
                SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[i].Value = when_v;
                i++;
                if (moving > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@child_document", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = (int)moving;
                    i++;
                }
                if (l_emp > 0)
                {
                    SyB_Acc.SQLCommand.Parameters.Add("@l_emp", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[i].Direction = ParameterDirection.Input;
                    SyB_Acc.SQLCommand.Parameters[i].Value = (int)l_emp;
                }
                reader=SyB_Acc.SQLCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    res = (int)reader.GetInt32(0);
                }
                else res = 0;
            }
            catch (Exception ex)
            {
                res = 0; 
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 70", ex.StackTrace);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return res;
        }

        public static bool Document_Newed(int news)
        {
            bool b = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_boss_newed ";
                SyB_Acc.SQLCommand.Parameters.Add("@news", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters["@news"].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters["@news"].Value = news;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception ex) 
            { 
                b = false;
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 71", ex.StackTrace);
            }
            return b;
        }

        public static bool Document_Viewed(int moving)
        {
            bool b = true;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_boss_view ";
                SyB_Acc.SQLCommand.Parameters.Add("@moving", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters["@moving"].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters["@moving"].Value = moving;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                b = false;
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 71.1", ex.StackTrace);
            }
            return b;
        }
        public static int Document_Newing(int document, DateTime dat_exe, int departament, int for_)
        {
            int res = 0;
            DateTime dt;
            
                try
                {
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandText = "select getdate()";
                    dt=(DateTime)SyB_Acc.SQLCommand.ExecuteScalar();
                    if (dat_exe > dt)
                    {
                        SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                        SyB_Acc.SQLCommand.Parameters.Clear();
                        if (for_ <= 0)
                            SyB_Acc.SQLCommand.CommandText = "insert into dbo.rkk_news (document,department,when_n) values (" + document.ToString() + "," + departament.ToString() + ",@dat_exe)";
                        else
                            SyB_Acc.SQLCommand.CommandText = "insert into dbo.rkk_news (document,for_,when_n) values (" + document.ToString() + "," + for_.ToString() + ",@dat_exe)";
                        SyB_Acc.SQLCommand.Parameters.Add("@dat_exe", SqlDbType.DateTime);
                        SyB_Acc.SQLCommand.Parameters["@dat_exe"].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters["@dat_exe"].Value = dat_exe;
                        SyB_Acc.SQLCommand.ExecuteNonQuery();

                        SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                        SyB_Acc.SQLCommand.Parameters.Clear();
                        SyB_Acc.SQLCommand.Parameters.Add("@dt", SqlDbType.DateTime);
                        SyB_Acc.SQLCommand.Parameters["@dt"].Direction = ParameterDirection.Input;
                        SyB_Acc.SQLCommand.Parameters["@dt"].Value = dt;
                        SyB_Acc.SQLCommand.CommandText = "select top 1 news from dbo.rkk_news where document=" + document.ToString() + " and typist=dbo.user_ident() and time_n>=@dt";
                        res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    res = 0;
                    SyB_Acc.EBBLP.AddError(ex.Message, "Command - 72", ex.StackTrace);
                }
            return res;
        }

        public static bool Denote_of_document_write(int doc, byte[] RTF, string Texx)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = " insert into dbo.RKK_denote_of_document (document, denote, RTFText) values (@doc,@Texx,@RTF)";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters.Add("@RTF", SqlDbType.Image);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = RTF;
                SyB_Acc.SQLCommand.Parameters.Add("@Texx", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = Texx;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 73", e.StackTrace);
                b = false;
            }
            return b;
        }


        public static bool Assign_to_Dep(long member,int vizing_or_moving)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.StoredProcedure;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "dbo.RKK_working_groups";
                SyB_Acc.SQLCommand.Parameters.AddWithValue("@member", member);
                switch (vizing_or_moving)
                {
                    case 1: SyB_Acc.SQLCommand.Parameters.AddWithValue("@executing", 1);
                        break;
                    case 2: SyB_Acc.SQLCommand.Parameters.AddWithValue("@vizing", 1);
                        break;
                }
                if ((int)SyB_Acc.SQLCommand.ExecuteScalar()  > 0) b = true;
            }
            catch (Exception ex) 
            { 
                b = false; 
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 72", ex.StackTrace);
            }
            return b;
        }

        public static int agent_phone_append(int agentId, string phone, string wdo)
        {
            int res = 0;
            try
            {
                DateTime dt;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "select getdate()";
                dt = (DateTime)SyB_Acc.SQLCommand.ExecuteScalar();
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.age_phone (phone,Wdo,agent) values (@phone,@wdo,@agent)";
                SyB_Acc.SQLCommand.Parameters.Add("@phone", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = phone;
                SyB_Acc.SQLCommand.Parameters.Add("@wdo", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = wdo;
                SyB_Acc.SQLCommand.Parameters.Add("@agent", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = agentId;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "select top 1 id from dbo.age_phone where userid=dbo.user_ident() and date_change>=@dt";
                SyB_Acc.SQLCommand.Parameters.Add("@dt", SqlDbType.DateTime);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = dt;
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 75", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static bool agent_phone_update(int Id, string phone, string wdo)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "update dbo.age_phone set phone=@phone, Wdo=@wdo where id=@id";
                SyB_Acc.SQLCommand.Parameters.Add("@phone", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = phone;
                SyB_Acc.SQLCommand.Parameters.Add("@wdo", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = wdo;
                SyB_Acc.SQLCommand.Parameters.Add("@Id", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = Id;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 76", e.StackTrace);
            }
            return b;
        }

        public static bool agent_phone_erise(int Id)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "delete dbo.age_phone  where id=@id";
                SyB_Acc.SQLCommand.Parameters.Add("@Id", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = Id;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 77", e.StackTrace);
            }
            return b;
        }

        public static int agent_email_append(int agentId, string email, string wdo)
        {
            int res = 0;
            try
            {
                DateTime dt;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "select getdate()";
                dt = (DateTime)SyB_Acc.SQLCommand.ExecuteScalar();
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.age_Email (Email,Wdo,agent) values (@email,@wdo,@agent)";
                SyB_Acc.SQLCommand.Parameters.Add("@email", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = email;
                SyB_Acc.SQLCommand.Parameters.Add("@wdo", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = wdo;
                SyB_Acc.SQLCommand.Parameters.Add("@agent", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = agentId;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "select top 1 id from dbo.age_email where userid=dbo.user_ident() and dat_change>=@dt";
                SyB_Acc.SQLCommand.Parameters.Add("@dt", SqlDbType.DateTime);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = dt;
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 78", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static bool agent_email_update(int Id, string email, string wdo)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "update dbo.age_email set Email=@email, Wdo=@wdo where id=@id";
                SyB_Acc.SQLCommand.Parameters.Add("@email", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = email;
                SyB_Acc.SQLCommand.Parameters.Add("@wdo", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[1].Value = wdo;
                SyB_Acc.SQLCommand.Parameters.Add("@Id", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[2].Value = Id;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 79", e.StackTrace);
            }
            return b;
        }

        public static bool agent_email_erise(int Id)
        {
            bool b = false;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandText = "delete dbo.age_email  where id=@id";
                SyB_Acc.SQLCommand.Parameters.Add("@Id", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters[0].Value = Id;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                b = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 80", e.StackTrace);
            }
            return b;
        }

        // Send document to mail

        public static int SendKindToMail(int doc)
        {
            int ret = 0;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                //SyB_Acc.SQLCommand.CommandText = "select  rkk_kind.one_answer from db.rkk_kind join dbo.rkk_flow_document on rkk_kind.kod=rkk_flow_document.kind where rkk_flow_document.kod=@doc";
                SyB_Acc.SQLCommand.CommandText = "select  kind from dbo.rkk_flow_document where rkk_flow_document.kod=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                reader.Read();
                ret = (int)reader.GetValue(0);
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 81", e.StackTrace);
                ret = 0;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static bool SendOneKindToMail(int kind)
        {
            bool ret = false;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  rkk_kind.one_answer from dbo.rkk_kind  where rkk_kind.kod=@kod";
                SyB_Acc.SQLCommand.Parameters.Add("@kod", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = kind;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                reader.Read();
                ret = (bool)reader.GetValue(0);
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 82", e.StackTrace);
                ret = false;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static bool SendOneDocumentKindSended(int kind,int doc)
        {
            bool ret = false;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  master.kod from dbo.rkk_flow_document as master join dbo.rkk_flow_document on master.master_document =rkk_flow_document.master_document join dbo.out_document on flow_document.kod=out_document.kod  where rkk_flow_document.kind=@kind and master.kod=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@kind", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = kind;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[1].Value = doc;
                SyB_Acc.SQLCommand.Parameters[1].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                //если выпущены документы, то возвращается "да"
                ret = reader.HasRows;
                reader.Close();
                if (!ret)
                {
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.CommandText = "select  master.id from dbo.rkk_flow_document as master join dbo.rkk_flow_document on master.master_document =rkk_flow_document.master_document join dbo.out_for_mail on flow_document.kod=out_for_mail.document  where rkk_flow_document.kind=@kind and master.kod=@doc";
                    reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                    //если выпущены документы, то возвращается "да"
                    ret = reader.HasRows;
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 82", e.StackTrace);
                ret = false;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static bool SendDocumentToMail(int doc)
        {
            bool ret=false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "insert into dbo.out_for_mail(document) values (@doc)";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 83", e.StackTrace);
                ret = false;
            }
            return ret;
        }

        public static string ShowDocumentFK(int doc)
        {
            string ret = "";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select top 1 find_cod from dbo.rkk_flow_Document where kod=" + doc.ToString();
                ret =(string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 83.1", e.StackTrace);
                ret = "";
            }
            return ret;
        }

        public static bool SendNotVizingDocument(int doc)
        {
            bool ret=false;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select *  from dbo.rkk_vizing where document=@doc and (viza is null or yes_no=0)";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                //если выпущены документы, то возвращается "да"
                ret = reader.HasRows;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 84", e.StackTrace);
                ret = false;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        // Out docs

        public static int GetOutDocID(int DOC)
        {
            int ret = 0;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select top 1 kod from dbo.out_document where out_document.document=@doc order by out_document.kod";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = DOC;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    ret = reader.GetInt32(0);
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 87", e.StackTrace);
                ret = 0;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static int CorrespondenTyp(int id)
        {
            int ret = -1;
            System.Data.SqlClient.SqlDataReader reader = null;
            SyB_Acc.SQLCommand.Parameters.Clear();
            SyB_Acc.SQLCommand.CommandType = CommandType.Text;
 
            SyB_Acc.SQLCommand.CommandText = "select top 1 WelInOut from dbo.rkk_kind where kod=" +id.ToString();
            try
            {
            reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    //if(reader.GetValue(1)!=null) 
                    try
                    {
                        ret = reader.GetInt32(0);
                    }
                    catch {ret=-1;}
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 87.1", e.StackTrace);
                ret = -1;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static int CorrespondenTypSet(int id, int TYP)
        {
            int ret = -1;
            try
            {
            SyB_Acc.SQLCommand.Parameters.Clear();
            SyB_Acc.SQLCommand.CommandType = CommandType.Text;

            SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_kind set WelInOut="+TYP.ToString()+" where kod=" + id.ToString();
            SyB_Acc.SQLCommand.ExecuteNonQuery();
            ret = TYP;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 87.2", e.StackTrace);
                ret = -1;
            }
            return ret;
        }


        public static string CreateOutDoc(int DOC)
        {
            string ret = "";
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                DateTime Dato;
                string findCod;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "delete dbo.out_for_mail where document=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = DOC;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                SyB_Acc.SQLCommand.CommandText = "select top 1 out_document.date_o, rkk_flow_document.find_cod from dbo.out_document join dbo.rkk_flow_document on out_document.kod=rkk_flow_document.kod where out_document.document=@doc order by out_document.kod";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    Dato = reader.GetDateTime(0);
                    findCod = reader.GetString(1);
                    ret = findCod.Trim() + " от " + Dato.ToShortDateString().Trim();
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 88", e.StackTrace);
                ret = "";
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static string OutDocSigner(int doc)
        {
            string ret = "";
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                bool io = false;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select *  from dbo.emp_unrank join dbo.out_document on emp_unrank.employee=out_document.autor where kod=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                    io = true;
                reader.Close();
                if (io)
                    SyB_Acc.SQLCommand.CommandText = "select org_jrd_degree.name as degree,fio,fio_short from dbo.org_jrd_degree join dbo.emp_ent_employee on org_jrd_degree.degree=emp_ent_employee.degree join dbo.emp_unrank on emp_ent_employee.id=emp_unrank.new_employee join dbo.out_document on emp_unrank.employee=out_document.autor where kod=@doc";
                else
                    SyB_Acc.SQLCommand.CommandText = "select org_jrd_degree.name as degree,fio,fio_short from dbo.emp_ent_employee,dbo.org_jrd_degree,dbo.out_document where org_jrd_degree.degree=emp_ent_employee.degree and emp_ent_employee.id=out_document.autor and kod=@doc order by leader desc";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (io)
                        ret = "И.о. " + reader.GetString(0) + " " + reader.GetString(1);
                    else
                         ret = reader.GetString(0) + " " + reader.GetString(1);
               }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 89", e.StackTrace);
                ret = "";
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static string OutDocOutcome(int doc)
        {
            string ret = "";
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                DateTime DT ;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select rkk_wellcome_document.outcoming, rkk_wellcome_document.date_out from dbo.rkk_flow_document as well join dbo.rkk_wellcome_document on well.kod=rkk_wellcome_document.kod, dbo.rkk_flow_document as outdoc join dbo.out_document on outdoc.kod=out_document.kod where outdoc.master_document=well.kod and out_document.kod=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    DT=reader.GetDateTime(1);
                    ret = reader.GetString(0) + " от " + DT.ToShortDateString();
                }
            }
            catch //(Exception e)
            {
                //SyB_Acc.EBBLP.AddError(e.Message, "Command - 90", e.StackTrace);
                ret = "";
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static string OutDocAnnotation(int doc)
        {
            string ret = "";
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select annot from dbo.rkk_flow_document where kod=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    ret = reader.GetString(0).Trim() ;
                }
            }
            catch //(Exception e)
            {
                //SyB_Acc.EBBLP.AddError(e.Message, "Command - 91", e.StackTrace);
                ret = "";
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return ret;
        }

        public static string OutDocExecutor(int doc)
        {
            string ret = "";
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select org_jrd_degree.name as degree,fio,short_fio,emp_ent_employee.id from dbo.emp_ent_employee,dbo.org_jrd_degree,dbo.rkk_flow_document where org_jrd_degree.degree=emp_ent_employee.degree and emp_ent_employee.id=rkk_flow_document.typist and rkk_flow_document.kod=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    ret = reader.GetString(0).Trim()+ " "+ reader.GetString(1).Trim();
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 92", e.StackTrace);
                ret = "";
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static byte[] OutDocFile(int doc)
        {
            byte[] ret = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select ole_doc from dbo.rkk_stuff where document=@doc";
                SyB_Acc.SQLCommand.Parameters.Add("@doc", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = doc;
                SyB_Acc.SQLCommand.Parameters[0].Direction = ParameterDirection.Input;
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    int len = 50000;
                    byte[] buff = new byte[len];
                    int i = 0;
                    byte[] newres;
                    int cnt;

                    do
                    {
                        cnt = (int)reader.GetBytes(0, i * len, buff, 0, len);
                        i++;
                        if (ret != null)
                        {
                            newres = new byte[ret.Length + cnt];
                            ret.CopyTo(newres, 0);
                            if ((int)buff.LongLength == cnt)
                                buff.CopyTo(newres, ret.Length);
                            else
                                for (int n = 0; n < cnt; n++)
                                    newres[n + ret.Length] = buff[n];
                        }
                        else
                        {
                            newres = new byte[cnt];
                            if ((int)buff.LongLength == cnt)
                                buff.CopyTo(newres, 0);
                            else
                                for (int n = 0; n < cnt; n++)
                                    newres[n] = buff[n];
                        }
                        ret = newres;
                    }
                    while (cnt == len);
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 93", e.StackTrace);
                ret = null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        // foundation

        public static byte[] GetUpdateModuleAMAS()
        {
            byte[] ret = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select top 1 applicationDistributive from dbo.set_foundation ";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows)
                {
                    reader.Read();
                    int len = 50000;
                    byte[] buff = new byte[len];
                    int i = 0;
                    byte[] newres;
                    int cnt;

                    do
                    {
                        cnt = (int)reader.GetBytes(0, i * len, buff, 0, len);
                        i++;
                        if (ret != null)
                        {
                            newres = new byte[ret.Length + cnt];
                            ret.CopyTo(newres, 0);
                            if ((int)buff.LongLength == cnt)
                                buff.CopyTo(newres, ret.Length);
                            else
                                for (int n = 0; n < cnt; n++)
                                    newres[n + ret.Length] = buff[n];
                        }
                        else
                        {
                            newres = new byte[cnt];
                            if ((int)buff.LongLength == cnt)
                                buff.CopyTo(newres, 0);
                            else
                                for (int n = 0; n < cnt; n++)
                                    newres[n] = buff[n];
                        }
                        ret = newres;
                    }
                    while (cnt == len);
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 97", e.StackTrace);
                ret = null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return ret;
        }

        public static bool IAmLeader()
        {
            bool res = false; 
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select * from dbo.emp_dep_degrees where employee=dbo.user_ident() and leader=1 ";
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.SequentialAccess);
                if (reader.HasRows) res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 98", e.StackTrace);
                res = false;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return res;
        }

        public static bool assignKindDelo(int kind, int delo)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_prefix set delo = " + delo.ToString() + " where kod=" + kind.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 99", e.StackTrace);
                res = false;
            }
            return res;
        }
        public static bool assignTemaDelo(int tema, int delo)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_suffix set delo = " + delo.ToString() + " where id=" + tema.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 100", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool AddKindJouCount(int kind, int count)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_prefix set couner = @couner where kod=" + kind.ToString();
                SyB_Acc.SQLCommand.Parameters.Add("@couner", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = count;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 101", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool AddKindJouWIO(int kind, int WIO)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_prefix set WelOut = @WelOut where kod=" + kind.ToString();
                SyB_Acc.SQLCommand.Parameters.Add("@WelOut", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = WIO;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 102", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool AddTemaJouCount(int tema, int count)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_suffix set counter = @counter where id=" + tema.ToString();
                SyB_Acc.SQLCommand.Parameters.Add("@counter", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = count;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 103", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool AddCorrWIO(int cor, int WIO)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "update dbo.rkk_kind set WelInOut = @WelInOut where kod=" + cor.ToString();
                SyB_Acc.SQLCommand.Parameters.Add("@WelInOut", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = WIO;
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 104", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static int GetCorrWIO(int cor)
        {
            int res = 0;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 WelInOut from dbo.rkk_kind where kod=" + cor.ToString();
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 105", e.StackTrace);
                res = 0;
            }
            return res;
        }

        public static int AddBusinessRole(int kod, string name, string descr_)
        {
            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters[1].Value = descr_;

                SyB_Acc.SQLCommand.CommandText = "insert into dbo.bpr_roles ([name],description_,instruction_) values(@name,@description_," + kod.ToString() + ")";
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 id from dbo.bpr_roles where  instruction_=" + kod.ToString()+ " order by id desc";
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 110", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static string UpdateBusinessRole(int id, string name, string descr_)
        {
            string     res = "-------------";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters[1].Value = descr_;

                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_roles set [name]=@name,description_=@description_ where id=" + id.ToString() ;
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 [name] from dbo.bpr_roles where id=" + id.ToString();
                res = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 110", e.StackTrace);
                res = "---------------";
            }
            return res;
        }

        public static bool RemoveBusinessRole(int id)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
               SyB_Acc.SQLCommand.CommandType = CommandType.Text;

               SyB_Acc.SQLCommand.CommandText = "delete dbo.bpr_roles where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 111", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static string GetOneKind(int kind)
        {
            string ret = "";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 kind from dbo.rkk_kind where  kod=" + kind.ToString() + " order by kod desc";
                ret = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 112", e.StackTrace);
                ret = "";
            }

            return ret;
        }

        public static string GetOneTema(int tema)
        {
            string ret = "";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 description_ from dbo.rkk_tema where  tema=" + tema.ToString() + " order by tema desc";
                ret = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 113", e.StackTrace);
                ret = "";
            }

            return ret;
        }

        public static string GetTaskDescription(int task)
        {
            string ret = "";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 description_ from dbo.bpr_executions where  id=" + task.ToString() + " order by id desc";
                ret = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 113", e.StackTrace);
                ret = "";
            }

            return ret;
        }

        public static string GetOneViza(int viza)
        {
            string ret = "";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 viza_yes+ '  '+viza_no as viza from dbo.rkk_vizing_list where  id=" + viza.ToString() + " order by id desc";
                ret = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 114", e.StackTrace);
                ret = "";
            }

            return ret;
        }

        public static int AddBusinessTask(int id, string name, int Minutes)
        {
            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@Minutes", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[1].Value = Minutes;

                SyB_Acc.SQLCommand.CommandText = "insert into dbo.bpr_executions ([name],role_,minutes) values(@name," + id.ToString() + ",@minutes)";
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 id from dbo.bpr_executions where  role_=" + id.ToString() + " order by id desc";
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 115", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static string UpdateBusinessTaskName(int id, string name)
        {
            string res = "-------------";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
 
                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_executions set [name]=@name where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 [name] from dbo.bpr_executions where id=" + id.ToString();
                res = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 116", e.StackTrace);
                res = "---------------";
            }
            return res;
        }


        public static string UpdateBusinessTaskDescription(int id, string descr_)
        {
            string res = "-------------";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters[0].Value = descr_;

                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_executions set description_=@description_ where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 [description_] from dbo.bpr_executions where id=" + id.ToString();
                res = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 116.1", e.StackTrace);
                res = "---------------";
            }
            return res;
        }

        public static bool RemoveBusinessTask(int id)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "delete dbo.bpr_executions where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 117", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool SetBusinessTaskVizing(int id, int Viza)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_executions set Viza=" + Viza .ToString()+ ",executing_=1 where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 118", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool SetBusinessTaskExecuting(int id, int Kind, int Tema)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_executions ";
                if (Kind > 0)
                {
                    SyB_Acc.SQLCommand.CommandText += " set Kind=" + Kind.ToString();
                    if (Tema > 0) SyB_Acc.SQLCommand.CommandText += ", Tema=" + Tema.ToString();
                    SyB_Acc.SQLCommand.CommandText +=", executing_=0 where id=" + id.ToString();
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                    res = true;
                }
                else if (Tema > 0)
                {
                    SyB_Acc.SQLCommand.CommandText += " Tema=" + Tema.ToString() + ", executing_=0 where id=" + id.ToString();
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                    res = true;
                }
                else res= false;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 119", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool SetBusinessTaskTimer(int id, int minutes)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_executions set minutes=" + minutes.ToString() +" where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 120", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool DeleteBusinessRoute(int id)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "delete dbo.bpr_Route where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 121", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static int InsertBusinessRoute( string name, string descr)
        {
            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters[1].Value = descr;

                SyB_Acc.SQLCommand.CommandText = "insert into dbo.bpr_Route ([name],[description_]) values(@name,@description_)";
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 id from dbo.bpr_Route where name=@name and description_= @description_ order by id desc";
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();

            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 122", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static int UpdateBusinessRoute(int id, string name, string descr)
        {
            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters[1].Value = descr;

                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_Route set [name]=@name,[description_]=@description_ where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 123", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static bool AddExecutionForRoute(int Execution, int Route, string name, string descr)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Value = descr;
                SyB_Acc.SQLCommand.Parameters.Add("@rote", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Value = Route;
                SyB_Acc.SQLCommand.Parameters.Add("@execution", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[3].Value = Execution;

                SyB_Acc.SQLCommand.CommandText = "insert into dbo.bpr_Connections (route_, execution_, [name], [description_]) values(@rote, @execution, @name, @description_) ";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 124", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool RemoveExecutionForRoute(int Execution, int Route)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.Parameters.Add("@route", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = Route;
                SyB_Acc.SQLCommand.Parameters.Add("@execution", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[1].Value = Execution;

                SyB_Acc.SQLCommand.CommandText = "delete  dbo.bpr_Connections where route_=@route and execution_=@execution";
                SyB_Acc.SQLCommand.ExecuteNonQuery();
                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 125", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static bool DeleteEvent(int id)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "delete dbo.bpr_Events where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 126", e.StackTrace);
                res = false;
            }
            return res;
        }

        public static int InsertEvent(string name, string descr,int kind, int tema,  int route_, int delay)
        {
            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Value = descr;
                SyB_Acc.SQLCommand.Parameters.Add("@kind", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Value = kind;
                SyB_Acc.SQLCommand.Parameters.Add("@tema", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[3].Value = tema;
                SyB_Acc.SQLCommand.Parameters.Add("@route_", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[4].Value = route_;
                SyB_Acc.SQLCommand.Parameters.Add("@TimeDelay", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[5].Value = delay;

                SyB_Acc.SQLCommand.CommandText = "insert into dbo.bpr_Events_library ([name],[description_],kind,tema,route_,TimeDelay) values(@name,@description_,@kind,@tema,@route_,@TimeDelay)";
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 id from dbo.bpr_Events_library where name=@name and description_= @description_ order by id desc";
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();

            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 127", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static int UpdateEvent(int id, string name, string descr, int kind, int tema, int route_,int delay)
        {
            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                 SyB_Acc.SQLCommand.Parameters[1].Value = descr;
                SyB_Acc.SQLCommand.Parameters.Add("@kind", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Value = kind;
                SyB_Acc.SQLCommand.Parameters.Add("@tema", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[3].Value = tema;
                SyB_Acc.SQLCommand.Parameters.Add("@route_", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[4].Value = route_;
                SyB_Acc.SQLCommand.Parameters.Add("@TimeDelay", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[5].Value = delay;

                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_Events_library set [name]=@name, [description_]=@description_, kind=@kind, tema=@tema,  route_=@route_, TimeDelay=@TimeDelay  where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 128", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static int InsertCondition(string name, string descr, int role_, int execution_)
        {
            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Value = descr;
                SyB_Acc.SQLCommand.Parameters.Add("@role", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Value = role_;
                SyB_Acc.SQLCommand.Parameters.Add("@execution_", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[3].Value = execution_;

                SyB_Acc.SQLCommand.CommandText = "insert into dbo.bpr_Conditions ([name],[description_],role,execution_) values(@name,@description_,@role,@execution_)";
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select  top 1 id from dbo.bpr_Conditions where name=@name and role= @role and execution_= @execution_ order by id desc";
                res = (int)SyB_Acc.SQLCommand.ExecuteScalar();

            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 129", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static int UpdateCondition(int id, string name, string descr, int role_, int execution_)
        {
            int res = -1;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@name", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[0].Value = name;
                SyB_Acc.SQLCommand.Parameters.Add("@description_", SqlDbType.VarChar);
                SyB_Acc.SQLCommand.Parameters[1].Value = descr;
                SyB_Acc.SQLCommand.Parameters.Add("@role", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[2].Value = role_;
                SyB_Acc.SQLCommand.Parameters.Add("@execution_", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[3].Value = execution_;

                SyB_Acc.SQLCommand.CommandText = "update dbo.bpr_Conditions set [name]=@name, [description_]=@description_, role=@role, execution_=@execution_ where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 130", e.StackTrace);
                res = -1;
            }
            return res;
        }

        public static bool DeleteCondition(int id)
        {
            bool res = false;
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                SyB_Acc.SQLCommand.CommandText = "delete dbo.bpr_Conditions where id=" + id.ToString();
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                res = true;
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 131", e.StackTrace);
                res = false;
            }
            return res;
        }


        public static void SetEventExecution(int event_, int[] executions)
        {
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.Parameters.Add("@event_", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = event_;
                SyB_Acc.SQLCommand.CommandText = "delete dbo.bpr_event_executions where event_=@event_";
                SyB_Acc.SQLCommand.ExecuteNonQuery();

                SyB_Acc.SQLCommand.Parameters.Add("@executions", SqlDbType.Int);
                foreach (int exec in executions)
                {
                    SyB_Acc.SQLCommand.Parameters[1].Value = exec;
                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.bpr_event_executions  (event_,executions) values (@event_,@executions)";
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e) { SyB_Acc.EBBLP.AddError(e.Message, "Command - 132", e.StackTrace); }
        }

        public static ArrayList PlaceLevelList(int id)
        {
            ArrayList PlaceList = new ArrayList();
            SyB_Acc.SQLCommand.Parameters.Clear();
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "exec [dbo].[adr_back_address] @flat_id = " + id.ToString();
                reader = SyB_Acc.SQLCommand.ExecuteReader(CommandBehavior.Default);
                if (reader.HasRows)
                    if(reader.Read())
                {
                    
                    int num;
                    string name;
                    string mess;
                    try { num = (int)reader.GetInt32(0); }
                    catch (Exception ex) 
                    { num = 0; mess = ex.Message; }
                    try { name = (string)reader.GetString(8); }
                    catch (Exception ex) 
                    { name = ""; mess = ex.Message; }

                       PlaceList.Add(new CommonClass.Arraysheet(name, num));

                       try { num = (int)reader.GetInt32(1); }
                       catch (Exception ex) 
                       { num = 0; mess = ex.Message; }
                       try { name = (string)reader.GetString(9); }
                       catch (Exception ex) 
                       { name = ""; mess = ex.Message; }

                       PlaceList.Add(new CommonClass.Arraysheet(name, num));

                       try { num = (int)reader.GetInt32(2); }
                       catch (Exception ex) { num = 0; mess = ex.Message; }
                       try { name = (string)reader.GetString(10); }
                       catch (Exception ex) { name = ""; mess = ex.Message; }

                       PlaceList.Add(new CommonClass.Arraysheet(name, num));

                       try { num = (int)reader.GetInt32(3); }
                       catch (Exception ex) { num = 0; mess = ex.Message; }
                       try { name = (string)reader.GetString(11); }
                       catch (Exception ex) { name = ""; mess = ex.Message; }

                       PlaceList.Add(new CommonClass.Arraysheet(name, num));

                       try { num = (int)reader.GetInt32(4); }
                       catch (Exception ex) { num = 0; mess = ex.Message; }
                       try { name = (string)reader.GetString(12); }
                       catch (Exception ex) { name = ""; mess = ex.Message; }

                       PlaceList.Add(new CommonClass.Arraysheet(name, num));

                       try { num = (int)reader.GetInt32(5); }
                       catch (Exception ex) { num = 0; mess = ex.Message; }
                       try { name = (string)reader.GetString(13); }
                       catch (Exception ex) { name = ""; mess = ex.Message; }

                       PlaceList.Add(new CommonClass.Arraysheet(name, num));

                       try { num = (int)reader.GetInt32(6); }
                       catch { num = 0; }
                       try { name = (string)reader.GetString(14); }
                       catch { name = ""; }

                       PlaceList.Add(new CommonClass.Arraysheet(name, num));

                       try { num = (int)reader.GetInt32(7); }
                       catch { num = 0; }
                       try { name = (string)reader.GetString(15); }
                       catch { name = ""; }

                       PlaceList.Add(new CommonClass.Arraysheet(name, num));
                }
            }
            catch (Exception e)
            {
                SyB_Acc.EBBLP.AddError(e.Message, "Command - 133", e.StackTrace);
                PlaceList = null;
            }
            if (reader != null) reader.Close();
            return PlaceList;
        }

        public static string GetJuridicName(int id)
        {
            string JurName = "";
            try
            {
            SyB_Acc.SQLCommand.Parameters.Clear();
            SyB_Acc.SQLCommand.CommandType = CommandType.Text;
            SyB_Acc.SQLCommand.CommandText =
              "select dbo.[org_get_juridic] (" + id.ToString()+")";

            JurName = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 134", ex.StackTrace);
            }

            return JurName;
        }

        public static string GetAddressName(int id)
        {
            string JurName = "";
            try
            {
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText =
                  "select dbo.[adr_get_address] ("+id.ToString()+") ";

                JurName = (string)SyB_Acc.SQLCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                SyB_Acc.EBBLP.AddError(ex.Message, "Command - 135", ex.StackTrace);
            }

            return JurName;
        }

        public static class Cargo
        {
            public static int InsertCargoRoute(int CargoCompany,string RouteName, int FromPlace,int ToPlace,decimal cost, int TimeStart,int Period)
            {
                int res = -1;
                try
                {
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.Parameters.Add("@CargoCompany", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[0].Value = CargoCompany;
                    SyB_Acc.SQLCommand.Parameters.Add("@FromPlace", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[1].Value = FromPlace;
                    SyB_Acc.SQLCommand.Parameters.Add("@ToPlace", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[2].Value = ToPlace;
                    SyB_Acc.SQLCommand.Parameters.Add("@cost", SqlDbType.Decimal);
                    SyB_Acc.SQLCommand.Parameters[3].Value = cost;
                    SyB_Acc.SQLCommand.Parameters.Add("@TimeStart", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[4].Value = TimeStart;
                    SyB_Acc.SQLCommand.Parameters.Add("@Period", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[5].Value = Period;
                    SyB_Acc.SQLCommand.Parameters.Add("@RouteName", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[6].Value = RouteName;

                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.Cargo_Routes ([CargoCompany],RouteName,[FromPlace],ToPlace,cost,TimeStart,Period) values(@CargoCompany,@RouteName,@FromPlace,@ToPlace,@cost,@TimeStart,@Period)";
                    SyB_Acc.SQLCommand.ExecuteNonQuery();

                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.CommandText = "select  top 1 id from dbo.Cargo_Routes where CargoCompany=@CargoCompany and FromPlace= @FromPlace and ToPlace= @ToPlace order by id desc";
                    res = (int)SyB_Acc.SQLCommand.ExecuteScalar();

                }
                catch (Exception e)
                {
                    SyB_Acc.EBBLP.AddError(e.Message, "Command - 140", e.StackTrace);
                    res = -1;
                }
                return res;
            }

            public static int UpdateCargoRoures(int id, int CargoCompany,string RouteName, int FromPlace, int ToPlace, decimal cost, int TimeStart, int Period)
            {
                int res = -1;
                try
                {
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.Parameters.Add("@CargoCompany", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[0].Value = CargoCompany;
                    SyB_Acc.SQLCommand.Parameters.Add("@FromPlace", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[1].Value = FromPlace;
                    SyB_Acc.SQLCommand.Parameters.Add("@ToPlace", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[2].Value = ToPlace;
                    SyB_Acc.SQLCommand.Parameters.Add("@cost", SqlDbType.Decimal);
                    SyB_Acc.SQLCommand.Parameters[3].Value = cost;
                    SyB_Acc.SQLCommand.Parameters.Add("@TimeStart", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[4].Value = TimeStart;
                    SyB_Acc.SQLCommand.Parameters.Add("@Period", SqlDbType.Int);
                    SyB_Acc.SQLCommand.Parameters[5].Value = Period;
                    SyB_Acc.SQLCommand.Parameters.Add("@RouteName", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[6].Value = RouteName;

                    SyB_Acc.SQLCommand.CommandText = "update dbo.Cargo_Routes set CargoCompany=@CargoCompany, FromPlace=@FromPlace, ToPlace=@ToPlace, cost=@cost, TimeStart=@TimeStart, Period=@Period, RouteName=@RouteName where id=" + id.ToString();
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    SyB_Acc.EBBLP.AddError(e.Message, "Command - 141", e.StackTrace);
                    res = -1;
                }
                return res;
            }

            public static bool DeleteCargoRoute(int id)
            {
                bool res = false;
                try
                {
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;

                    SyB_Acc.SQLCommand.CommandText = "delete dbo.Cargo_Routes where id=" + id.ToString();
                    SyB_Acc.SQLCommand.ExecuteNonQuery();

                    res = true;
                }
                catch (Exception e)
                {
                    SyB_Acc.EBBLP.AddError(e.Message, "Command - 142", e.StackTrace);
                    res = false;
                }
                return res;
            }

            public static CommonValues.PlaceLevel IsInside(int TheAddress, int AtAddress)
            {
                PlaceLevel ret = PlaceLevel.NothingLevel;
                SyB_Acc.SQLCommand.Parameters.Clear();
                SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                SyB_Acc.SQLCommand.CommandText = "select [dbo].[Cargo_inside] (@The_address" /*+ TheAddress.ToString()*/ + ",@At_Address" /*+ AtAddress .ToString()*/ + ") ";
                SyB_Acc.SQLCommand.Parameters.Add("@The_address", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[0].Value = TheAddress;
                SyB_Acc.SQLCommand.Parameters.Add("@At_Address", SqlDbType.Int);
                SyB_Acc.SQLCommand.Parameters[1].Value = AtAddress;
                try
                {
                    Int32 swt;
                    swt = (Int32)SyB_Acc.SQLCommand.ExecuteScalar();    
                    switch(swt)
                        {
                            case (Int32)PlaceLevel.NothingLevel:
                                ret =PlaceLevel.NothingLevel;
                                break;
                            case (Int32)PlaceLevel.ArealLevel:
                                ret = PlaceLevel.ArealLevel;
                                break;
                            case (Int32)PlaceLevel.CityLevel:
                                ret = PlaceLevel.CityLevel;
                                break;
                            case (Int32)PlaceLevel.DistrictLevel:
                                ret = PlaceLevel.DistrictLevel;
                                break;
                            case (Int32)PlaceLevel.FlatLevel:
                                ret = PlaceLevel.FlatLevel;
                                break;
                            case (Int32)PlaceLevel.HouseLevel:
                                ret = PlaceLevel.HouseLevel;
                                break;
                            case (Int32)PlaceLevel.RegionLevel:
                                ret =PlaceLevel.RegionLevel;
                                break;
                            case (Int32)PlaceLevel.StateLevel:
                                ret = PlaceLevel.StateLevel;
                                break;
                            case (Int32)PlaceLevel.StreetLevel:
                                ret = PlaceLevel.StreetLevel;
                                break;
                        }
                }
                catch (Exception ex)
                {
                    SyB_Acc.EBBLP.AddError(ex.Message, "Command - 144", ex.StackTrace);
                    ret = PlaceLevel.NothingLevel;
                }
                return ret;
            }           
        }
    }
}
