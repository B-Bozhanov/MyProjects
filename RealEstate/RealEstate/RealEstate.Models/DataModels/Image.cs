namespace RealEstate.Models.DataModels
{
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public byte[] Content { get; set; } = null!;

        public int PropertyId { get; set; }

        public virtual Property? Property { get; set; }
    }
}
