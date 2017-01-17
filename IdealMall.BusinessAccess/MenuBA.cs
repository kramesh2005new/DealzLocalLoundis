using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdealMall.Common;

namespace IdealMall.BusinessAccess
{
    public static class MenuBA
    {
        public static List<Department> GetMenuDetails(string excelPath)
        {
            List<Department> lstDepartment = null;

            lstDepartment = new List<Department>();
            Menu objMenu = new Menu();
            lstDepartment = objMenu.GetMenuDetails(excelPath);

            return lstDepartment;
        }
    }
}
