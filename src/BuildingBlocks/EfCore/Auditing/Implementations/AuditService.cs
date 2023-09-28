using EfCore.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Auditing.Implementations;

public class AuditService<TContext> : IAuditService where TContext : BaseDbContext
{
    private readonly TContext _context;

    public AuditService(TContext context)
    {
        _context = context;
    }

    public async Task<List<AuditDto>> GetUserTrailsAsync(Guid userId)
    {
        var trails = await _context.AuditTrails
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.DateTime)
            .Take(250)
            .ToListAsync();

        return trails.Adapt<List<AuditDto>>();
    }
}