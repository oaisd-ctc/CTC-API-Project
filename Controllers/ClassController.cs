using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

[ApiController]
[Route("api/classes")]
public class ClassController : ControllerBase
{
    private readonly string _connectionString;

    public ClassController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
    }

    [HttpPost(Name = "CreateClasses")]
    public async Task<IActionResult> Create([FromBody] Class _class)
    {
        string commandText = "INSERT INTO classes (class_name) VALUES (@className)";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using SqlCommand cmd = new SqlCommand(commandText, conn);


            cmd.Parameters.Add("@className", SqlDbType.VarChar);
            cmd.Parameters["@className"].Value = _class.ClassName;
            cmd.ExecuteNonQuery();

            return Ok("Class Created");

        }
    }

    [HttpGet(Name = "ReadClasses")]
    public IEnumerable<Class> Read([FromQuery(Name = "id")] string id)
    {
        var classes = new List<Class>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            string query = "SELECT * FROM classes WHERE class_id =" + id;

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    classes.Add(new Class
                    {
                        ClassId = (int)reader["class_id"],
                        ClassName = reader["class_name"].ToString()
                    });
                }
            }
        }
        return classes;
    }

    [HttpPut(Name = "UpdateClasses")]
    public async Task<IActionResult> Update([FromBody]Class _class)
    {
        string commandText = "UPDATE classes SET class_name = @className WHERE class_id = @id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using SqlCommand cmd = new SqlCommand(commandText, conn);


            cmd.Parameters.Add("@className", SqlDbType.VarChar);
            cmd.Parameters["@className"].Value = _class.ClassName;

            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = _class.ClassId;
            cmd.ExecuteNonQuery();

            return Ok("Classes Updated");

        }
    }

    [HttpDelete(Name = "DeleteClasses")]
    public async Task<IActionResult> Delete([FromBody] Class _class)
    {
        string commandText = "DELETE FROM classes WHERE class_id = @id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using SqlCommand cmd = new SqlCommand(commandText, conn);


            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = _class.ClassId;
            cmd.ExecuteNonQuery();

            return Ok("Class Deleted");

        }
    }

}