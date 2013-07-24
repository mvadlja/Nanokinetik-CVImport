namespace Ready.Model.Business
{
    public enum ParentEntityType
    {
        Null,
        AuthorisedProduct,
        Product,
        PharmaceuticalProduct,
        Activity,
        Task,
        Project
    }

    public class ParentEntity
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ParentEntityType Type { get; set; }
        public string ResponsibleUser { get; set; }
    }
}