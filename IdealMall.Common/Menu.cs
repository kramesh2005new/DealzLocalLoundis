using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Configuration;




namespace IdealMall.Common
{
    public class Menu
    {
        public List<Department> GetMenuDetails(string excelPath)
        {
            List<Department> lstDepartment = new List<Department>();
            DataSet DtSet = null;
            int tableCount = 0;
            int table1RowCount = 0;
            int table2RowCount = 0;
            string currDepartment = string.Empty;
            string oldDepartment = string.Empty;
            string currCategory = string.Empty;
            string oldCategory = string.Empty;

            OleDbConnection MyConnection = null;

            OleDbDataAdapter MyCommand;
            // need to pass relative path after deploying on server
//string excelPath = Server.MapPath("~/Content/CategoryMenu/MenuProducts.xls");
           // string excelPath = @"C:\Users\karthicksundaram\Desktop\DealzLocalService\DealzLocalService\DealzLocalService\Content\CategoryMenu\MenuProducts.xls";

            /* connection string  to work with excel file. HDR=Yes - indicates 
   that the first row contains columnnames, not data. HDR=No - indicates 
   the opposite. "IMEX=1;" tells the driver to always read "intermixed" 
   (numbers, dates, strings etc) data columns as text. 
Note that this option might affect excel sheet write access negative. */

            if (Path.GetExtension(excelPath) == ".xls")
            {
                MyConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
            }
            else if (Path.GetExtension(excelPath) == ".xlsx")
            {
                MyConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelPath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
            }


            //MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='c:\\csharp.net-informations.xls';Extended Properties=Excel 8.0;");
            MyCommand = new OleDbDataAdapter("select * from [Department1$]", MyConnection);
            //MyCommand.TableMappings.Add("Table1", "TestTable1");
            DtSet = new System.Data.DataSet();
            MyCommand.Fill(DtSet, "Department1");

            MyCommand = new OleDbDataAdapter("select * from [Department2$]", MyConnection);
            //MyCommand.TableMappings.Add("Table2", "TestTable2");
            MyCommand.Fill(DtSet, "Department2");

            tableCount = DtSet.Tables.Count;
            if (tableCount > 0)
            {
                if (DtSet.Tables[0].Rows != null)
                {
                    table1RowCount = DtSet.Tables[0].Rows.Count;
                }
                if (tableCount > 1)
                {
                    if (DtSet.Tables[1].Rows != null)
                    {
                        table2RowCount = DtSet.Tables[1].Rows.Count;
                    }
                }
            }
            Department objDepartment = null;
            category objCategory = null;
            List<category> lstCategory = null;
            DataRow[] result = null;
            //sheet1
            foreach (DataRow dr in DtSet.Tables[0].Rows)
            {
                currDepartment = dr[0].ToString();
                if (currDepartment != oldDepartment)
                {
                    objDepartment = new Department();
                    objDepartment.DepartmentName = Convert.ToString(dr[0]);
                    string filter = "Department = '" + objDepartment.DepartmentName + "'";
                    result = DtSet.Tables[0].Select(filter);
                    lstCategory = new List<category>();
                    foreach (DataRow dr_Category in result)
                    {
                        objCategory = new category();
                        objCategory.CategoryName = Convert.ToString(dr_Category[1]);
                        lstCategory.Add(objCategory);
                    }
                    objDepartment.lstCategory = lstCategory;
                    lstDepartment.Add(objDepartment);
                }
                oldDepartment = currDepartment;
            }
            //sheet2
            foreach (DataRow dr in DtSet.Tables[1].Rows)
            {
                currDepartment = dr[0].ToString();
                if (currDepartment != oldDepartment)
                {
                    objDepartment = new Department();
                    objDepartment.DepartmentName = Convert.ToString(dr[0]);
                    string filter = "Department = '" + objDepartment.DepartmentName + "'";
                    result = DtSet.Tables[1].Select(filter);
                    lstCategory = new List<category>();
                    foreach (DataRow dr_Category in result)
                    {
                        objCategory = new category();
                        objCategory.CategoryName = Convert.ToString(dr_Category[1]);
                        lstCategory.Add(objCategory);
                    }
                    objDepartment.lstCategory = lstCategory;
                    lstDepartment.Add(objDepartment);
                }
                oldDepartment = currDepartment;
            }

            MyConnection.Close();

            return lstDepartment;

        }


    }
    public class Department
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<category> lstCategory { get; set; }
    }

    public class category
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
