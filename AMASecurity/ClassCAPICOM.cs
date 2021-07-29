using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AMASecurity
{
    class ClassCAPICOM
    {
        private void Signfile(string InputFileName, string OutputFileName)
        {
            string c;
            string s;
            CAPICOM.StoreClass MyStore = new CAPICOM.StoreClass();
            CAPICOM.SignedDataClass Signobj = new CAPICOM.SignedDataClass();
            CAPICOM.SignerClass Signer = new CAPICOM.SignerClass();

            // NOTE: the name 'Attribute' is not a unique name
            // and must be preceded by 'CAPICOM.'
            CAPICOM.AttributeClass SigningTime = new CAPICOM.AttributeClass();

            // Open the MY store and retrieve the first certificate from the
            // Store. The signing operation will only work if this 
            // certificate is valid and has access to the signer's private key.
            MyStore.Open(CAPICOM.CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, "MY",
                CAPICOM.CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_ONLY);
            Signer.Certificate = (CAPICOM.Certificate)MyStore.IStore2_Certificates[1];

            // Open the input file and read the content to be signed from the file.
            StreamReader f1 = new StreamReader(InputFileName);
            c = f1.ReadToEnd();
            f1.Close();

            // Set the content to be signed.
            Signobj.Content = c;


            // Save the time the data was signed as a signer attribute. 
            SigningTime.Name = CAPICOM.CAPICOM_ATTRIBUTE.CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME;
            SigningTime.Value = DateTime.Now;
            Signer.AuthenticatedAttributes.Add(SigningTime);

            // Sign the content using the signer's private key.
            // The 'True' parameter indicates that the content signed is not
            // included in the signature string.
            s = Signobj.Sign(Signer, true, CAPICOM.CAPICOM_ENCODING_TYPE.CAPICOM_ENCODE_ANY);

            StreamWriter f2 = new StreamWriter(OutputFileName);
            f2.Write(s);
            f2.Close();

            string Text = "Signature done - Saved to file" + OutputFileName;
            Signobj = null;
            MyStore = null;
            Signer = null;
            SigningTime = null;
        }

        private void Create_XML_document()
        {


            //DataTable ss = new DataTable("Documents");

            //ss.Columns.Add("id", typeof(System.Int32));
            //ss.Columns["id"].AutoIncrement = true;
            //ss.Columns["id"].AllowDBNull = false;
            //ss.Columns["id"].AutoIncrementStep = -1;
            //ss.PrimaryKey = new DataColumn[] { ss.Columns["id"] };

            //ss.Columns.Add("number", typeof(System.Int32));
            //ss.Columns["number"].AllowDBNull = false;

            //ss.Columns.Add("typ", typeof(System.String));
            //ss.Columns["typ"].AllowDBNull = false;
            //ss.Columns["typ"].MaxLength = 18;

            //ss.Columns.Add("name", typeof(System.String));
            //ss.Columns["name"].AllowDBNull = false;
            //ss.Columns["name"].MaxLength = 255;

            //ss.Columns.Add("Memo", typeof(System.Byte[]));
            //ss.Columns["Memo"].AllowDBNull = false;

            //row.BeginEdit();
            //row.EndEdit();

        }

        private void VerifySig(string FileToVerify, string FileBase)
        {
            string sdContent;
            string sdCheck;
            CAPICOM.SignedDataClass mySD = new CAPICOM.SignedDataClass();
            // Open a file and read the signature.
            StreamReader f1 = new StreamReader(FileToVerify);
            sdCheck = f1.ReadToEnd();
            f1.Close();
            // Open a file and input the plaintext content that was signed.
            StreamReader f2 = new StreamReader(FileBase);
            sdContent = f2.ReadToEnd();
            f2.Close();
            // Set the detached content upon which the signature is based.
            mySD.Content = sdContent;
            // Verify the detached signature.
            string msg = "";
            try
            {
                mySD.Verify(sdCheck, true, CAPICOM.CAPICOM_SIGNED_DATA_VERIFY_FLAG.CAPICOM_VERIFY_SIGNATURE_ONLY);
            }
            catch (Exception ex) { msg = ex.Message; }
            if (msg.Length > 0)
                msg += "Signature verification failed. " + msg;
            else
                msg += "Verification complete.";
            // Release the SignedData object.
            mySD = null;
        }

    }
}
