using System;
using AMAS_DBI;
using System.Data;
using CommonValues;

namespace AMAS_Query
{

    public static class WellWork
    {
        private enum WelWk { well = 1, work }
        private static int Well_vis_Work = (int)WelWk.work;
        public static bool Well 
        { 
            get { if (Well_vis_Work == (int)WelWk.well) return true; else return false;}
            set { if (value == true) Well_vis_Work = (int)WelWk.well; else Well_vis_Work = (int)WelWk.work; }
        }
        public static bool Work 
        { 
            get { if (Well_vis_Work == (int)WelWk.work) return true; else return false;}
            set { if (value == true) Well_vis_Work = (int)WelWk.work; else Well_vis_Work = (int)WelWk.well; }
        }
    }

    public static class ClassAMAS_Buissnes_Process
    {
        public static string EventsList
        {
            get
            {
                string sql = "select id,name,description_, kind, tema, route_, timeDelay from dbo.bpr_events_library order by name asc";
                return sql;
            }
        }


        public static string ConditionsList
        {
            get
            {
                string sql = "select id,name,description_, Role, execution_ from dbo.bpr_conditions order by name asc";
                return sql;
            }
        }
        
        public static string event_execution_list 
        {
            get    
        {
            string Sql = "select bpr_executions.name,bpr_executions.id, case bpr_event_executions.event_ when @event_ then bpr_event_executions.event_ else -1 end as checked from dbo.bpr_executions left join dbo.bpr_event_executions on bpr_executions.id=bpr_event_executions.executions order by name asc"; // where bpr_event_executions.event_= @event_ order by name asc ";
                return Sql;
            }
        }
        
        public static string RoutesList
        {
            get
            {
                string sql = "select id,name,description_ from dbo.bpr_route order by name asc";
                return sql;
            }
        }

        public static string RoulesList
        {
            get
            {
                string sql = "select id, name, description_ from dbo.bpr_roles where instruction_=@instr order by name asc";
                return sql;
            }
        }

        public static string AllRoulesList
        {
            get
            {
                string sql = "select id, name, description_ from dbo.bpr_roles order by name asc";
                return sql;
            }
        }

        public static string AllTasksList
        {
            get
            {
                string sql = "select id, name, description_, role_, viza, kind, tema, minutes, executing_ from dbo.bpr_executions order by name asc";
                return sql;
            }
        }

        public static string BPTasksList
        {
            get
            {
                string sql = "select id, [name], description_, role_, viza, kind, tema, [minutes], executing_ from dbo.bpr_executions where role_=@role order by name asc";
                return sql;
            }
        }

        public static string MyTasksList
        {
            get
            {
                string sql = "select bpr_executions.id, bpr_executions.[name], bpr_executions.description_, bpr_executions.role_, bpr_executions.viza, bpr_executions.kind, bpr_executions.tema, bpr_executions.[minutes], bpr_executions.executing_ from dbo.bpr_executions join dbo.bpr_roles on bpr_executions.role_=bpr_roles.id order by bpr_executions.name asc";
                return sql;
            }
        }

        public static string BPTasksConnectList
        {
            get
            {
                string sql = "select bpr_executions.id, bpr_executions.name, bpr_executions.description_, bpr_executions.role_, bpr_executions.viza, bpr_executions.kind, bpr_executions.tema, bpr_executions.minutes, bpr_executions.executing_ ";
                sql += " , connTask=isnull(bpr_Connections.id,0) ";
                sql += " from dbo.bpr_executions full outer join dbo.bpr_Connections on bpr_executions.id=bpr_Connections.execution_  where bpr_executions.role_=@role order by name asc ";
                return sql;
            }
        }

        public static string BPRouteConnectList
        {
            get
            {
                string sql = "select id, name, description_, execution_  from  dbo.bpr_Connections where route_=@route order by name asc ";
                return sql;
            }

        }

        public static string InstructionsList
        {
            get
            {
                string sql = "select cod,rank,line from dbo.ORG_rank_instructions  order by line asc";
                return sql;
            }
        }
    }

    public static class Class_AMAS_Query
    {

        /*Загрузка системы*/
        public static string Get_foundation 
        { 
            get 
            {
                string Sql="";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql= "select org_cod,e_mail,org from dba.foundation";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql= "select org_cod,e_mail,org,ApplicationVersion from dbo.set_foundation";
                        break;
                }
                return Sql;
            } 
        }

        // Registers
        public static string SeekJuridic
        {
             get
             {
                 string Sql = "";
                 switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                 {
                     case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                         Sql = "select * from dba.sub_org";
                         break;
                     case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                         Sql = "select * from dbo.temp_sub_org";
                         break;
                 }
                 return Sql;
             }
        }

        public static string  SeekPeople
        {
            get
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.sub_population";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from dbo.temp_sub_population";
                        break;
                }
                return Sql;
            }
        }

        public static string SeekEmployee(int juridic)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select fio,name,cod from dba.ent_employee,dba.jrd_degree where jrd_degree.degree=ent_employee.degree and jrd_degree.juridic=" + juridic.ToString() + " order by fio ";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select family,emp_ent_employee.[name],father,fio,org_jrd_degree.name as degree,emp_ent_employee.cod,emp_ent_employee.id,org_jrd_degree.degree as iddeg, emp_ent_employee.agent from dbo.emp_ent_employee join dbo.org_jrd_degree on org_jrd_degree.degree=emp_ent_employee.degree where org_jrd_degree.juridic=" + juridic.ToString() + " order by fio ";
                        break;
                }
                return Sql;
            }
        }

        public static string SeekDegree
        {
            get
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.degree";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from dbo.org_degree";
                        break;
                }
                return Sql;
            }
        }

        //Address List

        public static string GetStateList
        {
            get
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.pip_state where id=sinonim order by state";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from amas.adr_state where id=sinonim order by state";
                        break;
                }
                return Sql;
            }
        }

        public static string GetAddressIds(int flat)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select pop_state.id as state, pop_city.id as city, pop_street.id as street, pop_house.id as house ";
                        Sql += "from dbo.pop_state join amas.pop_city join ";
                        Sql += "dbo.pop_street join ";
                        Sql += "dbo.pop_house join ";
                        Sql += "dbo.pop_flat ";
                        Sql += "where dbo.pop_flat.id=" + flat.ToString();
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select adr_state.id as state, adr_trc.id as region, adr_areal.id as areal, adr_city.id as city, adr_district.id as district, adr_street.id as street, adr_house.id as house ";
                        Sql += "from amas.adr_state join amas.adr_trc on amas.adr_state.id= amas.adr_trc.sta_id join ";
                        Sql += "amas.adr_areal on amas.adr_trc.id= amas.adr_areal.trc_id join ";
                        Sql += "amas.adr_city on amas.adr_areal.id= amas.adr_city.areal_id join ";
                        Sql += "amas.adr_district on amas.adr_city.id=amas.adr_district.cit_id join ";
                        Sql += "amas.adr_street on amas.adr_district.id=amas.adr_street.district_id join ";
                        Sql += "amas.adr_house on amas.adr_street.id=amas.adr_house.str_id join ";
                        Sql += "amas.adr_flat on amas.adr_house.id=amas.adr_flat.hou_id ";
                        Sql += "where amas.adr_flat.id=" + flat.ToString();
                        break;
                }
                return Sql;
            }
        }

        public static string GetTrcList(int sta_id)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.pip_city where id=sinonim and sta_id=" + sta_id.ToString() + " order by name_city";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from AMAS.adr_trc where id=sinonim and sta_id=" + sta_id.ToString() + " order by Region";
                        break;
                }
                return Sql;
            }
        }

        public static string GetArealList(int trc_id)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.pip_city where id=sinonim and sta_id=" + trc_id.ToString() + " order by name_city";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from AMAS.adr_areal where id=sinonim and trc_id=" + trc_id.ToString() + " order by areal";
                        break;
                }
                return Sql;
            }
        }

        public static string GetCityList (int areal_id)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.pip_city where id=sinonim and sta_id=" + areal_id.ToString() + " order by name_city";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from AMAS.adr_city where id=sinonim and areal_id=" + areal_id.ToString() + " order by name_city";
                        break;
                }
                return Sql;
            }
        }

        public static string GetDistrictList(int cit_id)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.pip_street where id=sinonim and cit_id=" + cit_id.ToString() + " order by streetname";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from AMAS.adr_district where id=sinonim and cit_id=" + cit_id.ToString() + " order by district";
                        break;
                }
                return Sql;
            }
        }

        public static string GetStreetList(int district_id)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.pip_street where id=sinonim and cit_id=" + district_id.ToString() + " order by streetname";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from AMAS.adr_street where id=sinonim and district_id=" + district_id.ToString() + " order by streetname";
                        break;
                }
                return Sql;
            }
        }

        public static string GetHouseList(int str_id)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.pip_house where id=sinonim and str_id=" + str_id.ToString() + " order by h_int";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from AMAS.adr_house where id=sinonim and str_id=" + str_id.ToString() + " order by h_int";
                        break;
                }
                return Sql;
            }
        }

        public static string GetFlatList(int hou_id)
        {
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select * from dba.pip_flat where id=sinonim and hou_id=" + hou_id.ToString() + " order by f_int";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select * from AMAS.adr_flat where id=sinonim and hou_id=" + hou_id.ToString() + " order by f_int";
                        break;
                }
                return Sql;
            }
        }

        //Start Setup
        public static string Get_Current_user
        {
            get
            {
                string Sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select current user as usr_id";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select usr_id=current_user "; 
                        break;
                }
                return Sql;
            }
        }

        public static string Get_juridic 
        { 
            get 
            { 
                string Sql="";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select Full_name from dba.jrd_juridic join dba.foundation on jrd_juridic.id=foundation.org";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select Full_name from dbo.org_jrd_juridic join dbo.set_foundation on org_jrd_juridic.id=set_foundation.org";
                        break;
                }
                return Sql;
            } 
        }

        public static string Get_jur_address 
        { 
            get 
            { 
                string Sql="";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        Sql = "select pip_state.state, pip_city.name_city,pip_street.streetname, pip_house.house, pip_flat.flat from dba.pip_state join dba.pip_city join dba.pip_street join dba.pip_house join dba.pip_flat join dba.jrd_juridic join dba.foundation on jrd_juridic.id=foundation.org";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        Sql = "select adr_state.state, adr_city.name_city,adr_street.streetname, adr_house.house, adr_flat.flat,";
                        Sql += "adr_state.id as state_id, adr_city.id as city_id,adr_street.id as street_id, adr_house.id as house_id, adr_flat.id as flat_id, ";
                        Sql += "adr_trc.region, adr_trc.id as trc_id, adr_areal.areal, adr_areal.id as areal_id,";
                        Sql += "adr_district.district, adr_district.id as district_id ";
                        Sql += "from AMAS.adr_state join amas.adr_trc on adr_state.id=adr_trc.sta_id ";
                        Sql += "join amas.adr_areal on adr_trc.id=adr_areal.trc_id  ";
                        Sql += "join AMAS.adr_city on adr_areal.id=adr_city.areal_id  ";
                        Sql += "join amas.adr_district on adr_city.id=adr_district.cit_id  ";
                        Sql += "join AMAS.adr_street on adr_district.id=adr_street.district_id  ";
                        Sql += "join AMAS.adr_house on adr_street.id=adr_house.str_id  ";
                        Sql += "join AMAS.adr_flat on adr_house.id=adr_flat.hou_id  ";
                        Sql += "join dbo.org_jrd_juridic on adr_flat.id=org_jrd_juridic.fla_id  ";
                        Sql += "join dbo.set_foundation on org_jrd_juridic.id=set_foundation.org ";
                        break;
                }
        return Sql;
    } 
        }
        
        /**/

        public static string Get_Anketa(int employee)
        {
            return "select ext,anketa from dbo.emp_f2 where  employee= " + employee.ToString();
        }

        public static string Get_photo(int employee)
        {
            return "select photo from dbo.pop_photo where  people= " + employee.ToString();
        }
 
        /*Списки документов*/
        public static string Get_document_by_ID(int doc_id)
        {
            if (doc_id > 0)
                return "select * from dbo.rkk_flow_document where kod=  " + (string)Convert.ToString(doc_id);
            else return "";
        }

        public static string Get_rank_from_heap(DateTime executed, int for_)
        {
            string sql="";
            int year = (int)executed.Year;
            if (executed < DateTime.MinValue)
            {
                executed = DateTime.MinValue;
                year = (int)executed.Year;
            }
            if (year >1990)
            {
                if (for_ > 0)
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select id, cod from dba.heap_dep_degrees where ins_date >cast('" + executed.ToString("yyyy.MM.dd") + "' as date) and cod=" + (string)Convert.ToString(for_) + " order by ins_date asc";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select id, cod from dbo.EMP_heap_dep_degrees where ins_date > @executed and cod=" + (string)Convert.ToString(for_) + " order by ins_date asc";
                        break;
                }
            }
            else
            {
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select id, cod from dba.heap_dep_degrees where cod=" + (string)Convert.ToString(for_) + " order by ins_date asc";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select id, cod from dbo.EMP_heap_dep_degrees where cod=" + (string)Convert.ToString(for_) + " order by ins_date asc";
                        break;
                }
            }
            return sql;
        }

        public static string Get_moving_from_heap(int moving)
        {
            string sql = "";
            if (moving > 0)
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql =  "select for_ from dba.heap_moving where for_ is not null and moving = " + (string)Convert.ToString(moving) + " order by id asc";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select for_ from dbo.RKK_heap_moving where for_ is not null and moving = " + (string)Convert.ToString(moving) + " order by id asc";
                        break;
                }
            return sql;
        }

        public static string Get_employee_by_rank_on_heap(int Id)
        {
            string sql = "";
            if (Id > 0)
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select employee.family + ' ' + employee.name + ' ' + employee.father as fio, employee.employee,heap_dep_degrees.cod,jrd_degree.name as rank, jrd_degree.degree as decod, heap_dep_degrees.department as department from dba.employee join dba.heap_dep_degrees on employee.employee=heap_dep_degrees.employee join dba.jrd_degree on heap_dep_degrees.degree=jrd_degree.degree where heap_dep_degrees.id  =" + (string)Convert.ToString(Id);
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select emp_employee.family + ' ' + emp_employee.name + ' ' + emp_employee.father as fio, emp_employee.employee,emp_heap_dep_degrees.cod,org_jrd_degree.name as rank, org_jrd_degree.degree as decod, emp_heap_dep_degrees.department as department from dbo.empemployee join dbo.emp_heap_dep_degrees on emp_employee.employee=emp_heap_dep_degrees.employee join dbo.org_jrd_degree on emp_heap_dep_degrees.degree=org_jrd_degree.degree where emp_heap_dep_degrees.id  =" + (string)Convert.ToString(Id);
                        break;
                }
            return sql;
        }

        public static string Get_short_fio_of_movie(int for_)
        {
            string sql = "";
            if (for_ > 0)
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql= "select jrd_degree.name, ent_employee.short_fio  from dba.jrd_degree join dba.dep_degrees join dba.ent_employee on dep_degrees.cod=ent_employee.cod where dep_degrees.cod=" + (string)Convert.ToString(for_);
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select org_jrd_degree.name, emp_ent_employee.short_fio  from dbo.org_jrd_degree join dbo.emp_dep_degrees on org_jrd_degree.degree=emp_dep_degrees.degree join dbo.emp_ent_employee on emp_dep_degrees.cod=emp_ent_employee.cod where emp_dep_degrees.cod=" + (string)Convert.ToString(for_);
                        break;
                }
            return sql;
        }

        public static string Get_Temy_Kind_Employee(int key)
        {
            string sql = "";
            switch (key)
                {
                    case 1:
                        sql = "select * from dbo.RKK_tema order by description_";
                        break;
                    case 2:
                        sql = "select * from dbo.RKK_kind order by kind";
                        break;
                    case 3:
                        sql = "select * from dbo.emp_employee order by family, name, father";
                        break;
                }
            return sql;
        }

        public static string Get_short_fio_by_rank(int for_)
        {
            string sql = "";
            if (for_ > 0)
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select short_fio from dba.ent_employee where cod=" + (string)Convert.ToString(for_);
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select short_fio from dbo.emp_ent_employee where cod=" + (string)Convert.ToString(for_);
                        break;
                }
             return sql;
        }

        public static string Get_employee_of_movie(int for_)
        {
            string sql = "";
            if (for_ > 0)
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select employee.family + ' ' + employee.name + ' ' + employee.father as fio, employee.employee,dep_degrees.cod,jrd_degree.name as rank, jrd_degree.degree as decod, dep_degrees.department as department from dba.employee join dba.dep_degrees on employee.employee=dep_degrees.employee join dba.jrd_degree where dep_degrees.cod  =" + (string)Convert.ToString(for_);
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select emp_employee.family + ' ' + emp_employee.name + ' ' + emp_employee.father as fio, emp_employee.employee,emp_dep_degrees.cod,org_jrd_degree.name as rank, org_jrd_degree.degree as decod, emp_dep_degrees.department as department from dbo.emp_employee join dbo.emp_dep_degrees on emp_employee.employee=emp_dep_degrees.employee join dbo.org_jrd_degree on emp_dep_degrees.degree=org_jrd_degree.degree where emp_dep_degrees.cod  =" + (string)Convert.ToString(for_);
                        break;
                }
            return sql;
        }

        public static string Get_department_of_movie(int Department)
        {
             string sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql= "select jrd_department.department, jrd_department.name from dba.jrd_department where jrd_department.department  =" + (string)Convert.ToString(Department);
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select org_jrd_department.department, ORG_jrd_department.name from dbo.ORG_jrd_department where ORG_jrd_department.department  =" + (string)Convert.ToString(Department);
                        break;
                }
            return sql;
        }

        public static string Get_movies_of_doc(int Doc_id)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.moving_order_degree where document  =" + (string)Convert.ToString(Doc_id) + " order by moving asc";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_moving_order_degree where document  =" + (string)Convert.ToString(Doc_id) + " order by moving asc";
                    break;
            }
            return sql;
        }

        public static string Get_own_movies_of_doc(int Doc_id)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "call dba.main_exec_docs(@document=" + (string)Convert.ToString(Doc_id) + ")";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "call dbo.rkk_main_exec_docs(@document=" + (string)Convert.ToString(Doc_id) + ")";
                    break;
            }
            return sql;
        }

        public static string Get_Vizy_of_doc(int Doc_id)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.vizing where document=" + Convert.ToString(Doc_id) + " order by dat_change asc";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select rkk_vizing.*,rkk_vizing_denote.notes from dbo.rkk_vizing left join dbo.rkk_vizing_denote on rkk_vizing.id=rkk_vizing_denote.vizing where document=" + Convert.ToString(Doc_id) + " order by dat_change asc";
                    break;
            }
            return sql;
        }

        public static string Get_own_Vizy_of_doc(int Doc_id)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "call dba.main_vizing_docs(@document=" + Convert.ToString(Doc_id) + ")";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "call dbo.rkk_main_vizing_docs(@document=" + Convert.ToString(Doc_id) + ")";
                    break;
            }
            return sql;
        }

        public static string Get_news_of_doc(int Doc_id)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.news where document=" + Convert.ToString(Doc_id) + " order by news asc";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_news where document=" + Convert.ToString(Doc_id) + " order by news asc";
                    break;
            }
            return sql;
        }

        public static string Get_own_news_of_doc(int Doc_id)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "call dba.main_news_docs(@document=" + Convert.ToString(Doc_id) + ")";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "call dbo.rkk_main_news_docs(@document=" + Convert.ToString(Doc_id) + ")";
                    break;
            }
            return sql;
        }

        /*Каталоги документов*/

        public static int DocIndex = 1;
        public static int FiltrIndex = 1;
        public static int Sorting_Check = 1;
        public static string FiltrSql = start_FiltrSql;

        private static string start_FiltrSql
        { 
            get
            { 
                string sql= "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select * from dba.flow_document";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select * from dbo.rkk_flow_document";
                        break;
                }
                return sql;
            }
        }

        public static string Sorting_Order = "";
        public static string viz_FiltrSql = start_viz_FiltrSql;
        private static string start_viz_FiltrSql     
        {            
            get
            {
                string sql= "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select * from dba.flow_document where kod in (select document from dba.vizing)";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select * from dbo.rkk_flow_document where kod in (select document from dbo.rkk_vizing)";
                        break;
                }
                return sql;
            }
    
        }
        
        public static int sorting_index;
        public static bool Indoor_or_Out_docs;

        public static string Get_List_Docs(string FindAttach)
        {
            switch (DocIndex)
            {
                case DocEnumeration.WellcomeDocs.Value:
                    Indoor_or_Out_docs = true;
                    switch (FiltrIndex)
                    {
                        case (int)DocEnumeration.WellcomeDocs.Filters.executor_docs:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_docs " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_docs " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_new:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_docs_new " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_docs_new " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_send:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_docs_send " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_docs_send " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_send_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_docs_send_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_docs_send_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_docs_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_docs_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_send_partly_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_docs_send_partly_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_docs_send_partly_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_new_repeat:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_docs_new_repeat " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_docs_new_repeat " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.WellcomeDocs.Filters.executing_docs_alarm:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_docs_alarm " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_docs_alarm " + Between_dates();
                                    break;
                            }
                            break;
                    }
                    if (FindAttach.Trim().Length > 0)
                        FiltrSql += " and " + FindAttach;
                    else Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case DocEnumeration.IndoorDosc.Value:
                    Indoor_or_Out_docs = true;
                    switch (FiltrIndex)
                    {
                        case (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.exe_in_docs" + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_exe_in_docs " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_new:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.exe_in_docs_new " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_exe_in_docs_new " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_send:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.exe_in_docs_send" + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_exe_in_docs_send " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_send_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.exe_in_docs_send_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_exe_in_docs_send_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.exe_in_docs_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_exe_in_docs_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_send_partly_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.exe_in_docs_send_partly_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_exe_in_docs_send_partly_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_new_repeat:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.exe_in_docs_new_repeat " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_exe_in_docs_new_repeat " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.IndoorDosc.Filters.executing_in_docs_alarm:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.exe_in_docs_alarm " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_exe_in_docs_alarm " + Between_dates();
                                    break;
                            }
                            break;
                    }
                    if (FindAttach.Trim().Length > 0)
                        FiltrSql += " and " + FindAttach;
                    else Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;
                case DocEnumeration.OutDocs.Value:
                    Indoor_or_Out_docs = false;
                    switch (FiltrIndex)
                    {
                        case (int)DocEnumeration.OutDocs.Filters.out_docs_new:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.out_docs_new " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.out_docs_new " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.OutDocs.Filters.out_docs_send:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.out_docs_send " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.out_docs_send " + Between_dates();
                                    break;
                            }
                            break;
                    }
                    if (FiltrIndex != (int)DocEnumeration.OutDocs.Filters.out_docs_new && FiltrIndex != (int)DocEnumeration.OutDocs.Filters.out_docs_send)
                    {
                        FiltrIndex = (int)DocEnumeration.OutDocs.Filters.out_docs_new;
                        FiltrSql = "dba.out_docs_new" + Between_dates();
                    }
                    if (FindAttach.Trim().Length > 0)
                        FiltrSql += " and " + FindAttach;
                    else Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year ," + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;

                    break;

                case DocEnumeration.DepartmentDocs.Value:
                    Indoor_or_Out_docs = true;
                    if (FindAttach.Trim().Length > 0)
                        FiltrSql += " and " + FindAttach;
                    else Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    FiltrSql = "dba.leader_docs" + Between_dates();
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case DocEnumeration.OwnDocs.Value:
                    Indoor_or_Out_docs = true;
                    switch (FiltrIndex)
                    {
                        case (int)DocEnumeration.OwnDocs.Filters.own_docs:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.own_docs " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_own_docs " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.OwnDocs.Filters.own_docs_new:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.own_docs_new " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_own_docs_new " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.OwnDocs.Filters.own_docs_send:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.own_docs_send " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_own_docs_send " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.OwnDocs.Filters.own_docs_send_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.own_docs_send_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_own_docs_send_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.OwnDocs.Filters.own_docs_signing:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.own_docs_signing " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_own_docs_signing " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.OwnDocs.Filters.own_docs_signed:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.own_docs_signed " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_own_docs_signed " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.OwnDocs.Filters.own_docs_not_signed:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.own_docs_not_signed " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_own_docs_not_signed " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.OwnDocs.Filters.own_docs_alarm:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.own_docs_alarm " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_own_docs_alarm " + Between_dates();
                                    break;
                            }
                            break;
                    }
                    if (FindAttach.Trim().Length > 0)
                        FiltrSql += " and " + FindAttach;
                    else Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case DocEnumeration.NewsDocs.Value:
                    Indoor_or_Out_docs = true;
                    FiltrSql = "dba.executor_news" + Between_dates();

                    switch (FiltrIndex)
                    {
                        case (int)DocEnumeration.NewsDocs.Filters.executor_news:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_news " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_news " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.NewsDocs.Filters.executor_news_new:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_news_new " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_news_new " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.NewsDocs.Filters.executor_news_old:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_news_old " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_news_old " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.NewsDocs.Filters.executor_news_alarm:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.executor_news_alarm " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_executor_news_alarm " + Between_dates();
                                    break;
                            }
                            break;
                    }
                    if (FindAttach.Trim().Length > 0)
                        FiltrSql += " and " + FindAttach;
                    else Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case DocEnumeration.VizingDocs.Value:
                    switch (FiltrIndex)
                    {
                        case (int)DocEnumeration.VizingDocs.Filters.vizing_docs:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.vizing_docs " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_vizing_docs " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.VizingDocs.Filters.vizing_docs_new:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.vizing_docs_new " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_vizing_docs_new " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.VizingDocs.Filters.vizing_docs_send:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.vizing_docs_send " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_vizing_docs_send " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.VizingDocs.Filters.vizing_docs_send_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.vizing_docs_send_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_vizing_docs_send_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.VizingDocs.Filters.vizing_docs_exec:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.vizing_docs_exec " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_vizing_docs_exec " + Between_dates();
                                    break;
                            }
                            break;
                        case (int)DocEnumeration.VizingDocs.Filters.vizing_docs_alarm:
                            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                            {
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                                    FiltrSql = "dba.vizing_docs_alarm " + Between_dates();
                                    break;
                                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                                    FiltrSql = "dbo.rkk_vizing_docs_alarm " + Between_dates();
                                    break;
                            }
                            break;
                    }
                    if (FindAttach.Trim().Length > 0)
                        FiltrSql += " and " + FindAttach;
                    else Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case DocEnumeration.ArchiveDocs.Value:
                    Indoor_or_Out_docs = true;
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            FiltrSql = "dba.archive_of_employee ";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            FiltrSql = "dbo.rkk_archive_of_employee ";
                            break;
                    }
                    if (FindAttach.Trim().Length > 0)
                        FiltrSql += " and " + FindAttach;
                    else Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;
            }
            return Tree_SQL;
        }
                
        public static string Get_List_WellDocs()
        {
            switch (DocIndex)
            {
                case CommonValues.DocWellEnumeration.AllDocs.Value:
                    Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    FiltrSql = "dbo.rkk_documents " + Between_dates(); 
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

            
                case CommonValues.DocWellEnumeration.SendDosc.Value:
                    Tree_SQL_query = " and kod in (select document from dbo.rkk_moving)";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    FiltrSql = "dbo.rkk_documents " + Between_dates(); 
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case CommonValues.DocWellEnumeration.NewDosc.Value:
                    Tree_SQL_query = " and kod not in(select document from dbo.rkk_moving)";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    FiltrSql = "dbo.rkk_documents " + Between_dates(); 
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year ," + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case CommonValues.DocWellEnumeration.ArchiveDocs.Value:
                    Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    FiltrSql = "dbo.rkk_archive " + Between_dates();
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case CommonValues.DocWellEnumeration.ExecDocs.Value:
                    Tree_SQL_query = " and kod in (select document from dbo.rkk_moving where exe_doc is not null and pattern = dbo.user_ident())";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by kod desc";
                    FiltrSql = "dbo.rkk_documents " + Between_dates();
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

                case CommonValues.DocWellEnumeration.PrintedDocs.Value:
                    Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_o";
                    Tree_SQL_order = " order by date_o desc";
                    FiltrSql = "dbo.rkk_ready_documents " + Between_dates();
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;

            
                case CommonValues.DocWellEnumeration.OutDocs.Value:
                    Tree_SQL_query = " ";
                    Tree_SQL_Cast_date = "date_f";
                    Tree_SQL_order = " order by date_o desc";
                    FiltrSql = "dbo.out_finish_documents " + Between_dates();
                    Tree_SQL = "select * from " + FiltrSql;
                    Tree_SQL_count = "select count(*) as cnt from " + FiltrSql;
                    Tree_SQL_year = "select distinct datepart( year , " + Tree_SQL_Cast_date + " ) as Year from " + FiltrSql;
                    Tree_SQL_month = "select distinct datepart( month , " + Tree_SQL_Cast_date + " ) as month from " + FiltrSql;
                    Tree_SQL_day = "select distinct datepart( day , " + Tree_SQL_Cast_date + " ) as day from " + FiltrSql;
                    break;
            }
            return Tree_SQL;
        }
            
        public static int Tree_RKKCnt=100;
        public static string Tree_SQL;
        public static string Tree_SQL_count;
        public static string Tree_SQL_query;
        public static string Tree_SQL_order;
        public static string Tree_SQL_year;
        public static string Tree_SQL_month;
        public static string Tree_SQL_day;
        public static string Tree_SQL_Cast_date = "SelDate";// "date_f";
        public static int Doc_Filtre = 0;


        public static string workflow_catalog()
        {
            switch (Doc_Filtre)
            {
                case 0:
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL = "select * from dba.documents ";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL = "select * from dbo.rkk_documents ";
                            break;
                    }
                    Tree_SQL_query = " ";
                    Tree_SQL_order = " order by kod desc";
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_count = "select count(*) as cnt from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_count = "select count(*) as cnt from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_year = "select distinct datepart( year , documents.date_f ) as Year from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_year = "select distinct datepart( year , documents.date_f ) as Year from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_month = "select distinct datepart( month , documents.date_f ) as month from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_month = "select distinct datepart( month , documents.date_f ) as month from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_day = "select distinct datepart( day , documents.date_f ) as day from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_day = "select distinct datepart( day , documents.date_f ) as day from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_Cast_date = "documents.date_f";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_Cast_date = "rkk_documents.date_f";
                            break;
                    }
                    break;
                    
                case 1:
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL = "select documents.* from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL = "select rkk_documents.* from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_query = " where kod in (select document from dba.moving)";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_query = " where kod in (select document from dbo.rkk_moving)";
                            break;
                    }
                    Tree_SQL_order = "  order by kod desc";
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_count = "select count(*) as cnt from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_count = "select count(*) as cnt from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_year = "select distinct datepart( year , documents.date_f ) as Year from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_year = "select distinct datepart( year , documents.date_f ) as Year from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_month = "select distinct datepart( month , documents.date_f ) as month from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_month = "select distinct datepart( month , documents.date_f ) as month from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_day = "select distinct datepart( day , documents.date_f ) as day from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_day = "select distinct datepart( day , documents.date_f ) as day from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_Cast_date = "documents.date_f";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_Cast_date = "rkk_documents.date_f";
                            break;
                    }
                    break;
                    
                case 2:
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL = "select * from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL = "select * from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_query = " where kod not in(select document from dba.moving)";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_query = " where kod not in(select document from dbo.rkk_moving)";
                            break;
                    }
                    Tree_SQL_order = " order by kod desc";
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_count = "select count(*) as cnt from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_count = "select count(*) as cnt from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_year = "select distinct datepart( year , documents.date_f ) as Year from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_year = "select distinct datepart( year , documents.date_f ) as Year from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_month = "select distinct datepart( month , documents.date_f ) as month from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_month = "select distinct datepart( month , documents.date_f ) as month from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_day = "select distinct datepart( day , documents.date_f ) as day from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_day = "select distinct datepart( day , documents.date_f ) as day from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_Cast_date = "documents.date_f";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_Cast_date = "rkk_documents.date_f";
                            break;
                    }
                    break;
                    
                case 3:
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL = "select * from dba.archive";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL = "select * from dbo.rkk_archive";
                            break;
                    }
                    Tree_SQL_query = " ";
                    Tree_SQL_order = " order by kod desc";
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_count = "select count(*) as cnt from dba.archive";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_count = "select count(*) as cnt from dbo.rkk_archive";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_year = "select distinct datepart( year , date_f ) as Year from dba.archive";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_year = "select distinct datepart( year , date_f ) as Year from dbo.rkk_archive";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_month = "select distinct datepart( month , date_f ) as month from dba.archive";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_month = "select distinct datepart( month , date_f ) as month from dbo.rkk_archive";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_day = "select distinct datepart( day , date_f ) as day from dba.archive";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_day = "select distinct datepart( day , date_f ) as day from dbo.rkk_archive";
                            break;
                    }
                    Tree_SQL_Cast_date = "date_f";
                    break;
                    
                case 4:
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL = "select * from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL = "select * from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_query = " where kod in (select document from dba.moving join dba.employees on pattern=employee  where exe_doc is not null and indoor_name like current user)";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_query = " where kod in (select document from dbo.rkk_moving where exe_doc is not null and pattern =dbo.user_ident())";
                            break;
                    }
                    Tree_SQL_order = " order by kod desc";
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_count = "select count(*) as cnt from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_count = "select count(*) as cnt from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_year = "select distinct datepart( year , documents.date_f ) as Year from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_year = "select distinct datepart( year , documents.date_f ) as Year from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_month = "select distinct datepart( month , documents.date_f ) as month from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_month = "select distinct datepart( month , documents.date_f ) as month from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_day = "select distinct datepart( day , documents.date_f ) as day from dba.documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_day = "select distinct datepart( day , documents.date_f ) as day from dbo.rkk_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_Cast_date = "documents.date_f";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_Cast_date = "rkk_documents.date_f";
                            break;
                    }
                    break;
                    
                case 5:
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL = "select * from dba.ready_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL = "select * from dbo.rkk_ready_documents";
                            break;
                    }
                    Tree_SQL_query = " ";
                    Tree_SQL_order = " order by date_o desc";
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_count = "select count(*) as cnt from dba.ready_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_count = "select count(*) as cnt from dbo.rkk_ready_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_year = "select distinct datepart( year , ready_documents.date_o ) as Year from dba.ready_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_year = "select distinct datepart( year , rkk_ready_documents.date_o ) as Year from dbo.rkk_ready_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_month = "select distinct datepart( month , ready_documents.date_o ) as month from dba.ready_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_month = "select distinct datepart( month , rkk_ready_documents.date_o ) as month from dbo.rkk_ready_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_day = "select distinct datepart( day , ready_documents.date_o ) as day from dba.ready_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_day = "select distinct datepart( day , rkk_ready_documents.date_o ) as day from dbo.rkk_ready_documents";
                            break;
                    }
                    Tree_SQL_Cast_date = "date_o";
                    break;
                    
                case 6:
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL = "select * from dba.finish_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL = "select * from dbo.out_finish_documents";
                            break;
                    }
                    Tree_SQL_query = " ";
                    Tree_SQL_order = " order by date_o desc";
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_count = "select count(*) as cnt from dba.finish_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_count = "select count(*) as cnt from dbo.out_finish_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_year = "select distinct datepart( year , finish_documents.date_f ) as Year from dba.finish_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_year = "select distinct datepart( year , out_finish_documents.date_f ) as Year from dbo.out_finish_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_month = "select distinct datepart( month , finish_documents.date_f ) as month from dba.finish_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_month = "select  distinct datepart( month , out_finish_documents.date_f ) as month from dbo.out_finish_documents";
                            break;
                    }
                    switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                    {
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                            Tree_SQL_day = "select distinct datepart( day , finish_documents.date_f ) as day from dba.finish_documents";
                            break;
                        case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                            Tree_SQL_day = "select distinct datepart( day , out_finish_documents.date_f ) as day from dbo.out_finish_documents";
                            break;
                    }
                    Tree_SQL_Cast_date = "date_f";
                    break;
            }
            return Tree_SQL + Tree_SQL_query + Tree_SQL_order;
        }

        public static string Fill_Tree(int day, int month , int  year )
        {
            string sql_add_Q=""; 
            

            if (day > 0)
                sql_add_Q = Where_OR_And(Tree_SQL + Tree_SQL_query + sql_add_Q) + "datepart( day ," + Tree_SQL_Cast_date + ") =" + (string)Convert.ToString(day);
            
            if ( month > 0)
                sql_add_Q += Where_OR_And(Tree_SQL + Tree_SQL_query + sql_add_Q) + "datepart( month ," + Tree_SQL_Cast_date + ") =" + (string)Convert.ToString(month);
            
            if( year > 0 )
                sql_add_Q += Where_OR_And(Tree_SQL + Tree_SQL_query + sql_add_Q) + "datepart( year ," + Tree_SQL_Cast_date + ") =" + (string)Convert.ToString(year);
            sql_add_Q =Tree_SQL + Tree_SQL_query + sql_add_Q + Tree_SQL_order;
            return sql_add_Q;
        }

        private static DateTime From_Date = DateTime.MinValue;
        private static DateTime To_Date = System.DateTime.Today.Date.AddDays(1);

        public static void DocsListofPeriod(DateTime FROM_date, DateTime TO_date)
        {
            From_Date = FROM_date;
            To_Date = TO_date;
        }

        public static string SubDocuments(int DocID)
        {
            return "select * from dbo.RKK_flow_document where parent_Document=" + DocID.ToString();
        }

        public static AMAS_DBI.Class_syb_acc.PrepareParameters[] PrepareDate()
        {
            AMAS_DBI.Class_syb_acc.PrepareParameters[] pD= new Class_syb_acc.PrepareParameters[2];
            try
            {
                pD[0] = new AMAS_DBI.Class_syb_acc.PrepareParameters("@from", SqlDbType.DateTime, From_Date);
            }
            catch
            {
                pD[0] = null;
            }
            try
            {
                pD[1] = new AMAS_DBI.Class_syb_acc.PrepareParameters("@to", SqlDbType.DateTime, To_Date);
            }
            catch
            {
                pD[1] = null;
            }
            return pD;
        }

        private static string Between_dates()
        {
            //return " where "+ Tree_SQL_Cast_date+ " between cast('" + (string)From_Date.ToString("yyyy.MM.dd") + "' as datetime) and cast('" + (string)To_Date.ToString("yyyy.MM.dd") + "' as datetime)";
            return " where "+ Tree_SQL_Cast_date+ " between @From and @To ";
        }

        public static string Where_OR_And(string sql)
        {
            if (sql.ToLower().Contains(" where ")) return " and "; else return " where " ;
        }

        // Структура организации

        public static string EnterpriceDeps()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select name, department, under from dba.department order by under asc, order_sub desc";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select name, department, under from dbo.org_department order by under asc, order_sub desc";
                    break;
            }
            return sql;
        }

        public static string EnterpriceSubDeps()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.our_employees order by department";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.org_our_employees order by department";
                    break;
            }
            return sql;
        }

        public static string EnterpriceUnderDepts()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select department from dba.my_department";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select department from dbo.emp_my_department";
                    break;
            }
            return sql;
        }

        public static string Degrees_in_Dep(long dep)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
            sql = "select  degree.name,dep_degrees.leader,dep_degrees.cod,'?' as fio from dba.degree,dba.dep_degrees  where dep_degrees.employee is Null and degree.degree= dep_degrees.degree and dep_degrees.department =" + dep.ToString();
            sql += " union ";
            sql += "select  degree.name,dep_degrees.leader,dep_degrees.cod,employee.family + ' ' + employee.name + ' ' + employee.father  as fio from dba.degree join dba.dep_degrees, dba.employee  where employee.employee= dep_degrees.employee and dep_degrees.department =" + dep.ToString();
            break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select  rtrim(org_degree.name) as name,emp_dep_degrees.leader,emp_dep_degrees.cod,'?' as fio, emp_dep_degrees.employee from dbo.org_degree,dbo.emp_dep_degrees  where emp_dep_degrees.employee is Null and org_degree.degree= emp_dep_degrees.degree and emp_dep_degrees.deleted is null and emp_dep_degrees.department =" + dep.ToString();
            sql += " union ";
            sql += "select  rtrim(org_degree.name) as name,emp_dep_degrees.leader,emp_dep_degrees.cod,rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.name) + ' ' + rtrim(emp_employee.father) as fio, emp_dep_degrees.employee from dbo.org_degree join dbo.emp_dep_degrees on org_degree.degree= emp_dep_degrees.degree, dbo.emp_employee  where emp_employee.employee= emp_dep_degrees.employee and emp_dep_degrees.deleted is null and emp_dep_degrees.department =" + dep.ToString();
            break;
            }
            return sql;
        }

        public static string Structure_Is_Moving(long document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select count(moving) as cnt from dba.moving,dba.employees where moving.document=" + (string)Convert.ToString(document) + " and moving.pattern=employees.employee and indoor_name like current user and main_executor=1";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select count(moving) as cnt from dbo.rkk_moving where rkk_moving.document=" + (string)Convert.ToString(document) + " and rkk_moving.pattern=dbo.user_ident() and main_executor=1";
                    break;
            }
            return sql;
        }

        public static string Structure_employee(string Emp)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.our_employees where cod=" + Emp;
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.org_our_employees where cod=" + Emp;
                    break;
            }
            return sql;
        }

        public static string Structure_signatures()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.wfl_sign";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_wfl_sign order by sign asc";
                    break;
            }
            return sql;
        }

        public static string Groups_listing()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.groups order by denote";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.org_groups order by denote";
                    break;
            }
            return sql;
        }

        public static string My_Groups_listing()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.groups order by denote";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select org_groups.* from dbo.org_groups join dbo.org_workgroup on org_groups.id=org_workgroup.group_w join dbo.emp_dep_degrees on emp_dep_degrees.cod=org_workgroup.rank where emp_dep_degrees.employee=dbo.user_ident() order by denote";
                    break;
            }
            return sql;
        }

        public static string Group_Employee(int grp)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
    
                    sql = "select  degree.name,dep_degrees.leader,dep_degrees.cod,'?' as fio from dba.degree join dba.dep_degrees join dba.workgroup where dep_degrees.employee is Null and workgroup.group_w =" +grp.ToString();
                    sql +=" union ";
                    sql += "select  degree.name,dep_degrees.leader,dep_degrees.cod,employee.family + ' ' + employee.name + ' ' + employee.father  as fio from dba.degree join dba.dep_degrees join dba.workgroup, dba.employee  where employee.employee= dep_degrees.employee  and workgroup.group_w =" + grp.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select  org_workgroup.id,rtrim(org_degree.name)as naming,emp_dep_degrees.leader,emp_dep_degrees.cod,'?' as fio from dbo.org_degree join dbo.emp_dep_degrees on org_degree.degree=emp_dep_degrees.degree join dbo.org_workgroup on emp_dep_degrees.cod=org_workgroup.rank where emp_dep_degrees.employee is null and org_workgroup.group_w =" + grp.ToString();
                    sql += " union ";
                    sql += "select  org_workgroup.id,rtrim(org_degree.name) as naming,emp_dep_degrees.leader,emp_dep_degrees.cod, rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father)  as fio from dbo.org_degree join dbo.emp_dep_degrees on org_degree.degree=emp_dep_degrees.degree join dbo.org_workgroup on emp_dep_degrees.cod=org_workgroup.rank , dbo.emp_employee  where emp_employee.employee= emp_dep_degrees.employee  and org_workgroup.group_w =" + grp.ToString();
                    break;
            }
            return sql;
        }

        public static string Get_Employee_of_Group(int IdWgp)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select  degree.name,dep_degrees.leader,dep_degrees.cod,'?' as fio from dba.degree join dba.dep_degrees join dba.workgroup where dep_degrees.employee is Null and workgroup.id =" + IdWgp.ToString();
                    sql += " union ";
                    sql += "select  degree.name,dep_degrees.leader,dep_degrees.cod,employee.family + ' ' + employee.name + ' ' + employee.father  as fio from dba.degree join dba.dep_degrees join dba.workgroup, dba.employee  where employee.employee= dep_degrees.employee  and workgroup.id=" + IdWgp.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select  org_workgroup.id,rtrim(org_degree.name)as naming,emp_dep_degrees.leader,emp_dep_degrees.cod,'?' as fio from dbo.org_degree join dbo.emp_dep_degrees on org_degree.degree=emp_dep_degrees.degree join dbo.org_workgroup on emp_dep_degrees.cod=org_workgroup.rank where emp_dep_degrees.employee is null and org_workgroup.id =" + IdWgp.ToString();
                    sql += " union ";
                    sql += "select  org_workgroup.id,rtrim(org_degree.name) as naming,emp_dep_degrees.leader,emp_dep_degrees.cod, rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father)  as fio from dbo.org_degree join dbo.emp_dep_degrees on org_degree.degree=emp_dep_degrees.degree join dbo.org_workgroup on emp_dep_degrees.cod=org_workgroup.rank , dbo.emp_employee  where emp_employee.employee= emp_dep_degrees.employee  and org_workgroup.id=" + IdWgp.ToString();
                    break;
            }
            return sql;
        }

        public static string List_of_dep_instructions(int dep)
        {
            {
                string sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select * from dba.dep_instructions";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select * from dbo.ORG_department_instructions where department=" + dep.ToString();
                        break;
                }
                return sql;
            }
        }

        public static string List_of_rank_instructions(int rank)
        {
            {
                string sql = "";
                switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
                {
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                        sql = "select * from dba.rank_instructions";
                        break;
                    case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                        sql = "select * from dbo.ORG_rank_instructions where rank=" + rank.ToString();
                        break;
                }
                return sql;
            }
        }

        // Содержимое документа

        public static string Documentcontent(int document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.stuff where document=" + document.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_stuff where document=" + document.ToString();
                    break;
            }
            return sql;
        }

        public static string DocumentCorrect(int document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.stuff where document=" + document.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_document_correct where document=" + document.ToString();
                    break;
            }
            return sql;
        }

        public static string DocumentDenote(int document)
        {
            return "Select * from dbo.RKK_denote_of_document where [document]=" + document.ToString();
        }

        //Сведения о документе

        public static string MetadataCommon(int document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.flow_document where kod=" + document.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_flow_document where kod=" + document.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataWellcome(int document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.wellcome_document where kod=" + document.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_wellcome_document where kod=" + document.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataIndoor(int document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.Indoor_document where kod=" + document.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_Indoor_document where kod=" + document.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataOutdoc(int document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.out_document where kod=" + document.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.out_document where kod=" + document.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadatArchive(int document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.archive_document where kod=" + document.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_archive_document where kod=" + document.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadatExecutors(int document, bool Archive, bool Owntasks)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    if (Archive)
                    {
                        if (!Owntasks)
                        {
                            sql = "select * from dba.Heap_moving_order_degree where document  =" + document.ToString() + " order by moving asc";
                        }
                        else
                        {
                            sql = "call dba.main_arch_exec_docs(@document=" + document.ToString() + ")";
                        }
                    }
                    else
                    {
                        if (!Owntasks)
                        {
                            sql = "select * from dba.moving_order_degree where document  =" + document.ToString() + " order by moving asc";
                        }
                        else
                        {
                            sql = "call dba.main_exec_docs(@document=" + document + ")";
                        }
                    }
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    if (Archive)
                    {
                        if (!Owntasks)
                        {
                            sql = "select * from dbo.RKK_Heap_moving_order_degree where document  =" + document.ToString() + " order by moving asc";
                        }
                        else
                        {
                            sql = "call dbo.RKK_main_arch_exec_docs(@document=" + document.ToString() + ")";
                        }
                    }
                    else
                    {
                        if (!Owntasks)
                        {
                            sql = "select * from dbo.RKK_moving_order_degree where document  =" + document.ToString() + " order by moving asc";
                        }
                        else
                        {
                            sql = "call dbo.RKK_main_exec_docs(@document=" + document + ")";
                        }
                    }
                    break;
            }
            return sql;
        }

        public static string MetadatAutor(int autor)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.autor where id  =" + autor.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_autor where id  =" + autor.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataOrg(int enterprice)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "Select *from dba.jrd_juridic where id  =" + enterprice.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "Select * from dbo.ORG_jrd_juridic where id  =" + enterprice.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataLeader(int employee)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.leaders where cod  =" + employee.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.EMP_leaders where cod  =" + employee.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataEmployee(int employee)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.employee where employee  =" + employee.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.EMP_employee where employee  =" + employee.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataKind(int kind)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.kind where kod=" + kind.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_kind where kod=" + kind.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataTema(int Tema)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.Tema where Tema=" + Tema.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_Tema where Tema=" + Tema.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataComing(int Coming)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.Coming where cod=" + Coming.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.RKK_Coming where cod=" + Coming.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataSender(int Sender)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.employee where employee  =" + Sender.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.emp_employee where employee  =" + Sender.ToString();
                    break;
            }
            return sql;
        }

        public static string MetadataFormular(int Document)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select flow_formular.*,ent_employee.fio from dba.flow_formular join dba.ent_employee on flow_formular.who_have=ent_employee.id where kod=" + Document.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select distinct RKK_flow_formular.*,EMP_ent_employee.fio from dbo.RKK_flow_formular join dbo.EMP_ent_employee on RKK_flow_formular.who_have=EMP_ent_employee.id where kod=" + Document.ToString();
                    break;
            }
            return sql;
        }

        //Workflow

        public static string Wflow_comings()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select * from dba.comings order by coming";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.emp_comings order by coming";
                    break;
            }
            return sql;
        }

        public static string Wflow_kinds()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select * from dba.kinds order by kind";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.emp_kinds order by kind";
                    break;
            }
            return sql;
        }

        public static string Wflow_temy(int kind)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select tema.description_ ,tema.tema from dba.tema join dba.tema_for_kind where kind=" + kind.ToString() + " order by description_";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select emp_temy.description_,emp_temy.tema from dbo.emp_temy join dbo.rkk_tema_for_kind on emp_temy.tema=rkk_tema_for_kind.tema where rkk_tema_for_kind.kind=" + kind.ToString() + " order by description_";
                    break;
            }
            return sql;
        }

        public static string Wflow_Clue()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select flow_document.* from dba.flow_document join dba.out_document on flow_document.kod=out_document.kod order by find_cod";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select RKK_flow_document.kod as id, RKK_flow_document.find_cod as name from dbo.RKK_flow_document join dbo.out_document on RKK_flow_document.kod=out_document.kod order by find_cod";
                    break;
            }
            return sql;
        }

        public static string Wflow_Folder()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select id, folder as name from dbo.RKK_folder order by folder";
                    break;
            }
            return sql;
        }

        public static string Wflow_attached_organizations()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.selected_org order by full_name";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_registred_org order by full_name";
                    break;
            }
            return sql;
        }

        public static string Wflow_organization(int id)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.selected_org where kod="+id.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_registred_org where kod=" + id.ToString();
                    break;
            }
            return sql;
        }
        
        public static string Wflow_attached_degrees(int ORG)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.jrd_degree where juridic=" +ORG.ToString()+ " order by name";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select degree as id, [name] from dbo.ORG_jrd_degree where juridic=" + ORG.ToString() + " order by name";
                    break;
            }
            return sql;
        }

        public static string Wflow_attached_employees(int Degree)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.leaders where degree=" +Degree.ToString()+ " order by fio";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.emp_leaders where degree=" + Degree.ToString() + " order by fio";
                    break;
            }
            return sql;
        }

        public static string Wflow_attached_Autors()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.selected_autors order by fio";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_registred_autors order by fio";
                    break;
            }
            return sql;
        }

        public static string Wflow_Autor(int id)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:
                    sql = "select * from dba.selected_autors where kod=" + id.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_registred_autors  where kod="+id.ToString();
                    break;
            }
            return sql;
        }

        //Registrator

        public static string Registrator_selected_autors()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select * from dba.selected_autors order by fio";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_selected_autors order by fio";
                    break;
            }
            return sql;
        }

        public static string Registrator_selected_Orgs()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select * from dba.selected_org order by full_name";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select * from dbo.rkk_selected_org order by full_name";
                    break;
            }
            return sql;
        }

        //personel

        public static string Get_Personel_reserve()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select employee.family + ' ' + employee.name + ' ' + employee.father as fio, employee.EMPLOYEE from dba.employee  where not exists (select employee from dba.dep_degrees where dep_degrees.deleted is null and dep_degrees.employee=employee.employee group by employee) order by fio";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.name) + ' ' + rtrim(emp_employee.father) as fio, emp_employee.EMPLOYEE from dbo.emp_employee  where not exists (select employee from dbo.emp_dep_degrees where emp_dep_degrees.deleted is null and emp_dep_degrees.employee=emp_employee.employee group by employee) order by fio asc";
                    break;
            }
            return sql;
        }
        
        public static string Get_Personel_ALL()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select family + ' ' + employee.name + ' ' + employee.father  as fio, EMPLOYEE from dba.employee  order by fio";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father) as fio, emp_employee.EMPLOYEE, emp_employee.cod  from dbo.emp_employee where emp_employee.EMPLOYEE in (select employee from dbo.emp_dep_degrees where deleted is null ) order by fio";
                    //sql = "select rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father) as fio, emp_employee.EMPLOYEE , emp_employees.indoor_name from dbo.emp_employee join dbo.emp_employees on emp_employee.employee=emp_employees.employee ";
                    //sql += " union ";
                    //sql += "select rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father) as fio, emp_employee.EMPLOYEE , '' as indoor_name from dbo.emp_employee where employee not in (select employee from dbo.emp_employees) ";
                    break;
            }
            return sql;
        }

        public static string Get_Personel_I_Degree()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select family + ' ' + employee.name + ' ' + employee.father  as fio, EMPLOYEE from dba.employee  order by fio";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select rtrim(EMP_personal.family) + ' ' + rtrim(EMP_personal.[name]) + ' ' + rtrim(EMP_personal.father) as fio, EMP_personal.EMPLOYEE, EMP_personal.cod  from dbo.EMP_personal where EMP_personal.EMPLOYEE in (select employee from dbo.emp_dep_degrees where deleted is null) order by fio";
                    //sql = "select rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father) as fio, emp_employee.EMPLOYEE , emp_employees.indoor_name from dbo.emp_employee join dbo.emp_employees on emp_employee.employee=emp_employees.employee ";
                    //sql += " union ";
                    //sql += "select rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father) as fio, emp_employee.EMPLOYEE , '' as indoor_name from dbo.emp_employee where employee not in (select employee from dbo.emp_employees) ";
                    break;
            }
            return sql;
        }
        public static string Get_Personel_Rights()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select family + ' ' + employee.name + ' ' + employee.father  as fio, EMPLOYEE from dba.employee  order by fio";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father) as fio, emp_employee.EMPLOYEE , emp_employees.indoor_name from dbo.emp_employee join dbo.emp_employees on emp_employee.employee=emp_employees.employee ";
                    sql += " union ";
                    sql += "select rtrim(emp_employee.family) + ' ' + rtrim(emp_employee.[name]) + ' ' + rtrim(emp_employee.father) as fio, emp_employee.EMPLOYEE , '' as indoor_name from dbo.emp_employee where employee not in (select employee from dbo.emp_employees) ";
                    break;
            }
            return sql;
        }

        public static string DocumentFormular(int docid)
        {
            return "select employee,document, dat_change as dateGet from dbo.RKK_FormularsMovie where kod=" + docid.ToString();
        }

        public static string My_Roles_list()
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select rolename, roledescription, id, uzed from dbo.Sec_RolesBoth order by RoleDescription asc";
                    break;
            }
            return sql;
        }

        public static string Roles_list(string login)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "";
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select all rolename, roledescription, id, uzed  from dbo.Sec_RolesAllboth where logamas like '" + login.Trim() + "' order by roledescription asc";
                    break;
            }
            return sql;
        }

        public static string Get_Ranks_of_empl(long cod)
        {
            string sql = "";
            switch ((AMAS_DBI.Class_syb_acc.AMAS_connections)AMAS_DBI.Class_syb_acc.AMAS_Base)
            {
                case AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase:

                    sql = "select department.name as department,dep_degrees.employee,degree.name,dep_degrees.cod from dba.department,dba.degree,dba.dep_degrees where dep_degrees.deleted is null and degree.degree=dep_degrees.degree and dep_degrees.department=department.department and dep_degrees.cod=" + cod.ToString();
                    break;
                case AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL:
                    sql = "select emp_dep_degrees.employee,org_degree.name,emp_dep_degrees.cod from dbo.org_degree join dbo.emp_dep_degrees on org_degree.degree=emp_dep_degrees.degree where emp_dep_degrees.deleted is null and emp_dep_degrees.employee=" + cod.ToString();
                    break;
            }
            return sql;
        }

        //

        public static string RKK_executor_docs_alarm()
        {
            return "select * from dbo.RKK_executor_docs_alarm";
        }

        public static string RKK_exe_in_docs_alarm()
        {
            return "select * from dbo.RKK_exe_in_docs_alarm";
        }

        public static string RKK_vizing_docs_alarm()
        {
            return "select * from dbo.RKK_vizing_docs_alarm";
        }

        public static string rkk_executor_docs3days()
        {
            return "select * from dbo.rkk_executor_docs where when_m<dateadd( d, 3, getdate()) and kod not in (select kod from dbo.RKK_executor_docs_alarm) and executed is null";
        }

        public static string rkk_exe_in_docs3days()
        {
            return "select * from dbo.rkk_exe_in_docs where when_m<dateadd( d, 3, getdate()) and kod not in (select kod from dbo.RKK_exe_in_docs_alarm) and executed is null";
        }

        public static string rkk_vizing_docs3days()
        {
            return "select * from dbo.rkk_vizing_docs where when_v<dateadd( d, 3, getdate()) and kod not in (select kod from dbo.RKK_vizing_docs_alarm) and executed is null";
        }

    }

    public class SEEK
    {
        public int val;
        public string desc;
        public string sql;

        public SEEK()
        {
            val = 0;
            desc = "";
            sql = "";
        }
    }

        
    public  class DOCUM
        {
            public SEEK[] SEEK_doc;
            public int count_seek=0;

            public int val;
            public string desc;

            public DOCUM(int sd)
            {
                count_seek = sd;
                SEEK_doc = new SEEK[count_seek];
                for (int i = 0; i < count_seek; i++) { SEEK_doc[i] = new SEEK(); }

            }

            public void fill_sd(int sd, int val, string desc)
            {
                if (sd >= 0 && sd < count_seek)
                {
                    SEEK_doc[sd].val = val;
                    SEEK_doc[sd].desc = desc;
                    SEEK_doc[sd].sql = "";
                }
            }
        }

    public class Workflowdoc
    {
        public DOCUM[] DocSeek;
        public readonly int DS_cnt = 0;
        public Workflowdoc(int count)
        {
            WellWork.Well = true;
            DS_cnt = count + 7;
            DocSeek = new DOCUM[DS_cnt];
            DocSeek[0] = new DOCUM(1);
            DocSeek[0].fill_sd(0, 1, "Все документы");
            DocSeek[0].val = CommonValues.DocWellEnumeration.AllDocs.Value;
            DocSeek[0].desc="Входящая корреспонденция";

            DocSeek[1] = new DOCUM(1);
            DocSeek[1].fill_sd(0, 1, "Все документы");
            DocSeek[1].val = CommonValues.DocWellEnumeration.SendDosc.Value;
            DocSeek[1].desc = "Назначенные документы";

            DocSeek[2] = new DOCUM(1);
            DocSeek[2].fill_sd(0, 1, "Все документы");
            DocSeek[2].val = CommonValues.DocWellEnumeration.NewDosc.Value;
            DocSeek[2].desc = "Новые документы";

            DocSeek[3] = new DOCUM(1);
            DocSeek[3].fill_sd(0, 1, "Все документы");
            DocSeek[3].val = CommonValues.DocWellEnumeration.ArchiveDocs.Value;
            DocSeek[3].desc = "Архив";

            DocSeek[4] = new DOCUM(1);
            DocSeek[4].fill_sd(0, 1, "Все документы");
            DocSeek[4].val = CommonValues.DocWellEnumeration.ExecDocs.Value;
            DocSeek[4].desc = "Исполненные документы";

            DocSeek[6] = new DOCUM(1);
            DocSeek[6].fill_sd(0, 1, "Все документы");
            DocSeek[6].val = CommonValues.DocWellEnumeration.PrintedDocs.Value;
            DocSeek[6].desc = "Отпечатанные документы";

            DocSeek[5] = new DOCUM(1);
            DocSeek[5].fill_sd(0, 1, "Очередные документы");
            DocSeek[5].val = CommonValues.DocWellEnumeration.OutDocs.Value;
            DocSeek[5].desc = "Исходящая корреспонденция";
        }
    }

    public class KINDdoc
    {
        
        public DOCUM[] DocSeek;
        public readonly int DS_cnt = 0;
        public KINDdoc(int count)
        {
            WellWork.Work = true;
            DS_cnt = count + 7;
            DocSeek = new DOCUM[DS_cnt];
            DocSeek[0] = new DOCUM(8);
            DocSeek[0].fill_sd(0, (int) DocEnumeration.WellcomeDocs.Filters.executor_docs, "Все документы");
            DocSeek[0].fill_sd(1, (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_new, "Очередные документы");
            DocSeek[0].fill_sd(2, (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_send, "Назначенные документы");
            DocSeek[0].fill_sd(3, (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_send_exec, "Исполненные документы");
            DocSeek[0].fill_sd(4, (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_send_partly_exec, "Частично исполненные документы");
            DocSeek[0].fill_sd(5, (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_exec, "Завершенные документы");
            DocSeek[0].fill_sd(6, (int)DocEnumeration.WellcomeDocs.Filters.executor_docs_new_repeat, "Повторно назначенные документы");
            DocSeek[0].fill_sd(7, (int)DocEnumeration.WellcomeDocs.Filters.executing_docs_alarm, "Просроченные документы");
            DocSeek[0].val = DocEnumeration.WellcomeDocs.Value;
            DocSeek[0].desc="Входящая корреспонденция";

            DocSeek[1] = new DOCUM(8);
            DocSeek[1].fill_sd(0, (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs, "Все документы");
            DocSeek[1].fill_sd(1, (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_new, "Очередные документы");
            DocSeek[1].fill_sd(2, (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_send, "Назначенные документы");
            DocSeek[1].fill_sd(3, (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_send_exec, "Исполненные документы");
            DocSeek[1].fill_sd(4, (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_send_partly_exec, "Частично исполненные документы");
            DocSeek[1].fill_sd(5, (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_exec, "Завершенные документы");
            DocSeek[1].fill_sd(6, (int)DocEnumeration.IndoorDosc.Filters.exe_in_docs_new_repeat, "Повторно назначенные документы");
            DocSeek[1].fill_sd(7, (int)DocEnumeration.IndoorDosc.Filters.executing_in_docs_alarm, "Просроченные документы");
            DocSeek[1].val = DocEnumeration.IndoorDosc.Value;
            DocSeek[1].desc = "Внутренняя корреспонденция";

            DocSeek[2] = new DOCUM(8);
            DocSeek[2].fill_sd(0, (int)DocEnumeration.OwnDocs.Filters.own_docs, "Все документы");
            DocSeek[2].fill_sd(1, (int)DocEnumeration.OwnDocs.Filters.own_docs_new, "Очередные документы");
            DocSeek[2].fill_sd(2, (int)DocEnumeration.OwnDocs.Filters.own_docs_send, "Назначенные документы");
            DocSeek[2].fill_sd(3, (int)DocEnumeration.OwnDocs.Filters.own_docs_send_exec, "Исполненные документы");
            DocSeek[2].fill_sd(4, (int)(int)DocEnumeration.OwnDocs.Filters.own_docs_alarm, "Просроченные документы");
            DocSeek[2].fill_sd(5, (int)DocEnumeration.OwnDocs.Filters.own_docs_signing, "Документы на визировании");
            DocSeek[2].fill_sd(6, (int)DocEnumeration.OwnDocs.Filters.own_docs_signed, "Документы завизированы");
            DocSeek[2].fill_sd(7, (int)DocEnumeration.OwnDocs.Filters.own_docs_not_signed, "Документам не завизированы");
            DocSeek[2].val = DocEnumeration.OwnDocs.Value;
            DocSeek[2].desc = "Собственная корреспонденция";

            DocSeek[3] = new DOCUM(6);
            DocSeek[3].fill_sd(0, (int)DocEnumeration.VizingDocs.Filters.vizing_docs, "Все документы");
            DocSeek[3].fill_sd(1, (int)DocEnumeration.VizingDocs.Filters.vizing_docs_new, "Очередные документы");
            DocSeek[3].fill_sd(2, (int)DocEnumeration.VizingDocs.Filters.vizing_docs_send, "Назначенные документы");
            DocSeek[3].fill_sd(3, (int)DocEnumeration.VizingDocs.Filters.vizing_docs_send_exec, "Исполненные документы");
            DocSeek[3].fill_sd(4, (int)DocEnumeration.VizingDocs.Filters.vizing_docs_exec, "Завершенные документы");
            DocSeek[3].fill_sd(5, (int)DocEnumeration.VizingDocs.Filters.vizing_docs_alarm, "Просроченные документы");
            DocSeek[3].val = DocEnumeration.VizingDocs.Value;
            DocSeek[3].desc = "Корреспонденция для визирования";

            DocSeek[4] = new DOCUM(4);
            DocSeek[4].fill_sd(0, (int)DocEnumeration.NewsDocs.Filters.executor_news, "Все документы");
            DocSeek[4].fill_sd(1, (int)DocEnumeration.NewsDocs.Filters.executor_news_new, "Очередные документы");
            DocSeek[4].fill_sd(2, (int)DocEnumeration.NewsDocs.Filters.executor_news_old, "Исполненные документы");
            DocSeek[4].fill_sd(3, (int)DocEnumeration.NewsDocs.Filters.executor_news_alarm, "Просроченные документы");
            DocSeek[4].val = DocEnumeration.NewsDocs.Value;
            DocSeek[4].desc = "Корреспонденция для работы";

            DocSeek[6] = new DOCUM(1);
            DocSeek[6].fill_sd(0, 1, "Все документы");
            DocSeek[6].val = DocEnumeration.ArchiveDocs.Value;
            DocSeek[6].desc = "Архив";

            DocSeek[5] = new DOCUM(2);
            DocSeek[5].fill_sd(0, (int)DocEnumeration.OutDocs.Filters.out_docs_new, "Очередные документы");
            DocSeek[5].fill_sd(1, (int)DocEnumeration.OutDocs.Filters.out_docs_send, "Исполненнные документы");
            DocSeek[5].val = DocEnumeration.OutDocs.Value;
            DocSeek[5].desc = "Исходящая корреспонденция";
        }
    }

    public class DocTipGrow
    {
        private int MacsCnt=0;
        private static string sql_moving = "select count(*) as cnt from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_vizing = "select count(*) as cnt from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_news = "select count(*) as cnt from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private string sql_new = "select count(*) as cnt from dbo.rkk_flow_document where rkk_flow_document.typist =dbo.user_ident() ";

        private static string sql_CurrentmovingYear = "select DATEPART (year, rkk_moving.time_m) as year from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_CurrentvizingYear = "select DATEPART (year, rkk_vizing.time_v) as year from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_CurrentnewsYear = "select DATEPART (year, rkk_news.time_n) as year from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutedmovingYear = "select DATEPART (year, rkk_moving.executed) as year from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutedvizingYear = "select DATEPART (year, rkk_vizing.executed) as year from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutednewsYear = "select DATEPART (year, rkk_news.newed) as year from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendedmovingYear = "select DATEPART (year, rkk_moving.when_m) as year from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendedvizingYear = "select DATEPART (year, rkk_vizing.when_v) as year from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendednewsYear = "select DATEPART (year, rkk_news.when_n) as year from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_newYear = "select  DATEPART (year, rkk_flow_document.date_f) as year from dbo.rkk_flow_document where rkk_flow_document.typist =dbo.user_ident() ";

        private static string sql_CurrentmovingMoonth = "select DATEPART (month, rkk_moving.time_m) as month from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_CurrentvizingMoonth = "select DATEPART (month, rkk_vizing.time_v) as month from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_CurrentnewsMoonth = "select DATEPART (month, rkk_news.time_n) as month from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutedmovingMoonth = "select DATEPART (month, rkk_moving.executed) as month from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutedvizingMoonth = "select DATEPART (month, rkk_vizing.executed) as month from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutednewsMoonth = "select DATEPART (month, rkk_news.newed) as month from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendedmovingMoonth = "select DATEPART (month, rkk_moving.when_m) as month from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendedvizingMoonth = "select DATEPART (month, rkk_vizing.when_v) as month from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendednewsMoonth = "select DATEPART (month, rkk_news.when_n) as month from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_newMoonth = "select  DATEPART (month, rkk_flow_document.date_f) as month from dbo.rkk_flow_document where rkk_flow_document.typist =dbo.user_ident() ";

        private static string sql_CurrentmovingDay = "select DATEPART (Day, rkk_moving.time_m) as Day from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_CurrentvizingDay = "select DATEPART (Day, rkk_vizing.time_v) as Day from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_CurrentnewsDay = "select DATEPART (Day, rkk_news.time_n) as Day from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutedmovingDay = "select DATEPART (Day, rkk_moving.executed) as Day from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutedvizingDay = "select DATEPART (Day, rkk_vizing.executed) as Day from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_ExecutednewsDay = "select DATEPART (Day, rkk_news.newed) as Day from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendedmovingDay = "select DATEPART (Day, rkk_moving.when_m) as Day from dbo.rkk_moving join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendedvizingDay = "select DATEPART (Day, rkk_vizing.when_v) as Day from dbo.rkk_vizing join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_SendednewsDay = "select DATEPART (Day, rkk_news.when_n) as Day from dbo.rkk_news join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_newDay = "select  DATEPART (Day, rkk_flow_document.date_f) as Day from dbo.rkk_flow_document where rkk_flow_document.typist =dbo.user_ident() ";

        private static string sql_movingID = "select rkk_flow_document.* from dbo.rkk_flow_document join dbo.rkk_moving on rkk_flow_document.kod=rkk_moving.document join dbo.emp_dep_degrees on rkk_moving.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_vizingID = "select rkk_flow_document.* from dbo.rkk_flow_document join dbo.rkk_vizing on rkk_flow_document.kod=rkk_vizing.document join dbo.emp_dep_degrees on rkk_vizing.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_newsID = "select rkk_flow_document.* from dbo.rkk_flow_document join dbo.rkk_news on rkk_flow_document.kod=rkk_news.document join dbo.emp_dep_degrees on rkk_news.for_= emp_dep_degrees.cod where emp_dep_degrees.employee=dbo.user_ident() ";
        private static string sql_newID = "select rkk_flow_document.* from dbo.rkk_flow_document where rkk_flow_document.typist =dbo.user_ident() ";
        
        public DocTipGrow(int maxDocs)
        {
            MacsCnt = maxDocs;
        }

        public string Documents_Catalog(int day, int month, int year, string cron)
        {
            string sql_add_Q = "";
            string Tree_SQL_Cast_date_moving = "";
            string Tree_SQL_Cast_date_vizing = "";
            string Tree_SQL_Cast_date_news = "";

            switch (cron)
            {
                case "ct":
                    Tree_SQL_Cast_date_moving = "rkk_moving.time_m";
                    Tree_SQL_Cast_date_vizing = "rkk_vizing.time_v";
                    Tree_SQL_Cast_date_news = "rkk_news.time_n";
                    break;
                case "ed":
                    Tree_SQL_Cast_date_moving = "rkk_moving.executed";
                    Tree_SQL_Cast_date_vizing = "rkk_vizing.executed";
                    Tree_SQL_Cast_date_news = "rkk_news.newed";
                    break;
                case "sd":
                    Tree_SQL_Cast_date_moving = "rkk_moving.when_m";
                    Tree_SQL_Cast_date_vizing = "rkk_vizing.when_v";
                    Tree_SQL_Cast_date_news = "rkk_news.when_n";
                    break;
                case "nw":
                    Tree_SQL_Cast_date_moving = "rkk_flow_document.date_f";
                    Tree_SQL_Cast_date_vizing = "rkk_flow_document.date_f";
                    Tree_SQL_Cast_date_news = "rkk_flow_document.date_f";
                    break;
            }

            if (cron.CompareTo("nw") != 0)
            {
                if (day > 0)
                {
                    sql_add_Q = sql_movingID + " and datepart( day ," + Tree_SQL_Cast_date_moving + ") =" + (string)Convert.ToString(day) + " and  datepart( month ," + Tree_SQL_Cast_date_moving + ") =" + (string)Convert.ToString(month) + " and datepart( year ," + Tree_SQL_Cast_date_moving + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "m");
                    sql_add_Q += " union ";
                    sql_add_Q += sql_vizingID + " and datepart( day ," + Tree_SQL_Cast_date_vizing + ") =" + (string)Convert.ToString(day) + " and  datepart( month ," + Tree_SQL_Cast_date_vizing + ") =" + (string)Convert.ToString(month) + " and datepart( year ," + Tree_SQL_Cast_date_vizing + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "v");
                    sql_add_Q += " union ";
                    sql_add_Q += sql_newsID + " and datepart( day ," + Tree_SQL_Cast_date_news + ") =" + (string)Convert.ToString(day) + " and  datepart( month ," + Tree_SQL_Cast_date_news + ") =" + (string)Convert.ToString(month) + " and datepart( year ," + Tree_SQL_Cast_date_news + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "n");
                }
                else if (month > 0)
                {
                    sql_add_Q = sql_movingID + " and  datepart( month ," + Tree_SQL_Cast_date_moving + ") =" + (string)Convert.ToString(month) + " and datepart( year ," + Tree_SQL_Cast_date_moving + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "m");
                    sql_add_Q += " union ";
                    sql_add_Q += sql_vizingID + " and  datepart( month ," + Tree_SQL_Cast_date_vizing + ") =" + (string)Convert.ToString(month) + " and datepart( year ," + Tree_SQL_Cast_date_vizing + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "v");
                    sql_add_Q += " union ";
                    sql_add_Q += sql_newsID + " and  datepart( month ," + Tree_SQL_Cast_date_news + ") =" + (string)Convert.ToString(month) + " and datepart( year ," + Tree_SQL_Cast_date_news + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "n");
                }
                else if (year > 0)
                {
                    sql_add_Q = sql_movingID + " and datepart( year ," + Tree_SQL_Cast_date_moving + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "m");
                    sql_add_Q += " union ";
                    sql_add_Q += sql_vizingID + " and datepart( year ," + Tree_SQL_Cast_date_vizing + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "v");
                    sql_add_Q += " union ";
                    sql_add_Q += sql_newsID + " and datepart( year ," + Tree_SQL_Cast_date_news + ") =" + (string)Convert.ToString(year);
                    sql_add_Q += SelectByStateDoc(cron, "n");
                }
                else
                {
                    sql_add_Q = sql_movingID ;
                    sql_add_Q += SelectByStateDoc(cron, "m");
                    sql_add_Q += " union ";
                    sql_add_Q += sql_vizingID;
                    sql_add_Q += SelectByStateDoc(cron, "v");
                    sql_add_Q += " union ";
                    sql_add_Q += sql_newsID ;
                    sql_add_Q += SelectByStateDoc(cron, "n");
                }
            }
            else sql_add_Q = sql_movingID + " and datepart( year ," + Tree_SQL_Cast_date_moving + ") =" + (string)Convert.ToString(year) +  SelectByStateDoc(cron, "m");
            ;
            
            return sql_add_Q;
        }

        private string SelectByStateDoc(string cron, string stat)
    {
        string sql="";
        switch (cron)
        {
            case "ct":
                switch (stat)
                {
                    case "m":
                        sql = CurrentmovingID();
                        break;
                    case "v":
                        sql = CurrentvizingID();
                        break;
                    case "n":
                        sql = CurrentnewsID();
                        break;
                }
                break;
            case "ed":
                switch (stat)
                {
                    case "m":
                        sql = ExecutedmovingID();
                        break;
                    case "v":
                        sql = ExecutedvizingID();
                        break;
                    case "n":
                        sql = ExecutednewsID();
                        break;
                }
                break;
            case "sd":
                switch (stat)
                {
                    case "m":
                        sql = SendedmovingID();
                        break;
                    case "v":
                        sql = SendedvizingID();
                        break;
                    case "n":
                        sql = SendednewsID();
                        break;
                }
                break;
            case "nw":
                switch (stat)
                {
                    case "m":
                    case "v":
                    case "n":
                        sql = NewID();
                        break;
               }
                break;
        }
        return sql;
    }
        //Года

        public string YearsCountCurrentmoving() 
        {
            return sql_moving + " and rkk_moving.exe_doc is null and rkk_moving.moving not in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident())"; 
        }

        public string YearsCurrentmovingID()
        {
            return sql_CurrentmovingYear + " and rkk_moving.exe_doc is null and rkk_moving.moving not in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident())";
        }

        public string YearsCountCurrentvizing()
        {
            return sql_vizing + " and rkk_vizing.viza is null and rkk_vizing.id not in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident())";
        }

        public string YearsCurrentvizingID()
        {
            return sql_CurrentvizingYear + " and rkk_vizing.viza is null and rkk_vizing.id not in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident())";
        }

        public string YearsCountCurrentnews()
        {
            return sql_news + " and rkk_news.newed is null and rkk_news.news not in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident())";
        }

        public string YearsCurrentnewsID()
        {
            return sql_CurrentnewsYear + " and rkk_news.newed is null and rkk_news.news not in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident())";
        }

        public string YearsCountExecutedmoving() 
        {
            return sql_moving + " and rkk_moving.exe_doc is not null"; 
        }

        public string YearsExecutedmovingID()
        {
            return sql_ExecutedmovingYear + " and rkk_moving.exe_doc is not null";
        }

        public string YearsCountExecutedvizing()
        {
            return sql_vizing + " and rkk_vizing.viza is not null";
        }

        public string YearsExecutedvizingID()
        {
            return sql_ExecutedvizingYear + " and rkk_vizing.viza is not null";
        }

        public string YearsCountExecutednews()
        {
            return sql_news + " and rkk_news.newed is not null";
        }

        public string YearsExecutednewsID()
        {
            return sql_ExecutednewsYear + " and rkk_news.newed is not null";
        }

        public string YearsCountSendedmoving() 
        {
            return sql_moving + "  and rkk_moving.exe_doc is null and rkk_moving.moving in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident())"; 
        }

        public string YearsSendedmovingID()
        {
            return sql_SendedmovingYear + "  and rkk_moving.exe_doc is null and rkk_moving.moving in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident())";
        }

        public string YearsCountSendedvizing()
        {
            return sql_vizing + "  and rkk_vizing.viza is null and rkk_vizing.id in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident())";
        }

        public string YearsSendedvizingID()
        {
            return sql_SendedvizingYear + "  and rkk_vizing.viza is null and rkk_vizing.id in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident())";
        }

        public string YearsCountSendednews()
        {
            return sql_news + "  and rkk_news.newed is null and rkk_news.news in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident())";
        }

        public string YearsSendednewsID()
        {
            return sql_SendednewsYear + "  and rkk_news.newed is null and rkk_news.news in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident())";
        }

        public string YearsCountNew() 
        {
            return sql_newYear + " and rkk_flow_document.kod not in (select document from dbo.rkk_moving as moving) and rkk_flow_document.kod not in (select document from dbo.rkk_vizing as vizing) and rkk_flow_document.kod not in (select document from dbo.rkk_news as news)"; 
        }

        public string YearsNewID()
        {
            return sql_new + " and rkk_flow_document.kod not in (select document from dbo.rkk_moving as moving) and rkk_flow_document.kod not in (select document from dbo.rkk_vizing as vizing) and rkk_flow_document.kod not in (select document from dbo.rkk_news as news)";
        }

        //Месяцы

        public string MoonthCountCurrentmoving(int year)
        {
            return sql_moving + " and rkk_moving.exe_doc is null and rkk_moving.moving not in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident()) and  DATEPART (year, rkk_moving.time_m) =" + year.ToString();
        }

        public string MoonthCurrentmovingID(int year)
        {
            return sql_CurrentmovingMoonth + " and rkk_moving.exe_doc is null and rkk_moving.moving not in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident()) and  DATEPART (year, rkk_moving.time_m) =" + year.ToString();
        }

        public string MoonthCountCurrentvizing(int year)
        {
            return sql_vizing + " and rkk_vizing.viza is null and rkk_vizing.id not in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident()) and  DATEPART (year, rkk_vizing.time_v) =" + year.ToString();
        }

        public string MoonthCurrentvizingID(int year)
        {
            return sql_CurrentvizingMoonth + " and rkk_vizing.viza is null and rkk_vizing.id not in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident()) and  DATEPART (year, rkk_vizing.time_v) =" + year.ToString();
        }

        public string MoonthCountCurrentnews(int year)
        {
            return sql_news + " and rkk_news.newed is null and rkk_news.news not in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident()) and  DATEPART (year, rkk_news.time_n) =" + year.ToString();
        }

        public string MoonthCurrentnewsID(int year)
        {
            return sql_CurrentnewsMoonth + " and rkk_news.newed is null and rkk_news.news not in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident()) and  DATEPART (year, rkk_news.time_n) =" + year.ToString();
        }

        public string MoonthCountExecutedmoving(int year)
        {
            return sql_moving + " and rkk_moving.exe_doc is not null and  DATEPART (year, rkk_moving.executed) =" + year.ToString();
        }

        public string MoonthExecutedmovingID(int year)
        {
            return sql_ExecutedmovingMoonth + " and rkk_moving.exe_doc is not null and  DATEPART (year, rkk_moving.executed) =" + year.ToString();
        }

        public string MoonthCountExecutedvizing(int year)
        {
            return sql_vizing + " and rkk_vizing.viza is not null and  DATEPART (year, rkk_vizing.executed) =" + year.ToString();
        }

        public string MoonthExecutedvizingID(int year)
        {
            return sql_ExecutedvizingMoonth + " and rkk_vizing.viza is not null and  DATEPART (year, rkk_vizing.executed) =" + year.ToString();
        }

        public string MoonthCountExecutednews(int year)
        {
            return sql_news + " and rkk_news.newed is not null and  DATEPART (year, rkk_news.newed) =" + year.ToString();
        }

        public string MoonthExecutednewsID(int year)
        {
            return sql_ExecutednewsMoonth + " and rkk_news.newed is not null and  DATEPART (year, rkk_news.newed) =" + year.ToString();
        }

        public string MoonthCountSendedmoving(int year)
        {
            return sql_moving + "  and rkk_moving.exe_doc is null and rkk_moving.moving in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident()) and  DATEPART (year, rkk_moving.when_m) =" + year.ToString();
        }

        public string MoonthSendedmovingID(int year)
        {
            return sql_SendedmovingMoonth + "  and rkk_moving.exe_doc is null and rkk_moving.moving in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident()) and  DATEPART (year, rkk_moving.when_m) =" + year.ToString();
        }

        public string MoonthCountSendedvizing(int year)
        {
            return sql_vizing + "  and rkk_vizing.viza is null and rkk_vizing.id in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident()) and  DATEPART (year, rkk_vizing.when_v) =" + year.ToString();
        }

        public string MoonthSendedvizingID(int year)
        {
            return sql_SendedvizingMoonth + "  and rkk_vizing.viza is null and rkk_vizing.id in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident()) and  DATEPART (year, rkk_vizing.when_v) =" + year.ToString();
        }

        public string MoonthCountSendednews(int year)
        {
            return sql_news + "  and rkk_news.newed is null and rkk_news.news in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident()) and  DATEPART (year, rkk_news.when_n) =" + year.ToString(); ;
        }

        public string MoonthSendednewsID(int year)
        {
            return sql_SendednewsMoonth + "  and rkk_news.newed is null and rkk_news.news in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident()) and  DATEPART (year, rkk_news.when_n) =" + year.ToString(); ;
        }

        public string MoonthCountNew(int year)
        {
            return sql_new + " and rkk_flow_document.kod not in (select document from dbo.rkk_moving as moving) and rkk_flow_document.kod not in (select document from dbo.rkk_vizing as vizing) and rkk_flow_document.kod not in (select document from dbo.rkk_news as news) and  DATEPART (year, rkk_flow_document.date_f) =" + year.ToString(); ;
        }

        public string MoonthNewID(int year)
        {
            return sql_newMoonth + " and rkk_flow_document.kod not in (select document from dbo.rkk_moving as moving) and rkk_flow_document.kod not in (select document from dbo.rkk_vizing as vizing) and rkk_flow_document.kod not in (select document from dbo.rkk_news as news) and  DATEPART (year, rkk_flow_document.date_f) =" + year.ToString(); 
        }

        //Дни

        public string DayCountCurrentmoving(int year, int month)
        {
            return sql_moving + " and rkk_moving.exe_doc is null and rkk_moving.moving not in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident()) and  DATEPART (year, rkk_moving.time_m) =" + year.ToString() + "  and  DATEPART (month, rkk_moving.time_m) =" + month.ToString();
        }

        public string DayCurrentmovingID(int year, int month)
        {
            return sql_CurrentmovingDay + " and rkk_moving.exe_doc is null and rkk_moving.moving not in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident()) and  DATEPART (year, rkk_moving.time_m) =" + year.ToString() + "  and  DATEPART (month, rkk_moving.time_m) =" + month.ToString();
        }

        public string DayCountCurrentvizing(int year, int month)
        {
            return sql_vizing + " and rkk_vizing.viza is null and rkk_vizing.id not in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident()) and  DATEPART (year, rkk_vizing.time_v) =" + year.ToString() + "  and  DATEPART (month, rkk_vizing.time_v) =" + month.ToString();
        }

        public string DayCurrentvizingID(int year, int month)
        {
            return sql_CurrentvizingDay + " and rkk_vizing.viza is null and rkk_vizing.id not in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident()) and  DATEPART (year, rkk_vizing.time_v) =" + year.ToString() + "  and  DATEPART (month, rkk_vizing.time_v) =" + month.ToString();
        }

        public string DayCountCurrentnews(int year, int month)
        {
            return sql_news + " and rkk_news.newed is null and rkk_news.news not in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident()) and  DATEPART (year, rkk_news.time_n) =" + year.ToString() + " and  DATEPART (month, rkk_news.time_n) =" + month.ToString();
        }

        public string DayCurrentnewsID(int year, int month)
        {
            return sql_CurrentnewsDay + " and rkk_news.newed is null and rkk_news.news not in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident()) and  DATEPART (year, rkk_news.time_n) =" + year.ToString() + " and  DATEPART (month, rkk_news.time_n) =" + month.ToString();
        }

        public string DayCountExecutedmoving(int year, int month)
        {
            return sql_moving + " and rkk_moving.exe_doc is not null and  DATEPART (year, rkk_moving.executed) =" + year.ToString() + " and  DATEPART (month, rkk_moving.executed) =" + month.ToString();
        }

        public string DayExecutedmovingID(int year, int month)
        {
            return sql_ExecutedmovingDay + " and rkk_moving.exe_doc is not null and  DATEPART (year, rkk_moving.executed) =" + year.ToString() + " and  DATEPART (month, rkk_moving.executed) =" + month.ToString();
        }

        public string DayCountExecutedvizing(int year, int month)
        {
            return sql_vizing + " and rkk_vizing.viza is not null and  DATEPART (year, rkk_vizing.executed) =" + year.ToString() + " and  DATEPART (month, rkk_vizing.executed) =" + month.ToString();
        }

        public string DayExecutedvizingID(int year, int month)
        {
            return sql_ExecutedvizingDay + " and rkk_vizing.viza is not null and  DATEPART (year, rkk_vizing.executed) =" + year.ToString() + " and  DATEPART (month, rkk_vizing.executed) =" + month.ToString();
        }

        public string DayCountExecutednews(int year, int month)
        {
            return sql_news + " and rkk_news.newed is not null and  DATEPART (year, rkk_news.newed) =" + year.ToString() + " and  DATEPART (month, rkk_news.newed) =" + month.ToString();
        }

        public string DayExecutednewsID(int year, int month)
        {
            return sql_ExecutednewsDay + " and rkk_news.newed is not null and  DATEPART (year, rkk_news.newed) =" + year.ToString() + " and  DATEPART (month, rkk_news.newed) =" + month.ToString();
        }

        public string DayCountSendedmoving(int year, int month)
        {
            return sql_moving + "  and rkk_moving.exe_doc is null and rkk_moving.moving in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident()) and  DATEPART (year, rkk_moving.when_m) =" + year.ToString() + "  and  DATEPART (month, rkk_moving.when_m) =" + month.ToString();
        }

        public string DaySendedmovingID(int year, int month)
        {
            return sql_SendedmovingDay + "  and rkk_moving.exe_doc is null and rkk_moving.moving in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident()) and  DATEPART (year, rkk_moving.when_m) =" + year.ToString() + "  and  DATEPART (month, rkk_moving.when_m) =" + month.ToString();
        }

        public string DayCountSendedvizing(int year, int month)
        {
            return sql_vizing + "  and rkk_vizing.viza is null and rkk_vizing.id in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident()) and  DATEPART (year, rkk_vizing.when_v) =" + year.ToString() + " and  DATEPART (month, rkk_vizing.when_v) =" + month.ToString();
        }

        public string DaySendedvizingID(int year, int month)
        {
            return sql_SendedvizingDay + "  and rkk_vizing.viza is null and rkk_vizing.id in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident()) and  DATEPART (year, rkk_vizing.when_v) =" + year.ToString() + " and  DATEPART (month, rkk_vizing.when_v) =" + month.ToString();
        }

        public string DayCountSendednews(int year, int month)
        {
            return sql_news + "  and rkk_news.newed is null and rkk_news.news in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident()) and  DATEPART (year, rkk_news.when_n) =" + year.ToString() + " and  DATEPART (month, rkk_news.when_n) =" + month.ToString(); 
        }

        public string DaySendednewsID(int year, int month)
        {
            return sql_SendednewsDay + "  and rkk_news.newed is null and rkk_news.news in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident()) and  DATEPART (year, rkk_news.when_n) =" + year.ToString() + " and  DATEPART (month, rkk_news.when_n) =" + month.ToString();
        }

        public string DayCountNew(int year, int month)
        {
            return sql_new + " and rkk_flow_document.kod not in (select document from dbo.rkk_moving as moving) and rkk_flow_document.kod not in (select document from dbo.rkk_vizing as vizing) and rkk_flow_document.kod not in (select document from dbo.rkk_news as news) and  DATEPART (year, rkk_flow_document.date_f) =" + year.ToString() + " and  DATEPART (month, rkk_flow_document.date_f) =" + month.ToString(); 
        }

        public string DayNewID(int year, int month)
        {
            return sql_newDay + " and rkk_flow_document.kod not in (select document from dbo.rkk_moving as moving) and rkk_flow_document.kod not in (select document from dbo.rkk_vizing as vizing) and rkk_flow_document.kod not in (select document from dbo.rkk_news as news) and  DATEPART (year, rkk_flow_document.date_f) =" + year.ToString() + " and  DATEPART (month, rkk_flow_document.date_f) =" + month.ToString();
        }

        // Документы

        public string CurrentmovingID()
        {
            return  " and rkk_moving.exe_doc is null and rkk_moving.moving not in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident())";
        }

        public string CurrentvizingID()
        {
            return  " and rkk_vizing.viza is null and rkk_vizing.id not in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident())";
        }

        public string CurrentnewsID()
        {
            return  " and rkk_news.newed is null and rkk_news.news not in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident())";
        }

        public string ExecutedmovingID()
        {
            return  " and rkk_moving.exe_doc is not null";
        }

        public string ExecutedvizingID()
        {
            return  " and rkk_vizing.viza is not null";
        }

        public string ExecutednewsID()
        {
            return  " and rkk_news.newed is not null";
        }

        public string SendedmovingID()
        {
            return  "  and rkk_moving.exe_doc is null and rkk_moving.moving in (select moving from dbo.rkk_moving as moving where moving.parent = dbo.user_ident())";
        }

        public string SendedvizingID()
        {
            return  "  and rkk_vizing.viza is null and rkk_vizing.id in (select id from dbo.rkk_vizing as vizing where vizing.parent = dbo.user_ident())";
        }

        public string SendednewsID()
        {
            return  "  and rkk_news.newed is null and rkk_news.news in (select news from dbo.rkk_news as news where news.typist = dbo.user_ident())";
        }

        public string NewID()
        {
            return  " and rkk_flow_document.kod not in (select document from dbo.rkk_moving as moving) and rkk_flow_document.kod not in (select document from dbo.rkk_vizing as vizing) and rkk_flow_document.kod not in (select document from dbo.rkk_news as news)";
        }

    }

    public class TreeGrow
    {
        public TreeGrow(int maxtree)
        {
            if (Class_AMAS_Query.Tree_RKKCnt > maxtree) Class_AMAS_Query.Tree_RKKCnt = maxtree;
        }

        public string YearsCount() { return AMAS_Query.Class_AMAS_Query.Tree_SQL_count + AMAS_Query.Class_AMAS_Query.Tree_SQL_query; }
        public string Years() {return AMAS_Query.Class_AMAS_Query.Tree_SQL_year + AMAS_Query.Class_AMAS_Query.Tree_SQL_query + " order by year asc";}
        
        public string MonthCount (int year)
        {
            return  Class_AMAS_Query.Tree_SQL_count + AMAS_Query.Class_AMAS_Query.Where_OR_And(Class_AMAS_Query.Tree_SQL_count) + " date_f between dbo.ymd(" + (string)Convert.ToString(year) + ",1,1) and dbo.ymd(" + (string)Convert.ToString(year) + ",12,32)";
            
        }
        public string Months(int year)
        {
            string sql= Class_AMAS_Query.Tree_SQL_month +  Class_AMAS_Query.Tree_SQL_query ;
            sql += Class_AMAS_Query.Where_OR_And(sql) + "datepart (year," + Class_AMAS_Query.Tree_SQL_Cast_date + ")=" + (string)Convert.ToString(year) + " order by month asc";
            return sql;
        }

        public string DaysCount (int year, int month)
        {
            return Class_AMAS_Query.Tree_SQL_count + AMAS_Query.Class_AMAS_Query.Where_OR_And(Class_AMAS_Query.Tree_SQL_count) + " date_f between dbo.ymd(" + (string)Convert.ToString(year) + "," + (string)Convert.ToString(month) + ",1) and dbo.ymd(" + (string)Convert.ToString(year) + "," + (string)Convert.ToString(month) + ",32)";
        }

        public string Days(int year, int month)
        {
            string sql = Class_AMAS_Query.Tree_SQL_day + Class_AMAS_Query.Tree_SQL_query;
            sql += Class_AMAS_Query.Where_OR_And(sql) + "datepart (year," + Class_AMAS_Query.Tree_SQL_Cast_date + ")=" + (string)Convert.ToString(year) + " and datepart(month," + Class_AMAS_Query.Tree_SQL_Cast_date + ")=" + (string)Convert.ToString(month) + " order by day asc";
            return sql;
        }
    }

    public static class WorkflowSelectDocs
    {
        public static string Tree_SQL = "";
        public static string Tree_SQL_query = "";
        public static string Tree_SQL_order = "";
        public static string Tree_SQL_count = "";
        public static string Tree_SQL_year = "";
        public static string Tree_SQL_month = "";
        public static string Tree_SQL_day = "";
        public static string Tree_SQL_Cast_date = "";

        public static void select(int Doc_Filtre, string Find)
        {
            switch (Doc_Filtre)
            {

                case 0:
                    Tree_SQL = "select * from dbo.rkk_documents";
                    Tree_SQL_query = " ";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL_count = "select count(*) as cnt from dbo.rkk_documents";
                    Tree_SQL_year = "select distinct datepart( year , rkk_documents.date_f ) as Year from dbo.rkk_documents";
                    Tree_SQL_month = "select distinct datepart( month , rkk_documents.date_f ) as month from dbo.rkk_documents";
                    Tree_SQL_day = "select distinct datepart( day , rkk_documents.date_f ) as day from dbo.rkk_documents";
                    Tree_SQL_Cast_date = "rkk_documents.date_f";
                    break;
                case 1:
                    Tree_SQL = "select documents.* from dbo.rkk_documents";
                    Tree_SQL_query = " where kod in (select document from dbo.rkk_moving)";
                    Tree_SQL_order = "  order by kod desc";
                    Tree_SQL_count = "select count(*) as cnt from dbo.rkk_documents";
                    Tree_SQL_year = "select distinct datepart( year , rkk_documents.date_f ) as Year from dbo.rkk_documents";
                    Tree_SQL_month = "select distinct datepart( month , rkk_documents.date_f ) as month from dbo.rkk_documents";
                    Tree_SQL_day = "select distinct datepart( day , rkk_documents.date_f ) as day from dbo.rkk_documents";
                    Tree_SQL_Cast_date = "rkk_documents.date_f";
                    break;
                case 2:
                    Tree_SQL = "select * from dbo.rkk_documents";
                    Tree_SQL_query = " where kod not in(select document from dbo.rkk_moving)";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL_count = "select count(*) as cnt from dbo.rkk_documents";
                    Tree_SQL_year = "select distinct datepart( year , rkk_documents.date_f ) as Year from dbo.rkk_documents";
                    Tree_SQL_month = "select distinct datepart( month , rkk_documents.date_f ) as month from dbo.rkk_documents";
                    Tree_SQL_day = "select distinct datepart( day , rkk_documents.date_f ) as day from dbo.rkk_documents";
                    Tree_SQL_Cast_date = "rkk_documents.date_f";
                    break;
                case 3:
                    Tree_SQL = "select * from dbo.rkk_archive";
                    Tree_SQL_query = " ";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL_count = "select count(*) as cnt from dbo.rkk_archive";
                    Tree_SQL_year = "select distinct datepart( year , date_f ) as Year from dbo.rkk_archive";
                    Tree_SQL_month = "select distinct datepart( month , date_f ) as month from dbo.rkk_archive";
                    Tree_SQL_day = "select distinct datepart( day , date_f ) as day from dbo.rkk_archive";
                    Tree_SQL_Cast_date = "date_f";
                    break;
                case 4:
                    Tree_SQL = "select * from dbo.rkk_documents";
                    Tree_SQL_query = " where kod in (select document from dbo.rkk_moving where exe_doc is not null and pattern =dbo.user_ident())";
                    Tree_SQL_order = " order by kod desc";
                    Tree_SQL_count = "select count(*) as cnt from dbo.rkk_documents";
                    Tree_SQL_year = "select distinct datepart( year , rkk_documents.date_f ) as Year from dbo.rkk_documents";
                    Tree_SQL_month = "select distinct datepart( month , rkk_documents.date_f ) as month from dbo.rkk_documents";
                    Tree_SQL_day = "select distinct datepart( day , rkk_documents.date_f ) as day from dbo.rkk_documents";
                    Tree_SQL_Cast_date = "rkk_documents.date_f";
                    break;
                case 5:
                    Tree_SQL = "select * from dbo.rkk_ready_documents";
                    Tree_SQL_query = " ";
                    Tree_SQL_order = " order by date_o desc";
                    Tree_SQL_count = "select count(*) as cnt from dbo.rkk_ready_documents";
                    Tree_SQL_year = "select distinct datepart( year , rkk_ready_documents.date_o ) as Year from dbo.rkk_ready_documents";
                    Tree_SQL_month = "select distinct datepart( month , rkk_ready_documents.date_o ) as month from dbo.rkk_ready_documents";
                    Tree_SQL_day = "select distinct datepart( day , rkk_ready_documents.date_o ) as day from dbo.rkk_ready_documents";
                    Tree_SQL_Cast_date = "date_o";
                    break;
                case 6:
                    Tree_SQL = "select * from dbo.out_finish_documents";
                    Tree_SQL_query = " ";
                    Tree_SQL_order = " order by date_o desc";
                    Tree_SQL_count = "select count(*) as cnt from dbo.out_finish_documents";
                    Tree_SQL_year = "select distinct datepart( year , out_finish_documents.date_f ) as Year from dbo.out_finish_documents";
                    Tree_SQL_month = "select distinct datepart( month , out_finish_documents.date_f ) as month from dbo.out_finish_documents";
                    Tree_SQL_day = "select distinct datepart( day , out_finish_documents.date_f ) as day from dbo.out_finish_documents";
                    Tree_SQL_Cast_date = "date_f";
                    break;
            }
    
        }

    }

    public static class OutDocument
    {
        public static string ListDocsToMail
        {
            get { return "select rkk_flow_document.find_cod,rkk_flow_document.kod from  dbo.rkk_flow_document join dbo.out_for_mail on out_for_mail.document=rkk_flow_document.kod"; }
        }
        public static string ListOutDocs
        {
            get { return "select rkk_flow_document.find_cod,rkk_flow_document.kod from  dbo.rkk_flow_document join dbo.out_document on out_document.kod=rkk_flow_document.kod order by rkk_flow_document.kod desc"; }
        }
    }

    public static class Cargo
    {
        public static string CargoEntRoutes
        {
            get
            {
                string sql = "select id, CargoCompany, RouteName, FromPlace, ToPlace, FromAddress, ToAddress, TimeStart, Period, cost from  dbo.cargo_routes where CargoCompany=@CargoCompany order by cost asc ";
                return sql;
            }
        }

        public static string CargToPlaceRoutes
        {
            get
            {
                string sql = "select id, CargoCompany, RouteName, FromPlace, ToPlace, FromAddress, ToAddress, TimeStart, Period, cost from  dbo.cargo_routes where ToPlace=@ToPlace order by cost asc ";
                return sql;
            }
        }

        public static string CargFromPlaceRoutes
        {
            get
            {
                string sql = "select id, CargoCompany, RouteName, FromPlace, ToPlace, FromAddress, ToAddress, TimeStart, Period, cost from  dbo.cargo_routes where FromPlace=@FromPlace order by cost asc ";
                return sql;
            }
        }

        public static string CargInPlaceRoutes
        {
            get
            {
                string sql = "select id, CargoCompany, RouteName, FromPlace, ToPlace, FromAddress, ToAddress, TimeStart, Period, cost from  dbo.cargo_routes where FromPlace=@InPlace and ToPlace=@InPlace order by cost asc ";
                return sql;
            }
        }

        public static string CargRoutes
        {
            get
            {
                string sql = "select id, CargoCompany, RouteName, FromPlace, ToPlace, FromAddress, ToAddress, TimeStart, Period, cost from  dbo.cargo_routes order by id asc";
                return sql;
            }
        }

        public static string TheCargRoute
        {
            get
            {
                string sql = "select id, CargoCompany, RouteName, FromPlace, ToPlace, FromAddress, ToAddress, TimeStart, Period, cost from  dbo.cargo_routes where id =@id";
                return sql;
            }
        }
        public static string CargoContragents
        {
            get
            {
                string sql = "select distinct  agent_all.agentid, agent_all.agentname from dbo.agent_all join dbo.Cargo_routes on agent_all.agentid=Cargo_routes.cargoCompany";
                return sql;
            }
        }

    }
}

