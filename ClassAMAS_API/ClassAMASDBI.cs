using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
//using iAnywhere.Data.SQLAnywhere;
using System.IO;
using AMAS_Query;
using CommonValues;
using ClassErrorProvider;

namespace AMAS_DBI
{
	/// <summary>
	/// Summary description for Class API for AMAS.
	/// </summary>


    public class Class_syb_acc : ErrorBBLProvider
	{

        public ErrorBBLProvider EBBLP;

        private String ODBC_entry;
		private String ODBC_uid;
		private String ODBC_pwd;

        public string ScanDirectory;
        public string PDFDirectory;
        public string DocumentDirectory;

        public string DataBaseName
        {
            get
            {
                if (MSsql_Connection == null)
                    return "";
                else return MSsql_Connection.Database;
            }
        }

        public string ApplicationVersion = "4.0.2.0";

        public string Login_name
        {
            get { return ODBC_uid.Trim(); }
        }

		private String ORG_COD="";
		private String ORG_MAIL="";
		private Int32 CROSS_ORG=0;
		private String ORG_NAME="";
		
		private String ORG_State="";
		private String ORG_City="";
		private String ORG_Street="";
        private String ORG_House = "";
        private String ORG_Flat = "";
        private String ORG_Region = "";
        private String ORG_Areal = "";
        private String ORG_District = "";

        private int ORG_State_id ;
        private int ORG_City_id ;
        private int ORG_Street_id ;
        private int ORG_House_id ;
        private int ORG_Flat_id;
        private int ORG_Region_id;
        private int ORG_Areal_id;
        private int ORG_District_id;

        private int DefORG_ID;

		public int Default_State {get {return ORG_State_id;}}
        public int Default_City { get { return ORG_City_id; } }
        public int My_Organization { get { return DefORG_ID; } }

        public  int Sybase_BLOB_heads {get {return 428;}}
		public  int Sybase_BLOB_shift {get {return 142;}}

        //private static iAnywhere.Data.SQLAnywhere.SAConnection SAConnection_AMAS;
        //private static iAnywhere.Data.SQLAnywhere.SADataAdapter SADataAdapter_AMAS;
        //private static iAnywhere.Data.SQLAnywhere.SACommand SACommand_AMAS;

        private static System.Data.SqlClient.SqlConnection MSsql_Connection;
        private static System.Data.SqlClient.SqlCommand MSsql_SelectCommand;
        private static System.Data.SqlClient.SqlCommand MSsql_ExecuteCommand;
        private static System.Data.SqlClient.SqlDataAdapter MSsql_DataAdapte;

        public System.Data.SqlClient.SqlDataAdapter MSSQLDATAADAPTER { get { return MSsql_DataAdapte; } }
        public enum AMAS_connections {Sybase=1, MSSQL, Cansel}
        private static int AMASCOnn;
        
        public static int AMAS_Base { get { return AMASCOnn; } }

        public string Current_User = "";
        public string UserName = "";
        public Array MyRights
        {
            set 
            { 
                ItsMyRights = value;
                if (ItsMyRights!=null) GetRights = new GetMyRights(ItsMyRights);
            }
        }
        private Array ItsMyRights;
        public GetMyRights GetRights; 

        //public iAnywhere.Data.SQLAnywhere.SACommand SybCommand { get { return SACommand_AMAS; } }
        public System.Data.SqlClient.SqlCommand SQLCommand { get { return MSsql_ExecuteCommand; } }
		protected DataTable Tune_table;

        public DataTable Current_table
        {
            get { return Tune_table; }
        }

        private class last_point
        {
            public DataTable Last_Table;
            public int row;
            public int field;
            public last_point chain;

            public last_point(last_point stack, DataTable Table, int r, int f)
            {
                Last_Table = Table;
                row = r;
                field = f;
                chain = stack;
            }

            public last_point(last_point stack, DataTable Table, int r)
            {
                Last_Table = Table;
                row = r;
                field = 0;
                chain = stack;
            }

        }

		private int Current_row;

		public int Rows_count 
		{get 
		 {
			 if (Tune_table !=null)
			 return Tune_table.Rows.Count;
			 else return 0;
		 }
		}
		public int This_row {get {return Current_row;}}
		
		public String ResultString;
       
        public class PrepareParameters
        {
            public string name;
            public SqlDbType dbType;
            public object Value;
            public PrepareParameters(string n, SqlDbType type, object val)
            {
                name = n;
                dbType = type;
                Value = val;
            }
        }

		private class CLass_Tables_dim
		{
			private string[] da;
			private static int da_count=0;
			private DataTable[] tn;
            private PrepareParameters[][] arr;
            private ErrorBBLProvider EBBLP;

            public CLass_Tables_dim(ErrorBBLProvider Ee) 
			{
				da= new string[1000];
				tn= new DataTable[1000];
                arr = new PrepareParameters[1000][];
                EBBLP = Ee;
			}

            public CLass_Tables_dim(int dim, ErrorBBLProvider Ee)	
			{
				da= new string[dim];
				tn= new DataTable[dim];
                arr = new PrepareParameters[dim][];
                EBBLP = Ee;
            }

            public int append_table(string app, DataTable table, PrepareParameters[] Parameters)
			{
				da_count++;
				da[da_count]=app;
				tn[da_count]=table;
                arr[da_count] = Parameters;
                fill(da_count);
				return da_count;
			}

            public DataTable prev_table()
            {
                if (da_count > 0)
                    return tn[da_count - 1];
                else return null;
            }

            public DataTable return_table()
            {
                da_count--;
                return tn[da_count];
            }

			public void clear()
			{
				da_count=0;
			}
			
			public int count()
			{
				return da_count;
			}

            public bool fill(int Index)
            {
                bool res=false;
                try
                {
                    switch ((AMAS_connections)AMAS_Base)
                    {
                        case AMAS_connections.Sybase:
                            //SACommand_AMAS.CommandText = da[Index];
                            //tn[Index].Clear();
                            //SADataAdapter_AMAS.Fill(tn[Index]);
                            //res = true;
                            break;
                        case AMAS_connections.MSSQL:
                            MSsql_SelectCommand.CommandText = da[Index];
                            tn[Index].Clear();
                            MSsql_SelectCommand.Parameters.Clear();
                            if (MSsql_SelectCommand.CommandText.Length > 1)
                            {
                                if (arr[Index] != null)
                                    foreach (PrepareParameters param in arr[Index])
                                        MSsql_SelectCommand.Parameters.Add(param.name, param.dbType).Value = param.Value;
                                MSsql_DataAdapte.Fill(tn[Index]);
                                MSsql_DataAdapte.AcceptChangesDuringFill = true;
                                res = true;
                            }
                            break;
                    }
                }
                catch (Exception e) 
                { 
                    EBBLP.AddError(e.Message, "TablesDim: " + MSsql_SelectCommand.CommandText, e.StackTrace);
                    res = false; }
                return res;
            }

            public bool requery(int Index, string sql, PrepareParameters[] Parameters)
            {
                da[Index] = sql;
                bool res=false;
                try
                {
                    tn[Index].Clear();
                    switch ((AMAS_connections)AMAS_Base)
                    {
                        case AMAS_connections.Sybase:
                            //SACommand_AMAS.CommandText = da[Index];
                            //SADataAdapter_AMAS.Fill(tn[Index]);
                            //res=true;
                            break;
                        case AMAS_connections.MSSQL:
                            MSsql_SelectCommand.CommandText = da[Index];
                            MSsql_SelectCommand.Parameters.Clear();
                            if (Parameters!=null)
                            foreach (PrepareParameters p in Parameters)
                                MSsql_SelectCommand.Parameters.Add(p.name, p.dbType).Value = p.Value;
                            arr[Index] = Parameters;
                            MSsql_DataAdapte.Fill(tn[Index]);
                            res = true;
                            break;
                    }
                }
                catch (Exception e) 
                { 
                    EBBLP.AddError(e.Message, "TablesDim", e.StackTrace);
                    res = false; 
                }
                return res;
            }

			//public iAnywhere.Data.SQLAnywhere.SADataAdapter DA(int Index)
			//{
			//	iAnywhere.Data.SQLAnywhere.SADataAdapter w = SADataAdapter_AMAS;
			//	return w;
			//}

            public System.Data.SqlClient.SqlDataAdapter DM(int index)
            {
                System.Data.SqlClient.SqlDataAdapter w = MSsql_DataAdapte;
                return w;
            }

			public DataTable Table (int Index)
			{
				return tn[Index];
			}

			public DataTable Find_Table (String Tname)
			{
				
                DataTable t=null;
				for (int i=1; i<=da_count;i++)
					if (tn[i].TableName.CompareTo(Tname)==0)
					{
						t= tn[i];
                        return t;
					}
				return t;
			}

            public void remove_table(DataTable Tbl)
            {
                for (int i = 0; i <= da_count; i++)
                    if (tn[i] == Tbl)
                    {
                        for (int j = i; j < da_count; j++)
                        {
                            da[j] = da[j + 1];
                            tn[j] = tn[j + 1];
                            arr[j] = arr[j + 1];
                        }
                        da_count--;
                    }
            }

            public int Find_Table_index(String Tname)
			{
				int t = -1;
				for (int i=1; i<=da_count;i++)
					if (tn[i].TableName.CompareTo(Tname)==0)
					{
						t= i;
                        return t;
					}
				return t;
			}

			~CLass_Tables_dim()
			{
				foreach (DataTable t in tn)
                //for (int i=1;i<=da_count;i++)
				{
                    try
                    {
                        if (t != null) t.Dispose();
                         }
                    catch { }
				}
			}
		}
        private void ErrLogInit()
        {
            EBBLP = new ErrorBBLProvider();
            EBBLP.ErrorOfBBL += new ErrorBBLHandler(EBBLP_ErrorOfBBL);
        }

        private bool IsConnect = false;
        public bool Connected
        {
            get { return IsConnect; }
        }

        public Class_syb_acc(int AMAS_base)
		{
            ErrLogInit();
            AMASCOnn = AMAS_base;
            switch ((AMAS_connections)AMAS_Base)
            {
                case AMAS_connections.Sybase:
                    //if (SAConnection_AMAS == null)
                    //{
                    //    ODBC_entry = "AMAS10";
                     //   ODBC_uid = null;
                    //    ODBC_pwd = null;
                    //    Connect();
                    //}
                    break;

                case AMAS_connections.MSSQL:
                    if (MSsql_Connection == null)
                    {
                        ODBC_entry = "AMAS/AMAS";
                        ODBC_uid = null;
                        ODBC_pwd = null;
                        IsConnect=Connect();
                    }
                    break;
            }
		}
		public Class_syb_acc(int AMAS_base,String entry_ODBC)
		{
            ErrLogInit();
            AMASCOnn = AMAS_base;
            switch ((AMAS_connections)AMAS_Base)
            {
                case AMAS_connections.Sybase:
                    //if (SAConnection_AMAS == null)
                    //{
                    //    ODBC_entry = entry_ODBC;
                    //    ODBC_uid = null;
                    //    ODBC_pwd = null;
                    //    Connect();
                    //}
                    break;

                case AMAS_connections.MSSQL:
                    if (MSsql_Connection == null)
                    {
                        ODBC_entry = entry_ODBC;
                        ODBC_uid = null;
                        ODBC_pwd = null;
                        IsConnect=Connect();
                    }
                    break;
    
            }
        }

        public Class_syb_acc(int AMAS_base,String entry_ODBC,String pwd_ODBC)
		{
            ErrLogInit();
            AMASCOnn = AMAS_base;
            switch ((AMAS_connections)AMAS_Base)
            {
                case AMAS_connections.Sybase:
                    //if (SAConnection_AMAS == null)
                    //{
                    //    ODBC_entry=entry_ODBC;
                    //    ODBC_uid=null;
                    //    ODBC_pwd=pwd_ODBC;
                    //    Connect();
                    //}
                    break;

                case AMAS_connections.MSSQL:
                    if (MSsql_Connection == null)
                    {
                        ODBC_entry=entry_ODBC;
                        ODBC_uid=null;
                        ODBC_pwd=pwd_ODBC;
                        IsConnect=Connect();
                    }
                    break;
			}

		}

		public Class_syb_acc(int AMAS_base,String entry_ODBC,String pwd_ODBC,String uid_ODBC)
		{
            ErrLogInit();
            AMASCOnn = AMAS_base;
            switch ((AMAS_connections)AMAS_Base)
            {
                case AMAS_connections.Sybase:
                    //if (SAConnection_AMAS == null)
                    //{
                    //    ODBC_entry = entry_ODBC;
                    //    ODBC_uid = uid_ODBC;
                    //    ODBC_pwd = pwd_ODBC;
                    //    Connect();
                    //}
                    break;

                case AMAS_connections.MSSQL:
                    if (MSsql_Connection == null)
                    {
                        ODBC_entry = entry_ODBC;
                        ODBC_uid = uid_ODBC;
                        ODBC_pwd = pwd_ODBC;
                        IsConnect=Connect();
                    }
                    break;
            }
		}

		private CLass_Tables_dim T_ARRAY ;

		public String org_cod
		{
			get {return ORG_COD;}
		}

		public int Field_count
		{
			get 
			{
				if (Tab_fields!=null)
					return Tab_fields.Fld_count;
				else return 0;
			}
		}

        public void refresh_execomm()
        {
            MSsql_ExecuteCommand = new SqlCommand();
            MSsql_ExecuteCommand.Connection = MSsql_Connection;
        }

        public delegate void ErrorBBLHand(string ErrS, int Ident);
        public event ErrorBBLHand ErrOfBBL;
        public delegate void ErrorBBLMaster(string ErrS, int Ident);
        public event ErrorBBLMaster ErrOfMaster;
        public delegate void ErrorBBLChief(string ErrS, int Ident);
        public event ErrorBBLChief ErrOfChief;
        public delegate void ErrorBBLRegistration(string ErrS, int Ident);
        public event ErrorBBLRegistration ErrOfRegistration;
        public delegate void ErrorBBLWFL_Admin(string ErrS, int Ident);
        public event ErrorBBLWFL_Admin ErrOfWFL_Admin;
        public delegate void ErrorBBLRights(string ErrS, int Ident);
        public event ErrorBBLRights ErrOfRights;
        public delegate void ErrorBBLResources(string ErrS, int Ident);
        public event ErrorBBLResources ErrOfResources;
        public delegate void ErrorBBLPersonel(string ErrS, int Ident);
        public event ErrorBBLPersonel ErrOfPersonel;
        public delegate void ErrorBBLOrganization(string ErrS, int Ident);
        public event ErrorBBLOrganization ErrOfOrganization;
        public delegate void ErrorBBLOutputDoc(string ErrS, int Ident);
        public event ErrorBBLOutputDoc ErrOfOutputDoc;

        private void EBBLP_ErrorOfBBL(string sss, int ModuleId,int Ident)
        {
            if (ErrOfBBL != null) ErrOfBBL(sss, Ident);
            switch (ModuleId)
            {
                case (int)Modules.Master:
                    if (ErrOfMaster != null) ErrOfMaster(sss, Ident);
                    return;
                case (int)Modules.Chief:
                    if (ErrOfChief != null) ErrOfChief(sss, Ident);
                    return;
                case (int)Modules.Personel:
                    if (ErrOfPersonel != null) ErrOfPersonel(sss, Ident);
                    return;
                case (int)Modules.Registration:
                    if (ErrOfRegistration != null) ErrOfRegistration(sss, Ident);
                    return;
                case (int)Modules.Resources:
                    if (ErrOfResources != null) ErrOfResources(sss, Ident);
                    return;
                case (int)Modules.Rights:
                    if (ErrOfRights != null) ErrOfRights(sss, Ident);
                    return;
                case (int)Modules.Structure:
                    if (ErrOfOrganization != null) ErrOfOrganization(sss, Ident);
                    return;
                case (int)Modules.WFL_Admin:
                    if (ErrOfWFL_Admin != null) ErrOfWFL_Admin(sss, Ident);
                    return;
                case (int)Modules.OutputDoc:
                    if (ErrOfOutputDoc != null) ErrOfOutputDoc(sss, Ident);
                    return;
            }
        }

		private bool Connect()
		{
            ResultString = "";
            bool connected = true;
            try
            { T_ARRAY = new CLass_Tables_dim(200, EBBLP); }
			catch (Exception err) 
            {ResultString +=" Error load T_ARRAY: " +err.Message;
            EBBLP.AddError(" Error load T_ARRAY: " + err.Message, "DBI Connect", err.StackTrace);
            }

            switch ((AMAS_connections)AMAS_Base)
            {
                case AMAS_connections.Sybase:
                    //SADataAdapter_AMAS = new iAnywhere.Data.SQLAnywhere.SADataAdapter();
                    //SACommand_AMAS = new iAnywhere.Data.SQLAnywhere.SACommand();
                    //SAConnection_AMAS = new iAnywhere.Data.SQLAnywhere.SAConnection();
			
                    //SADataAdapter_AMAS.SelectCommand = SACommand_AMAS;
                // 
                // SACommand_AMAS
                // 
                    //SACommand_AMAS.Connection = SAConnection_AMAS;

                // 
                // SAConnection_AMAS
                // 
                    //SAConnection_AMAS.ConnectionString = 
                        //"ENG="+ODBC_entry+";";
                    //    "DSN=" + ODBC_entry + ";";
                    if (ODBC_uid != null) 
                    {
                     //   SAConnection_AMAS.ConnectionString += 
                      //      "UID="+ODBC_uid+";"; 
                    }
                    if (ODBC_pwd != null)
                    {
                     //   SAConnection_AMAS.ConnectionString +=
                     //       "PWD=" + ODBC_pwd + ";";
                    }
                    try 
                    {
                    //    SAConnection_AMAS.Open();
                    }
                    catch(Exception e)
                    {
                        ResultString=e.Message;
                        EBBLP.AddError(e.Message, "DBI Connect +1", e.StackTrace);
                    }
                    break;
                
                case AMAS_connections.MSSQL:
                    string Constring = "server=" + ODBC_entry + ";";
                    if (ODBC_uid.Length >0)
                    {
                        Constring += "uid=" + ODBC_uid + ";";
                    }
                    if (ODBC_pwd.Length >0)
                    {
                        Constring += "password=" + ODBC_pwd + ";";
                    }
                    try
                    {
                        MSsql_Connection = new SqlConnection(Constring);
                        MSsql_DataAdapte = new SqlDataAdapter();
                        MSsql_SelectCommand = new SqlCommand();
                        MSsql_ExecuteCommand = new SqlCommand();
                        MSsql_DataAdapte.SelectCommand = MSsql_SelectCommand;
                        MSsql_SelectCommand.Connection = MSsql_Connection;
                        MSsql_ExecuteCommand.Connection = MSsql_Connection;
                        try
                        {
                            MSsql_Connection.Open();
                        }
                        catch 
                        { 
                            
                            connected = false; MSsql_Connection = null; 
                        }
                    }
                    catch 
                    {  
                        connected = false; MSsql_Connection = null; 
                    }
                    break;
            }
            DataTable tbl = null;
            if(connected)
            try
            {
                tbl = add_Table("Foundation", AMAS_Query.Class_AMAS_Query.Get_foundation, null);
                Class_Tools.Table_fld t = Get_row(0);
                t.First_field();
                try
                {
                    ORG_COD = t.T_value.ToString();
                }
                catch
                {
                    ORG_COD = "";
                }
                ResultString += ORG_COD;
                t.Next_field();
                try
                {
                    ORG_MAIL = t.T_value.ToString();
                }
                catch
                {
                    ORG_MAIL = "";
                } 
                ResultString += ORG_MAIL;
                t.Next_field();
                try
                {
                    CROSS_ORG = Convert.ToInt32(t.T_value);
                }
                catch
                {
                    CROSS_ORG = -1;
                }
                ResultString += CROSS_ORG.ToString();
                if (t.Find_field("org"))
                    try
                    {
                        DefORG_ID = (int)t.T_value;
                    }
                    catch
                    {
                        DefORG_ID = -1;
                    }
                
                if (t.Find_field("ApplicationVersion")) 
                    ApplicationVersion = t.T_value.ToString();
            }
            catch (Exception err)
            {
                ResultString += " Error load foundation=" + err.Message;
                EBBLP.AddError(" Error load foundation=" + err.Message, "DBI connect +2", err.StackTrace);
            }
            finally
            {
                if (tbl != null) ReturnTable();
            }
        
            if (connected)
            try
			{
                tbl = add_Table("Juridic", AMAS_Query.Class_AMAS_Query.Get_juridic, null);
				Class_Tools.Table_fld t= Get_row(0);
				t.First_field();
				ORG_NAME= t.T_value.ToString();
				ResultString +=ORG_NAME;
			}
			catch (Exception err)
			{
				ResultString +=" Error load foundation=" +err.Message;
                EBBLP.AddError(" Error load foundation=" + err.Message, "DBI connect +3", err.StackTrace);
			}
            finally
            {
                if (tbl != null) ReturnTable();
            }

            if (connected)
			try
			{
                tbl = add_Table("Juridic_address", AMAS_Query.Class_AMAS_Query.Get_jur_address, null);
				Class_Tools.Table_fld t= Get_row(0);
				t.First_field();
				ORG_State=t.T_value.ToString();
				ResultString +=ORG_State;
				t.Next_field();
				ORG_City=t.T_value.ToString();
				ResultString +=ORG_City;
				t.Next_field();
				ORG_Street=t.T_value.ToString();
				ResultString +=ORG_Street;
				t.Next_field();
				ORG_House=t.T_value.ToString();
				ResultString +=ORG_House;
				t.Next_field();
				ORG_Flat=t.T_value.ToString();
				ResultString +=ORG_Flat;
                t.Next_field();
                ORG_State_id = (int)t.T_value;
                t.Next_field();
                ORG_City_id = (int)t.T_value;
                t.Next_field();
                ORG_Street_id = (int)t.T_value;
                t.Next_field();
                ORG_House_id = (int)t.T_value;
                t.Next_field();
                ORG_Flat_id = (int)t.T_value;
                t.Find_field("region");
                ORG_Region = t.T_value.ToString();
                ResultString += ORG_Region;
                t.Find_field("areal");
                ORG_Areal = t.T_value.ToString();
                ResultString += ORG_Areal;
                t.Find_field("district");
                ORG_District = t.T_value.ToString();
                ResultString += ORG_District;

                t.Find_field("trc_id");
                ORG_Region_id = (int)t.T_value;
                t.Find_field("areal_id");
                ORG_Areal_id = (int)t.T_value;
                t.Find_field("district_id");
                ORG_District_id = (int)t.T_value;

            }
			catch (Exception err)
			{
				ResultString +=" Error load address=" +err.Message;
                EBBLP.AddError(" Error load address=" + err.Message, "DBI connect +4", err.StackTrace);
			}
            finally
            {
                if (tbl != null) ReturnTable();
            }

        if (connected)
        {
            try
            {
                tbl = add_Table("Tableuserid", "select current_user as usr_id", null);
                Class_Tools.Table_fld t = Get_row(0);
                t.First_field();
                Current_User = t.T_value.ToString();
            }
            catch (Exception err)
            {
                Current_User = ""; ResultString += " Error load Current_user=" + err.Message;
                EBBLP.AddError(" Error load Current_user=" + err.Message, "DBI connect +5", err.StackTrace);
            }
            finally
            {
                if (tbl != null) ReturnTable();
            }
            try
            {
                tbl = add_Table("TableuserName", "select  FIO from dbo.pop_my_name ", null);
                Class_Tools.Table_fld t = Get_row(0);
                t.First_field();
                UserName = t.T_value.ToString();
            }
            catch (Exception err)
            {
                UserName = ""; ResultString += " Error load Current_user=" + err.Message;
                EBBLP.AddError(" Error show FIO for user=" + err.Message, "DBI connect +5", err.StackTrace);
            }
            finally
            {
                if (tbl != null) ReturnTable();
            }
        }
        return connected;
		}

        private static bool RightsAddress = false;
        private static bool RightsAmas = false;
        private static bool RightsArchive = false;
        private static bool RightsBuilding = false;
        private static bool RightsEmployee = false;
        private static bool RightsEntrprice = false;
        private static bool RightsPeople = false;
        private static bool RightsRegistrator = false;
        private static bool RightsStructure = false;
        private static bool RightsMail = false;
        private static bool RightsListing = false;
        private static bool RightsWorkflow = false;
        private static bool RightsWorkflow_admin = false;
        private static bool RightsGlobal_search = false;
        private static bool RightsPost = false;
        private static bool RightsInpost = false;
        private static bool RightsSecurity = false;
        private static bool RightsDebug = false;
        private static bool RightsLeader = false;
        private static bool RightsBusinessProcess = false;

        public class GetMyRights
        {
            public bool Address { get { return RightsAddress; } }
            public bool Amas { get { return RightsAmas; } }
            public bool Archive { get { return RightsArchive; } }
            public bool BusinessProcess { get { return RightsBusinessProcess; } }
            public bool Building { get { return RightsBuilding; } }
            public bool Employee { get { return RightsEmployee; } }
            public bool Entrprice { get { return RightsEntrprice; } }
            public bool Registrator { get { return RightsRegistrator; } }
            public bool Structure { get { return RightsStructure; } }
            public bool Mail { get { return RightsMail; } }
            public bool Listing { get { return RightsListing; } }
            public bool Workflow { get { return RightsWorkflow; } }
            public bool Workflow_admin { get { return RightsWorkflow_admin; } }
            public bool Global_search { get { return RightsGlobal_search; } }
            public bool People { get { return RightsPeople; } }
            public bool Post { get { return RightsPost; } }
            public bool Inpost { get { return RightsInpost; } }
            public bool Security { get { return RightsSecurity; } }
            public bool Debug { get { return RightsDebug; } }
            public bool Leader { get { return RightsLeader; } }

            public GetMyRights( Array Rights)
            {
                SetMyRights(Rights);
                RightsLeader = AMASCommand.IAmLeader();
            }

            private void SetMyRights(Array ItsMyRights)
            {
                if (ItsMyRights != null)
                    for (int i = 0; i < ItsMyRights.GetLength(0); i++)
                    {
                        string RTS = (string)ItsMyRights.GetValue(i, 0);
                        switch (RTS.Trim().ToLower())
                        {
                            case "address":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsAddress = true;
                                break;
                            case "amas":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsAmas = true;
                                break;
                            case "archive":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsArchive = true;
                                break;
                            case "building":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsBuilding = true;
                                break;
                            case "employee":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsEmployee = true;
                                break;
                            case "entrprice":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsEntrprice = true;
                                break;
                            case "people":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsPeople = true;
                                break;
                            case "registrator":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsRegistrator = true;
                                break;
                            case "structure":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsStructure = true;
                                break;
                            case "mail":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsMail = true;
                                break;
                            case "listing":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsListing = true;
                                break;
                            case "workflow":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsWorkflow = true;
                                break;
                            case "workflow_admin":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsWorkflow_admin = true;
                                break;
                            case "global_search":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsGlobal_search = true;
                                break;
                            case "post":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsPost = true;
                                break;
                            case "inpost":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsInpost = true;
                                break;
                            case "security":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsSecurity = true;
                                break;
                            case "debug":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsDebug = true;
                                break;
                            case "busynessprocess":
                                if (Convert.ToInt32(ItsMyRights.GetValue(i, 2)) == 1)
                                    RightsBusinessProcess = true;
                                break;
                        }
                    }
            }
        }

        private last_point TO_point;

		public DataTable add_Table(String tableName, String sql, PrepareParameters[] Parameters )
		{
			ResultString="";
            //int res;
                if (Tab_fields != null) { TO_point = new last_point(TO_point,Tune_table, Current_row, Tab_fields.The_field); }
                else { TO_point = new last_point(TO_point,Tune_table, Current_row); }

			try 
			{
				Tune_table=new DataTable(tableName);
                Tune_table.Clear();
                switch ((AMAS_connections)AMAS_Base)
                {
                    case AMAS_connections.Sybase:
                        //SACommand_AMAS.CommandText = sql;
                        //SADataAdapter_AMAS.Fill(Tune_table);
                        break;
                    case AMAS_connections.MSSQL:
                        //MSsql_SelectCommand.CommandText = sql;
                        //MSsql_SelectCommand.Parameters.Clear();
                        //foreach (PrepareParameters p in Parameters)
                        //    MSsql_SelectCommand.Parameters.Add(p.name, p.dbType).Value = p.Value;
                        //MSsql_DataAdapte.Fill(Tune_table);
                        //res = T_ARRAY.append_table(sql, Tune_table, Parameters); 
                        break;
                }
			}
			catch(Exception e)
			{
                ResultString=e.Message;
                EBBLP.AddError(e.Message, "DBI +6", e.StackTrace);
                Tune_table=null;
            }
			T_ARRAY.append_table (sql,Tune_table,Parameters);
			return Tune_table;
		}

        public bool Requery_Table(String sql, PrepareParameters[] Parameters)
        {
            ResultString = "";
            bool res;
            Tune_table.Clear();
            res = T_ARRAY.requery(T_ARRAY.Find_Table_index(Tune_table.TableName), sql, Parameters);
            Get_row(0);
            return res;
        }

        public bool Set_table(string Table, string sql,PrepareParameters[] Parameters)
        {
            bool res;
            //if (this.FindTable(Table) != null)
            //{
            //    res=this.Requery_Table(sql,Parameters);
            //}
            //else
            {
                if (this.add_Table( Table, sql, Parameters) != null) res = true; else res=false ;
            }
            if (res) this.Get_row(0);
            return res;
        }

        public void Show_Schema()
		{
			ResultString="";
			if (Tune_table != null)
				for (int i=0;i<Tune_table.Columns.Count;i++) ResultString+=Tune_table.Columns[i].ColumnName+" ! ";
			ResultString+="\n\r";
		}
	
		private Class_Tools.Table_fld Tab_fields;

		public Class_Tools.Table_fld Get_row(int Seek_row) //
		{
			DataRow r;
			ResultString="";
            if (Seek_row < Rows_count && Tune_table != null && Seek_row>=0)
			{
				Current_row=Seek_row;
				Tab_fields= new Class_Tools.Table_fld();
				try 
				{
					r=Tune_table.Rows[Seek_row];
					for (int k=0; k< Tune_table.Columns.Count;k++) 
					{
						Tab_fields.name=Tune_table.Columns[k].ColumnName;
						Tab_fields.T_value=r[Tab_fields.name];
                        if ("System.Byte[]".CompareTo(Tab_fields.T_value.GetType().ToString()) == 0)
                            {
                            
                            MemoryStream m= new MemoryStream((Byte[])r[Tab_fields.name]);
                            Tab_fields.T_stream =(Stream) m ;
                            ResultString += Tab_fields.name + "<" + Tab_fields.typ + ">" + "= BLOB \n\r";
						}
						else ResultString+=Tab_fields.name+"<"+Tab_fields.typ+">"+"="+Tab_fields.T_value.ToString()+"\n\r";
					}
				}
				catch(Exception ex) 
                {
                    //ResultString+="Error in Table  --  "+ex.Message;
                    EBBLP.AddError("Error in Table  --  " + ex.Message, "DBI +7: " + Tune_table.TableName, ex.StackTrace);
                    Tab_fields=null;}
			}
			else {Tab_fields=null; Current_row=Rows_count-1;}
			return Tab_fields;

		}

		public Object Find_Field(string Fname)
		{
			ResultString="";
			if (Tab_fields !=null)
				if (Tab_fields.Find_field(Fname))
					return Tab_fields.T_value;
				else return null;
			else return null;
		}

        public Stream Find_Stream(string Fname)
        {
            ResultString = "";
            if (Tab_fields != null)
                if (Tab_fields.Find_field(Fname))
                    return Tab_fields.T_stream;
                else return null;
            else return null;
        }
        
        public Object get_current_Field()
		{
			ResultString="";
			if (Tab_fields !=null)
					return Tab_fields.T_value;
			else return null;
		}

        public string get_current_File()
        {
            ResultString = "";
            if (Tab_fields != null)
                return Tab_fields.T_file;
            else return null;
        }
        
        public Type get_current_typ()
		{
			ResultString="";
			if (Tab_fields !=null)
				return Tab_fields.typ;
			else return null;
		}

		public Stream get_current_Stream()
		{
			ResultString="";
			if (Tab_fields !=null)
				return Tab_fields.T_stream;
			else return null;
		}

		public Object get_first_Field()
		{
			ResultString="";
			if (Tab_fields !=null)
			{
				Tab_fields.First_field();
				return Tab_fields.T_value;
			}
			else return null;
		}

		public Object get_last_Field()
		{
			ResultString="";
			if (Tab_fields !=null)
			{
				Tab_fields.last_field();
				return Tab_fields.T_value;
			}
			else return null;
		}

		public Object get_next_Field()
		{
			ResultString="";
			if (Tab_fields !=null)
			{
				Tab_fields.Next_field();
				return Tab_fields.T_value;
			}
			else return null;
		}

		public Object get_prev_Field()
		{
			ResultString="";
			if (Tab_fields !=null)
			{
				Tab_fields.prev_field();
				return Tab_fields.T_value;
			}
			else return null;
		}

		public Object get_index_Field(int index)
		{
			ResultString="";
			if (Tab_fields !=null)
			{
				Tab_fields.seek_field(index);
				return Tab_fields.T_value;
			}
			else return null;
		}

		public string get_Field_name()
		{
			ResultString="";
			if (Tab_fields !=null)
				return Tab_fields.name;
			else return null;
		}

		public String Show_Columns()
		{
			DataTable q=Tune_table;
			String t="";
			ResultString="";
			try 
			{
				for (int k=0;k<q.Columns.Count;k++) 
				{
					t+=q.Columns[k].ColumnName;
					t+=" type:"+q.Columns[k].DataType.Name+"\r\n";
				}
			}
			catch(Exception ex)
            {
                ResultString+="Error  --  "+ex.Message;
                EBBLP.AddError(ex.Message, "DBI +8", ex.StackTrace);
            }
			return t;
		}

		public DataTable FindTable (string table)
		{
			
			ResultString="";
            
            if (Tune_table != null)
                if (Tab_fields != null) { TO_point = new last_point(TO_point,Tune_table, Current_row, Tab_fields.The_field); }
                else { TO_point = new last_point(TO_point,Tune_table, Current_row); }

			try
			{
				Tune_table=T_ARRAY.Find_Table(table);
				ResultString+=Tune_table.TableName;
				return Tune_table;
			}
			catch(Exception ex)
			{ResultString+="Error  --  "+ex.Message;
            EBBLP.AddError(ex.Message, "DBI +(", ex.StackTrace);
			return null;
			}
		}
        public void ReturnTable()
        {
            if (TO_point != null)
            {
                T_ARRAY.remove_table(Tune_table);
                Tune_table = TO_point.Last_Table;
                Current_row = TO_point.row;
                Get_row(Current_row);
                if (Tab_fields != null) Tab_fields.The_field = TO_point.field;
                TO_point = TO_point.chain;
                //if (Tune_table == T_ARRAY.prev_table()) 
                //    T_ARRAY.return_table();
            }
        }

        public string ReturnTableName()
        {
            return Tune_table.TableName;
        }
    }

	public class Class_Tools
	{
		public Class_Tools()
		{
		}

		private enum TYP_data : byte
		{
			INT=1, STR=2, FLOAT=3, LOGICAL=4, INT16=5, INT32=6, INT64=7, BYTE=8, BYTES=9, BIT=10, 
			DATE=11, GUID=12, SINGLE=15, DOUBLE=16, DEFAULT=17, DECIMAL=18
		}
		public class Table_fld
		{
			
			private int Fields_count;
			const int first_field=1;
			private int Current_field;
			Field_content[] F_C;

            public int The_field 
            { 
                get { return Current_field; }
                set { if (The_field < 0) The_field = 0; else if (The_field > Fields_count) The_field = Fields_count; } 
            }

			public Table_fld (){Fields_count=0;F_C=new Field_content[1];Current_field=0;}
			
			private struct Field_content
			{
				public Type O_typ;
				public String O_name;
				public Object O_value;
				public Stream O_stream;
                public string O_file;
			}
			
			public Object FAC
			{
				get{return F_C;}
			}

			public int Fld_count
			{
				get {return Fields_count;}
			}

			public Type typ
			{
				get{return F_C[Current_field].O_typ;}
			}
			
			private Field_content[] fc;
			
			public String name
			{
				get {	return F_C[Current_field].O_name;}
				set 
				{
					fc= new Field_content[++Fields_count+1];
					for (int i=1; i<Fields_count;i++)
					{
						fc[i].O_name=F_C[i].O_name;
						fc[i].O_typ=F_C[i].O_typ;
						fc[i].O_value=F_C[i].O_value;
                        fc[i].O_stream = F_C[i].O_stream;
                        fc[i].O_file = F_C[i].O_file;
					}
					Current_field=Fields_count;
                    fc[Current_field].O_name = value;
					F_C=fc;
                    fc[Current_field].O_value = fc[0].O_value;
                    fc[Current_field].O_stream = fc[0].O_stream;
                    fc[Current_field].O_file = fc[0].O_file;
                }
			}

            private string To_File()
            {
                string filename = "";
                try
                {
                    Stream s = F_C[Current_field].O_stream;
                    int bufsize = 100000;
                    byte[] buf = new byte[bufsize];
                    string ext = "";
                    int numBytes = 0;
                    //ext = (string)AMASacc.Find_Field("ext");
                    //s = AMASacc.Find_Stream(a_field);
                    numBytes=s.Read(buf, 0, CommonValues.CommonClass.headShift);
                    for (int i = 0; i < CommonValues.CommonClass.headShift; i++)
                        ext += Convert.ToString((char)Convert.ToChar(buf[i]));
                    ext = ext.Trim();
                    if (ext != null) if (ext.Length > 0) ext = "." + ext;
                    filename = ext;
                    //CommonValues.CommonClass.tempFiles.Delete();
                    //CommonValues.CommonClass.tempFiles.AddFile(filename, false);
                    filename = CommonValues.CommonClass.TempDirectory + Path.GetRandomFileName() + ext;
                    bool goodFile = false;
                    do
                    {
                        try
                        {
                            goodFile = true;
                            if (File.Exists(filename))
                            {
                                goodFile = false;
                                File.Delete(filename);
                                goodFile = true;
                            }
                        }
                        catch { filename = CommonValues.CommonClass.TempDirectory + Path.GetRandomFileName() + ext;  }
                    }
                    while (!goodFile);
                    FileStream stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                    long pos = CommonValues.CommonClass.headShift;
                    while (pos < (s.Length - bufsize))
                    {
                        s.Read(buf, 0, bufsize);
                        stream.Write(buf, 0, bufsize);
                        pos += bufsize;
                    }
                    bufsize = (int)(s.Length - pos);
                    s.Read(buf, 0, bufsize);
                    stream.Write(buf, 0, bufsize);
                    //s.Close();
                    stream.Close();
                }
                catch
                {

                }
                return filename;

            }

			public Object T_value
			{
				get {return F_C[Current_field].O_value;}
				set 
				{
						
					F_C[Current_field].O_typ=value.GetType();
					F_C[Current_field].O_value=value;
                    F_C[Current_field].O_stream = null;
                    F_C[Current_field].O_file = null;
                }
			}

			public Stream T_stream
			{
				get {return F_C[Current_field].O_stream;}
				set
				{
					F_C[Current_field].O_typ=value.GetType();
                    F_C[Current_field].O_value = null;
                    //F_C[Current_field].O_value = value;
                    F_C[Current_field].O_stream = (Stream)value;
                    //F_C[Current_field].O_stream = null;
                }
			}

            public string T_file
            {
                get { return To_File(); }
            }
            private long BLOB_fun()
            {
                int blobLen = 0;
                return blobLen;
            }

			public bool First_field()
			{
				if (Fields_count>=first_field )
				{
					Current_field=first_field;
					return true;
				}
				else 
				{
					Current_field=0;
					return false;
				}
			}

			public bool Next_field()
			{
				if (Current_field<Fields_count)
				{
					Current_field++;
					return true;
				}
				else
				{
					Current_field=Fields_count;
					return false;
				}

			}

			public bool last_field()
			{
				Current_field=Fields_count;
				return true;
			}

			public bool seek_field(int nfld)
			{
				if ((nfld>=0) && (nfld<=Fields_count))
				{
					Current_field=nfld;
					return true;
				}
				else if (nfld<0)
					 {
						 Current_field=0;
						 return false;
					 }
				else
				{
					Current_field=Fields_count;
					return false;
				}


			}

			public bool prev_field()
			{
				if (Current_field>0)
				{
					Current_field--;
					return true;
				}
				else
				{
					Current_field=0;
					return false;
				}
			}

			public bool Find_field(string fname)
			{
			bool finded=false;
            try
            {
                for (Current_field = 1; Current_field <= Fields_count; Current_field++)
                    if (F_C[Current_field].O_name.ToLower().CompareTo(fname.ToLower()) == 0)
                    {
                        finded = true;
                        break;
                    }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message);  finded = false; }
			return finded;
			}

            //			public string T_String;
			//			public int T_Integer;
			//			public bool T_bool;
			//			public byte T_byte;
			//			public byte[] T_bytes;
			//			public float T_float;
			//			public Int64 T_int64;
			//			public Decimal T_decimal;
			//			public Double T_double;
			//			public Guid T_guid;
			//			public Single T_single;
			//			public Int16 T_Int16;
		}
		private TYP_data ODBC_convert(System.Data.Odbc.OdbcType odbc)
		{
			TYP_data t;
			switch (odbc )
			{			
				case System.Data.Odbc.OdbcType.BigInt: //SQL_BIGINT:
						
					t=TYP_data.INT64;
					break;
				case System.Data.Odbc.OdbcType.Binary: //SQL_BINARY:
				case System.Data.Odbc.OdbcType.VarBinary: //SQL_VARBINARY: //SQL_LONGVARBINARY:
				case System.Data.Odbc.OdbcType.Image:
					t=TYP_data.BYTES;
					break;
				case System.Data.Odbc.OdbcType.Bit: //SQL_BIT:
					t=TYP_data.LOGICAL;
					break;
				case System.Data.Odbc.OdbcType.Char: //SQL_CHAR:
				case System.Data.Odbc.OdbcType.NChar: //SQL_WCHAR:
				case System.Data.Odbc.OdbcType.Text: //SQL_LONGVARCHAR:
				case System.Data.Odbc.OdbcType.NText: //SQL_WLONGVARCHAR:
				case System.Data.Odbc.OdbcType.NVarChar: //SQL_WVARCHAR:
				case System.Data.Odbc.OdbcType.VarChar: //SQL_LONG_VARCHAR:
					t=TYP_data.STR;
					break;
				case System.Data.Odbc.OdbcType.Numeric: //SQL_NUMERIC:
				case System.Data.Odbc.OdbcType.Decimal: //SQL_DECIMAL:
					t=TYP_data.DECIMAL;
					break;
				case System.Data.Odbc.OdbcType.Double: //SQL_DOUBLE:
					t=TYP_data.DOUBLE;
					break;
				case System.Data.Odbc.OdbcType.UniqueIdentifier://SQL_GUID:
					t=TYP_data.GUID;
					break;
				case System.Data.Odbc.OdbcType.Int: //SQL_INTEGER:
					t=TYP_data.INT;
					break;
				case System.Data.Odbc.OdbcType.Real: //SQL_REAL:
					t=TYP_data.SINGLE;
					break;
				case System.Data.Odbc.OdbcType.SmallInt: //SQL_SMALLINT:
					t=TYP_data.INT16;
					break;
				case System.Data.Odbc.OdbcType.TinyInt: //SQL_TINYINT:
					t=TYP_data.BYTE;
					break;
				case System.Data.Odbc.OdbcType.Time: //SQL_TYPE_TIMES:
				case System.Data.Odbc.OdbcType.Timestamp: //SQL_TYPE_TIMESTAMP:
				case System.Data.Odbc.OdbcType.SmallDateTime:
				case System.Data.Odbc.OdbcType.DateTime:
				case System.Data.Odbc.OdbcType.Date:
					t=TYP_data.DATE;
					break;
				default:
					t=TYP_data.DEFAULT;
					break;
			}
			return t;
		}

	}
}


