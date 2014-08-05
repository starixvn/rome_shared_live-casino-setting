namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using System;
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log data base class.
    /// </summary>
    internal class LogDataBase : ILogData
    {
        protected const string TrackingSubAcc = "SubAccId=\"{0}\" SubAccName=\"{1}\"";

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string Language { get; set; }

        public int CustId { get; set; }

        public string SubAccName { get; set; }

        public int SubAccId { get; set; }

        public LogActions Action { get; set; }

        public int ActionId
        {
            get { return Convert.ToInt32(this.Action); }
        }

        public virtual bool IsValid
        {
            get { return true; }
        }

        public virtual string SpName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual string NewValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual string OldValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string SignUpIp
        {
            get;
            set;
        }

        public string CopiedFrom
        {
            get;
            set;
        }
    }
}