//using JustCare_MB.Models.Lookup;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JustCare_MB.Data
//{
//    public static class DbInitializer
//    {
//        public static void Initialize(JustCareContext context)
//        {
//            context.Database.EnsureCreated();

//            // Look for any students.
//            if (context.Users.Any())
//            {
//                return;   // DB has been seeded
//            }

//            var userType = new UserType[]
//            {
//            new UserType{UserTypeID=1,EngType="Admin"},
//            new UserType{UserTypeID=2,EngType="DentalStudent"},
//            new UserType{UserTypeID=3,EngType="Patient"},
            
//            };
//            foreach (UserType s in userType)
//            {
//                context.UserTypes.Add(s);
//            }
//            context.SaveChanges();


//        }
//    }
//}