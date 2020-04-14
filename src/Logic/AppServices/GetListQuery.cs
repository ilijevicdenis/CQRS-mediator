using System.Collections.Generic;
using Logic.Dtos;
using MediatR;

namespace Logic.AppServices
{
    public sealed class GetListQuery : IRequest<List<StudentDto>>
    {
        public string EnrolledIn { get; }
        public int? NumberOfCourses { get; }

        public GetListQuery(string enrolledIn, int? numberOfCourses)
        {
            EnrolledIn = enrolledIn;
            NumberOfCourses = numberOfCourses;
        }
    }
}
