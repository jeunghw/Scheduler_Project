using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class CommData
    {
        List<LoginData> loginDataList;
        MemberManager memberManager = new MemberManager();
        static CommData commData = new CommData();

        private CommData()
        {
            setLoginDataList();
        }

        public static CommData GetCommData()
        {
            return commData;
        }

        public void setLoginDataList()
        {
            loginDataList = memberManager.SelectName();
        }

        public List<LoginData> getLoginDataList()
        {
            return loginDataList;
        }
    }
}
