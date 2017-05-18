using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace fletnix.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class WalledGarden : Controller
    {

    }
}