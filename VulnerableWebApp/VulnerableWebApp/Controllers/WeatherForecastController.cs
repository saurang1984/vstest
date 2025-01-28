using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=True;";

    [HttpGet("GetUser")]
    public IActionResult GetUser(string userId)
    {
        // Unsanitized input (SQL Injection vulnerability)
        string query = $"SELECT * FROM Users WHERE Id = {userId}";

        using (var connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Ok(new { Id = reader["Id"], Name = reader["Name"] });
                }
                else
                {
                    return NotFound("User not found");
                }
            }
        }
    }
}
