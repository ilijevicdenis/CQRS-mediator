using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.AppServices;
using Logic.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async  Task<IActionResult> GetList(string enrolled, int? number)
        {
            List<StudentDto> list = await _mediator.Send(new GetListQuery(enrolled, number));
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] NewStudentDto dto)
        {
            var command = new RegisterCommand(
                dto.Name, dto.Email,
                dto.Course1, dto.Course1Grade,
                dto.Course2, dto.Course2Grade);

            Result result = await _mediator.Send(command);
            return FromResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unregister(long id)
        {
            Result result =  await  _mediator.Send(new UnregisterCommand(id));
            return FromResult(result);
        }

        [HttpPost("{id}/enrollments")]
        public async Task<IActionResult> Enroll(long id, [FromBody] StudentEnrollmentDto dto)
        {
            Result result = await  _mediator.Send(new EnrollCommand(id, dto.Course, dto.Grade));
            return FromResult(result);
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public async Task<IActionResult> Transfer(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
        {
            var result =  await _mediator.Send(new TransferCommand(id, enrollmentNumber, dto.Course, dto.Grade));
            return FromResult(result);
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/deletion")]
        public async Task<IActionResult> Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
        {
            var result = await _mediator.Send(new DisenrollCommand(id, enrollmentNumber, dto.Comment));
            return FromResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPersonalInfo(long id, [FromBody] StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand(id, dto.Name, dto.Email);
            var result = await  _mediator.Send(command);
            return FromResult(result);
        }
    }
}
