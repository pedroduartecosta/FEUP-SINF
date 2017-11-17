﻿using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace FirstREST.Controllers
{
    public class SalesController : Controller
    {
        public class SalesModel
        {
            public List<InvoiceModel> CompanyInvoices = new List<InvoiceModel>();
            public SalesInfoModel SalesInfo = new SalesInfoModel();
            public double averageTransactionPrice;
            public double sumTotalTaxes;
        }

        public class InvoiceModel
        {
            public string invoiceNo;
            public string invoiceStatus;
            public int period;
            public DateTime invoiceDate;
            public string invoiceType;
            public string customerID;
            public double grossTotal;
            public double netTotal;
            public double taxTotal;
        }

        public class SalesInfoModel
        {
            public double totalInvoiceDebit;
            public double totalInvoiceCredit;
        }


        // GET: /sales/
        public ActionResult Index()
        {
            DataSet invoiceTable = new DataSet();
            SalesModel SalesDashboardModel = new SalesModel();

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Invoice", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(invoiceTable, "Invoices");

                        foreach (DataRow row in invoiceTable.Tables["Invoices"].Rows)
                        {
                            InvoiceModel temp_invoice = new InvoiceModel();
                            temp_invoice.invoiceNo = row.Field<string>("invoiceNo");
                            temp_invoice.invoiceStatus = row.Field<string>("invoiceStatus");
                            temp_invoice.period = row.Field<int>("period");
                            temp_invoice.invoiceDate = row.Field<DateTime>("invoiceDate");
                            temp_invoice.invoiceType = row.Field<string>("invoiceType");
                            temp_invoice.customerID = row.Field<string>("customerID");
                            temp_invoice.grossTotal = row.Field<double>("grossTotal");
                            temp_invoice.netTotal = row.Field<double>("netTotal");
                            temp_invoice.taxTotal = row.Field<double>("taxTotal");
                            SalesDashboardModel.CompanyInvoices.Add(temp_invoice);
                        }

                    }
                }
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Sales", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(invoiceTable, "sales");
                        SalesInfoModel temp = new SalesInfoModel();
                        temp.totalInvoiceCredit = invoiceTable.Tables["sales"].Rows[0].Field<double>("InvoicesTotalCredit");
                        temp.totalInvoiceDebit = invoiceTable.Tables["sales"].Rows[0].Field<double>("InvoicesTotalDebit");
                        SalesDashboardModel.SalesInfo = temp;

                    }
                }
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select AVG(GrossTotal) as average From dbo.Invoice", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(invoiceTable, "Average");
                        SalesDashboardModel.averageTransactionPrice = invoiceTable.Tables["Average"].Rows[0].Field<double>("average");
                    }
                }
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select SUM(taxTotal) as sum From dbo.Invoice", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(invoiceTable, "Sum");
                        SalesDashboardModel.sumTotalTaxes = invoiceTable.Tables["Sum"].Rows[0].Field<double>("sum");
                    }
                }
            }

            return View(SalesDashboardModel);
        }
    }

}
