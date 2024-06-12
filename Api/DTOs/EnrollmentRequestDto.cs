using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class EnrollmentRequestDto
    {
        public string StudentEmail { get; set; }
        public string SubjectName { get; set; }
    }
}