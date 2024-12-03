﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJ.Database
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string PriorityLevel { get; set; }
        public string Status { get; set; }
    }

}
