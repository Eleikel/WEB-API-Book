using API_Books.Models;
using API_Books.Models.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Books.BookMapper
{
    public class BookMappers : Profile
    {
        public BookMappers()
        {
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
