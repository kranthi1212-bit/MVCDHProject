namespace MVCDHProject.Models
{
    public class CustomerOracleDAL:ICustomerDAL
    {
        public List<Customer> Customers_Select()
        {
            List<Customer> customers = new List<Customer>();
            return customers;
        }
        public Customer Customer_Select(int Custid)
        {
            Customer obj = new Customer();
            return obj;
        }
        public void Customer_Insert(Customer customer)
        {
        }
        public void Customer_Update(Customer customer)
        {
        }
        public void Customer_Delete(int Custid)
        {
        }
    }
}
