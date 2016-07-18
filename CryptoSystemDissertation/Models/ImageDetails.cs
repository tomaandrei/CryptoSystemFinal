using System.ComponentModel.DataAnnotations;

namespace CryptoSystemDissertation.Models
{
    public class ImageDetails
    {
        [Key]
        public string ImageId { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string Image { get; set; }

        public string Parameters { get; set; }

        public byte[] IVAes { get; set; }
    }
}