using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    public class VMUpLoad
    {
       
        
        public string UpLoadId { get; set; }

     
        public string UpLoadTitle { get; set; }

      
        public string UpLoadUrl { get; set; }

        public string ShowUrl { get; set; }

      
        public string UpLoadType { get; set; }       

       
        public int Status { get; set; }

       
        public DateTime CreateTime { get; set; }

       
        public string CreateUser { get; set; }

       
        public DateTime UpdateTime { get; set; }

       
        public string UpdateUser { get; set; }

      

    }
}
