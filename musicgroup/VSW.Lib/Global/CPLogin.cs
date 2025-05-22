using VSW.Core.Global;
using VSW.Lib.Models;

namespace VSW.Lib.Global
{
    public static class CPLogin
    {
        public static void Logout()
        {
            Cookies.Remove("CP.UserID");
        }

        public static bool CheckLogin(string loginName, string password)
        {
            var user = CPUserService.Instance.GetLogin(loginName, password);

            if (user == null) return false;
            SetLogin(user.ID);

            return true;
        }

        private static void SetLogin(int userId)
        {
            Cookies.SetValue("CP.UserID", userId.ToString(), 0, true);
        }

        public static bool IsLogin()
        {
            return (UserID > 0);
            //return (CurrentUser != null);
        }

        public static int UserID
        {
            get
            {
                var userId = Convert.ToInt(Cookies.GetValue("CP.UserID", true));

                if (userId > 0)
                    SetLogin(userId);

                return userId;
            }
        }

        public static CPUserEntity CurrentUser => UserID > 0 ? CPUserService.Instance.GetLogin(UserID) : null;
    }
}