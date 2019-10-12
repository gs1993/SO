using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class CommentDto
    {
        public DateTime CreationDate { get; set; }
        public int? Score { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
    }
}
