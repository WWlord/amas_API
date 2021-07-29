using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace ClassErrorProvider
{
    public class ErrorBBLProvider
    {
    
        public enum Modules {Master=1, Chief, Registration, Structure, Personel, WFL_Admin, Rights, Resources, OutputDoc}
        public delegate void ErrorBBLHandler(string ErrS, int Module, int Ident);
        public event ErrorBBLHandler ErrorOfBBL;

        public int ModuleId = 0;
        //private ErrorStrings Last_error = null;

        private class ErrorStrings
        {
            private string ErrorS;
            private string ModulS;
            private string ErrorDescription;

            public string ErrorLine
            {
                get { return ErrorS; }
            }

            public string ErrorModulS
            {
                get { return ModulS; }
            }
            public string ErrorDesc
            {
                get { return ErrorDescription; }
            }

            public ErrorStrings(string ErrS, string ModS, string EDS)
            {
                ErrorS = ErrS;
                ModulS = ModS;
                ErrorDescription = EDS;
            }
        }

        public string[] ErrorSList
        {
            
            get 
            {
                string[] sss = new string[ES.Count];
                int i = 0;
                foreach (ErrorStrings EE in ES)
                {
                    sss[i] = EE.ErrorLine;
                    i++;
                }
                return sss;
            }
        }

        private ArrayList ESus;

        public ArrayList ES
        {
            get { return ESus; }
        }

        public ErrorBBLProvider()
        {
            ESus = new ArrayList();
        }

        public void AddError(string ErrS, string ModS, string EDS)
        {
            if (ESus != null)
            {
                ErrorStrings EEE = new ErrorStrings(ErrS, ModS, EDS);
                ESus.Add(EEE);
                if (ErrorOfBBL != null)
                    ErrorOfBBL(ErrS, ModuleId, ESus.Count - 1);
                ModuleId = 0;
            }
        }

        public string Last_errorS()
        {
            ErrorStrings ee = (ErrorStrings)ES[ES.Count - 1];
            return ee.ErrorLine;
        }

        public string[] ErrorContent(int Ident)
        {
                string[] content = new string[3];
            try
            {
                ErrorStrings EEE = (ErrorStrings)ESus[Ident];
                content[0] = EEE.ErrorLine;
                content[1] = EEE.ErrorModulS;
                content[2] = EEE.ErrorDesc;
            }
            catch { content = null; }
            return content;
        }
    }
}
