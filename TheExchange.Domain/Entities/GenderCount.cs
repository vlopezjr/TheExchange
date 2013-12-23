using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheExchange.Domain.Entities
{
    public class Gender
    {
        public int Id { get; set; }        
        public string Name { get; set; }
    }

    public static class Genders
    {
        public static List<Gender> Count;

        static Genders()
        {
            Count = new List<Gender>();
            for(int i=1; i<=30; i++)
            {
                Count.Add(new Gender(){Id = i, Name = i.ToString()});
            }
        }
    }
}