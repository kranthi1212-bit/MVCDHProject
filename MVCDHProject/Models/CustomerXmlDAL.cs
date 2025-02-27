﻿using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Data;

namespace MVCDHProject.Models
{
    public class CustomerXmlDAL:ICustomerDAL
    {
        DataSet ds;
        public CustomerXmlDAL() 
        {
            ds = new DataSet();
            ds.ReadXml("Customer.Xml");
            //Adding Primary Key on Custid of DataTable
            ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["Custid"] };
        }
        public List<Customer> Customers_Select()
        {
            List<Customer> customers=new List<Customer>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                Customer obj = new Customer
                {
                    Custid = Convert.ToInt32(dr["Custid"]),
                    Name = Convert.ToString(dr["Name"]),
                    Balance = Convert.ToDouble(dr["Balance"]),
                    City = Convert.ToString(dr["City"]),
                    Status = Convert.ToBoolean(dr["Status"])
                };
                customers.Add(obj);
            }
            return customers;
        }
        public Customer Customer_Select(int Custid)
        {
            DataRow dr = ds.Tables[0].Rows.Find(Custid);
            Customer obj = new Customer
            {
                Custid = Convert.ToInt32(dr["Custid"]),
                Name = Convert.ToString(dr["Name"]),
                Balance = Convert.ToDouble(dr["Balance"]),
                City = Convert.ToString(dr["City"]),
                Status = Convert.ToBoolean(dr["Status"])
            };
            return obj;
        }
        public void Customer_Insert(Customer customer)
        {
            //Creating a new DataRow based on the DataTable structure
            DataRow dr = ds.Tables[0].NewRow();
            //Assigning values to each Column of the DataRow
            dr["Custid"]= customer.Custid;
            dr["Name"]=customer.Name;
            dr["Balance"]=customer.Balance;
            dr["City"] = customer.City;
            dr["Status"] = customer.Status;
            //Adding the new DataRow to DataTable
            ds.Tables[0].Rows.Add(dr);
            //Saving data back to XML file
            ds.WriteXml("Customer.Xml");
        }
        public void Customer_Update(Customer customer)
        {
            //Finding a DataRow based on its Primary Key value
            DataRow dr = ds.Tables[0].Rows.Find(customer.Custid);
            //Finding the Index of DataRow by calling IndexOf method
            int Index = ds.Tables[0].Rows.IndexOf(dr);
            ds.Tables[0].Rows[Index]["Name"] = customer.Name;
            ds.Tables[0].Rows[Index]["Balance"] = customer.Balance;
            ds.Tables[0].Rows[Index]["City"]=customer.City;
            //Saving data back to XML file
            ds.WriteXml("Customer.Xml");
        }
        public void Customer_Delete(int Custid)
        {
            //Finding a DataRow based on its Primary Key value
            DataRow dr = ds.Tables[0].Rows.Find(Custid);
            //Finding the Index of DataRow by calling IndexOf method
            int Index = ds.Tables[0].Rows.IndexOf(dr);
            //Deleting the DataRow from DataTable by using Inde
            ds.Tables[0].Rows[Index].Delete();
            ds.WriteXml("Customer.Xml");
        }
    }
}
