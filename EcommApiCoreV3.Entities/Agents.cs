using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class Agents
    {
        public int AgentId { get; set; } = 0;
        public string Fname { get; set; }
        public string LName { get; set; }
        public string Mobile { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int CreatedBy { get; set; } = 0;
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; } = 0;
        public string ModifiedDate { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public bool IsActive { get; set; } = false;
        public string UserIds { get; set; }
        public string Token { get; set; }
    }
}
