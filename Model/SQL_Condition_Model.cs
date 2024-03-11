using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingOrderCOAFilter.Model
{
    public class SQL_Condition_Model
    {
        public string ConnectionStringOfShippingOrder()
        {
            return "Data Source=YourIP;Initial Catalog=YourDB;User ID=YourID;Password=YourPassword";
        }
        public string ShippingOrder_DB_SQL_Condition()
        {
            return @"   SELECT YourColumn
                        from YourTable
                        where YourCondition";
        }
        public string ShippingOrder_DB_CustomerEName_SQL_Condition()
        {
            return @"   SELECT YourColumn
                        from YourTable
                        where YourCondition";
        }
        public string ConnectionStringOfCOA()
        {
            return "User Id=YourID;Password=YourPassWord;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=YourIP)(PORT=YourPort)))(CONNECT_DATA=(SERVICE_NAME=YourDB)))";
        }
        // 針對 Pass 和 Fail 的要分開來，因為有些客戶不良品還是要出貨
        // 因此出貨單也會有不良品出貨的單、也需要比較
        public string COA_DB_SQL_Condition_Pass()
        {
            return @"   SELECT YourColumn
                        from YourTable
                        where YourCondition";
        }
        public string COA_DB_SQL_Condition_Fail()
        {
            return @"   SELECT YourColumn
                        from YourTable
                        where YourCondition";
        }
    }
}
