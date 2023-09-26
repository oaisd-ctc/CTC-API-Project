using Microsoft.AspNetCore.Mvc;
using CTC_API.Models;
using System.Data.SqlClient;
using System.Data;

[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    private readonly string _connectionString;

    public StudentController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ctc_dev_DBConnection");
    }

    [HttpPost(Name = "CreateStudent")]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        string commandText = "INSERT INTO students (first_name, last_name, username, session_code, school_id, class_id) VALUES (@firstName, @lastName, @username, @sessionCode, @schoolId, @classId)";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using SqlCommand cmd = new SqlCommand(commandText, conn);


            cmd.Parameters.Add("@firstName", SqlDbType.VarChar);
            cmd.Parameters["@firstName"].Value = student.FirstName;

            cmd.Parameters.Add("@lastName", SqlDbType.VarChar);
            cmd.Parameters["@lastName"].Value = student.LastName;

            cmd.Parameters.Add("@username", SqlDbType.VarChar);
            cmd.Parameters["@username"].Value = student.Username;

            cmd.Parameters.Add("@sessionCode", SqlDbType.VarChar);
            cmd.Parameters["@sessionCode"].Value = student.SessionCode;

            cmd.Parameters.Add("@schoolId", SqlDbType.Int);
            cmd.Parameters["@schoolId"].Value = student.SchoolId;

            cmd.Parameters.Add("@classId", SqlDbType.Int);
            cmd.Parameters["@classId"].Value = student.ClassId;

            cmd.ExecuteNonQuery();

            return Ok("Student Created");

        }
    }

    [HttpGet(Name = "ReadStudents")]
    public IEnumerable<Student> Read([FromQuery(Name = "id")] string id)
    {
        var students = new List<Student>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            string query = "SELECT * FROM students WHERE student_id =" + id;

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = (int)reader["student_id"],
                        FirstName = reader["first_name"].ToString(),
                        LastName = reader["last_name"].ToString(),
                        Username = reader["username"].ToString(),
                        SessionCode = reader["session_code"].ToString(),
                        SchoolId = (int)reader["school_id"],
                        ClassId = (int)reader["class_id"]
                    });
                }
            }
        }
        return students;
    }

    [HttpPut(Name = "UpdateStudents")]
    public async Task<IActionResult> Update([FromBody] Student student)
    {
        string commandText = "UPDATE schools SET first_name = @firstName, last_name = @lastName, username = @username, session_code = @sessionCode, school_id = @schoolId, class_id = @classId WHERE school_id = @id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using SqlCommand cmd = new SqlCommand(commandText, conn);


            cmd.Parameters.Add("@firstName", SqlDbType.VarChar);
            cmd.Parameters["@firstName"].Value = student.FirstName;

            cmd.Parameters.Add("@lastName", SqlDbType.VarChar);
            cmd.Parameters["@lastName"].Value = student.LastName;

            cmd.Parameters.Add("@username", SqlDbType.VarChar);
            cmd.Parameters["@username"].Value = student.Username;

            cmd.Parameters.Add("@sessionCode", SqlDbType.VarChar);
            cmd.Parameters["@sessionCode"].Value = student.SessionCode;

            cmd.Parameters.Add("@schoolId", SqlDbType.Int);
            cmd.Parameters["@schoolId"].Value = student.SchoolId;

            cmd.Parameters.Add("@classId", SqlDbType.Int);
            cmd.Parameters["@classId"].Value = student.ClassId;

            cmd.ExecuteNonQuery();

            return Ok("Students Updated");

        }
    }

    [HttpDelete(Name = "DeleteStudents")]
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

            return Ok("Students Deleted");

        }
    }
}

