using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;
using System.Data;

[ApiController]
[Route("api/schools")]
public class SchoolController : ControllerBase
{
    private readonly string _connectionString;

    public SchoolController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
    }

    [HttpPost(Name = "CreateSchools")]
    public async Task<IActionResult> Create([FromBody] School school)
    {
        string commandText = "INSERT INTO schools (school_name) VALUES (@schoolName)";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using SqlCommand cmd = new SqlCommand(commandText, conn);


            cmd.Parameters.Add("@schoolName", SqlDbType.VarChar);
            cmd.Parameters["@schoolName"].Value = school.SchoolName;
            cmd.ExecuteNonQuery();

            return Ok("School Created");

        }
    }

    [HttpGet(Name = "ReadSchools")]
    public IEnumerable<School> Read([FromQuery(Name = "id")] string id)
    {
        var schools = new List<School>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            string query = "SELECT * FROM schools WHERE school_id =" + id;

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    schools.Add(new School
                    {
                        SchoolId = (int)reader["school_id"],
                        SchoolName = reader["school_name"].ToString()
                    });
                }
            }
        }
        return schools;
    }

    [HttpPut(Name = "UpdateSchools")]
    public async Task<IActionResult> Update([FromBody] School school)
    {
        string commandText = "UPDATE schools SET school_name = @schoolName WHERE school_id = @id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using SqlCommand cmd = new SqlCommand(commandText, conn);


            cmd.Parameters.Add("@schoolName", SqlDbType.VarChar);
            cmd.Parameters["@schoolName"].Value = school.SchoolName;

            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = school.SchoolId;
            cmd.ExecuteNonQuery();

            return Ok("Schools Updated");

        }
    }

    [HttpDelete(Name = "DeleteSchools")]
    public async Task<IActionResult> Delete([FromBody] School school)
    {
        string commandText = "DELETE FROM schools WHERE school_id = @id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using SqlCommand cmd = new SqlCommand(commandText, conn);


            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = school.SchoolId;
            cmd.ExecuteNonQuery();

            return Ok("School Deleted");

        }
    }
}

