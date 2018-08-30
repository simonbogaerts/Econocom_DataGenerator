using Econocom.DataGenerator.Entities.Interfaces;

namespace Econocom.DataGenerator.Entities.Entities
{
    public class PanicDataPayload: IPayload
    {
        public int LastSingleClick { get; set; }
        public string SourceId { get; set; }
    }
}