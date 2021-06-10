using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business
{
    public class VerifyEmailRequestDTO
    {
        [Required]
        public string Token { get; set; }

    }
}
