namespace Currency.Exchange.Data.DbContext
{
    public partial class Exchange
    {
        public int Id { get; set; }
        public int Clientid { get; set; }
        public string Fromcurrency { get; set; } = null!;
        public string Tocurrency { get; set; } = null!;
        public decimal Exchangerate { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Datecreated { get; set; }
    }
}
