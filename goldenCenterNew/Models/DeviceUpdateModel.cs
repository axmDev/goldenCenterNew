namespace goldenCenterNew.Models
{
    public class DeviceUpdateModel
    {
        public string SerialNumber { get; set; }
        public int FKTypeID { get; set; }
        public int Cycles { get; set; }
        public int WeeklyCycles { get; set; }
    }
}
