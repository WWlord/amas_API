using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Net.Sockets;
using POPMailException;

namespace AMASControlRegisters
{
    public partial class GetMail : UserControl
    {
        int pozThis;

        public delegate void MailSelected(DateTime sended, string From, string Subject, ArrayList Attach);
        public event MailSelected MessagePicked;

        public string ServerSMTP
        {
            set { tbServer.Text = value; }
            get { return tbServer.Text; }
        }

        public string Password
        {
            set { tbPassword.Text = value; }
            get { return tbPassword.Text; }
        }

        public string UserName
        {
            set { tbName.Text = value; }
            get { return tbName.Text; }
        }

        POP3 oPOP;
        ArrayList MessageList;
        public GetMail()
        {
            InitializeComponent();

            try
            {
                lbMessages.Items.Clear();
                oPOP = new POP3();
            }
            catch 
            { }

            pozThis = this.Width;

            lbMessages.Click += new EventHandler(lbMessages_Click);
            this.Resize+=new EventHandler(GetMail_Resize);
        }

        private void GetMail_Resize(object sender, EventArgs e)
        {
            tbServer.Width+=this.Width-pozThis;
            tbName.Width += this.Width - pozThis;
            tbPassword.Width += this.Width - pozThis;
            pozThis = this.Width;
        }

        public  bool ResieveMail()
        {
            bool ret=true;
            try
            {
                if (!oPOP.Connected) oPOP.ConnectPOP(tbServer.Text, tbName.Text, tbPassword.Text);
                MessageList = oPOP.ListMessages();
                lbMessages.Items.Clear();
                foreach (POP3EmailMessage POPmsg in MessageList)
                {
                    POP3EmailMessage POPmsgContent = oPOP.RetrteveMessage(POPmsg);
                    lbMessages.Items.Add("Сообщение от " + POPmsgContent.From);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ret = false;
            }
            return ret;
        }

        private void lbMessages_Click(object sender, EventArgs e)
        {
            TouchMessage();
        }

        private void lbMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            TouchMessage();
        }

        private void TouchMessage()
        {
            POP3EmailMessage POPmsgContent=null;
            if (lbMessages.SelectedIndex >= 0)
            {
                POPmsgContent = (POP3EmailMessage)MessageList[lbMessages.SelectedIndex];
                if (POPmsgContent.msgReceived == false)
                    POPmsgContent = oPOP.RetrteveMessage(POPmsgContent);
                if (POPmsgContent.msgContent != null)
                {
                }
            }
            if (POPmsgContent!=null)
                MessagePicked(POPmsgContent.SendDate, POPmsgContent.From, POPmsgContent.Subject, POPmsgContent.MailAttachs);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResieveMail();
        }
    }

    public class POP3EmailMessage
    {
        public long msgNumber;
        public long msgSize;
        public string msgContent;
        public ArrayList MailAttachs=new ArrayList();
        public enum services { POP3 = 1, SMTP }
        public enum EnCoding { d7bit = 1, d8bit, base64, binary, quoted_printable, unknown }
        public enum MailPriority { Normal = 1, Other }
        public enum ContentType { multipart = 1, text, image, application, audio, message, video, xtoken, unknown }
        public enum ContentSubType { mixed = 1, digest, parallel, patrial, plain, jpeg, audio, unknown }

        private bool msgReceived_;
        private services ServerType_ = services.POP3;
        private string Received_;
        private DateTime SendDate_;
        private string Message_ID_;
        private string From_ = "";
        private string To_ = "";
        private string Subject_;
        private ContentType Content_Type_;
        private ContentSubType Content_Subtype_;
        private string ContentBoundary_;
        private string Content_format_;
        private string Content_charset_;
        private string Content_reply_type_;
        private EnCoding Content_Transfer_Encoding_;
        private int XPriority_;
        private MailPriority X_MSMail_Priority_;
        private DateTime X_OriginalArrivalTime_;

        public string TextMessage;

        public services ServerType { get { return ServerType_; } }
        public string From { get { return From_; } }
        public string Subject { get { return Subject_; } }
        public DateTime SendDate { get { return SendDate_; } }

        public bool msgReceived
        {
            get { return msgReceived_; }
            set
            {
                bool Res = msgReceived_;
                msgReceived_ = value;
                if (msgReceived_ && msgReceived_ != Res && msgContent != null)
                {
                    string lastContent = msgContent;
                    {
                        if (lastContent.Substring(0, "Received:".Length).CompareTo("Received:") == 0)
                        {
                            int len = lastContent.IndexOf("Message-ID:");
                            Received_ = WithoutReturn(lastContent.Substring("Received:".Length, len - "Received:".Length));
                            lastContent = lastContent.Substring(len < 0 ? 0: len);
                        }
                        if (lastContent.Substring(0, "Message-ID:".Length).CompareTo("Message-ID:") == 0)
                        {
                            int len = lastContent.IndexOf("From:");
                            Message_ID_ = WithoutReturn(lastContent.Substring("Message-ID:".Length, len - "Message-ID:".Length));
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "From:".Length).CompareTo("From:") == 0)
                        {
                            int len = lastContent.IndexOf("To:");
                            From_ = WithoutReturn(lastContent.Substring("From:".Length, len - "From:".Length));
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "To:".Length).CompareTo("To:") == 0)
                        {
                            int len = lastContent.IndexOf("Subject:");
                            To_ = WithoutReturn(lastContent.Substring("To:".Length, len - "To:".Length));
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "Subject:".Length).CompareTo("Subject:") == 0)
                        {
                            int len = lastContent.IndexOf("Date:");
                            Subject_ = WithoutReturn(lastContent.Substring("Subject:".Length, len - "Subject:".Length));
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "Date:".Length).CompareTo("Date:") == 0)
                        {
                            int len = lastContent.IndexOf("MIME-Version:");
                            try
                            {
                                SendDate_ = Convert.ToDateTime(WithoutReturn(lastContent.Substring("Date:".Length, len - "Date:".Length)));
                            }
                            catch { }
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "MIME-Version:".Length).CompareTo("MIME-Version:") == 0)
                        {
                            int len = lastContent.IndexOf("Content-Type:");
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "Content-Type:".Length).CompareTo("Content-Type:") == 0)
                        {
                            int len = lastContent.IndexOf("\r\n");
                            string ct = lastContent.Substring("Content-Type:".Length, len - "Content-Type:".Length);
                            string sct;
                            try
                            {
                                sct = ct.Substring(ct.IndexOf("/") + 1, ct.IndexOf(";")).Trim();
                            }
                            catch { sct = ""; };
                            try
                            {
                            ct = ct.Substring(0, ct.IndexOf("/")).Trim();
                            }
                            catch {ct="";}
                            switch (ct.ToLower())
                            {
                                case "text":
                                    Content_Type_ = ContentType.text;
                                    len = lastContent.IndexOf("format=");
                                    break;
                                case "multipart":
                                    Content_Type_ = ContentType.multipart;
                                    lastContent = lastContent.Substring(lastContent.IndexOf("boundary=\"") + "boundary=\"".Length);
                                    ContentBoundary_ = lastContent.Substring(0, lastContent.IndexOf("\"")).Trim();
                                    len = lastContent.IndexOf("X-Priority");
                                    break;
                                case "image":
                                    Content_Type_ = ContentType.image;
                                    break;
                                case "application":
                                    Content_Type_ = ContentType.application;
                                    break;
                                case "audio":
                                    Content_Type_ = ContentType.audio;
                                    break;
                                case "message":
                                    Content_Type_ = ContentType.message;
                                    break;
                                case "video":
                                    Content_Type_ = ContentType.video;
                                    break;
                                case "x-token":
                                    Content_Type_ = ContentType.xtoken;
                                    break;
                                default:
                                    Content_Type_ = ContentType.unknown;
                                    break;
                            }
                                                        
                            switch (sct.ToLower())
                            {
                                case "mixed":
                                    Content_Subtype_=ContentSubType.mixed;
                                    break;
                                case "digest":
                                    Content_Subtype_=ContentSubType.digest;
                                    break;
                                case "parallel":
                                    Content_Subtype_=ContentSubType.parallel;
                                    break;
                                case "patrial":
                                    Content_Subtype_=ContentSubType.patrial;
                                    break;
                                case "plain":
                                    Content_Subtype_=ContentSubType.plain;
                                    break;
                                case "jpeg":
                                    Content_Subtype_=ContentSubType.jpeg;
                                    break;
                                case "audio":
                                    Content_Subtype_=ContentSubType.audio;
                                    break;
                                default:
                                    Content_Subtype_=ContentSubType.unknown;
                                    break;

                            }
                            lastContent = lastContent.Substring(len);
                        }
                        if (lastContent.Substring(0, "format=".Length).CompareTo("format=") == 0)
                        {
                            int len = lastContent.IndexOf("charset=");
                            Content_format_ = WithoutReturn(lastContent.Substring("format=".Length, len - "format=".Length));
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "charset=".Length).CompareTo("charset=") == 0)
                        {
                            int len = lastContent.IndexOf("reply-type=");
                            Content_charset_ = WithoutReturn(lastContent.Substring("charset=".Length, len - "charset=".Length));
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "reply-type=".Length).CompareTo("reply-type=") == 0)
                        {
                            int len = lastContent.IndexOf("Content-Transfer-Encoding:");
                            Content_reply_type_ = WithoutReturn(lastContent.Substring("reply-type=".Length, len - "reply-type=".Length));
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "Content-Transfer-Encoding:".Length).CompareTo("Content-Transfer-Encoding:") == 0)
                        {
                            int len = lastContent.IndexOf("X-Priority:");
                            string ecd = WithoutReturn(lastContent.Substring("Content-Transfer-Encoding:".Length, len - "Content-Transfer-Encoding:".Length));
                            Content_Transfer_Encoding_ = EnCodingCod(ecd);
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "X-Priority:".Length).CompareTo("X-Priority:") == 0)
                        {
                            int len = lastContent.IndexOf("X-MSMail-Priority:");
                            try
                            {
                                XPriority_ = Convert.ToInt32(WithoutReturn(lastContent.Substring("X-Priority:".Length, len - "X-Priority:".Length)));
                            }
                            catch { XPriority_ = -1; }
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "X-MSMail-Priority:".Length).CompareTo("X-MSMail-Priority:") == 0)
                        {
                            int len = lastContent.IndexOf("X-Mailer:");
                            string mp = WithoutReturn(lastContent.Substring("X-MSMail-Priority:".Length, len - "X-MSMail-Priority:".Length));
                            switch (mp.ToLower())
                            {
                                case " normal":
                                    X_MSMail_Priority_ = MailPriority.Normal;
                                    break;
                                default:
                                    X_MSMail_Priority_ = MailPriority.Other;
                                    break;
                            }
                            lastContent = lastContent.Substring(len);
                        }
                        if (lastContent.Substring(0, "X-Mailer:".Length).CompareTo("X-Mailer:") == 0)
                        {
                            int len;
                            try { len = lastContent.IndexOf("X-MimeOLE:"); }
                            catch { len = 0; }
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "X-MimeOLE:".Length).CompareTo("X-MimeOLE:") == 0)
                        {
                            int len = lastContent.IndexOf("Return-Path:");
                            lastContent = lastContent.Substring(len < 0 ? 0 : len);
                        }
                        if (lastContent.Substring(0, "Return-Path:".Length).CompareTo("Return-Path:") == 0)
                        {
                            int len;

                            try { len = lastContent.IndexOf("X-OriginalArrivalTime:"); }
                            catch { len = 0; }
                            lastContent = lastContent.Substring(len <0 ? 0:len);
                        }
                        if (lastContent.Substring(0, "X-OriginalArrivalTime:".Length).CompareTo("X-OriginalArrivalTime:") == 0)
                        {
                            int len = lastContent.IndexOf("\r\n");
                            try
                            {
                                X_OriginalArrivalTime_ = Convert.ToDateTime(WithoutReturn(lastContent.Substring("X-OriginalArrivalTime:".Length, len - "X-OriginalArrivalTime::".Length)));
                            }
                            catch { }
                            lastContent = lastContent.Substring((len < 0 ? 0 : len) + "\r\n".Length);
                        }
                        if (lastContent.Length > 0)
                        {
                            string pat="This is a multi-part message in MIME format.";
                            int len = lastContent.IndexOf(pat);
                            if (len > 0)
                            {
                                lastContent = lastContent.Substring(len + pat.Length);
                                switch (Content_Type_)
                                {
                                    case ContentType.text:
                                        TextMessage = lastContent;
                                        break;
                                    case ContentType.multipart:
                                        MailAttachProperty MAP;
                                        int indexxx = lastContent.IndexOf(ContentBoundary_);
                                        string str;
                                        lastContent = EriseLeft(lastContent.Substring(indexxx + ContentBoundary_.Length));
                                        while (indexxx >= 0)
                                        {
                                            if (lastContent.Substring(0, "Content-Type:".Length).CompareTo("Content-Type:") == 0)
                                            {
                                                len = lastContent.IndexOf("\r\n");
                                                string ct = lastContent.Substring("Content-Type:".Length, len - "Content-Type:".Length);
                                                ct = ct.Substring(0, ct.IndexOf("/")).Trim();
                                                switch (ct.ToLower())
                                                {
                                                    case "text":
                                                        Content_Type_ = ContentType.text;
                                                        len = lastContent.IndexOf("format=");
                                                        break;
                                                    case "multipart":
                                                        Content_Type_ = ContentType.multipart;
                                                        lastContent = lastContent.Substring(lastContent.IndexOf("boundary=\"") + "boundary=\"".Length);
                                                        ContentBoundary_ = lastContent.Substring(0, lastContent.IndexOf("\"")).Trim();
                                                        len = lastContent.IndexOf("X-Priority");
                                                        break;
                                                    case "image":
                                                        Content_Type_ = ContentType.image;
                                                        break;
                                                    default:
                                                        Content_Type_ = ContentType.unknown;
                                                        break;
                                                }
                                                lastContent = EriseLeft(lastContent.Substring(len < 0 ? 0 : len));
                                            }
                                            if (lastContent.Substring(0, "format=".Length).CompareTo("format=") == 0)
                                            {
                                                len = lastContent.IndexOf("\r\n");
                                                Content_format_ = EriseLeft(EriseRight(lastContent.Substring("format=".Length, len - "format=".Length)));

                                                lastContent = EriseLeft(lastContent.Substring(len < 0 ? 0 : len));
                                            }
                                            if (lastContent.Substring(0, "charset=".Length).CompareTo("charset=") == 0)
                                            {
                                                len = lastContent.IndexOf("\r\n");
                                                Content_charset_ = EriseLeft(EriseRight(lastContent.Substring("charset=".Length, (len < 0 ? 0 : len) - "charset=".Length)));

                                                lastContent = EriseLeft(lastContent.Substring(len < 0 ? 0 : len));
                                            }
                                            if (lastContent.Substring(0, "reply-type=".Length).CompareTo("reply-type=") == 0)
                                            {
                                                len = lastContent.IndexOf("\r\n");
                                                Content_reply_type_ = EriseLeft(EriseRight(lastContent.Substring("reply-type=".Length, (len < 0 ? 0 : len) - "reply-type=".Length)));

                                                lastContent = EriseLeft(lastContent.Substring(len < 0 ? 0 : len));
                                            }

                                            if (lastContent.Substring(0, "Content-Transfer-Encoding:".Length).CompareTo("Content-Transfer-Encoding:") == 0)
                                            {
                                                len = lastContent.IndexOf("\r\n");
                                                string ecd = EriseLeft(EriseRight(lastContent.Substring("Content-Transfer-Encoding:".Length, len - "Content-Transfer-Encoding:".Length)));
                                                Content_Transfer_Encoding_ = EnCodingCod(ecd);
                                                lastContent = EriseLeft(lastContent.Substring(len < 0 ? 0 : len));
                                            }

                                            string name_ = "";
                                            string filename_ = "";
                                            if (Content_Type_ == ContentType.image)
                                            {
                                                if (lastContent.Substring(0, "name=".Length).CompareTo("name=") == 0)
                                                {
                                                    len = lastContent.IndexOf("\r\n");
                                                    name_ = EriseLeft(EriseRight(lastContent.Substring("name=".Length, (len < 0 ? 0 : len) - "name=".Length)));
                                                    lastContent = EriseLeft(lastContent.Substring(len));
                                                }

                                                if (lastContent.Substring(0, "Content-Transfer-Encoding:".Length).CompareTo("Content-Transfer-Encoding:") == 0)
                                                {
                                                    len = lastContent.IndexOf("\r\n");
                                                    string ecd = EriseLeft(EriseRight(lastContent.Substring("Content-Transfer-Encoding:".Length, (len < 0 ? 0 : len) - "Content-Transfer-Encoding:".Length)));
                                                    Content_Transfer_Encoding_ = EnCodingCod(ecd);
                                                    lastContent = EriseLeft(lastContent.Substring(len < 0 ? 0 : len));
                                                }
                                                string IMG_Content_Disposition_ = "";
                                                if (lastContent.Substring(0, "Content-Disposition:".Length).CompareTo("Content-Disposition:") == 0)
                                                {
                                                    len = lastContent.IndexOf("\r\n");
                                                    IMG_Content_Disposition_ = EriseLeft(EriseRight(lastContent.Substring("Content-Disposition:".Length, len - "Content-Disposition:".Length)));
                                                    lastContent = EriseLeft(lastContent.Substring(len < 0 ? 0 : len));
                                                }
                                                if (lastContent.Substring(0, "filename=".Length).CompareTo("filename=") == 0)
                                                {
                                                    len = lastContent.IndexOf("\r\n");
                                                    filename_ = EriseLeft(EriseRight(lastContent.Substring("filename=".Length, (len < 0 ? 0 : len) - "filename=".Length)));
                                                    lastContent = EriseLeft(lastContent.Substring(len < 0 ? 0 : len));
                                                }

                                            }
                                            MAP = new MailAttachProperty(Content_Type_, Content_Subtype_, Content_format_, Content_charset_, Content_reply_type_,
                                                Content_Transfer_Encoding_, MailAttachProperty.Dicposition.attachment, filename_);
                                            indexxx = lastContent.IndexOf(ContentBoundary_);
                                            if (indexxx > 0)
                                                str = lastContent.Substring(0, indexxx);
                                            else str = lastContent;
                                            char[] spl ={ '\r', '\n' };
                                            MAP.AppendBody(str.Split(spl));
                                            MailAttachs.Add(MAP);
                                            indexxx = lastContent.IndexOf(ContentBoundary_);
                                            lastContent = EriseRight(EriseLeft(lastContent.Substring(indexxx + ContentBoundary_.Length)));
                                            if (lastContent.Length < ContentBoundary_.Length)
                                            {
                                                lastContent = "";
                                                indexxx = -1;
                                            }
                                        }
                                        break;
                                }
                            }
                            else 
                                TextMessage = lastContent;
                        }
                        lastContent = "";
                    }
                }
            }
        }

        private EnCoding EnCodingCod(string ecd)
        {
            EnCoding CTE = EnCoding.unknown;
            ecd.ToLower();
            if (ecd.IndexOf("7bit") >= 0)
                CTE = EnCoding.d7bit;
            if (ecd.IndexOf("8bit") >= 0)
                CTE = EnCoding.d8bit;
            if (ecd.IndexOf("base") >= 0)
                CTE = EnCoding.base64;
            if (ecd.IndexOf("quoted-printable") >= 0)
                CTE = EnCoding.quoted_printable;
            if (ecd.IndexOf("binary") >= 0)
                CTE = EnCoding.binary;
            return CTE;
        }
        
        private string Sqeesh = "\r\n\t\";";
        private string EriseLeft(string str)
        {
            bool exit =false;
            do
            {
                if(str.Length>0)
                    if (Sqeesh.IndexOf( str.Substring(0, 1))!=-1)
                    {
                        str = str.Substring(1);
                        exit = false;
                    }
                    else
                        exit = true;
            }
            while (!exit);
            return str;
        }

        private string EriseRight(string str)
        {
            bool exit = false;
            do
            {
                if (str.Length > 0)
                    if (Sqeesh.IndexOf(str.Substring(str.Length - 1, 1)) != -1)
                    {
                        str = str.Substring(0, str.Length - 1);
                        exit = false;
                    }
                    else
                        exit = true;
            }
            while (!exit);
            return str;
        }

        public class MailAttachProperty
        {
            public enum Dicposition { attachment = 1 }
            private ContentType Content_Type_;
            private ContentSubType Content_Subtype_;
            private string Content_format_;
            private string Content_charset_;
            private string Content_reply_type_;
            private EnCoding Content_Transfer_Encoding_;
            private Dicposition Content_Disposition_;
            private string filename_;
            private MemoryStream Body_;

            public MailAttachProperty(ContentType Content_Type, ContentSubType Content_Subtype,  string Content_format,
                string Content_charset, string Content_reply_type, EnCoding Content_Transfer_Encoding,
                Dicposition Content_Disposition, string filename)
            {
                try
                {
                    Content_Type_ = Content_Type;
                    Content_Subtype_ = Content_Subtype;
                    Content_format_ = Content_format;
                    Content_charset_ = Content_charset;
                    Content_reply_type_ = Content_reply_type;
                    Content_Transfer_Encoding_ = Content_Transfer_Encoding;
                    Content_Disposition_ = Content_Disposition;
                    filename_ = filename;
                    Body_ = null;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

            public MemoryStream AppendBody(string[] strim)
            {
                MemoryStream Bodys = new MemoryStream();
                byte[] bytes;
                char[] chars;
                    foreach (string str in strim)
                    {
                        try
                        {
                            if (Content_Transfer_Encoding_ != EnCoding.base64)
                            {
                                if (str.Length > 0)
                                    bytes = StringtoByte(str);
                                else
                                    bytes = StringtoByte("\r\n");
                            }
                            else
                            {
                                chars = str.ToCharArray();
                                bytes = Convert.FromBase64CharArray(chars, 0, chars.Length); //EncriptMail.Base64(str);
                            }
                            Bodys.Write(bytes, 0, bytes.Length);
                        }
                        catch
                        { }
                    }
                Body_ = Bodys;
                return Bodys;
            }

            private byte[] StringtoByte(string str)
            {
                char[] chars = str.ToCharArray();
                byte[] bytes = new byte[chars.Length];
                for (int i = 0; i < chars.Length; i++)
                    bytes[i] = Convert.ToByte(chars[i]);
                return bytes;
            }

        }

        private string WithoutReturn(string sSource)
        {
            string sResult = sSource.Substring(0, sSource.IndexOf("\r\n"));
            return sResult;
        }
    }

    public class POP3 : System.Net.Sockets.TcpClient
    {

        public void ConnectPOP(string sServerName, string sUserName, string sPassword)
        {
            string sMessage;
            string sResult;
            Connect(sServerName, 110);

            sResult = Response();
            if (sResult.Substring(0, 3).CompareTo("+OK") != 0)
                throw new POPException(sResult);

            sMessage = "USER " + sUserName + "\r\n";
            Write(sMessage);
            
            sResult = Response();
            if (sResult.Substring(0, 3).CompareTo("+OK") != 0)
                throw new POPException(sResult);

            sMessage = "PASS " + sPassword + "\r\n";
            Write(sMessage);

            sResult = Response();
            if (sResult.Substring(0, 3).CompareTo("+OK") != 0)
                throw new POPException(sResult);

        }

        public void DisconnectPOP()
        {
            string sMessage;
            string sResult;

            sMessage = "QUIT\r\n";
            Write(sMessage);

            sResult = Response();
            if (sResult.Substring(0, 3).CompareTo("+OK") != 0)
            {   
                sMessage = "QUIT\r\n";
                Write(sMessage);
                sResult = Response();
            }
            //if (sResult.Substring(0, 3).CompareTo("+OK") != 0)
                //throw new POPException(sResult);
        }

        public ArrayList ListMessages()
        {
            string sResult;

            ArrayList returnValue = new ArrayList();
            string sMessage="LIST\r\n";
            Write(sMessage);

            sResult = Response();
            if (sResult.Substring(0, 3).CompareTo("+OK") != 0)
                throw new POPException(sResult);

            while (true)
            {
                sResult = Response();

                if (sResult.CompareTo(".\r\n")==0)
                {
                    return returnValue;
                }
                else
                {
                    POP3EmailMessage oMailMessage = new POP3EmailMessage();

                    char[] sep = {' '};

                    string[] values = sResult.Split(sep);

                    oMailMessage.msgNumber = Int32.Parse(values[0]);
                    oMailMessage.msgSize = Int32.Parse(values[1]);
                    oMailMessage.msgReceived = false;
                    returnValue.Add(oMailMessage);
                    continue;
                }
            }
        }

        public POP3EmailMessage RetrteveMessage(POP3EmailMessage msgRETR)
        {
            string sMessage;
            string sResult;
            POP3EmailMessage oMailMessage = null;
            try
            {
                oMailMessage = new POP3EmailMessage();
                oMailMessage.msgSize = msgRETR.msgSize;
                oMailMessage.msgNumber = msgRETR.msgNumber;

                sMessage = "RETR " + msgRETR.msgNumber.ToString() + "\r\n";
                Write(sMessage);
                sResult = Response();
                if (sResult.Length != 0)
                {
                    if (sResult.Substring(0, 3).CompareTo("+OK") != 0)
                        throw new POPException(sResult);
                    try
                    {
                        while (true)
                        {
                            sResult = Response();
                            if (sResult.CompareTo(".\r\n") == 0)
                                break;
                            else
                                oMailMessage.msgContent += sResult;
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    oMailMessage.msgReceived = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
                return oMailMessage;
        }

        public void DeleteMessage(POP3EmailMessage msgDELE)
        {
            string sMessage;
            string sResult;

            sMessage = "DELE " + msgDELE.msgNumber + "\r\n";
            Write(sMessage);
            sResult = Response();
            if (sResult.Substring(0, 3).CompareTo("+OK") != 0)
                throw new POPException(sResult);
        }

        private void Write(string sMessage)
        {
            System.Text.ASCIIEncoding oEncodedData = new ASCIIEncoding();

            byte[] WriteBuffer = new byte[1024];
            WriteBuffer=oEncodedData.GetBytes(sMessage);

            NetworkStream NetStream = GetStream();
            NetStream.Write(WriteBuffer,0,WriteBuffer.Length);
        }

        private string Response()
        {
            System.Text.ASCIIEncoding oEncodedData = new ASCIIEncoding();
            byte[] ServerBuffer = new byte[1024];
            NetworkStream NetStream = GetStream();
            int count = 0;
            string ReturnValue;
            try
            {
                while (true)
                {
                    byte[] buff = new byte[2];
                    int bytes = NetStream.Read(buff, 0, 1);

                    if (bytes == 1)
                    {
                        ServerBuffer[count] = buff[0];
                        count++;
                        if (buff[0] == '\n')
                            break;
                    }
                    else
                        break;
                }
            ReturnValue = oEncodedData.GetString(ServerBuffer, 0, count);
            }
            catch
            {
                ReturnValue = "";
            }
            return ReturnValue;
        }


    }

    static class EncriptMail
    {
        public static byte[] Base64(string Cod)
        {
            byte[] decod= new byte[48];
            int bytescount = 0;
            byte charCod = 0;
            byte bufbyte = 0;
            char[] CodArray=Cod.ToCharArray();
            for (int count = 0; count < Cod.Length; count++)
            {
                switch (CodArray[count])
                {
                    case 'A': charCod = 0; break;
                    case 'B': charCod = 1; break;
                    case 'C': charCod = 2; break;
                    case 'D': charCod = 3; break;
                    case 'E': charCod = 4; break;
                    case 'F': charCod = 5; break;
                    case 'G': charCod = 6; break;
                    case 'H': charCod = 7; break;
                    case 'I': charCod = 8; break;
                    case 'J': charCod = 9; break;
                    case 'K': charCod = 10; break;
                    case 'L': charCod = 11; break;
                    case 'M': charCod = 12; break;
                    case 'N': charCod = 13; break;
                    case 'O': charCod = 14; break;
                    case 'P': charCod = 15; break;
                    case 'Q': charCod = 16; break;
                    case 'R': charCod = 17; break;
                    case 'S': charCod = 18; break;
                    case 'T': charCod = 19; break;
                    case 'U': charCod = 20; break;
                    case 'V': charCod = 21; break;
                    case 'W': charCod = 22; break;
                    case 'X': charCod = 23; break;
                    case 'Y': charCod = 24; break;
                    case 'Z': charCod = 25; break;
                    case 'a': charCod = 26; break;
                    case 'b': charCod = 27; break;
                    case 'c': charCod = 28; break;
                    case 'd': charCod = 29; break;
                    case 'e': charCod = 30; break;
                    case 'f': charCod = 31; break;
                    case 'g': charCod = 32; break;
                    case 'h': charCod = 33; break;
                    case 'i': charCod = 34; break;
                    case 'j': charCod = 35; break;
                    case 'k': charCod = 36; break;
                    case 'l': charCod = 37; break;
                    case 'm': charCod = 38; break;
                    case 'n': charCod = 39; break;
                    case 'o': charCod = 40; break;
                    case 'p': charCod = 41; break;
                    case 'q': charCod = 42; break;
                    case 'r': charCod = 43; break;
                    case 's': charCod = 44; break;
                    case 't': charCod = 45; break;
                    case 'u': charCod = 46; break;
                    case 'v': charCod = 47; break;
                    case 'w': charCod = 48; break;
                    case 'x': charCod = 49; break;
                    case 'y': charCod = 50; break;
                    case 'z': charCod = 51; break;
                    case '0': charCod = 52; break;
                    case '1': charCod = 53; break;
                    case '2': charCod = 54; break;
                    case '3': charCod = 55; break;
                    case '4': charCod = 56; break;
                    case '5': charCod = 57; break;
                    case '6': charCod = 58; break;
                    case '7': charCod = 59; break;
                    case '8': charCod = 60; break;
                    case '9': charCod = 61; break;
                    case '+': charCod = 62; break;
                    case '/': charCod = 63; break;
                    case '=': charCod = 255; break; //(pad)
                }
                if(charCod<64)
                    switch (count & 3)
                    {
                        case 0:
                            if (bytescount > 0) bytescount++;
                            decod[bytescount] = charCod;
                            break;
                        case 1:
                            //bufbyte = (charCod & 3) >> 6;
                            //decod[bytescount] = decod[bytescount] | bufbyte;
                            break;
                        case 2:
                            //decod[++bytescount] += ((bufbyte & 252) << (byte)2) + (charCod & (byte)15) >> 4;
                            break;
                        case 3:
                            //decod[++bytescount] += ((bufbyte << 4) & (byte)3) + (charCod >> 2);
                            break;
                    }
                bufbyte = charCod;
            }

            return decod;
        }
    }
}
namespace POPMailException
{
    public class POPException : System.ApplicationException
    {
        public POPException(string Str)
        {
        }
    }
}
