using Application.Features.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Model;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? PhotoUrl { get; set; }
    public Guid? StoreId { get; set; }
    public IList<RefreshToken> RefreshTokens { get; private set; } = [];
}
