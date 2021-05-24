namespace VDCoreLib.Interfaces
{
    public interface IOrderBy
    {
        public string OrderDirection { get; set; }
        public string OrderField { get; set; }
    }
}