﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class ManagementController : Controller
    {

        public class ManagementModel
        {
            public List<EmployeeModel> employees = new List<EmployeeModel>();
            public double averageEmployeesSalesMonth;
        }

        public class EmployeeModel
        {
            public int id;
            public string name;
            public Double moneyMade;
        }

        // GET: /Management/
        public ActionResult Index()
        {
            DataSet employeesTable = new DataSet();
            ManagementModel ManagementModel = new ManagementModel();

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Employee", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(employeesTable, "Employees");

                        foreach (DataRow row in employeesTable.Tables["Employees"].Rows)
                        {
                            EmployeeModel temp_employee = new EmployeeModel();
                            temp_employee.id = row.Field<int>("id");
                            temp_employee.name = row.Field<string>("name");
                            temp_employee.moneyMade = row.Field<Double>("moneyMade");
                            ManagementModel.employees.Add(temp_employee);

                           
                        }

                    }
                }
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select AVG(moneyMade) as average From dbo.Employee", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(employeesTable, "Average");
                        ManagementModel.averageEmployeesSalesMonth = employeesTable.Tables["Average"].Rows[0].Field<double>("average");

                    }
                }
            }

             return View(ManagementModel);
        }
    }
}
