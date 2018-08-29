namespace Econocom.DataGenerator.Entities.Interfaces
{
    public interface IPayload
    {
        int LastSingleClick { get; set; }
        string SourceId { get; set; }
    }
}