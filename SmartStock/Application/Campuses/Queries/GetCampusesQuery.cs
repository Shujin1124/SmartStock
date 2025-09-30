using MediatR;
using SmartStock.Application.Campuses.Dto_s;

namespace SmartStock.Application.Campuses.Queries
{
    public record GetCampusesQuery() : IRequest<IEnumerable<CampusDto>>;

    // Query to get a single campus by ID
    public record GetCampusByIdQuery(int Id) : IRequest<CampusDto?>;
}
