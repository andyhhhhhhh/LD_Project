using System.Data;

namespace JsonController
{
    public enum AccessLevel//用戶權限劃分
    {
        None = 0,   // Order is important!
        Operator,
        //Techician,
        Engineer,
        Admin,
        SuperuserZH

    }

    public class AccessUser
    {
        #region 私有成員

        private int _userId;
        private string _userName;
        private string _userPwd;

        private AccessLevel _access;


        private bool _exist;		//是否存在标志

        #endregion 私有成員

        #region 屬性
        public int UserId
        {
            set
            {
                _userId = value;
            }
            get
            {
                return _userId;
            }
        }

        public string UserName
        {
            set
            {
                _userName = value;
            }
            get
            {
                return _userName;
            }
        }
        public string UserPwd
        {
            set
            {
                _userPwd = value;
            }
            get
            {
                return _userPwd;
            }
        }

        public AccessLevel Access
        {
            set
            {
                _access = value;
            }
            get
            {
                return _access;
            }
        }

        public bool Exist
        {
            get
            {
                return _exist;
            }
        }

        #endregion 屬性

        #region 方法
        public AccessUser()
        {
            _userId = 0;
            _userName = "";
            _userPwd = "";

            _access = AccessLevel.None;

            _exist = false;		//是否存在标志
        }

        /// <summary>
        /// 根据参数UserName，获取用户详细信息
        /// </summary>
        /// <param name="userName">用户名</param>
        public void LoadData(string userName)
        {
            var db = new AccessDatabase();		//实例化一个myDatabase类
            string sql = string.Format("SELECT ID,UserName,PWD,AccessLevel FROM SystemUser WHERE UserName='{0}'", userName);

            DataRow dr = db.GetDataRow(sql);	//利用myDatabase类的GetDataRow方法查询用户数据

            //根据查询得到的数据，对成员赋值
            if (dr != null)
            {
                _userId = GetSafeData.ValidateDataRowN(dr, "ID");
                _userName = GetSafeData.ValidateDataRowS(dr, "UserName");
                _userPwd = GetSafeData.ValidateDataRowS(dr, "PWD");

                //获取权限集合
                _access = (AccessLevel)GetSafeData.ValidateDataRowN(dr, "AccessLevel");

                _exist = true;
            }
            else
            {
                _exist = false;
            }
            db.Dispose();
        }

        /// <summary>
        /// 判断是否存在登录名为UserName的用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>如果存在，返回true；否则，返回false</returns>
        public bool HasUser(string userName)
        {
            var db = new AccessDatabase();

            string sql = string.Format("SELECT ID,UserName,PWD,AccessLevel FROM SystemUser WHERE (UserName='{0}') ", userName);

            DataRow row = db.GetDataRow(sql);

            db.Dispose();

            return row != null;
        }

        /// <summary>
        /// 修改用戶密碼
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public int ChangePassword(string userName, string userPwd)
        {
            string sql = string.Format("UPDATE SystemUser SET PWD='{0}' WHERE UserName='{1}'", userPwd, userName);

            var db = new AccessDatabase();

            int r=db.ExecuteSql(sql);

            db.Dispose();

            return r;
        }
        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public int AddUser(AccessUser newUser)
        {
            string sql = string.Format("INSERT INTO SystemUser(ID,UserName,PWD) VALUES ({0},{1},{2})", newUser.UserId, newUser.UserName, newUser._userPwd);

            var db = new AccessDatabase();

            int r=db.ExecuteSql(sql);

            db.Dispose();

            return r;
        }
        /// <summary>
        /// 刪除用戶
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int DeleteUser(string userName)
        {
            string sql = string.Format("DELETE FROM SystemUser WHERE UserName='{0}'", userName);

            var db = new AccessDatabase();

            int r=db.ExecuteSql(sql);

            db.Dispose();

            return r;
        }


        public void LoadAllUserNames()
        {
           
        }
        #endregion 方法
    }
}