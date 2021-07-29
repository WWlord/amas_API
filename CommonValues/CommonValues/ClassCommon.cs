using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace CommonValues
{
    public static class CommonClass
    {
        public static string TempDirectory
        {
            get
            {
                string sss = Path.GetTempPath();
                if (sss.Substring(sss.Length - 1).CompareTo(@"\") != 0) sss += @"\";
                return sss ;
            }
        }

        public static class CommonDocumentLibrary
        {
            public static DataTable Documents;
            private static int Rows = 0;

            public static int SaveDocument(string InputFile)
            {
                int res = 0;
                byte[] buff = null;
                string att = "";
                buff = GetData(InputFile);
                int index=0;
                if (buff != null)
                {
                    try
                    {
                        DataRow Row = Documents.NewRow();
                        att = ExtFile(InputFile);
                        Row["page"] = index;
                        if (att.ToLower().CompareTo("txt") == 0 || att.ToLower().CompareTo("rtf") == 0 || att.ToLower().CompareTo("bmp") == 0 || att.ToLower().CompareTo("jpg") == 0 || att.ToLower().CompareTo("pdf") == 0 || att.ToLower().CompareTo("gif") == 0 || att.ToLower().CompareTo("AMASplb".ToLower()) == 0)
                        {
                            Row["content"] = buff;
                            Row["file_typ"] = att;
                        }
                        else
                        {
                        }

                        Documents.Rows.Add(Row);
                        Row.EndEdit();
                        res = (int)Row["id"];
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message; res = -1;
                    }
                }
                return res;
            }

            private static byte[] GetData(string InputFileName)
            {
                byte[] buffer = null;
                try
                {
                    StreamReader f1 = new StreamReader(InputFileName);
                    buffer = new byte[f1.BaseStream.Length];
                    //for (int i = 0; i < f1.BaseStream.Length; i++)
                    //    buffer[i] = (byte)f1.BaseStream.ReadByte();
                    f1.BaseStream.Read(buffer, 0, (int)f1.BaseStream.Length);
                    f1.Close();
                }
                catch { buffer = null; }
                return buffer;
            }

            public static string ExtFile(string InputFile)
            {
                int lp = LastPoint(InputFile);
                string ret = "";
                if (lp >= 0)
                    ret = InputFile.Substring(lp + 1, InputFile.Length - LastPoint(InputFile) - 1);
                else
                    ret = "";
                return ret;
            }

            private static int LastPoint(string DOTstring)
            {
                int dot = -1;
                for (int i = 0; i < DOTstring.Length; i++)
                {
                    if (DOTstring.Substring(i, 1).CompareTo(".") == 0) dot = i;
                }
                return dot;
            }

            public static void Structure_Document()
            {
                Documents = new DataTable("Documents");

                Documents.Columns.Add("id", typeof(System.Int32));
                Documents.Columns["id"].AutoIncrement = true;
                Documents.Columns["id"].AllowDBNull = false;
                Documents.Columns["id"].AutoIncrementStep = 1;
                Documents.PrimaryKey = new DataColumn[] { Documents.Columns["id"] };

                Documents.Columns.Add("page", typeof(System.Int32));
                Documents.Columns["page"].AllowDBNull = false;

                Documents.Columns.Add("content", typeof(System.Byte[]));
                Documents.Columns["content"].AllowDBNull = false;

                Documents.Columns.Add("file_typ", typeof(System.String));
                Documents.Columns["file_typ"].AllowDBNull = false;

                Documents.Columns.Add("BASE_content", typeof(System.Byte[]));
                Documents.Columns["BASE_content"].AllowDBNull = true;

                Documents.Columns.Add("BASE_file_typ", typeof(System.String));
                Documents.Columns["BASE_file_typ"].AllowDBNull = true;
            }

            public static void CloseDocument(string filename)
            {
                Documents.WriteXml(filename);
            }

        }

        public static int headShift = 24;

        public enum Lists { Archive = 1, Kind, Tema, Coming, Prefix, Suffix, Viza, WellKind, OutKind, Delo };
        public enum TypeofDocument { Archive = 1, Wellcome, Indoor, Outdoor, Corporative };

        public class Arraysheet
        {
            private int the_Id;
            private string the_Name;

            public Arraysheet(string Name_, int Id_)
            {

                the_Name = Name_;
                the_Id = Id_;
            }

            public string Name
            {
                get
                {
                    return the_Name;
                }
            }

            public string Id
            {
                get
                {
                    return the_Id.ToString();
                }
            }

            public override string ToString()
            {
                return the_Name;
            }
        }

        public class ArrayThree
        {
            private int the_Id;
            private int the_FileId;
            private string the_Name;

            public ArrayThree(string Name_, int Id_)
            {
                the_Name = Name_;
                the_Id = Id_;
            }

            public string Name
            {
                get
                {
                    return the_Name;
                }
            }

            public string Id
            {
                get
                {
                    return the_Id.ToString();
                }
            }

            public int FId
            {
                get
                {
                    return the_FileId;
                }
                set
                {
                    the_FileId = value;
                }
            }

            public override string ToString()
            {
                return FId.ToString();
            }
        }

        public class Arrayagents
        {
            private int the_Id;
            private string the_Name;
            private int the_address;
            private int the_type;
            public Arrayagents(string Name_, int Id_, int addr, int type_)
            {

                the_Name = Name_;
                the_Id = Id_;
                the_address = addr;
                the_type = type_;
            }

            public int Adress
            {
                get { return the_address; }
            }

            public int Type
            {
                get { return the_type; }
            }

            public string Name
            {
                get
                {
                    return the_Name;
                }
            }

            public string Id
            {
                get
                {
                    return the_Id.ToString();
                }
            }

            public override string ToString()
            {
                return the_Name;
            }
        }

        public static string SaveFilewithHead(string BlockFilePath)
        {
                string filename = "Save.xml";
            try
            {
                filename = TempDirectory + Path.GetRandomFileName()+".docx";
                try { File.Delete(filename); }
                catch { }
                char[] extchar = getfileExtention(BlockFilePath).PadRight(headShift, ' ').ToCharArray();
                byte[] Block = GetImage(BlockFilePath);
                byte[] image = new byte[Block.Length + headShift];
                for (int i = 0; i < headShift; i++)
                    image[i] = (byte)extchar[i];
                for (int i = 0; i < Block.Length; i++)
                    image[i + headShift] = Block[i];
                FileStream filen = new FileStream(filename, FileMode.CreateNew);
                filen.Write(image, 0, image.Length);
                filen.Close();
            }
            catch { filename = ""; }
            return filename;
        }

        public static byte[] SetImageHead(string BlockFilePath)
        {
            char[] extchar = getfileExtention(BlockFilePath).PadRight(headShift, ' ').ToCharArray();
            byte[] Block = GetImage(BlockFilePath);
            byte[] image = new byte[Block.Length + headShift];
            for (int i = 0; i < headShift; i++)
                image[i] = (byte)extchar[i];
            for (int i = 0; i < Block.Length; i++)
                image[i + headShift] = Block[i];
            return image;
        }

        public static byte[] GetImage(string filePath)
        {
            byte[] image=null;
            if (filePath.Length>1)
                try
                {
                    FileStream stream = new FileStream(
                        filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(stream);
                    //long lenght =0;  
                    //int bufsize = 30000;
                    image = new byte[(int)stream.Length];
                    //image = new byte[stream.Length];
                    //while (lenght < image.Length - bufsize)
                    //{
                    //    image.SetValue(reader.ReadBytes(bufsize), lenght);
                    //    lenght += bufsize;
                    //}
                    //image.SetValue(reader.ReadBytes((int)(image.Length - lenght)), lenght);
                    image = reader.ReadBytes((int)stream.Length);
                    reader.Close();
                    stream.Close();
                }
                catch  {  image = null; }
            return image;
        }

        public static string ForwardSlash(string source)
        {
            string finish = "";
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Substring(i, 1).CompareTo(@"\") == 0) finish += @"/"; else finish += source.Substring(i, 1);
            }
            return finish;
        }

        public static string BackSlash(string source)
        {
            string finish = "";
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Substring(i, 1).CompareTo(@"/") == 0) finish += @"\"; else finish += source.Substring(i, 1);
            }
            return finish;
        }

        public static string getfileExtention(string filename)
        {
            string lostExt = "";

            if (filename.Length > 0)
            {
                string ext = "";
                ext = filename.Substring(filename.IndexOf(".") + 1);
                if (ext.Contains("."))
                    lostExt = getfileExtention(ext);
                else lostExt = ext;
                if (ext.IndexOf("\\")  >= 0)
                {
                    ext = ext.Substring(ext.IndexOf("\\") + 1);
                    if (ext.Contains("\\"))
                        lostExt = getfileExtention(ext);
                    else lostExt = ext;
                }
                if (ext.IndexOf("/")  >= 0)
                {
                    ext = ext.Substring(ext.IndexOf("/") + 1);
                    if (ext.Contains("/"))
                        lostExt = getfileExtention(ext);
                    else lostExt = ext;
                }
                if (ext.IndexOf(":") >=0)
                {
                    ext = ext.Substring(ext.IndexOf(":") + 1);
                    if (ext.Contains(":"))
                        lostExt = getfileExtention(ext);
                    else lostExt = ext;
                }
            }
            else lostExt = filename;

            return lostExt;
        }
    }
    public class PhonesMails
    {
        private int the_Id;
        private string the_Name;

        public int Id
        {
            get { return the_Id; }
            set { the_Id = (int)value; }
        }

        public string Name
        {
            get { return the_Name; }
            set { the_Name = (string)value; }
        }

        public PhonesMails(string Name_, int Id_)
        {

            the_Name = Name_;
            the_Id = Id_;
        }
    }

    public enum PlaceLevel
    {
        NothingLevel = -1,
        StateLevel = 0,
        RegionLevel = 1,
        ArealLevel = 2,
        CityLevel = 3,
        DistrictLevel = 4,
        StreetLevel = 5,
        HouseLevel = 6,
        FlatLevel = 7
    }

    public class FindProperty
    {
        public string field_org;                   // название организации
        public int Combo_kind;                      // идентификатор вида документа
        public int Combo_tema;                      // идентификатор темы документа
        public int Executor;                        // идентификатор исполнителя
        public string field_autor;                 // сотрудник, подписавший документ
        public string OUT_cod;                     // исходящий номер
        public string find_cod;                    // РКК
        public DateTime OUT_date;   // дата исходящего
        public string FirstName;                   // имя автора
        public string Surname;                     // Отчество автора
        public string LastName;                    // Фамилия автора
        public string Text_Note;                   // Записки
        public string Text_ANNOT;                  // Аннотация
        public string Text_Content;                // Контент

        public FindProperty()
        {
            field_org = "";
            Combo_kind = 0;
            Combo_tema = 0;
            Executor = 0;
            field_autor = "";
            OUT_cod = "";
            find_cod = "";
            OUT_date = DateTime.MinValue;
            FirstName = "";
            Surname = "";
            LastName = "";
            Text_Note = "";
            Text_ANNOT = "";
            Text_Content = "";
        }
    }
    public static class DocEnumeration
    {
        public static class WellcomeDocs // Входящие документы
        {
            public const int Value = 1;
            public enum Filters
            {
                executor_docs = 1,                  // Все документы
                executor_docs_new,                  // Вновь поступившие
                executor_docs_send,                 // Назначенные
                executor_docs_send_exec,            // Исполненнеые
                executor_docs_exec,                 // Завершенные
                executor_docs_send_partly_exec,     // Частично исполненные
                executor_docs_new_repeat,           // Повторно назначенные
                executing_docs_alarm                // Просроченные
            }
        }
        public static class IndoorDosc     // Внутренние документы
        {
            public const int Value = 2;
            public enum Filters
            {
                exe_in_docs = 1,                  // Все документы
                exe_in_docs_new,                  // Вновь поступившие
                exe_in_docs_send,                 // Назначенные
                exe_in_docs_send_exec,            // Исполненнеые
                exe_in_docs_exec,                 // Завершенные
                exe_in_docs_send_partly_exec,     // Частично исполненные
                exe_in_docs_new_repeat,           // Повторно назначенные
                executing_in_docs_alarm           // Просроченные
            }
        }
        public static class OutDocs        // Исходящие документы
        {
            public const int Value = 3;
            public enum Filters
            {
                out_docs_new = 2,                  // Вновь поступившие
                out_docs_send = 4                 // Исполненнеые
            }
        }
        public static class DepartmentDocs // Документы отдела
        {
            public const int Value = 4;
        }
        public static class OwnDocs        // Свои документы
        {
            public const int Value = 5;
            public enum Filters
            {
                own_docs = 1,                   // Все документы
                own_docs_new,                   // Вновь поступившие
                own_docs_send,                  // Назначенные
                own_docs_send_exec,             // Исполненные
                own_docs_signing,               // На подписи
                own_docs_signed,                // Завизированные
                own_docs_not_signed,            // Не завизированные
                own_docs_alarm                  // Просроченные
            }
        }
        public static class NewsDocs       // Новости
        {
            public const int Value = 6;
            public enum Filters
            {
                executor_news = 1,            // Все документы
                executor_news_new = 2,            // Вновь поступившие
                executor_news_old = 4,            // Ознакомлены
                executor_news_alarm = 6             // Просроченные
            }
        }
        public static class VizingDocs     // Документы на визирование 
        {
            public const int Value = 7;
            public enum Filters
            {
                vizing_docs = 1,                  // Все документы
                vizing_docs_new,                  // Вновь поступившие
                vizing_docs_send,                 // Назначенные
                vizing_docs_send_exec,            // Исполненнеые
                vizing_docs_exec,                 // Завершенные
                vizing_docs_alarm = 8              // Просроченные
            }
        }
        public static class ArchiveDocs     // Архивные документы
        {
            public const int Value = 8;
        }
    }

    public static class DocWellEnumeration
    {
        public static class AllDocs // Входящие документы
        {
            public const int Value = 1;
        }
        public static class SendDosc     // Назначенные документы
        {
            public const int Value = 2;
        }
        public static class NewDosc     // Новые документы
        {
            public const int Value = 3;
        }
        public static class ArchiveDocs // Архив
        {
            public const int Value = 4;
        }
        public static class ExecDocs        // Исполненные документы
        {
            public const int Value = 5;
        }
        public static class PrintedDocs       // Отпечатанные документы
        {
            public const int Value = 6;
        }
        public static class OutDocs     // Исходящие документы  
        {
            public const int Value = 7;
        }
    }


}
