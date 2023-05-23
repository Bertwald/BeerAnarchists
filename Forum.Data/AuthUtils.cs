using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data;
/// <summary>
/// To carry an authorization request
/// </summary>
internal class AuthRequest {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
/// <summary>
/// To carry the authorization response
/// </summary>
public class AuthResponse {
    public string UserName = null!;
    public string Email = null!;
    public string Token = null!;
}