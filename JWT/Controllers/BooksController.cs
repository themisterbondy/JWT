using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using JWT.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        [HttpGet, Authorize]
        public IEnumerable<BookModel> Get()
        {
            var currentUser = HttpContext.User;
            int userAge = 0;
            var resultBookList = new BookModel[] {
                new BookModel { Author = "Ray Bradbury", Title = "Fahrenheit 451", AgeRestriction = false },
                new BookModel { Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude", AgeRestriction = false },
                new BookModel { Author = "George Orwell", Title = "1984", AgeRestriction = false },   
                new BookModel { Author = "Anais Nin", Title = "Delta of Venus", AgeRestriction = true }
            };

            if (currentUser.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                DateTime birthDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth).Value);
                userAge = DateTime.Today.Year - birthDate.Year;
            }

            if (userAge < 18)
            {
                resultBookList = resultBookList.Where(b => !b.AgeRestriction).ToArray();
            }

            return resultBookList;
        }
    }
}