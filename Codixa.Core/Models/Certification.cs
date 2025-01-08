﻿using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models
{
    public class Certification
    {
        [Key]
        public int CertificationId { get; set; }



        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }


        [ForeignKey("TestResult")]
        public int TestResultId { get; set; }
        public virtual TestResult TestResult { get; set; }
    }
}
