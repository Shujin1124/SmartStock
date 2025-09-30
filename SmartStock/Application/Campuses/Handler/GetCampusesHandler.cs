using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStock.Application.Campuses.Dto_s;
using SmartStock.Application.Campuses.Queries;
using SmartStock.Infrastructure.Data;

namespace SmartStock.Application.Campuses.Handler
{
    public class GetCampusesHandler : IRequestHandler<GetCampusesQuery, IEnumerable<CampusDto>>
    {
        private readonly AppDbContext _context;

        public GetCampusesHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CampusDto>> Handle(GetCampusesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Campuses
                .Select(c => new CampusDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Location = c.Location
                })
                .ToListAsync(cancellationToken);
        }
    }

    // Handler for single campus by ID
    public class GetCampusByIdHandler : IRequestHandler<GetCampusByIdQuery, CampusDto?>
    {
        private readonly AppDbContext _context;

        public GetCampusByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CampusDto?> Handle(GetCampusByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Campuses
                .Where(c => c.Id == request.Id)
                .Select(c => new CampusDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Location = c.Location
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

