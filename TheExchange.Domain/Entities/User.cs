//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheExchange.Domain.Entities
{
    using System;
    using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("AspNetUsers")]
    public class User
    {        
        public string Id { get; set; }
        public string UserName { get; set; }        
        public string First { get; set; }
        public string Last { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
    
    }
}