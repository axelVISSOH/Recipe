using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeAPI.Models;

namespace RecipeWebClient.ViewModels
{
    public class UserViewModel
    {
        public Person Person { get; set; }
        public bool isConnected { get; set; }
    }
}
