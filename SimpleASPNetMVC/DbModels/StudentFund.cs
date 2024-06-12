namespace SimpleASPNetMVC.DbModels
{
    public class StudentFund
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public decimal Balance { get; set; }
        public int Quota { get; set; }

        public Student Student { get; set; }
    }
}
