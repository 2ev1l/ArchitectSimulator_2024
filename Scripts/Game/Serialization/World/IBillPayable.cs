namespace Game.Serialization.World
{
    public interface IBillPayable
    {
        public bool CanAddBill { get; }
        public int BillPaymentAmount { get; }
        public string BillDescription { get; }
    }
}