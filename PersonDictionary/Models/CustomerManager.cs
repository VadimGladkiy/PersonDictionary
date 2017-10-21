using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PersonDictionary.Models
{
    public class CustomerManager : UserManager<Customer>
    {
        public CustomerManager(IUserStore<Customer>store) : base(store) { }

        public static CustomerManager Create(
            IdentityFactoryOptions<CustomerManager> options,
                IOwinContext context)
        {
            MyIdentityContext db = context.Get<MyIdentityContext>();
            CustomerManager manager 
                = new CustomerManager(new UserStore<Customer>(db));
            return manager;
        }
    }
}