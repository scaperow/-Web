using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
    /// <summary>
    /// sys_line:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class sys_line
    {
        public sys_line()
        { }
        #region Model
        private Guid _id;
        private string _linename;
        private string _description;
        private string _ipaddress;
        private string _port;
        private string _datasourceaddress;
        private string _username = "";
        private string _password = "";
        private string _databasename = "";
        private int _startupload = 0;
        private string _uploadaddress = "";
        private string _testroomcodemap = "";
        private string _jsdwcode = "";
        private int _isactive = 1;
        private string _domain;
        /// <summary>
        /// 
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LineName
        {
            set { _linename = value; }
            get { return _linename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IPAddress
        {
            set { _ipaddress = value; }
            get { return _ipaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Port
        {
            set { _port = value; }
            get { return _port; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DataSourceAddress
        {
            set { _datasourceaddress = value; }
            get { return _datasourceaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PassWord
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DataBaseName
        {
            set { _databasename = value; }
            get { return _databasename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StartUpload
        {
            set { _startupload = value; }
            get { return _startupload; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UploadAddress
        {
            set { _uploadaddress = value; }
            get { return _uploadaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TestRoomCodeMap
        {
            set { _testroomcodemap = value; }
            get { return _testroomcodemap; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string JSDWCode
        {
            set { _jsdwcode = value; }
            get { return _jsdwcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsActive
        {
            set { _isactive = value; }
            get { return _isactive; }
        }
        public string Domain
        {
            set { _domain = value; }
            get { return _domain; }
        }

        #endregion Model

    }
}
