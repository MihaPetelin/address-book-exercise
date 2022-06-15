using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressBookController : ControllerBase
{
    private readonly ILogger<AddressBookController> _logger;

    public AddressBookController(ILogger<AddressBookController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAddressBook")]
    public IEnumerable<AddressBookEntry> Get()
    {
        return AddressBookStorage.GetEntries();
    }

    [HttpPost(Name = "PostAddressBookEntry")]
    public HttpResponseMessage Post(AddressBookEntry entry)
    {
        var result = AddressBookStorage.InsertEntry(entry);
        var statusCode = HttpStatusCode.Accepted;
        string message = string.Empty;

        switch (result)
        {
            case AddressBookStorage.InsertEntryResult.InvalidEntry:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid entry!";
                break;
            case AddressBookStorage.InsertEntryResult.EmailAlreadyExists:
                statusCode = HttpStatusCode.Conflict;
                message = "The address book already contains this e-mail address!";
                break;
        }

        return new HttpResponseMessage()
        {
            StatusCode = statusCode,
            ReasonPhrase = message
        };
    }
}
