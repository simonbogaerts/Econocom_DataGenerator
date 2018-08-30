namespace Econocom.DataGenerator.Entities.Interfaces
{
    public interface IData
    {
        string DeviceId { get; set; }
        string Type { get; set; }
        IPayload Payload { get; set; }
    }
}