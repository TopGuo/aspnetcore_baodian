using TB.AspNetCore.Domain.Enums;

namespace TB.AspNetCore.Domain.Models.Web
{
    public class InformationModel
    {
        public string Id { get; set; }
        public InformationType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
