namespace HomeBanking.Models
{
    public class ClientLoan
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }
        public long ClientId { get; set; } //para que exista una relacion de uno a muchos con la clase Client
        public Client Client { get; set; }
        public long LoanId { get; set; } //para que exista una relacion de uno a muchos con la clase Loan
        public Loan Loan { get; set; }
    }
}
