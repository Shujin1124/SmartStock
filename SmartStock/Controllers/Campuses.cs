using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Application.Campuses.Dto_s;
using SmartStock.Application.Campuses.Queries;

namespace SmartStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Campuses : ControllerBase
    {
        private readonly IMediator _mediator;

        public Campuses(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all/campus")]
        public async Task<ActionResult<IEnumerable<CampusDto>>> GetCampuses()
        {
            var result = await _mediator.Send(new GetCampusesQuery());
            return Ok(result);
        }
    }
}
