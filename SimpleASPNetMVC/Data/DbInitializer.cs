using SimpleASPNetMVC.DbModels;

namespace SimpleASPNetMVC.Data
{
    public static class DbInitializer
    {
        public static void Initialize(StudentContext context)
        {
            context.Database.EnsureCreated();
            // Looking for existing students 
            if (context.Students.Any())
            {
                return;
            }

            var students = new Student[]
            {
                new Student() {FirstName = "Muhammad", LastName = "Saad", Email = "saad@xyzuniversity.com", Password = "Pass@123" },
                new Student(){FirstName = "Abdul", LastName = "Karim", Email = "karim@xyzuniversity.com", Password = "Pass@123"}
            };

            foreach (var student in students)
            {
                context.Students.Add(student);
            }
            context.SaveChanges();

            var studentFunds = new StudentFund[]
            {
                new StudentFund(){StudentId = 1, Balance = 80, Quota = 1000},
                new StudentFund(){StudentId = 2, Balance = 40, Quota = 500}
            };

            foreach (var studentFund in studentFunds)
            {
                context.Funds.Add(studentFund);
            }
            context.SaveChanges();
        }
    }
}
